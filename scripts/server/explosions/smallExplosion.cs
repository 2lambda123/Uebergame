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

//-----------------------------------------------------------------------------
// Explosion Sounds

datablock SFXProfile(SmallExplosionSound)
{
   filename = "art/sound/weapons/Crossbow_explosion";
   description = AudioDefault3D;
   preload = true;
};

//-----------------------------------------------------------------------------
// Debris

datablock ParticleData(SmallDebrisSpark)
{
   textureName          = "art/particles/firefly.png";
   dragCoefficient      = 0;
   gravityCoefficient   = 0.0;
   windCoefficient      = 0;
   inheritedVelFactor   = "0.499022";
   constantAcceleration = 0.0;
   lifetimeMS           = "250";
   lifetimeVarianceMS   = "25";
   spinRandomMin = -90.0;
   spinRandomMax =  90.0;
   useInvAlpha   = false;

   colors[0]     = "0.795276 0.19685 0 1";
   colors[1]     = "0.795276 0.19685 0 1";
   colors[2]     = "0 0 0 0";

   sizes[0]      = 0.2;
   sizes[1]      = "0.2";
   sizes[2]      = "1";

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
   animTexName = "art/particles/firefly.png";
};

datablock ParticleEmitterData(SmallDebrisSparkEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 0;
   ejectionVelocity = "8";
   velocityVariance = "2";
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = 0;
   orientParticles  = false;
   lifetimeMS       = "250";
   particles = "SmallDebrisSpark";
   blendStyle = "ADDITIVE";
};

datablock ExplosionData(SmallDebrisExplosion)
{
   emitter[0] = SmallDebrisSparkEmitter;

   // Turned off..
   shakeCamera = false;
   lightStartRadius = 0;
   lightEndRadius = 0;
};

datablock ParticleData(SmallDebrisTrail)
{
   textureName          = "art/particles/dustParticle.png";
   dragCoefficient      = "0.997067";
   gravityCoefficient   = 0;
   inheritedVelFactor   = "1";
   windCoefficient      = 0;
   constantAcceleration = 0;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 100;
   spinSpeed     = 0;
   spinRandomMin = -90.0;
   spinRandomMax =  90.0;
   useInvAlpha   = "0";

   colors[0] = "1 1 0 0.5";
   colors[1] = "0.1 0.1 0.1 0.5";
   colors[2] = "0.3 0.3 0.3 0.5";

   sizes[0]  = "0.198376";
   sizes[1]  = "0.2";
   sizes[2]  = "0.3";

   times[0]  = "0";
   times[1]  = 0.2;
   times[2]  = 1.0;
   animTexName = "art/particles/dustParticle.png";
};

datablock ParticleEmitterData(SmallDebrisTrailEmitter)
{
   ejectionPeriodMS = 30;
   periodVarianceMS = 0;
   ejectionVelocity = 0.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 170;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   lifetimeMS       = 5000;
   particles = "SmallDebrisTrail";
   blendStyle = "NORMAL";
   softParticles = "0";
};

datablock DebrisData(SmallExplosionDebris)
{
   shapeFile = "art/shapes/weapons/grenade/grenadeDebris.dts";

   emitters = "SmallDebrisTrailEmitter";
   explosion = SmallDebrisExplosion;

   elasticity = 0.6;
   friction = 0.5;

   numBounces = 2;
   bounceVariance = 1;
   explodeOnMaxBounce = true;
   staticOnMaxBounce = false;
   snapOnMaxBounce = false;

   minSpinSpeed = 0;
   maxSpinSpeed = 500;

   lifetime = 15;
   lifetimeVariance = 0.0;

   velocity = 5;
   velocityVariance = 2;

   fade = false;

   useRadiusMass = true;
   baseRadius = 0.3;

   gravModifier = 0.5;
   terminalVelocity = 12;
   ignoreWater = true;
};

//-----------------------------------------------------------------------------
// Land Explosion

datablock ParticleData(SmallExplosionSmoke)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "-0.30525";
   inheritedVelFactor = "0";
   constantAcceleration = "0";
   lifetimeMS = "800";
   lifetimeVarianceMS = "288";
   spinSpeed = "0";
   spinRandomMin = "-80";
   spinRandomMax = "80";
   useInvAlpha = "1";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/smoke.png";
   animTexName[0] = "art/particles/smoke.png";
   colors[0] = "0.462745 0.427451 0.392157 0";
   colors[1] = "0.181102 0.181102 0.181102 0.685039";
   colors[2] = "0.385827 0.385827 0.385827 0";
   colors[3] = "1 1 1 1";
   sizes[0] = "2";
   sizes[1] = "6";
   sizes[2] = "10";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "0.5";
   times[2] = "1";
   times[3] = "1";
   allowLighting = "0";
      dragCoeffiecient = "0";
};

datablock ParticleEmitterData(SmallExplosionSmokeEmitter)
{
   ejectionPeriodMS = "15";
   periodVarianceMS = "5";
   ejectionVelocity = "3";
   velocityVariance = "2";
   ejectionOffset = "1";
   thetaMin = "0";
   thetaMax = "180";
   phiReferenceVel = "0";
   phiVariance = "360";
   overrideAdvance = "0";
   orientParticles = "0";
   orientOnVelocity = "1";
   particles = "SmallExplosionSmoke";
   lifetimeMS = "500";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
   blendStyle = "NORMAL";
};

datablock ParticleData(SmallExplosionSubSmoke)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "-1.00855";
   inheritedVelFactor = "0";
   constantAcceleration = "0";
   lifetimeMS = "672";
   lifetimeVarianceMS = "192";
   spinSpeed = "0";
   spinRandomMin = "-120";
   spinRandomMax = "120";
   useInvAlpha = "1";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/smoke.png";
   animTexName[0] = "art/particles/smoke.png";
   colors[0] = "0.787402 0.692913 0.598425 0.299213";
   colors[1] = "0.496063 0.496063 0.496063 0.787402";
   colors[2] = "0.188976 0.188976 0.188976 0";
   colors[3] = "1 1 1 1";
   sizes[0] = "1.99597";
   sizes[1] = "4.99603";
   sizes[2] = "9.99512";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "0.247059";
   times[2] = "1";
   times[3] = "2";
   allowLighting = "0";
      dragCoeffiecient = "0";
};

datablock ParticleEmitterData(SmallExplosionSubSmokeEmitter)
{
   ejectionPeriodMS = "30";
   periodVarianceMS = "10";
   ejectionVelocity = "1.5";
   velocityVariance = "0.5";
   ejectionOffset = "2";
   thetaMin = "0";
   thetaMax = "90";
   phiReferenceVel = "0";
   phiVariance = "360";
   overrideAdvance = "0";
   orientParticles = "0";
   orientOnVelocity = "1";
   particles = "SmallExplosionSubSmoke";
   lifetimeMS = "0";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
};

datablock ParticleData(SmallExplosionFireball)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "0";
   inheritedVelFactor = "0";
   constantAcceleration = "0";
   lifetimeMS = "500";
   lifetimeVarianceMS = "200";
   spinSpeed = "0";
   spinRandomMin = "-180";
   spinRandomMax = "180";
   useInvAlpha = "0";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/fireball.png";
   animTexName[0] = "art/particles/fireball.png";
   colors[0] = "1 0.889764 0.787402 0.889764";
   colors[1] = "0.787402 0.385827 0 0.299213";
   colors[2] = "0 0 0 0";
   colors[3] = "1 1 1 1";
   sizes[0] = "1.99292";
   sizes[1] = "6.99506";
   sizes[2] = "3.99194";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "0.5";
   times[2] = "1";
   times[3] = "1";
   allowLighting = "0";
      dragCoeffiecient = "0";
};

datablock ParticleEmitterData(SmallExplosionFireballEmitter)
{
   ejectionPeriodMS = "50";
   periodVarianceMS = "5";
   ejectionVelocity = "8";
   velocityVariance = "2";
   ejectionOffset = "0";
   thetaMin = "0";
   thetaMax = "180";
   phiReferenceVel = "0";
   phiVariance = "360";
   overrideAdvance = "0";
   orientParticles = "0";
   orientOnVelocity = "1";
   particles = "SmallExplosionFireball";
   lifetimeMS = "300";
   lifetimeVarianceMS = "100";
   useEmitterSizes = "0";
   useEmitterColors = "0";
   blendStyle = "ADDITIVE";
};

datablock ParticleData(SmallExplosionSubFireball)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "-3.01099";
   inheritedVelFactor = "0";
   constantAcceleration = "0";
   lifetimeMS = "300";
   lifetimeVarianceMS = "100";
   spinSpeed = "0";
   spinRandomMin = "-280";
   spinRandomMax = "280";
   useInvAlpha = "0";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/fireball.png";
   animTexName[0] = "art/particles/fireball.png";
   colors[0] = "1 0.603922 0 0.0866142";
   colors[1] = "1 0.496063 0 0.299213";
   colors[2] = "1 0 0 0";
   colors[3] = "1 1 1 1";
   sizes[0] = "0.994934";
   sizes[1] = "3.99194";
   sizes[2] = "4.99298";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "0.494118";
   times[2] = "1";
   times[3] = "1";
   allowLighting = "0";
      dragCoeffiecient = "0";
};

datablock ParticleEmitterData(SmallExplosionSubFireballEmitter)
{
   ejectionPeriodMS = "10";
   periodVarianceMS = "5";
   ejectionVelocity = "3.5";
   velocityVariance = "2";
   ejectionOffset = "2";
   thetaMin = "0";
   thetaMax = "120";
   phiReferenceVel = "0";
   phiVariance = "360";
   overrideAdvance = "0";
   orientParticles = "0";
   orientOnVelocity = "1";
   particles = "SmallExplosionSubFireball";
   lifetimeMS = "250";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
   blendStyle = "ADDITIVE";
};

datablock ParticleData(SmallExplosionSparks)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "0";
   inheritedVelFactor = "0.5";
   constantAcceleration = "0";
   lifetimeMS = "150";
   lifetimeVarianceMS = "0";
   spinSpeed = "0";
   spinRandomMin = "0";
   spinRandomMax = "0";
   useInvAlpha = "0";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/spark.png";
   animTexName[0] = "art/particles/spark.png";
   colors[0] = "1 0.929412 0 0.181102";
   colors[1] = "1 0.889764 0.787402 0.787402";
   colors[2] = "0.787402 0.385827 0 0";
   colors[3] = "1 1 1 1";
   sizes[0] = "0.994934";
   sizes[1] = "2.49344";
   sizes[2] = "2.5";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "0.5";
   times[2] = "1";
   times[3] = "1";
   allowLighting = "0";
};

datablock ParticleEmitterData(SmallExplosionSparksEmitter)
{
   ejectionPeriodMS = "10";
   periodVarianceMS = "2";
   ejectionVelocity = "50";
   velocityVariance = "5";
   ejectionOffset = "0";
   thetaMin = "0";
   thetaMax = "180";
   phiReferenceVel = "0";
   phiVariance = "360";
   overrideAdvance = "0";
   orientParticles = "1";
   orientOnVelocity = "1";
   particles = "SmallExplosionSparks";
   lifetimeMS = "250";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
   blendStyle = "ADDITIVE";
};

datablock ExplosionData(GenericSmallSubExplosion1)
{
   emitter[0] = "SmallExplosionSubFireballEmitter";
};

datablock ExplosionData(SmallExplosionSubExplosion2)
{
   emitter[0] = "SmallExplosionSubFireballEmitter";
   emitter[1] = "SmallExplosionSubSmokeEmitter";
};

datablock ExplosionData(SmallExplosion)
{
   faceViewer = "0";
   particleEmitter = "SmallExplosionSmokeEmitter";
   particleDensity = "10";
   particleRadius = "0.6";
   explosionScale = "1 1 1";
   playSpeed = "1";

   emitter[0] = "SmallExplosionFireballEmitter";
   emitter[1] = "SmallExplosionSubFireballEmitter";
   emitter[2] = "SmallExplosionSparksEmitter";
   emitter[3] = "SmallExplosionSparksEmitter";

   // Exploding debris
   debris = SmallExplosionDebris;
   debrisThetaMin = 0;
   debrisThetaMax = 60;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisNum = 5;
   debrisNumVariance = 2;
   debrisVelocity = 1;
   debrisVelocityVariance = 0.5;

   subExplosion[0] = "SmallExplosionSubExplosion1";
   subExplosion[1] = "SmallExplosionSubExplosion2";

   delayMS = "0";
   delayVariance = "0";
   lifetimeMS = "64";
   lifetimeVariance = "0";

   shakeCamera = true;
   camShakeFreq = "8.0 9.0 7.0";
   camShakeAmp = "10.0 10.0 10.0";
   camShakeDuration = 1.0;
   camShakeRadius = 5.0;

   lightStartRadius = 10.0;
   lightEndRadius = 1.0;
   lightStartColor = "0.75 0.2 0.2";
   lightEndColor = "0.5 0.5 0.0";
   lightStartBrightness = 5.0;
   lightEndBrightness = 0.0;
   lightNormalOffset = 2.0;
};
