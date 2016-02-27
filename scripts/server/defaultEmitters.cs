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

datablock ParticleEmitterNodeData(DefaultEmitterNodeData)
{
   timeMultiple = 1;
};

datablock ParticleEmitterNodeData(HalfTimeEmitterNode)
{
   timeMultiple = 0.5;
};

datablock ParticleEmitterNodeData(DoubleTimeEmitterNode)
{
   timeMultiple = 2.0;
};

datablock ParticleData(DefaultParticle)
{
   //The texture file  less extension to be used by the particle.
   textureName = "art/editor/defaultParticle";

   //The amount of initial velocity to be substracted from the velocity per second.
   dragCoeffiecient = 0;

   //The amount of gravity acceleration to be aplied on the particle.
   gravityCoefficient = 0;

   //The amount of wind acceleration is added to the particle´s velocity.
   windCoefficient = 0;

   //The amount of velocity to be ported over from the emitter object to the particle.
   inheritedVelFactor = 0;

   //The amount of the initial velocity to be added to the velocity per second.
   constantAcceleration = 0;

   //The time in ms each particle has before it dissapates.
   lifetimeMS = 500;

   //This will make lifetimeMS an value between 620+lifetimeVarianceMS and 620-lifetimeVarianceMS
   lifetimeVarianceMS = 250;

   //This inverses the colors of the texture used by the particle.
   useInvAlpha = 0;

   //The minium angle/second an particle spins if it´s not oriented.
   spinRandomMin = -30.0;

   //The maximum angle/second an particle spins if it´s not oriented.
   spinRandomMax = 30.0;

   spinSpeed = 0;

   animateTexture = 0;
   framesPerSec = 1;
   animTexName = "art/editor/defaultParticle";
   animTexTiling = "0 0";

   //Each particle has 4 key frames.
   //times[i] tells the particle when the key frame happens.
   //Time[2] = 0.5; would make the third key frame start at 1/2 the particles live time.
   //At each key frame the particle has it´s collor and size exactly like the keyframe 
   //values tell it to, inbetween keyframes the collor and size gradually change to match.
   colors[0] = "0.8 0.6 0.0 0.1";
   colors[1] = "0.8 0.6 0.0 0.1";
   colors[2] = "1.0 1.0 1.0 0.0";
   colors[3] = "0.0 0.0 0.0 0.0";

   sizes[0] = 1.0;   // One meter across
   sizes[1] = 2.0;
   sizes[2] = 3.0;
   sizes[3] = 4.0;

   times[0] = 0.0;
   times[1] = 0.5;
   times[2] = 1.0;
   times[3] = 1.5;
};

datablock ParticleEmitterData(DefaultEmitter)
{
   //The amount of time between particle emissions.
   ejectionPeriodMS = 15;

   //The variance of ejectionPeriodMs.
   period = 15;
   periodVarianceMS = 5;

   //Each particle will start out with this amount of velocity in 
   //the direction of it´s emission.
   ejectionVelocity = 0.25;

   //The variance of this velocity.
   velocityVariance = 0.10;

   //Each particle will start this amout of meters away from 
   //the emitter in the direction of it´s emission.
   ejectionOffset = 0;

   //Theta and phi are bassically the angles determining the initial direction of the particle.
   //Assume your a torque player, and you emitt particles in the direction your looking.
   //Theta would be you looking up or down with 0 being totally up and 180 being totally down.
   //Phi would be you looking more left or right.. with 0 being forward, 180 would make you turn to face backward.. and 360 would make you face forward again.
   //The combination of those two will allow you to create certain emittion paterns.. like spheres and circles.
   thetaMin = 0.0; //The minium theta angle to emitter particles at.
   thetaMax = 90.0; //The maximum theta angle to emitter particles at.

   //This value makes your emitter turn it´s ´0´ phi around in time.
   phiReferenceVel = 0;

   //The maximum phi angle to use (minimun would be 0)
   //90 would be front to right
   //180 would be front to right to backward.
   //270 would be front to right to backward to left.
   //360 would be a full circle and thus all directions.
   phiVariance = 360;

   overrideAdvances = 0;

   //This will make the particles face their emission direction instead of the player.
   orientParticles = 0;

   //This along with orientParticles being true will make particles face the direction 
   //they are flying to (including gravity/wind).
   orientOnVelocity = 1;

   lifetimeMS = 0;
   lifetimeVarianceMS = 0;
   useEmitterSizes = 0;
   useEmitterColors = 0;

   //The particle datablock to use for emission.
   particles = "DefaultParticle";

   blendStyle = "ADDITIVE";
   alignParticles = 0;
   alignDirection = "0 1 0";
   sortParticles = 0;
   softParticles = 0;
   softnessDistance = 1;
   highResOnly = 0;
   reverseOrder = 0;
   renderReflection = 1;
};

datablock ParticleData(ParticleMist)
{
   textureName = "art/particles/sickieparticles/mist.png";
   dragCoeffiecient = 0;
   gravityCoefficient = "-0.0854701";
   inheritedVelFactor = 0;
   constantAcceleration = -1;
   lifetimeMS = 2500;
   lifetimeVarianceMS = 0;
   useInvAlpha = 0;
   spinRandomMin = -125;
   spinRandomMax = 125;
   spinSpeed = 0.520833;

   colors[0] = "0.992126 0.992126 0.992126 0.436";
   colors[1] = "0.992126 0.992126 0.992126 0.465";
   colors[2] = "0.992126 0.992126 0.992126 0.668";
   colors[3] = "0.992126 0.992126 0.992126 0.23622";
   
   sizes[0] = 2;
   sizes[1] = 8;
   sizes[2] = 9;
   sizes[3] = "10.5";
   
   times[0] = 0.0;
   times[1] = "0.4";
   times[2] = "0.5";
   times[3] = "0.6";
   
   animTexName = "art/particles/sickieparticles/mist.png";
   
   dragCoefficient = "0.889541";
};

datablock ParticleData(ParticleMainFalls01)
{
   textureName = "art/particles/sickieparticles/main falls 01.png";
   animTexName = "art/particles/sickieparticles/main falls 01.png";
   gravityCoefficient = "0.788767";
   sizes[0] = "5.20662";
   sizes[1] = "14.5822";
   constantAcceleration = "0.416666";
   sizes[2] = "10.4163";
   sizes[3] = "0";
   lifetimeMS = "3001";
   inheritedVelFactor = "0.41683";
   times[1] = "0.329412";
   times[2] = "0.658824";
   colors[0] = "0.235294 0.403922 0.345098 1";
   colors[1] = "0.807843 0.85098 0.87451 1";
};

datablock ParticleData(ParticleMainFalls02)
{
   textureName = "art/particles/sickieparticles/main falls 02.png";
   animTexName = "art/particles/sickieparticles/main falls 02.png";
   gravityCoefficient = "0.788767";
   sizes[0] = "1.99902";
   sizes[1] = "7.99915";
   constantAcceleration = "0.416666";
   sizes[2] = "0";
   sizes[3] = "0";
   lifetimeMS = "2064";
};

datablock ParticleData(ParticleMainFalls03)
{
   textureName = "art/particles/sickieparticles/main falls 03.png";
   animTexName = "art/particles/sickieparticles/main falls 03.png";
   gravityCoefficient = "0.788767";
   sizes[0] = "1.99902";
   sizes[1] = "7.99915";
   constantAcceleration = "0.416666";
   sizes[2] = "0";
   sizes[3] = "0";
   lifetimeMS = "2064";
};

datablock ParticleData(ParticleRockImpactInner01)
{
   textureName = "art/particles/sickieparticles/rock_impact_1_inner.png";
   animTexName = "art/particles/sickieparticles/rock_impact_1_inner.png";
   gravityCoefficient = "0.541667";
   inheritedVelFactor = 1;
   spinSpeed = "0.229167";
   spinRandomMin = "-416";
   spinRandomMax = "541.9";
   colors[2] = "1 1 1 0.124481";
   colors[3] = "1 1 1 0.136929";
   sizes[1] = "3";
   sizes[2] = "3";
   sizes[3] = "3";
   times[1] = "0.25";
   times[2] = "0.6875";
};

datablock ParticleData(ParticleRockImpactTop)
{
   textureName = "art/particles/sickieparticles/top and top spray.png";
   animTexName = "art/particles/sickieparticles/top and top spray.png";
   gravityCoefficient = "0.124542";
   colors[1] = "0.996078 0.996078 0.996078 0.813";
   colors[2] = "1 1 1 0.581";
   colors[3] = "1 1 1 0.015748";
   sizes[1] = "1.99597";
   sizes[2] = "1.99597";
   sizes[3] = "1.99597";
   times[1] = "0.329412";
   times[2] = "0.658824";
   spinRandomMin = "-43";
   spinRandomMax = "42";
   colors[0] = "1 1 1 0";
   sizes[0] = "1.99597";
   times[3] = "0.992157";
};

datablock ParticleData(ParticleWaterDisturbance)
{
   textureName = "art/particles/sickieparticles/ripple.png";
   animTexName = "art/particles/sickieparticles/ripple.png";
   lifetimeMS = "2626";
   lifetimeVarianceMS = "49";
   spinRandomMin = "0";
   spinRandomMax = "5";
   colors[0] = "1 1 1 0.173228";
   colors[1] = "1 1 1 1";
   colors[2] = "1 1 1 0.409449";
   colors[3] = "1 1 1 0.0393701";
   sizes[1] = "10.6543";
   sizes[2] = "15.9952";
   sizes[3] = "18.6565";
   times[1] = "0.247059";
   times[2] = "0.513726";
   times[3] = "1";
   spinSpeed = "0";
   sizes[0] = "5.32564";
};

datablock ParticleEmitterData(EmitterMist)
{
   particles = "ParticleMist";
   blendStyle = "NORMAL";
   ejectionVelocity = "5.11";
   velocityVariance = "0";
   thetaMax = "165";
   softParticles = "1";
   ambientFactor = "0.416667";
   ejectionPeriodMS = "167";
};

datablock ParticleEmitterData(EmitterMainFalls)
{
   particles = "ParticleMainFalls01";
   blendStyle = "NORMAL";
   ejectionVelocity = "3.5";
   velocityVariance = "0";
   thetaMax = "10";
   ejectionPeriodMS = "146";
   softnessDistance = "1";
   orientParticles = "1";
   softParticles = "1";
   lifetimeMS = "0";
   ambientFactor = "0";
};

datablock ParticleEmitterData(EmitterMainFalls02)
{
   particles = "ParticleMainFalls02";
   blendStyle = "NORMAL";
   ejectionVelocity = "3.5";
   velocityVariance = "0";
   thetaMax = "10";
   ejectionPeriodMS = "146";
   softnessDistance = "1";
   orientParticles = "1";
   softParticles = "1";
   lifetimeMS = "0";
   ambientFactor = "0";
};

datablock ParticleEmitterData(EmitterMainFalls03)
{
   particles = "ParticleMainFalls03";
   blendStyle = "NORMAL";
   ejectionVelocity = "3.5";
   velocityVariance = "0";
   thetaMax = "25";
   ejectionPeriodMS = "146";
   softnessDistance = "1";
   orientParticles = "1";
   softParticles = "1";
   lifetimeMS = "0";
   ambientFactor = "0";
};

datablock ParticleEmitterData(EmitterTopSpray)
{
   particles = "ParticleRockImpactTop";
   blendStyle = "NORMAL";
   ejectionVelocity = "1";
   velocityVariance = "0";
   ejectionPeriodMS = "272";
   ambientFactor = "0";
   periodVarianceMS = "0";
   thetaMin = "0";
   thetaMax = "90";
   phiVariance = "360";
   alignParticles = "0";
   softParticles = "0";
};

datablock ParticleEmitterData(EmitterWaterDisturbance)
{
   particles = "ParticleWaterDisturbance";
   blendStyle = "NORMAL";
   thetaMin = "7";
   thetaMax = "180";
   orientParticles = "0";
   ejectionPeriodMS = "646";
   periodVarianceMS = "24";
   ejectionVelocity = "0";
   velocityVariance = "0";
   phiVariance = "1";
   alignParticles = "1";
   softnessDistance = "1";
   ambientFactor = "0";
   softParticles = "0";
   alignDirection = "0 0 1";
};

datablock ParticleEmitterData(EmitterRockImpact)
{
   particles = "ParticleRockImpactInner01";
   blendStyle = "NORMAL";
   ejectionPeriodMS = "167";
   velocityVariance = "0";
   softParticles = "1";
   softnessDistance = "1";
   ambientFactor = "0";
};

datablock ParticleEmitterData(EmitterTop)
{
   particles = "DefaultParticle";
   blendStyle = "NORMAL";
   ejectionPeriodMS = "417";
   ejectionVelocity = "69.05";
   velocityVariance = "59.81";
   ambientFactor = "0";
};

datablock ParticleData(ParticleSplinter)
{
   textureName = "art/particles/splinters.png";
   animTexName = "art/particles/splinters.png";
   lifetimeMS = "1501";
   lifetimeVarianceMS = "0";
   spinRandomMin = "-750";
   spinRandomMax = "666";
   colors[0] = "0.992126 0.992126 0.992126 1";
   colors[1] = "0.992126 0.992126 0.992126 1";
   colors[2] = "0.992126 0.992126 0.992126 1";
   colors[3] = "0.992126 0.992126 0.992126 1";
   sizes[1] = "0.25";
   sizes[2] = "0.25";
   sizes[3] = "0";
   times[1] = "0.247059";
   times[2] = "0.513726";
   times[3] = "0.686275";
   spinSpeed = "1";
   sizes[0] = "0.5";
   gravityCoefficient = "0.542";
   inheritedVelFactor = "0.833";
   constantAcceleration = "0.417";
};

datablock ParticleData(ParticleSteamData)
{
   textureName = "art/particles/smoke2.png";
   animTexName = "art/particles/smoke2.png";
   lifetimeMS = "5000";
   lifetimeVarianceMS = "250";
   spinRandomMin = "-25";
   spinRandomMax = "25";
   colors[0] = "0.992126 0.992126 0.992126 0";
   colors[1] = "0.992126 0.992126 0.992126 0.207";
   colors[2] = "0.992126 0.992126 0.992126 0";
   colors[3] = "0.992126 0.992126 0.992126 1";
   sizes[1] = "33.3303";
   sizes[2] = "31.2489";
   sizes[3] = "0";
   times[1] = "0.498039";
   times[2] = "1";
   times[3] = "1";
   spinSpeed = "0.05";
   sizes[0] = "27.083";
   gravityCoefficient = "0";
   inheritedVelFactor = "0";
   constantAcceleration = "0";
};

datablock ParticleEmitterData(ParticleSteamEmitter)
{
   ejectionPeriodMS = "271";
   periodVarianceMS = "208";
   ejectionVelocity = 0;
   velocityVariance = 0;
   thetaMin         = 0.0;
   thetaMax         = 39;
   lifetimeMS       = 0;
   particles = "ParticleSteamData";
   blendStyle = "NORMAL";
   ejectionOffset = "0";
   softnessDistance = "1";
   softParticles = "1";
   lifetimeVarianceMS = "0";
};

datablock ParticleData(WaterVortexParticle)
{
   textureName = "art/particles/sickieparticles/rock impact top and side spray.png";
   animTexName = "art/particles/sickieparticles/rock impact top and side spray.png";
   lifetimeMS = "8000";
   lifetimeVarianceMS = "4800";
   spinRandomMin = "-100";
   spinRandomMax = "120";
   colors[0] = "0.643137 0.643137 0.643137 0.141";
   colors[1] = "0.913726 0.909804 0.909804 0.763";
   colors[2] = "0.917647 0.913726 0.913726 0.726";
   colors[3] = "0.933333 0.929412 0.929412 0.11811";
   sizes[1] = "8";
   sizes[2] = "4";
   sizes[3] = "2";
   times[1] = "0.25";
   times[2] = "0.5";
   times[3] = "0.8";
   spinSpeed = "0.4";
   sizes[0] = "11";
   constantAcceleration = "-2.5";
   inheritedVelFactor = "1";
};

datablock ParticleEmitterData(WaterVortexEmitter)
{
   particles = "WaterVortexParticle";
   blendStyle = "NORMAL";
   thetaMin = "7";
   thetaMax = "180";
   orientParticles = "0";
   ejectionPeriodMS = "646";
   periodVarianceMS = "24";
   ejectionVelocity = "0";
   velocityVariance = "0";
   phiVariance = "360";
   alignParticles = "0";
   softnessDistance = "1";
   ambientFactor = "0";
   softParticles = "1";
   ejectionOffset = "0";
};
