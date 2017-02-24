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

// ----------------------------------------------------------------------------
// AI Turret Head
// ----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Turret Bullet Projectile
//-----------------------------------------------------------------------------

datablock ProjectileData( TurretBulletProjectile )
{
   projectileShapeName = "";

   directDamage        = 5;
   radiusDamage        = 0;
   damageRadius        = 0.5;
   areaImpulse         = 0.5;
   impactForce         = 1;
   
   damageType          = $DamageType::Turret;  // Type of damage applied by this weapon

   explosion           = BulletExplosion;
   waterExplosion      = BulletWaterExplosion;
   playerExplosion     = PlayerBloodExplosion;
   decal               = BulletHoleDecal;

   //particleEmitter     = "BulletTrailEmitter";
   //particleWaterEmitter = "UWBulletTrailEmitter";

   Splash              = "";
   muzzleVelocity      = 500;
   velInheritFactor    = 0;

   armingDelay         = 0;
   lifetime            = 992;
   fadeDelay           = 800;

   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod          = 1;

   lightDesc           = "";
};

//-----------------------------------------------------------------------------
// Turret Bullet Ammo
//-----------------------------------------------------------------------------

datablock ItemData(AITurretAmmo : DefaultAmmo)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Turret/Turret_Legs.dts";

   // Dynamic properties defined by the scripts
   pickUpName = 'turret ammo';
};

//-----------------------------------------------------------------------------
// AI Turret Weapon
//-----------------------------------------------------------------------------

datablock ItemData(AITurretHead : DefaultWeapon)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Turret/Turret_Head.dts";

   // Dynamic properties defined by the scripts
   pickUpName = 'an AI turret head';
   image = AITurretHeadImage;
};

datablock ShapeBaseImageData(AITurretHeadImage)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Turret/Turret_Head.dts";
   emap = true;

   // Specify mount point
   mountPoint = 0;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";

   // Projectiles and Ammo.
   item = AITurretHead;
   //ammo = AITurretAmmo;

   usesEnergy = true;
   useMountEnergy = true;
   fireEnergy = 1;
   minEnergy = 2;

   projectile = TurretBulletProjectile;
   projectileType = Projectile;
   projectileSpread = 0;

   casing = BulletShell;
   shellExitDir        = "1.0 0.3 1.0";
   shellExitOffset     = "0.15 -0.56 -0.1";
   shellExitVariance   = 15.0;
   shellVelocity       = 3.0;

   // Weapon lights up while firing
   lightType = "WeaponFireLight";
   lightColor = "0.992126 0.968504 0.708661 1";
   lightRadius = "4";
   lightDuration = "100";
   lightBrightness = 2;

   // Shake camera while firing.
   shakeCamera = false;
   camShakeFreq = "0 0 0";
   camShakeAmp = "0 0 0";

   useRemainderDT = true;

   // Initial start up state
   stateName[0]                     = "Preactivate";
   stateIgnoreLoadedForReady[0]     = false;
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNotLoaded[0]    = "WaitingDeployment";  // If the turret weapon is not loaded then it has not yet been deployed
   stateTransitionOnNoAmmo[0]       = "NoAmmo";

   // Activating the gun.  Called when the weapon is first
   // mounted and there is ammo.
   stateName[1]                     = "Activate";
   stateTransitionGeneric0In[1]     = "Destroyed";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 0.5;
   stateSequence[1]                 = "Activate";

   // Ready to fire, just waiting for the trigger
   stateName[2]                     = "Ready";
   stateTransitionGeneric0In[2]     = "Destroyed";
   stateTransitionOnNoAmmo[2]       = "NoAmmo";
   stateTransitionOnTriggerDown[2]  = "Fire";
   stateSequence[2]                 = "scan";

   // Fire the weapon. Calls the fire script which does
   // the actual work.
   stateName[3]                     = "Fire";
   stateTransitionGeneric0In[3]     = "Destroyed";
   stateTransitionOnTimeout[3]      = "Reload";
   stateTimeoutValue[3]             = 0.2;
   stateFire[3]                     = true;
   stateRecoil[3]                   = "LightRecoil";
   stateAllowImageChange[3]         = false;
   stateSequence[3]                 = "fire";
   stateSequenceRandomFlash[3]      = true;        // use muzzle flash sequence
   stateScript[3]                   = "onFire";
   stateSound[3]                    = TurretFireSound;
   stateEmitter[3]                  = GunFireSmokeEmitter;
   stateEmitterTime[3]              = 0.025;
   stateEjectShell[3]               = true;

   // Play the reload animation, and transition into
   stateName[4]                     = "Reload";
   stateTransitionGeneric0In[4]     = "Destroyed";
   stateTransitionOnNoAmmo[4]       = "NoAmmo";
   stateTransitionOnTimeout[4]      = "Ready";
   stateWaitForTimeout[4]           = "0";
   stateTimeoutValue[4]             = 0.0;
   stateAllowImageChange[4]         = false;
   stateSequence[4]                 = "Reload";

   // No ammo in the weapon, just idle until something
   // shows up. Play the dry fire sound if the trigger is
   // pulled.
   stateName[5]                     = "NoAmmo";
   stateTransitionGeneric0In[5]     = "Destroyed";
   stateTransitionOnAmmo[5]         = "Reload";
   stateSequence[5]                 = "NoAmmo";
   stateTransitionOnTriggerDown[5]  = "DryFire";

   // No ammo dry fire
   stateName[6]                     = "DryFire";
   stateTransitionGeneric0In[6]     = "Destroyed";
   stateTimeoutValue[6]             = 1.0;
   stateTransitionOnTimeout[6]      = "NoAmmo";
   stateScript[6]                   = "onDryFire";

   // Waiting for the turret to be deployed
   stateName[7]                     = "WaitingDeployment";
   stateTransitionGeneric0In[7]     = "Destroyed";
   stateTransitionOnLoaded[7]       = "Deployed";
   stateSequence[7]                 = "wait_deploy";

   // Turret has been deployed
   stateName[8]                     = "Deployed";
   stateTransitionGeneric0In[8]     = "Destroyed";
   stateTransitionOnTimeout[8]      = "Ready";
   stateWaitForTimeout[8]           = true;
   stateTimeoutValue[8]             = 2.5;   // Same length as turret base's Deploy state
   stateSequence[8]                 = "deploy";

   // Turret has been destroyed
   stateName[9]                     = "Destroyed";
   stateSequence[9]                 = "destroyed";
};

function AITurretHeadImage::onFire(%data, %obj, %slot)
{
   //LogEcho("AITurretHeadImage::onFire(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");

   if ( %data.ammo !$="" )
   {
      if ( %obj.getInventory( %data.ammo ) <= 0 )
         return;

      // Decrement inventory ammo. The image's ammo state is update
      // automatically by the ammo inventory hooks.
      %obj.decInventory( %data.ammo, 1 );
   }

   if ( %data.usesEnergy )
   {
      if ( %obj.getEnergyLevel() < %data.minEnergy )
         return;

      %obj.setEnergyLevel( %obj.getEnergyLevel() - %data.fireEnergy );
   }

   %data.lightStart = $Sim::Time;

   if( %data.projectileSpread > 0 )
   {
      %vec = %obj.getMuzzleVector(%slot);
      %x = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %y = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %z = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
      %muzzleVector = MatrixMulVector(%mat, %vec);
   }
   else
   {
      %muzzleVector = MatrixMulVector("0 0 0 0 0 1 0", %obj.getMuzzleVector(%slot));
   }

   // Determin initial projectile velocity based on the 
   // gun's muzzle point and the object's current velocity
   %objectVelocity = %obj.getVelocity();
   %muzzleVelocity = VectorAdd(VectorScale(%muzzleVector, %data.projectile.muzzleVelocity), VectorScale(%objectVelocity, %data.projectile.velInheritFactor));

   // Create the projectile object
   %p = new (%data.projectileType)() {
      dataBlock        = %data.projectile;
      initialVelocity  = %muzzleVelocity;
      initialPosition  = %obj.getMuzzlePoint(%slot);
      // This parameter is deleted about 7 ticks into the projectiles flight
      sourceObject     = %obj;
      sourceSlot       = %slot;
      // We use this for the source object when applying damage because it isn't deleted
      origin           = %obj;
      client           = %obj.client;
      target           = %obj.getTarget();
   };

   MissionCleanup.add(%p);

   return %p;
}

//-----------------------------------------------------------------------------
// AI Turret Base
//-----------------------------------------------------------------------------

datablock AITurretShapeData(SentryTurret : TurretDamageScale)
{
   category = "Turrets";
   shapeFile = "art/shapes/weapons/Turret/Turret_Legs.dts";

   maxDamage = 75.1; // Must be higher then destroyed level
   destroyedLevel = 75;
   disabledLevel = 65;
   explosion = MediumExplosion;
   damageRadius = 10.0;
   radiusDamage = 25;
   damageType = $DamageType::Explosion;
   areaImpulse = 2000;

   isShielded = true;
   energyPerDamagePoint = 50;
   maxEnergy = 800;
   rechargeRate = 0.31;
   repairRate = 0.005;
   heat = 1.0;

   nameTag = 'Sentry Turret';

   startLoaded = false;          //Does the turret's mounted weapon(s) start in a loaded state.

   mass = 5;
   elasticity = 0.1;
   friction = 0.6;

   simpleServerCollision = false;
   zRotOnly = false;
   
   // Rotation settings
   minPitch = 15;                 //Minimum number of degrees to rotate down from straight ahead.
   maxPitch = 80;                 //Maximum number of degrees to rotate up from straight ahead.
   pitchRate = 50;                //Degrees per second rotation.
   maxHeading = 120;              //Maximum number of degrees to rotate from center.
   headingRate = 50;              //Degrees per second rotation.

   // Scan settings
   scanTickFrequency = 3;         //How often should we perform a full scan when looking for a target.
   scanTickFrequencyVariance = 1; //Random amount that should be added to the scan tick frequency each scan period.
   maxScanPitch = 45;             //Maximum number of degrees to scan up and down.
   maxScanHeading = 90;           //Maximum number of degrees to scan left and right.
   maxScanDistance = 50;          //Maximum distance to scan.

   trackLostTargetTime = 2;       //How long after the turret has lost the target should it still track it.

   maxWeaponRange = 40;           //Maximum distance that the weapon will fire upon a target.
   weaponLeadVelocity = 0;        //Velocity used to lead target.

   // Weapon mounting
   numWeaponMountPoints = 1;

   //FireTogether: All weapons fire under trigger 0.
   //GroupedFire: Weapon mounts 0,2 fire under trigger 0, mounts 1,3 fire under trigger 1.
   //IndividualFire: Each weapon mount fires under its own trigger 0-3.
   weaponLinkType = "FireTogether";

   weapon[0] = AITurretHead;
   weaponAmmo[0] = AITurretAmmo;
   weaponAmmoAmount[0] = 10000;

   maxInv[AITurretHead] = 1;
   maxInv[AITurretAmmo] = 10000;

   // Initial start up state
   stateName[0]                     = "Preactivate";

   // If this is being deployed via a weapon leave these in, else if from a special, comment out
   stateTransitionOnAtRest[0]       = "Scanning";
   stateTransitionOnNotAtRest[0]    = "Thrown";
   
   // Scan for targets
   stateName[1]                     = "Scanning";
   stateScan[1]                     = true;
   stateTransitionOnTarget[1]       = "Target";
   stateSequence[1]                 = "scan";
   stateScript[1]                   = "onScanning";

   // Have a target
   stateName[2]                     = "Target";
   stateTransitionOnNoTarget[2]     = "NoTarget";
   stateTransitionOnTimeout[2]      = "Firing";
   stateTimeoutValue[2]             = 0.25;//2.0;
   stateScript[2]                   = "onTarget";

   // Fire at target
   stateName[3]                     = "Firing";
   stateFire[3]                     = true;
   stateTransitionOnNoTarget[3]     = "NoTarget";
   stateScript[3]                   = "onFiring";

   // Lost target
   stateName[4]                     = "NoTarget";
   stateTransitionOnTimeout[4]      = "Scanning";
   stateTimeoutValue[4]             = 2.0;
   stateScript[4]                   = "onNoTarget";

   // Player thrown turret
   stateName[5]                     = "Thrown";
   stateTransitionOnAtRest[5]       = "Deploy";
   stateSequence[5]                 = "throw";
   stateScript[5]                   = "onThrown";

   // Player thrown turret is deploying
   stateName[6]                     = "Deploy";
   stateTransitionOnTimeout[6]      = "Scanning";
   stateTimeoutValue[6]             = 2.5;
   stateSequence[6]                 = "deploy";
   stateScaleAnimation[6]           = true;
   stateScript[6]                   = "onDeploy";

   // Special state that is set when the turret is destroyed.
   // This state is set in the onDestroyed() callback.
   stateName[7]                     = "Destroyed";
   stateSequence[7]                 = "destroyed";
};
