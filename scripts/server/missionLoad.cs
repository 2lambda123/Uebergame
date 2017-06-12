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

//-----------------------------------------------------------------------------
// Server mission loading
//-----------------------------------------------------------------------------

// On every mission load except the first, there is a pause after
// the initial mission info is downloaded to the client.
$MissionLoadPause = 5000;

//-----------------------------------------------------------------------------

function Torque::loadMission(%this, %missionFile, %missionType, %isFirstMission)
{
   // Do not allow clients to connect during loading process
   //allowConnections(false);

   // cleanup
   if(!%isFirstMission)
      %this.endMission();

   echo("<>>>> LOADING MISSION: " @ %missionFile @ " <<<<>");
   echo("<>>>> Stage 1 load <<<<>");

   $LoadingMission = true;

   // Reset all of these
   if (isFunction("clearCenterPrintAll"))
      clearCenterPrintAll();
   if (isFunction("clearBottomPrintAll"))
      clearBottomPrintAll();

   // increment the mission sequence (used for ghost sequencing)
   $missionSequence++;
   $missionRunning = false;

   // Setup some vars needed for server filter
   $Server::MissionType = %missionType;
   $Server::MissionName = fileBase(%missionFile);
   $Server::MissionFile = %missionFile;
   $Server::LoadFailMsg = "";

   // Create a list of inventory items banned by the game type.
   SmsInv.CreateInvBanCount();

   // Extract mission info from the mission file,
   // including the display name and stuff to send
   // to the client.
   buildLoadInfo(%missionFile, %missionType);

   // Download mission info to the clients
   %count = ClientGroup.getCount();
   for( %cl = 0; %cl < %count; %cl++ )
   {
      %client = ClientGroup.getObject( %cl );
      if (!%client.isAIControlled())
         sendLoadInfoToClient(%client);
   }

   // Mission scripting support, activate mission package
   // the level designer may have included
   if ( isPackage( $Server::MissionName ) )
   {
      if(!isActivePackage($Server::MissionName))
         activatePackage($Server::MissionName);

      eval($Server::MissionName @ "::preLoad(%first);");
   }

   // if this isn't the first mission, allow some time for the server
   // to transmit information to the clients:
   if ( %isFirstMission || !$pref::Server::Multiplayer )
      %this.loadMissionStage2(%missionFile, %missionType, %isFirstMission);
   else
      %this.schedule( $MissionLoadPause, "loadMissionStage2", %missionFile, %missionType, %isFirstMission );
  
}

//-----------------------------------------------------------------------------

function Torque::loadMissionStage2(%this, %missionFile, %missionType, %isFirstMission) 
{
   echo("<>>>> Stage 2 load <<<<>");

   // Create the mission group off the ServerGroup
   $instantGroup = ServerGroup;

   if ( %missionType $= "" )
   {
      new ScriptObject(Game) {
         class = CoreGame;
      };
   }
   else
   {
      new ScriptObject(Game) {
         class = %missionType @ "Game";
         superClass = CoreGame;
      };
   }

   // Allow the game to activate any packages.
   if ( isPackage( %missionType @ "Game" ) )
      Game.activatePackages();

   // Make sure the mission exists
   if ( !isFile( %missionFile ) )
   {
      $Server::LoadFailMsg = "Could not find mission \"" @ %missionFile @ "\"";
      error($Server::LoadFailMsg);

      // Inform clients that are already connected
      for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
         messageClient(ClientGroup.getObject(%clientIndex), 'MsgLoadFailed', $Server::LoadFailMsg);

      Game.schedule(3000, "cycleMissions");
      return( false );
   }
   
   // Calculate the mission CRC.  The CRC is used by the clients
   // to caching mission lighting.
   $missionCRC = getFileCRC( %missionFile );

   // Mission cleanup group.  This is where run time components will reside.  The MissionCleanup
   // group will be added to the ServerGroup.
   new SimGroup(MissionCleanup);

   // Exec the mission.  The MissionGroup (loaded components) is added to the ServerGroup
   exec(%missionFile);

   // If there was a problem with the load, let's try another mission
   if ( !isObject( MissionGroup ) )
   {
      $Server::LoadFailMsg = "No 'MissionGroup' found in mission \"" @ %missionFile @ "\".";
      error($Server::LoadFailMsg);
      // Inform clients that are already connected
      for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
         messageClient(ClientGroup.getObject(%clientIndex), 'MsgLoadFailed', $Server::LoadFailMsg);

      Game.schedule(3000, "cycleMissions");
      return( false );
   }

   // Hide objects not associated with the current gametype
   MissionGroup.setupGameType(%missionType);

   // Deal with power
   //MissionGroup.clearPower(); //decactivated since it spams and is not in use
   //MissionGroup.powerInit(0); //decactivated since it spams and is not in use

   // Make the MissionCleanup group the place where all new objects will automatically be added.
   $instantGroup = MissionCleanup;
   
   // Construct MOD paths
   pathOnMissionLoadDone();

   if ( $pref::Server::TournamentMode )
      $FriendlyFire = 1;
   else
      $FriendlyFire = $pref::Server::FriendlyFire;

   $CountdownStarted = false;
   $Game::Running = false;
   
   // Start all the clients in the mission
   $missionRunning = true;
   for( %clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++ )
   {
      ClientGroup.getObject(%clientIndex).loadMission();
   }

   physicsStartSimulation( "server" );

   Game.onMissionLoaded();

   // Mission loading done...
   echo("<>>>> Mission loaded <<<<>");
   $missionRunning = true;

   // Mission scripting support, activate mission package
   // the level designer has included
   if ( isActivePackage( $Server::MissionName ) )
      eval($Server::MissionName @ "::mapLoaded();");

   // Load the Navigation mesh
   %mesh = NameToID("MissionGroup/Nav");
   if ( isObject( %mesh ) )
   {
      if ( %mesh.load() )
         echo( "NavMesh loaded" );
      else
         echo( "No NavMesh available" );
   }

   // Allow clients to connect
   //allowConnections(true);
   
   // This throws away old bots so that connectAiClients can connect fresh ones
   // This is a workaround to avoid having broken and dublicated bots in the next mission
   for(%i = 0; %i < ClientGroup.getCount(); )
   {
   %outdatedBot = ClientGroup.getobject(%i);
      if (%outdatedBot.isAIControlled())
      {
         %outdatedBot.delete();
      } 
      else
      {
         %i++;
      }
   }

   //re-execute BadBehaviorTreeManager, since it gets removed by mission cleanup somehow
   exec("./ai/behaviorTreeManager.cs");
   
   // Need a group to put all the aiPlayer's in so we can index them etc.
   $Bot::Set = new SimSet( "BotSet" );
   MissionCleanup.add( $Bot::Set );

   // Add the aiPlayer bots and aiClients
   if ( $pref::Server::AiCount > 0 && !$UsingMainMenuLevel )
   {
      //addAiPlayers( $pref::Server::AiCount );
      connectBots( $pref::Server::AiCount );
   }

   $LoadingMission = false;
}

//-----------------------------------------------------------------------------

function Torque::endMission(%this)
{
   if (!isObject( MissionGroup ))
      return;

   echo("<>>>> ENDING MISSION <<<<>");

   physicsStopSimulation( "server" );

   // Mission scripting support, kill the mission package
   // the level designer has included
   if ( isActivePackage( $Server::MissionName ) )
   {
      eval($Server::MissionName @ "::DeactivateMap();");
      deactivatePackage($Server::MissionName);
   }

   // If this is first mission then the MissionGroup, MissionCleanup and game object aren't there yet.
   // if a mission group was there, delete prior mission stuff
   // clear out the previous mission paths
   for(%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
   {
      // clear ghosts and paths from all clients
      %cl = ClientGroup.getObject(%clientIndex);
      if(!%cl.isAiControlled())
      {
         %cl.endMission();
         %cl.resetGhosting();
         %cl.clearPaths();
         %cl.matchStartReady = false;
         if( $pref::Server::TournamentMode )
         {
            %cl.notready = 1;
            %cl.notReadyCount = "";
         }
      }
   }
   
   // Delete everything
   $Bot::Set.delete();
   MissionGroup.delete();
   MissionCleanup.delete();
   Game.deactivatePackages();
   Game.delete();
 
   $ServerGroup.delete();
   $ServerGroup = new SimGroup(ServerGroup);

   clearServerPaths();
}

//-----------------------------------------------------------------------------

function SimGroup::setupGameType(%this, %type)
{
   for (%i = 0; %i < %this.getCount(); %i++)
      %this.getObject(%i).setupGameType(%type);
}

function SimObject::setupGameType(%this, %type)
{
   // Thou shalt not spam
}

function ShapeBase::setupGameType(%this, %type)
{
   //error("ShapeBase::setupGameType(" SPC %this.getClassName() SPC %type SPC ")");
   if(%this.gameTypesList $= "")
      return;

   for(%i = 0; (%allow = getWord(%this.gameTypesList, %i)) !$= ""; %i++)
      if(%allow $= %type)
         return;

   %this.setHidden(true);
}

function TSStatic::setupGameType(%this, %type)
{
   if(%this.gameTypesList $= "")
      return;

   for(%i = 0; (%allow = getWord(%this.gameTypesList, %i)) !$= ""; %i++)
      if(%allow $= %type)
         return;

   %this.schedule( 50, "delete");
}

function Trigger::setupGameType(%this, %type)
{
   if(%this.gameTypesList $= "")
      return;

   for(%i = 0; (%allow = getWord(%this.gameTypesList, %i)) !$= ""; %i++)
      if(%allow $= %type)
         return;

   %this.schedule( 50, "delete");
}

function ParticleEmitterNode::setupGameType(%this, %type)
{
   if(%this.gameTypesList $= "")
      return;

   for(%i = 0; (%allow = getWord(%this.gameTypesList, %i)) !$= ""; %i++)
      if(%allow $= %type)
         return;

   %this.schedule( 50, "delete");
}

function SimGroup::initializeObjective(%this)
{
   for (%i = 0; %i < %this.getCount(); %i++)
      %this.getObject(%i).initializeObjective();   
}

function GameBase::initializeObjective(%this)
{
   if( isObject( %this ) )
      %this.getDataBlock().initializeObjective(%this);
}

function SimGroup::activatePhysicalZones(%this)
{
   for (%i = 0; %i < %this.getCount(); %i++)
   {
      %obj = %this.getObject(%i);
      if ( %obj.getClassName() $= SimGroup )
         %obj.activatePhysicalZones();
      else
      {
         if ( %obj.getClassName() $= PhysicalZone )
            %obj.activate();
      }
   }
}

function SimGroup::deactivatePhysicalZones(%this)
{
   for (%i = 0; %i < %this.getCount(); %i++)
   {
      %obj = %this.getObject(%i);
      if ( %obj.getClassName() $= SimGroup )
         %obj.activatePhysicalZones();
      else
      {
         if ( %obj.getClassName() $= PhysicalZone )
            %obj.deactivate();
      }
   }
}
