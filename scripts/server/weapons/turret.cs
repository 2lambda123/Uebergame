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

// Respawntime is the amount of time it takes for a static "auto-respawn"
// turret to re-appear after it's been picked up.  Any turret marked as "static"
// is automaticlly respawned.
$TurretShape::RespawnTime = 30 * 1000;

// DestroyedFadeDelay is the how long a destroyed turret sticks around before it
// fades out and is deleted.
$TurretShape::DestroyedFadeDelay = 5 * 1000;

datablock SFXProfile(TargetAquiredSound)
{
   filename = "";
   description = AudioClose3D;
   preload = false;
};

datablock SFXProfile(TargetLostSound)
{
   filename = "";
   description = AudioClose3D;
   preload = false;
};

datablock SFXProfile(TurretDestroyed)
{
   filename = "";
   description = AudioClose3D;
   preload = false;
};

datablock SFXProfile(TurretThrown)
{
   filename = "";
   description = AudioClose3D;
   preload = false;
};

datablock SFXProfile(TurretFireSound)
{
   filename = "art/sound/weapons/turret/wpn_turret_fire";
   description = AudioBulletFire;
   preload = true;
};

datablock SFXProfile(TurretActivatedSound)
{
   filename = "art/sound/weapons/turret/wpn_turret_deploy";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(TurretScanningSound)
{
   filename = "art/sound/weapons/turret/wpn_turret_scan";
   description = AudioCloseLoop3D;
   preload = true;
};

datablock SFXProfile(TurretSwitchinSound)
{
   filename = "art/sound/weapons/turret/wpn_turret_switchin";
   description = AudioClosest3D;
   preload = true;
};

// ----------------------------------------------------------------------------
// TurretShapeData
// ----------------------------------------------------------------------------

function TurretShapeData::onAdd(%this, %obj)
{
   echo( "TurretShapeData::onAdd(" SPC %this.getName() @", "@ %obj.getClassName() SPC ")" );
   %obj.setRechargeRate( %this.rechargeRate );
   %obj.setEnergyLevel( %this.MaxEnergy );
   %obj.setRepairRate( %this.repairRate );

   if ( %obj.mountable || %obj.mountable $= "" )
      %this.isMountable(%obj, true);
   else
      %this.isMountable(%obj, false);

   if (%this.nameTag !$= "")
      %obj.setShapeName(%this.nameTag);

   // Mount weapons
   for(%i = 0; %i < %this.numWeaponMountPoints; %i++)
   {
      // Handle inventory
      %obj.incInventory(%this.weapon[%i], 1);
      if ( %this.weaponAmmo[%i] !$= "" ) // Might be energy based image
         %obj.incInventory(%this.weaponAmmo[%i], %this.weaponAmmoAmount[%i]);
      
      // Mount the image
      %obj.mountImage(%this.weapon[%i].image, %i, %this.startLoaded);
      %obj.setImageGenericTrigger(%i, 0, false); // Used to indicate the turret is destroyed
   }

   if ( %this.enterSequence !$= "" )
   {
      %obj.entranceThread = 0;
      %obj.playThread( %obj.entranceThread, %this.enterSequence );
      %obj.pauseThread(%obj.entranceThread);
   }
   else
   {
      %obj.entranceThread = -1;
   }
}

function TurretShapeData::onRemove(%this, %obj)
{
   //echo( "TurretShapeData::onRemove(" SPC %this.getName() @", "@ %obj.getClassName() SPC ")" );

   // if there are passengers/driver, kick them out
   for(%i = 0; %i < %this.numMountPoints; %i++)
   {
      if (%obj.getMountNodeObject(%i))
      {
         %passenger = %obj.getMountNodeObject(%i);
         %passenger.getDataBlock().doDismount(%passenger, true);
      }
   }

   if ( %obj.lastPilot.lastVehicle == %obj )
      %obj.lastPilot.lastVehicle = "";
}

// This is on MissionGroup so it doesn't happen when the mission has ended
function MissionGroup::respawnTurret(%this, %datablock, %className, %transform, %static, %respawn)
{
   %turret = new (%className)()
   {
      datablock = %datablock;
      static = %static;
      respawn = %respawn;
   };

   %turret.setTransform(%transform);
   MissionGroup.add(%turret);
   return %turret;
}

// ----------------------------------------------------------------------------
// TurretShapeData damage state
// ----------------------------------------------------------------------------

// This method is called by weapons fire
function TurretShapeData::damage(%this, %turret, %sourceObject, %position, %damage, %damageType)
{
   echo("TurretShapeData::damage(" SPC %turret @", "@ %sourceObject @", "@ %position @", "@ %damage @", "@ %damageType SPC ")" );

   if ( %turret.getState() $= "Dead" || %this.isInvincible )
      return;

   if ( isObject( %sourceObject ) )
      %turret.lastDamagedBy = %sourceObject;
   else
      %turret.lastDamagedBy = 0;

   %turret.damageTimeMS = GetSimTime();

   if ( %this.isShielded )
      %amount = %turret.imposeShield(%position, %damage, %damageType);

   // Cap the amount of damage applied if same team
   if ( !$FriendlyFire && !%this.deployedObject )
   {
      if ( isObject( %sourceObject ) )
      {
         if ( %sourceObject.team == %turret.team )
         {
            %curDamage = %turret.getDamageLevel();
            %availableDamage = %this.disabledLevel - %curDamage - 0.05;
            if ( %amount > %availableDamage )
               %amount = %availableDamage;
         }
      }
   }

   %damageScale = %this.damageScale[%damageType];
   if ( %damageScale !$= "" )
      %amount *= %damageScale;

   // apply damage
   if ( %amount > 0 )
      %turret.applyDamage( %amount );

   // Update the numerical Health HUD
   //%mountedObject = %turret.getObjectMount();
   //if (%mountedObject)
   //   %mountedObject.updateHealth();
}

function TurretShapeData::onDamage(%this, %obj, %delta)
{
   echo( "TurretShapeData::onDamage(" SPC %this.getName() @", "@ %obj.getClassName() @", "@ %delta SPC ")" );
   // This method is invoked by the ShapeBase code whenever the
   // object's damage level changes.

   %damage = %obj.getDamageLevel();
   if ( %damage >= %this.destroyedLevel )
   {
      if ( %obj.getDamageState() !$= "Destroyed" )
      {
         %obj.setDamageState(Destroyed);

         // Let the game object have a shot at doing something with this information..
         // We put this here so we have some time before the vehicle is deleted.
         if ( isObject( %obj.lastDamagedBy ) && isObject( Game ) )
            Game.turretDestroyed( %obj, %obj.lastDamagedBy );

         // if object has an explosion damage radius associated with it, apply explosion damage
         if ( %this.damageRadius )
            radiusDamage(%obj, %obj, %obj.getWorldBoxCenter(), %this.damageRadius, %this.radiusDamage, %this.radiusDamageType, %this.areaImpulse);

         %obj.setDamageLevel(%this.maxDamage);
      }
   }
   else if ( %damage >= %this.disabledLevel )
   {
      if ( %obj.getDamageState() !$= "Disabled" )
         %obj.setDamageState(Disabled);
   }
   else
   {
      // Lets add some sound to this, grab the sound profile from the datablock
      if ( %this.damageSound !$= "" )
         ServerPlay3D(%this.damageSound, %obj.getTransform());

      if ( %obj.getDamageState() !$= "Enabled" )
         %obj.setDamageState(Enabled);
   }
}

function TurretShapeData::onDestroyed(%this, %obj, %lastState)
{
   echo( "TurretShapeData::onDestroyed(" SPC %this.getName() @", "@ %obj.getClassName() @", "@ %lastState SPC ")" );
   // This method is invoked by the ShapeBase code whenever the
   // object's damage state changes.

   // Kill any occupants
   if ( %turret.getState() $= "Dead" )
   {
      for (%i = 0; %i < %this.numMountPoints; %i++)
      {
         %flingee = %turret.getMountNodeObject(%i);
         if ( %flingee.isMemberOfClass("Player") )
         {
            %flingee.getDataBlock().doDismount( %flingee, true );
            %xVel = 150.0 - (getRandom() * 300.0);
            %yVel = 150.0 - (getRandom() * 300.0);
            %zVel = (getRandom() * 50.0) + 50.0;
            %flingVel = %xVel @ " " @ %yVel @ " " @ %zVel;
            %flingee.applyImpulse( %flingee.getTransform(), %flingVel );
            %flingee.damage( %turret, %turret.getPosition(), 0.4, $DamageType::Crash );
         }
      }
   }

   if ( %this.deployedObject )
   {
      $TeamDeployedCount[%obj.team, %this.getName()]--;
      %obj.startFade(1000, $TurretShape::DestroyedFadeDelay, true);
      %obj.schedule($TurretShape::DestroyedFadeDelay + 1000, "delete");
   }
}

function TurretShapeData::onDisabled(%this, %obj, %lastState)
{
   echo( "TurretShapeData::onDisabled(" SPC %this.getName() @", "@ %obj.getClassName() @", "@ %lastState SPC ")" );
   // This method is invoked by the ShapeBase code whenever the
   // object's damage state changes.
   if ( %turret.getClassName() $= "AITurretShape" )
      %turret.deactivateTurret();

   %turret.setRechargeRate(0.0);
   %turret.setEnergyLevel(0.0);
}

function TurretShapeData::onEnabled(%this, %obj, %lastState)
{
   echo( "TurretShapeData::onEnabled" SPC %this.getName() @", "@ %obj.getClassName() @", "@ %lastState SPC ")" );
   // This method is invoked by the ShapeBase code whenever the
   // object's damage state changes.
   if ( %turret.getClassName() $= "AITurretShape" )
      %turret.activateTurret();

   %turret.setRechargeRate( %this.rechargeRate );
   %turret.setEnergyLevel( %this.MaxEnergy );
}

// ----------------------------------------------------------------------------
// TurretShapeData player mounting and dismounting
// ----------------------------------------------------------------------------

function TurretShapeData::isMountable(%this, %obj, %val)
{
   %obj.mountable = %val;
}

function TurretShapeData::onMountObject(%this, %turret, %player, %node)
{
   if (%turret.entranceThread >= 0)
   {
      %turret.setThreadDir(%turret.entranceThread, true);
      %turret.setThreadPosition(%turret.entranceThread, 0);
      %turret.playThread(%turret.entranceThread, "");
   }
}

function TurretShapeData::onUnmountObject(%this, %turret, %player)
{
   if (%turret.entranceThread >= 0)
   {
      // Play the entrance thread backwards for an exit
      %turret.setThreadDir(%turret.entranceThread, false);
      %turret.setThreadPosition(%turret.entranceThread, 1);
      %turret.playThread(%turret.entranceThread, "");
   }
}

function TurretShapeData::mountPlayer(%this, %turret, %player)
{
   //echo("\c4TurretShapeData::mountPlayer("@ %this.getName() @", "@ %turret @", "@ %player.client.nameBase @")");

   if (isObject(%turret) && %turret.getDamageState() !$= "Destroyed")
   {
      //%player.startFade(1000, 0, true);
      //%this.schedule(1000, "setMountTurret", %turret, %player);
      //%player.schedule(1500, "startFade", 1000, 0, false);
      %this.setMountTurret(%turret, %player);
   }
}

function TurretShapeData::setMountTurret(%this, %turret, %player)
{
   //echo("\c4TurretShapeData::setMountTurret("@ %this.getName() @", "@ %turret @", "@ %player.client.nameBase @")");

   if (isObject(%turret) && %turret.getDamageState() !$= "Destroyed")
   {
      %node = %this.findEmptySeat(%turret, %player);
      if (%node >= 0)
      {
         //echo("\c4Mount Node: "@ %node);
         %turret.mountObject(%player, %node);
         //%player.playAudio(0, MountVehicleSound);
         %player.mVehicle = %turret;
         %this.playerMounted( %turret, %player, %node );
      }
   }
}

function TurretShapeData::playerMounted(%data, %turret, %player, %node)
{
   //echo("\c2TurretShapeData::playerMounted(" SPC %data.getName() @ ", " @ %turret @ ", " @ %player @ ", " @ %node SPC ")");

   if ( %player.lastVehicle.lastPilot == %player && %player.lastVehicle != %turret )
   {
      %player.lastVehicle.lastPilot = "";
   }

   if ( %vehicle.lastPilot !$= "" && %turret.lastPilot.lastVehicle == %turret )
      %turret.lastPilot.lastVehicle = "";
            
   %turret.lastPilot = %player;
   %player.lastVehicle = %turret;

   // update spectators who are following this guy...
   if( %player.client.observeCount > 0 )
      resetSpectatorFollow( %player.client, false );
}

function TurretShapeData::playerDismounted(%data, %turret, %player, %node)
{
   //echo("\c2TurretShapeData::playerDismounted(" SPC %data.getName() @ ", " @ %turret @ ", " @ %player @ ", " @ %node SPC ")");

   %player.unmount();

   if( %player.client.observeCount > 0 )
      resetSpectatorFollow( %player.client, true );
}

function TurretShapeData::findEmptySeat(%this, %turret, %player)
{
   //echo("\c4This turret has "@ %this.numMountPoints @" mount points.");

   for (%i = 0; %i < %this.numMountPoints; %i++)
   {
      %node = %turret.getMountNodeObject(%i);
      if (%node == 0)
         return %i;
   }
   return -1;
}

function TurretShapeData::switchSeats(%this, %turret, %player)
{
   for (%i = 0; %i < %this.numMountPoints; %i++)
   {
      %node = %turret.getMountNodeObject(%i);
      if (%node == %player || %node > 0)
         continue;

      if (%node == 0)
         return %i;
   }
   return -1;
}

function TurretShapeData::onMount(%this, %turret, %player, %node)
{
   //echo("\c4TurretShapeData::onMount("@ %this.getName() @", "@ %turret @", "@ %player.client.nameBase @")");

   //%player.client.RefreshVehicleHud(%turret, %this.reticle, %this.zoomReticle);
   //%player.client.UpdateVehicleHealth(%turret);
}

function TurretShapeData::onUnmount(%this, %turret, %player, %node)
{
   //echo("\c4TurretShapeData::onUnmount(" @ %this.getName() @ ", " @ %turret @ ", " @ %player.client.nameBase @ ")");

   //%player.client.RefreshVehicleHud(0, "", "");
}

// ----------------------------------------------------------------------------
// TurretShape damage
// ----------------------------------------------------------------------------

// This method is called by weapons fire
function TurretShape::damage(%this, %sourceObject, %position, %damage, %damageType)
{
   echo("TurretShape::damage(" @ %this @ ", "@ %sourceObject @ ", " @ %position @ ", "@ %damage @ ", "@ %damageType @ ")");

   %this.getDataBlock().damage(%this, %sourceObject, %position, %damage, %damageType);
}

//-----------------------------------------------------------------------------
// AI Turret
//-----------------------------------------------------------------------------
function AITurretShapeData::onNewDatablock(%this, %turret)
{
   //LogEcho("\c2AITurretShapeData::onNewDatablock(" SPC %this.getName() @ ", " @ %turret.getClassName() SPC ")");
}

function AITurretShapeData::onAdd(%this, %turret)
{
   //LogEcho("\c2AITurretShapeData::onAdd(" SPC %this.getName() @", "@ %turret.getClassName() SPC ")" );

   %turret.setRechargeRate( %this.rechargeRate );
   %turret.setEnergyLevel( %this.maxEnergy );
   %turret.setRepairRate(0);
   %this.isMountable( %turret, false );

   if ( %this.nameTag !$= "" )
      %turret.setShapeName( %this.nameTag );

   // Mount weapons
   for ( %i = 0; %i < %this.numWeaponMountPoints; %i++ )
   {
      // Handle inventory
      %turret.incInventory( %this.weapon[%i], 1 );
      if ( %this.weaponAmmo[%i] !$= "" ) // Might be energy based image
         %turret.incInventory( %this.weaponAmmo[%i], %this.weaponAmmoAmount[%i] );
      
      // Mount the image
      %turret.mountImage( %this.weapon[%i].image, %i, %this.startLoaded );
      %turret.setImageGenericTrigger( %i, 0, false ); // Used to indicate the turret is destroyed
   }
}

function AITurretShapeData::onDestroyed(%this, %turret, %lastState)
{
   // This method is invoked by the ShapeBase code whenever the
   // object's damage state changes.
   //LogEcho("\c2AITurretShapeData::onDestroyed(" SPC %this.getName() @", "@ %turret.getClassName() @", "@ %lastState SPC ")" );

   %turret.playAudio( 0, TurretDestroyed );
   %turret.setAllGunsFiring( false );
   %turret.stopScanForTargets();
   %turret.resetTarget();
   %turret.setTurretState( "Destroyed", true );

   if ( isEventPending( %turret.thinkSchedule ) )
      cancel( %turret.thinkSchedule );

   // Set the weapons to destoryed
   for ( %i = 0; %i < %this.numWeaponMountPoints; %i++ )
   {
      %turret.setImageGenericTrigger( %i, 0, true );
   }

   if ( %this.deployedObject )
   {
      $TeamDeployedCount[%turret.team, %this.getName()]--;
      %turret.startFade( 1000, $TurretShape::DestroyedFadeDelay, true );
      %turret.schedule( $TurretShape::DestroyedFadeDelay + 1000, "delete" );
   }
}

function AITurretShapeData::onScanning(%this, %turret)
{
   if ( %turret.getDamageState() $= "Destroyed" )
      return;

   //LogEcho("\c2AITurretShapeData::onScanning(" SPC %this.getName() @", "@ %turret.getClassName() SPC ")" );

   %turret.startScanForTargets();
   %turret.playAudio(0, TurretScanningSound);
}

function AITurretShapeData::onTarget(%this, %turret)
{
   %target =  %turret.getTarget();
   %targName = isObject( %target.client ) ? %target.client.nameBase : %target.nameBase;
   //LogEcho("\c2AITurretShapeData::onTarget(" SPC deTag(%this.nameTag) @", "@ %targName SPC ")" );

   %turret.startTrackingTarget();
   //%turret.playAudio(0, TargetAquiredSound);
}

function AITurretShapeData::onNoTarget(%this, %turret)
{
   //LogEcho("\c2AITurretShapeData::onNoTarget(" SPC %this.getName() @", "@ %turret.hasTarget() SPC ")" );

   %turret.setAllGunsFiring(false);
   %turret.recenterTurret();
   //%turret.playAudio(0, TargetLostSound);
}

function AITurretShapeData::onFiring(%this, %turret)
{
   %target =  %turret.getTarget();
   %targName = isObject( %target.client ) ? %target.client.nameBase : %target.nameBase;
   //LogEcho("\c2AITurretShapeData::onFiring(" SPC %this.getName() @", "@ %targName SPC ")" );

   %turret.setAllGunsFiring(true);
}

function AITurretShapeData::onThrown(%this, %turret)
{
   //LogEcho("\c2AITurretShapeData::onThrown(" SPC %this.getName() @", "@ %turret.getClassName() SPC ")" );

   //%turret.playAudio(0, TurretThrown);
}

function AITurretShapeData::onDeploy(%this, %turret)
{
   //LogEcho("\c2AITurretShapeData::onDeploy(" SPC %this.getName() @", "@ %turret.getClassName() SPC ")" );

   // Set the weapons to loaded
   for(%i = 0; %i < %this.numWeaponMountPoints; %i++)
   {
      %turret.setImageLoaded(%i, true);
   }

   //> ZOD: Testing
   //%turret.thinkSchedule = %turret.schedule( 2000, "scanForTargets" );

   //%turret.playAudio(0, TurretActivatedSound);
}

function TurretShape::scanForTargets(%turret)
{
   //warn("AITurretShapeData::scanForTargets(" SPC deTag( %turret.getDataBlock().nameTag ) SPC ")");

   if ( isEventPending( %turret.thinkSchedule ) )
      cancel( %turret.thinkSchedule );

   if ( %turret.getDamageState() $= "Destroyed" )
      return;

   %turPos = %turret.getPosition();
   %reference = %data.maxScanDistance;

   // Create an array of potential targets filtering non desirables..
   InitContainerRadiusSearch( %turPos, %reference, ( $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType ) );
   while ( ( %target = containerSearchNext() ) != 0 )
   {
      if ( %target.getDamageState() !$= "Enabled" || %target.team == %turret.team ||
           %target.isCloaked() || !%target.isMemberOfClass( "Player" ) )
      {
         %turret.addToIgnoreList( %target );
         continue;
      }
      %turret.addToTargetList( %target ); //< ZOD: Added to engine code
   }

   %turret.thinkSchedule = %turret.schedule( 500, "scanForTargets" );
}
