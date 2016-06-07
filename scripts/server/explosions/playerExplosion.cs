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

datablock SFXProfile(PlayerHitSound)
{
   fileName = "art/sound/player/hit/player_hit_08";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(PlayerHitSound1)
{
   fileName = "art/sound/player/hit/player_hit_09";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(PlayerHitSound2)
{
   fileName = "art/sound/player/hit/player_hit_10";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile(PlayerHitSound3)
{
   fileName = "art/sound/player/hit/player_hit_11";
   description = AudioClose3D;
   preload = true;
};

datablock SFXPlayList(PlayerHitSoundList)
{
   random = "StrictRandom";
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "AudioClosest3D";
   track[ 0 ] = PlayerHitSound0;
   track[ 1 ] = PlayerHitSound1;
   track[ 2 ] = PlayerHitSound2;
   track[ 3 ] = PlayerHitSound3;
};

//-----------------------------------------------------------------------------
// Particles
//-----------------------------------------------------------------------------

datablock ParticleData(PlayerBloodImpactParticle)
{
   dragCoefficient = "0.99218";
   gravityCoefficient = "-0.2";
   inheritedVelFactor = "0.499022";
   constantAcceleration = "-0.2";
   lifetimeMS = "180";
   lifetimeVarianceMS = "170";
   spinSpeed = "0";
   spinRandomMax = "90";
   useInvAlpha = "1";
   textureName = "art/particles/millsplash01.png";
   animTexName = "art/particles/millsplash01.png";
   colors[0] = "0.535433 0 0.00787402 0.909";
   colors[1] = "0.992126 0 0.00787402 0.701";
   colors[2] = "0.322835 0 0.00787402 0.303";
   colors[3] = "0.580392 0.196078 0.25098 0";
   sizes[0] = "0.2";
   sizes[1] = "0.5";
   sizes[2] = "0.8";
   sizes[3] = "1.2";
   times[1] = "0.33";
   times[2] = "0.75";
};

datablock ParticleEmitterData(PlayerBloodImpactEmitter)
{
   ejectionPeriodMS = "18";
   periodVarianceMS = "15";
   ejectionVelocity = "1";
   velocityVariance = "0.5";
   thetaMin = "120";
   thetaMax = "180";
   ambientFactor = "0.5";
   orientParticles = "1";
   particles = "PlayerBloodImpactParticle";
   lifetimeMS = "150";
   lifetimeVarianceMS = "100";
   blendStyle = "NORMAL";
   softParticles = "0";
};

datablock ParticleData(PlayerBloodSpillParticle)
{ 
   lifetimeMS = "280";
   lifetimeVarianceMS = "270";
   thetaMin = "120";
   dragCoefficient = "1";
   gravityCoefficient = "1";
   inheritedVelFactor = "0.5";
   spinRandomMin = "-120";
   spinRandomMax = "120";
   useInvAlpha = "1";
   textureName = "art/particles/underwaterSpark.png";
   animTexName = "art/particles/underwaterSpark.png";
   colors[0] = "0.543307 0 0.00787402 0";
   colors[1] = "0.619608 0.00392157 0.0470588 0.755";
   colors[2] = "0.592157 0.00392157 0.0745098 0.361";
   colors[3] = "0.596078 0.00392157 0.0588235 0";
   sizes[0] = "0.05";
   sizes[1] = "0.1";
   sizes[2] = "0.12";
   sizes[3] = "0.15";
   times[1] = "0.247059";
   times[2] = "0.705882";
   times[3] = "1";
};

datablock ParticleEmitterData(PlayerBloodSpillEmitter)
{
   ejectionPeriodMS = "8";
   periodVarianceMS = "7";
   ejectionVelocity = "4";
   velocityVariance = "2";
   thetaMin = "145";
   thetaMax = "180";
   orientParticles = "1";
   particles = "PlayerBloodSpillParticle";
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
   softParticles = "0";  
   phiVariance = "90";
   lifetimeMS = "150";
   lifetimeVarianceMS = "140";
   alignDirection = "1 0 0";
};

datablock ParticleData(PlayerBloodDustParticle)
{
   dragCoefficient = "0.99218";
   gravityCoefficient = "0.0952387";
   inheritedVelFactor = "0.199609";
   constantAcceleration = "0.2";
   lifetimeMS = "300";
   lifetimeVarianceMS = "200";
   spinSpeed = "0.2";
   spinRandomMin = "-180";
   spinRandomMax = "180";
   useInvAlpha = "1";
   textureName = "art/particles/impact.png";
   animTexName = "art/particles/impact.png";
   colors[0] = "0.486275 0.392157 0.282353 0.606";
   colors[1] = "0.52549 0.411765 0.262745 0.398";
   colors[2] = "0.482353 0.258824 0.188235 0.183";
   colors[3] = "0.45098 0.34902 0.329412 0";
   sizes[0] = "0.4";
   sizes[1] = "0.8";
   sizes[2] = "1.1";
   sizes[3] = "1.3";
   times[1] = "0.435294";
   times[2] = "0.729167";
};

datablock ParticleEmitterData(PlayerBloodDustEmitter)
{
   ejectionPeriodMS = "7";
   periodVarianceMS = "6";
   ejectionVelocity = "0.5";
   velocityVariance = "0.4";
   thetaMax = "180";
   orientParticles = "1";
   particles = "PlayerBloodDustParticle";
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
   softParticles = "0";   
   lifetimeMS = "200";
   lifetimeVarianceMS = "100";
};

datablock ExplosionData(PlayerBloodExplosion)
{
   soundProfile = PlayerHitSoundList;
   lifeTimeMS = 750; // Quick flash, short burn, and moderate dispersal

   // Volume particles
   particleEmitter = PlayerBloodDustEmitter;
   particleDensity = 20;
   particleRadius = 0.35;

   // Point emission
   emitter[0] = PlayerBloodSpillEmitter;
   emitter[1] = PlayerBloodImpactEmitter;
   
   shakeCamera = true;
   camShakeFreq = "2.5 2.5 1.5";
   camShakeAmp = "8.0 8.0 8.0";
   camShakeDuration = 1.0;
   camShakeRadius = 1.0;
};
