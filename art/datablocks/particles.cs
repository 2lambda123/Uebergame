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

// Smoke

datablock ParticleData(Smoke)
{
   textureName          = "art/particles/smoke";
   dragCoefficient      = 0.3;
   gravityCoefficient   = -0.2;   // rises slowly
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 3000;
   lifetimeVarianceMS   = 250;
   useInvAlpha          = true;
   spinRandomMin        = -30.0;
   spinRandomMax        = 30.0;

   sizes[0]      = 1.5;
   sizes[1]      = 2.75;
   sizes[2]      = 6.5;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(SmokeEmitter)
{
   ejectionPeriodMS = 400;
   periodVarianceMS = 5;

   ejectionVelocity = 0.0;
   velocityVariance = 0.0;

   thetaMin         = 0.0;
   thetaMax         = 90.0;

   particles        = Smoke;
   ambientFactor = "0.5";
   blendStyle = "NORMAL";
};

datablock ParticleEmitterNodeData(SmokeEmitterNode)
{
   timeMultiple = 1;
};

// Ember

datablock ParticleData(EmberParticle)
{
   textureName          = "art/particles/ember";
   dragCoefficient      = 0.0;
   windCoefficient      = 0.0;
   gravityCoefficient   = -0.05;   // rises slowly
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 5000;
   lifetimeVarianceMS   = 0;
   useInvAlpha          = false;
   spinRandomMin        = -90.0;
   spinRandomMax        = 90.0;
   spinSpeed            = 1;

   colors[0]     = "1.000000 0.800000 0.000000 0.800000";
   colors[1]     = "1.000000 0.700000 0.000000 0.800000";
   colors[2]     = "1.000000 0.000000 0.000000 0.200000";

   sizes[0]      = 0.05;
   sizes[1]      = 0.1;
   sizes[2]      = 0.05;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(EmberEmitter)
{
   ejectionPeriodMS = 100;
   periodVarianceMS = 0;

   ejectionVelocity = 0.75;
   velocityVariance = 0.00;
   ejectionOffset   = 2.0;

   thetaMin         = 1.0;
   thetaMax         = 100.0;

   particles        = "EmberParticle";
};

datablock ParticleEmitterNodeData(EmberNode)
{
   timeMultiple = 1;
};

// Fire

datablock ParticleData(FireParticle)
{
   textureName          = "art/particles/smoke";
   dragCoefficient      = 0.0;
   windCoefficient      = 0.0;
   gravityCoefficient   = -0.05;   // rises slowly
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 5000;
   lifetimeVarianceMS   = 1000;
   useInvAlpha          = false;
   spinRandomMin        = -90.0;
   spinRandomMax        = 90.0;
   spinSpeed            = 1.0;

   colors[0]     = "0.2 0.2 0.0 0.2";
   colors[1]     = "0.6 0.2 0.0 0.2";
   colors[2]     = "0.4 0.0 0.0 0.1";
   colors[3]     = "0.1 0.04 0.0 0.3";

   sizes[0]      = 0.5;
   sizes[1]      = 4.0;
   sizes[2]      = 5.0;
   sizes[3]      = 6.0;

   times[0]      = 0.0;
   times[1]      = 0.1;
   times[2]      = 0.2;
   times[3]      = 0.3;
};

datablock ParticleEmitterData(FireEmitter)
{
   ejectionPeriodMS = 50;
   periodVarianceMS = 0;

   ejectionVelocity = 0.55;
   velocityVariance = 0.00;
   ejectionOffset   = 1.0;


   thetaMin         = 1.0;
   thetaMax         = 100.0;

   particles        = "FireParticle";
};

datablock ParticleEmitterNodeData(FireNode)
{
   timeMultiple = 1;
};

// Torch Fire

datablock ParticleData(TorchFire1)
{
   textureName          = "art/particles/smoke";
   dragCoefficient      = 0.0;
   gravityCoefficient   = -0.3;   // rises slowly
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 250;
   useInvAlpha          = false;
   spinRandomMin        = -30.0;
   spinRandomMax        = 30.0;
   spinSpeed            = 1;

   colors[0]     = "0.6 0.6 0.0 0.1";
   colors[1]     = "0.8 0.6 0.0 0.1";
   colors[2]     = "0.0 0.0 0.0 0.1";

   sizes[0]      = 0.5;
   sizes[1]      = 0.5;
   sizes[2]      = 2.4;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleData(TorchFire2)
{
   textureName          = "art/particles/smoke";
   dragCoefficient      = 0.0;
   gravityCoefficient   = -0.5;   // rises slowly
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 150;
   useInvAlpha          = false;
   spinRandomMin        = -30.0;
   spinRandomMax        = 30.0;
   spinSpeed            = 1;

   colors[0]     = "0.8 0.6 0.0 0.1";
   colors[1]     = "0.6 0.6 0.0 0.1";
   colors[2]     = "0.0 0.0 0.0 0.1";

   sizes[0]      = 0.3;
   sizes[1]      = 0.3;
   sizes[2]      = 0.3;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(TorchFireEmitter)
{
   ejectionPeriodMS = 15;
   periodVarianceMS = 5;

   ejectionVelocity = 0.25;
   velocityVariance = 0.10;

   thetaMin         = 0.0;
   thetaMax         = 45.0;

   particles        = "TorchFire1" TAB "TorchFire2";
};

datablock ParticleEmitterNodeData(TorchFireEmitterNode)
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
   colors[0] = "0.574803 0 0.00787402 1";
   colors[1] = "0.992126 0 0.00787402 1";
   colors[2] = "0.322835 0 0.00787402 0.291339";
   sizes[0] = "0.0976622";
   sizes[1] = "0.497467";
   times[1] = "0.498039";
   times[2] = "1";
   inheritedVelFactor = "0.299413";
   sizes[2] = "0.997986";
};

    datablock ParticleEmitterData(bloodBulletDirtSprayEmitter)  
    {   
       dragCoefficient      = "0.646";  
       gravityCoefficient   = "4";  
       lifetimeMS           = "16";  
       lifetimeVarianceMS   = "12";  
       spinRandomMin = -140;  
       spinRandomMax =  140;  
       useInvAlpha   = 1;  
         
       colors[0]     = "0.582677 0 0.00787402 1";  
       colors[1]     = "0.992126 0 0.00787402 1";  
       colors[2]     = "0.322835 0 0.00787402 0.291339";  
      
       sizes[0]      = "0.299091";  
       sizes[1]      = "0.796557";  
       sizes[2]      = "1.19636";  
      
       times[0]      = 0.0;  
       times[1]      = "0.494118";  
       times[2]      = 1.0;  
       constantAcceleration = "-3";
       ejectionPeriodMS = "15";
       periodVarianceMS = "10";
       ejectionVelocity = "1";
       velocityVariance = "1";
       thetaMax = "180";
       orientParticles = "1";
       particles = "bloodBulletDirtSpray";
       ambientFactor = "0.5";
       blendStyle = "NORMAL";
       softParticles = "0";
         
    };  
