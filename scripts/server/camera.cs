//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

// Global movement speed that affects all cameras.  This should be moved
// into the camera datablock.
$Camera::movementSpeed = 30;

function Observer::onTrigger(%this,%obj,%trigger,%state)
{
   // state = 0 means that a trigger key was released
   if (%state == 0)
      return;

   // Default player triggers: 0=fire 1=altFire 2=jump
   %client = %obj.getControllingClient();
   switch$ (%obj.mode)
   {
      case "Observer":
         // Do something interesting.

      case "Corpse":
         // Viewing dead corpse, so we probably want to respawn.
         %client.spawnPlayer();

         // Set the camera back into observer mode, since in
         // debug mode we like to switch to it.
         %this.setMode(%obj,"Observer");
   }
}

function Observer::setMode(%this,%obj,%mode,%arg1,%arg2,%arg3)
{
   switch$ (%mode)
   {
      case "Observer":
         // Let the player fly around
         %obj.setFlyMode();

      case "Corpse":
         // Lock the camera down in orbit around the corpse,
         // which should be arg1
         %transform = %arg1.getTransform();
         %obj.setOrbitMode(%arg1, %transform, 0.5, 4.5, 4.5);

   }
   %obj.mode = %mode;
}

function Spectator::onTrigger(%data, %obj, %trigger, %state)
{
   // state = 0 means that a trigger key was released
   if (%state == 0)
      return;

   //trigger types:   0:fire 1:altTrigger 2:jump 3:jet 4:throw
   %client = %obj.getControllingClient();
   if (%client == 0) // How'd this happen?!?!
      return;

   //no respawn when in editor
   if (EditorIsActive() || GuiEditorIsActive())
      return;
	  
   LogEcho("Spectator::onTrigger( " @ %data.getName() @ ", " @ %obj @ ", " @ %trigger @ ", " @ %state @ " )");

   // Hand it over to the game object
   if(isObject(Game))
      Game.SpectatorOnTrigger(%data, %obj, %trigger, %state, %client);
}

function Spectator::setMode(%data, %obj, %mode, %targetObj)
{
   if(%mode $= "")
      return;

   LogEcho("Spectator::setMode( " @ %data.getName() @ ", " @ %obj @ ", " @ %mode @ ", " @ %targetObj @ " )");

   // Hand it over to the game object
   if(isObject(Game))
      Game.SpectatorSetMode(%data, %obj, %mode, %targetObj);   
}

//-----------------------------------------------------------------------------
// Camera methods
//-----------------------------------------------------------------------------

function Camera::onAdd(%this, %obj)
{
   // Default start mode
   %this.setMode(%this.mode);
}

function Camera::setMode(%this, %mode, %arg1, %arg2, %arg3)
{
   // Punt this one over to our datablock
   %this.getDatablock().setMode(%this, %mode, %arg1, %arg2, %arg3);
}

//-----------------------------------------------------------------------------
// Camera server commands
//-----------------------------------------------------------------------------

function serverCmdTogglePathCamera(%client, %val)
{
   if(%val)
   {
      %control = %client.PathCamera;
   }
   else
   {
      %control = %client.camera;
   }
   %client.setControlObject(%control);
   clientCmdSyncEditorGui();
}

function serverCmdToggleCamera(%client)
{
   if (%client.getControlObject() == %client.player)
   {
      %client.camera.setVelocity("0 0 0");
      %control = %client.camera;
   }
   else
   {
      %client.player.setVelocity("0 0 0");
      %control = %client.player;
   }
   %client.setControlObject(%control);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraPlayer(%client)
{
   // Switch to Player Mode
   %client.player.setVelocity("0 0 0");
   %client.setControlObject(%client.player);
   ServerConnection.setFirstPerson(1);
   $isFirstPersonVar = 1;

   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraPlayerThird(%client)
{
   // Swith to Player Mode
   %client.player.setVelocity("0 0 0");
   %client.setControlObject(%client.player);
   ServerConnection.setFirstPerson(0);
   $isFirstPersonVar = 0;

   clientCmdSyncEditorGui();
}

function serverCmdDropPlayerAtCamera(%client)
{
   // If the player is mounted to something (like a vehicle) drop that at the
   // camera instead. The player will remain mounted.
   %obj = %client.player.getObjectMount();
   if (!isObject(%obj))
      %obj = %client.player;

   %obj.setTransform(%client.camera.getTransform());
   %obj.setVelocity("0 0 0");

   %client.setControlObject(%client.player);
   clientCmdSyncEditorGui();
}

function serverCmdDropCameraAtPlayer(%client)
{
   %client.camera.setTransform(%client.player.getEyeTransform());
   %client.camera.setVelocity("0 0 0");
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdCycleCameraFlyType(%client)
{
   if(%client.camera.getMode() $= "Fly")
	{
		if(%client.camera.newtonMode == false) // Fly Camera
		{
			// Switch to Newton Fly Mode without rotation damping
			%client.camera.newtonMode = "1";
			%client.camera.newtonRotation = "0";
			%client.camera.setVelocity("0 0 0");
		}
		else if(%client.camera.newtonRotation == false) // Newton Camera without rotation damping
		{
			// Switch to Newton Fly Mode with damped rotation
			%client.camera.newtonMode = "1";
			%client.camera.newtonRotation = "1";
			%client.camera.setAngularVelocity("0 0 0");
		}
		else // Newton Camera with rotation damping
		{
			// Switch to Fly Mode
			%client.camera.newtonMode = "0";
			%client.camera.newtonRotation = "0";
		}
		%client.setControlObject(%client.camera);
		clientCmdSyncEditorGui();
	}
}

function serverCmdSetEditorCameraStandard(%client)
{
   // Switch to Fly Mode
   %client.camera.setFlyMode();
   %client.camera.newtonMode = "0";
   %client.camera.newtonRotation = "0";
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraNewton(%client)
{
   // Switch to Newton Fly Mode without rotation damping
   %client.camera.setFlyMode();
   %client.camera.newtonMode = "1";
   %client.camera.newtonRotation = "0";
   %client.camera.setVelocity("0 0 0");
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorCameraNewtonDamped(%client)
{
   // Switch to Newton Fly Mode with damped rotation
   %client.camera.setFlyMode();
   %client.camera.newtonMode = "1";
   %client.camera.newtonRotation = "1";
   %client.camera.setAngularVelocity("0 0 0");
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorOrbitCamera(%client)
{
   %client.camera.setEditOrbitMode();
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdSetEditorFlyCamera(%client)
{
   %client.camera.setFlyMode();
   %client.setControlObject(%client.camera);
   clientCmdSyncEditorGui();
}

function serverCmdEditorOrbitCameraSelectChange(%client, %size, %center)
{
   if(%size > 0)
   {
      %client.camera.setValidEditOrbitPoint(true);
      %client.camera.setEditOrbitPoint(%center);
   }
   else
   {
      %client.camera.setValidEditOrbitPoint(false);
   }
}

function serverCmdEditorCameraAutoFit(%client, %radius)
{
   %client.camera.autoFitRadius(%radius);
   %client.setControlObject(%client.camera);
  clientCmdSyncEditorGui();
}

//-----------------------------------------------------------------------------
// Spectator Camera
//-----------------------------------------------------------------------------

function findNextObserveClient(%client)
{
   %index = -1;
   %count = ClientGroup.getCount();
   if (%count <= 1)
      return -1;

   for (%i = 0; %i < %count; %i++)
   {
      %cl = ClientGroup.getObject(%i);
      if (%cl == %client.observeClient)
      {
         %index = %i;
         break;
      }
   }

   //now find the next client (note, if not found, %index still == -1)
   %index++;
   if (%index >= %count)
      %index = 0;

   %newClient = -1;
   for (%i = %index; %i < %count; %i++)
   {
      %cl = ClientGroup.getObject(%i);
      if (%cl != %client && %cl.player > 0)
      {
         %newClient = %cl;
         break;
      }
   }

   //if we didn't find anyone, search from the beginning again
   if (%newClient < 0)
   {
      for (%i = 0; %i < %count; %i++)
      {
         %cl = ClientGroup.getObject(%i);
         if (%cl != %client && %cl.player > 0)
         {
            %newClient = %cl;
            break;
         }
      }
   }

   //if we still haven't found anyone (new), give up..
   if (%newClient < 0 || %newClient.player == %player)
      return -1;
}

function findPrevObserveClient(%client)
{
   %index = -1;
   %count = ClientGroup.getCount();
   if (%count <= 1)
      return -1;

   for (%i = 0; %i < %count; %i++)
   {
      %cl = ClientGroup.getObject(%i);
      if (%cl == %client.observeClient)
      {
         %index = %i;
         break;
      }
   }

   //now find the prev client
   %index--;
   if (%index < 0)
      %index = %count - 1;

   %newClient = -1;
   for (%i = %index; %i >= 0; %i--)
   {
      %cl = ClientGroup.getObject(%i);
      if (%cl != %client && %cl.player > 0)
      {
         %newClient = %cl;
         break;
      }
   }

   //if we didn't find anyone, search from the end again
   if (%newClient < 0)
   {
      for (%i = %count - 1; %i >= 0; %i--)
      {
         %cl = ClientGroup.getObject(%i);
         if (%cl != %client && %cl.player > 0)
         {
            %newClient = %cl;
            break;
         }
      }
   }

   //if we still haven't found anyone (new), give up..
   if (%newClient < 0 || %newClient.player == %player)
      return -1;
}

function observeClient(%client, %target)
{
   //clear the observer fly mode var...
   %client.observeFlyClient = -1;

   //cancel any scheduled update
   cancel(%client.obsHudSchedule);

   // must be an observer when observing other clients
   if( %client.getControlObject() != %client.camera)
      return;
   
   //can't observer yourself
   if (%client == %target)
      return;

   %count = ClientGroup.getCount();

   //can't go into observer mode if you're the only client
   if (%count <= 1)
      return;

   //make sure the target actually exists
   if (%target > 0)
   {
      %found = false;
      for (%i = 0; %i < %count; %i++)
      {
          %cl = ClientGroup.getObject(%i);
          if (%cl == %target)
          {
            %found = true;
            break;
          }
      }

      if (!%found)
         return;
   }
   else 
   {
      %client.observeClient = -1;
      %target = findNextObserveClient(%client);
      if (%target <= 0)
         return;
   }

   //send the message
   if (%client.camera.mode !$= "spectatorFollow")
   {
      if (isObject(%client.player))
         %client.player.scriptKill(0);

      //messageAllExcept(%client, -1, 'ClientNowSpectator', '\c1%1 is now an spectator.', %client.playerName);
      //messageClient(%client, 'YouNowSpectator', '\c1You are now spectating %1.', %target.playerName);   
   }

   %client.camera.getDataBlock().setMode(%client.camera, "spectatorFollow", %target.player);
   %client.setControlObject(%client.camera);

   //tag is used if a client who is being observed dies...
   %client.observeClient = %target;
}

function spectatorFollowUpdate( %client, %nextClient, %cycle )
{
   %Oclient = %client.observeClient;
   if( %Oclient $= "" )
      return;
   
   // changed to observer fly...
   if( %nextClient == -1 )
   {
      // find us in their observer list and remove, then reshuffle the list...
      for( %i = 0; %i < %Oclient.observeCount; %i++ )
      {
         if( %Oclient.observers[%i] == %client )
         {
            %Oclient.observeCount--;
            %Oclient.observers[%i] = %Oclient.observers[%Oclient.observeCount];
            %Oclient.observers[%Oclient.observeCount] = "";
            break;
         }
      }
      return; // were done..
   }

   // changed from observer fly to observer follow...
   if( !%cycle && %nextClient != -1 )
   {
      // if nobody is observing this guy, initialize their observer count...
      if( %nextClient.observeCount $= "" )
         %nextClient.observeCount = 0;

      // add us to their list of observers...
      %nextClient.observers[%nextClient.observeCount] = %client;
      %nextClient.observeCount++;
      return; // were done.
   }

   if( %nextClient != -1 )
   {
      // cycling to the next client...
      for( %i = 0; %i < %Oclient.observeCount; %i++ )
      {
         // first remove us from our prev client's list...
         if( %Oclient.observers[%i] == %client )
         {
            %Oclient.observeCount--;
            %Oclient.observers[%i] = %Oclient.observers[%Oclient.observeCount];
            %Oclient.observers[%Oclient.observeCount] = "";
            break; // screw you guys, i'm goin home!
         }
      }

      // if nobody is observing this guy, initialize their observer count...
      if( %nextClient.observeCount $= "" )
         %nextClient.observeCount = 0;

      // now add us to the new clients list...
      %nextClient.observeCount++;
      %nextClient.observers[%nextClient.observeCount - 1] = %client;
   }
}

function updateSpectatorHud(%client)
{
   //just in case there are two threads going...
   cancel(%client.obsHudSchedule);
   %client.observeFlyClient = -1;

   //make sure the client is supposed to be in spectator fly mode...
   if (!isObject(%client) || %client.team != 0 || %client.getControlObject() != %client.camera || %client.camera.mode $= "spectatorFollow")
      return;

   //get various info about the player's eye
   %srcEyeTransform = %client.camera.getTransform();
   %srcEyePoint = firstWord(%srcEyeTransform) @ " " @ getWord(%srcEyeTransform, 1) @ " " @ getWord(%srcEyeTransform, 2);

   %srcEyeVector = MatrixMulVector("0 0 0 " @ getWords(%srcEyeTransform, 3, 6), "0 1 0");
   %srcEyeVector = VectorNormalize(%srcEyeVector);

   //see if there's an enemy near our defense location...
   %clientCount = 0;
   %count = ClientGroup.getCount();
   %viewedClient = -1;
   %clientDot = -1;
   for(%i = 0; %i < %count; %i++)
   {
      %cl = ClientGroup.getObject(%i);

      //make sure we find an AI who's alive and not the client
      if (%cl != %client && isObject(%cl.player))
      {
         //make sure the player is within range
         %clPos = %cl.player.getWorldBoxCenter();
         %distance = VectorDist(%clPos, %srcEyePoint);
         if (%distance <= 30)
         {
            //create the vector from the client to the client
            %clVector = VectorNormalize(VectorSub(%clPos, %srcEyePoint));

            //see if the dot product is greater than our current, and greater than 0.6
            %dot = VectorDot(%clVector, %srcEyeVector);

            if (%dot > 0.6 && %dot > %clientDot)
            {
               //make sure we're not looking through walls...
               %mask = $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::StaticShapeObjectType;
               %losResult = containerRayCast(%srcEyePoint, %clPos, %mask);
               %losObject = GetWord(%losResult, 0);
               if (!isObject(%losObject))
               {
                  %viewedClient = %cl;
                  %clientDot = %dot;
               }
            }
         }
      }
   }
   if (isObject(%viewedClient))
      displaySpectatorHud(%client, 0, %viewedClient);
   else
      displaySpectatorHud(%client, 0);

   %client.observeFlyClient = %viewedClient;

   //schedule the next...
   %client.obsHudSchedule = schedule(500, %client, "updateSpectatorHud", %client);
}

function displaySpectatorHud(%client, %targetClient, %potentialClient)
{
   %targName = %targetClient.nameBase;
   %potName = %potentialClient.nameBase;
   %targTeam = $pref::Server::teamName[%targetClient.team];
   %potTeam = $pref::Server::teamName[%potentialClient.team];

   if ( %targetClient > 0 )
   {
      if ( %targetClient.team == 1 )
         bottomPrint(%client, "You are now spectating:\n" @ %targName @ "\nTeam: <color:0000ff>" @ %targTeam, 0, 3);
      else
         bottomPrint(%client, "You are now spectating:\n" @ %targName @ "\nTeam: <color:ff0000>" @ %targTeam, 0, 3);
   }
   else if ( %potentialClient > 0 )
   {
      if ( %potentialClient.team == 1 )
         bottomPrint(%client, "Spectator Fly Mode\n" @ %potName @ "\nTeam: <color:0000ff>" @ %potTeam, 0, 3);
      else
         bottomPrint(%client, "Spectator Fly Mode\n" @ %potName @ "\nTeam: <color:ff0000>" @ %potTeam, 0, 3);
   }   
   else
      BottomPrint( %client, "Spectator Fly Mode.\nChoose a team or\npress FIRE to be auto assigned.", 0, 3 );
}

function resetSpectatorFollow( %client, %dismount )
{
   if( %dismount )
   {
      if( !isObject( %client.player ) )
         return;

      for( %i = 0; %i < %client.observeCount; %i++ )
      {
         if ( %client.observers[%i].clientObserve != %client )
            continue;

         %client.observers[%i].camera.setOrbitMode( %client.player, %client.player.getTransform(), 0.5, 4.5, 4.5); 
      }
   }
   else
   {
      if( !%client.player.isMounted() )
         return;

      // grab the vehicle...
      %mount = %client.player.getObjectMount();
      if( %mount.getDataBlock().observeParameters $= "" )
         %params = %client.player.getTransform();
      else
         %params = %mount.getDataBlock().observeParameters;
      
      for( %i = 0; %i < %client.observeCount; %i++ )
      {
         if ( %client.observers[%i].clientObserve != %client )
            continue;

         %client.observers[%i].camera.setOrbitMode(%mount, %mount.getTransform(), getWord( %params, 0 ), getWord( %params, 1 ), getWord( %params, 2 ));
      }
   }
}

//-----------------------------------------------------------------------------
// Path Camera
//-----------------------------------------------------------------------------

datablock PathCameraData(LoopingCam)
{
   mode = "";
};

function PathCameraData::create(%block, %team, %pos)
{
   %obj = new PathCamera() {
      dataBlock = %block;
      position = %pos;
      team = %team;
   };
   return(%obj);
}

function PathCameraData::onNode(%this,%camera,%node)
{
   if (%node == %camera.loopNode)
   {
      %camera.pushPath(%camera.path);
      %camera.loopNode += %camera.path.getCount();
   }
}

function PathCamera::reset(%this,%speed)
{
   %this.path = 0;
   Parent::reset(%this,%speed);
}

function PathCamera::followPath(%this,%path)
{
   %this.path = %path.getId();
   if (!(%this.speed = %path.speed))
      %this.speed = 100;
   if (%path.isLooping)
      %this.loopNode = %path.getCount() - 2;
   else
      %this.loopNode = -1;

   %this.pushPath(%path);
   %this.popFront();
}

function PathCamera::pushPath(%this,%path)
{
   for (%i = 0; %i < %path.getCount(); %i++)
      %this.pushNode(%path.getObject(%i));
}

function PathCamera::pushNode(%this,%node)
{
   if (!(%speed = %node.speed))
      %speed = %this.speed;
   if ((%type = %node.type) $= "")
      %type = "Normal";
   if ((%smoothing = %node.smoothing) $= "")
      %smoothing = "Linear";

   %this.pushBack(%node.getTransform(),%speed,%type,%smoothing);
}
