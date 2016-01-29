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

// Timeouts for corpse deletion.
$CorpseTimeoutValue = 10 * 1000;
$InvincibleTime = 8;

// Damage Rate for entering Liquid
$DamageLava       = 0.01;
$DamageHotLava    = 0.01;
$DamageCrustyLava = 0.01;

// Death Animations
$PlayerDeathAnim::TorsoFrontFallForward = 1;
$PlayerDeathAnim::TorsoFrontFallBack = 2;
$PlayerDeathAnim::TorsoBackFallForward = 3;
$PlayerDeathAnim::TorsoLeftSpinDeath = 4;
$PlayerDeathAnim::TorsoRightSpinDeath = 5;
$PlayerDeathAnim::LegsLeftGimp = 6;
$PlayerDeathAnim::LegsRightGimp = 7;
$PlayerDeathAnim::TorsoBackFallForward = 8;
$PlayerDeathAnim::HeadFrontDirect = 9;
$PlayerDeathAnim::HeadBackFallForward = 10;
$PlayerDeathAnim::ExplosionBlowBack = 11;

//$player::jumpTrigger = 2;
//$player::crouchTrigger = 3;
//$player::proneTrigger = 4;
//$player::sprintTrigger = 5;
//$player::jumpJetTrigger = 1;
//$player::imageTrigger0 = 0;
//$player::imageTrigger1 = 1;
//$player::maxImpulseVelocity = 
//$player::maxPredictionTicks = 
//$player::minWarpTicks = 
//$player::maxWarpTicks = 
//$player::renderCollision = 
//$player::renderMyItems = 
//$player::renderMyPlayer =
//$Player::Gravity =

//----------------------------------------------------------------------------
// Player Audio Profiles
//----------------------------------------------------------------------------

datablock SFXProfile(DeathCrySound)
{
   fileName = "art/sound/player/pain/orc_death";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(PainCrySound)
{
   fileName = "art/sound/player/pain/orc_pain";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(PainCrySound1)
{
   fileName = "art/sound/player/pain/orc_pain";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(PainCrySound2)
{
   fileName = "art/sound/player/pain/orc_pain";
   description = AudioClose3D;
   preload = true;
};

datablock SFXPlayList(PainCrySoundList)
{
   // Use a looped description so the list playback will loop.
   description = AudioClosest3D;

   track[ 0 ] = PainCrySound0;
   track[ 1 ] = PainCrySound1;
   track[ 2 ] = PainCrySound2;
};

//----------------------------------------------------------------------------

datablock SFXProfile(FootLightSoftSound)
{
   filename    = "art/sound/footsteps/lgtStep_mono_01";
   description = AudioClosest3D;
   preload = true;
};
datablock SFXPlayList(FootLightSoftSoundList)
{
   random = "StrictRandom";
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "AudioClosest3D";
   track[0] = "FootLightSoftSound";
   track[1] = "FootLightSoftSound";
   track[2] = "FootLightSoftSound";
   track[3] = "FootLightSoftSound";
   pitchScaleVariance[0] = "-0.1 0.2";
   volumeScaleVariance[0] = "-0.2 0";
   pitchScaleVariance[1] = "-0.1 0.2";
   volumeScaleVariance[1] = "-0.2 0";
   pitchScaleVariance[2] = "-0.1 0.2";
   volumeScaleVariance[2] = "-0.2 0";
   pitchScaleVariance[3] = "-0.1 0.2";
   volumeScaleVariance[3] = "-0.2 0";
};

datablock SFXProfile(FootLightHardSound)
{
   filename    = "art/sound/footsteps/hvystep_ mono_01";
   description = AudioClosest3D;
   preload = true;
};
datablock SFXPlayList(FootLightHardSoundList)
{
   random = "StrictRandom";
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "AudioClosest3D";
   track[0] = "FootLightHardSound";
   track[1] = "FootLightHardSound";
   track[2] = "FootLightHardSound";
   track[3] = "FootLightHardSound";
   pitchScaleVariance[0] = "-0.2 0.2";
   pitchScaleVariance[1] = "-0.2 0.2";
   pitchScaleVariance[2] = "-0.2 0.2";
   pitchScaleVariance[3] = "-0.2 0.2";
};

datablock SFXProfile(FootLightMetalSound)
{
   filename    = "art/sound/footsteps/metalstep_mono_01";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(FootLightSnowSound)
{
   filename    = "art/sound/footsteps/snowstep_mono_01";
   description = AudioClosest3D;
   preload = true;
};

datablock SFXProfile(FootLightShallowSplashSound)
{
   filename    = "art/sound/footsteps/waterstep_mono_01";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(FootLightWadingSound)
{
   filename    = "art/sound/footsteps/waterstep_mono_01";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(FootLightUnderwaterSound)
{
   filename    = "art/sound/footsteps/waterstep_mono_01";
   description = AudioClosest3D;
   preload = true;
};

//----------------------------------------------------------------------------
/* broken
datablock SFXProfile(ImpactLightSoftSound)
{
   filename    = "art/sound/player/pain/orc_death";
   description = AudioClose3D;
   preload = true;
};
*/
datablock SFXProfile(ImpactLightHardSound)
{
   filename    = "art/sound/impacts/impact_concrete_01";
   description = AudioClose3D;
   preload = true;
};

//----------------------------------------------------------------------------
// Splash
//----------------------------------------------------------------------------

datablock ParticleData(PlayerSplashMist)
{
   dragCoefficient      = 2.0;
   gravityCoefficient   = -0.05;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 400;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = false;
   spinRandomMin        = -90.0;
   spinRandomMax        = 500.0;
   textureName          = "art/shapes/actors/common/splash";
   colors[0]     = "0.7 0.8 1.0 1.0";
   colors[1]     = "0.7 0.8 1.0 0.5";
   colors[2]     = "0.7 0.8 1.0 0.0";
   sizes[0]      = 0.5;
   sizes[1]      = 0.5;
   sizes[2]      = 0.8;
   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(PlayerSplashMistEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 3.0;
   velocityVariance = 2.0;
   ejectionOffset   = 0.0;
   thetaMin         = 85;
   thetaMax         = 85;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   lifetimeMS       = 250;
   particles = "PlayerSplashMist";
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
};


datablock ParticleData(PlayerBubbleParticle)
{
   dragCoefficient      = 0.0;
   gravityCoefficient   = -0.50;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 400;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = false;
   textureName          = "art/shapes/actors/common/splash";
   colors[0]     = "0.7 0.8 1.0 0.4";
   colors[1]     = "0.7 0.8 1.0 0.4";
   colors[2]     = "0.7 0.8 1.0 0.0";
   sizes[0]      = 0.1;
   sizes[1]      = 0.3;
   sizes[2]      = 0.3;
   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(PlayerBubbleEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 2.0;
   ejectionOffset   = 0.5;
   velocityVariance = 0.5;
   thetaMin         = 0;
   thetaMax         = 80;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "PlayerBubbleParticle";
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
};

datablock ParticleData(PlayerFoamParticle)
{
   dragCoefficient      = 2.0;
   gravityCoefficient   = -0.05;
   inheritedVelFactor   = 0.1;
   constantAcceleration = 0.0;
   lifetimeMS           = 600;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = false;
   spinRandomMin        = -90.0;
   spinRandomMax        = 500.0;
   textureName          = "art/particles/millsplash01";
   colors[0]     = "0.7 0.8 1.0 0.20";
   colors[1]     = "0.7 0.8 1.0 0.20";
   colors[2]     = "0.7 0.8 1.0 0.00";
   sizes[0]      = 0.2;
   sizes[1]      = 0.4;
   sizes[2]      = 1.6;
   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(PlayerFoamEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 3.0;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 85;
   thetaMax         = 85;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "PlayerFoamParticle";
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
};


datablock ParticleData( PlayerFoamDropletsParticle )
{
   dragCoefficient      = 1;
   gravityCoefficient   = 0.2;
   inheritedVelFactor   = 0.2;
   constantAcceleration = -0.0;
   lifetimeMS           = 600;
   lifetimeVarianceMS   = 0;
   textureName          = "art/shapes/actors/common/splash";
   colors[0]     = "0.7 0.8 1.0 1.0";
   colors[1]     = "0.7 0.8 1.0 0.5";
   colors[2]     = "0.7 0.8 1.0 0.0";
   sizes[0]      = 0.8;
   sizes[1]      = 0.3;
   sizes[2]      = 0.0;
   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData( PlayerFoamDropletsEmitter )
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 0;
   ejectionVelocity = 2;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 60;
   thetaMax         = 80;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   orientParticles  = true;
   particles = "PlayerFoamDropletsParticle";
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
};

datablock ParticleData( PlayerWakeParticle )
{
   textureName          = "art/particles/wake";
   dragCoefficient     = "0.0";
   gravityCoefficient   = "0.0";
   inheritedVelFactor   = "0.0";
   lifetimeMS           = "2500";
   lifetimeVarianceMS   = "200";
   windCoefficient = "0.0";
   useInvAlpha = "1";
   spinRandomMin = "30.0";
   spinRandomMax = "30.0";

   animateTexture = true;
   framesPerSec = 1;
   animTexTiling = "2 1";
   animTexFrames = "0 1";

   colors[0]     = "1 1 1 0.1";
   colors[1]     = "1 1 1 0.7";
   colors[2]     = "1 1 1 0.3";
   colors[3]     = "0.5 0.5 0.5 0";

   sizes[0]      = "1.0";
   sizes[1]      = "2.0";
   sizes[2]      = "3.0";
   sizes[3]      = "3.5";

   times[0]      = "0.0";
   times[1]      = "0.25";
   times[2]      = "0.5";
   times[3]      = "1.0";
};

datablock ParticleEmitterData( PlayerWakeEmitter )
{
   ejectionPeriodMS = "200";
   periodVarianceMS = "10";

   ejectionVelocity = "0";
   velocityVariance = "0";

   ejectionOffset   = "0";

   thetaMin         = "89";
   thetaMax         = "90";

   phiReferenceVel  = "0";
   phiVariance      = "1";

   alignParticles = "1";
   alignDirection = "0 0 1";

   particles = "PlayerWakeParticle";
};

datablock ParticleData( PlayerSplashParticle )
{
   dragCoefficient      = "0.997067";
   gravityCoefficient   = "0.197803";
   inheritedVelFactor   = "0.199609";
   constantAcceleration = "0";
   lifetimeMS           = 600;
   lifetimeVarianceMS   = 0;
   colors[0]     = "0.692913 0.795276 1 1";
   colors[1]     = "0.692913 0.795276 1 0.496063";
   colors[2]     = "0.692913 0.795276 1 0";
   sizes[0]      = "0.497467";
   sizes[1]      = "0.497467";
   sizes[2]      = "0.497467";
   times[0]      = 0.0;
   times[1]      = "0.498039";
   times[2]      = 1.0;
   textureName = "art/particles/splash.png";
   animTexName = "art/particles/splash.png";
};

datablock ParticleEmitterData( PlayerSplashEmitter )
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 3;
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 60;
   thetaMax         = 80;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   orientParticles  = true;
   lifetimeMS       = 100;
   particles = "PlayerSplashParticle";
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
};

datablock SplashData(PlayerSplash)
{
   numSegments = 15;
   ejectionFreq = 15;
   ejectionAngle = 40;
   ringLifetime = 0.5;
   lifetimeMS = 300;
   velocity = 4.0;
   startRadius = 0.0;
   acceleration = -3.0;
   texWrap = 5.0;

   texture = "art/particles/millsplash01";

   emitter[0] = PlayerSplashEmitter;
   emitter[1] = PlayerSplashMistEmitter;

   colors[0] = "0.7 0.8 1.0 0.0";
   colors[1] = "0.7 0.8 1.0 0.3";
   colors[2] = "0.7 0.8 1.0 0.7";
   colors[3] = "0.7 0.8 1.0 0.0";
   times[0] = 0.0;
   times[1] = 0.4;
   times[2] = 0.8;
   times[3] = 1.0;
};

//----------------------------------------------------------------------------
// Foot puffs
//----------------------------------------------------------------------------

datablock ParticleData(LightPuff)
{
   dragCoefficient      = "1.99902";
   gravityCoefficient   = "0.0170946";
   inheritedVelFactor   = "0.598826";
   constantAcceleration = 0.0;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = true;
   spinRandomMin        = -35.0;
   spinRandomMax        = 35.0;
   colors[0]     = "0.854902 0.854902 0.854902 1";
   colors[1]     = "0.815686 0.803922 0.745098 0";
   colors[2]     = "0.835294 0.835294 0.835294 1";
   sizes[0]      = "0.0976622";
   sizes[1]      = "0.799609";
   sizes[2]      = "1";
   times[0]      = "0";
   times[1]      = 1.0;
   times[2]      = 1.0;
   textureName = "art/particles/dustParticle.png";
   animTexName = "art/particles/dustParticle.png";

};

datablock ParticleEmitterData(LightPuffEmitter)
{
   ejectionPeriodMS = 35;
   periodVarianceMS = 10;
   ejectionVelocity = 0.2;
   velocityVariance = 0.1;
   ejectionOffset   = 0.0;
   thetaMin         = 20;
   thetaMax         = 60;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   useEmitterColors = true;
   particles = "LightPuff";
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
};

//----------------------------------------------------------------------------
// Liftoff dust
//----------------------------------------------------------------------------

datablock ParticleData(LiftoffDust)
{
   dragCoefficient      = "0.997067";
   gravityCoefficient   = "-0.01221";
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = true;
   spinRandomMin        = -90.0;
   spinRandomMax        = 500.0;
   colors[0]     = "0.780392 0.74902 0.65098 1";
   colors[1]     = "0.878431 0.870588 0.835294 1";
   colors[2]     = "0.784314 0.784314 0.784314 1";
   sizes[0]      = "0.997986";
   sizes[1]      = "0.997986";
   sizes[2]      = "0.997986";
   times[0]      = "0";
   times[1]      = "0.329412";
   times[2]      = "0.658824";
   textureName = "art/particles/dustParticle";
   animTexName = "art/particles/dustParticle";
};

datablock ParticleEmitterData(LiftoffDustEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 2.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 90;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   useEmitterColors = true;
   particles = "LiftoffDust";
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
};

//----------------------------------------------------------------------------

datablock DecalData(PlayerFootprint)
{
   size = "0.35";
   material = CommonPlayerFootprint;
   lifeSpan = "16000";
   fadeTime = "16000";
   textureCoordCount = "0";
};

datablock DebrisData( PlayerDebris )
{
   explodeOnMaxBounce = false;

   elasticity = 0.15;
   friction = 0.5;

   lifetime = 4.0;
   lifetimeVariance = 0.0;

   minSpinSpeed = 40;
   maxSpinSpeed = 600;

   numBounces = 5;
   bounceVariance = 0;

   staticOnMaxBounce = true;
   gravModifier = 1.0;

   useRadiusMass = true;
   baseRadius = 1;

   velocity = 20.0;
   velocityVariance = 12.0;
};

// ----------------------------------------------------------------------------
// This is our default player datablock that all others will derive from.
// ----------------------------------------------------------------------------

datablock PlayerData(DefaultSoldier : ArmorDamageScale)
{
   className = Armor;
	
   renderFirstPerson = false;
   computeCRC = false;

   // Third person shape
   shapeFile = "art/shapes/actors/Soldier/soldier_rigged.dts";
   cameraMaxDist = "2.5";
   allowImageStateAnimation = true;

   // First person arms
   imageAnimPrefixFP = "soldier";
   shapeNameFP[0] = "art/shapes/actors/Soldier/FP/FP_SoldierArms.dts";

   canObserve = 1;
   cmdCategory = "Clients";

   cameraDefaultFov = "75";
   cameraMinFov = "5";
   cameraMaxFov = "75";

   debrisShapeName = "art/shapes/weapons/Grenade/grenadeDebris.dts";
   debris = playerDebris;
   
   throwForce = 25;

   minLookAngle = "-1.5";
   maxLookAngle = "1.5";
   maxFreelookAngle = 3.0;

   mass = "100";
   drag = "1.3";
   maxdrag = 0.4;
   density = "1.1";
   maxDamage = 100;
   maxEnergy =  "100";
   repairRate = "0.016";
   energyPerDamagePoint = 75;

   rechargeRate = 0.256;

   runForce = "3240";
   runEnergyDrain = "0.136";
   minRunEnergy = "10";
   // value * 2.48 = kph
   maxForwardSpeed = "4";
   maxBackwardSpeed = "3";
   maxSideSpeed = "3";

   sprintForce = "1500";
   sprintEnergyDrain = "0.512";
   minSprintEnergy = "0";
   maxSprintForwardSpeed = "8";
   maxSprintBackwardSpeed = "4";
   maxSprintSideSpeed = "5";
   sprintStrafeScale = "0.5";
   sprintYawScale = "0.5";
   sprintPitchScale = "0.5";
   sprintCanJump = true;

   crouchForce = 405;
   maxCrouchForwardSpeed = "2";
   maxCrouchBackwardSpeed = "1";
   maxCrouchSideSpeed = "1.5";

   swimForce = "4320";
   maxUnderwaterForwardSpeed = "1.5";
   maxUnderwaterBackwardSpeed = "1";
   maxUnderwaterSideSpeed = "1";

   jumpForce = "720";
   jumpEnergyDrain = "10";
   minJumpEnergy = "15";
   jumpDelay = "3";
   airControl = "0.3";

   fallingSpeedThreshold = "-4";

   landSequenceTime = "1.2";
   transitionToLand = "1";
   recoverDelay = "32";
   recoverRunForceScale = "0.5";

   minImpactSpeed = "10";
   minLateralImpactSpeed = 20;
   speedDamageScale = 3;

   boundingBox = "0.65 0.75 1.85";
   crouchBoundingBox = "0.65 0.75 1.25";
   swimBoundingBox = "1 2 2";
   pickupRadius = 1;

   // Damage location details
   boxHeadPercentage       = 0.83;
   boxTorsoPercentage      = 0.49;
   boxHeadLeftPercentage         = 0.30;
   boxHeadRightPercentage        = 0.60;
   boxHeadBackPercentage         = 0.30;
   boxHeadFrontPercentage        = 0.60;

   // Foot Prints
   decalOffset = 0.25;

   footPuffEmitter = "LightPuffEmitter";
   footPuffNumParts = "5";
   footPuffRadius = "0.25";

   dustEmitter = "LightPuffEmitter";

   splash = PlayerSplash;
   splashVelocity = 4.0;
   splashAngle = 67.0;
   splashFreqMod = 300.0;
   splashVelEpsilon = 0.60;
   bubbleEmitTime = 0.4;
   splashEmitter[0] = PlayerWakeEmitter;
   splashEmitter[1] = PlayerFoamEmitter;
   splashEmitter[2] = PlayerBubbleEmitter;
   mediumSplashSoundVelocity = 10.0;
   hardSplashSoundVelocity = 20.0;
   exitSplashSoundVelocity = 5.0;

   // Controls over slope of runnable/jumpable surfaces
   runSurfaceAngle  = 38;
   jumpSurfaceAngle = "80";
   maxStepHeight = 0.35;  //two meters
   minJumpSpeed = 20;
   maxJumpSpeed = 30;

   horizMaxSpeed = 68;
   horizResistSpeed = 33;
   horizResistFactor = 0.35;

   upMaxSpeed = 80;
   upResistSpeed = 25;
   upResistFactor = 0.3;

   footstepSplashHeight = 0.35;

   //NOTE:  some sounds commented out until wav's are available

   // Footstep Sounds
   FootSoftSound        = FootLightSoftSoundList;
   FootHardSound        = FootLightHardSoundList;
   FootMetalSound       = FootLightMetalSound;
   FootSnowSound        = FootLightSnowSound;
   FootShallowSound     = FootLightShallowSplashSound;
   FootWadingSound      = FootLightWadingSound;
   FootUnderwaterSound  = FootLightUnderwaterSound;

   //FootBubblesSound     = FootLightBubblesSound;
   //movingBubblesSound   = ArmorMoveBubblesSound;
   //waterBreathSound     = WaterBreathMaleSound;

   impactSoftSound      = ImpactLightSoftSound;
   impactHardSound      = ImpactLightHardSound;
   //impactMetalSound     = ImpactLightMetalSound;
   //impactSnowSound      = ImpactLightSnowSound;

   //impactWaterEasy      = ImpactLightWaterEasySound;
   //impactWaterMedium    = ImpactLightWaterMediumSound;
   //impactWaterHard      = ImpactLightWaterHardSound;

   groundImpactMinSpeed    = "4.1";
   groundImpactShakeFreq   = "3 3 3";
   groundImpactShakeAmp    = "1.0 1.0 1.0";
   groundImpactShakeDuration = "1";
   groundImpactShakeFalloff = 10.0;

   //exitingWater         = ExitingWaterLightSound;

   observeParameters = "0.5 4.5 4.5";

   cameraMinDist = "0";
   DecalData = "PlayerFootprint";

   // Allowable Inventory Items

   maxWeapons = 2;
   maxSpecials = 1;
   maxGrenades = 1;
   maxMines = 1;

   // Radius damage
   canImpulse = true;

   // Used by AI
   MoveSpeed = 0.5;          // You could call this the AI's throttle 1 = 100% throttle.
   MoveTolerance = 1;        // Distance from target position that is accepted as "reached"

   firstPersonShadows = "1";
   jumpTowardsNormal = "1";
   shadowSize = "512";
};

//-----------------------------------------------------------------------------
// SMS
//            |Datablock|     |$SMS::ArmorName|     |Index|
SmsInv.AddArmor( DefaultSoldier, "Soldier", 0 );