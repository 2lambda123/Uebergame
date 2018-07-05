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

//--------------------------------------------------------------------------
// Sounds
//--------------------------------------------------------------------------

datablock SFXProfile(ShotgunFireSound)
{
   filename = "art/sound/weapons/shotgun_fire";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(ShotgunCockSound)
{
   fileName = "art/sound/weapons/shotgun_reload";
   description = "AudioClose3D";
   preload = true;
};

datablock SFXProfile(ShotgunReloadSound)
{
   filename = "art/sound/weapons/wpn_lurker_reload";
   description = AudioClosest3D;
   preload = true;
};
/*
datablock SFXPlayList(ShotgunFireSoundList)
{
   // Use a looped description so the list playback will loop.
   description = AudioClose3D;

   track[ 0 ] = ShotgunFireSound;
};
*/
datablock SFXProfile(ShotgunSwitchinSound)
{
   fileName = "art/sound/weapons/wpn_lurker_switchin";
   description = "AudioClosest3D";
   canSave = "1";
};

//-----------------------------------------------------------------------------
// Projectile Object
//-----------------------------------------------------------------------------

datablock ProjectileData( ShotgunProjectile )
{
   projectileShapeName = "";
   //sound               = "";
   directDamage        = 13;
   radiusDamage        = 0;
   damageRadius        = 0;
   areaImpulse         = 0.5;
   impactForce         = 1;
   damageType          = $DamageType::Shotgun;

   explosion           = BulletExplosion;
   waterExplosion      = BulletWaterExplosion;
   playerExplosion     = PlayerBloodExplosion;
   decal               = BulletHoleDecal;

   //particleEmitter     = "BulletTrailEmitter";
   //particleWaterEmitter = "UWBulletTrailEmitter";

   Splash              = "";
   muzzleVelocity      = 300;
   velInheritFactor    = 0;

   armingDelay         = 0; // How long it should not detonate on impact
   lifetime            = 500; // How long the projectile should exist before deleting itself
   fadeDelay           = 0; // Brief Amount of time, in milliseconds, before the projectile begins to fade out.

   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   gravityMod          = 1;

   lightDesc           = "";
};

datablock ProjectileData( ShotgunUnderWaterProjectile : ShotgunProjectile )
{
   particleWaterEmitter = "UWBulletTrailEmitter";
   lifetime             = 400;
   muzzleVelocity       = 10;
};

//-----------------------------------------------------------------------------
// Ammo Item
//-----------------------------------------------------------------------------
datablock ItemData(ShotgunClip : DefaultClip)
{
   // Basic Item properties
   //shapeFile = "art/shapes/weapons/shotgun/shotgun_TP.dts";

   // Dynamic properties defined by the scripts
   pickUpName = 'Shotgun magazine';
};

datablock ItemData(ShotgunAmmo : DefaultAmmo)
{
   //shapeFile = "art/shapes/weapons/shotgun/shotgun_TP.dts";
   pickUpName = 'Shotgun ammo';
   clip = ShotgunClip;
};

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the LurkerWeaponImage is used.
//-----------------------------------------------------------------------------
datablock ItemData(Shotgun : DefaultWeapon)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/shotgun/shotgun_TP.dts";

   // Dynamic properties defined by the scripts
   pickUpName = 'Shotgun';
   image = ShotgunWeaponImage;
};

datablock ShapeBaseImageData(ShotgunWeaponImage)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/shotgun/shotgun_TP.dts";
   shapeFileFP = "art/shapes/weapons/shotgun/shotgun_FP.dts";
   emap = true;
   computeCRC = false;

   imageAnimPrefix = "Rifle";
   imageAnimPrefixFP = "Rifle";

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   firstPerson = true;
   useEyeNode = false;
   animateOnServer = true;
   cloakable = true;
   eyeOffset = "0.025 -0.13 -0.115";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.
   correctMuzzleVector = true;
   correctMuzzleVectorTP = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";

   // Projectiles and Ammo.
   item = Shotgun;
   ammo = ShotgunAmmo;
   clip = ShotgunClip;
   ironSight = ShotgunIronSightImage;

   usesEnergy = 0;
   minEnergy = 0;

   projectile = ShotgunProjectile;
   underWaterProjectile = ShotgunUnderWaterProjectile;
   projectileType = Projectile;
   projectileSpread = 0.02;
   projectileNum = 8;

   altProjectile = GrenadeLauncherProjectile;
   altProjectileSpread = "0.02";

   casing = BulletShell;
   shellExitDir        = "1.0 0.3 1.0";
   shellExitOffset     = "0.15 -0.56 -0.1";
   shellExitVariance   = 15.0;
   shellVelocity       = 3.0;

   // Weapon lights up while firing
   lightType = "WeaponFireLight";
   lightColor = "0.992126 0.968504 0.708661 1";
   lightRadius = "3.5";
   lightDuration = "150";
   lightBrightness = 1;

   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "4 4 4";
   camShakeAmp = "6 6 6";
   camShakeDuration = "0.8";
   camShakeRadius = "1.2";

   useRemainderDT = true;

   stateName[0]                     = "Preactivate";
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNoAmmo[0]       = "NoAmmo";

   stateName[1]                     = "Activate";
   stateTransitionGeneric0In[1]     = "SprintEnter";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 0.5;
   stateWaitForTimeout[1]           = true;
   stateSequence[1]                 = "idle";
   //stateSound[1]                    = ShotgunSwitchinSound;

   stateName[2]                     = "Ready";
   stateTransitionGeneric0In[2]     = "SprintEnter";
   stateTransitionOnMotion[2]       = "ReadyMotion";
   //stateTransitionOnTimeout[2]      = "ReadyFidget";
   //stateTimeoutValue[2]             = 10;
   stateWaitForTimeout[2]           = false;
   stateScaleAnimation[2]           = false;
   stateScaleAnimationFP[2]         = false;
   stateTransitionOnNoAmmo[2]       = "NoAmmo";
   stateTransitionOnTriggerDown[2]  = "Fire";
   stateSequence[2]                 = "idle";

   stateName[3]                     = "ReadyFidget";
   stateTransitionGeneric0In[3]     = "SprintEnter";
   stateTransitionOnMotion[3]       = "ReadyMotion";
   stateTransitionOnTimeout[3]      = "Ready";
   stateTimeoutValue[3]             = 6;
   stateWaitForTimeout[3]           = false;
   stateTransitionOnNoAmmo[3]       = "NoAmmo";
   stateTransitionOnTriggerDown[3]  = "Fire";
   stateSequence[3]                 = "idle_fidget1";
   //stateSound[3]                    = ShotgunCockSound;

   stateName[4]                     = "ReadyMotion";
   stateTransitionGeneric0In[4]     = "SprintEnter";
   stateTransitionOnNoMotion[4]     = "Ready";
   stateWaitForTimeout[4]           = false;
   stateScaleAnimation[4]           = false;
   stateScaleAnimationFP[4]         = false;
   stateSequenceTransitionIn[4]     = true;
   stateSequenceTransitionOut[4]    = true;
   stateTransitionOnNoAmmo[4]       = "NoAmmo";
   stateTransitionOnTriggerDown[4]  = "Fire";
   stateSequence[4]                 = "run";

   stateName[5]                     = "Fire";
   stateTransitionGeneric0In[5]     = "SprintEnter";
   stateTransitionOnTimeout[5]      = "WaitForRelease";
   stateTimeoutValue[5]             = 0.20;
   stateWaitForTimeout[5]           = true;
   stateFire[5]                     = true;
   stateRecoil[5]                   = "light_recoil";
   stateAllowImageChange[5]         = false;
   stateSequence[5]                 = "fire";
   stateScaleAnimation[5]           = false;
   stateSequenceNeverTransition[5]  = true;
   stateSequenceRandomFlash[5]      = true;
   stateScript[5]                   = "onFire";
   stateSound[5]                    = ShotgunFireSound;
   stateEmitter[5]                  = GunFireSmokeEmitter;
   stateEmitterTime[5]              = 0.025;

   stateName[6]                     = "WaitForRelease";
   //stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnTriggerUp[6]    = "NewRound";
   stateTimeoutValue[6]             = 0.05;
   stateWaitForTimeout[6]           = true;
   stateAllowImageChange[6]         = false;

   stateName[7]                     = "NewRound";
   //stateTransitionGeneric0In[7]     = "SprintEnter";
   stateTransitionOnNoAmmo[7]       = "NoAmmo";
   stateTransitionOnTimeout[7]      = "Ready";
   stateWaitForTimeout[7]           = true;
   stateTimeoutValue[7]             = 0.60;
   stateSequence[7]                 = "fire_alt";
   stateScaleAnimation[7]           = true;
   stateAllowImageChange[7]         = false;
   stateEjectShell[7]               = true;
   stateSound[7]                    = ShotgunCockSound;

   stateName[8]                     = "NoAmmo";
   stateTransitionGeneric0In[8]     = "SprintEnter";
   stateTransitionOnMotion[8]       = "NoAmmoMotion";
   stateTimeoutValue[8]             = 0.1;
   stateScript[8]                   = "onClipEmpty";
   stateSequence[8]                 = "idle";
   stateScaleAnimation[8]           = false;
   stateScaleAnimationFP[8]         = false;
   stateTransitionOnTriggerDown[8]  = "DryFire";
   
   stateName[9]                     = "NoAmmoMotion";
   stateTransitionGeneric0In[9]     = "SprintEnter";
   stateTransitionOnNoMotion[9]     = "NoAmmo";
   stateWaitForTimeout[9]           = false;
   stateScaleAnimation[9]           = false;
   stateScaleAnimationFP[9]         = false;
   stateSequenceTransitionIn[9]     = true;
   stateSequenceTransitionOut[9]    = true;
   stateTransitionOnTriggerDown[9]  = "DryFire";
   stateSequence[9]                 = "run";

   stateName[10]                     = "DryFire";
   stateTransitionGeneric0In[10]     = "SprintEnter";
   stateWaitForTimeout[10]           = true;
   stateTimeoutValue[10]             = 0.6;
   stateTransitionOnTimeout[10]      = "NoAmmo";
   stateScript[10]                   = "onDryFire";
   stateSound[10]                    = MachineGunDryFire;

   stateName[11]                     = "ReloadClip";
   stateTransitionOnTimeout[11]      = "ReloadFinish";
   stateWaitForTimeout[11]           = true;
   stateScaleAnimation[11]           = false;
   stateScaleAnimationFP[11]         = false;
   stateTimeoutValue[11]             = 3.0;
   stateReload[11]                   = true;
   stateSequence[11]                 = "reload";
   stateShapeSequence[11]            = "Reload";
   stateScaleShapeSequence[11]       = true;
   stateSound[11]                    = ShotgunReloadSound;
   stateAllowImageChange[11]         = false; 

   stateName[12]                    = "SprintEnter";
   stateTransitionGeneric0Out[12]   = "SprintExit";
   stateTransitionOnTimeout[12]     = "Sprinting";
   stateWaitForTimeout[12]          = false;
   stateTimeoutValue[12]            = 0.5;
   stateWaitForTimeout[12]          = false;
   stateScaleAnimation[12]          = false;
   stateScaleAnimationFP[12]        = false;
   stateSequenceTransitionIn[12]    = true;
   stateSequenceTransitionOut[12]   = true;
   stateAllowImageChange[12]        = false;
   stateSequence[12]                = "sprint";

   stateName[13]                    = "Sprinting";
   stateTransitionGeneric0Out[13]   = "SprintExit";
   stateWaitForTimeout[13]          = false;
   stateScaleAnimation[13]          = false;
   stateScaleAnimationFP[13]        = false;
   stateSequenceTransitionIn[13]    = true;
   stateSequenceTransitionOut[13]   = true;
   stateAllowImageChange[13]        = false;
   stateSequence[13]                = "sprint";
   
   stateName[14]                    = "SprintExit";
   stateTransitionGeneric0In[14]    = "SprintEnter";
   stateTransitionOnTimeout[14]     = "Ready";
   stateWaitForTimeout[14]          = false;
   stateTimeoutValue[14]            = 0.25;
   stateSequenceTransitionIn[14]    = true;
   stateSequenceTransitionOut[14]   = true;
   stateAllowImageChange[14]        = false;
   stateSequence[14]                = "sprint";
   
   stateName[15]                     = "ReloadFinish";
   stateTimeoutValue[15]             = 0.1;
   stateTransitionOnAmmo[15]         = "Ready";
   stateScript[15]                   = "onReloadFinish"; 
};

datablock ShapeBaseImageData( ShotgunIronSightImage : ShotgunWeaponImage )
{
   firstPerson = false;
   useEyeNode = false;
   animateOnServer = false;
   useEyeOffset = false;
   //eyeOffset = "-0.147 -0.225 0.025";
   //eyeOffset = "-0.16 -0.29 0.065";
   eyeOffset = "-0.16 0 0.05";
   eyeRotation = "0.574892 0.0910342 0.813149 4.72198";

   projectileSpread = 0.016;
   parentImage = "ShotgunWeaponImage";

   // Called when the weapon is first mounted and there is ammo.
   // We want a smooth transition from datablocks, change Activate params
   stateTimeoutValue[1]             = 0.6;
   stateWaitForTimeout[1]           = true;
   stateSequence[1]     = "idle";
   stateSound[1]                    = "";
   stateTransitionOnTimeout[1]      = "Ready";
   stateAllowImageChange[1]         = false; 
};

function ShotgunWeaponImage::onFire(%data, %obj, %slot)
{
   if( %obj.getInventory( %data.ammo ) <= 0)
      return;

   %obj.decInventory( %data.ammo, 1 );

   %data.lightStart = $Sim::Time;
/*
   if( %obj.inStation $= "" && %obj.isCloaked() )
   {
      if( %obj.respawnCloakThread !$= "" )
      {
         cancel(%obj.respawnCloakThread);
         %obj.setCloaked( false );
         %obj.respawnCloakThread = "";
      }
      else
      {
         if( %obj.getEnergyLevel() > 20 )
         {   
            %obj.setCloaked( false );
            %obj.reCloak = %obj.schedule( 1000, "setCloaked", true ); 
         }
      }   
   }
*/
   if (isObject(%obj.lastProjectile) && %obj.deleteLastProjectile)
      %obj.lastProjectile.delete();

   %vec = %obj.getMuzzleVector(%slot);

   for(%i = 0; %i < %data.projectileNum; %i++)
   {
      %x = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %y = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %z = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
      %muzzleVector = MatrixMulVector(%mat, %vec);

      if ( %obj.weaponUnderwater == true )
      {
      // Determin initial projectile velocity based on the gun's muzzle point and the object's current velocity
      %objectVelocity = %obj.getVelocity();
      %muzzleVelocity = VectorAdd(VectorScale(%muzzleVector, %data.underWaterProjectile.muzzleVelocity), VectorScale(%objectVelocity, %data.underWaterProjectile.velInheritFactor));
   
      // Create the projectile object
      %p = new (%data.projectileType)() {
         dataBlock        = %data.underWaterProjectile;
         initialVelocity  = %muzzleVelocity;
         initialPosition  = %obj.getMuzzlePoint(%slot);
         // This parameter is deleted about 7 ticks into the projectiles flight
         sourceObject     = %obj;
         sourceSlot       = %slot;
         // We use this for the source object when applying damage because it isn't deleted
         origin           = %obj;
         client           = %obj.client;
         };

      %obj.lastProjectile = %p;
      %obj.deleteLastProjectile = %data.deleteLastProjectile;
      if(%obj.client)
         %obj.client.projectile = %p;

      MissionCleanup.add(%p);
      }
      else 
      {
      %objectVelocity = %obj.getVelocity();
      %muzzleVelocity = VectorAdd(VectorScale(%muzzleVector, %data.projectile.muzzleVelocity), VectorScale(%objectVelocity, %data.projectile.velInheritFactor));

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
         };
 
      %obj.lastProjectile = %p;
      if(%obj.client)
         %obj.client.projectile = %p;
   
      MissionCleanup.add(%p);
      }
   }
   return %p;
}

function ShotgunIronSightImage::onFire(%data, %obj, %slot)
{
   ShotgunWeaponImage::onFire(%data, %obj, %slot);
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowWeapon("Soldier");
SmsInv.AddWeapon(Shotgun, "Shotgun", 1);

SmsInv.AllowClip("armor\tSoldier\t4");
SmsInv.AddClip(ShotgunClip, "Shotgun Clip", 4);

SmsInv.AllowAmmo("armor\tSoldier\t8");
SmsInv.AddAmmo(ShotgunAmmo, 8);
