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

datablock SFXProfile(MineExplosionSound)
{
   filename = "art/sound/weapons/Crossbow_explosion";
   description = AudioDefault3D;
   preload = true;
};

datablock SFXProfile(UnderwaterMineExplosionSound)
{
   filename = "art/sound/weapons/Crossbow_explosion";
   description = AudioDefault3D;
   preload = true;
};

//---------------------------------------------------------------------------
// Underwater Explosion

datablock ParticleData(MineExplosionBubbleParticle)
{
   dragCoefficient      = 0.0;
   gravityCoefficient   = -0.25;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 2000;
   lifetimeVarianceMS   = 750;
   useInvAlpha          = false;
   textureName = "art/particles/bubble";

   spinRandomMin        = -100.0;
   spinRandomMax        =  100.0;

   colors[0]     = "0.7 0.8 1.0 0.0";
   colors[1]     = "0.7 0.8 1.0 0.4";
   colors[2]     = "0.7 0.8 1.0 0.0";

   sizes[0]      = 1.0;
   sizes[1]      = 1.0;
   sizes[2]      = 1.0;

   times[0]      = 0.0;
   times[1]      = 0.3;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(MineExplosionBubbleEmitter)
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 0;
   ejectionVelocity = 1.0;
   ejectionOffset   = 2.0;
   velocityVariance = 0.5;
   thetaMin         = 0;
   thetaMax         = 80;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   particles = "MineExplosionBubbleParticle";
};

datablock ParticleData( UnderwaterMineCrescentParticle )
{
   dragCoefficient      = 2;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = -0.0;
   lifetimeMS           = 600;
   lifetimeVarianceMS   = 000;
   textureName = "art/particles/crescent1";

   colors[0] = "0.5 0.5 1.0 1.0";
   colors[1] = "0.5 0.5 1.0 1.0";
   colors[2] = "0.5 0.5 1.0 0.0";

   sizes[0]      = 0.5;
   sizes[1]      = 1.0;
   sizes[2]      = 2.0;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData( UnderwaterMineCrescentEmitter )
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 5.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 80;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   orientParticles  = true;
   lifetimeMS       = 200;
   particles = "UnderwaterMineCrescentParticle";
};

datablock ParticleData(UnderwaterMineExplosionSmoke)
{
   dragCoeffiecient     = 105.0;
   gravityCoefficient   = -0.0;
   inheritedVelFactor   = 0.025;
   constantAcceleration = -1.0;

   lifetimeMS           = 1200;
   lifetimeVarianceMS   = 00;

   useInvAlpha =  false;
   spinRandomMin = -200.0;
   spinRandomMax =  200.0;

   textureName  = "art/particles/smoke";

   colors[0]     = "0.7 0.7 1.0 1.0";
   colors[1]     = "0.3 0.3 1.0 1.0";
   colors[2]     = "0.0 0.0 1.0 0.0";
   sizes[0]      = 1.0;
   sizes[1]      = 3.0;
   sizes[2]      = 1.0;
   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(UnderwaterMineExplosionSmokeEmitter)
{
   ejectionPeriodMS = 8;
   periodVarianceMS = 0;

   ejectionVelocity = 4.25;
   velocityVariance = 1.25;

   thetaMin         = 0.0;
   thetaMax         = 80.0;

   lifetimeMS       = 250;

   particles = "UnderwaterMineExplosionSmoke";
};

datablock ExplosionData(UnderwaterMineExplosion)
{
   //explosionShape = "art/shapes/disc_explosion.dts";
   playSpeed      = 1.0;
   sizes[0] = "0.4 0.4 0.4";
   sizes[1] = "0.4 0.4 0.4";
   soundProfile   = UnderwaterMineExplosionSound;
   faceViewer     = true;

   emitter[0] = UnderwaterMineExplosionSmokeEmitter;
   emitter[1] = UnderwaterMineCrescentEmitter;
   emitter[2] = MineExplosionBubbleEmitter;

   shakeCamera = true;
   camShakeFreq = "8.0 7.0 9.0";
   camShakeAmp = "50.0 50.0 50.0";
   camShakeDuration = 1.0;
   camShakeRadius = 10.0;
};

//-----------------------------------------------------------------------------
// Explosion

datablock ParticleData(MineCrescent)
{
   textureName = "art/particles/crescent1";
   dragCoefficient = 2;
   gravityCoefficient = 0;
   inheritedVelFactor = 0.2;
   constantAcceleration = -0.0;
   lifetimeMS = 600;
   lifetimeVarianceMS = 0;

   colors[0] = "1.0 0.8 0.2 1.0";
   colors[1] = "1.0 0.4 0.2 1.0";
   colors[2] = "1.0 0.0 0.0 0.0";

   sizes[0] = 0.5;
   sizes[1] = 1;
   sizes[2] = 2;

   times[0] = 0;
   times[1] = 0.5;
   times[2] = 1;
};

datablock ParticleEmitterData(MineCrescentEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 5.0;
   ejectionOffset= 0.0;
   thetaMin = 0;
   thetaMax = 80;
   phiReferenceVel = 0;
   phiVariance = 360;
   overrideAdvances = false;
   orientParticles = true;
   lifetimeMS = 200;
   particles = "MineCrescent";
};

datablock ParticleData(MineExpSmoke)
{
   textureName = "art/particles/smokeParticle";
   dragCoeffiecient = 105;
   gravityCoefficient = -0.0;
   inheritedVelFactor = 0.025;
   lifetimeMS = 1200;
   lifetimeVarianceMS = 0;
   useInvAlpha = true;
   spinRandomMin = -200;
   spinRandomMax =  200;

   colors[0] = "1.0 0.7 0.0 1.0";
   colors[1] = "0.2 0.2 0.2 1.0";
   colors[2] = "0.0 0.0 0.0 0.0";

   sizes[0] = 1;
   sizes[1] = 3;
   sizes[2] = 1;

   times[0] = 0;
   times[1] = 0.5;
   times[2] = 1;
};

datablock ParticleEmitterData(MineExpSmokeEmitter)
{
   ejectionPeriodMS = 8;
   periodVarianceMS = 0;
   ejectionVelocity = 4.25;
   velocityVariance = 1.25;
   thetaMin = 0.0;
   thetaMax = 80.0;
   lifetimeMS = 250;
   particles = "MineExpSmoke";
};

datablock ExplosionData(MineExplosion)
{
   //explosionShape = "art/shapes/effect_plasma_explosion.dts";
   playSpeed      = 1.0;

   sizes[0] = "0.5 0.5 0.5";
   sizes[1] = "0.5 0.5 0.5";
   soundProfile = MineExplosionSound;
   faceViewer = true;

   emitter[0] = MineExpSmokeEmitter;
   emitter[1] = MineCrescentEmitter;

   shakeCamera = true;
   camShakeFreq = "8.0 7.0 9.0";
   camShakeAmp = "50.0 50.0 50.0";
   camShakeDuration = 1.0;
   camShakeRadius = 20.0;
};