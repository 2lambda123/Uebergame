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

datablock SFXProfile(GrenadeExplosionSound)
{
   filename = "art/sound/weapons/GRENADELAND.wav";
   description = AudioGrenadeImpact;
   preload = true;
};

datablock SFXProfile(GrenadeLauncherExplosionSound)
{
   filename = "art/sound/weapons/Crossbow_explosion";
   description = AudioGrenadeImpact;
   preload = true;
};

//-----------------------------------------------------------------------------
// Splash particles
//-----------------------------------------------------------------------------

datablock ParticleData(GrenadeSplashParticle)
{
   canSaveDynamicFields = "1";
   dragCoefficient = "0.99218";
   windCoefficient = "1";
   gravityCoefficient = "-0.00732601";
   inheritedVelFactor = "0.197652";
   constantAcceleration = "-1.4";
   lifetimeMS = "288";
   lifetimeVarianceMS = "150";
   spinSpeed = "0";
   spinRandomMin = "0";
   spinRandomMax = "0";
   useInvAlpha = "0";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/sickieparticles/mist.png";
   animTexName[0] = "art/particles/sickieparticles/mist.png";
   colors[0] = "0.685039 0.787402 1 1";
   colors[1] = "0.685039 0.787402 1 0.496063";
   colors[2] = "0.685039 0.787402 1 0";
   colors[3] = "1 1 1 1";
   sizes[0] = "0.08";
   sizes[1] = "0.192272";
   sizes[2] = "0.25";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "0.494118";
   times[2] = "1";
   times[3] = "1";
};

datablock ParticleEmitterData(GrenadeSplashEmitter)
{
   ejectionPeriodMS = "4";
   periodVarianceMS = "0";
   ejectionVelocity = "4";
   velocityVariance = "1";
   ejectionOffset = "0";
   thetaMin = "0";
   thetaMax = "50";
   phiReferenceVel = "0";
   phiVariance = "360";
   overrideAdvance = "0";
   orientParticles = "1";
   orientOnVelocity = "1";
   particles = "GrenadeSplashParticle";
   lifetimeMS = "96";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
   overrideAdvances = "0";
};

datablock SplashData(GrenadeSplash)
{
   emitter[0] = "GrenadeSplashEmitter";
};


//--------------------------------------------------------------------------
// Underwater Projectile Explosion
//--------------------------------------------------------------------------

datablock ParticleData(GrenadeExplosionBubbleParticle)
{
   textureName = "art/particles/bubble";
   dragCoefficient = 0.0;
   gravityCoefficient = "-0.1";
   inheritedVelFactor = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS = "800";
   lifetimeVarianceMS = "600";
   useInvAlpha = false;

   spinRandomMin = -100.0;
   spinRandomMax =  100.0;

   colors[0] = "0.494118 0.607843 0.709804 0.394";
   colors[1] = "0.478431 0.576471 0.682353 0.303";
   colors[2] = "0.482353 0.564706 0.682353 0.195";

   sizes[0] = "0.05";
   sizes[1] = "0.1";
   sizes[2] = "0.2";

   times[0] = 0.0;
   times[1] = "0.498039";
   times[2] = 1.0;
   animTexName = "art/particles/bubble";
};

datablock ParticleEmitterData(GrenadeExplosionBubbleEmitter)
{
   ejectionPeriodMS = "2";
   periodVarianceMS = "1";
   ejectionVelocity = 1.0;
   ejectionOffset   = 2.0;
   velocityVariance = 0.5;
   thetaMin = 0;
   thetaMax = 80;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = 0;
   particles = "GrenadeExplosionBubbleParticle";
   blendStyle = "ADDITIVE";
};

datablock ParticleData(UnderwaterGrenadeSparks)
{
   textureName = "art/particles/droplet";
   dragCoefficient = 1;
   gravityCoefficient = 0.0;
   inheritedVelFactor = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS = 800;
   lifetimeVarianceMS = 350;
   colors[0] = "0.6 0.6 1.0 1.0";
   colors[1] = "0.6 0.6 1.0 1.0";
   colors[2] = "0.6 0.6 1.0 0.0";
   sizes[0] = 0.5;
   sizes[1] = 0.25;
   sizes[2] = 0.25;
   times[0] = 0.0;
   times[1] = 0.5;
   times[2] = 1.0;
};

datablock ParticleEmitterData(UnderwaterGrenadeSparkEmitter)
{
   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 6.75;
   ejectionOffset = 0.0;
   thetaMin = 0;
   thetaMax = 180;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   orientParticles = true;
   lifetimeMS = 100;
   particles = "UnderwaterGrenadeSparks";
};

datablock ParticleData(UnderwaterGrenadeExplosionSmoke)
{
   textureName = "art/particles/sickieparticles/rock_impact_1_inner.png";
   dragCoeffiecient = 105;
   gravityCoefficient = "1";
   inheritedVelFactor = "1";
   constantAcceleration = -1.0;
   
   lifetimeMS = "800";
   lifetimeVarianceMS = "400";

   useInvAlpha = false;
   spinRandomMin = -100.0;
   spinRandomMax = 100.0;

   colors[0] = "0.527559 0.598425 0.692913 0.502";
   colors[1] = "0.511811 0.606299 0.716535 0.353";
   colors[2] = "0.480315 0.574803 0.700787 0.253";
   colors[3] = "0.505882 0.611765 0.701961 0.104";
   sizes[0] = "2";  
   sizes[1] = "2.5";
   sizes[2] = "3.5";
   sizes[3] = "4.5";
   times[0] = 0.0;
   times[1] = 0.2;
   times[2] = "0.6875";
   times[3] = 1;
   animTexName = "art/particles/sickieparticles/rock_impact_1_inner.png";
   ejectionPeriodMS = "1";
   periodVarianceMS = "0";
   ejectionVelocity = "12";
   velocityVariance = "8";
   thetaMax = "180";
   particles = "UnderwaterGrenadeExplosionSmoke";
   blendStyle = "ADDITIVE";
};

datablock ParticleEmitterData(UnderwaterGrenadeExplosionSmokeEmitter)
{
   ejectionPeriodMS = "2";
   periodVarianceMS = "1";

   ejectionVelocity = "10";
   velocityVariance = "6";

   thetaMin = 0.0;
   thetaMax = 90.0;

   lifetimeMS = "200";

   particles = "UnderwaterGrenadeExplosionSmoke";
   blendStyle = "ADDITIVE";
   lifetimeVarianceMS = "100";
};

datablock ExplosionData(UnderwaterHandGrenadeSubExplosion1)
{
   offset = 1.0;
   emitter[0] = UnderwaterGrenadeExplosionSmokeEmitter;
   emitter[1] = UnderwaterGrenadeSparkEmitter;
};

datablock ExplosionData(UnderwaterGrenadeSubExplosion2)
{
   offset = 1.0;
   emitter[0] = UnderwaterGrenadeExplosionSmokeEmitter;
   emitter[1] = UnderwaterGrenadeSparkEmitter;
};

datablock ExplosionData(UnderwaterGrenadeExplosion)
{
   soundProfile = GrenadeExplosionSound;

   emitter[0] = UnderwaterGrenadeExplosionSmokeEmitter;
   emitter[1] = UnderwaterGrenadeSparkEmitter;
   emitter[2] = GrenadeExplosionBubbleEmitter;

   subExplosion[0] = UnderwaterGrenadeSubExplosion1;
   subExplosion[1] = UnderwaterGrenadeSubExplosion2;
   
   shakeCamera = true;
   camShakeFreq = "12.0 13.0 11.0";
   camShakeAmp = "35.0 35.0 35.0";
   camShakeDuration = 1.0;
   camShakeRadius = 10.0;
};

//--------------------------------------------------------------------------
// Projectile Explosion
//--------------------------------------------------------------------------

datablock ParticleData(GrenadeSparks)
{
   textureName          = "art/particles/bigSpark";
   dragCoefficient      = 1;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 350;

   colors[0] = "0.56 0.36 0.26 1.0";
   colors[1] = "0.56 0.36 0.26 1.0";
   colors[2] = "1.0 0.36 0.26 0.0";

   sizes[0] = 0.5;
   sizes[1] = 0.25;
   sizes[2] = 0.25;

   times[0] = 0.0;
   times[1] = 0.5;
   times[2] = 1.0;
};

datablock ParticleEmitterData(GrenadeSparkEmitter)
{
   ejectionPeriodMS = "6";
   periodVarianceMS = 0;
   ejectionVelocity = 18;
   velocityVariance = 6.75;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = 0;
   orientParticles  = true;
   lifetimeMS       = 100;
   particles        = "GrenadeSparks";
   blendStyle = "ADDITIVE";
};

datablock ParticleData(GrenadeExplosionSmoke)
{
   textureName        = "art/particles/smoke";
   dragCoeffiecient   = 105.0;
   gravityCoefficient = -0.0;
   inheritedVelFactor = 0.025;

   constantAcceleration = -0.80;
   
   lifetimeMS         = 1250;
   lifetimeVarianceMS = 0;

   useInvAlpha   = true;
   spinRandomMin = -200.0;
   spinRandomMax =  200.0;

   colors[0] = "1.0 0.7 0.0 0.5";
   colors[1] = "0.2 0.2 0.2 0.7";
   colors[2] = "0.0 0.0 0.0 0.0";

   sizes[0] = 1.0;
   sizes[1] = 3.0;
   sizes[2] = 5.0;

   times[0] = 0.0;
   times[1] = 0.2;
   times[2] = 1.0;
};

datablock ParticleEmitterData(GrenadeExplosionSmokeEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 10.25;
   velocityVariance = 0.25;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles        = "GrenadeExplosionSmoke";
};

datablock ExplosionData(GrenadeSubExplosion1)
{
   offset = 2.0;
   emitter[0] = GrenadeExplosionSmokeEmitter;
   emitter[1] = GrenadeSparkEmitter;
};

datablock ExplosionData(GrenadeSubExplosion2)
{
   offset = 2.0;
   emitter[0] = GrenadeExplosionSmokeEmitter;
   emitter[1] = GrenadeSparkEmitter;
};

datablock ExplosionData(GrenadeExplosion)
{
   soundProfile = GrenadeExplosionSound;

   emitter[0] = GrenadeExplosionSmokeEmitter;
   emitter[1] = GrenadeSparkEmitter;

   subExplosion[0] = GrenadeSubExplosion1;
   subExplosion[1] = GrenadeSubExplosion2;
   
   shakeCamera = true;
   camShakeFreq = "12.0 13.0 11.0";
   camShakeAmp = "35.0 35.0 35.0";
   camShakeDuration = 1.0;
   camShakeRadius = 12.0;

   lightStartRadius = 10;
   lightEndRadius = 5;
   lightStartColor = "0.2 0.2 0.2";
   lightEndColor = "0.5 0.5 0.0";
};

//--------------------------------------------------------------------------
// Projectile Explosion
//--------------------------------------------------------------------------

datablock ParticleData(FragFireball)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "-0.515262";
   inheritedVelFactor = "0";
   constantAcceleration = "0";
   lifetimeMS = "500";
   lifetimeVarianceMS = "300";
   spinSpeed = "0";
   spinRandomMin = "-180";
   spinRandomMax = "180";
   useInvAlpha = "0";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/flameExplosion";
   animTexName[0] = "art/particles/flameExplosion";
   colors[0] = "1 1 1 1";
   colors[1] = "1 1 1 0.531";
   colors[2] = "1 1 1 0";
   colors[3] = "1 1 1 0";
   sizes[0] = "3";
   sizes[1] = "8";
   sizes[2] = "8";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "1";
   times[2] = "1";
   times[3] = "1";
   allowLighting = "0";
   dragCoeffiecient = "0";
};

datablock ParticleEmitterData(FragFireballEmitter)
{
   ejectionPeriodMS = "10";
   periodVarianceMS = "5";
   ejectionVelocity = "3";
   velocityVariance = "2";
   ejectionOffset = "0";
   thetaMin = "0";
   thetaMax = "90";
   phiReferenceVel = "0";
   phiVariance = "360";
   overrideAdvance = "0";
   orientParticles = "0";
   orientOnVelocity = "1";
   particles = "FragFireball";
   lifetimeMS = "0";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
   blendStyle = "ADDITIVE";
};

datablock ParticleData(FragSubFireball)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "-3.01099";
   inheritedVelFactor = "0";
   constantAcceleration = "0";
   lifetimeMS = "288";
   lifetimeVarianceMS = "96";
   spinSpeed = "0";
   spinRandomMin = "-280";
   spinRandomMax = "280";
   useInvAlpha = "0";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/fxpack1/explosion";
   animTexName[0] = "art/particles/fxpack1/explosion";
   colors[0] = "1 0.889764 0.787402 0.195";
   colors[1] = "1 0.496063 0 0.49";
   colors[2] = "0.0866142 0.0866142 0.0866142 0";
   colors[3] = "1 1 1 1";
   sizes[0] = "0.994934";
   sizes[1] = "1.99292";
   sizes[2] = "2.99396";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "0.494118";
   times[2] = "1";
   times[3] = "1";
   allowLighting = "0";
   dragCoeffiecient = "0";
};

datablock ParticleEmitterData(FragSubFireballEmitter)
{
   ejectionPeriodMS = "6";
   periodVarianceMS = "4";
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
   particles = "FragSubFireball";
   lifetimeMS = "0";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
   blendStyle = "ADDITIVE";
};

datablock ParticleData(FragSparks)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "0.5";
   inheritedVelFactor = "0.3";
   constantAcceleration = "0";
   lifetimeMS = "100";
   lifetimeVarianceMS = "60";
   spinSpeed = "0";
   spinRandomMin = "0";
   spinRandomMax = "0";
   useInvAlpha = "0";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/largeSpark";
   animTexName[0] = "art/particles/largeSpark";
   colors[0] = "0.360784 0.227451 0.00392157 0.407";
   colors[1] = "0.498039 0.435294 0.196078 0.606";
   colors[2] = "0.541176 0.521569 0.321569 0.324";
   colors[3] = "0.364706 0.364706 0.364706 0.266";
   sizes[0] = "0.994934";
   sizes[1] = "2.49344";
   sizes[2] = "1.99292";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "0.34902";
   times[2] = "0.708333";
   times[3] = "1";
   allowLighting = "0";
};

datablock ParticleEmitterData(FragSparksEmitter)
{
   ejectionPeriodMS = "2";
   periodVarianceMS = "0";
   ejectionVelocity = "60";
   velocityVariance = "4";
   ejectionOffset = "0";
   thetaMin = "0";
   thetaMax = "180";
   phiReferenceVel = "0";
   phiVariance = "360";
   overrideAdvance = "0";
   orientParticles = "1";
   orientOnVelocity = "1";
   particles = "FragSparks";
   lifetimeMS = "0";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
   blendStyle = "ADDITIVE";
};

datablock ParticleData(FragSmoke)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "-0.105006";
   inheritedVelFactor = "0";
   constantAcceleration = "0";
   lifetimeMS = "2000";
   lifetimeVarianceMS = "700";
   spinSpeed = "0";
   spinRandomMin = "-80";
   spinRandomMax = "80";
   useInvAlpha = "1";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/fxpack1/smoke01";
   animTexName[0] = "art/particles/fxpack1/smoke01";
   colors[0] = "0.996078 0.996078 0.996078 0.324";
   colors[1] = "0.729412 0.729412 0.729412 0.519";
   colors[2] = "0.556863 0.556863 0.556863 0.531";
   colors[3] = "0.160784 0.160784 0.160784 0.299";
   sizes[0] = "2.5";
   sizes[1] = "6";
   sizes[2] = "9";
   sizes[3] = "12";
   times[0] = "0";
   times[1] = "0.3125";
   times[2] = "0.645833";
   times[3] = "1";
   allowLighting = "0";
   dragCoeffiecient = "0";
};

datablock ParticleEmitterData(FragSmokeEmitter)
{
   ejectionPeriodMS = "15";
   periodVarianceMS = "5";
   ejectionVelocity = "2";
   velocityVariance = "1";
   ejectionOffset = "1";
   thetaMin = "0";
   thetaMax = "180";
   phiReferenceVel = "0";
   phiVariance = "360";
   overrideAdvance = "0";
   orientParticles = "0";
   orientOnVelocity = "1";
   particles = "FragSmoke";
   lifetimeMS = "0";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
};

datablock ParticleData(FragSubSmoke)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "-0.407814";
   inheritedVelFactor = "0";
   constantAcceleration = "0";
   lifetimeMS = "2672";
   lifetimeVarianceMS = "192";
   spinSpeed = "0";
   spinRandomMin = "-120";
   spinRandomMax = "120";
   useInvAlpha = "0";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/fxpack1/smoke01.png";
   animTexName[0] = "art/particles/fxpack1/smoke01.png";
   colors[0] = "0.787402 0.685039 0.590551 0.299213";
   colors[1] = "0.889764 0.889764 0.889764 0.787402";
   colors[2] = "0.889764 0.889764 0.889764 0";
   colors[3] = "1 1 1 1";
   sizes[0] = "1.99292";
   sizes[1] = "3.99194";
   sizes[2] = "6.99506";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "0.247059";
   times[2] = "1";
   times[3] = "1";
   allowLighting = "0";
      dragCoeffiecient = "0";
};

datablock ParticleEmitterData(FragSubSmokeEmitter)
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
   particles = "FragSubSmoke";
   lifetimeMS = "0";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
};

datablock ExplosionData(FragSubExplosion1)
{
   faceViewer = "0";
   particleDensity = "10";
   particleRadius = "1";
   explosionScale = "1 1 1";
   playSpeed = "1";
   emitter[0] = "FragSubFireballEmitter";
   debrisThetaMin = "0";
   debrisThetaMax = "90";
   debrisPhiMin = "0";
   debrisPhiMax = "360";
   debrisNum = "1";
   debrisNumVariance = "0";
   debrisVelocity = "2";
   debrisVelocityVariance = "0";
   delayMS = "0";
   delayVariance = "0";
   lifetimeMS = "64";
   lifetimeVariance = "0";
   offset = "0.2";
   times[0] = "0";
   times[1] = "1";
   times[2] = "1";
   times[3] = "1";
   sizes[0] = "1 1 1";
   sizes[1] = "1 1 1";
   sizes[2] = "1 1 1";
   sizes[3] = "1 1 1";
   shakeCamera = "0";
   camShakeFreq = "10 10 10";
   camShakeAmp = "1 1 1";
   camShakeDuration = "1.5";
   camShakeRadius = "10";
   camShakeFalloff = "10";
   lightStartRadius = "0";
   lightEndRadius = "0";
   lightStartColor = "1 1 1 1";
   lightEndColor = "1 1 1 1";
};

datablock ExplosionData(FragSubExplosion2)
{
   faceViewer = "0";
   particleDensity = "10";
   particleRadius = "1";
   explosionScale = "1 1 1";
   playSpeed = "1";
   emitter[0] = "FragSubFireballEmitter";
   emitter[1] = "FragSubSmokeEmitter";
   debrisThetaMin = "0";
   debrisThetaMax = "90";
   debrisPhiMin = "0";
   debrisPhiMax = "360";
   debrisNum = "1";
   debrisNumVariance = "0";
   debrisVelocity = "2";
   debrisVelocityVariance = "0";
   delayMS = "0";
   delayVariance = "0";
   lifetimeMS = "64";
   lifetimeVariance = "0";
   offset = "0.5";
   times[0] = "0";
   times[1] = "1";
   times[2] = "1";
   times[3] = "1";
   sizes[0] = "1 1 1";
   sizes[1] = "1 1 1";
   sizes[2] = "1 1 1";
   sizes[3] = "1 1 1";
   shakeCamera = "0";
   camShakeFreq = "10 10 10";
   camShakeAmp = "1 1 1";
   camShakeDuration = "1.5";
   camShakeRadius = "10";
   camShakeFalloff = "10";
   lightStartRadius = "0";
   lightEndRadius = "0";
   lightStartColor = "1 1 1 1";
   lightEndColor = "1 1 1 1";
};

datablock ParticleData(FragDebrisTrail)
{
   dragCoefficient = "0";
   windCoefficient = "1";
   gravityCoefficient = "-0.00732601";
   inheritedVelFactor = "0";
   constantAcceleration = "0";
   lifetimeMS = "1184";
   lifetimeVarianceMS = "576";
   spinSpeed = "0";
   spinRandomMin = "-180";
   spinRandomMax = "180";
   useInvAlpha = "1";
   animateTexture = "0";
   framesPerSec = "1";
   textureName = "art/particles/fxpack1/smoke01";
   animTexName[0] = "art/particles/fxpack1/smoke01";
   colors[0] = "1 0.889764 0.787402 0.188976";
   colors[1] = "0.787402 0.787402 0.787402 0.299213";
   colors[2] = "0.385827 0.385827 0.385827 0";
   colors[3] = "1 1 1 1";
   sizes[0] = "0.396753";
   sizes[1] = "1.99597";
   sizes[2] = "3.99499";
   sizes[3] = "1";
   times[0] = "0";
   times[1] = "0.498039";
   times[2] = "1";
   times[3] = "2";
   allowLighting = "0";
};

datablock ParticleEmitterData(FragDebrisTrailEmitter)
{
   ejectionPeriodMS = "6";
   periodVarianceMS = "2";
   ejectionVelocity = "1";
   velocityVariance = "0.8";
   ejectionOffset = "0";
   thetaMin = "0";
   thetaMax = "180";
   phiReferenceVel = "0";
   phiVariance = "360";
   overrideAdvance = "0";
   orientParticles = "0";
   orientOnVelocity = "1";
   particles = "FragDebrisTrail";
   lifetimeMS = "0";
   lifetimeVarianceMS = "0";
   useEmitterSizes = "0";
   useEmitterColors = "0";
};

datablock DebrisData(FragDebris)
{
   shapeFile = "art/shapes/weapons/Grenade/grenadeDebris.dts";
   render2D = "0";
   emitters[0] = "FragDebrisTrailEmitter";
   elasticity = "0.6";
   friction = "0.5";
   numBounces = "1";
   bounceVariance = "1";
   minSpinSpeed = "300";
   maxSpinSpeed = "700";
   gravModifier = "0";
   terminalVelocity = "40";
   velocity = "40";
   velocityVariance = "10";
   lifetime = "0.15";
   lifetimeVariance = "0";
   useRadiusMass = "1";
   baseRadius = "0.3";
   explodeOnMaxBounce = "1";
   staticOnMaxBounce = "0";
   snapOnMaxBounce = "0";
   fade = "1";
   ignoreWater = "0";
};

datablock ExplosionData(FragExplosion)
{
   soundProfile = GrenadeLauncherExplosionSound;
   faceViewer = "0";
   particleEmitter = "FragSmokeEmitter";
   particleDensity = "10";
   particleRadius = "0.6";
   explosionScale = "1 1 1";
   playSpeed = "1";
   emitter[0] = "FragFireballEmitter";
   emitter[1] = "FragSubFireballEmitter";
   emitter[2] = "FragSparksEmitter";
   emitter[3] = "FragSparksEmitter";
   Debris = "FragDebris";
   debrisThetaMin = "0";
   debrisThetaMax = "90";
   debrisPhiMin = "0";
   debrisPhiMax = "360";
   debrisNum = "8";
   debrisNumVariance = "2";
   debrisVelocity = "1";
   debrisVelocityVariance = "0.2";
   subExplosion[0] = "FragSubExplosion1";
   subExplosion[1] = "FragSubExplosion2";
   delayMS = "0";
   delayVariance = "0";
   lifetimeMS = "64";
   lifetimeVariance = "0";
   offset = "0";
   times[0] = "0";
   times[1] = "1";
   times[2] = "1";
   times[3] = "1";
   sizes[0] = "1 1 1";
   sizes[1] = "1 1 1";
   sizes[2] = "1 1 1";
   sizes[3] = "1 1 1";
   shakeCamera = "1";
   camShakeFreq = "2 2 2";
   camShakeAmp = "8 8 8";
   camShakeDuration = "1.5";
   camShakeRadius = "10";
   camShakeFalloff = "10";
   lightStartRadius = "0";
   lightEndRadius = "0";
   lightStartColor = "1 1 1 1";
   lightEndColor = "1 1 1 1";
};
