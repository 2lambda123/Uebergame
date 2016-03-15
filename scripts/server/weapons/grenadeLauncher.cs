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
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(GLSwitchinSound)
{
   filename = "art/sound/weapons/wpn_lurker_switchin";
   description = AudioClose3D;
   preload = true;
};

//-----------------------------------------------------------------------------
// Tracer particles
//-----------------------------------------------------------------------------

datablock ParticleData(UWGrenadeTrailParticle)
{
   dragCoefficient      = 0.0;
   gravityCoefficient   = -0.25;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 1500;
   lifetimeVarianceMS   = 600;
   useInvAlpha          = false;

   textureName = "art/particles/bubble.png";
   animTexName[0] = "art/particles/bubble.png";

   spinRandomMin        = -100.0;
   spinRandomMax        =  100.0;

   colors[0]     = "0.7 0.8 1.0 0.4";
   colors[1]     = "0.7 0.8 1.0 0.4";
   colors[2]     = "0.7 0.8 1.0 0.0";

   sizes[0]      = 0.5;
   sizes[1]      = 0.5;
   sizes[2]      = 0.5;

   times[0]      = 0.0;
   times[1]      = 0.5;
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
   blendStyle = "NORMAL";
   orientParticles = "0";
   softParticles = "1";
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
   radiusDamage        = 100;
   damageRadius        = 4;
   areaImpulse         = 1;
   impactForce         = 5;

   damageType          = $DamageType::GrenadeLauncher;
   areaImpulse         = 1500;

   explosion           = GrenadeExplosion;
   waterExplosion      = UnderwaterGrenadeExplosion;
   decal               = ScorchRXDecal;

   particleEmitter     = GrenadeTrailEmitter;
   particleWaterEmitter = UWGrenadeTrailEmitter;

   Splash              = GrenadeSplash;
   muzzleVelocity      = 50;
   velInheritFactor    = 0.5;

   armingDelay         = 1000; // How long it should not detonate on impact
   lifetime            = 15000; // How long the projectile should exist before deleting itself
   fadeDelay           = 10000; // Brief Amount of time, in milliseconds, before the projectile begins to fade out.

   bounceElasticity    = 0.4;
   bounceFriction      = 0.3;
   isBallistic         = 1; // Causes the projectile to be affected by gravity "arc".
   gravityMod          = 1;

   lightDesc           = "";
};

//-----------------------------------------------------------------------------
// Ammo Item
//-----------------------------------------------------------------------------
datablock ItemData(GrenadeLauncherClip : DefaultClip)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dts";
   pickUpName = 'Grenade clip';
};

datablock ItemData(GrenadeLauncherAmmo : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dts";
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
   shapeFile = "art/shapes/weapons/Lurker/TP_Lurker.dts";
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
   correctMuzzleVector = true;
   correctMuzzleVectorTP = true;

   // Projectiles and Ammo.
   item = GrenadeLauncher;
   ammo = GrenadeLauncherAmmo;
   clip = GrenadeLauncherClip;

   usesEnergy = 0;
   minEnergy = 0;

   projectile = GrenadeProjectile;
   projectileType = Projectile;
   projectileSpread = 0.008;

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
   stateSequence[1]                 = "switch_in";
   stateSound[1]                    = GLSwitchinSound;

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

//SMS Inventory----------------------------------------------------------------

SmsInv.AllowWeapon("Soldier");
SmsInv.AddWeapon(GrenadeLauncher, "Grenade Launcher", 1);

SmsInv.AllowClip("armor\tSoldier\t12");
SmsInv.AddClip(GrenadeLauncherClip, "Grenade Belt", 3);

SmsInv.AllowAmmo("armor\tSoldier\t6");
SmsInv.AddAmmo(GrenadeLauncherAmmo, 6);
