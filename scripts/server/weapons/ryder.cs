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

datablock SFXProfile(RyderFireSound)
{
   filename = "art/sound/weapons/V_22P_01.wav";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(RyderReloadSound)
{
   filename = "art/sound/weapons/wpn_ryder_reload";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(RyderSwitchinSound)
{
   filename = "art/sound/weapons/wpn_ryder_switchin";
   description = AudioClose3D;
   preload = true;
};

// ----------------------------------------------------------------------------
// Particles
// ----------------------------------------------------------------------------
datablock ParticleData(RyderFireSmoke)
{
   textureName          = "art/particles/smoke";
   dragCoefficient      = 0;
   gravityCoefficient   = "-1";
   windCoefficient      = 0;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 200;
   spinRandomMin = -180.0;
   spinRandomMax =  180.0;
   useInvAlpha   = true;

   colors[0]     = "0.795276 0.795276 0.795276 0.692913";
   colors[1]     = "0.866142 0.866142 0.866142 0.346457";
   colors[2]     = "0.897638 0.834646 0.795276 0";

   sizes[0]      = "0.399805";
   sizes[1]      = "1.19941";
   sizes[2]      = "1.69993";

   times[0]      = 0.0;
   times[1]      = "0.498039";
   times[2]      = 1.0;
   animTexName = "art/particles/smoke";
};

datablock ParticleEmitterData(RyderFireSmokeEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = "0";
   velocityVariance = "0";
   thetaMin         = "0";
   thetaMax         = "0";
   lifetimeMS       = 250;
   particles = "RyderFireSmoke";
   blendStyle = "NORMAL";
   softParticles = "0";
   originalName = "RyderFireSmokeEmitter";
   alignParticles = "0";
   orientParticles = "0";
};

//-----------------------------------------------------------------------------
// Explosion
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Shell ejected during reload.
//-----------------------------------------------------------------------------

datablock DebrisData(RyderBulletShell)
{
   shapeFile = "art/shapes/weapons/shared/RifleShell.dts";
   lifetime = 6.0;
   minSpinSpeed = 300.0;
   maxSpinSpeed = 400.0;
   elasticity = 0.65;
   friction = 0.05;
   numBounces = 5;
   staticOnMaxBounce = true;
   snapOnMaxBounce = false;
   ignoreWater = true;
   fade = true;
};

//-----------------------------------------------------------------------------
// Projectile Object
//-----------------------------------------------------------------------------

datablock ProjectileData( RyderProjectile )
{
   projectileShapeName = "";

   directDamage        = 25;
   radiusDamage        = 0;
   damageRadius        = 0;
   areaImpulse         = 0.5;
   impactForce         = 1;
   damageType          = $DamageType::Pistol;
	
   explosion           = BulletExplosion;
   waterExplosion      = BulletWaterExplosion;
   playerExplosion     = BloodSpillEmitter;
   decal               = BulletHoleDecal;

   //particleEmitter     = "BulletTrailEmitter";
   particleWaterEmitter = "UWBulletTrailEmitter";

   Splash              = BulletSplash;
   muzzleVelocity      = 300;
   velInheritFactor    = 0;

   armingDelay         = 0; // How long it should not detonate on impact
   lifetime            = 335; // How long the projectile should exist before deleting itself
   fadeDelay           = 0; // Brief Amount of time, in milliseconds, before the projectile begins to fade out.

   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod          = 1;

   lightDesc           = "";
};

//-----------------------------------------------------------------------------
// Ammo Item
//-----------------------------------------------------------------------------
datablock ItemData(RyderClip : DefaultClip)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Ryder/TP_Ryder.dts";

   // Dynamic properties defined by the scripts
   pickUpName = 'Ryder magazine';
};

datablock ItemData(RyderAmmo : DefaultAmmo)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Ryder/TP_Ryder.dts";

   // Dynamic properties defined by the scripts
   pickUpName = 'Ryder bullet';
   clip = RyderClip;
};

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the SoldierWeaponImage is used.
//-----------------------------------------------------------------------------
datablock ItemData(Ryder : DefaultWeapon)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Ryder/TP_Ryder.dts";

   // Dynamic properties defined by the scripts
   pickUpName = 'Ryder pistol';
   image = RyderWeaponImage;
   reticle = "crossHair";
};

datablock ShapeBaseImageData(RyderWeaponImage)
{
   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";

   // Basic Item properties
   shapeFile = "art/shapes/weapons/Ryder/TP_Ryder.dts";
   shapeFileFP = "art/shapes/weapons/Ryder/FP_Ryder.dts";
   emap = true;
   computeCRC = false;

   imageAnimPrefix = "Pistol";
   imageAnimPrefixFP = "Pistol";

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   firstPerson = true;
   useEyeNode = true;
   animateOnServer = true;
   cloakable = true;

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.
   correctMuzzleVector = true;
   correctMuzzleVectorTP = true;

   // Projectiles and Ammo.
   item = Ryder;
   ammo = RyderAmmo;
   clip = RyderClip;
   ironSight = RyderIronSightImage;

   usesEnergy = 0;
   minEnergy = 0;

   projectile = RyderProjectile;
   projectileType = Projectile;
   projectileSpread = "0.002";

   //altProjectile = GrenadeLauncherProjectile;
   //altProjectileSpread = "0.02";

   casing = RyderBulletShell;
   shellExitDir        = "1.0 0.3 1.0";
   shellExitOffset     = "0.15 -0.56 -0.1";
   shellExitVariance   = 15.0;
   shellVelocity       = 3.0;

   // Weapon lights up while firing
   lightType = "WeaponFireLight";
   lightColor = "0.992126 0.968504 0.700787 1";
   lightRadius = "3.5";
   lightDuration = "100";
   lightBrightness = 1;

   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "2 2 2";
   camShakeAmp = "5 5 5";
   camShakeDuration = "0.5";
   camShakeRadius = "1.2";

   useRemainderDT = true;

   // Initial start up state
   stateName[0]                     = "Preactivate";
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNoAmmo[0]       = "NoAmmo";

   // Activating the gun.  Called when the weapon is first
   // mounted and there is ammo.
   stateName[1]                     = "Activate";
   stateTransitionGeneric0In[1]     = "SprintEnter";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 1.0;
   stateSequence[1]                 = "switch_in";
   stateSound[1]                    = RyderSwitchinSound;

   // Ready to fire, just waiting for the trigger
   stateName[2]                     = "Ready";
   stateTransitionGeneric0In[2]     = "SprintEnter";
   stateTransitionOnMotion[2]       = "ReadyMotion";
   stateScaleAnimation[2]           = false;
   stateScaleAnimationFP[2]         = false;
   stateTransitionOnNoAmmo[2]       = "NoAmmo";
   stateTransitionOnTriggerDown[2]  = "Fire";
   stateSequence[2]                 = "idle";

   // Ready to fire with player moving
   stateName[3]                     = "ReadyMotion";
   stateTransitionGeneric0In[3]     = "SprintEnter";
   stateTransitionOnNoMotion[3]     = "Ready";
   stateWaitForTimeout[3]           = false;
   stateScaleAnimation[3]           = false;
   stateScaleAnimationFP[3]         = false;
   stateSequenceTransitionIn[3]     = true;
   stateSequenceTransitionOut[3]    = true;
   stateTransitionOnNoAmmo[3]       = "NoAmmo";
   stateTransitionOnTriggerDown[3]  = "Fire";
   stateSequence[3]                 = "run";

   // Fire the weapon. Calls the fire script which does
   // the actual work.
   stateName[4]                     = "Fire";
   stateTransitionGeneric0In[4]     = "SprintEnter";
   stateTransitionOnTimeout[4]      = "WaitForRelease";
   stateTimeoutValue[4]             = 0.23;
   stateWaitForTimeout[4]           = true;
   stateFire[4]                     = true;
   //stateRecoil[4]                   = "";
   stateAllowImageChange[4]         = false;
   stateSequence[4]                 = "fire";
   stateScaleAnimation[4]           = true;
   stateSequenceNeverTransition[4]  = true;
   stateSequenceRandomFlash[4]      = true;        // use muzzle flash sequence
   stateScript[4]                   = "onFire";
   stateEmitter[4]                  = RyderFireSmokeEmitter;
   stateEmitterTime[4]              = 0.025;
   stateEjectShell[4]               = true;
   stateSound[4]                    = RyderFireSound;

   // Wait for the player to release the trigger
   stateName[5]                     = "WaitForRelease";
   stateTransitionGeneric0In[5]     = "SprintEnter";
   stateTransitionOnTriggerUp[5]    = "NewRound";
   stateTimeoutValue[5]             = 0.05;
   stateWaitForTimeout[5]           = true;
   stateAllowImageChange[5]         = false;

   // Put another round in the chamber
   stateName[6]                     = "NewRound";
   stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnNoAmmo[6]       = "NoAmmo";
   stateTransitionOnTimeout[6]      = "Ready";
   stateWaitForTimeout[6]           = false;
   stateTimeoutValue[6]             = 0.05;
   stateAllowImageChange[6]         = false;

   // No ammo in the weapon, just idle until something
   // shows up. Play the dry fire sound if the trigger is
   // pulled.
   stateName[7]                     = "NoAmmo";
   stateTransitionGeneric0In[7]     = "SprintEnter";
   stateTransitionOnMotion[7]       = "NoAmmoMotion";
   stateTransitionOnAmmo[7]         = "ReloadClip";
   stateTimeoutValue[7]             = 0.1;   // Slight pause to allow script to run when trigger is still held down from Fire state
   stateScript[7]                   = "onClipEmpty";
   stateTransitionOnTriggerDown[7]  = "DryFire";
   stateSequence[7]                 = "idle";
   stateScaleAnimation[7]           = false;
   stateScaleAnimationFP[7]         = false;
   
   stateName[8]                     = "NoAmmoMotion";
   stateTransitionGeneric0In[8]     = "SprintEnter";
   stateTransitionOnNoMotion[8]     = "NoAmmo";
   stateWaitForTimeout[8]           = false;
   stateScaleAnimation[8]           = false;
   stateScaleAnimationFP[8]         = false;
   stateSequenceTransitionIn[8]     = true;
   stateSequenceTransitionOut[8]    = true;
   stateTransitionOnAmmo[8]         = "ReloadClip";
   stateTransitionOnTriggerDown[8]  = "DryFire";
   stateSequence[8]                 = "run";

   // No ammo dry fire
   stateName[9]                     = "DryFire";
   stateTransitionGeneric0In[9]     = "SprintEnter";
   stateTransitionOnAmmo[9]         = "ReloadClip";
   stateWaitForTimeout[9]           = "0";
   stateTimeoutValue[9]             = 0.7;
   stateTransitionOnTimeout[9]      = "NoAmmo";
   stateScript[9]                   = "onDryFire";

   // Play the reload clip animation
   stateName[10]                     = "ReloadClip";
   stateTransitionGeneric0In[10]     = "SprintEnter";
   stateTransitionOnTimeout[10]      = "Ready";
   stateWaitForTimeout[10]           = true;
   stateTimeoutValue[10]             = 2.0;
   stateReload[10]                   = true;
   stateSequence[10]                 = "reload";
   stateShapeSequence[10]            = "Reload";
   stateScaleShapeSequence[10]       = true;
   stateScaleAnimation[10]           = true;
   stateScaleAnimationFP[10]         = false;
   stateSound[10]                    = RyderReloadSound;

   // Start Sprinting
   stateName[11]                    = "SprintEnter";
   stateTransitionGeneric0Out[11]   = "SprintExit";
   stateTransitionOnTimeout[11]     = "Sprinting";
   stateWaitForTimeout[11]          = false;
   stateTimeoutValue[11]            = 0.5;
   stateWaitForTimeout[11]          = false;
   stateScaleAnimation[11]          = false;
   stateScaleAnimationFP[11]        = false;
   stateSequenceTransitionIn[11]    = true;
   stateSequenceTransitionOut[11]   = true;
   stateAllowImageChange[11]        = false;
   stateSequence[11]                = "sprint";

   // Sprinting
   stateName[12]                    = "Sprinting";
   stateTransitionGeneric0Out[12]   = "SprintExit";
   stateWaitForTimeout[12]          = false;
   stateScaleAnimation[12]          = false;
   stateScaleAnimationFP[12]        = false;
   stateSequenceTransitionIn[12]    = true;
   stateSequenceTransitionOut[12]   = true;
   stateAllowImageChange[12]        = false;
   stateSequence[12]                = "sprint";
   
   // Stop Sprinting
   stateName[13]                    = "SprintExit";
   stateTransitionGeneric0In[13]    = "SprintEnter";
   stateTransitionOnTimeout[13]     = "Ready";
   stateWaitForTimeout[13]          = false;
   stateTimeoutValue[13]            = 0.2;
   stateSequenceTransitionIn[13]    = true;
   stateSequenceTransitionOut[13]   = true;
   stateAllowImageChange[13]        = false;
   stateSequence[13]                = "sprint";
};

datablock ShapeBaseImageData( RyderIronSightImage: RyderWeaponImage )
{
   firstPerson = false;
   useEyeNode = false;
   animateOnServer = false;
   useEyeOffset = false;
   //eyeOffset = "-0.1815 -0.25 0.0495"; // L/R - F/B - T/B
   eyeOffset = "-0.19488 -0.3 0.049";
   eyeRotation = "0.405853 0 0.913938 3.80559";

   parentImage = "RyderWeaponImage";

   // Called when the weapon is first mounted and there is ammo.
   // We want a smooth transition from datablocks, change Activate params
   stateTimeoutValue[1] = 0.1;
   stateSequence[1]     = "idle";
   stateSound[1]        = "";
};

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowWeapon("Soldier");
SmsInv.AddWeapon(Ryder, "Ryder pistol", 0);

SmsInv.AllowClip("armor\tSoldier\t3");
SmsInv.AddClip(RyderClip, "Ryder clips", 3);

SmsInv.AllowAmmo("armor\tSoldier\t15");
SmsInv.AddAmmo(RyderAmmo, 15);
