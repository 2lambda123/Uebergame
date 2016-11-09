//-----------------------------------------------------------------------------
// Copyright (c) 2014 Guy Allard
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
// Base frequency for behavior tree ticks (in milliseconds)
//-----------------------------------------------------------------------------
$BotTickFrequency = 100;

// ----------------------------------------------------------------------------
// make the local player invisible to the AI
//-----------------------------------------------------------------------------
/*
function setGodMode(%val)
{
   LocalClientConnection.player.isGod = %val;
}

// don't damage an invincible player
function DefaultPlayerData::damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
{
   if(%obj.isGod)
      return;
   
   Parent::damage(%this, %obj, %sourceObject, %position, %damage, %damageType);
}
*/
//-----------------------------------------------------------------------------
// bot datablock
//-----------------------------------------------------------------------------
/*
datablock PlayerData(BotDefaultPlayerData : DefaultPlayerData)
{
	
   // max visible distance
   VisionRange = 40;
   
   // vision field of view
   VisionFov = 120;
   
   // max range to look for items
   findItemRange = 20;
   
   // the type of object to search for when looking for targets
   targetObjectTypes = $TypeMasks::PlayerObjectType;
   
   // the type of object to search for when looking for items
   itemObjectTypes = $TypeMasks::itemObjectType;
   
   // some numbers for testing
   
   // distance the bot wants to be from its target when using the Ryder
   optimalRange["Ryder"] = 8;
   
   // number of milliseconds to hold the trigger down when using the Ryder
   burstLength["Ryder"] = 100;
   
   // distance the bot wants to be from its target when using the Lurker
   optimalRange["Lurker"] = 12;
   
   // number of milliseconds to hold the trigger down when using the Lurker
   burstLength["Lurker"] = 750;
   
   // +/- deviation from optimal range that is tolerated
   rangeTolerance = 3;
   
   // probability that the bot will switch from its current target to another, closer target
   switchTargetProbability = 0.1;
   
   // disable other weapons, we don't know how to use them yet
   maxInv[LurkerGrenadeLauncher] = 0;
   maxInv[LurkerGrenadeAmmo] = 0;
   maxInv[ProxMine] = 0;
   maxInv[DeployableTurret] = 0;
   
   //BadBot AI settings
   VisionRange = 40;
   VisionFov = 180;
   findItemRange = 20;
   targetObjectTypes = $TypeMasks::PlayerObjectType;
   itemObjectTypes = $TypeMasks::itemObjectType;
   optimalRange["Ryder"] = 12;
   burstLength["Ryder"] = 100;
   optimalRange["Lurker"] = 16;
   burstLength["Lurker"] = 2000;
   optimalRange["Shotgun"] = 8;
   burstLength["Shotgun"] = 100;
   optimalRange["SniperRifle"] = 30;
   burstLength["SniperRifle"] = 2000;
   optimalRange["GrenadeLauncher"] = 25;
   burstLength["GrenadeLauncher"] = 2000;
   rangeTolerance = 3;
   switchTargetProbability = 0.5;
};

datablock PlayerData(BotPaintballPlayerData : DefaultPlayerData)
{
   shapeFile = "art/shapes/actors/paintball_player/paintball_player.dts";
   shapeNameFP[0] = "";
   boundingBox = "0.75 0.75 1.8";
   crouchBoundingBox = "0.75 0.75 1.25";
   renderFirstPerson = "1";
   
   groundImpactMinSpeed    = "4.1";
   groundImpactShakeFreq   = "3 3 3";
   groundImpactShakeAmp    = "0.2 0.2 0.2";
   groundImpactShakeDuration = "1";
   groundImpactShakeFalloff = 10.0;
   
   maxInvRyder = "0";
	
   VisionRange = 40;
   VisionFov = 180;
   findItemRange = 20;
   targetObjectTypes = $TypeMasks::PlayerObjectType;
   itemObjectTypes = $TypeMasks::itemObjectType;
   optimalRange["PaintballMarkerBlue"] = 10;
   burstLength["PaintballMarkerBlue"] = 2000;
   optimalRange["PaintballMarkerRed"] = 10;
   burstLength["PaintballMarkerRed"] = 2000;
   optimalRange["PaintballMarkerGreen"] = 10;
   burstLength["PaintballMarkerGreen"] = 2000;
   optimalRange["PaintballMarkerYellow"] = 10;
   burstLength["PaintballMarkerYellow"] = 2000;
   rangeTolerance = 5;
   switchTargetProbability = 0.5;
};
*/
//=============================================================================
// Supporting functions for an AIPlayer driven by a behavior tree
//=============================================================================

// Spawn a bot called %name located at %startpos
function BadBot::spawn(%name, %startPos)
{
   // create the bot
   %bot = new AIPlayer(%name) {
      dataBlock = BotDefaultPlayerData; 
      class = "BadBot";
   };
   
   // give it a name
   if(%name !$= "")
      %bot.setShapeName(%name);
   
   // set its position, or use the default if no position is given
   if(isObject(%startPos))
   {
      %startPos = %startPos.position;
   }
   else if(%startPos $= "")
   {
      %spawnPoint = pickPlayerSpawnPoint(PlayerDropPoints);
      if(isObject(%spawnPoint))
         %startPos = %spawnPoint.getPosition();
   }
   
   %bot.setPosition(%startPos);
   
   // tetherpoint will give the bot a place to call home
   %bot.tetherPoint = %startPos;
   
   return %bot;      
}

// override getMuzzleVector so that the bots aim at where they are looking
function BadBot::getMuzzleVector(%this, %slot)
{
   return %this.getEyeVector();
}

// moveTo command, %dest can be either a location or an object
function BadBot::moveTo(%obj, %dest, %slowDown)
{
   %pos = isObject(%dest) ? %dest.getPosition() : %dest;
   /*
   if ( %obj.getNavMesh() ) 
   { 
      %obj.setPathDestination(%pos); //setPath uses navMesh
   } 
   else 
   { 
      %obj.setMoveDestination(%pos); //old function wihtout navMesh
   } 
   */
   // above function is broken, sometimes bots do not find navMesh at the beginning
   // and get stuck on their first spawn, so only use navMesh for now until this is figured out
   %obj.setPathDestination(%pos);
   %obj.atDestination = false;
}

//=============================================================================
// Supporting playerData functions
//=============================================================================
/*
// use onAdd to equip the bot
function BotDefaultPlayerData::onAdd(%data, %obj)
{
   // give him the standard player loadout
   //game.loadout(%obj);
   
   if ( %obj.isBot )
   {
      // $Bot::Set is created in loadMissionStage2
      if ( $Bot::Set.acceptsAsChild( %obj ) )
         $Bot::Set.add( %obj );
      else
         error( "Failed to add new AiPlayer object to Bot Set!" );
   }
   
   %obj.setbehavior(BotTree, $BotTickFrequency);
}

// Override onDisabled so we can stop running the behavior tree
function BotDefaultPlayerData::onDisabled(%this, %obj, %state)
{
   Parent::onDisabled(%this, %obj, %state);
   
   if ( $Bot::Set.isMember( %obj ) )
      $Bot::Set.remove( %obj );
   else
      error( "Tried to remove AiPlayer from Bot Set that wasn't in the set!" );
  
   %obj.behaviorTree.stop();
}

// onMoveStuck has issues it seems

function BotDefaultPlayerData::onMoveStuck(%this, %obj)
{
   %obj.setShapeName("onMoveStuck"); //debug feature
   
   %obj.clearAim();
   %basePoint = %obj.position;
   %obj.moveTo(RandomPointOnCircle(%basePoint, 2));
}

// forward onReachDestination to the behavior tree as a signal
function BotDefaultPlayerData::onReachDestination(%data, %obj)
{
   if(isObject(%obj.behaviorTree))
      %obj.behaviorTree.postSignal("onReachDestination");
      
   %obj.atDestination = true;
   %obj.setShapeName("onReachDestination"); //debug feature
}

// forward animationDone callback to the behavior tree as a signal
function BotDefaultPlayerData::animationDone(%data, %obj)
{
   if(isObject(%obj.behaviorTree))
      %obj.behaviorTree.postSignal("onAnimationDone");
}
*/
// get the index of the closest node on the specified path
function BadBot::getClosestNodeOnPath(%this, %path)
{
   if(isObject(%path) && %path.isMemberOfClass("SimSet") && (%numNodes = %path.getCount()) > 0)
   {
      %bestNode = 0;
      %bestDist = VectorDist(%path.getObject(%bestNode).position, %this.position);
      
      for(%i=1; %i < %numNodes; %i++)
      {
         %node = %path.getObject(%i);
         %dist = VectorDist(%node.position, %this.position);
         
         if(%dist < %bestDist)
         {
            %bestNode = %i;
            %bestDist = %dist;  
         }
      }
      
      return %bestNode;
   }
   return -1;
}

// send a chat message from the bot
function BadBot::say(%this, %message)
{
   chatMessageAll(%this, '\c3%1: %2', %this.getShapeName(), %message);  
}

function BadBot::AIreloadWeapon(%player)
{
   //%player = %client.getControlObject(); // might be a camera
   //%player = %client.player;
   %image = %player.getMountedImage( $WeaponSlot );
   
   if ( %player.isReloading == true ) return;
   if ( !%image.isField("clip") ) return;
   if ( %player.getInventory(%image.clip) <= 0 ) return;
   
   // Don't reload if the weapon's full. //function is broken since we are using the sms system.
   //if ( %player.getInventory(%image.ammo) == %image.ammo.maxInventory )
   //return;
 
   // No Iron Sight aiming while reloading.
   if (%player.isInIronSights == true)
      return;
 
   if ( %image > 0 )
   {
      //TODO: partial clip storage and drop to ground.
      %image.clearAmmoClip( %player, $WeaponSlot );
      %image.startReloadAmmoClip(%player, $WeaponSlot);
      %player.isReloading = true;
	  %player.allowSprinting(false);
   }
}

// Check to see if we hit target with scan instead of checking if we hit obstruction.
// This check prevents shooting teammates in the back
function BadBot::hasLosToTarget(%obj, %target)
{
   if ( !isObject(%obj) || %obj.getState() $= "Dead" )
      return( false );

   %start = %obj.getMuzzlePoint(%slot);
   %end = getBoxCenter(%target);
   %mask = ( $TypeMasks::StaticTSObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::ShapeBaseObjectType );
   
   // 4th argument of the raycast should be hostile bots to ignore, since we want only check for friendly  bots in the way, but not done yet
   //%scan = containerRayCast(%eyePos, %eyeEnd, %mask, %obj.toIgnore);
   
   %scan = containerRayCast(%start, %end, %mask);
   %result = firstWord( %scan );

   if (isObject(%obj))
      if (%scan == %target)
         return true;

   return( false );
}

//=============================Global Utility==================================
function RandomPointOnCircle(%center, %radius)
{
   %randVec = (getRandom() - 0.5) SPC (getRandom() - 0.5) SPC "0";
   %randVec = VectorNormalize(%randVec);
   %randVec = VectorScale(%randVec, getrandom() * %radius);
   return VectorAdd(%center, %randVec);  
}

//===========================ScriptedBehavior Tasks=============================
/*
   ScriptedBehavior Tasks are composed of four (optional) parts:
   1) precondition - this function should return a boolean indicating whether
      or not the behavior should continue. If precondition returns true, the
      rest of the behavior is evaluated. If precondition returns false, the
      behavior will abort.
      
      There are two options for the evaluation of the precondition that can be 
      set in the editor:
      ONCE - The precondition is run the first time the behavior becomes active
      TICK - The precondition is run each time the behavior is ticked (if latent)
   
   2) onEnter - This is called the first time the behavior is run if the 
      precondition was successful. onEnter does not use a return value.
      
   3) onExit - This is called if the behavior reaches completion. onExit does
      not use a return value.
   
   4) behavior - This is the main behavior function, evaluated each tick.
      behavior must return a status (SUCCES / FAILURE / RUNNING).
*/

//==============================================================================
// wander behavior task
//==============================================================================
function wanderTask::behavior(%this, %obj)
{
   %obj.setShapeName(wanderTask); //debug feature
   // stop aiming at things
   %obj.clearAim();
   
   // if the bot has a tetherPoint, use that as the center of his wander area,
   // otherwise use his current position
   //%basePoint = %obj.tetherPoint !$= "" ? %obj.tetherPoint : %obj.position;
   %basePoint = %obj.position;
   
   // move   
   %obj.moveTo(RandomPointOnCircle(%basePoint, 15));
   
   return SUCCESS;
}

//==============================================================================
// Move to closest node task
//==============================================================================
function moveToClosestNodeTask::precondition(%this, %obj)
{
   // check that the object has a path to folow
   return isObject(%obj.path);  
}

function moveToClosestNodeTask::onEnter(%this, %obj)
{
   // stop aiming
   %obj.clearAim();  
}

function moveToClosestNodeTask::behavior(%this, %obj)
{
   %obj.setShapeName(moveToClosestNodeTask); //debug feature
   // get the closest node
   %obj.currentNode = %obj.getClosestNodeOnPath(%obj.path);
   
   // move toward it
   %obj.moveTo(patrolPath.getObject(%obj.currentNode));
   return SUCCESS;
}

//==============================================================================
// Patrol behavior task
//==============================================================================
function patrolTask::precondition(%this, %obj)
{
   // the bot needs a path object
   return isObject(%obj.path);
}

function patrolTask::onEnter(%this, %obj)
{
   // stop aiming
   %obj.clearAim();
}

function patrolTask::behavior(%this, %obj)
{
   %obj.setShapeName(patrolTask); //debug feature
   // hook into the standard AIPlayer path following
   %obj.moveToNextNode();
   return SUCCESS;
}

//=============================================================================
// findHealth task
//=============================================================================
function findHealthTask::behavior(%this, %obj)
{
   %obj.setShapeName(findHealthTask); //debug feature
   // get the objects datablock
   %db = %obj.dataBlock;
   
   // do a container search for items
   initContainerRadiusSearch( %obj.position, %db.findItemRange, %db.itemObjectTypes );
   while ( (%item = containerSearchNext()) != 0 )
   {
      // filter out irrelevant items
      if(%item.dataBlock.category !$= "Health" || !%item.isEnabled() || %item.isHidden())
         continue;
      
      // check that the item is within the bots view cone
      if(%obj.checkInFov(%item, %db.visionFov))
      {
         // set the targetItem field on the bot
         %obj.targetItem = %item;
         break;
      }
   }
   
   return isObject(%obj.targetItem) ? SUCCESS : FAILURE;
}

//=============================================================================
// getHealth task
//=============================================================================
function getHealthTask::precondition(%this, %obj)
{
   // check that we have a valid health item to go for
   return (isObject(%obj.targetItem) && %obj.targetItem.isEnabled() && !%obj.targetItem.isHidden());  
}

function getHealthTask::onEnter(%this, %obj)
{
   // move to the item
   %obj.moveTo(%obj.targetItem.position);  
}

function getHealthTask::behavior(%this, %obj)
{
   %obj.setShapeName(getHealthTask); //debug feature
   // succeed when we reach the item
   if(!%obj.atDestination)
      return RUNNING;
   
   return SUCCESS;
}

//=============================================================================
// pickTarget task
//=============================================================================
function pickTargetTask::precondition(%this, %obj)
{
   // decide if we should pick a new target or keep the old one
   if(isObject(%obj.targetObject))
   {
      return (VectorDist(%obj, %obj.targetObject) > %obj.dataBlock.visionRange ||
              getRandom() < %obj.dataBlock.switchTargetProbability);
   }
   
   return true;
}

function pickTargetTask::onEnter(%this, %obj)
{
   // stop aiming
   %obj.clearAim();
}

function pickTargetTask::behavior(%this, %obj)
{
   //%obj.setShapeName(pickTargetTask); //debug feature
   
   %obj.targetObject = -1;
   %db = %obj.dataBlock;
   
   // container search for other players
   initContainerRadiusSearch( %obj.position, %db.VisionRange, %db.targetObjectTypes );
   while ( (%target = containerSearchNext()) != 0 )
   {
      // don't target ourself, dead players or god.
      if(%target == %obj || !%target.isEnabled() || %target.isGod)
         continue;
      // don't target our team mates
      if(%target.team == %obj.team)
         continue;
      // Check that the target is within the bots view cone
      if(%obj.checkInFov(%target, %db.visionFov))
      {
         // set the targetObject
         %obj.targetObject = %target;
         break;
      }
   }
   
   return SUCCESS;
}

//=============================================================================
// followTargetTask
//=============================================================================
function huntTargetTask::precondition(%this, %obj)
{
   // need to be alive and have a target
   return isObject(%obj.targetObject) && %obj.isEnabled();
}

function huntTargetTask::onEnter(%this, %obj)
{
   // stop aiming
   %obj.clearAim();
}

function huntTargetTask::behavior(%this, %obj)
{
   %obj.setShapeName(huntTargetTask); //debug feature

   %target = %obj.targetObject;

   //%obj.followObject(%target, 1);
   %obj.moveTo(%target);
   
   return SUCCESS;
}

//=============================================================================
// aimAtTargetTask
//=============================================================================
function aimAtTargetTask::precondition(%this, %obj)
{
   // need to be alive and have a target
   return isObject(%obj.targetObject) && %obj.isEnabled() && %obj.checkInLos(%obj.targetObject);
}

function aimAtTargetTask::behavior(%this, %obj)
{
   %obj.setShapeName(aimAtTargetTask); //debug feature
   // calculate an aim offset
   %targetPos = %obj.targetObject.getWorldBoxCenter();
   %weaponImage = %obj.getMountedImage($WeaponSlot);
   %projectile = %weaponImage.projectile;
   %correction = "0 0 1";
   if(isObject(%projectile))
   {
      // simple target leading approximation (not for ballistics)
      %targetDist = VectorDist(%targetPos, %obj.position);
      %bulletVel = %projectile.muzzleVelocity;
      %targetVel = %obj.targetObject.getVelocity();      
      %correction = VectorAdd(%correction, VectorScale( %targetVel, (%targetDist / %bulletVel) ));
   }
   %obj.setAimObject(%obj.targetObject, %correction);
   
   return SUCCESS;
}

//=============================================================================
// shootAtTargetTask
//=============================================================================
function shootAtTargetTask::precondition(%this, %obj)
{
   //echo ( "obj has los to target : " @ %obj.hasLosToTarget() );
   return isObject(%obj.targetObject) && 
          %obj.checkInLos(%obj.targetObject) &&
          VectorDot(VectorNormalize(VectorSub(%obj.getAimLocation(), %obj.position)), %obj.getForwardVector()) > 0.9 &&
          %obj.getImageAmmo($WeaponSlot); // && %obj.hasLosToTarget(); //not working well right now, needs fixing
}

function shootAtTargetTask::behavior(%this, %obj)
{
   %obj.setShapeName(shootAtTargetTask); //debug feature
   if(!isEventPending(%obj.triggerSchedule))
   {
      %obj.setImageTrigger($WeaponSlot, true);
      //%burstLength = %obj.dataBlock.burstLength[%obj.getMountedImage($WeaponSlot).item]
	  %burstRandomLength = getRandom ( 100, %obj.dataBlock.burstLength[%obj.getMountedImage($WeaponSlot).item] );
	  //echo( "Burst length: " @ %burstRandomLength );
      %obj.triggerSchedule = %obj.schedule(%burstRandomLength, setImageTrigger, $WeaponSlot, false);
   }

   return SUCCESS;
}

//=============================================================================
// reloadWeaponTask
//=============================================================================
function reloadWeaponTask::precondition(%this, %obj)
{
   %weaponImage = %obj.getMountedImage($WeaponSlot);

   //if( %obj.getImageAmmo($WeaponSlot) <= 0 && %obj.getInventory(%obj.getMountedImage($WeaponSlot).clip > 0)) return true;
   //echo ("has ai clip? : " @ %obj.getInventory(%obj.getMountedImage($WeaponSlot).clip));
   //echo ("has ai clip? : " @ %obj.getInventory (%weaponImage.clip));

   if (%obj.getInventory (%weaponImage.clip) > 0 && %obj.isReloading == false) return true;
}

function reloadWeaponTask::behavior(%this, %obj)
{
   %obj.setShapeName(reloadWeaponTask); //debug feature
   if(!isEventPending(%obj.triggerSchedule))
   {
	  //%reload = commandToServer( 'reloadWeapon' );
	  //%reload = %obj.reloadWeapon;
	  %reloadRandomLength = getRandom ( 100, 1500 );
      %obj.triggerSchedule = %obj.schedule(%reloadRandomLength, AIreloadWeapon);
   }
   return SUCCESS;
}

//=============================================================================
// combatMoveTask
//=============================================================================
function combatMoveTask::behavior(%this, %obj)
{
   %obj.setShapeName(combatMoveTask); //debug feature
   %image = %obj.getMountedImage($WeaponSlot);
   %db = %obj.getDatablock();
   %optimalRange = %db.optimalRange[%image.item.description];
   %currentRange = VectorDist(%obj.position, %obj.targetObject.position);
   %rangeDelta = %currentRange - %optimalRange;

   %moveVec = "0 0 0";
   %fwd = %obj.getForwardVector();
   %right = %obj.getRightVector();
   
   // forward / back to stay in range
   if(mAbs(%rangeDelta) > %db.rangeTolerance)
      %moveVec = VectorScale(%fwd, %rangeDelta);
   
   // random side strafe
   %moveVec = VectorAdd(%moveVec, VectorScale(%right, 5 * (getRandom(0,2) - 1)));
      
   %obj.moveTo(VectorAdd(%obj.position, %moveVec));
   
   return SUCCESS;
}
