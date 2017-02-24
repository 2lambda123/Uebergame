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

datablock SFXProfile(cheetahEngine)
{
   preload = "1";
   description = "AudioCloseLoop3D";
   fileName = "art/sound/cheetah/cheetah_engine.ogg";
};

datablock SFXProfile(cheetahSqueal)
{
   preload = "1";
   description = "AudioDefault3D";
   fileName = "art/sound/cheetah/cheetah_squeal.ogg";
};

datablock SFXProfile(hardImpact)
{
   preload = "1";
   description = "AudioDefault3D";
   fileName = "art/sound/cheetah/hardImpact.ogg";
};

datablock SFXProfile(softImpact)
{
   preload = "1";
   description = "AudioDefault3D";
   fileName = "art/sound/cheetah/softImpact.ogg";
};

datablock SFXProfile(DirtKickup)
{
   preload = "1";
   description = "AudioDefault3D";
   fileName = "art/sound/cheetah/softImpact.ogg";
};

datablock SFXProfile(CheetahTurretFireSound)
{
   //filename = "art/sound/cheetah/turret_firing.wav";
   filename = "art/sound/weapons/turret/wpn_turret_fire.wav";
   description = BulletFireDesc;
   preload = true;
};

datablock DebrisData(CheetahDebris)
{
   shapeFile = "art/shapes/vehicles/Cheetah/wheelBack.dts";
   explodeOnMaxBounce = false;

   elasticity = 0.15;
   friction = 0.5;

   lifetime = 10.0;
   lifetimeVariance = 2.0;

   minSpinSpeed = 40;
   maxSpinSpeed = 600;

   numBounces = 6;
   bounceVariance = 2.3;

   staticOnMaxBounce = true;
   gravModifier = 1.0;

   useRadiusMass = true;
   baseRadius = 1;

   velocity = 15.0;
   velocityVariance = 8.0;
};

datablock ParticleData(CheetahTireParticle)
{
   textureName          = "art/particles/dustParticle";
   dragCoefficient      = "1.99902";
   gravityCoefficient   = "-0.100122";
   inheritedVelFactor   = "0.0998043";
   constantAcceleration = 0.0;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 400;

   colors[0]            = "0.456693 0.354331 0.259843 1";
   colors[1]            = "0.456693 0.456693 0.354331 0";

   sizes[0]             = "0.997986";
   sizes[1]             = "3.99805";
   sizes[2]             = "1.0";
   sizes[3]             = "1.0";

   times[0]             = "0.0";
   times[1]             = "1";
   times[2]             = "1";
   times[3]             = "1";
};

datablock ParticleEmitterData(CheetahTireEmitter)
{
   ejectionPeriodMS = 20;
   periodVarianceMS = 10;
   ejectionVelocity = "14.57";
   velocityVariance = 1.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 60;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "CheetahTireParticle";
   blendStyle = "ADDITIVE";
};

datablock ProjectileData(CheetahTurretProjectile)
{
   projectileShapeName = "art/shapes/weapons/shared/rocket.dts";
   directDamage        = 0;
   radiusDamage        = 50;
   damageRadius        = 10;
   areaImpulse         = 1500;
   impactForce         = 5;

   damageType          = $DamageType::RocketLauncher;

   explosion           = RocketLauncherExplosion;
   waterExplosion      = UnderwaterGrenadeExplosion;

   particleEmitter     = GrenadeTrailEmitter;
   //particleWaterEmitter = UWGrenadeTrailEmitter;

   decal = ScorchRXDecal;
   splash = "";

   muzzleVelocity = 250;
   velInheritFactor = 0.7;

   armingDelay = 0;
   lifetime = 5000;
   fadeDelay = 4500;

   bounceElasticity = 0;
   bounceFriction = 0;
   isBallistic = false;
   gravityMod = 0.80;

   lightDesc           = "RocketLauncherLightDesc";
};

datablock ParticleEmitterData(TurretFireSmokeEmitter)
{
   ejectionPeriodMS  = 10;
   periodVarianceMS  = 5;
   ejectionVelocity  = 6.5;
   velocityVariance  = 1.0;
   thetaMin          = "0";
   thetaMax          = "0";
   lifetimeMS        = 350;
   particles         = "GunFireSmoke";
   blendStyle        = "NORMAL";
   softParticles     = "0";
   alignParticles    = "0";
   orientParticles   = "0";
};

datablock ShapeBaseImageData(CheetahTurretImage)
{
   // Basic Item properties
   shapeFile = "art/shapes/vehicles/Cheetah/Cheetah_Turret.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 1;
   firstPerson = false;

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.
   correctMuzzleVector = false;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";
   class = "WeaponImage";

   // Projectile and Ammo
   ammo = BulletAmmo;
   usesEnergy = true;
   useMountEnergy = true;
   fireEnergy = 10;
   minEnergy = 12;

   projectile = CheetahTurretProjectile;
   projectileType = Projectile;
   projectileSpread = "0.01";

   // Weapon lights up while firing
   lightColor = "0.992126 0.968504 0.708661 1";
   lightRadius = "4";
   lightDuration = "100";
   lightType = "WeaponFireLight";
   lightBrightness = 2;

   // Shake camera while firing.
   shakeCamera = false;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   useRemainderDT = true;

   // Initial start up state
   stateName[0]                     = "Preactivate";
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNoAmmo[0]       = "NoAmmo";

   // Activating the gun.  Called when the weapon is first
   // mounted and there is ammo.
   stateName[1]                     = "Activate";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 0.5;
   stateSequence[1]                 = "Activate";

   // Ready to fire, just waiting for the trigger
   stateName[2]                     = "Ready";
   stateTransitionOnNoAmmo[2]       = "NoAmmo";
   stateTransitionOnTriggerDown[2]  = "Fire";

   // Fire the weapon. Calls the fire script which does
   // the actual work.
   stateName[3]                     = "Fire";
   stateTransitionOnTimeout[3]      = "Reload";
   stateTimeoutValue[3]             = 0.1;
   stateFire[3]                     = true;
   stateRecoil[3]                   = "";
   stateAllowImageChange[3]         = false;
   stateSequence[3]                 = "Fire";
   stateSequenceRandomFlash[3]      = true;        // use muzzle flash sequence
   stateScript[3]                   = "onFire";
   stateSound[3]                    = CheetahTurretFireSound;
   stateEmitter[3]                  = TurretFireSmokeEmitter;
   stateEmitterTime[3]              = 0.025;

   // Play the reload animation, and transition into
   stateName[4]                     = "Reload";
   stateTransitionOnNoAmmo[4]       = "NoAmmo";
   stateWaitForTimeout[4]           = "0";
   stateTransitionOnTimeout[4]      = "Ready";
   stateTimeoutValue[4]             = 1.2;
   stateAllowImageChange[4]         = false;
   stateSequence[4]                 = "Reload";
   //stateEjectShell[4]               = true;

   // No ammo in the weapon, just idle until something
   // shows up. Play the dry fire sound if the trigger is
   // pulled.
   stateName[5]                     = "NoAmmo";
   stateTransitionOnAmmo[5]         = "Reload";
   stateSequence[5]                 = "NoAmmo";
   stateTransitionOnTriggerDown[5]  = "DryFire";

   // No ammo dry fire
   stateName[6]                     = "DryFire";
   stateTimeoutValue[6]             = 1.0;
   stateTransitionOnTimeout[6]      = "NoAmmo";
   stateScript[6]                   = "onDryFire";
};

function CheetahTurretImage::onFire(%data, %obj, %slot)
{
   //LogEcho("CheetahTurretImage::onFire(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");

   if ( %obj.getEnergyLevel() < %data.minEnergy )
      return;

   %obj.setEnergyLevel( %obj.getEnergyLevel() - %data.fireEnergy );

   %data.lightStart = $Sim::Time;

   if (isObject(%obj.lastProjectile) && %obj.deleteLastProjectile)
      %obj.lastProjectile.delete();

   if( %data.projectileSpread > 0 )
   {
      %vec = %obj.getMuzzleVector(%slot);
      %x = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %y = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %z = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
      %muzzleVector = MatrixMulVector(%mat, %vec);
   }
   else
   {
      %muzzleVector = MatrixMulVector("0 0 0 0 0 1 0", %obj.getMuzzleVector(%slot));
   }

   // Determin initial projectile velocity based on the 
   // gun's muzzle point and the object's current velocity
   %objectVelocity = %obj.getVelocity();
   %muzzleVelocity = VectorAdd(VectorScale(%muzzleVector, %data.projectile.muzzleVelocity), VectorScale(%objectVelocity, %data.projectile.velInheritFactor));

   // Create the projectile object
   %p = new (%data.projectileType)() {
      dataBlock        = %data.projectile;
      initialVelocity  = %muzzleVelocity;
      initialPosition  = %obj.getMuzzlePoint(%slot);
      // This parameter is deleted about 7 ticks into the projectiles flight
      sourceObject     = %obj;
      sourceSlot       = %slot;
      // We use this for the source object when applying damage because it isn't deleted
      origin           = %obj;
      client           = %obj.client;
   };

   %obj.lastProjectile = %p;
   %obj.deleteLastProjectile = %data.deleteLastProjectile;
   if(%obj.client)
      %obj.client.projectile = %p;

   MissionCleanup.add(%p);
   return %p;
}

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
datablock WheeledVehicleTire(CheetahCarTire)
{
   // Tires act as springs and generate lateral and longitudinal
   // forces to move the vehicle. These distortion/spring forces
   // are what convert wheel angular velocity into forces that
   // act on the rigid body.
   shapeFile = "art/shapes/vehicles/Cheetah/wheel.dts";
   staticFriction = 4;
   kineticFriction = 1.25;

   // Spring that generates lateral tire forces
   lateralForce = 18000;
   lateralDamping = 4000;
   lateralRelaxation = 1;

   // Spring that generates longitudinal tire forces
   longitudinalForce = 18000;
   longitudinalDamping = 4000;
   longitudinalRelaxation = 1;
   radius = "0.609998";
};

datablock WheeledVehicleTire(CheetahCarTireRear)
{
   // Tires act as springs and generate lateral and longitudinal
   // forces to move the vehicle. These distortion/spring forces
   // are what convert wheel angular velocity into forces that
   // act on the rigid body.
   shapeFile = "art/shapes/vehicles/Cheetah/wheelBack.dts";
   staticFriction = "7.2";
   kineticFriction = "1";

   // Spring that generates lateral tire forces
   lateralForce = "19000";
   lateralDamping = 6000;
   lateralRelaxation = 1;

   // Spring that generates longitudinal tire forces
   longitudinalForce = 18000;
   longitudinalDamping = 4000;
   longitudinalRelaxation = 1;
   radius = "0.840293";
};

datablock WheeledVehicleSpring(CheetahCarSpring)
{
   // Wheel suspension properties
   length = 0.85;             // Suspension travel
   force = 3000;              // Spring force
   damping = 600;             // Spring damping
   antiSwayForce = 3;         // Lateral anti-sway force
};

datablock WheeledVehicleData(CheetahCar)
{
   category = "Vehicles";
   shapeFile = "art/shapes/vehicles/Cheetah/Cheetah_Body.dts";
   cloakTexture = "art/particles/cloakTexture.png";
   emap = 1;
   computeCRC = false;

   explosion = LargeExplosion;
   underwaterExplosion = LargeWaterExplosion;
   debris = CheetahDebris;
   renderWhenDestroyed = false;
   debrisShapeName = "art/shapes/vehicles/Cheetah/wheel.dts";

   mountPose[0] = sitting;
   numMountPoints = 1;
   isProtectedMountPoint[0] = true;

   maxDamage = 501; // Must be higher then destroyed level
   disabledLevel = 490;
   destroyedLevel = 500;
   repairRate = 0.005;
   isInvincible = false;

   firstPersonOnly = false;
   useEyePoint = true; // Use the vehicle's camera node rather than the player's
   observeThroughObject = false;

   maxSteeringAngle = 0.785;  // Maximum steering angle, should match animation

   // 3rd person camera settings
   cameraRoll = false;        // Roll the camera with the vehicle
   cameraMaxDist = 10;       // Far distance from vehicle
   cameraOffset = 1.0;        // Vertical offset from camera mount point
   cameraLag = "0.3";           // Velocity lag of camera
   cameraDecay = 1.25;        // Decay per sec. rate of velocity lag

   // Rigid Body
   mass = "400";
   massCenter = "0 0.5 0";    // Center of mass for rigid body
   massBox = "0 0 0";         // Size of box used for moment of inertia,
                              // if zero it defaults to object bounding box
   drag = 0.6;                // Drag coefficient
   bodyFriction = 0.6;        //when this gets high it CAN cause probs, doesn't always
   bodyRestitution = 0.4;
   minImpactSpeed = 5;        // Impacts over this invoke the script callback
   softImpactSpeed = 5;       // Play SoftImpact Sound
   hardImpactSpeed = 15;      // Play HardImpact Sound
   integration = 8;           // Physics integration: TickSec/Rate
   collisionTol = "0.1";        // Collision distance tolerance
   contactTol = "0.4";          // Contact velocity tolerance

   // Engine
   engineTorque = 3000;       // Engine power
   engineBrake = "800";         // Braking when throttle is 0
   brakeTorque = "8000";        // When brakes are applied
   maxWheelSpeed = 25;        // Engine scale by current speed / max speed

   // Energy
   inheritEnergyFromMount = false;
   maxEnergy = 200;
   jetForce = 3000;
   minJetEnergy = 30;
   jetEnergyDrain = 2;
   rechargeRate = 0.8;
   energyPerDamagePoint = 75;

   // Impact damage
   speedDamageScale = 0.06;
   collDamageThresholdVel = 20;
   collDamageMultiplier = 0.02;

   // vehicleData Smoke under the vehicle
   dustEmitter = VehicleDustEmitter;
   triggerDustHeight = 0.5;
   dustHeight = 1.0;

   damageEmitter[0] = LightDamageEmitter;
   damageEmitter[1] = HeavyDamageEmitter;
   damageEmitter[2] = DamageBubblesEmitter;
   damageEmitterOffset[0] = "0.0 -3.0 -0.5 ";
   damageLevelTolerance[0] = 0.4;
   damageLevelTolerance[1] = 0.7;
   numDmgEmitterAreas = 1;

   splashEmitter[0] = VehicleSplashDropletsEmitter;
   splashEmitter[1] = VehicleSplashEmitter;
   splashFreqMod = 300.0;
   splashVelEpsilon = 0.50;
   exitSplashSoundVelocity = 15;
   softSplashSoundVelocity = 5;
   mediumSplashSoundVelocity = 15;
   hardSplashSoundVelocity = 25;

   // Sounds
   engineSound = cheetahEngine;
   //squealSound = cheetahSqueal;
   softImpactSound = softImpact;
   hardImpactSound = hardImpact;

   // Dynamic fields accessed via script
   nameTag = 'Cheetah';
   maxDismountSpeed = 10;
   maxMountSpeed = 5;
   checkRadius = 10;
   dismountRadius = 20;
   maxDismountSpeed = 200;
   maxMountSpeed = 5;
   createHoverHeight = 0.5;
   // Radius damage
   canImpulse = true;
   numWeapons = 1;

   tireEmitter = "CheetahTireEmitter";

   // Mount slots
   turretSlot = 1;
   rightBrakeSlot = 2;
   leftBrakeSlot = 3;
};

//-----------------------------------------------------------------------------

function CheetahCar::onAdd(%this, %obj)
{
   Parent::onAdd(%this, %obj);

   %obj.setWheelTire(0,CheetahCarTire);
   %obj.setWheelTire(1,CheetahCarTire);
   %obj.setWheelTire(2,CheetahCarTireRear);
   %obj.setWheelTire(3,CheetahCarTireRear);

   // Setup the car with some tires & springs
   for (%i = %obj.getWheelCount() - 1; %i >= 0; %i--)
   {
      %obj.setWheelPowered(%i, true);
      %obj.setWheelSpring(%i, CheetahCarSpring);
   }

   // Steer with the front tires
   %obj.setWheelSteering(0, 1);
   %obj.setWheelSteering(1, 1);
   %obj.setWheelSteering(2, 0);
   %obj.setWheelSteering(3, 0);

   // Add tail lights
   %obj.rightBrakeLight = new PointLight() 
   {
      radius = "1";
      isEnabled = "0";
      color = "1 0 0.141176 1";
      brightness = "2";
      castShadows = "1";
      priority = "1";
      animate = "0";
      animationPeriod = "1";
      animationPhase = "1";
      flareScale = "1";
      attenuationRatio = "0 1 1";
      shadowType = "DualParaboloidSinglePass";
      texSize = "512";
      overDarkFactor = "2000 1000 500 100";
      shadowDistance = "400";
      shadowSoftness = "0.15";
      numSplits = "1";
      logWeight = "0.91";
      fadeStartDistance = "0";
      lastSplitTerrainOnly = "0";
      representedInLightmap = "0";
      shadowDarkenColor = "0 0 0 -1";
      includeLightmappedGeometryInShadow = "0";
      rotation = "1 0 0 0";
      canSave = "1";
      canSaveDynamicFields = "1";
         splitFadeDistances = "10 20 30 40";
   };
   %obj.leftBrakeLight = new PointLight() 
   {
      radius = "1";
      isEnabled = "0";
      color = "1 0 0.141176 1";
      brightness = "2";
      castShadows = "1";
      priority = "1";
      animate = "0";
      animationPeriod = "1";
      animationPhase = "1";
      flareScale = "1";
      attenuationRatio = "0 1 1";
      shadowType = "DualParaboloidSinglePass";
      texSize = "512";
      overDarkFactor = "2000 1000 500 100";
      shadowDistance = "400";
      shadowSoftness = "0.15";
      numSplits = "1";
      logWeight = "0.91";
      fadeStartDistance = "0";
      lastSplitTerrainOnly = "0";
      representedInLightmap = "0";
      shadowDarkenColor = "0 0 0 -1";
      includeLightmappedGeometryInShadow = "0";
      rotation = "1 0 0 0";
      canSave = "1";
      canSaveDynamicFields = "1";
         splitFadeDistances = "10 20 30 40";
   };

   // Mount a ShapeBaseImageData
   %didMount = %obj.mountImage(CheetahTurretImage, %this.turretSlot);
   %obj.selectedWeapon = %this.turretSlot;

   // Mount the brake lights
   %obj.mountObject(%obj.rightBrakeLight, %this.rightBrakeSlot);
   %obj.mountObject(%obj.leftBrakeLight, %this.leftBrakeSlot);
}

function CheetahCar::onRemove(%this, %obj)
{
   Parent::onRemove(%this, %obj);

   if(isObject(%obj.rightBrakeLight))
      %obj.rightBrakeLight.delete();

   if(isObject(%obj.leftBrakeLight))
      %obj.leftBrakeLight.delete();

   if(isObject(%obj.turret))
      %obj.turret.delete();
}

function CheetahCar::playerDismounted(%data, %obj, %player)
{
   %obj.fireWeapon = false;
   %obj.setImageTrigger(1, false);

   Parent::playerDismounted(%data, %obj, %player);
}

function serverCmdtoggleBrakeLights(%client)
{
   %car = %client.player.getControlObject();

   if ( isObject( %car ) && %car.getClassName() $= "WheeledVehicle" )
   {
      if(%car.rightBrakeLight.isEnabled)
      {
         %car.rightBrakeLight.setLightEnabled(0);
         %car.leftBrakeLight.setLightEnabled(0);
      }
      else
      {
         %car.rightBrakeLight.setLightEnabled(1);
         %car.leftBrakeLight.setLightEnabled(1);
      }
   }
}

// Callback invoked when an input move trigger state changes when the CheetahCar
// is the control object
function CheetahCar::onTrigger(%this, %obj, %index, %state)
{
   // Pass trigger states on to TurretImage (to fire weapon)
   //%player = %obj.getMountNodeObject(0);
   switch ( %index )
   {
      case 0: %obj.setImageTrigger( %this.turretSlot, %state );
      case 1: %obj.setImageAltTrigger( %this.turretSlot, %state );
      //case 2: if ( %state == 1 ) { %player.getDataBlock().doDismount( %player, 0 ); }
   }
}

function TurretImage::onMount(%this, %obj, %slot)
{
   // Load the gun
   %obj.setImageAmmo(%slot, true);
}

//-----------------------------------------------------------------------------
// SMS

//             |Vehicle DB| |Vehicle Name| |Max Allowed|
SmsInv.AddVehicle(CheetahCar, "Cheetah", 3);
