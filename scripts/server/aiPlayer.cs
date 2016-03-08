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
// AIPlayer callbacks
// The AIPlayer class implements the following callbacks:
//
//    PlayerData::onStop(%this,%obj)
//    PlayerData::onMove(%this,%obj)
//    PlayerData::onReachDestination(%this,%obj)
//    PlayerData::onMoveStuck(%this,%obj)
//    PlayerData::onTargetEnterLOS(%this,%obj)
//    PlayerData::onTargetExitLOS(%this,%obj)
//    PlayerData::onAdd(%this,%obj)
//
// Since the AIPlayer doesn't implement it's own datablock, these callbacks
// all take place in the PlayerData namespace.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Demo Pathed AIPlayer.
//-----------------------------------------------------------------------------

function Armor::onReachDestination(%this, %obj)
{
   echo( "\c4Armor::onReachDestination(" SPC %this.getName() @", "@ %obj.getShapeName() SPC ")" );

   // Moves to the next node on the path.
   // Override for all player.  Normally we'd override this for only
   // a specific player datablock or class of players.
   if ( %obj.pathSet )
   {
      %obj.pathSet = 0;
   }

   if ( %obj.path !$= "" )
   {
      if ( %obj.currentNode == %obj.targetNode )
         %obj.nextTask();
      else
         %obj.moveToNextNode();
   }
}

function Armor::onMoveStuck(%this, %obj)
{
   echo( "\c4Armor::onMoveStuck(" SPC %this.getName() @", "@ %obj.getShapeName() SPC ")" );
   %obj.clearTasks();
   %obj.pushTask( "moveRandom()" );
}

function Armor::onTargetExitLOS(%this,%obj)
{
   echo( "\c4Armor::onTargetExitLOS(" SPC %this.getName() @", "@ %obj.getShapeName() SPC ")" );
}

function Armor::onTargetEnterLOS(%this,%obj)
{
   echo( "\c4Armor::onTargetEnterLOS(" SPC %this.getName() @", "@ %obj.getShapeName() SPC ")" );
}

function Armor::onEndSequence(%this,%obj,%slot)
{
   echo("Sequence Done!");
   %obj.stopThread(%slot);
   %obj.nextTask();
}

//-----------------------------------------------------------------------------
// AIPlayer static functions
//-----------------------------------------------------------------------------

function AIPlayer::spawnAtLocation(%name, %spawnPoint)
{
   // Create the demo player object
   %player = new AiPlayer()
   {
      dataBlock = DefaultSoldier;
      client = 0;
      team = 0;
      path = "";
      isBot = true;
   };
   MissionCleanup.add(%player);
   %player.setTeamId(0);
   %player.setShapeName(%name);
   %player.setTransform(%spawnPoint);
   return %player;
}

function AIPlayer::spawnOnPath(%name, %path)
{
   // Spawn a player and place him on the first node of the path
   if ( !isObject( %path ) )
      return 0;

   %node = %path.getObject(0);
   %player = AIPlayer::spawnAtLocation(%name, %node.getTransform());
   return %player;
}

//-----------------------------------------------------------------------------
// AIPlayer methods
//-----------------------------------------------------------------------------

function AIPlayer::followPath(%this, %path, %node)
{
   // Start the player following a path
   %this.stopThread(0);
   if ( !isObject( %path ) )
   {
      %this.path = "";
      return;
   }

   if ( %node > %path.size() - 1 )
      %this.targetNode = %path.size() - 1;
   else
      %this.targetNode = %node;

   if ( %this.path $= %path )
      %this.moveToNode( %this.currentNode );
   else
   {
      %this.path = %path;
      %this.moveToNode(0);
   }
}

function AIPlayer::moveToNextNode(%this)
{
   if (%this.targetNode < 0 || %this.currentNode < %this.targetNode)
   {
      if (%this.currentNode < %this.path.size() - 1)
         %this.moveToNode(%this.currentNode + 1);
      else
         %this.moveToNode(0);
   }
   else
      if (%this.currentNode == 0)
         %this.moveToNode(%this.path.size() - 1);
      else
         %this.moveToNode(%this.currentNode - 1);
}

function AIPlayer::moveToNode(%this,%index)
{
   // Move to the given path node index
   %this.currentNode = %index;
   %xfm = %this.path.getNode(%index);
   //%this.setMoveDestination(%xfm, %index == %this.targetNode);
   %this.setPathDestination(%xfm);
}

//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function AIPlayer::pushTask(%this, %method)
{
   if ( %this.taskIndex $= "" )
   {
      %this.taskIndex = 0;
      %this.taskCurrent = -1;
   }

   %this.task[%this.taskIndex] = %method;
   %this.taskIndex++;

   if ( %this.taskCurrent == -1 )
      %this.executeTask( %this.taskIndex - 1 );
}

function AIPlayer::clearTasks(%this)
{
   %this.taskIndex = 0;
   %this.taskCurrent = -1;
}

function AIPlayer::nextTask(%this)
{
   if (%this.taskCurrent != -1)
      if (%this.taskCurrent < %this.taskIndex - 1)
         %this.executeTask(%this.taskCurrent++);
      else
         %this.taskCurrent = -1;
}

function AIPlayer::executeTask(%this,%index)
{
   %this.taskCurrent = %index;
   eval(%this.getId() @"."@ %this.task[%index] @";");
}

//-----------------------------------------------------------------------------

function AIPlayer::singleShot(%this)
{
   // The shooting delay is used to pulse the trigger
   %this.setImageTrigger(0, true);
   %this.setImageTrigger(0, false);
   %delay = %this.getDataBlock().shootingDelay;
   if (%delay $= "")
      %delay = 1000;

   %this.trigger = %this.schedule(%delay, singleShot);
}

//-----------------------------------------------------------------------------

function AIPlayer::wait(%this, %time)
{
   %this.schedule(%time * 1000, "nextTask");
}

function AIPlayer::done(%this,%time)
{
   %this.schedule( 50, "delete");
}

function AIPlayer::fire(%this,%bool)
{
   if (%bool)
   {
      cancel(%this.trigger);
      %this.singleShot();
   }
   else
      cancel(%this.trigger);

   %this.nextTask();
}

function AIPlayer::aimAt(%this,%object)
{
   echo("Aim: "@ %object);
   %this.setAimObject(%object);
   %this.nextTask();
}

function AIPlayer::animate(%this,%seq)
{
   //%this.stopThread(0);
   //%this.playThread(0,%seq);
   %this.setActionThread(%seq);
}

function AIPlayer::test()
{
   %player = AIPlayer::spawnOnPath("xasd","MissionGroup/Paths/Path2");
   %player.mountImage(PSWRifleImage,0);
   %player.setInventory(PSWAmmo,1000);
   
   %player.pushTask("followPath(\"MissionGroup/Paths/Path2\")");
   %player.pushTask("aimAt(\"MissionGroup/Room6/target\")");
   %player.pushTask("wait(1)");   
   %player.pushTask("fire(true)");
   %player.pushTask("wait(10)");
   %player.pushTask("fire(false)");
   %player.pushTask("playThread(0,\"celwave\")");
   %player.pushTask("done()");
}

// ----------------------------------------------------------------------------
// Tasks
// ----------------------------------------------------------------------------

function AIPlayer::doWanderTask(%player)
{
   if ( !isObject( %player ) || %player.getState() $= "Dead" )
      return;

   %client = %player.client;

   if ( !%player.pathSet )
   {
      %ranPos = %player.getRanBotDest();

      if ( !%player.setPathDestination( %ranPos ) )
         %player.setMoveDestination( %ranPos, false );

      %player.setAimLocation( %ranPos );
      %player.pathSet = 1;
   }

   // Find a target. If one is found our mode will change to attack
   //%player.choosePlayerTarget();

   echo("\c5AIPlayer::doWanderTask(" SPC deTag(%player.getShapeName()) @", "@ %ranPos SPC ")");
}

function AIPlayer::moveRandom(%player)
{
   if ( !isObject( %player ) || %player.getState() $= "Dead" )
      return;

   %x = getRandom( -5, 5 );
   %y = getRandom( -5, 5 );
   %pos = %player.getPosition();

   %newX = setWord( %pos, 0, ( getWord(%pos, 0) + %x ) );
   %newY = setWord( %pos, 1, ( getWord(%pos, 1) + %y ) );

   %player.pathSet = 1;
   %player.setMoveDestination( %newX SPC %newY SPC getTerrainHeight( %newX SPC %newY ), false );

   echo("\c5AIPlayer::moveRandom(" SPC deTag(%player.getShapeName()) @", "@ %player.getMoveDestination() SPC ")");
}

// Look around in all directions, needs to be looped
function AIPlayer::scanArea(%player)
{
   if ( isEventPending( %player.scanSchedule ) )
      cancel( %player.scanSchedule );

   %player.scanCheck++;
   %player.scanSchedule = %player.schedule(1000, "scanArea");

   echo("\c5AIPlayer::scanArea(" SPC deTag(%player.getShapeName()) @", "@ %player.scanCheck SPC ")");

   switch( %player.scanCheck )
   {
      case 1:
         %player.setAimLocation("0 0 1");
      case 2:
         %player.setAimLocation("99999 0 1");
      case 3:
         %player.setAimLocation("-99999 0 1");
      case 4:
         %player.setAimLocation("0 99999 1");
      case 5:
         cancel( %player.scanSchedule );
         %player.setAimLocation("0 -99999 1");
         %player.scanCheck = 0;
         %player.nextTask();
   }
}

// ----------------------------------------------------------------------------
// Some handy getDistance/nearestTarget functions for the AI to use
// ----------------------------------------------------------------------------

function AIPlayer::getRanBotDest(%player)
{
   // Grab a position from preplaced via a script object
   %array = NameToID("MissionGroup/WanderArray");

   if ( !isObject( %array ) )
      error( "No WanderArray present in mission file!" );

   %count = %array.WanderPosCount;
   %ran = mFloor( getRandom( 1, %count ) );
   %pos = %array.WanderPos[%ran];

   return %pos;
}

function AIPlayer::getTargetDistance(%this, %target)
{
   echo("\c4AIPlayer::getTargetDistance("@ %this @", "@ %target @")");
   $tgt = %target;
   %tgtPos = %target.getPosition();
   %eyePoint = %this.getWorldBoxCenter();
   %distance = VectorDist(%tgtPos, %eyePoint);
   echo("Distance to target = "@ %distance);
   return %distance;
}

function AIPlayer::getNearestPlayerTarget(%this)
{
   echo("\c4AIPlayer::getNearestPlayerTarget("@ %this @")");

   %index = -1;
   %botPos = %this.getPosition();
   %count = ClientGroup.getCount();
   for(%i = 0; %i < %count; %i++)
   {
      %client = ClientGroup.getObject(%i);
      if (%client.player $= "" || %client.player == 0)
         return -1;
      %playerPos = %client.player.getPosition();

      %tempDist = VectorDist(%playerPos, %botPos);
      if (%i == 0)
      {
         %dist = %tempDist;
         %index = %i;
      }
      else
      {
         if (%dist > %tempDist)
         {
            %dist = %tempDist;
            %index = %i;
         }
      }
   }
   return %index;
}

//-----------------------------------------------------------------------------

function AIPlayer::think(%player)
{
   // Thinking allows us to consider other things...
   if ( isEventPending( %player.thinkSchedule ) )
      cancel( %player.thinkSchedule );

   //if the bot is dead or doesnt exist then exit the scan Loop. But only after the schedule is canceled
   if ( !isObject( %player ) || %player.getState() $= "Dead" )
      return( false );

   // No point in going further until the game starts
   if ( !$Game::Running )
   {
      %player.thinkSchedule = %player.schedule(500, "think");
      return( false );
   }

   if ( !%player.pathSet )
      %player.pushTask("doWanderTask()");

   %player.thinkSchedule = %player.schedule( 1000, "think" );
   return( true );
}

function AIPlayer::spawn(%path)
{
   %player = AIPlayer::spawnOnPath("Shootme", %path);

   if (isObject(%player))
   {
      %player.followPath(%path, -1);

      // slow this sucker down, I'm tired of chasing him!
      %player.setMoveSpeed(0.5);

      //%player.mountImage(xxxImage, 0);
      //%player.setInventory(xxxAmmo, 1000);
      //%player.think();

      return %player;
   }
   else
      return 0;
}

//-----------------------------------------------------------------------------

$RandomBotNameCount = 0;
function addBotName(%name)
{
   $RandomBotName[$RandomBotNameCount] = %name;
   $RandomBotNameCount++;
}

addBotName("Bullseye");
addBotName("Casualty");
addBotName("Fodder");
addBotName("Grunt");
addBotName("Bait");
addBotName("Meat");
addBotName("Tardo");
addBotName("Roadkill");
addBotName("SkidMark");
addBotName("Flatline");
addBotName("Spud");
addBotName("WormChow");
addBotName("Endangered");
addBotName("Squidloaf");
addBotName("Gimp");
addBotName("Masochist");
addBotName("Terminal");
addBotName("KickMe");
addBotName("Fred");
addBotName("Fluffy Bunny");
addBotName("Carcass");
addBotName("Spastic");
addBotName("Bumpkin");
addBotName("Mad Cow");
addBotName("Oblivious");

function getRandomBotName()
{
   %index = getRandom( $RandomBotNameCount-1 );
   return($RandomBotName[%index]); 
}

function AIPlayer::testBot()
{
   // Create the demo player object
   %player = new AiPlayer()
   {
      dataBlock = DefaultSoldier;
      client = 0;
      team = mFloor(getRandom( 1, 2 ));
      path = "";
      pathSet = 0;
      isBot = true;
      mMoveTolerance = 2;
      allowWalk = true;
      allowJump = true;
      allowDrop = true;
      allowSwim = true;
      allowLedge = true;
      allowClimb = true;
      allowTeleport = true;
   };
   MissionCleanup.add(%player);
   %player.setTeamId(%player.team);
   %player.setShapeName(getRandomBotName());
   %player.setSkinName("base");
   %player.setTransform(Game.pickSpawnPoint(%player.team));
   %player.setRechargeRate(%player.getDataBlock().rechargeRate);
   %player.setEnergyLevel(%player.getDataBlock().maxEnergy);
   %player.setRepairRate(0);
   %player.setMoveSpeed( %player.getDataBlock().MoveSpeed );
   %player.setInventory( HealthKit, 1 );
   %player.setInventory( Ryder, 1, 1 );
   %player.setInventory( RyderClip, %player.maxInventory(RyderClip), 1 );
   %player.setInventory( RyderAmmo, %player.maxInventory(RyderAmmo), 1 );
   %player.weaponCount = 1;
   %player.use( %player.weaponSlot[0] );
   %player.think();

   if ( !%player.getNavMesh() )
      error( "No Nav Mesh found for" SPC %player.getShapeName() );

   return( %player );
}


MoveMap.bindCmd(keyboard, "x", "commandToServer(\'setWanderPosition\');", "");

$BotWander::PosCount = 0;
function serverCmdsetWanderPosition(%client)
{
   if( isObject( %client.player ) )
   {
      %array = NameToID("MissionGroup/WanderArray");

      if ( !isObject( %array ) )
      {
         new ScriptObject(WanderArray) {
            class = "Wander";
            canSave = "1";
            canSaveDynamicFields = "1";
         };

         MissionGroup.add(WanderArray);
      }

      %transform = %client.player.getTransform();
      %pos = posFromTransform( %transform );

      $BotWander::PosCount++;

      // Drop a waypoint so we can see where we have been
      %waypoint = new WayPoint() {
         team = 0;
         markerName = "Wander Point" SPC $BotWander::PosCount;
         dataBlock = "WayPointMarker";
         position = VectorAdd( %pos, "0 0 1.15" );
         rotation = rotFromTransform( %transform );
      };
      %waypoint.setTransform(%transform);
      MissionCleanup.add(%waypoint);

      %array.WanderPosCount = $BotWander::PosCount;
      %array.WanderPos[$BotWander::PosCount] = VectorAdd( %pos, "0 0 0.25" );
      %client.lastPos = %pos;
      $BotWander::Pos[$BotWander::PosCount] = %array.WanderPos[$BotWander::PosCount];
   }
   //export("$BotWander::*", "logs/pos.cs", false);
}