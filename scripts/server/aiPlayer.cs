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
/*
//-----------------------------------------------------------------------------
// Demo Pathed AIPlayer.
//-----------------------------------------------------------------------------

function Armor::onReachDestination(%this, %player)
{
   //echo( "\c4Armor::onReachDestination(" SPC %this.getName() @", "@ %player.client.nameBase SPC ")" );
   %client = %player.client;
   if ( isObject( %client ) && %client.isAiControlled() )
   {
      if ( %client.taskMode $= "Health" )
      {
         %player.gotHealth = 1;
      }   
   }
   %player.pathSet = 0;
}

function Armor::onMoveStuck(%this, %player)
{
   echo( "\c4Armor::onMoveStuck(" SPC %this.getName() @", "@ %player.client.nameBase SPC ")" );
   %player.moveRandom();
}

function Armor::onTargetExitLOS(%this,%player)
{

   //echo( "\c4Armor::onTargetExitLOS(" SPC %this.getName() @", "@ %player.client.nameBase SPC ")" );
   %client = %player.client;
   if ( isObject( %client ) && %client.isAiControlled() )
   {
      if ( %client.firing )
      {
          %client.fire( $WeaponSlot, 0 );
      }
   }

}

function Armor::onTargetEnterLOS(%this,%player)
{

   //echo( "\c4Armor::onTargetEnterLOS(" SPC %this.getName() @", "@ %player.client.nameBase SPC ")" );
   %client = %player.client;
   %target = %player.getAimObject();

   if ( %target != -1 && %target.getState() !$= "Dead" && !%target.isCloaked() )
   {
      %targPos = %target.getBoxCenter();
      %dist = vectorDist( %player.getPosition(), %targPos );

      // AiClient is assigned to this AiPlayer?
      if ( isObject( %client ) && %client.isAiControlled() )
      {
         //echo( %client.nameBase @ "::onTargetEnterLOS(" SPC %target.client.nameBase SPC ")" );

         if ( %dist < $Bot::FireDistance )
         {
            //if ( %dist <= $Bot::MeleeDistance )
            //{
            //   %player.doMelee( %target );
            //}
            //else
            //{
               %player.setAimLocation( %targPos );

               if ( %player.getMountedImage( $GrenadeSlot ) != 0 && %player.hasAmmo( %player.getMountedImage( $GrenadeSlot ).item.getId() ) )
               {
                  //%player.triggerGrenade( $GrenadeSlot );
               }
            //}
         }
         else
         {
            if ( !%player.firing )
            {
               %client.chooseWeapon( %target, %dist );
               %player.triggerFire( $WeaponSlot, 1 );
            }
         }
      }
   }

}

function Armor::onEndSequence(%this,%player,%slot)
{
   %player.stopThread(%slot);
}

// ----------------------------------------------------------------------------
// Tasks
// ----------------------------------------------------------------------------

function AIPlayer::doWanderTask(%player)
{
   if ( !isObject( %player ) || %player.getState() $= "Dead" )
      return;

   %client = %player.client;

   %ranPos = %player.getRanBotDest();

   if ( !%player.setPathDestination( %ranPos ) )
      %player.setMoveDestination( %ranPos, false );

   %player.setAimLocation( %ranPos );
   %player.pathSet = 1;

   if ( %player.getMoveTrigger(5) ) // Stop sprinting if we are
      %player.clearMoveTrigger(5);

   // Find a target. If one is found our mode will change to attack
   %client.choosePlayerTarget();

   echo("\c5AIPlayer::doWanderTask(" SPC %player.client.nameBase @", "@ %ranPos SPC ")");
}

function AIPlayer::doGuardTask(%player)
{
   if ( !isObject( %player ) || %player.getState() $= "Dead" )
      return;

   %client = %player.client;

   // Look around
   %player.scanArea();
   //%player.moveRandom();

   // Find a target. If one is found our mode will change to attack
   %client.choosePlayerTarget();

   echo("\c5AIPlayer::doGuardTask(" SPC %player.client.nameBase SPC ")");
}

function AIPlayer::doHealthTask(%player, %health)
{
   %client = %player.client;
   //%player.allowTacticalMovement(false);

   %player.clearAim();
   %health = %player.findHealth();
   if ( isObject( %health ) )
   {
      %healthPos = %health.getPosition();
      %player.setPathDestination( %healthPos );
      //%player.setMoveDestination( %healthPos, true );
      %player.pathSet = 1;
   }
   else // Can't find any, back to business
   {
      if ( isObject( %client ) )
      {
         %client.taskMode = %client.prevMode;
         %client.prevMode = "";
      }
      else
      {
         %player.taskMode = %player.prevMode;
         %player.prevMode = "";
      }
   }

   echo("\c5AIPlayer::doHealthTask(" SPC %player.client.nameBase SPC ")");
}

function AIPlayer::findHealth(%player)
{
   %target = -1;
   InitContainerRadiusSearch( %player.getBoxCenter(), $Bot::DetectionDistance, $TypeMasks::ItemObjectType );
   while ((%tgt = containerSearchNext()) != 0)
   {
      if ( %tgt.getDataBlock().getName() $= "HealthPatch" )
      {
         %target = %tgt;
         break;
      }
   }
   return( %target );
}

function AIPlayer::doWingmanTask(%player)
{
   %client = %player.client;

   %wingman = %client.wingman;
   if ( isObject( %wingman ) && %wingman.getState() !$= "Dead" )
   {
      %player.followObject( %wingman, 5 );
      %player.pathSet = 1;

      // Find a target. If one is found our mode will change to attack
      %client.choosePlayerTarget();
   }
   else
   {
      // Could not find a wingman, switch tasks
      %client.setUpTasks();
   }

   echo("\c5AIPlayer::doWingmanTask(" SPC %player.client.nameBase SPC ")");
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

   %player.setMoveDestination( %newX SPC %newY SPC getTerrainHeight( %newX SPC %newY ), false );

   echo("\c5AIPlayer::moveRandom(" SPC %player.client.nameBase @", "@ %player.getMoveDestination() SPC ")");
}

// Look around in all directions, needs to be looped
function AIPlayer::scanArea(%player)
{
   echo("\c5AIPlayer::scanArea(" SPC %player.client.nameBase @", "@ %player.scanCheck SPC ")");

   %player.scanCheck++;
   switch( %player.scanCheck )
   {
      case 1:
         %player.setAimLocation( "0 0 1" );
      case 2:
         %player.setAimLocation( "99999 0 1" );
      case 3:
         %player.setAimLocation( "-99999 0 1" );
      case 4:
         %player.setAimLocation( "0 99999 1" );
      case 5:
         %player.setAimLocation( "0 -99999 1" );
      case 6:
         %player.scanCheck = 0;
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
*/
// ----------------------------------------------------------------------------
// Image Triggering/Combat
// ----------------------------------------------------------------------------

function AIPlayer::doMelee(%player, %target)
{
   if ( !isObject( %player ) || !isObject( %target ) || %player.getState() $= "Dead" || %target.getState() $= "Dead" || !$Game::Running )
      return;

   if ( %player.getEnergyLevel() <= 15 )
      return;

   %timeout = getSimTime() - %player.lastMeleeTime;
   if ( %timeout < $Bot::MeleeDelay ) // 1.5 second between melee attacks
      return;

   %player.setActionThread("ProxMine_Fire"); // Rigged DAE model

   // Tiring
   %player.setEnergyLevel( %player.getEnergyLevel() - 15 );

   if ( %player.checkInLos( %target, false, true )  )
   {
      %target.damage( %player, %player.getPosition(), 25, $DamageType::Melee );

      if ( %target.isMemberOfClass( "Player" ) )
      {
         //%target.repulse( %player );

         %pushDirection = VectorNormalize( VectorSub( %targPos, %player.getBoxCenter() ) );  
         %pushVec = getwords( VectorScale( %pushDirection, 200 ), 0, 1);   
         %target.applyImpulse( %targPos, %pushVec );
      }
   }

   // Get this last fire time and store it.
   %player.lastMeleeTime = getSimTime();
}

function AIPlayer::triggerFire(%player, %slot, %bool)
{
   //error("AIPlayer::triggerFire(" SPC %player.nameBase @", "@ %slot @", "@ %bool SPC ")");

   if ( !isObject( %player ) || %player.getState() $= "Dead" )
   {
      cancel( %player.fireTrigger );
      %player.clearMoveTrigger( %slot );
      //%player.setImageTrigger( %slot, 0 );
      return;
   }

   if ( %bool )
   {
      cancel( %player.fireTrigger );

      // Limit time between firings
      %timeout = getSimTime() - %player.lastFireTime;
      if ( %timeout < $Bot::FireDelay )
         return;

      %player.setMoveTrigger( %slot );
      //%player.setImageTrigger( %slot, 1 );
      %player.firing = true;

      %weapon = %player.getMountedImage(%slot).item.getName();
      switch$ ( %weapon )
      {
         case "Ryder":
            %time = 500;
         case "Lurker":
            %time = 200;
         case "Shotgun":
            %time = 1000;
         case "GrenadeLauncher":
            %time = 1200;
         case "SniperRifle":
            %time = 1100;
         default:
            %time = $Bot::FireTimeout;
      }

      %player.fireTrigger = %player.schedule( %time, "singleShot",  %slot, 0 );
   }
   else
   {
      cancel( %player.fireTrigger );
      %player.clearMoveTrigger( %slot );
      //%player.setImageTrigger( %slot, 0 );
      %player.firing = false;

      // Get this last fire time and store it.
      %player.lastFireTime = getSimTime();
   }
}

function AIPlayer::triggerGrenade(%player, %slot)
{
   cancel( %player.grenadeTrigger );

   if ( !isObject( %player ) || %player.getState() $= "Dead" )
   {
      %player.setImageTrigger( %slot, 0 );
      return;
   }

   // Limit time between firings
   %timeout = getSimTime() - %player.lastGrenadeTime;
   if ( %timeout < $Bot::GrenadeDelay )
      return;

   %player.setImageTrigger( %slot, 1 );
   %player.setImageTrigger( %slot, 0 );

   %player.lastGrenadeTime = getSimTime();
}

function AIPlayer::getVectorTo(%this, %target)
{
   %pos = %target.getPosition();
   %box1 = %this.getDataBlock().boundingBox;
   %box2 = %target.getDataBlock().boundingBox;

   %z = getWord(%box1, 2) / 2;
   %offset = "0 0" SPC %z;
     
   %vec = VectorAdd(%offset, %this.getPosition());
 
   %z = getWord(%box2, 2) / 2;
   %offset = "0 0" SPC %z;
     
   %pos = VectorAdd(%offset, %pos);
     
   return VectorSub(%pos, %vec);
}

//-----------------------------------------------------------------------------
// Waypoints
//-----------------------------------------------------------------------------

//MoveMap.bindCmd(keyboard, "x", "commandToServer(\'setWanderPosition\');", "");

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
      mMoveTolerance = 0.10;
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