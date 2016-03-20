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
// VEHICLE EFFECTS
// Explosions, sound efx, trail emitters etc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// VEHICLE DUST - For Hover vehicles

datablock ParticleData(VehicleDust)
{
   textureName = "art/particles/dustParticle";
   dragCoefficient = 1;
   gravityCoefficient = -0.01;
   inheritedVelFactor = 0;
   constantAcceleration = 0;
   lifetimeMS = 2250;
   lifetimeVarianceMS = 100;
   useInvAlpha = true;
   spinRandomMin = -90;
   spinRandomMax = 500;

   colors[0] = "0.46 0.36 0.26 0.2";
   colors[1] = "0.46 0.46 0.36 0.4";
   colors[2] = "0.46 0.46 0.36 0.0";

   sizes[0] = 2;
   sizes[1] = 4;
   sizes[2] = 6;

   times[0] = 0;
   times[1] = 0.5;
   times[2] = 1;
};

datablock ParticleEmitterData(VehicleDustEmitter)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 0;
   ejectionVelocity = 15;
   velocityVariance = 2;
   ejectionOffset = 0;
   thetaMin = 85;
   thetaMax = 90;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   useEmitterColors = 1;
   particles = "VehicleDust";
};

//-----------------------------------------------------------------------------
// VEHICLE JET - For Hover vehicles

datablock ParticleData(JetParticle)
{
   textureName = "art/particles/smokeParticle";
   dragCoefficient = 1.5;
   gravityCoefficient = 0;
   inheritedVelFactor = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS = 200;
   lifetimeVarianceMS = 0;

   colors[0] = "0.5 0.2 0.0 1.0";
   colors[1] = "0.5 0.1 0.0 0.5";
   //colors[0] = "0.9 0.7 0.3 0.6";
   //colors[1] = "0.3 0.3 0.5 0";

   sizes[0] = 2;
   sizes[1] = 6;
};

datablock ParticleEmitterData(JetEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 20;
   velocityVariance = 1.0;
   ejectionOffset = 0.0;
   thetaMin = 0;
   thetaMax = 10;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   particles = "JetParticle";
};

//-----------------------------------------------------------------------------
// VEHICLE CONTRAILS 

datablock ParticleData(ContrailParticle)
{
   textureName = "art/particles/dustParticle";
   dragCoefficient = 1.5;
   gravityCoefficient = 0;
   inheritedVelFactor = 0.2;
   constantAcceleration = 0;
   lifetimeMS = 3000;
   lifetimeVarianceMS = 0;
   colors[0] = "0.46 0.36 0.26 0.5";
   colors[1] = "0.46 0.36 0.26 0";
   sizes[0] = 0.6;
   sizes[1] = 5;
};

datablock ParticleEmitterData(ContrailEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 1;
   velocityVariance = 1.0;
   ejectionOffset = 0;
   thetaMin = 0;
   thetaMax = 10;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   particles = "ContrailParticle";
   blendStyle = "ADDITIVE";
};

//-----------------------------------------------------------------------------
//TIRE TRAILS

datablock ParticleData(TireParticle)
{
   textureName = "art/particles/dustParticle";
   dragCoefficient = 2.0;
   gravityCoefficient = -0.1;
   inheritedVelFactor = 0.1;
   constantAcceleration = 0.0;
   lifetimeMS = 1000;
   lifetimeVarianceMS = 400;

   colors[0] = "0.46 0.36 0.26 1.0";
   colors[1] = "0.46 0.46 0.36 0.0";

   sizes[0] = 1;
   sizes[1] = 1.5;
};

datablock ParticleEmitterData(TireEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 1;
   velocityVariance = 1.0;
   ejectionOffset = 0.0;
   thetaMin = 5;
   thetaMax = 20;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   particles = "TireParticle";
   blendStyle = "ADDITIVE";
};

//-----------------------------------------------------------------------------
// WATER SPLASH EFFECTS

datablock ParticleData(VehicleSplashParticle)
{
   textureName = "art/particles/splash";
   dragCoefficient = 2.0;
   gravityCoefficient = -0.05;
   inheritedVelFactor = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS = 1200;
   lifetimeVarianceMS = 400;
   useInvAlpha = false;
   spinRandomMin = -90.0;
   spinRandomMax = 500.0;
   colors[0] = "0.7 0.8 1.0 1.0";
   colors[1] = "0.7 0.8 1.0 0.5";
   colors[2] = "0.7 0.8 1.0 0.0";
   sizes[0] = 2;
   sizes[1] = 4;
   sizes[2] = 6;
   times[0] = 0.0;
   times[1] = 0.5;
   times[2] = 1.0;
};

datablock ParticleEmitterData(VehicleSplashEmitter)
{
   ejectionPeriodMS = 40;
   periodVarianceMS = 0;
   ejectionVelocity = 10.0;
   velocityVariance = 1.0;
   ejectionOffset = 0.0;
   thetaMin = 85;
   thetaMax = 85;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   particles = "VehicleSplashParticle";
};

datablock ParticleData(VehicleSplashDropletsParticle)
{
   textureName = "art/particles/droplet";
   dragCoefficient = 1;
   gravityCoefficient = 0.2;
   inheritedVelFactor = 0.2;
   constantAcceleration = -0.0;
   lifetimeMS = 800;
   lifetimeVarianceMS = 300;
   colors[0] = "0.7 0.8 1.0 1.0";
   colors[1] = "0.7 0.8 1.0 0.5";
   colors[2] = "0.7 0.8 1.0 0.0";
   sizes[0]= 8;
   sizes[1] = 3;
   sizes[2] = 0;
   times[0] = 0;
   times[1] = 0.5;
   times[2] = 1;
};

datablock ParticleEmitterData(VehicleSplashDropletsEmitter)
{
   ejectionPeriodMS = 34;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 5.0;
   ejectionOffset = 0.0;
   thetaMin = 60;
   thetaMax = 80;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   orientParticles = true;
   particles = "VehicleSplashDropletsParticle";
};

//-----------------------------------------------------------------------------
// WATER DAMAGE BUBBLES

datablock ParticleData(DamageBubbleParticle)
{
   textureName = "art/particles/bubble";
   dragCoefficient = 0.0;
   gravityCoefficient = "-0.041514";
   inheritedVelFactor = "0.499022";
   constantAcceleration = 0.0;
   lifetimeMS = "1000";
   lifetimeVarianceMS = "400";
   useInvAlpha = false;
   spinRandomMin = -90.0;
   spinRandomMax = 90.0;
   colors[0] = "0.692913 0.692913 0.692913 0";
   colors[1] = "0.299213 0.299213 0.299213 1";
   colors[2] = "0.0 0.0 0.0 0.0";
   sizes[0] = "0.15";
   sizes[1] = "0.25";
   sizes[2] = "0.4";
   times[0] = 0.0;
   times[1] = "0.498039";
   times[2] = 1.0;
   animTexName = "art/particles/bubble";
};

datablock ParticleEmitterData(DamageBubbles)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 0;
   ejectionVelocity = 3.0;
   velocityVariance = 0.0;
   ejectionOffset = 1.5;
   thetaMin = 0;
   thetaMax = 35;
   overrideAdvances = false;
   particles = "DamageBubbleParticle";
};

//-----------------------------------------------------------------------------
//HEAVY DAMAGE SMOKE

datablock ParticleData(HeavyDamageSmokeParticle)
{
   dragCoefficient = 0;
   gravityCoefficient = -0.01;
   inheritedVelFactor = 0.5;
   constantAcceleration = 0;
   lifetimeMS = 1500;
   lifetimeVarianceMS = 200;
   useInvAlpha = true;
   spinRandomMin = -90;
   spinRandomMax = 90;
   textureName = "art/particles/smoke";
   colors[0] = "1.0 0.75 0.2 1.0";
   colors[1] = "0.3 0.3 0.3 0.7";
   colors[2] = "0.0 0.0 0.0 0.0";
   sizes[0] = 0.8;
   sizes[1] = 1.6;
   sizes[2] = 2.4;
   times[0] = 0;
   times[1] = 0.5;
   times[2] = 1;
};

datablock ParticleEmitterData(HeavyDamageSmoke)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 6;
   ejectionVelocity = 2.5;
   velocityVariance = 1.5;
   ejectionOffset = 1.5;
   thetaMin = 0;
   thetaMax = 25;
   overrideAdvances = false;
   particles = "HeavyDamageSmokeParticle";
};

//-----------------------------------------------------------------------------
//LIGHT DAMAGE SMOKE

datablock ParticleData(LightDamageSmokeParticle)
{
   textureName = "art/particles/smoke";
   dragCoefficient = 2.0;
   gravityCoefficient = -0.1;
   inheritedVelFactor = 0.1;
   constantAcceleration = 0.0;
   lifetimeMS = 1000;
   lifetimeVarianceMS = 0;
   colors[0] = "0.46 0.46 0.46 1.0";
   colors[1] = "0.16 0.16 0.16 0.0";
   sizes[0] = 1.0;
   sizes[1] = 2.0;
};

datablock ParticleEmitterData(LightDamageSmoke)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 6;
   ejectionVelocity = 4;
   velocityVariance = 1.5;
   ejectionOffset = 1.5;
   thetaMin = 5;
   thetaMax = 60;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   particles = "LightDamageSmokeParticle";
};

//-----------------------------------------------------------------------------
// VEHICLE DEBRIS

datablock ParticleData(VehicleDebrisFireParticle)
{
   //textureName = "art/particles/fireParticle";
   dragCoeffiecient = 0;
   gravityCoefficient = -0.2;
   inheritedVelFactor = 0;
   lifetimeMS = 350;
   lifetimeVarianceMS = 0;
   useInvAlpha = false;
   spinRandomMin = -160;
   spinRandomMax = 160;
/*
   animateTexture = true;
   framesPerSec = 15;

   animTexName[0] = "common/art/shapes/effects/blow01";
   animTexName[1] = "common/art/shapes/effects/blow02";
   animTexName[2] = "common/art/shapes/effects/blow03";
   animTexName[3] = "common/art/shapes/effects/blow04";
   animTexName[4] = "common/art/shapes/effects/blow05";
   animTexName[5] = "common/art/shapes/effects/blow06";
   animTexName[6] = "common/art/shapes/effects/blow07";
   animTexName[7] = "common/art/shapes/effects/blow08";
   animTexName[8] = "common/art/shapes/effects/blow09";
   animTexName[9] = "common/art/shapes/effects/blow10";
   animTexName[10] = "common/art/shapes/effects/blow11";
*/
   colors[0] = "1.0 0.7 0.5 1.0";
   colors[1] = "1.0 0.5 0.2 1.0";
   colors[2] = "1.0 0.25 0.1 0.0";

   sizes[0] = 0.5;
   sizes[1] = 2;
   sizes[2] = 1;

   times[0] = 0;
   times[1] = 0.2;
   times[2] = 1;
};

datablock ParticleEmitterData(VehicleDebrisFireEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 1;
   ejectionVelocity = 0.25;
   velocityVariance = 0;
   thetaMin = 0;
   thetaMax = 30;
   particles = "VehicleDebrisFireParticle";
};

datablock ParticleData(VehicleDebrisSmokeParticle)
{
   textureName = "art/particles/smoke";
   dragCoeffiecient = 4;
   gravityCoefficient = -0.00;
   inheritedVelFactor = 0.2;
   lifetimeMS = 1000;  
   lifetimeVarianceMS = 100;
   useInvAlpha = true;
   spinRandomMin = -50;
   spinRandomMax = 50;

   colors[0] = "0.3 0.3 0.3 0.0";
   colors[1] = "0.3 0.3 0.3 1.0";
   colors[2] = "0.0 0.0 0.0 0.0";

   sizes[0] = 2;
   sizes[1] = 3;
   sizes[2] = 5;

   times[0] = 0;
   times[1] = 0.7;
   times[2] = 1;
};

datablock ParticleEmitterData(VehicleDebrisSmokeEmitter)
{
   ejectionPeriodMS = 25;
   periodVarianceMS = 5;
   ejectionVelocity = 1;
   velocityVariance = 0.5;
   thetaMin = 10;
   thetaMax = 30;
   useEmitterSizes = true;
   particles = "VehicleDebrisSmokeParticle";
};

datablock DebrisData(VehicleDebris)
{
   shapeFile = "art/shapes/weapons/grenade/grenadeDebris.dts";
   emitters[0] = VehicleDebrisSmokeEmitter;
   emitters[1] = VehicleDebrisFireEmitter;
   explosion = ShapeDebrisExplosion; // scripts/server/staticshapes/staticShape.cs

   elasticity = 0.6;
   friction = 0.5;
   numBounces = 1;
   bounceVariance = 1;
   explodeOnMaxBounce = true;
   staticOnMaxBounce = false;
   snapOnMaxBounce = false;
   minSpinSpeed = 0;
   maxSpinSpeed = 500;
   render2D = false;
   lifetime = 100;
   lifetimeVariance = 30;
   velocity = 50;
   velocityVariance = 0.5;
   fade = false;
   useRadiusMass = true;
   baseRadius = 0.3;
   gravModifier = 0.5;
   terminalVelocity = 6;
   ignoreWater = true;
};

//-----------------------------------------------------------------------------
// VEHICLE EXPLOSION

datablock ParticleData(VehicleSubFireballParticle)
{
   textureName          = "art/particles/fireParticle";
   gravityCoefficient   = -4;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 200;
   useInvAlpha =  false;
   spinRandomMin = -280.0;
   spinRandomMax =  280.0;

   colors[0]     = "1.0 0.9 0.8 0.1";
   colors[1]     = "1.0 0.5 0.0 0.3";
   colors[2]     = "0.1 0.1 0.1 0.0";

   sizes[0]      = 8.0;
   sizes[1]      = 16.0;
   sizes[2]      = 12.0;

   times[0]      = 0.0;
   times[1]      = 0.35;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(VehicleSubFireballEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 5;
   ejectionVelocity = 5.0;
   velocityVariance = 3.0;
   thetaMin         = 0.0;
   thetaMax         = 120.0;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   ejectionoffset   = 8;
   particles = VehicleSubFireballParticle;
};

datablock ParticleData(VehicleSubSmokeParticle)
{
   textureName          = "art/particles/smoke";
   dragCoeffiecient     = 0.0;
   gravityCoefficient   = -1.5;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 1200;
   lifetimeVarianceMS   = 400;
   useInvAlpha =  true;
   spinRandomMin = -60.0;
   spinRandomMax =  60.0;

   colors[0]     = "0.4 0.3 0.2 0.2";
   colors[1]     = "0.5 0.5 0.5 0.8";
   colors[2]     = "0.1 0.1 0.1 0.0";

   sizes[0]      = 4.0;
   sizes[1]      = 16.0;
   sizes[2]      = 24.0;

   times[0]      = 0.0;
   times[1]      = 0.25;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(VehicleSubSmokeEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = 6.0;
   velocityVariance = 3.0;
   thetaMin         = 0.0;
   thetaMax         = 120.0;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   ejectionoffset   = 7;
   particles = VehicleSubSmokeParticle;
};

datablock ParticleData(VehicleExplosionSparkParticle)
{
   textureName          = "art/particles/spark";
   dragCoeffiecient     = 0.0;
   gravityCoefficient   = 1.0;
   inheritedVelFactor   = 0.4;
   constantAcceleration = 0.0;
   lifetimeMS           = 200;
   lifetimeVarianceMS   = 100;
   useInvAlpha =  false;
   spinRandomMin = -0.0;
   spinRandomMax =  0.0;

   colors[0]     = "1.0 0.9 0.8 0.0";
   colors[1]     = "1.0 0.9 0.8 0.8";
   colors[2]     = "0.8 0.4 0.0 0.0";

   sizes[0]      = 2.0;
   sizes[1]      = 7.0;
   sizes[2]      = 2.0;

   times[0]      = 0.0;
   times[1]      = 0.35;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(VehicleExplosionSparkEmitter)
{
   ejectionPeriodMS = 4;
   periodVarianceMS = 2;
   ejectionVelocity = 90;
   velocityVariance = 4;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   orientParticles  = true;
   orientOnVelocity = true;
   particles = VehicleExplosionSparkParticle;
};

datablock ParticleData(VehicleExplosionSmokeParticle)
{
   textureName          = "art/particles/smoke";
   dragCoeffiecient     = 0.0;
   gravityCoefficient   = -0.4;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 1200;
   lifetimeVarianceMS   = 400;
   useInvAlpha =  true;
   spinRandomMin = -80.0;
   spinRandomMax =  80.0;

   colors[0]     = "0.4 0.3 0.2 0.3";
   colors[1]     = "0.2 0.2 0.2 1.0";
   colors[2]     = "0.4 0.4 0.4 0.0";

   sizes[0]      = 12.0;
   sizes[1]      = 24.0;
   sizes[2]      = 36.0;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(VehicleExplosionSmokeEmitter)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 5;
   ejectionVelocity = 4.8;
   velocityVariance = 2.0;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   ejectionOffset   = 3;
   particles = VehicleExplosionSmokeParticle;
};

datablock ParticleData(FireballParticle)
{
   textureName          = "art/particles/fireParticle";
   dragCoeffiecient     = 0.0;
   gravityCoefficient   = -0.5;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 200;
   useInvAlpha =  false;
   spinRandomMin = -180.0;
   spinRandomMax =  180.0;

   colors[0]     = "1.0 0.9 0.8 0.9";
   colors[1]     = "0.8 0.4 0 0.3";
   colors[2]     = "0.0 0.0 0.0 0.0";

   sizes[0]      = 15.0;
   sizes[1]      = 30.0;
   sizes[2]      = 10.0;

   times[0]      = 0.0;
   times[1]      = 0.35;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(VehicleExplosionFireballEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 5;
   ejectionVelocity = 3;
   velocityVariance = 2;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   lifetimeMS       = 250;
   particles = FireballParticle;
};

datablock ExplosionData(VehicleSubExplosion1)
{
   lifeTimeMS = 100;
   offset = 1;
   emitter[0] = VehicleSubFireballEmitter;
   emitter[1] = VehicleSubSmokeEmitter; 
};

datablock ExplosionData(VehicleSubExplosion2)
{
   lifeTimeMS = 100;
   offset = 3;
   emitter[0] = VehicleSubFireballEmitter;
   emitter[1] = VehicleSubSmokeEmitter;
};

datablock ExplosionData(VehicleExplosion)
{
   //soundProfile = VehicleExplosionSound;
   lifeTimeMS = 100;

   particleEmitter = VehicleExplosionSmokeEmitter;
   particleDensity = 10;
   particleRadius = 1;

   emitter[0] = VehicleExplosionFireballEmitter; 
   emitter[1] = VehicleSubFireballEmitter; 
   emitter[2] = VehicleExplosionSparkEmitter; 
   emitter[3] = VehicleExplosionSparkEmitter;

   subExplosion[0] = VehicleSubExplosion1;
   subExplosion[1] = VehicleSubExplosion2;

   debris = VehicleDebris;
   debrisThetaMin = 0;
   debrisThetaMax = 60;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisNum = 4;
   debrisNumVariance = 1;
   debrisVelocity = 1;
   debrisVelocityVariance = 0.5;

   shakeCamera = true;
   camShakeFreq = "11.0 13.0 9.0";
   camShakeAmp = "40.0 40.0 40.0";
   camShakeDuration = 0.7;
   camShakeRadius = 25.0;

   lightStartRadius = 10;
   lightEndRadius = 5;
   lightStartColor = "0.5 0.5 0";
   lightEndColor = "0 0 0";
};
