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

datablock SFXProfile(GLFireSound)
{
   filename = "art/sound/weapons/wpn_lurker_grenadelaunch";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(GLReloadSound)
{
   filename = "art/sound/weapons/wpn_lurker_reload";
   description = AudioClosest3D;
   preload = true;
};

datablock SFXProfile(GLSwitchinSound)
{
   filename = "art/sound/weapons/wpn_lurker_switchin";
   description = AudioClosest3D;
   preload = true;
};

//-----------------------------------------------------------------------------
// Tracer particles
//-----------------------------------------------------------------------------

datablock ParticleData(UWGrenadeTrailParticle)
{
   dragCoefficient      = 0.0;
   gravityCoefficient   = "-0.251526";
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = "400";
   lifetimeVarianceMS   = "300";
   useInvAlpha          = false;

   textureName = "art/particles/bubble.png";
   animTexName[0] = "art/particles/bubble.png";

   spinRandomMin        = -100.0;
   spinRandomMax        =  100.0;

   colors[0]     = "0.692913 0.795276 1 0.519";
   colors[1]     = "0.692913 0.795276 1 0.315";
   colors[2]     = "0.692913 0.795276 1 0.166";

   sizes[0]      = "0.05";
   sizes[1]      = "0.08";
   sizes[2]      = "0.12";

   times[0]      = 0.0;
   times[1]      = "0.498039";
   times[2]      = 1.0;
};

datablock ParticleEmitterData(UWGrenadeTrailEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 1.0;
   ejectionOffset   = 0.1;
   velocityVariance = 0.5;
   thetaMin         = 0;
   thetaMax         = 80;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   particles = "UWGrenadeTrailParticle";
   blendStyle = "NORMAL";
   ambientFactor = "0.2";
};

datablock ParticleData(GrenadeTrailParticle)
{
   dragCoeffiecient     = 0;
   gravityCoefficient   = "-0.021978";   // rises slowly
   inheritedVelFactor   = "0.0998043";

   lifetimeMS           = 750;  // lasts 2 second
   lifetimeVarianceMS   = 100;   // ...more or less

   textureName = "art/particles/smokeThick.png";
   animTexName[0] = "art/particles/smokeThick.png";

   useInvAlpha = 0;
   spinRandomMin = -90.0;
   spinRandomMax = 90.0;

   colors[0]     = "0.897638 0.897638 0.897638 1";
   colors[1]     = "0.582677 0.582677 0.582677 1";
   colors[2]     = "0.393701 0.393701 0.393701 0";

   sizes[0]      = "0.25";
   sizes[1]      = "0.5";
   sizes[2]      = "1";

   times[0]      = 0.0;
   times[1]      = "0.498039";
   times[2]      = 1.0;
};

datablock ParticleEmitterData(GrenadeTrailEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 5;

   ejectionVelocity = 1;
   velocityVariance = 0.25;

   thetaMin         = 0.0;
   thetaMax         = 50.0;  
   phiReferenceVel = 90;

   particles = "GrenadeTrailParticle";
   ejectionOffset = "0.1";
   orientParticles = "0";
   softParticles = "1";
   blendStyle = "NORMAL";
   ambientFactor = "0.65";
};

//-----------------------------------------------------------------------------
// Light
//-----------------------------------------------------------------------------

datablock LightDescription(GrenadeLauncherLightDesc)
{
   color = "0.8 0.8 0.75";
   range = 3.0;
   brightness = 1.0;
};

//-----------------------------------------------------------------------------
// Shell ejected during reload.
//-----------------------------------------------------------------------------

datablock DebrisData(GrenadeShell)
{
   shapeFile = "art/shapes/weapons/shared/RifleShell.dts";
   scale = "2 2 2";
   lifetime = 6.0;
   minSpinSpeed = 150.0;
   maxSpinSpeed = 300.0;
   elasticity = 0.65;
   friction = 0.05;
   numBounces = 2;
   staticOnMaxBounce = true;
   snapOnMaxBounce = false;
   ignoreWater = true;
   fade = true;
};

//-----------------------------------------------------------------------------
// Projectile Object
//-----------------------------------------------------------------------------

datablock ProjectileData( GrenadeProjectile )
{
   projectileShapeName = "art/shapes/weapons/shared/rocket.dts";
   //sound               = "";
   directDamage        = 0;
   radiusDamage        = 130;
   damageRadius        = 4.5;
   areaImpulse         = 1;
   impactForce         = 5;

   damageType          = $DamageType::GrenadeLauncher;
   areaImpulse         = 1500;

   explosion           = GrenadeExplosion;
   waterExplosion      = UnderwaterGrenadeExplosion;
   decal               = ScorchRXDecal;

   particleEmitter     = GrenadeTrailEmitter;
   particleWaterEmitter= UWGrenadeTrailEmitter;

   Splash              = "";
   muzzleVelocity      = 50;
   velInheritFactor    = 0.4;

   armingDelay         = 600; // How long it should not detonate on impact
   lifetime            = 15000; // How long the projectile should exist before deleting itself
   fadeDelay           = 10000; // Brief Amount of time, in milliseconds, before the projectile begins to fade out.

   bounceElasticity    = 0.25;
   bounceFriction      = 0.25;
   isBallistic         = 1; // Causes the projectile to be affected by gravity "arc".
   gravityMod          = 1;

   lightDesc           = "";
};

datablock ProjectileData( GrenadeUnderWaterProjectile : GrenadeProjectile )
{
   particleWaterEmitter= "UWBulletTrailEmitter";
   muzzleVelocity      = 8;
   gravityMod          = 0.5;
};

//-----------------------------------------------------------------------------
// Ammo Item
//-----------------------------------------------------------------------------
datablock ItemData(GrenadeLauncherClip : DefaultClip)
{
   //shapeFile = "art/shapes/weapons/shared/rocket.dts";
   pickUpName = 'Grenade clip';
};

datablock ItemData(GrenadeLauncherAmmo : DefaultAmmo)
{
   //shapeFile = "art/shapes/weapons/shared/rocket.dts";
   pickUpName = 'Grenades';
   clip = GrenadeLauncherClip;
};

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the SoldierWeaponImage is used.
//-----------------------------------------------------------------------------
datablock ItemData(GrenadeLauncher : DefaultWeapon)
{
   shapeFile = "art/shapes/weapons/Lurker_brown/TP_Lurker_brown.dts";

   pickUpName = 'Grenade Launcher';
   image = GrenadeLauncherImage;
};

datablock ShapeBaseImageData(GrenadeLauncherImage)
{
   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";

   // Basic Item properties
   shapeFile = "art/shapes/weapons/Lurker_brown/TP_Lurker_brown.dts";
   shapeFileFP = "art/shapes/weapons/Lurker_brown/FP_Lurker_brown.dts";
   
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
   correctMuzzleVector = true;
   correctMuzzleVectorTP = true;

   // Projectiles and Ammo.
   item = GrenadeLauncher;
   ammo = GrenadeLauncherAmmo;
   clip = GrenadeLauncherClip;
   scopeSight = GrenadeLauncherScopeImage;
   

   usesEnergy = 0;
   minEnergy = 0;

   projectile = GrenadeProjectile;
   underWaterProjectile = GrenadeUnderWaterProjectile;
   projectileType = Projectile;
   projectileSpread = 0.025;

   //altProjectile = GrenadeProjectileAlt;
   //altProjectileSpread = "0.02";

   casing = GrenadeShell;
   shellExitDir        = "1.0 0.3 1.0";
   shellExitOffset     = "0.15 -0.56 -0.1";
   shellExitVariance   = 15.0;
   shellVelocity       = 3.0;

   // Weapon lights up while firing
   lightType = "WeaponFireLight";
   lightColor = "0.992126 0.968504 0.708661 1";
   lightRadius = "4";
   lightDuration = "200";
   lightBrightness = 1;

   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "2 2 2";
   camShakeAmp = "5 5 5";
   camShakeDuration = "1.2";
   camShakeRadius = "1.5";

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
   stateTimeoutValue[1]             = 0.5;
   stateSequence[1]                 = "idle";
   //stateSound[1]                    = GLSwitchinSound;

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
   stateTimeoutValue[4]             = 0.75;
   stateWaitForTimeout[4]           = true;
   stateFire[4]                     = true;
   stateRecoil[4]                   = LightRecoil;
   stateAllowImageChange[4]         = false;
   stateSequence[4]                 = "idle";
   stateScaleAnimation[4]           = true;
   stateSequenceNeverTransition[4]  = true;
   stateSequenceRandomFlash[4]      = false;
   stateScript[4]                   = "onFire";
   //stateEmitter[4]                  = RyderFireSmokeEmitter;
   //stateEmitterTime[4]              = 0.025;
   stateEjectShell[4]               = true;
   stateSound[4]                    = GLFireSound;

   stateName[5]                     = "WaitForRelease";
   stateTransitionGeneric0In[5]     = "SprintEnter";
   stateTransitionOnTriggerUp[5]    = "NewRound";
   stateTimeoutValue[5]             = 0.05;
   stateWaitForTimeout[5]           = true;
   stateAllowImageChange[5]         = false;

   stateName[6]                     = "NewRound";
   stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnNoAmmo[6]       = "NoAmmo";
   stateTransitionOnTimeout[6]      = "Ready";
   stateWaitForTimeout[6]           = "0";
   stateTimeoutValue[6]             = 0.5;
   stateAllowImageChange[6]         = false;

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
   stateTimeoutValue[9]             = 1.0;
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
   stateSound[10]                    = GLReloadSound;
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
   stateTimeoutValue[13]            = 0.8;
   stateSequenceTransitionIn[13]    = true;
   stateSequenceTransitionOut[13]   = true;
   stateAllowImageChange[13]        = false;
   stateSequence[13]                = "sprint";
   
   stateName[15]                     = "ReloadFinish";
   stateTimeoutValue[15]             = 0.1;
   stateTransitionOnAmmo[15]         = "Ready";
   stateScript[15]                   = "onReloadFinish";  
};

datablock ShapeBaseImageData( GrenadeLauncherScopeImage : GrenadeLauncherImage )
{
   
   firstPerson = false;
   useEyeNode = false;
   animateOnServer = false;
   useEyeOffset = true;
   
   eyeOffset = "0 -0.36 -0.25";
   eyeRotation = "0.574892 0.0910342 0.813149 4.72198";

   projectileSpread = "0.007";
   parentImage = "GrenadeLauncherImage";

   // Called when the weapon is first mounted and there is ammo.
   // We want a smooth transition from datablocks, change Activate params
   stateTimeoutValue[1]             = 0.8;
   stateWaitForTimeout[1]           = true;
   stateSequence[1]                 = "idle";
   stateSound[1]                    = "";
   stateTransitionOnTimeout[1]      = "Ready";
   stateAllowImageChange[1]         = false; 
};

//SMS Inventory----------------------------------------------------------------

SmsInv.AllowWeapon("Soldier");
SmsInv.AddWeapon(GrenadeLauncher, "Grenade Launcher", 1);

SmsInv.AllowClip("armor\tSoldier\t4");
SmsInv.AddClip(GrenadeLauncherClip, "Grenade Belt", 4);

SmsInv.AllowAmmo("armor\tSoldier\t6");
SmsInv.AddAmmo(GrenadeLauncherAmmo, 6);
