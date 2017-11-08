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

datablock ParticleEmitterNodeData(SmokeEmitterNode)
{
   timeMultiple = 1;
};

datablock ParticleData(bloodBulletDirtSpray)
{
   dragCoefficient = "0.498534";
   gravityCoefficient = "0.495727";
   constantAcceleration = "-1";
   lifetimeMS = "300";
   lifetimeVarianceMS = "250";
   spinRandomMin = "-120";
   spinRandomMax = "120";
   useInvAlpha = "1";
   textureName = "art/particles/blood_particle_01";
   animTexName = "art/particles/blood_particle_01";
   colors[0] = "0.566929 0 0.00787402 1";
   colors[1] = "0.992126 0 0.00787402 1";
   colors[2] = "0.322835 0 0.00787402 0.291339";
   sizes[0] = "0.0946103";
   sizes[1] = "0.497467";
   sizes[2] = "0.997986";
   times[0] = "0";
   times[1] = "0.625";
   times[2] = 1;
   inheritedVelFactor = "0.299413";
};

datablock ParticleEmitterData(bloodBulletDirtSprayEmitter)  
{   
   ejectionPeriodMS = "15";
   periodVarianceMS = "10";
   ejectionVelocity = "1";
   velocityVariance = "1";
   thetaMax = "180";
   orientParticles = "1";
   particles = "bloodBulletDirtSpray";
   ambientFactor = "0.8";
   blendStyle = "NORMAL";
   softParticles = "0";     
};  
