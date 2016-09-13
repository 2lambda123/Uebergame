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

datablock ItemData(GrenadeThrown : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/Grenade/grenade.dts";
   //scale = "0.5 0.5 0.5";
   mass = 1.0;
   density = 20;
   elasticity = 0.2;
   friction = 1;
   gravityMod = 0.5;
   dynamicType = $TypeMasks::DamagableItemObjectType;

   maxDamage = 0.21;
   destroyedLevel = 0.2;

   directDamage = 0;
   radiusDamage = 165;
   damageRadius = 5.0;
   damageType = $DamageType::Grenade;
   areaImpulse = 1500;
   detonationTime = 3000;

   explosion = FragExplosion;
   underwaterExplosion = UnderwaterGrenadeExplosion;
   decal               = ScorchRXDecal;
};

datablock ItemData(Grenade : DefaultWeapon)
{
   category = "Handheld";
   className = "HandInventory";
   shapeFile = "art/shapes/weapons/Grenade/grenade.dts";
   computeCRC = false;

   image = FragGrenadeImage;
   thrownItem = GrenadeThrown;
   pickUpName = 'Frag Grenade';
   throwTimeout = 800;
   isGrenade = true;
};

datablock ItemData(GrenadeAmmo : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/Grenade/grenade.dts";
   pickUpName = 'grenade';
};

datablock ShapeBaseImageData(FragGrenadeImage)
{
   class = "GrenadeImage";
   className = GrenadeImage;

   shapeFile = "art/editor/invisible.dts";
   //shapeFile = "art/shapes/weapons/Grenade/grenade.dts";
   //shapeFileFP = "art/shapes/weapons/Grenade/grenade.dts";
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
   item = Grenade;
   ammo = GrenadeAmmo;
   thrownItem = GrenadeThrown;

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
   stateTimeoutValue[7]            = 3;
   stateWaitForTimeout[7]          = false;
};

//-----------------------------------------------------------------------------

function GrenadeAmmo::onInventory(%data, %obj, %amount)
{
   if ( !%obj.isMemberOfClass( "Player" ) )
      return;

   //LogEcho("\c3GrenadeAmmo::onInventory(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %amount SPC ")");

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

function GrenadeThrown::onCollision( %data, %obj, %col )
{
   // Spam suppression
}

function GrenadeThrown::detonate(%data, %gren)
{
   if(isEventPending(%gren.detThread))
      cancel(%gren.detThread);

   %gren.setDamageState(Destroyed);
}

function GrenadeThrown::onDestroyed(%data, %obj, %prevState)
{
   radiusDamage(%obj, %obj.sourceObject, %obj.getPosition(), %data.damageRadius, %data.radiusDamage, %data.damageType, %data.areaImpulse);
   %obj.schedule(500, "delete");
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowGrenade("Soldier");
SmsInv.AddGrenade(Grenade, "Grenade");
//             |Item| |InvGrenade| |AmmoIncrement|

SmsInv.AllowAmmo("armor\tSoldier\t2");
SmsInv.AddAmmo(GrenadeAmmo, 1);
