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
datablock SFXProfile(PaintballMarkerFireSound1)
{
   filename = "art/sound/weapons/paintball/paintball_shot_1.wav";
   description = AudioClose3D;
   preload = true;
};
datablock SFXPlayList(PaintballMarkerFireSoundList)
{
   description = "AudioClose3D";
   random = "StrictRandom";
   loopMode = "Single";
   numSlotsToPlay = "1";
   track[0] = "PaintballMarkerFireSound1";
   track[1] = "PaintballMarkerFireSound1";
   track[2] = "PaintballMarkerFireSound1";
   pitchScaleVariance[0] = "0 0.1";
   pitchScaleVariance[1] = "0 0.1";
   pitchScaleVariance[2] = "-0.1 0.1";
};

datablock SFXProfile(PaintballImpactSound1)
{
   filename = "art/sound/weapons/paintball/paintball_impact_1.wav";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(PaintballImpactSound2)
{
   filename = "art/sound/weapons/paintball/paintball_impact_2.wav";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(PaintballImpactSound3)
{
   filename = "art/sound/weapons/paintball/paintball_impact_3.wav";
   description = AudioClose3D;
   preload = true;
};

datablock SFXPlayList(PaintballImpactSoundList)
{
   description = "AudioClosest3D";
   random = "StrictRandom";
   loopMode = "Single";
   numSlotsToPlay = "1";
   track[0] = "PaintballImpactSound1";
   track[1] = "PaintballImpactSound2";
   track[2] = "PaintballImpactSound3";
   pitchScaleVariance[0] = "-0.2 0.2";
   volumeScaleVariance[0] = "-0.3 0";
   volumeScaleVariance[1] = "-0.3 0";
   pitchScaleVariance[1] = "-0.1 0.2";
   volumeScaleVariance[2] = "-0.3 0";
   pitchScaleVariance[2] = "0 0.2";
};
// ----------------------------------------------------------------------------
// Particles
// ----------------------------------------------------------------------------
datablock ParticleData(PaintballMarkerSmoke)
{
   textureName          = "art/particles/smoke";
   dragCoefficient      = 0;
   gravityCoefficient   = "-0.8";
   windCoefficient      = 0;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = "400";
   lifetimeVarianceMS   = "100";
   spinRandomMin = -180.0;
   spinRandomMax =  180.0;
   useInvAlpha   = true;

   colors[0]     = "0.996078 0.996078 0.996078 0.556";
   colors[1]     = "0.905882 0.905882 0.905882 0.456";
   colors[2]     = "0.8 0.8 0.8 0.307";
   colors[3]     = "0.996078 0.996078 0.996078 0.245";

   sizes[0]      = "0.35";
   sizes[1]      = "0.595129";
   sizes[2]      = "0.9";
   sizes[3]      = "1.2";

   times[0]      = "0.208333";
   times[1]      = "0.395833";
   times[2]      = "0.645833";
   times[3]      = "0.9375";
   animTexName = "art/particles/smoke";
};

datablock ParticleEmitterData(PaintballMarkerSmokeEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = "0";
   velocityVariance = "0";
   thetaMin         = "0";
   thetaMax         = "0";
   lifetimeMS       = 250;
   particles = "PaintballMarkerSmoke";
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
   softParticles = "0";
   originalName = "PaintballMarkerSmokeEmitter";
   alignParticles = "0";
   orientParticles = "0";
};

datablock ParticleData(PaintDustBlue)
{
   textureName          = "art/particles/impactDrops.png";
   dragCoefficient      = "0.957967";
   gravityCoefficient   = "0.197803";
   windCoefficient      = 0;
   inheritedVelFactor   = "0";
   constantAcceleration = "0";
   lifetimeMS           = "200";
   lifetimeVarianceMS   = "100";
   spinRandomMin = -180.0;
   spinRandomMax =  180.0;
   useInvAlpha   = true;

   colors[0]     = "0 0.102362 0.677165 1";
   colors[1]     = "0.102362 0.204724 0.677165 1";
   colors[2]     = "0.141732 0.299213 0.677165 1";

   sizes[0]      = "0.4";
   sizes[1]      = "0.7";
   sizes[2]      = "1.1";

   times[0]      = "0.1";
   times[1]      = "0.494118";
   times[2]      = "1.0";
   animTexName = "art/particles/impactDrops.png";
};

datablock ParticleEmitterData(PaintExplosionBlueEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = "1";
   velocityVariance = 1.0;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles = "PaintDustBlue";
   blendStyle = "NORMAL";
   ambientFactor = "0.5";
};
//-----------------------------------------------------------------------------
// Explosion
//-----------------------------------------------------------------------------
datablock ExplosionData(PaintExplosionBlue)
{
   soundProfile = PaintballImpactSoundList;
   lifeTimeMS = 65;

   // Volume particles
   particleEmitter = PaintExplosionBlueEmitter;
   particleDensity = 4;
   particleRadius = 0.3;

   // Point emission
   emitter[0] = BulletDirtSprayEmitter;
   emitter[1] = BulletDirtSprayEmitter;
   emitter[2] = BulletDirtRocksEmitter;
};

//-----------------------------------------------------------------------------
// Projectile Object
//-----------------------------------------------------------------------------
datablock ProjectileData( PaintballBlueProjectile )
{
   projectileShapeName = "art/shapes/weapons/paintball/paintball_blue.dts";

   directDamage        = 200;
   radiusDamage        = 0;
   damageRadius        = 0;
   areaImpulse         = 0.5;
   impactForce         = 1;
   damageType          = $DamageType::Paintball;

   explosion           = PaintExplosionBlue;
   decal               = bluePaintSplatterDecal;

   muzzleVelocity      = 40;
   velInheritFactor    = 0;

   armingDelay         = 0;
   lifetime            = 12000;
   fadeDelay           = 0;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod          = 0.8;
};

datablock ProjectileData( PaintballBlueUnderWaterProjectile : PaintballBlueProjectile )
{
   particleWaterEmitter = "UWBulletTrailEmitter";
   muzzleVelocity       = 5;
   gravityMod           = 0.5;
};

//-----------------------------------------------------------------------------
// Ammo Item
//-----------------------------------------------------------------------------
datablock ItemData(PaintballClip)
{
   // Mission editor category
   category = "AmmoClip";

   // Add the Ammo namespace as a parent.  The ammo namespace provides
   // common ammo related functions and hooks into the inventory system.
   className = "AmmoClip";

   // Basic Item properties
   shapeFile = "art/shapes/weapons/paintball/paintball_marker_01_clip.dts";
   mass = 8;
   elasticity = 0.2;
   friction = 0.6;

   // Dynamic properties defined by the scripts
   pickUpName = "Paintball clip";
   count = 1;
   maxInventory = 10;
};

datablock ItemData(PaintballAmmo)
{
   // Mission editor category
   category = "Ammo";

   // Add the Ammo namespace as a parent.  The ammo namespace provides
   // common ammo related functions and hooks into the inventory system.
   className = "Ammo";

   // Basic Item properties
   shapeFile = "art/shapes/weapons/paintball/paintball_blue.dts";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;

   // Dynamic properties defined by the scripts
   pickUpName = "Paintball Ammo";
   maxInventory = 60;
   clip = PaintballClip;
};

//--------------------------------------------------------------------------
// Weapon Item.  This is the item that exists in the world, i.e. when it's
// been dropped, thrown or is acting as re-spawnable item.  When the weapon
// is mounted onto a shape, the SoldierWeaponImage is used.
//-----------------------------------------------------------------------------
datablock ItemData(PaintballMarkerBlue)
{
   category = "Weapon";
   className = "Weapon";

   shapeFile = "art/shapes/weapons/paintball/paintball_marker_01_blue.dts";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   PreviewImage = 'ryder.png';

   pickUpName = "A blue PaintballMarker weapon";
   description = "PaintballMarkerBlue";
   image = PaintballMarkerBlueWeaponImage;
   reticle = "crossHair";
};

datablock ShapeBaseImageData(PaintballMarkerBlueWeaponImage)
{
   shapeFile = "art/shapes/weapons/paintball/paintball_marker_01_blue.dts";
   shapeFileFP = "art/shapes/weapons/paintball/paintball_marker_01_blue.dts";
   emap = true;

   imageAnimPrefix = "Pistol";
   imageAnimPrefixFP = "Pistol";

   mountPoint = 0;
   firstPerson = true;
   useEyeNode = true;
   animateOnServer = true;
   correctMuzzleVector = true;

   class = "WeaponImage";
   className = "WeaponImage";

   item = PaintballMarkerBlue;
   ammo = PaintballAmmo;
   clip = PaintballClip;
   
   ironSight = PaintballMarkerBlueIronSightImage;

   projectile = PaintballBlueProjectile;
   underWaterProjectile = PaintballBlueUnderWaterProjectile;
   projectileType = Projectile;
   projectileSpread = "0.02";

   shakeCamera = true;
   camShakeFreq = "2 2 2";
   camShakeAmp = "2 2 2";
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
   //stateSound[1]                    = LurkerSwitchinSound;

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
   //stateSound[3]                    = LurkerIdleSound;

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
   stateTimeoutValue[5]             = 0.15;
   stateFire[5]                     = true;
   stateRecoil[5]                   = "";
   stateAllowImageChange[5]         = false;
   stateSequence[5]                 = "Fire";
   stateScaleAnimation[5]           = false;
   stateSequenceNeverTransition[5]  = true;
   stateSequenceRandomFlash[5]      = false;
   stateScript[5]                   = "onFire";
   stateSound[5]                    = PaintballMarkerFireSoundList;
   stateEmitter[5]                  = PaintballMarkerSmokeEmitter;
   stateEmitterTime[5]              = 0.025;

   stateName[6]                     = "NewRound";
   stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnNoAmmo[6]       = "NoAmmo";
   stateTransitionOnTimeout[6]      = "Ready";
   stateWaitForTimeout[6]           = true;
   stateTimeoutValue[6]             = 0.05;
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
   stateTimeoutValue[9]             = 0.4;
   stateSequence[9]                 = "idle";
   stateScript[9]                   = "onDryFire";
   stateTransitionOnTimeout[9]      = "NoAmmo";
   stateSound[9]                    = MachineGunDryFire;

   stateName[10]                     = "ReloadClip";
   stateTransitionOnTimeout[10]      = "ReloadFinish";
   stateWaitForTimeout[10]           = true;
   stateTimeoutValue[10]             = 2.5;
   stateReload[10]                   = true;
   stateSequence[10]                 = "reload";
   stateShapeSequence[10]            = "Reload";
   stateScaleShapeSequence[10]       = true;
   stateScaleAnimation[10]           = true;
   stateScaleAnimationFP[10]         = false;
   stateSound[10]                    = LurkerReloadSound;
   stateAllowImageChange[10]         = false; 

   stateName[11]                    = "SprintEnter";
   stateTransitionGeneric0Out[11]   = "SprintExit";
   stateTransitionOnTimeout[11]     = "Sprinting";
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
   stateTimeoutValue[13]            = 0.1;
   stateSequenceTransitionIn[13]    = true;
   stateSequenceTransitionOut[13]   = true;
   stateAllowImageChange[13]        = false;
   stateSequence[13]                = "sprint"; 
   
   stateName[14]                     = "ReloadFinish";
   stateTimeoutValue[14]             = 0.1;
   stateTransitionOnAmmo[14]         = "Ready";
   stateScript[14]                   = "onReloadFinish"; 
};

datablock ShapeBaseImageData( PaintballMarkerBlueIronSightImage : PaintballMarkerBlueWeaponImage )
{
   firstPerson = false;
   useEyeNode = true;
   animateOnServer = false;
   useEyeOffset = false;
   eyeOffset = "0 0.17 -0.147";
   eyeRotation = "0 0 0 0";

   projectileSpread = "0.005";
   parentImage = "PaintballMarkerBlueWeaponImage";
   
   stateTimeoutValue[1]             = 0.55;
   stateWaitForTimeout[1]           = true;
   stateSequence[1]                 = "idle";
   stateSound[1]                    = "";
   stateTransitionOnTimeout[1]      = "Ready";
   stateAllowImageChange[1]         = false; 
};

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowWeapon("Paintballer");
SmsInv.AddWeapon(PaintballMarkerBlue, "Blue Marker", 1);

SmsInv.AllowClip("armor\tPaintballer\t3");
SmsInv.AddClip(PaintballClip, "Paintball clips", 3);

SmsInv.AllowAmmo("armor\tPaintballer\t60");
SmsInv.AddAmmo(PaintballAmmo, 60);