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
datablock SFXProfile(LurkerFireSound)
{
   filename = "art/sound/weapons/D_32P_01_loop.wav";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(LurkerFireStopSound)
{
   filename = "art/sound/weapons/D_32P_01_end.wav";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(LurkerReloadSound)
{
   filename = "art/sound/weapons/wpn_lurker_reload";
   description = AudioClosest3D;
   preload = true;
};
/* unused
datablock SFXProfile(LurkerIdleSound)
{
   filename = "art/sound/weapons/wpn_lurker_idle";
   description = AudioClose3D;
   preload = true;
};
*/
datablock SFXProfile(LurkerSwitchinSound)
{
   filename = "art/sound/weapons/wpn_lurker_switchin";
   description = AudioClosest3D;
   preload = true;
};

datablock SFXProfile(LurkerGrenadeFireSound)
{
   filename = "art/sound/weapons/wpn_lurker_grenadelaunch";
   description = AudioClose3D;
   preload = true;
};
/*
datablock SFXPlayList(LurkerFireSoundList)
{
   // Use a looped description so the list playback will loop.
   description = AudioClose3D;

   track[ 0 ] = LurkerFireSound;
};
*/
datablock SFXProfile(MachineGunDryFire)
{
   filename = "art/sound/weapons/dry_fire_01";
   description = AudioClosest3D;
   preload = true;
};

// ----------------------------------------------------------------------------
// Particles
// ----------------------------------------------------------------------------
datablock ParticleData(GunFireSmoke)
{
   textureName          = "art/particles/smoke";
   dragCoefficient      = 0;
   gravityCoefficient   = "-0.803419";
   windCoefficient      = 0;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = "500";
   lifetimeVarianceMS   = "200";
   spinRandomMin = -180.0;
   spinRandomMax =  180.0;
   useInvAlpha   = true;

   colors[0]     = "0.992126 0.992126 0.992126 0.353";
   colors[1]     = "0.905512 0.905512 0.905512 0.216";
   colors[2]     = "0.84252 0.84252 0.84252 0.34";

   sizes[0]      = "0.399805";
   sizes[1]      = "1.09565";
   sizes[2]      = "1.28487";

   times[0]      = 0.0;
   times[1]      = "0.494118";
   times[2]      = 1.0;
   animTexName = "art/particles/smoke";
};

datablock ParticleEmitterData(GunFireSmokeEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = "0";
   velocityVariance = "0";
   thetaMin         = "0";
   thetaMax         = "0";
   lifetimeMS       = "250";
   particles = "GunFireSmoke";
   blendStyle = "NORMAL";
   softParticles = "0";
   originalName = "GunFireSmokeEmitter";
   alignParticles = "0";
   orientParticles = "0";
   lifetimeVarianceMS = "0";
   ambientFactor = "0.5";
};

datablock ParticleData(BulletDirtDust)
{
   textureName          = "art/particles/impact";
   dragCoefficient      = "0.99218";
   gravityCoefficient   = "-0.100122";
   windCoefficient      = 0;
   inheritedVelFactor   = 0.0;
   constantAcceleration = "-0.83";
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 300;
   spinRandomMin = -180.0;
   spinRandomMax =  180.0;
   useInvAlpha   = true;

   colors[0]     = "0.780392 0.694118 0.466667 0.502";
   colors[1]     = "0.792157 0.717647 0.509804 0.349";
   colors[2]     = "0.882353 0.882353 0.882353 0.241";

   sizes[0]      = "0.5";
   sizes[1]      = "1.4985";
   sizes[2]      = "1.5";

   times[0]      = 0.0;
   times[1]      = "0.494118";
   times[2]      = 1.0;
   animTexName = "art/particles/impact";
};

datablock ParticleEmitterData(BulletDirtDustEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = "1";
   velocityVariance = 1.0;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = "250";
   particles = "BulletDirtDust";
   blendStyle = "NORMAL";
   ambientFactor = "0.5";
   softParticles = "0";
   softnessDistance = "1";
};

//--------------------------------------------------------------------------
// Shell ejected during reload.
//-----------------------------------------------------------------------------
datablock DebrisData(BulletShell)
{
   shapeFile = "art/shapes/weapons/shared/RifleShell.dts";
   lifetime = 30.0;
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

datablock ProjectileData( LurkerProjectile )
{
   projectileShapeName = "";

   directDamage        = 20;
   radiusDamage        = 0;
   damageRadius        = 0;
   areaImpulse         = 0.5;
   impactForce         = 1;
   damageType          = $DamageType::Rifle;

   explosion           = BulletExplosion;
   waterExplosion      = BulletWaterExplosion;
   playerExplosion     = BloodSpillEmitter;
   decal               = BulletHoleDecal;

   //particleEmitter     = "BulletTrailEmitter";
   particleWaterEmitter = "UWBulletTrailEmitter";

   Splash              = BulletSplash;
   muzzleVelocity      = 600;
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

//-----------------------------------------------------------------------------
// Ammo Item
//-----------------------------------------------------------------------------
datablock ItemData(LurkerClip : DefaultClip)
{
   // Dynamic properties defined by the scripts
   pickUpName = 'Lurker magazine';
};

datablock ItemData(LurkerAmmo : DefaultAmmo)
{
   pickUpName = 'Lurker ammo';
   clip = LurkerClip;
};

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the LurkerWeaponImage is used.
//-----------------------------------------------------------------------------

datablock ItemData(Lurker : DefaultWeapon)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Lurker_dark/TP_Lurker_dark.dts";

   // Dynamic properties defined by the scripts
   pickUpName = 'Lurker rifle';
   image = LurkerWeaponImage;
   reticle = "crossHair";
};

datablock ShapeBaseImageData(LurkerWeaponImage)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Lurker_dark/TP_Lurker_dark.dts";
   shapeFileFP = "art/shapes/weapons/Lurker_dark/FP_Lurker_dark.dts";
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
   eyeOffset = "0.025 -0.13 -0.095";

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
   item = Lurker;
   ammo = LurkerAmmo;
   clip = LurkerClip;
   ironSight = LurkerIronSightImage;

   projectile = LurkerProjectile;
   projectileType = Projectile;
   projectileSpread = "0.02";
   projectileNum = 1;

   casing = BulletShell;
   shellExitDir        = "1.0 0.3 1.0";
   shellExitOffset     = "0.15 -0.56 -0.1";
   shellExitVariance   = 15.0;
   shellVelocity       = 3.0;

   lightType = "WeaponFireLight";
   lightColor = "0.992126 0.968504 0.708661 1";
   lightRadius = "3.5";
   lightDuration = "100";
   lightBrightness = 1;

   shakeCamera = true;
   camShakeFreq = "4 4 4";
   camShakeAmp = "3 3 3";
   camShakeDuration = "0.5";
   camShakeRadius = "1.2";

   useRemainderDT = true;

   stateName[0]                     = "Preactivate";
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNoAmmo[0]       = "NoAmmo";

   stateName[1]                     = "Activate";
   stateTransitionGeneric0In[1]     = "SprintEnter";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 0.5;
   stateSequence[1]                 = "switch_in";
   stateSound[1]                    = LurkerSwitchinSound;

   stateName[2]                     = "Ready";
   stateTransitionGeneric0In[2]     = "SprintEnter";
   stateTransitionOnMotion[2]       = "ReadyMotion";
   stateTransitionOnTimeout[2]      = "ReadyFidget";
   stateTimeoutValue[2]             = 10;
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
   stateTransitionGeneric0In[5]     = "FireStop";
   stateTransitionOnTimeout[5]      = "NewRound";
   stateTransitionOnTriggerUp[5]    = "FireStop";
   stateTimeoutValue[5]             = 0.11;
   stateFire[5]                     = true;
   stateRecoil[5]                   = "";
   stateAllowImageChange[5]         = false;
   stateSequence[5]                 = "fire";
   stateScaleAnimation[5]           = false;
   stateSequenceNeverTransition[5]  = true;
   stateSequenceRandomFlash[5]      = true;
   stateScript[5]                   = "onFire";
   stateSound[5]                    = LurkerFireSound;
   stateEmitter[5]                  = GunFireSmokeEmitter;
   stateEmitterTime[5]              = 0.025;
   stateTransitionOnNoAmmo[5]       = "FireStop";
   stateEjectShell[5]               = true;

   stateName[6]                     = "NewRound";
   stateTransitionOnTimeout[6]      = "Fire";
   stateWaitForTimeout[6]           = true;
   stateTimeoutValue[6]             = 0.02;

   stateName[7]                     = "NoAmmo";
   stateTransitionGeneric0In[7]     = "SprintEnter";
   stateTransitionOnMotion[7]       = "NoAmmoMotion";
   stateTimeoutValue[7]             = 0.1;
   stateSequence[7]                 = "idle";
   stateScaleAnimation[7]           = false;
   stateScaleAnimationFP[7]         = false;
   stateTransitionOnTriggerDown[7]  = "DryFire";
   
   stateName[8]                     = "NoAmmoMotion";
   stateTransitionGeneric0In[8]     = "SprintEnter";
   stateTransitionOnNoMotion[8]     = "NoAmmo";
   stateWaitForTimeout[8]           = false;
   stateScaleAnimation[8]           = false;
   stateScaleAnimationFP[8]         = false;
   stateSequenceTransitionIn[8]     = true;
   stateSequenceTransitionOut[8]    = true;
   stateTransitionOnTriggerDown[8]  = "DryFire";
   stateSequence[8]                 = "run";

   stateName[9]                     = "DryFire";
   stateTransitionGeneric0In[9]     = "SprintEnter";
   stateTransitionOnMotion[9]       = "NoAmmoMotion";
   stateWaitForTimeout[9]           = true;
   stateTimeoutValue[9]             = 0.25;
   stateSequence[9]                 = "idle";
   stateScript[9]                   = "onDryFire";
   stateTransitionOnTimeout[9]      = "NoAmmo";
   stateSound[9]                    = MachineGunDryFire;

   stateName[10]                     = "ReloadClip";
   stateTransitionOnTimeout[10]      = "ReloadFinish";
   stateWaitForTimeout[10]           = true;
   stateScaleAnimation[10]           = false;
   stateScaleAnimationFP[10]         = false;
   stateTimeoutValue[10]             = 3.0;
   stateReload[10]                   = true;
   stateSequence[10]                 = "reload";
   stateShapeSequence[10]            = "Reload";
   stateScaleShapeSequence[10]       = true;
   stateSound[10]                    = LurkerReloadSound;
   stateAllowImageChange[10]         = false;  

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

   stateName[12]                    = "Sprinting";
   stateTransitionGeneric0Out[12]   = "SprintExit";
   stateWaitForTimeout[12]          = false;
   stateScaleAnimation[12]          = false;
   stateScaleAnimationFP[12]        = false;
   stateSequenceTransitionIn[12]    = true;
   stateSequenceTransitionOut[12]   = true;
   stateAllowImageChange[12]        = false;
   stateSequence[12]                = "sprint";
   
   stateName[13]                    = "SprintExit";
   stateTransitionGeneric0In[13]    = "SprintEnter";
   stateTransitionOnTimeout[13]     = "Ready";
   stateWaitForTimeout[13]          = false;
   stateTimeoutValue[13]            = 0.3;
   stateSequenceTransitionIn[13]    = true;
   stateSequenceTransitionOut[13]   = true;
   stateAllowImageChange[13]        = false;
   stateSequence[13]                = "sprint";

   stateName[14]                     = "FireStop";  
   stateScript[14]                   = "onFireStop";  
   stateSound[14]                    = LurkerFireStopSound;  
   stateTimeoutValue[14]             = 0.1;  
   stateWaitForTimeout[14]           = true;  
   stateTransitionOnTimeout[14]      = "Ready";    
   stateTransitionOnTriggerDown[14]  = "Fire";  
   stateTransitionOnNoAmmo[14]       = "NoAmmo"; 

   stateName[15]                     = "ReloadFinish";
   stateTimeoutValue[15]             = 0.1;
   stateTransitionOnAmmo[15]         = "Ready";
   stateScript[15]                   = "onReloadFinish";    
};

datablock ShapeBaseImageData( LurkerIronSightImage : LurkerWeaponImage )
{
   firstPerson = false;
   useEyeNode = false;
   animateOnServer = false;
   useEyeOffset = false;
   //eyeOffset = "-0.147 -0.225 0.025";
   eyeOffset = "-0.161 -0.30 0.060";
   eyeRotation = "0.574892 0.0910342 0.813149 4.72198";

   projectileSpread = "0.007";
   parentImage = "LurkerWeaponImage";

   // Called when the weapon is first mounted and there is ammo.
   // We want a smooth transition from datablocks, change Activate params
   stateTimeoutValue[1]             = 0.5;
   stateWaitForTimeout[1]           = true;
   stateSequence[1]                 = "idle";
   stateSound[1]                    = "";
   stateTransitionOnTimeout[1]      = "Ready";
   stateAllowImageChange[1]         = false; 
}; 

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowWeapon("Soldier");
SmsInv.AddWeapon(Lurker, "Lurker rifle", 1);

SmsInv.AllowClip("armor\tSoldier\t4");
SmsInv.AddClip(LurkerClip, "Lurker Clips", 4);

SmsInv.AllowAmmo("armor\tSoldier\t35");
SmsInv.AddAmmo(LurkerAmmo, 35);
