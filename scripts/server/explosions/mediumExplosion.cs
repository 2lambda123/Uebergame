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

datablock SFXProfile(MediumExplosionWaterSound)
{
   filename = "art/sound/weapons/Crossbow_explosion";
   description = AudioDefault3D;
   preload = true;
};

datablock SFXProfile(MediumExplosionSound)
{
   filename = "art/sound/weapons/Crossbow_explosion";
   description = AudioDefault3D;
   preload = true;
};

//-----------------------------------------------------------------------------
// Water Explosion

datablock ParticleData(MediumWaterBlastParticle)
{
   textureName          = "art/particles/smokeBlast";
   dragCoefficient      = 5;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.2;
   windCoefficient      = 0;
   constantAcceleration = 0;
   lifetimeMS           = 400;
   lifetimeVarianceMS   = 200;
   spinRandomMin        = -20;
   spinRandomMax        = 20;
   useInvAlpha          = false;

   colors[0] = "1.0 1.0 1.0 0.2";
   colors[1] = "1.0 1.0 1.0 0.1";
   colors[2] = "1.0 1.0 1.0 0.0";

   sizes[0]  = 8.0;
   sizes[1]  = 16.0;
   sizes[2]  = 30.0;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(MediumWaterBlastEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 5;
   ejectionVelocity = 65;
   velocityVariance = 0.5;
   ejectionOffset   = 0.4;
   thetaMin         = 0;
   thetaMax         = 10;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   orientParticles  = true;
   orientOnVelocity = true;
   particles        = "MediumWaterBlastParticle";
};

datablock ParticleData(MediumWaterSprayParticle)
{
   textureName          = "art/particles/splatter";
   dragCoeffiecient     = 0;
   gravityCoefficient   = 7;
   inheritedVelFactor   = 0;
   constantAcceleration = 0;
   lifetimeMS           = 1200;
   lifetimeVarianceMS   = 800;
   useInvAlpha          = true;
   spinRandomMin        = -180;
   spinRandomMax        = 180;
   useInvAlpha          = false;

   colors[0] = "1.0 1.0 1.0 0.5";
   colors[1] = "1.0 1.0 1.0 0.3";
   colors[2] = "1.0 1.0 1.0 0.0";

   sizes[0]  = 5.0;
   sizes[1]  = 10.0;
   sizes[2]  = 15.0;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(MediumWaterSprayEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 2;
   ejectionVelocity = 35;
   velocityVariance = 5;
   thetaMin         = 0;
   thetaMax         = 15;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   particles        = "MediumWaterSprayParticle";
};

datablock ParticleData(MediumWaterDropletParticle)
{
   textureName          = "art/particles/droplet";
   dragCoeffiecient     = 0;
   gravityCoefficient   = 7;
   inheritedVelFactor   = 0;
   constantAcceleration = 0;
   lifetimeMS           = 1200;
   lifetimeVarianceMS   = 800;
   useInvAlpha          = true;
   spinRandomMin        = -180;
   spinRandomMax        = 180;
   useInvAlpha          = false;

   colors[0] = "1.0 1.0 1.0 0.5";
   colors[1] = "1.0 1.0 1.0 0.3";
   colors[2] = "1.0 1.0 1.0 0.0";

   sizes[0]  = 0.75;
   sizes[1]  = 1.0;
   sizes[2]  = 1.5;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(MediumWaterDropletEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 2;
   ejectionVelocity = 25;
   velocityVariance = 5;
   thetaMin         = 10;
   thetaMax         = 40;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   particles        = "MediumWaterDropletParticle";
};

datablock ParticleData(MediumWaterBubbleParticle)
{
   textureName          = "art/particles/bubble";
   dragCoeffiecient     = 0;
   gravityCoefficient   = -0.25;
   inheritedVelFactor   = 0;
   constantAcceleration = 0;
   lifetimeMS           = 2000;
   lifetimeVarianceMS   = 750;
   useInvAlpha          = false;
   spinRandomMin        = -100;
   spinRandomMax        = 100;
   useInvAlpha          = false;

   colors[0] = "1.0 1.0 1.0 0.75";
   colors[1] = "1.0 1.0 1.0 0.4";
   colors[2] = "1.0 1.0 1.0 0.0";

   sizes[0]  = 0.75;
   sizes[1]  = 1.0;
   sizes[2]  = 1.5;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(MediumWaterBubbleEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 1;
   ejectionVelocity = 5;
   velocityVariance = 2;
   thetaMin         = 0;
   thetaMax         = 80;
   particles        = "MediumWaterBubbleParticle";
};

datablock ParticleData(MediumWaterVolumeParticle)
{
   textureName          = "art/particles/smokeParticle";
   dragCoeffiecient     = 105;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.025;
   constantAcceleration = -1;
   lifetimeMS           = 1250;
   lifetimeVarianceMS   = 0;
   useInvAlpha          = false;
   spinRandomMin        = -200;
   spinRandomMax        = 200;
   useInvAlpha          = false;

   colors[0] = "0.4 0.4 1.0 1.0";
   colors[1] = "0.4 0.4 1.0 0.5";
   colors[2] = "0.0 0.0 0.0 0.0";

   sizes[0]  = 4.0;
   sizes[1]  = 8.0;
   sizes[2]  = 12.0;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(MediumWaterVolumeEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 0.25;
   thetaMin         = 0;
   thetaMax         = 180;
   lifetimeMS       = 250;
   particles        = "MediumWaterVolumeParticle";
};

datablock ExplosionData(MediumWaterExplosion)
{
   soundProfile = MediumExplosionWaterSound;
   lifeTimeMS = 250;

   particleEmitter = MediumWaterVolumeEmitter;
   particleDensity = 15;
   particleRadius = 2;

   emitter[0] = MediumWaterBlastEmitter;
   emitter[1] = MediumWaterDropletEmitter;
   emitter[2] = MediumWaterSprayEmitter;
   emitter[3] = MediumWaterBubbleEmitter;

   shakeCamera = true;
   camShakeFreq = "8.0 9.0 7.0";
   camShakeAmp = "100.0 100.0 100.0";
   camShakeDuration = 1.3;
   camShakeRadius = 25.0;

   lightStartRadius = 12;
   lightEndRadius = 6;
   lightStartColor = "0.05 0.075 0.2";
   lightEndColor = "0.1 0.1 0.5";
};

//-----------------------------------------------------------------------------
// Land Explosion

datablock ParticleData(MediumSubExplosionFireParticle)
{
   textureName          = "art/particles/fireParticle";
   dragCoeffiecient     = 0;
   gravityCoefficient   = -5;
   inheritedVelFactor   = 0;
   constantAcceleration = 0;
   lifetimeMS           = 350;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = false;
   spinRandomMin        = -280;
   spinRandomMax        = 280;

   colors[0] = "1.0 0.9 0.8 0.1";
   colors[1] = "1.0 0.5 0.0 0.3";
   colors[2] = "0.1 0.1 0.1 0.0";

   sizes[0]  = 2.0;
   sizes[1]  = 4.0;
   sizes[2]  = 6.0;

   times[0]  = 0.0;
   times[1]  = 0.2;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(MediumSubExplosionFireEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 5;
   ejectionVelocity = 3.5;
   velocityVariance = 2.0;
   thetaMin         = 0.0;
   thetaMax         = 120.0;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   ejectionoffset   = 4;
   particles        = "MediumSubExplosionFireParticle";
};

datablock ParticleData(MediumSubExplosionSmokeParticle)
{
   textureName          = "art/particles/smoke";
   dragCoeffiecient     = 0;
   gravityCoefficient   = -1.5;
   inheritedVelFactor   = 0;
   constantAcceleration = 0;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 200;
   useInvAlpha          = true;
   spinRandomMin        = -60;
   spinRandomMax        = 60;

   colors[0] = "0.8 0.7 0.6 0.2";
   colors[1] = "0.5 0.5 0.5 0.8";
   colors[2] = "0.1 0.1 0.1 0.0";

   sizes[0]  = 3.0;
   sizes[1]  = 12.0;
   sizes[2]  = 20.0;

   times[0]  = 0.0;
   times[1]  = 0.25;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(MediumSubExplosionSmokeEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = 4;
   velocityVariance = 2;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   ejectionoffset   = 3;
   particles        = "MediumSubExplosionSmokeParticle";
};

datablock ParticleData(MediumExplosionSparkParticle)
{
   textureName          = "art/particles/largeSpark";
   gravityCoefficient   = 1.0;
   inheritedVelFactor   = 0.4;
   lifetimeMS           = 200;
   lifetimeVarianceMS   = 50;
   useInvAlpha          = false;
   spinRandomMin        = -10.0;
   spinRandomMax        = 10.0;

   colors[0] = "1.0 0.9 0.8 0.2";
   colors[1] = "1.0 0.9 0.8 0.8";
   colors[2] = "0.8 0.4 0.0 0.0";

   sizes[0]  = 1.0;
   sizes[1]  = 2.0;
   sizes[2]  = 1.0;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(MediumExplosionSparkEmitter)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 2;
   ejectionVelocity = 60;
   velocityVariance = 5;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   orientParticles  = true;
   orientOnVelocity = true;
   particles        = "MediumExplosionSparkParticle";
};

datablock ParticleData(MediumExplosionSmokeParticle)
{
   textureName          = "art/particles/smokeThick";
   dragCoeffiecient     = 0;
   gravityCoefficient   = -0.4;
   inheritedVelFactor   = 0;
   constantAcceleration = 0;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 600;
   useInvAlpha          = true;
   spinRandomMin        = -90;
   spinRandomMax        = 90;

   colors[0] = "0.9 0.8 0.7 0.0";
   colors[1] = "0.2 0.2 0.2 0.8";
   colors[2] = "0.4 0.4 0.4 0.0";

   sizes[0]  = 6.0;
   sizes[1]  = 9.0;
   sizes[2]  = 12.0;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(MediumExplosionSmokeEmitter)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 5;
   ejectionVelocity = 4.8;
   velocityVariance = 2;
   thetaMin         = 0;
   thetaMax         = 180;
   ejectionOffset   = 2;
   particles        = "MediumExplosionSmokeParticle";
};

datablock ParticleData(MediumExplosionFireParticle)
{
   textureName          = "art/particles/fire";
   dragCoeffiecient     = 0;
   gravityCoefficient   = -0.5;
   inheritedVelFactor   = 0;
   constantAcceleration = 0;
   lifetimeMS           = 400;
   lifetimeVarianceMS   = 200;
   useInvAlpha          = false;
   spinRandomMin        = -180;
   spinRandomMax        = 180;

   colors[0] = "1.0 0.9 0.8 0.9";
   colors[1] = "0.8 0.4 0 0.3";
   colors[2] = "0.0 0.0 0.0 0.0";

   sizes[0]  = 4.0;
   sizes[1]  = 10.0;
   sizes[2]  = 8.0;

   times[0]  = 0.0;
   times[1]  = 0.5;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(MediumExplosionFireEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 5;
   ejectionVelocity = 3;
   velocityVariance = 2;
   thetaMin         = 0;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   particles        = "MediumExplosionFireParticle";
};

datablock ExplosionData(MediumSubExplosion1)
{
   lifeTimeMS = 100;
   offset = 1.0;
   emitter[0] = MediumSubExplosionFireEmitter;
};

datablock ExplosionData(MediumSubExplosion2)
{
   lifeTimeMS = 100;
   offset = 1.5;
   emitter[0] = MediumSubExplosionFireEmitter;
   emitter[1] = MediumSubExplosionSmokeEmitter;
};

datablock ExplosionData(MediumExplosion)
{
   soundProfile = MediumExplosionSound;
   lifeTimeMS = 250;

   particleEmitter = MediumExplosionSmokeEmitter;
   particleDensity = 15;
   particleRadius = 2;

   emitter[0] = MediumExplosionFireEmitter; 
   emitter[1] = MediumSubExplosionFireEmitter; 
   emitter[2] = MediumExplosionSparkEmitter;  

   subExplosion[0] = MediumSubExplosion1;
   subExplosion[1] = MediumSubExplosion2;

   shakeCamera = true;
   camShakeFreq = "8.0 9.0 7.0";
   camShakeAmp = "100.0 100.0 100.0";
   camShakeDuration = 1.3;
   camShakeRadius = 25.0;

   lightStartRadius = 12;
   lightEndRadius = 6;
   lightStartColor = "0.8 0.4 0";
   lightEndColor = "0 0 0";
};
