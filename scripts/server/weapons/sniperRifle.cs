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

datablock SFXProfile(SniperFireSound)
{
   filename = "art/sound/weapons/sniper_fire";
   description = BulletFireDesc;
   preload = true;
};

datablock SFXProfile(SniperReloadSound)
{
   filename = "art/sound/weapons/sniper_reload";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(SniperSwitchinSound)
{
   filename = "art/sound/weapons/wpn_ryder_switchin";
   description = AudioClose3D;
   preload = true;
};

// ----------------------------------------------------------------------------
// Particles
// ----------------------------------------------------------------------------
datablock ParticleData(SniperFireSmoke)
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

datablock ParticleEmitterData(SniperFireSmokeEmitter)
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
   originalName = "SniperFireSmokeEmitter";
   alignParticles = "0";
   orientParticles = "0";
};

//-----------------------------------------------------------------------------
// Explosion
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Shell ejected during reload.
//-----------------------------------------------------------------------------

datablock DebrisData(SniperBulletShell)
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

datablock ProjectileData( SniperProjectile )
{
   projectileShapeName = "";
   //sound               = "";
   directDamage        = 55;
   radiusDamage        = 0;
   damageRadius        = 0;
   areaImpulse         = 0.5;
   impactForce         = 1;
   damageType          = $DamageType::Sniper;

   explosion           = BulletExplosion;
   waterExplosion      = BulletWaterExplosion;
   playerExplosion     = BloodSpillEmitter;
   decal               = BulletHoleDecal;

   particleEmitter     = "BulletTrailEmitter";
   particleWaterEmitter = "UWBulletTrailEmitter";

   Splash              = BulletSplash;
   muzzleVelocity      = 600;
   velInheritFactor    = 0;

   armingDelay         = 0; // How long it should not detonate on impact
   lifetime            = 1200; // How long the projectile should exist before deleting itself
   fadeDelay           = 500; // Brief Amount of time, in milliseconds, before the projectile begins to fade out.

   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod          = 1;

   lightDesc           = "";
};

//-----------------------------------------------------------------------------
// Ammo Item
//-----------------------------------------------------------------------------
datablock ItemData(SniperMag : DefaultClip)
{
   // Dynamic properties defined by the scripts
   pickUpName = 'Sniper magazine';
};

datablock ItemData(SniperAmmo : DefaultAmmo)
{
   // Dynamic properties defined by the scripts
   pickUpName = 'Sniper bullet';
   clip = RyderClip;
};

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the SoldierWeaponImage is used.
//-----------------------------------------------------------------------------
datablock ItemData(SniperRifle : DefaultWeapon)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Lurker/TP_Lurker.dts";

   // Dynamic properties defined by the scripts
   pickUpName = 'Sniper Rifle';
   image = SniperRifleWeaponImage;
};

datablock ShapeBaseImageData(SniperRifleWeaponImage)
{
   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";

   // Basic Item properties
   shapeFile = "art/shapes/weapons/Lurker/TP_Lurker.dts";
   shapeFileFP = "art/shapes/weapons/Lurker/FP_Lurker.dts";
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

   // Projectiles and Ammo.
   item = SniperRifle;
   ammo = SniperAmmo;
   clip = SniperMag;

   usesEnergy = 0;
   minEnergy = 0;

   projectile = SniperProjectile;
   projectileType = Projectile;
   projectileSpread = 0.001;

   //altProjectile = GrenadeLauncherProjectile;
   //altProjectileSpread = "0.02";

   casing = SniperBulletShell;
   shellExitDir        = "1.0 0.3 1.0";
   shellExitOffset     = "0.15 -0.56 -0.1";
   shellExitVariance   = 15.0;
   shellVelocity       = 3.0;

   // Weapon lights up while firing
   lightType = "WeaponFireLight";
   lightColor = "0.992126 0.968504 0.708661 1";
   lightRadius = "4";
   lightDuration = "150";
   lightBrightness = 1;

   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "3 3 3";
   camShakeAmp = "5 5 5";
   camShakeDuration = "1.0";
   camShakeRadius = "1.2";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   useRemainderDT = true;

   stateName[0]                     = "Preactivate";
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNoAmmo[0]       = "NoAmmo";

   stateName[1]                     = "Activate";
   stateTransitionGeneric0In[1]     = "SprintEnter";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 0.5;
   stateSequence[1]                 = "switch_in";
   stateSound[1]                    = SniperSwitchinSound;

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
   stateSound[3]                    = LurkerIdleSound;

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
   stateTransitionOnTimeout[5]      = "NewRound";
   stateTimeoutValue[5]             = 0.25;
   stateFire[5]                     = true;
   stateRecoil[5]                   = "light_recoil";
   stateAllowImageChange[5]         = false;
   stateSequence[5]                 = "fire";
   stateScaleAnimation[5]           = false;
   stateSequenceNeverTransition[5]  = true;
   stateSequenceRandomFlash[5]      = true;        // use muzzle flash sequence
   stateScript[5]                   = "onFire";
   stateSound[5]                    = SniperFireSound;
   stateEmitter[5]                  = GunFireSmokeEmitter;
   stateEmitterTime[5]              = 0.025;

   stateName[6]                     = "NewRound";
   stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnNoAmmo[6]       = "NoAmmo";
   stateTransitionOnTimeout[6]      = "Ready";
   stateWaitForTimeout[6]           = true;
   stateTimeoutValue[6]             = 0.45;
   stateAllowImageChange[6]         = false;
   stateEjectShell[6]               = true;

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
   stateWaitForTimeout[9]           = true;
   stateTimeoutValue[9]             = 0.7;
   stateTransitionOnTimeout[9]      = "NoAmmo";
   stateScript[9]                   = "onDryFire";
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
   stateTimeoutValue[13]            = 0.5;
   stateSequenceTransitionIn[13]    = true;
   stateSequenceTransitionOut[13]   = true;
   stateAllowImageChange[13]        = false;
   stateSequence[13]                = "sprint";
   
   stateName[15]                     = "ReloadFinish";
   stateTimeoutValue[15]             = 0.1;
   stateTransitionOnAmmo[15]         = "Ready";
   stateScript[15]                   = "onReloadFinish";      
};

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowWeapon("Soldier");
SmsInv.AddWeapon(SniperRifle, "Sniper Rifle", 1);

SmsInv.AllowClip("armor\tSoldier\t4");
SmsInv.AddClip(SniperMag, "Sniper Magazine", 4);

SmsInv.AllowAmmo("armor\tSoldier\t6");
SmsInv.AddAmmo(SniperAmmo, 6);
