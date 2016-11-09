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
$InvincibleTime = 2 * 1000;

// Damage Rate for entering Liquid
$DamageLava       = 0.01;
$DamageHotLava    = 0.01;
$DamageCrustyLava = 0.01;

// Death Animations
$PlayerDeathAnim::Death1 = 1;
$PlayerDeathAnim::Death2 = 2;
//$PlayerDeathAnim::TorsoBackFallForward = 3;
//$PlayerDeathAnim::TorsoLeftSpinDeath = 4;
//$PlayerDeathAnim::TorsoRightSpinDeath = 5;
//$PlayerDeathAnim::LegsLeftGimp = 6;
//$PlayerDeathAnim::LegsRightGimp = 7;
//$PlayerDeathAnim::TorsoBackFallForward = 8;
//$PlayerDeathAnim::HeadFrontDirect = 9;
//$PlayerDeathAnim::HeadBackFallForward = 10;
//$PlayerDeathAnim::ExplosionBlowBack = 11;

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
/*
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
   random = "StrictRandom";
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "AudioClosest3D";
   track[ 0 ] = PainCrySound0;
   track[ 1 ] = PainCrySound1;
   track[ 2 ] = PainCrySound2;
};
*/

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
// Jump Jets

datablock ParticleData(JumpJetParticle)
{
   textureName = "art/particles/flameExplosion.png";
   animTexName = "art/particles/flameExplosion.png";

   dragCoefficient      = 0;
   gravityCoefficient   = "0.998779";
   inheritedVelFactor   = "0.199609";
   constantAcceleration = 0;
   lifetimeMS           = "150";
   lifetimeVarianceMS   = 0;
   useInvAlpha          = false;

   colors[0] = "1 1 1 1";
   colors[1] = "1 1 1 0.5";
   colors[2] = "0 1 1 0";
   colors[3] = "1 1 1 0";

   sizes[0] = "0.5";
   sizes[1] = "0.75";

   times[0] = "0";
   times[1] = "0.8";
   times[2] = "1";
   times[3] = "1";
};

datablock ParticleEmitterData(JumpJetEmitter)
{
   ejectionPeriodMS = "10";
   periodVarianceMS = "2";
   ejectionVelocity = "4";
   velocityVariance = "0";
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 5;
   phiReferenceVel  = 0;
   phiVariance      = 90;
   overrideAdvances = 0;
   particles        = "JumpJetParticle";
   blendStyle = "ADDITIVE";
   softParticles = "1";
};

//----------------------------------------------------------------------------
// Player explosion            

datablock DebrisData( PlayerDebris )
{
   explodeOnMaxBounce = false;
   explodeOnMaxBounce = false;
   staticOnMaxBounce = false;
   snapOnMaxBounce = false;

   elasticity = 0.15;
   friction = 0.5;

   lifetime = 6.0;
   lifetimeVariance = 0.0;

   minSpinSpeed = 40;
   maxSpinSpeed = 500;

   numBounces = 3;
   bounceVariance = 1;

   gravModifier = 1.0;

   fade = true;
   useRadiusMass = true;
   baseRadius = 1;

   velocity = 5.0;
   velocityVariance = 2.0;

   terminalVelocity = 20;
   ignoreWater = false;
};

//----------------------------------------------------------------------------

datablock DecalData(PlayerFootprint)
{
   size = "0.35";
   material = CommonPlayerFootprint;
   lifeSpan = "2000";
   fadeTime = "28000";
   textureCoordCount = "0";
};

// ----------------------------------------------------------------------------
// This is our default player datablock that all others will derive from.
// ----------------------------------------------------------------------------

datablock PlayerData(DefaultPlayerData : ArmorDamageScale)
{
   className = Armor;
   cmdCategory = "Clients";

   // Third person shape
   imageAnimPrefix = "soldier";
   shapeFile = "art/shapes/actors/Soldier/soldier_rigged.dts";
   allowImageStateAnimation = true;

   // First person arms
   imageAnimPrefixFP = "soldier";
   shapeNameFP[0] = "art/shapes/actors/Soldier/FP/FP_SoldierArms.dts";
   // available skins (see materials.cs in model folder)
   availableSkins =  "base	olive	urban	desert	swamp	water	blue	red	green	yellow";
   computeCRC = false;
   emap = true;
   renderFirstPerson = false;
   firstPersonShadows = true;
   cloakTexture = "art/particles/cloakTexture.png";

   canObserve = 1;
   cameraMaxDist = 2.25;
   cameraMinDist = 0.2;
   cameraDefaultFov = "90";
   cameraMinFov = "5";
   cameraMaxFov = "120";

   explosion = PlayerExplosion;
   debrisShapeName = "art/shapes/weapons/Grenade/grenadeDebris.dts";
   debris = playerDebris;
   
   throwForce = 30;

   minLookAngle = "-1.5";
   maxLookAngle = "1.5";
   maxFreelookAngle = 3.0;

   mass = "100";
   drag = "1";
   maxdrag = 0.4;
   density = "1.1";
   maxDamage = 100;
   isInvincible = false;
   renderWhenDestroyed = true;
   maxEnergy =  "100";
   repairRate = "0.016";
   energyPerDamagePoint = 75;
   inheritEnergyFromMount = 0;
   rechargeRate = 0.256;

   runForce = "3240";
   runEnergyDrain = "0.136";
   minRunEnergy = "0";
   // assumed to be meters per second
   // so 1kph = 0.278m/s
   // 1ms = 3.6 kph
   maxForwardSpeed = "3.33"; //average jogging speed of ~12 kph
   maxBackwardSpeed = "2";
   maxSideSpeed = "2.5";

   sprintForce = "1100"; //low force to give some acceleration time
   sprintEnergyDrain = "0.512";
   minSprintEnergy = "1";
   maxSprintForwardSpeed = "8"; //28.8 kph, average sprinting speed of a man
   maxSprintBackwardSpeed = "4";
   maxSprintSideSpeed = "5";
   sprintStrafeScale = "0.417"; // 8/3.33 , sprint is 2.4 times faster so scale mouse accordingly
   sprintYawScale = "0.417";
   sprintPitchScale = "0.417";
   sprintCanJump = true;

   crouchForce = 405;
   maxCrouchForwardSpeed = "2";
   maxCrouchBackwardSpeed = "1.25";
   maxCrouchSideSpeed = "1.5";

   proneForce = 405;
   maxProneForwardSpeed = 1.5;
   maxProneBackwardSpeed = 1.0;
   maxProneSideSpeed = 1.25;

   swimForce = "4320";
   maxUnderwaterForwardSpeed = "1.5";
   maxUnderwaterBackwardSpeed = "1";
   maxUnderwaterSideSpeed = "1";

   jumpForce = "720";
   jumpEnergyDrain = "10";
   minJumpEnergy = "15";
   jumpDelay = "3";
   airControl = "0.3";
/*
   jetJumpForce = 140;
   jetJumpEnergyDrain = 0.8;    //< Energy per jump
   jetMinJumpEnergy = 3;
   jetMinJumpSpeed = 5;
   jetMaxJumpSpeed = 20;
   jetJumpSurfaceAngle = 80;   //< Angle vertical degrees
   jetEmitter = JumpJetEmitter;
   jetEmitterNumParts = 2;
   jetEmitterRadius = 0.25;
*/
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
   proneBoundingBox = "1 2.3 1";
   swimBoundingBox = "1 2 2";

   // Damage location details
   boxHeadPercentage       = 0.83;
   boxTorsoPercentage      = 0.49;
   boxHeadLeftPercentage         = 0.30;
   boxHeadRightPercentage        = 0.60;
   boxHeadBackPercentage         = 0.30;
   boxHeadFrontPercentage        = 0.60;

   // Foot Prints
   footPuffEmitter = "LightPuffEmitter";
   footPuffNumParts = "5";
   footPuffRadius = "0.25";
   DecalData = "PlayerFootprint";
   decalOffset = 0.25;
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
   footstepSplashHeight = 0.35;

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

   //JetJumpSound         = PlayerJumpJetSound;

   groundImpactMinSpeed    = "4.1";
   groundImpactShakeFreq   = "3 3 3";
   groundImpactShakeAmp    = "1.0 1.0 1.0";
   groundImpactShakeDuration = "1";
   groundImpactShakeFalloff = 10.0;

   //exitingWater         = ExitingWaterLightSound;

   observeParameters = "0.5 4.5 4.5";

   pickupRadius = 1.3;

   // Allowable Inventory Items

   maxWeapons = $pref::Server::MaxWeapons;
   maxSpecials = $pref::Server::MaxSpecials;
   maxGrenades = $pref::Server::MaxGrenades;
   maxMines = $pref::Server::MaxMines;

   // Radius damage
   canImpulse = true;

   jumpTowardsNormal = "1";
   shadowSize = "512";
   
   //BadBot AI settings
   VisionRange = 80;
   VisionFov = 180;
   findItemRange = 20;
   targetObjectTypes = $TypeMasks::PlayerObjectType;
   itemObjectTypes = $TypeMasks::itemObjectType;
   optimalRange["Ryder"] = 12;
   burstLength["Ryder"] = 100;
   optimalRange["Lurker"] = 16;
   burstLength["Lurker"] = 2000;
   optimalRange["Shotgun"] = 8;
   burstLength["Shotgun"] = 100;
   optimalRange["SniperRifle"] = 30;
   burstLength["SniperRifle"] = 2000;
   optimalRange["GrenadeLauncher"] = 25;
   burstLength["GrenadeLauncher"] = 2000;
   rangeTolerance = 3;
   switchTargetProbability = 0.5;
};

datablock PlayerData(PaintballPlayerData : DefaultPlayerData)
{
   shapeFile = "art/shapes/actors/paintball_player/paintball_player.dts";
   shapeNameFP[0] = "";
   boundingBox = "0.75 0.75 1.8";
   crouchBoundingBox = "0.75 0.75 1.25";
   renderFirstPerson = "1";
   
   groundImpactMinSpeed    = "4.1";
   groundImpactShakeFreq   = "3 3 3";
   groundImpactShakeAmp    = "0.2 0.2 0.2";
   groundImpactShakeDuration = "1";
   groundImpactShakeFalloff = 10.0;
   
   maxInvRyder = "0";
   
   //BadBot AI settings
   VisionRange = 80;
   VisionFov = 180;
   findItemRange = 20;
   targetObjectTypes = $TypeMasks::PlayerObjectType;
   itemObjectTypes = $TypeMasks::itemObjectType;
   optimalRange["PaintballMarkerBlue"] = 10;
   burstLength["PaintballMarkerBlue"] = 2000;
   optimalRange["PaintballMarkerRed"] = 10;
   burstLength["PaintballMarkerRed"] = 2000;
   optimalRange["PaintballMarkerGreen"] = 10;
   burstLength["PaintballMarkerGreen"] = 2000;
   optimalRange["PaintballMarkerYellow"] = 10;
   burstLength["PaintballMarkerYellow"] = 2000;
   rangeTolerance = 5;
   switchTargetProbability = 0.5;
};

//-----------------------------------------------------------------------------
// SMS
//            |Datablock|     |$SMS::ArmorName|     |Index|

SmsInv.AddArmor( DefaultPlayerData, "Soldier", 0 );
//SmsInv.AddArmor( BotDefaultPlayerData, "BotSoldier", 0 );
SmsInv.AddArmor( PaintballPlayerData, "Paintballer", 0 );
//SmsInv.AddArmor( BotPaintballPlayerData, "BotPaintballer", 0 );

//----------------------------------------------------------------------------
// Drowning script
//----------------------------------------------------------------------------
// How often to check the player's underwater status.
$Drowning::TickTime = 1 * 1000;   

// The length of the hold breath timer in number of ticks.  Used in combination
// with $Drowning::TickTime to calculate how long a player can hold his breath
// before taking damage.
$Drowning::HoldBreathTicks = 20; 

// Damage done per $Drowning::TickTime if the player is underwater and the
// hold breath timer has expired.
$Drowning::DamagePerTick = 10;

function checkUnderwater(%obj)
{
   if (!isObject(%obj)) return;  //prevent console spam, returns when object ID is not valid
   if (%obj.getState() $= "Dead")
   {
      // If we get here and the player is dead we should hide the breath meter
      // and make sure that the next underWater check is cancelled. 
      cancel(%obj.drowning);
   }
   else
   {
      // We'll use a "drowning" damageType so the game will know to distinguish
      // between a drowning and other types of death.
      %damageType = "Drowning";

      // If a ray cast straight up from the eye point of the player intersects
      // the surface of a waterblock then the player is obviously underwater.
      %start = %obj.getEyePoint();
      %end = VectorAdd(%start, "0 0 100"); // change if water is deeper than 100
      %searchMasks = $TypeMasks::WaterObjectType;
      if (ContainerRayCast(%start, %end, %searchMasks))
      {   
         // If the player is underwater increment a "holding breath" counter.
         %obj.holdingbreath++;

         // A GuiProgressCtrl expects values 0-1.  We're just calculating a 
         // percentage in order to generate the appropriate range of values
         // that will be used to adjust the lenght of the bar shown.  The
         // bar shrinks over time until we're out of air.
         %remainingTime = ($Drowning::HoldBreathTicks - %obj.holdingBreath) / $Drowning::HoldBreathTicks;
         if(%remainingTime < 0)
            %remainingTime = 0;
         
         // If the holdingbreath counter is greater than $Drowning::HoldBreathTicks
         // then damage the player - he's choking on water!
         if (%obj.holdingbreath > $Drowning::HoldBreathTicks)
            %obj.damage(0, %obj.getPosition(), $Drowning::DamagePerTick, %damageType);
      }
      else
      {
         // The player appears to have surfaced and is breathing normally.
         // Reset the holdingbreath counter and hide the breathmeter.
         %obj.holdingbreath = 0;
      }

      // We're still in the water and not dead yet, so do it again.
      %obj.drowning = schedule($Drowning::TickTime, 0, "checkUnderwater", %obj);
   }
}

function sendMsgClientKilled_Drowning(%msgType, %client, %sourceClient, %damLoc)
{
   messageAll(%msgType, '%1 drowned', %client.playerName);// Customized kill message for drowning
}

//----------------------------------------------------------------------------
// Player Datablock methods
//----------------------------------------------------------------------------

function Armor::onAdd(%this, %obj)
{
   //LogEcho("Armor::onAdd(" SPC %this @", "@ %obj SPC ")");
   // Seems to work ok, but some other things need to be adjusted such as movement speed.
   %scale = %this.scale $= "" ? "1 1 1" : %this.scale;
   if ( %obj.getScale() !$= %scale )
      %obj.setScale(%scale);

   // Vehicle timeout
   %obj.mountVehicles(true);
   %obj.mVehicle = "";

   %obj.scriptKilled = "";
   %obj.isInWater = 0;
   %obj.outOfBounds = false;

   // Default dynamic armor stats
   %obj.setEnergyLevel( %this.maxEnergy );
   %obj.setRechargeRate( %this.rechargeRate );
   %obj.setRepairRate( %this.repairRate );

   // If the player's client has some owned turrets, make sure we let them
   // know that we're a friend too.
   %client = %obj.client;
   if ( isObject( %client ) && %client.ownedTurrets )
   {
      %count = %client.ownedTurrets.getCount();
      for ( %i=0; %i<%count; %i++ )
      {
         %turret = %client.ownedTurrets.getObject(%i);
         %turret.addToIgnoreList( %obj );
      }
   }

   // AiPlayer class
   if ( %obj.isBot )
   {
      // $Bot::Set is created in loadMissionStage2
      if ( $Bot::Set.acceptsAsChild( %obj ) )
         $Bot::Set.add( %obj );
      else
         error( "Failed to add new AiPlayer object to Bot Set!" );
	 
	 %obj.setbehavior(BotTree, $BotTickFrequency);
   }
}

function Armor::onRemove(%this, %obj)
{
//   if (%obj.client.player == %obj)
//      %obj.client.player = 0;
}

function Armor::onNewDataBlock(%this, %obj)
{
   %scale = %this.scale $= "" ? "1 1 1" : %this.scale;
   if ( %obj.getScale() !$= %scale )
      %obj.setScale(%scale);
}

//----------------------------------------------------------------------------

function Armor::onMount(%this, %obj, %vehicle, %node)
{
   //LogEcho("Armor::onMount( " @ %this.getName() @ ", " @ %obj @ ", " @ %vehicle @ ", " @ %node @ " )");
   %vData = %vehicle.getDatablock();
   %type = %vData.getName();
   %class = %vehicle.getClassName();
   if ( %node == 0 )
   {
      // Release the main weapon trigger and unmount the weapon
      %obj.setImageTrigger($WeaponSlot, false);
      %obj.unmountImage($WeaponSlot);

      // Node 0 is the pilot's position.
      commandToClient(%obj.client, 'setHudMode', 'Pilot'); // Must be called before the key push

      %obj.setTransform("0 0 0 0 0 1 0");
      %obj.setActionThread(%vData.mountPose[%node], true, true);

      // Reposition to make sure we are not standing
      %obj.schedule(500, "setActionThread", %vData.mountPose[%node], true, true);

      %obj.setControlObject(%vehicle);
      %obj.mountVehicles(false);
   }
   else
   {
      // Passenger positions
      commandToClient(%obj.client, 'setHudMode', 'Passenger'); // Must be called before the key push
	  
      if(%vData.mountPose[%node] !$= "")
         %obj.setActionThread(%vData.mountPose[%node]);
      else
         %obj.setActionThread("root", true);
   }
   %obj.client.vehicleMounted = %vehicle;
}

function Armor::onUnmount(%this, %obj, %vehicle, %node)
{
   %vData = %vehicle.getDatablock();
   %class = %vehicle.getClassName();

   %obj.setActionThread("run", true, true);
   if ( %node == 0 || ( %class $= "Turret" && %node == 1 ) )
   {
      %obj.setArmThread("look");
      if(%obj.inv[%obj.lastWeapon])
         %obj.use(%obj.lastWeapon);
      else
      {
         if(%obj.getMountedImage($WeaponSlot) == 0) 
            %obj.use( %obj.weaponSlot[0] );
      }                      
   }
   %obj.client.vehicleMounted = "";
}

function Armor::doDismount(%this, %obj, %forced)
{
   //LogEcho("\c4Armor::doDismount(" @ %this @", "@ %obj.client.nameBase @", "@ %forced @")");

   // This function is called by player.cc when the jump trigger is true while mounted
   if( %obj.getState() !$= "Mounted" )
      return;

   %vehicle = %obj.mVehicle;
   if ( !%obj.isMounted() || !isObject(%vehicle) )
   {
      messageClient(%obj.client, 'MsgDismount', 'No vehicle to exit.');
      return;
   }

   // Vehicle must be at rest! Or result of vehicle destruction
   if( ( VectorLen( %vehicle.getVelocity()) <= %vehicle.getDataBlock().maxDismountSpeed ) || %forced)
   {
      commandToClient(%obj.client, 'setHudMode', 'Play'); // Takes care of keymaps as well as huds

      // Position above dismount point
      %rot    = %obj.rotFromTransform( %obj.getTransform() );
      %pos    = %obj.posFromTransform( %obj.getTransform() );
      %oldPos = %pos;
      %vec[0] = " 1  0  0";
      %vec[1] = " 0  -1  0";
      %vec[2] = " 0  1  0";
      %vec[3] = "-1  0  0";
      %vec[4] = " 0  0  2";
      %numAttempts = 5;
      %success     = -1;
      %impulseVec  = "0 0 0";
      %vec[0] = MatrixMulVector( %obj.getTransform(), %vec[0]);

      %pos = "0 0 0";
      for (%i = 0; %i < %numAttempts; %i++)
      {
         %pos = VectorAdd(%oldPos, VectorScale(%vec[%i], 5));
 
         if (%obj.checkDismountPoint(%oldPos, %pos))
         {
            %success = %i;
            %impulseVec = %vec[%i];
            break;
         }
      }
      if (%forced && %success == -1)
      {
         %pos = %oldPos;
      }
      %obj.unmount();
      %obj.client.setControlObject(%obj); // May have been a turret
      %obj.setControlObject(%obj);
      %obj.mVehicle.getDataBlock().playerDismounted(%obj.mVehicle, %obj);

      %obj.mVehicle = "";
      %obj.mountVehicles(false);
      %obj.schedule(1500, "mountVehicles", true);

      // Position above dismount point
      %obj.setTransform( %pos SPC %rot);
      %obj.playAudio( 0, UnmountVehicleSound );
      %obj.applyKick( 5, 50, "foward" );

      // Set player velocity when ejecting
      %vel = %obj.getVelocity();
      %vec = vectorDot(%vel, vectorNormalize(%vel));
      if(%vec > 50)
      {
         %scale = 50 / %vec;
         %obj.setVelocity(VectorScale(%vel, %scale));
      }
   }
   else
   {
      messageClient(%obj.client, 'msgUnmount', '\c2Cannot exit %1 while moving.', %vehicle.getDataBlock().nameTag);
   }
}

//----------------------------------------------------------------------------

function Armor::onCollision(%this, %obj, %col)
{
   //if ( %className $= "AIPlayer" )
   //	return;

   // Mounting vehicles is done via a raycast instead of collision.
   // This function would be used for player to player collisions only.
   if ( !isObject( %col ) || %obj.getState() $= "Dead" )
      return;

   %className = %col.getClassName();
   if ( %className $= "Player" )//|| %className $= "AIPlayer" )
   {
      // Other players alive do something.
      if ( %col.getState() !$= "Dead" )
      {
         %obj.repulse( %col );
         serverPlay3D( PlayerImpactSoftSound, %obj.getTransform() );
      }
      else
      {
         // We dont deal with weapons here as the corpse doesn't throw its mounted weapon before hand.
         %gotSomething = 0;
         // We can only pickup what the corpse was carrying..
         for ( %i = 0; %i < $SMS::MaxAmmos; %i++ )
         {
            if ( %col.hasInventory( $SMS::AmmoName[%i] ) )
            {
               %increase = %obj.incInventory( $SMS::AmmoName[%i], %col.getInventory( $SMS::AmmoName[%i] ) );
               if ( %increase > 0 )
               {
                  %gotSomething = 1;
                  %col.decInventory( $SMS::AmmoName[%i], %increase );
                  messageClient(%obj.client, 'MsgItemPickup', "", nameToID($SMS::AmmoName[%i]).pickUpName, %increase);
               }
            }
         }

         // We can only pickup what the corpse was carrying..
         for ( %i = 0; %i < $SMS::MaxClips; %i++ )
         {
            if ( %col.hasInventory( $SMS::Clip[%i] ) )
            {
               %increase = %obj.incInventory( $SMS::Clip[%i], %col.getInventory( $SMS::Clip[%i] ) );
               if ( %increase > 0 )
               {
                  %gotSomething = 1;
                  %col.decInventory( $SMS::Clip[%i], %increase );
                  messageClient(%obj.client, 'MsgItemPickup', "", nameToID( $SMS::Clip[%i]).pickUpName, %increase);
               }
            }
         }

         //if ( %gotSomething )
         //   serverPlay3D( CorpseLootingSound, %obj.getTransform() );
      }
   }
}

function Armor::onImpact(%this, %obj, %collidedObject, %vec, %vecLen)
{
   //LogEcho("Armor::onImpact(" SPC %this.getName() @", "@ %obj.client.nameBase @", "@ %collidedObject.getName() @", "@ %vec @", "@ %vecLen SPC ")");
   // This is called by the engine when a collision occurs over the minImpactSpeed player datablock parameter setting
   %this.damage(%obj, %collidedObject, VectorAdd(%obj.getPosition(),%vec), %vecLen * %this.speedDamageScale, $DamageType::Impact);

   //%obj.damage(0, VectorAdd(%obj.getPosition(), %vec), %vecLen * %this.speedDamageScale, "Impact");
}

//----------------------------------------------------------------------------

//Creates blood spatter decals
function DamageTypeCollision(%obj, %damage, %damageType, %position){
   //echo("DAMAGE TYPE COLLISION YO: " @ %damageType); //debug
   switch$ ($DamageText[%damageType])
   {
      case "Suicide": return;
      case "Drowning": return;
      case "Paintball": return;	 
	   case "MissionAreaDamage": return;	
      case "ScriptDamage": return;	
      case "Impact": return;
      default: // Process all other damage types               
   }
   
   %centerpoint = %obj.getWorldBoxCenter();
   
   %normal[0] = "0.0 0.0 1.0";
   %abNormal[0] = "2.0 2.0 0.0";
   %normal[1] = "0.0 1.0 0.0";
   %abNormal[1] = "2.0 0.0 2.0";
   %normal[2] = "1.0 0.0 0.0";
   %abNormal[2] = "0.0 2.0 2.0";
   %normal[3] = "0.0 0.0 -1.0";
   %abNormal[3] = "2.0 2.0 0.0";
   %normal[4] = "0.0 -1.0 0.0";
   %abNormal[4] = "2.0 0.0 2.0";
   %normal[5] = "-1.0 0.0 0.0";
   %abNormal[5] = "0.0 2.0 2.0";
   
   %clientCount = ClientGroup.getCount();
   for (%i=0;%i<6;%i++)
   {
       	%normalScaled = VectorScale(%normal[%i],-1); //distance to walls the blood splatters
		%targetPoint = VectorAdd(%centerpoint,%normalScaled);
		%mask = $TypeMasks::StaticObjectType | $TypeMasks::TerrainObjectType;
		%hitObject = ContainerRayCast(%centerpoint, %targetPoint, %mask, %obj);
        
        if (%hitObject)
        {
            %splatterPoint = getWords(%hitObject,1,3);
			%splatterNorm = getWords(%hitObject,4,6);
			%splatterScaling = getRandom()*1.5 + %damage/30;
            %splatterVary = getRandom()*getword(%abNormal[%i],0)-getword(%abNormal[%i],0)/2 
			SPC getRandom()*getword(%abNormal[%i],1)-getword(%abNormal[%i],1)/2 SPC getRandom()*getword(%abNormal[%i],2)-getword(%abNormal[%i],2)/2;            
            %Decalposition = VectorAdd(%splatterPoint, %splatterVary);
			if (%splatterScaling > 8)
			{ %splatterScaling = 8;} 
			   
			for (%clientIndex = 0; %clientIndex < %clientCount; %clientIndex++)
            {
                %client = ClientGroup.getObject(%clientIndex);
                %ghostIndex = %client.getGhostID(%obj);
       
                commandToClient(%client,'Spatter', %Decalposition, %splatterNorm, %splatterScaling);
            }
        }
   }
   /*
   %particles = new ParticleEmitterNode()   
{
      position = %position;  
      rotation = "1 0 0 0";  
      scale = "1 1 1";  
      dataBlock = "SmokeEmitterNode";  
      emitter = "bloodBulletDirtSprayEmitter";  
      velocity = "1";  
   };  
   MissionCleanup.add(%particles);  
   %particles.schedule(1000, "delete");
   */
}

//----------------------------------------------------------------------------

function Armor::damage(%this, %obj, %source, %position, %damage, %damageType)
{
   //LogEcho("Armor::damage(" SPC %this.getName() @", "@ %obj.getClassname() @", "@ %source.getClassname() @", "@ %position @", "@ %amount @", "@ $DamageText[%damageType] SPC ")");

   // The source is either the object that produced the damage or the objects owner
   if ( !isObject( %obj ) || %obj.invincible || %obj.getState() $= "Dead" )
      return;

   // If it's an AiPlayer, then send the source as client, this should be safe...
   %targetClient = isObject( %obj.client ) ? %obj.client : 0; // This HAS to be valid
   %sourceClient = isObject( %source.client ) ? %source.client : %source; // This SHOULD be ok
   %sourceTeam = isObject( %source ) ? %source.team : 0; // Used for team damage penalty

   // See if the shape is protected
   if ( %obj.isMounted() && %obj.scriptKilled $= "")
   {
      %mount = %obj.getObjectMount();
      %found = -1;
      for ( %i = 0; %i < %mount.getDataBlock().numMountPoints; %i++ )
      {
         if ( %mount.getMountNodeObject(%i) == %obj )
         {
            %found = %i;
            break;
         }
      }

      if ( %found != -1 )
      {
         if ( %mount.getDataBlock().isProtectedMountPoint[%found] )
         {
            return;
         }
      }
   }

   %location = %obj.getDamageLocation( %position );

   // If friendly fire is on, damage the source and no damage applied to target
   if ( isObject( %source ) && %source.isMemberOfClass( "Player" ) )
   {
      if( %obj.team == %sourceTeam && %obj != %source )
      {
         if ( $FriendlyFire )
            %this.damage( %source, %source, %source.getPosition(), %damage * 0.5, %damageType );
         else
            return;
      }
   }
   
   if ( %obj.isShielded && %obj.scriptKilled $= "" )
      %damage = %obj.imposeShield( %position, %damage, %damageType ); // Resides in shapeBase.cs

   %damageScale = %this.damageScale[%damageType];
   if ( %damageScale !$= "" )
      %damage *= %damageScale;
   
   // locational damage modifier start
      if (!isObject(%obj) || %obj.getState() $= "Dead" || !%damage)
      return;    
 
   %location = %obj.getDamageLocation(%position);//"Body";
   %bodyPart = getWord(%location, 0);
   %region = getWord(%location, 1);
   //echo(%obj @" \c4% DAMAGELOCATION:  bodyPart = "@ %bodyPart @" || REGION = "@ %region);
   switch$ (%bodyPart)
   {
      case "head":
         %damage = %damage*2; // 2 times the damage for a headshot
      case "torso":
      case "legs":
         %damage = %damage/1.6; // about two third damage for legs
   }
   
   DamageTypeCollision(%obj, %damage, %damageType, %position);
   // locational damage modifier end

   if ( %damage > 0 )
   {
      %obj.applyDamage(%damage);
	  
	  %location = "Body"; //from locational damage modifier

      Game.onDamaged( %targetClient, %sourceClient, %source, %damageType );

      if ( isObject( %targetClient ) && !%targetClient.isAiControlled() )
      {
         // Determine damage direction and prevent it on certain damage types
         if (%damageType !$= "Suicide"&& %damageType !$= "Drowning"&& %damageType !$= "MissionAreaDamage"&& %damageType !$= "ScriptDamage")
         %obj.setDamageDirection(%sourceObject, %position);
      }
   }

   // Return values: Dead, Mounted, Move, Recover
   // Dead is checked first so if dead and mounted, only dead is returned by C++..
   if ( %obj.getState() $= "Dead" )
   {
      if ( $DamageText[%damageType] $= "Grenade" || $DamageText[%damageType] $= "Explosion" )
         %obj.setVelocity( "0 0" SPC ( 1 / %this.mass ) ); 

      if ( isObject( Game ) )
         Game.onDeath( %obj, %targetClient, %source, %sourceClient, %damageType, %location );
   }
}

function Armor::onDamage(%this, %obj, %delta)
{
   //LogEcho("Armor::onDamage(" SPC %this.getName() @", "@ %obj.client.nameBase @", "@ %delta SPC ")");

   // This method is invoked by the ShapeBase code whenever the 
   // object's damage level changes.
   if (%delta > 0 && %obj.getState() !$= "Dead")
   {
      // Increment the flash based on the amount.
      %flash = %obj.getDamageFlash() + ((%delta / %this.maxDamage) * 2);
      if (%flash > 0.70)
         %flash = 0.70;

      %obj.setDamageFlash(%flash);

      // If the pain is excessive, let's hear about it.
      if (%delta > 33)
         %obj.playPain();

      // Send this off to the AI functions
      //if ( isObject( %obj.client ) && %obj.client.isAiControlled() )
      //   %obj.client.onDamaged( %obj, %delta );
   }
}

// ----------------------------------------------------------------------------
// The player object sets the "disabled" state when damage exceeds it's
// maxDamage value. This is method is invoked by ShapeBase state mangement code.

// If we want to deal with the damage information that actually caused this
// death, then we would have to move this code into the script "damage" method.

function Armor::onDisabled(%this, %player, %state)
{
   //LogEcho("Armor::onDisabled(" SPC %this.getName() @", "@ %player.client.nameBase @", "@ %state SPC ")");
   // Release the image triggers
   %player.setImageTrigger($WeaponSlot, false);
   %player.setImageTrigger($SpecialSlot, false);
   %player.setImageTrigger($AuxiliarySlot, false);
   %player.setImageTrigger($EffectsSlot, false);
   %player.setImageTrigger($GrenadeSlot, false);
   %player.setImageTrigger($FlagSlot, false);

   // Unmount from vehicles
   if ( %player.isMounted() )
      %this.doDismount( %player, 1 );

   // Toss current mounted weapon if any
   //%item = %player.getMountedImage( $WeaponSlot ).item;
   //if ( isObject( %item ) )
   //   %player.throw( %item );

   //%item = %player.getMountedImage( $SpecialSlot ).item;
   //if ( isObject( %item ) )
   //   %player.throw( %item );

   // Toss current mounted weapon and ammo if any
   %item = %player.getMountedImage($WeaponSlot).item;
   if (isObject(%item))
   {
      %amount = %player.getInventory( %item.image.clip );
      
      //if (!%item.image.clip)
      //   warn("No clip exists to throw for item ", %item);
      if( %amount )
         %player.throw( %item.image.clip, %amount );
   }

   // Toss out a health patch
   //%player.tossPatch();

   // Remove the special effects image
   if ( %player.getMountedImage($EffectsSlot) != 0 )
      %player.unmountImage($EffectsSlot);

   %player.playDeathCry();
   %player.playDeathAnimation();
   %player.setDamageFlash(0.70);

   if( isObject( %player ) )
      %player.setShapeName("");

   // Clear some possible goings on
   %player.setRepairRate(0);

   // Delete any remote detonated explosives
   if( isObject( %player.thrownChargeId ) )
   {
      %player.thrownChargeId.schedule(250, "delete");
      %player.thrownChargeId = 0;
   }

   //cancel( %player.scanMissileSchedule ); //unused?!
   cancel( %player.progressMeter );
   cancel( %player.reCloak );

   %player.clearDamageDt();

   //clear the deployable HUD
   messageClient(%clVictim, 'msgDeploySensorOff', "");
   %player.client.deploySpecial = false;
   cancel(%player.deployCheckThread);
   deactivateDeploySensor(%player);

   // reset the alarm for out of bounds
   if(%player.outOfBounds)
   {
      messageClient(%player.client, 'EnterMissionArea', "");
      %player.outOfBounds = false; // z0dd - ZOD, 5/19/03. Clear the var as well
   }

   if ( isObject( %player.lastVehicle ) )
   {
      schedule( 15000, %player.lastVehicle, "abandonTimeOut", %player.lastVehicle );
      %player.lastVehicle.lastPilot = "";
   }

   // AiPlayer class
   if ( %player.isBot )
   {
      //Parent::onDisabled(%this, %player, %state);
   
      if ( $Bot::Set.isMember( %player ) )
         $Bot::Set.remove( %player );
      else
         error( "Tried to remove AiPlayer from Bot Set that wasn't in the set!" );
  
      %player.behaviorTree.stop();
   }

   // Remove warning Gui in case the player was outside the mission area when he died
   //Canvas.popDialog (missionAreaWarningHud); //broken
   
   // Schedule corpse fade out
   %player.schedule( $CorpseTimeoutValue - 3000, "startFade", 3000, 0, true );

   // Schedule corpse removal.  Just keeping the place clean.
   %player.schedule( $CorpseTimeoutValue, "delete" );
}

function Armor::onDestroyed(%this, %player, %lastState)
{
   echo("Armor::onDestroyed(" SPC %this.getName() @", "@ %player.client.nameBase @", "@ %lastState SPC ")");
}

function Armor::applyConcussion(%data, %player)
{
   if ( %player.getState() !$= "Dead" )
   {
      %random = mDegToRad( getRandom( 360 ) );
      %player.setTransform( %player.getPosition() SPC "0 0 1 " @ %random );
      if ( getRandom() < 0.5 )
      {
         if ( %player.getMountedImage( $WeaponSlot ) != 0 )
            %player.unmountImage( $WeaponSlot );
      }
      %player.playPain();
   }
}

//-----------------------------------------------------------------------------

function Armor::onLeaveMissionArea(%this, %obj)
{
   //LogEcho("\c4Leaving Mission Area at POS:"@ %obj.getPosition());

   //if( isObject( %obj.client ) && !%obj.client.isAiControlled() )
   //   messageClient(%obj.client, 'LeaveMissionArea', '\c2You have left the mission area.');

   %obj.outOfBounds = true;

   // Hand it off to the game object
   Game.onLeaveMissionArea(%obj);
   
   //Canvas.pushDialog (missionAreaWarningHud); //broken

   // Damage over time and kill the coward! //broken
   //%obj.sheduleMissionAreaDamage = %obj.schedule ( 10000, setDamageDt, 15.0, "MissionAreaDamage");
}

function Armor::onEnterMissionArea(%this, %obj)
{
   // The control objects invoked this method when they move back into the mission area.
   //if( isObject( %obj.client ) && !%obj.client.isAiControlled() )
   //   messageClient(%obj.client, 'EnterMissionArea', '\c2You are back in the mission area.');

   %obj.outOfBounds = false;

   // Hand it off to the game object
   Game.onEnterMissionArea(%obj);
   
   //Canvas.popDialog (missionAreaWarningHud); //broken

   // Stop the punishment //broken
   //cancel(%obj.sheduleMissionAreaDamage);
   //%obj.clearDamageDt(); 
}

//-----------------------------------------------------------------------------

function Armor::onEnterLiquid(%this, %obj, %coverage, %type)
{
   //echo("\c4this:"@ %this @" object:"@ %obj @" just entered water of type:"@ %type @" for "@ %coverage @"coverage");
   %obj.drowning = schedule($Drowning::TickTime, 0, "checkUnderwater", %obj);
   
   //LogEcho("\c4this:"@ %this @" object:"@ %obj @" just entered water of type:"@ %type @" for "@ %coverage @"coverage");
   switch(%type)
   {
      case 0: //Water
         %obj.isInWater = 1;
      case 1: //Ocean Water
         %obj.isInWater = 1;
      case 2: //River Water
         %obj.isInWater = 1;
      case 3: //Stagnant Water
         %obj.isInWater = 1;
      case 4: //Lava
         %obj.isInWater = 1;
         %obj.setDamageDt(1000, 0.25, "Lava");
      case 5: //Hot Lava
         %obj.isInWater = 1;
         %obj.setDamageDt(1000, 0.5, "Lava");
      case 6: //Crusty Lava
         %obj.isInWater = 1;
         %obj.setDamageDt(1000, 1.0, "Lava");
      case 7: //Quick Sand
   }
}

function Armor::onLeaveLiquid(%this, %obj, %type)
{
   cancel(%obj.drowning); //stop drowning script
   %obj.clearDamageDt();
   %obj.isInWater = 0;
}

//-----------------------------------------------------------------------------

function echoTriggers()
{
   echo( "Jump:" SPC $player::jumpTrigger );
   echo( "Crouch:" SPC $player::crouchTrigger );
   echo( "Prone:" SPC $player::proneTrigger );
   echo( "Sprint:" SPC $player::sprintTrigger );
   echo( "JumpJet:" SPC $player::jumpJetTrigger );
   echo( "Image 0:" SPC $player::imageTrigger0 );
   echo( "Image 1:" SPC $player::imageTrigger1 );
}

function Armor::onTrigger(%this, %player, %triggerNum, %val)
{
   //echo("Armor::onTrigger( " @ %this.getName() SPC %player.client.nameBase SPC %triggerNum SPC %val @ " )");
   // This method is invoked when the player receives a trigger
   // move event.  The player automatically triggers slot 0 and
   // slot one off of triggers # 0 & 1.  Trigger # 2 is also used
   // as the jump key.
   switch$(%triggerNum)
   {
      case 0:
         // Image 0
      case 1:
         // Image 1
      case 2:
         // Jump
         if(%val == 1)
            %player.isJumping = true;
         else
            %player.isJumping = false;

      case 3:
         // Crouch
      case 4:
         // prone
      case 5:
         // Sprint
      case 6:
         // ?
      case 7:
         // ?
   }
}

function Armor::onForceUncloak(%this, %obj, %reason)
{
   %pack = %obj.getMountedImage($SpecialSlot);
   if((%pack <= 0) || (%pack.item !$= "StealthDevice"))
      return;

   // cancel recloak thread
   if(%obj.reCloak !$= "")
   {   
      Cancel(%obj.reCloak);
      %obj.reCloak = "";
   }

   messageClient(%obj.client, 'MsgCloakingPackOff', '\c2Cloaking pack off.  Jammed.');
   %obj.setCloaked(false);
   %obj.setImageTrigger($SpecialSlot, false);
}

//-----------------------------------------------------------------------------

function Armor::onPoseChange(%this, %obj, %oldPose, %newPose)
{
   // Set the script anim prefix to be that of the current pose
   %obj.setImageScriptAnimPrefix( $WeaponSlot, addTaggedString(%newPose) );
}

//-----------------------------------------------------------------------------

function Armor::onStartSwim(%this, %obj)
{
   %obj.setImageGenericTrigger($WeaponSlot, 0, true);
}

function Armor::onStopSwim(%this, %obj)
{
   %obj.setImageGenericTrigger($WeaponSlot, 0, false);
}

function Armor::onStartSprintMotion(%this, %obj)
{
   %obj.setImageGenericTrigger($WeaponSlot, 0, true);
}

function Armor::onStopSprintMotion(%this, %obj)
{
   %obj.setImageGenericTrigger($WeaponSlot, 0, false);
}

function Armor::onReachDestination(%data, %obj)
{
   if ( %obj.isBot )
   {
   if(isObject(%obj.behaviorTree))
      %obj.behaviorTree.postSignal("onReachDestination");
      
   %obj.atDestination = true;
   //%obj.setShapeName("onReachDestination"); //debug feature
   }
}

function Armor::animationDone(%data, %obj)
{
   if ( %obj.isBot )
   {
   if(isObject(%obj.behaviorTree))
      %obj.behaviorTree.postSignal("onAnimationDone");
   }
}

//-----------------------------------------------------------------------------
// Player methods
//-----------------------------------------------------------------------------

function Player::kill(%player, %damageType)
{
   %player.setInvincible( false );
   
   warn("Player::kill(" SPC %player.client.nameBase @", "@ %damageType SPC ")");
   %player.scriptKilled = true;

   %data = %player.getDataBlock();
   if ( %damageType $= "Suicide" )
   {
      // See if we have a shape charge mounted and not armed
      if ( %player.hasInventory( ShapeCharge.getId() ) )
      {
         if ( !isObject( %player.thrownChargeId ) )
         {
            %item = ItemData::create(ShapeChargeTossed);
            %item.setTransform( %player.getBoxCenter SPC "1 0 0 0" );
            %item.static = true;
            %item.rotate = false;
            %item.armed = true;
            %item.sourceObject = %player;
            MissionCleanup.add(%item);
            %item.setDamageState(Destroyed);
         }
      }

      // If the shape charge didn't kill us or we didn't have one..
      %data.damage(%player, %player, %player.getPosition(), %data.maxDamage, $DamageType::Suicide);
   }
   else
      %data.damage(%player, 0, %player.getPosition(), %data.maxDamage, %damageType);
}

function Player::causedTeamDamage(%this, %val)
{
   %this.causedRecentDamage = %val; 
}

function Player::setRespawnCloakOff(%player)
{
   %player.setCloaked(false);
   %player.respawnCloakThread = "";
}

function Player::setInvincible(%player, %val)
{
   %player.invincible = %val;
}

function Player::setInvincibleOff(%player)
{
   %player.invincible = false;
}

//----------------------------------------------------------------------------

// Keeps player from spawning multiple vehicles at once.
function Player::resetVpurchase(%player)
{
   %player.vBuyCmd = 0;
}

function Player::mountVehicles(%this, %bool)
{
   // If set to false, this variable disables vehicle mounting.
   %this.mountVehicle = %bool;
}

function Player::isPilot(%this)
{
   if ( !%this.isMounted() )
      return false;

   // There are two "if" statements to avoid a script warning.
   %vehicle = %this.getObjectMount();
   if (isObject(%vehicle))
   {
      if (%vehicle.getMountNodeObject(0) == %this)
         return true;
   }
   return false;
}

//----------------------------------------------------------------------------

function Player::playDeathAnimation(%this)
{
   %numDeathAnimations = %this.getNumDeathAnimations();
   if ( %numDeathAnimations > 0 )
   {
      if (isObject(%this.client))
      {
         if (%this.client.deathIdx++ > %numDeathAnimations)
            %this.client.deathIdx = 1;
         %this.setActionThread("Death" @ %this.client.deathIdx);
      }
      else
      {
         %rand = getRandom(1, %numDeathAnimations);
         %this.setActionThread("Death" @ %rand);
      }
   }
}

function Player::playCelAnimation(%this, %anim)
{
   if (%this.getState() !$= "Dead")
      %this.setActionThread("cel"@%anim);
}

//----------------------------------------------------------------------------

function Player::playDeathCry(%this)
{
   %this.playAudio( 0, DeathCrySound );
}

function Player::playPain(%this)
{
   %this.playAudio( 0, PainCrySound );
}

// ----------------------------------------------------------------------------

function Player::setDamageDirection(%player, %sourceObject, %damagePos)
{
   if (isObject(%sourceObject))
   {
      if (%sourceObject.isField(initialPosition))
      {
         // Projectiles have this field set to the muzzle point of
         // the firing weapon at the time the projectile was created.
         // This gives a damage direction towards the firing player,
         // turret, vehicle, etc.  Bullets and weapon fired grenades
         // are examples of projectiles.
         %damagePos = %sourceObject.initialPosition;
      }
      else
      {
         // Other objects that cause damage, such as mines, use their own
         // location as the damage position.  This gives a damage direction
         // towards the explosive origin rather than the person that lay the
         // explosives.
         %damagePos = %sourceObject.getPosition();
      }
   }

   // Rotate damage vector into object space
   %damageVec = VectorSub(%damagePos, %player.getWorldBoxCenter());
   %damageVec = VectorNormalize(%damageVec);
   %damageVec = MatrixMulVector(%player.client.getCameraObject().getInverseTransform(), %damageVec);

   // Determine largest component of damage vector to get direction
   %vecComponents = -%damageVec.x SPC %damageVec.x SPC -%damageVec.y SPC %damageVec.y SPC -%damageVec.z SPC %damageVec.z;
   %vecDirections = "Left"        SPC "Right"      SPC "Bottom"      SPC "Front"      SPC "Bottom"      SPC "Top";

   %max = -1;
   for (%i = 0; %i < 6; %i++)
   {
      %value = getWord(%vecComponents, %i);
      if (%value > %max)
      {
         %max = %value;
         %damageDir = getWord(%vecDirections, %i);
      }
   }
   commandToClient(%player.client, 'setDamageDirection', %damageDir);
}

function Player::use(%player, %data)
{
   // No mounting/using weapons when you're driving!
   if (%player.isPilot())
      return(false);

   Parent::use(%player, %data);
}

function Player::setArmor(%player, %armor)
{
   %client = %player.client;
   %player.setDataBlock(%armor);
   %client.armor = %armor;
}

function getDamagePercent(%maxDmg, %dmgLvl)
{
   return (%dmgLvl / %maxDmg);
}

// Somewhere in here we may want to disarm satchel charges..
function Player::unDeployObject(%player)
{
   if ( %player.inStation )
      return;

   %Masks = ( $TypeMasks::StaticShapeObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::GameBaseObjectType );
   %eyeVec = VectorNormalize( %player.getEyeVector() );
   %srchRange = VectorScale( %eyeVec, 5.0 );
   %plTm = %player.getEyeTransform();
   %plyrLoc = firstWord(%plTm) @ " " @ getWord(%plTm, 1) @ " " @ getWord(%plTm, 2);
   %srchEnd = VectorAdd( %plyrLoc, %srchRange );
   %scan = ContainerRayCast( %player.getEyeTransform(), %srchEnd, %Masks, %player );
   %potDep = firstWord( %scan );

   if ( %potDep )
   {
      %item = %potDep.getDataBlock().item;

      //LogEcho( "Potential UnDeploable:" SPC %potDep.getClassName() SPC "Datablock:" SPC %potDep.getDataBlock().getName() SPC
      //      "Is a Deployable:" SPC %potDep.getDataBlock().deployedObject SPC "Team:" SPC %potDep.team SPC "Owner:" SPC %potDep.getId().owner.nameBase );

      if ( %item !$= "" && %potDep.getDataBlock().deployedObject == true )
      {
         if ( %potDep.getDamageLevel() < 0.5 && !%potDep.isDisabled() )
         {
            if ( %player.maxInventory(%item) > 0 )
            {
               if( %potDep.team == %player.team )
               {
                  if ( %player.client == %potDep.getId().owner )
                  {
                     if ( !%potDep.justdeployed )
                     {
                        switch$ ( %item ) // ZOD 1-7-03: Special case for certain turrets etc.
                        {
                           case "DeployedTurret":
                              %potDep.justdeployed = true;
                              if(isObject(%potDep.lastProjectile))
                                 %potDep.lastProjectile.delete();

                              %potDep.clearSelfPowered();
                              $TeamDeployedCount[%player.team, %potDep.getDataBlock().getName()]--;
                              %potDep.schedule(250, "delete");

                           default:
                              %potDep.justdeployed = true;
                              %potDep.schedule(100, "delete");
                              $TeamDeployedCount[%player.team, %item]--;
                        }
                        %nSpecial = %item.create();
                        %nSpecial.static = false;
                        MissionCleanup.add(%nSpecial);
                        %pos = %potDep.getPosition();
                        %nSpecial.setTransform(VectorAdd(%pos, "0 0 0.75") SPC "0 0 1" SPC (getRandom() * 360));
                        %nSpecial.schedulePop(); // ZOD: Really important, otherwise the item lingers on the map forever or until its picked up.
                     }
                     else
                        messageClient(%player.client, 'MsgJustDeployed', '\c0Object was just deployed, please wait a moment.');
                  }
                  else
                     messageClient(%player.client, 'MsgNotDeployer', '\c0You did not deploy this object.');
               }
               else
                  messageClient(%player.client, 'MsgWrongTeam', '\c0Access Denied. Wrong Team.');
            }
            else
               messageClient(%player.client, 'MsgTooSmall', '\c0You can\'t undeploy this object in your armor.');
         }
         else
            messageClient(%player.client, 'MsgDisabled', '\c0Object is to heavily damaged.');
      }
      else
         messageClient(%player.client, 'MsgNotDeployable', '\c0Object is not undeployable.');
   }
   else
      messageClient(%player.client, 'MsgNothing', '\c0No undeployable in sight.');
}

function Player::progressMeter(%this, %time, %text)
{
   if(isEventPending(%this.progressMeter))
      cancel(%this.progressMeter);

   if(%time == 0)
      return;

   if(isObject(%this))
   {
      if(%this.getState() !$= "Dead")
      {
         if(%time == 60)
            %msg = "||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||";
         else
            %msg = getSubStr("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||", 0, %time);

         bottomPrint(%this.client, "<font:verdana bold:14>" @ %text @ ":" SPC %msg, 1, 1);
         %count = %time--;
         %this.progressMeter = %this.schedule(1000, "progressMeter", %count);
      }
   }
}