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

datablock SFXProfile(LargeExplosionWaterSound)
{
   filename = "art/sound/weapons/Crossbow_explosion";
   description = AudioDefault3D;
   preload = true;
};

datablock SFXProfile(LargeExplosionSound)
{
   filename = "art/sound/weapons/Crossbow_explosion";
   description = AudioDefault3D;
   preload = true;
};

//-----------------------------------------------------------------------------
// Debris

datablock ParticleData(LargeDebrisSpark)
{
   textureName          = "art/particles/fire";
   dragCoefficient      = 0;
   gravityCoefficient   = 0;
   windCoefficient      = 0;
   inheritedVelFactor   = 0.5;
   constantAcceleration = 0;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 50;
   spinRandomMin        = -90;
   spinRandomMax        =  90;
   useInvAlpha          = false;

   colors[0] = "0.8 0.2 0 1.0";
   colors[1] = "0.8 0.2 0 1.0";
   colors[2] = "0 0 0 0";

   sizes[0]  = 0.2;
   sizes[1]  = 0.3;
   sizes[2]  = 0.1;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(LargeDebrisSparkEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 0;
   ejectionVelocity = 0.5;
   velocityVariance = 0.25;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   orientParticles  = false;
   lifetimeMS       = 300;
   particles        = "LargeDebrisSpark";
   ambientFactor    = "0.2";
};

datablock ExplosionData(LargeDebrisExplosion)
{
   emitter[0] = LargeDebrisSparkEmitter;

   // Turned off..
   shakeCamera      = false;
   impulseRadius    = 0;
   lightStartRadius = 0;
   lightEndRadius   = 0;
};

datablock ParticleData(LargeDebrisFireParticle)
{
   textureName          = "art/particles/fireParticle";
   dragCoeffiecient     = 0;
   gravityCoefficient   = -0.2;
   inheritedVelFactor   = 0;
   constantAcceleration = 0;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = false;
   spinRandomMin        = -160;
   spinRandomMax        = 160;
/*
   animateTexture = true;
   framesPerSec = 11;
   animTexName[0] = "art/shapes/effects/blow01";
   animTexName[1] = "art/shapes/effects/blow02";
   animTexName[2] = "art/shapes/effects/blow03";
   animTexName[3] = "art/shapes/effects/blow04";
   animTexName[4] = "art/shapes/effects/blow05";
   animTexName[5] = "art/shapes/effects/blow06";
   animTexName[6] = "art/shapes/effects/blow07";
   animTexName[7] = "art/shapes/effects/blow08";
   animTexName[8] = "art/shapes/effects/blow09";
   animTexName[9] = "art/shapes/effects/blow10";
   animTexName[10] = "art/shapes/effects/blow11";
*/
   colors[0] = "1.0 0.7 0.5 1.0";
   colors[1] = "1.0 0.5 0.2 0.7";
   colors[2] = "0.1 0.1 0.1 0.0";

   //colors[0] = "1.0 0.9 0.8 0.2";
   //colors[1] = "1.0 0.5 0.0 0.6";
   //colors[2] = "0.1 0.1 0.1 0.0";

   sizes[0]  = 0.5;
   sizes[1]  = 2.0;
   sizes[2]  = 1.0;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(LargeDebrisFireEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 4;
   ejectionVelocity = 2.5; //5
   velocityVariance = 0.5; // 3
   thetaMin         = 0;
   thetaMax         = 90; //180
   phiReferenceVel  = 0;
   phiVariance      = 360;
   ejectionoffset   = 0.3;
   particles        = "LargeDebrisFireParticle";
   ambientFactor    = "0.15";
};

datablock ParticleData(LargeDebrisSmokeParticle)
{
   textureName          = "art/particles/smoke";
   dragCoeffiecient     = 4; //0
   gravityCoefficient   = -0.5; // -1.5
   inheritedVelFactor   = 0.2; //0
   constantAcceleration = 0;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 200;
   useInvAlpha          = true;
   spinRandomMin        = -90;
   spinRandomMax        = 90;

   colors[0] = "0.8 0.8 0.8 0.1";
   colors[1] = "0.5 0.5 0.5 0.4";
   colors[2] = "0.3 0.3 0.3 0.0";

   sizes[0]  = 1.5;
   sizes[1]  = 2.5;
   sizes[2]  = 3.5;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(LargeDebrisSmokeEmitter)
{
   ejectionPeriodMS = 25;
   periodVarianceMS = 5;
   ejectionVelocity = 1; //5
   velocityVariance = 0.5; //3
   thetaMin         = 0; //0
   thetaMax         = 90;//180
   phiReferenceVel  = 0;
   phiVariance      = 360;
   ejectionOffset   = 0; // 0.3
   lifetimeMS       = 2500;
   particles        = "LargeDebrisSmokeParticle";
   ambientFactor    = "0.35";
};

datablock DebrisData(LargeExplosionDebris)
{
   shapeFile = "art/shapes/weapons/Grenade/grenadeDebris.dts";
   emitters[0] = "LargeDebrisFireEmitter";
   //emitters[1] = "LargeDebrisSmokeEmitter";
   explosion = LargeDebrisExplosion;

   elasticity         = 0.6;
   friction           = 0.5;
   numBounces         = 1;
   bounceVariance     = 1;
   explodeOnMaxBounce = true;
   staticOnMaxBounce  = false;
   snapOnMaxBounce    = false;
   minSpinSpeed       = 300;
   maxSpinSpeed       = 600;
   render2D           = false;
   lifetime           = 4.0;
   lifetimeVariance   = 0.4;
   velocity           = 50;
   velocityVariance   = 5;
   fade               = false;
   useRadiusMass      = true;
   baseRadius         = 0.3;
   gravModifier       = 5;
   terminalVelocity   = 0;
   ignoreWater        = false;
};

//-----------------------------------------------------------------------------
// Water Explosion

datablock ParticleData(LargeBubbleParticle)
{
   textureName          = "art/particles/fxpack1/rocks.png";
   dragCoeffiecient     = 0;
   gravityCoefficient   = "0";
   inheritedVelFactor   = 0;
   constantAcceleration = 0;
   lifetimeMS           = "1000";
   lifetimeVarianceMS   = "800";
   useInvAlpha          = false;
   spinRandomMin        = "-90";
   spinRandomMax        = "90";

   colors[0] = "0.503937 0.606299 0.669291 0.755906";
   colors[1] = "0.511811 0.606299 0.685039 0.519685";
   colors[2] = "0.511811 0.606299 0.692913 0.141732";

   sizes[0]  = "1";
   sizes[1]  = "1.5";
   sizes[2]  = "2";

   times[0]  = 0.0;
   times[1]  = 0.4;
   times[2]  = 1.0;
   animTexName = "art/particles/fxpack1/rocks.png";
};

datablock ParticleEmitterData(LargeBubblesEmitter)
{
   ejectionPeriodMS = "5";
   periodVarianceMS = "4";
   ejectionVelocity = "4";
   velocityVariance = "2";
   thetaMin         = 0;
   thetaMax         = 80;
   ejectionOffset   = 4;
   particles        = "LargeBubbleParticle";
   blendStyle = "NORMAL";
   lifetimeMS = "0";
   lifetimeVarianceMS = "0";
   ambientFactor = "0.3";
};

datablock ParticleData(LargeWaterMistParticle)
{
   textureName          = "art/particles/splatter";
   dragCoefficient      = "0.99218";
   gravityCoefficient   = "0.197803";
   inheritedVelFactor   = 1;
   constantAcceleration = 0;
   lifetimeMS           = "1200";
   lifetimeVarianceMS   = "1000";
   spinRandomMin        = -90;
   spinRandomMax        = "90";
   useInvAlpha          = false;

   colors[0] = "0.223529 0.34902 0.478431 0.598425";
   colors[1] = "0.321569 0.470588 0.596078 0.401575";
   colors[2] = "0.494118 0.607843 0.717647 0.299213";

   sizes[0]  = "2.99701";
   sizes[1]  = "5.99707";
   sizes[2]  = "7.99915";

   times[0]  = 0.0;
   times[1]  = "0.329412";
   times[2]  = "0.74902";
   animTexName = "art/particles/splatter";
   colors[3] = "0.596078 0.694118 0.796079 0.108";
   sizes[3] = "11.9972";
};

datablock ParticleEmitterData(LargeWaterMistEmitter)
{
   ejectionPeriodMS = "2";
   periodVarianceMS = "1";
   ejectionVelocity = "20";
   velocityVariance = "10";
   ejectionOffset   = 0;
   thetaMin         = "45";
   thetaMax         = "90";
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = 0;
   lifetimeMS       = "500";
   particles        = "LargeWaterMistParticle";
   blendStyle = "ADDITIVE";
   lifetimeVarianceMS = "250";
   ambientFactor = "0.4";
};

datablock ParticleData(LargeWaterSparkParticle)
{
   textureName          = "art/particles/droplet";
   dragCoeffiecient     = 0.2;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.5;
   constantAcceleration = 0;
   lifetimeMS           = 250;
   lifetimeVarianceMS   = 75;
   useInvAlpha          = false;
   spinRandomMin        = -0;
   spinRandomMax        = 0;

   colors[0] = "0.6 0.6 1.0 1.0";
   colors[1] = "0.6 0.6 1.0 1.0";
   colors[2] = "0.6 0.6 1.0 0.0";

   sizes[0]  = 2.0;
   sizes[1]  = 4.0;
   sizes[2]  = 2.0;

   times[0]  = 0.0;
   times[1]  = 0.35;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(LargeWaterSparksEmitter)
{
   ejectionPeriodMS   = 5;
   periodVarianceMS   = 2;
   ejectionVelocity   = 50;
   velocityVariance   = 10;
   lifetimeMS         = 200;
   lifetimeVarianceMS = 10;
   thetaMin           = 0;
   thetaMax           = 180;
   phiReferenceVel    = 0;
   phiVariance        = 360;
   orientParticles    = true;
   orientOnVelocity   = true;
   particles          = "LargeWaterSparkParticle";
   ambientFactor      = "0.15";
};

datablock ExplosionData(LargeWaterExplosion)
{
   soundProfile = LargeExplosionWaterSound;
   //lifeTimeMS = 1000; // Affects only the emitter array

   particleEmitter = LargeBubblesEmitter;
   particleDensity = 2000;
   particleRadius  = 5;

   emitter[0] = LargeWaterSparksEmitter; 
   emitter[1] = LargeWaterMistEmitter;

   shakeCamera = true;
   camShakeFreq = "8.0 9.0 7.0";
   camShakeAmp = "100.0 100.0 100.0";
   camShakeDuration = 1.3;
   camShakeRadius = 25.0;

   lightStartRadius = 20;
   lightEndRadius = 5;
   lightStartColor = "0.0 0.5 0.5";
   lightEndColor = "0 0 0";
};

//-----------------------------------------------------------------------------
// Land Explosion

datablock ParticleData(LargeSubBlastParticle)
{
   textureName          = "art/particles/smokeBlast";
   dragCoefficient      = 5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.2;
   windCoefficient      = 0;
   constantAcceleration = 0;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 300;
   spinRandomMin        = -20.0;
   spinRandomMax        = 20.0;
   useInvAlpha          = true;

   colors[0] = "1.0 0.9 0.8 0.2";
   colors[1] = "0.9 0.8 0.7 1.0";
   colors[2] = "1.0 1.0 1.0 0.0";

   sizes[0]  = 16.0;
   sizes[1]  = 30.0;
   sizes[2]  = 60.0;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(LargeSubBlastEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 5;
   ejectionVelocity = 105.0;
   velocityVariance = 10;
   ejectionOffset   = 0.4;
   thetaMin         = 0;
   thetaMax         = 55;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   orientParticles  = true;
   orientOnVelocity = true;
   particles        = "LargeSubBlastParticle";
   ambientFactor    = "0.3";
};

datablock ParticleData(LargeSubFireParticle)
{
   textureName        = "art/particles/flameExplosion.png";
   gravityCoefficient = "-1";
   lifetimeMS         = "600";
   lifetimeVarianceMS = "300";
   useInvAlpha        = false;
   spinRandomMin      = -280.0;
   spinRandomMax      = 280.0;

   colors[0] = "1 0.897638 0.795276 0.692913";
   colors[1] = "1 0.496063 0 0.393701";
   colors[2] = "0.299213 0.19685 0.0944882 0";

   sizes[0]  = "4";
   sizes[1]  = "12";
   sizes[2]  = "6";

   times[0]  = 0.0;
   times[1]  = "0.498039";
   times[2]  = 1.0;
   animTexName = "art/particles/flameExplosion.png";
};

datablock ParticleEmitterData(LargeSubFireEmitter)
{
   ejectionPeriodMS = 25;
   periodVarianceMS = 15;
   ejectionVelocity = 15.0;
   velocityVariance = 3.0;
   thetaMin         = 0.0;
   thetaMax         = 150.0;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   ejectionoffset   = 5;
   particles        = "LargeSubFireParticle";
   ambientFactor    = "0.15";
};

datablock ParticleData(LargeSubSmokeParticle)
{
   textureName        = "art/particles/smokeThick";
   gravityCoefficient = "-0.50061";
   lifetimeMS         = 1600;
   lifetimeVarianceMS = "800";
   useInvAlpha        = true;
   spinRandomMin      = -60.0;
   spinRandomMax      = 60.0;

   colors[0] = "1 0.897638 0.795276 0.299213";
   colors[1] = "0.448819 0.425197 0.393701 0.897638";
   colors[2] = "0.0944882 0.0944882 0.0944882 0";

   sizes[0]  = "3.99805";
   sizes[1]  = "19.9994";
   sizes[2]  = "29.9976";

   times[0]  = 0.0;
   times[1]  = "0.34902";
   times[2]  = 1.0;
   animTexName = "art/particles/smokeThick";
};

datablock ParticleEmitterData(LargeSubSmokeEmitter)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 10;
   ejectionVelocity = 8.0;
   velocityVariance = 2.0;
   thetaMin         = 0.0;
   thetaMax         = 90.0;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   ejectionoffset   = 7;
   particles        = "LargeSubSmokeParticle";
   ambientFactor    = "0.4";
};

datablock ParticleData(LargeSubSplatterParticle)
{
   textureName        = "art/particles/splatter";
   gravityCoefficient = "3.99756";
   lifetimeMS         = "1000";
   lifetimeVarianceMS = "500";
   useInvAlpha        = true;
   spinRandomMin      = -60.0;
   spinRandomMax      = 60.0;

   colors[0] = "0.795276 0.692913 0.598425 0";
   colors[1] = "0.299213 0.299213 0.299213 0.496063";
   colors[2] = "0.19685 0.19685 0.19685 0";

   sizes[0]  = "9.99817";
   sizes[1]  = "19.9994";
   sizes[2]  = "29.9976";

   times[0]  = 0.0;
   times[1]  = "0.498039";
   times[2]  = 1.0;
   animTexName = "art/particles/splatter";
};

datablock ParticleEmitterData(LargeSubSplatterEmitter)
{
   ejectionPeriodMS = 15; // 20
   periodVarianceMS = 5;
   ejectionVelocity = 35.0;
   velocityVariance = 10.0;
   thetaMin         = 0.0;
   thetaMax         = 15.0;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   particles        = "LargeSubSplatterParticle";
   ambientFactor    = "0.35";
};

datablock ParticleData(LargeSparkParticle)
{
   textureName          = "art/particles/largeSpark";
   dragCoeffiecient     = 1.0;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 250;
   lifetimeVarianceMS   = 75;
   useInvAlpha          = false;
   spinRandomMin        = -0.0;
   spinRandomMax        = 0.0;

   colors[0] = "1.0 0.9 0.8 0.2";
   colors[1] = "1.0 0.9 0.8 0.6";
   colors[2] = "0.8 0.4 0.0 0.0";

   sizes[0]  = 2.0;
   sizes[1]  = 4.0;
   sizes[2]  = 2.0;

   times[0]  = 0.0;
   times[1]  = 0.35;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(LargeSparkEmitter)
{
   ejectionPeriodMS   = 20;
   periodVarianceMS   = 2;
   ejectionVelocity   = 60;
   velocityVariance   = 10;
   lifetimeMS         = 100;
   lifetimeVarianceMS = 10;
   thetaMin           = 0;
   thetaMax           = 180;
   phiReferenceVel    = 0;
   phiVariance        = 360;
   orientParticles    = true;
   orientOnVelocity   = true;
   particles          = "LargeSparkParticle";
   ambientFactor      = "0.1";
};

datablock ParticleData(LargeSmokeParticle)
{
   textureName          = "art/particles/smokeThick";
   dragCoeffiecient     = 0;
   gravityCoefficient   = -0.2;
   inheritedVelFactor   = 0;
   constantAcceleration = 0;
   lifetimeMS           = 1300;
   lifetimeVarianceMS   = 400;
   useInvAlpha          = true;
   spinRandomMin        = -180.0;
   spinRandomMax        = 180.0;

   colors[0] = "1.0 0.9 0.8 0.9";
   colors[1] = "0.35 0.3 0.25 0.7";
   colors[2] = "0.5 0.45 0.4 0.0";

   sizes[0]  = 8.0;
   sizes[1]  = 16.0;
   sizes[2]  = 24.0;

   times[0]  = 0.0;
   times[1]  = 0.4;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(LargeSmokeEmitter)
{
   ejectionPeriodMS = 25;
   periodVarianceMS = 10;
   ejectionVelocity = 4;
   velocityVariance = 2.0;
   thetaMin         = 120;
   thetaMax         = 180;
   ejectionOffset   = 7;
   particles        = "LargeSmokeParticle";
   ambientFactor    = "0.5";
};

datablock ParticleData(LargeDustParticle)
{
   textureName          = "art/particles/smoke";
   dragCoefficient      = 1;
   gravityCoefficient   = -0.01;
   inheritedVelFactor   = 1;
   constantAcceleration = 0;
   lifetimeMS           = 2000;
   lifetimeVarianceMS   = 250;
   spinRandomMin        = -90;
   spinRandomMax        = 500;
   useInvAlpha          = 1;

   colors[0] = "0.1 0.1 0.1 0.6";
   colors[1] = "0.5 0.5 0.5 0.2";
   colors[2] = "0 0 0 0";
   colors[3] = "0 0 0 0";

   sizes[0]  = 2.5;
   sizes[1]  = 5.0;
   sizes[2]  = 7.0;
   sizes[3]  = 1.25;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 0.75;
   times[3]  = 1.0;
};

datablock ParticleEmitterData(LargeDustEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 30.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 85;
   thetaMax         = 85;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   lifetimeMS       = 500;
   particles        = "LargeDustParticle";
   ambientFactor    = "0.4";
};

datablock ExplosionData(LargeSubExplosion1)
{
   lifeTimeMS = 100;
   offset     = 2;
   emitter[0] = LargeSubFireEmitter;  
   emitter[1] = LargeSubSmokeEmitter;
   //emitter[2] = LargeSubBlastEmitter;
};

datablock ExplosionData(LargeSubExplosion2)
{
   lifeTimeMS = 100;
   offset     = 0;
   emitter[0] = LargeSubSplatterEmitter;
   emitter[1] = LargeSmokeEmitter;
   emitter[2] = LargeSubFireEmitter;
};

datablock ExplosionData(LargeExplosion)
{
   soundProfile = LargeExplosionSound;
   lifeTimeMS = 1000; // Affects only the emitter array

   particleEmitter = LargeSmokeEmitter;
   particleDensity = 20;
   particleRadius  = 5;

   emitter[0] = LargeSubFireEmitter;
   emitter[1] = LargeSparkEmitter; 
   emitter[2] = LargeDustEmitter;

   subExplosion[0] = LargeSubExplosion1;
   subExplosion[1] = LargeSubExplosion2;

   debris = LargeExplosionDebris;
   debrisThetaMin = 0;
   debrisThetaMax = 60;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisNum = 5;
   debrisNumVariance = 1;
   debrisVelocity = 1;
   debrisVelocityVariance = 0.5;

   shakeCamera = true;
   camShakeFreq = "8.0 9.0 7.0";
   camShakeAmp = "100.0 100.0 100.0";
   camShakeDuration = 1.3;
   camShakeRadius = 25.0;

   lightStartRadius = 20;
   lightEndRadius = 5;
   lightStartColor = "0.8 0.4 0";
   lightEndColor = "0 0 0";
};
