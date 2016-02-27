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

datablock SFXProfile(SmokeGrenadeExplosionSound)
{
   filename = "art/sound/weapons/grenades/smokeGrenade";
   description = AudioClose3D;
   preload = true;
};

//--------------------------------------------------------------------------
// Projectile Explosion

datablock ParticleData(SmokeScreenSmoke)
{
   textureName          = "art/particles/smokeScreen";
   dragCoefficient      = 0.75;
   gravityCoefficient   = -0.01;
   inheritedVelFactor   = 0.2;
   windCoefficient      = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 10000;
   lifetimeVarianceMS   = 500;
   useInvAlpha          = true;
   spinRandomMin        = -20.0;
   spinRandomMax        = 20.0;
   spinSpeed            = 1.0;

   colors[0] = "0.6 0.6 0.6 0.3";
   colors[1] = "0.6 0.6 0.6 0.3";
   colors[2] = "0.6 0.6 0.6 0.0";

   sizes[0] = 6.0;
   sizes[1] = 8.0;
   sizes[2] = 10.0;

   times[0] = 0.0;
   times[1] = 0.75;
   times[2] = 1.0;
};

datablock ParticleEmitterData(SmokeScreenSmokeEmitter1)
{
   ejectionPeriodMS = 50;
   periodVarianceMS = 1;
   ejectionVelocity = 8.0;
   velocityVariance = 4.0;
   ejectionOffset   = 3.0;
   thetaMin         = 85;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   orientParticles  = false;
   orientOnVelocity = false;
   particles = "SmokeScreenSmoke";
};

datablock ParticleEmitterData(SmokeScreenSmokeEmitter2 : SmokeScreenSmokeEmitter1)
{
   ejectionPeriodMS = 250;
   periodVarianceMS = 1;
   ejectionVelocity = 5.0;
   velocityVariance = 5.0;
   ejectionOffset   = 0.0;
};

datablock ParticleEmitterData(SmokeScreenEmitter : SmokeScreenSmokeEmitter1)
{
   ejectionPeriodMS = 250;
   ejectionVelocity = 1;
   velocityVariance = 0.5;
   ejectionOffset   = 2.0;
};

datablock ExplosionData(SmokeGrenadeExplosion)
{
   lifeTimeMS = 20000;

   soundProfile = SmokeGrenadeExplosionSound;

   particleEmitter = SmokeScreenEmitter;
   particleDensity = 5;
   particleRadius = 1;

   emitter[0] = SmokeScreenSmokeEmitter1; 
   emitter[1] = SmokeScreenSmokeEmitter2;
};

//-----------------------------------------------------------------------------

datablock ItemData(SmokeGrenadeThrown : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dts";
   mass = 1.0;
   density = 20;
   elasticity = 0.2;
   friction = 1;
   gravityMod = 0.5;
   dynamicType = $TypeMasks::DamagableItemObjectType;

   maxDamage = 0.21;
   destroyedLevel = 0.2;

   // Script varibles
   detonationTime = 3000;
   canImpulse = true;

   explosion = SmokeGrenadeExplosion;
   underwaterExplosion = SmokeGrenadeExplosion;
};

datablock ItemData(SmokeGrenade : DefaultWeapon)
{
   category = "Handheld";
   className = "HandInventory";
   shapeFile = "art/shapes/weapons/grenade/grenade.dts";
   computeCRC = false;

   // Script varibles
   image = SmokeGrenadeImage;
   thrownItem = SmokeGrenadeThrown;
   pickUpName = 'Smoke Grenade';
   throwTimeout = 20000;
   isGrenade = true;
};

datablock ItemData(SmokeGrenadeAmmo : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/grenade/grenade.dts";
   pickUpName = 'smoke grenade';
};

datablock ShapeBaseImageData(SmokeGrenadeImage)
{
   class = "GrenadeImage";
   className = GrenadeImage;

   shapeFile = "art/editor/invisible.dts";
   //shapeFile = "art/shapes/weapons/grenade/grenade.dts";
   //shapeFileFP = "art/shapes/weapons/grenade/grenade.dts";
   emap = true;
   computeCRC = false;
   cloakable = true;

   imageAnimPrefix = "Pistol";
   imageAnimPrefixFP = "Pistol";

   //mountPoint = 3;
   //offset = "0 +0.3 +1.25"; // L/R - F/B - T/B
   //rotation = "1 0 0 22";
   firstPerson = true;
   useEyeNode = true;
   animateOnServer = true;
   mass = 2;

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.
   correctMuzzleVector = false;

   throwTimeout = 800;
   item = SmokeGrenade;
   ammo = SmokeGrenadeAmmo;
   thrownItem = SmokeGrenadeThrown;
   usesEnergy = 0;
   minEnergy = 0;

   lightType = "NoLight";

   sprintDisallowed = false;
   crouchDisallowed = false;
   proneDisallowed = false;
   swimmingDisallowed = false;

   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "1 1 1";
   camShakeAmp = "6 6 6";
   camShakeDuration = "1.5";
   camShakeRadius = "1.2";

   stateName[0]                    = "Preactivate";
   stateTransitionOnLoaded[0]      = "Activate";
   stateTransitionOnNoAmmo[0]      = "NoAmmo";
   stateSound[0]                   = WeaponSwitchSound;

   stateName[1]                    = "Activate";
   stateTransitionOnTimeout[1]     = "Ready";
   stateTimeoutValue[1]            = 0.2;
   stateSequence[1]                = "Activate";

   stateName[2]                    = "Ready";
   stateTransitionOnNoAmmo[2]      = "NoAmmo";
   stateTransitionOnTriggerDown[2] = "Charge";

   stateName[3]                    = "Fire";
   stateTransitionOnTimeout[3]     = "Reload";
   stateTimeoutValue[3]            = 0.2;
   stateFire[3]                    = true;
   stateRecoil[3]                  = LightRecoil;
   stateAllowImageChange[3]        = false;
   stateScript[3]                  = "onThrowGrenade";

   stateName[4]                    = "Reload";
   stateTransitionOnNoAmmo[4]      = "NoAmmo";
   stateTransitionOnTimeout[4]     = "Activate";
   stateTimeoutValue[4]            = 0.2;
   stateAllowImageChange[4]        = false;
   stateSequence[4]                = "Reload";

   stateName[5]                    = "NoAmmo";
   stateTransitionOnAmmo[5]        = "Reload";
   stateSequence[5]                = "NoAmmo";
   stateTransitionOnTriggerDown[5] = "DryFire";

   stateName[6]                    = "DryFire";
   stateSound[6]                   = WeaponEmptySound;
   stateTimeoutValue[6]            = 0.2;
   stateTransitionOnTimeout[6]     = "NoAmmo";
   stateScript[6]                  = "onDryFire";

   stateName[7]                    = "Charge";
   stateScript[7]                  = "chargeStart";
   stateTransitionOnTriggerUp[7]   = "Fire";
   stateTransitionOnTimeout[7]     = "Fire";
   stateTimeoutValue[7]            = 2.5;
   stateWaitForTimeout[7]          = false;
};

//-----------------------------------------------------------------------------

function SmokeGrenadeAmmo::onInventory(%data, %obj, %amount)
{
   if ( !%obj.isMemberOfClass( "Player" ) )
      return;

   LogEcho("\c3SmokeGrenadeAmmo::onInventory(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %amount SPC ")");

   // The ammo inventory state has changed, we need to update any mounted images using this ammo to reflect the new state.
   if ( ( %image = %obj.getMountedImage( $GrenadeSlot ) ) > 0 )
   {
      if ( isObject( %image.ammo ) && %image.ammo.getId() == %data.getId() )
      {
         %obj.setImageAmmo( $GrenadeSlot, %amount != 0 );
      }

      // Now send the client a silent message containing the ammo amount and the short name of the weapon its for.
      // The client can stuff this info into an array so we can order it properly.
      if ( isObject( %obj.client ) )
         messageClient( %obj.client, 'MsgGrenadeCnt', "", addTaggedString($DataToName[%image.item]), $GrenadeSlot, addTaggedString(%obj.getInventory(%data)) );
   }
}

function SmokeGrenadeThrown::onCollision( %data, %obj, %col )
{
   // Spam suppression
}

function SmokeGrenadeThrown::detonate(%data, %gren)
{
   if(isEventPending(%gren.detThread))
      cancel(%gren.detThread);

   %gren.setDamageState(Destroyed);
}

function SmokeGrenadeThrown::onDestroyed(%data, %obj, %prevState)
{
   %obj.schedule(20000, "delete");
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowGrenade("Soldier");
SmsInv.AddGrenade(SmokeGrenade, "Smoke Grenade");
//             |Item| |InvGrenade| |AmmoIncrement|
SmsInv.AllowAmmo("armor\tSoldier\t2");
SmsInv.AddAmmo(SmokeGrenadeAmmo, 1);
