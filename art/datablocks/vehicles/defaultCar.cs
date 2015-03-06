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

// Information extacted from the shape.
//
// Wheel Sequences
//    spring#        Wheel spring motion: time 0 = wheel fully extended,
//                   the hub must be displaced, but not directly animated
//                   as it will be rotated in code.
// Other Sequences
//    steering       Wheel steering: time 0 = full right, 0.5 = center
//    breakLight     Break light, time 0 = off, 1 = breaking
//
// Wheel Nodes
//    hub#           Wheel hub, the hub must be in it's upper position
//                   from which the springs are mounted.
//
// The steering and animation sequences are optional.
// The center of the shape acts as the center of mass for the car.

//-----------------------------------------------------------------------------
/*
datablock SFXProfile(buggyEngineSound)
{
   filename    = "art/sound/vehicles/buggy/engine_idle";
   description = AudioClosestLooping3d;
   preload = true;
};
*/

datablock ParticleData(TireParticle)
{
   dragCoefficient = "1.99413";
   gravityCoefficient = "1";
   inheritedVelFactor = "1";
   lifetimeMS = "938";
   lifetimeVarianceMS = "562";
   textureName = "art/shapes/vehicles/buggy/dustParticle";
   animTexName = "art/shapes/vehicles/buggy/dustParticle";
   colors[0] = "0.0901961 0.0627451 0.0313726 0.502";
   colors[1] = "0.286275 0.207843 0.12549 0.415";
   sizes[0] = "3.12214";
   sizes[1] = "7.29109";
   sizes[2] = "5.20662";
   sizes[3] = "13.5415";
   colors[2] = "0.27451 0.2 0.117647 0.203";
   colors[3] = "0.278431 0.207843 0.113725 0.137";
   times[1] = "0.329412";
   times[2] = "0.658824";
   
};

datablock ParticleEmitterData(TireEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = "14.58";
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 60;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "TireParticle";
   blendStyle = "ADDITIVE";
};


//----------------------------------------------------------------------------

datablock WheeledVehicleTire(DefaultCarTire)
{
   // Tires act as springs and generate lateral and longitudinal
   // forces to move the vehicle. These distortion/spring forces
   // are what convert wheel angular velocity into forces that
   // act on the rigid body.
   shapeFile = "art/shapes/vehicles/buggy/wheel.dts";
   staticFriction = "2";
   kineticFriction = "2";

   // Spring that generates lateral tire forces
   lateralForce = "48000";
   lateralDamping = 6000;
   lateralRelaxation = 1;

   // Spring that generates longitudinal tire forces
   longitudinalForce = 18000;
   longitudinalDamping = 4000;
   longitudinalRelaxation = 1;
   radius = "0.6022";
};

datablock WheeledVehicleSpring(DefaultCarSpring)
{
   // Wheel suspension properties
   length = 0.85;             // Suspension travel
   force = "10000";              // Spring force
   damping = "6000";             // Spring damping
   antiSwayForce = 3;         // Lateral anti-sway force
};

datablock WheeledVehicleData(DefaultCar)
{
   category = "Vehicles";
   shapeFile = "art/shapes/vehicles/buggy/buggy.dts";
   emap = 1;

   mountPose[0] = sitting;
   numMountPoints = 1;

   maxSteeringAngle = "0.45";  // Maximum steering angle, should match animation
   tireEmitter = TireEmitter; // All the tires use the same dust emitter

   // 3rd person camera settings
   cameraRoll = true;         // Roll the camera with the vehicle
   cameraMaxDist = "8";         // Far distance from vehicle
   cameraOffset = "2";        // Vertical offset from camera mount point
   cameraLag = 0.26;           // Velocity lag of camera
   cameraDecay = 1.25;        // Decay per sec. rate of velocity lag

   // Rigid Body
   mass = "1200";
   massCenter = "0 -0.2 0";    // Center of mass for rigid body
   massBox = "0 0 0";         // Size of box used for moment of inertia,
                              // if zero it defaults to object bounding box
   drag = 0.6;                // Drag coefficient
   bodyFriction = "0.6";
   bodyRestitution = "0.4";
   minImpactSpeed = 5;        // Impacts over this invoke the script callback
   softImpactSpeed = 5;       // Play SoftImpact Sound
   hardImpactSpeed = 15;      // Play HardImpact Sound
   integration = 8;           // Physics integration: TickSec/Rate
   collisionTol = 0.1;        // Collision distance tolerance
   contactTol = 0.1;          // Contact velocity tolerance

   // Engine
   engineTorque = "12000";       // Engine power
   engineBrake = "1000";         // Braking when throttle is 0
   brakeTorque = "6000";        // When brakes are applied
   maxWheelSpeed = "60";        // Engine scale by current speed / max speed

   // Energy
   maxEnergy = 100;
   jetForce = 3000;
   minJetEnergy = 30;
   jetEnergyDrain = 2;

   // Sounds
//   jetSound = ScoutThrustSound;
//   wheelImpactSound = WheelImpactSound;

//   explosion = VehicleExplosion;

   // Dynamic fields accessed via script
   nameTag = 'Buggy';
   maxDismountSpeed = 10;
   maxMountSpeed = 5;
   mountPose0 = "Sitting";
   cameraMinDist = "3";
   steeringReturn = "0";
   powerSteering = "0";
};
