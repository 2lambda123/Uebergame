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

datablock ItemData(TimeBombThrown : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/explosives/time_bomb/time_bomb.dts";
   mass = 0.8;
   density = 20;
   elasticity = 0.1;
   friction = 1;
   dynamicType = $TypeMasks::DamagableItemObjectType;

   maxDamage = 0.21;
   destroyedLevel = 0.2;

   directDamage = 0;
   radiusDamage = 200;
   damageRadius = 12;

   damageType = $DamageType::Explosion;
   areaImpulse = 2000;
   detonationTime = 30000;
   explosion = LargeExplosion;
   underwaterExplosion = LargeWaterExplosion;

   simpleServerCollision = false;
};

datablock ItemData(TimeBomb : DefaultWeapon)
{
   category = "Handheld";
   className = "HandInventory";
   shapeFile = "art/shapes/weapons/explosives/time_bomb/time_bomb.dts";
   computeCRC = false;

   // Script varibles
   image = TimeBombImage;
   thrownItem = TimeBombThrown;
   pickUpName = 'Time Bomb';
   throwTimeout = 5000;
   isGrenade = true;
};

datablock ItemData(TimeBombAmmo : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/explosives/time_bomb/time_bomb.dts";
   pickUpName = 'time bomb';
};

datablock ShapeBaseImageData(TimeBombImage)
{
   className = GrenadeImage;
   shapeFile = "art/editor/invisible.dts";
   computeCRC = false;
   cloakable = true;
   
   item = TimeBomb;
   ammo = TimeBombAmmo;
   thrownItem = TimeBombThrown;
   throwTimeout = 800;
   
   mass = 2;
   mountPoint = 1;
   //offset = "0 0 +1";
   //rotation = "1 0 0 90";
   //scale = "0.5 0.5 0.5";
   
   lightType = "NoLight";
   
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

function serverCmdsetBombTimer(%client, %time)
{
   %player = %client.player;
   if ( isObject( %player ) && isObject( %player.bombThrown ) )
   {
      if ( %time < 5 )
         %time = 5;
      if ( %time > 30 )
         %time = 30;

      %player.bombThrown.armed = true;
      %player.bombThrown.detThread = %player.bombThrown.getDataBlock().schedule(%time * 1000, "detonate", %player.bombThrown);
      bottomPrint(%client, "<font:Arial Bold:14>Bomb armed with a time of" SPC %time SPC "seconds!", 3, 2);
   }
}

function TimeBombImage::onThrowGrenade(%data, %obj, %slot)
{
   LogEcho("TimeBombImage::onThrowGrenade(" SPC %data @", "@ %obj.client.nameBase SPC ")");

   if ( isObject( %obj.client ) )
   {
	  schedule(100, %obj, "commandToClient", %obj.client, 'OpenBombTimer');
      %thrownItem = Parent::onThrowGrenade(%data, %obj, %slot);
      cancel( %thrownItem.detThread );
      %obj.bombThrown = %thrownItem;
      %thrownItem.armed = false;
      //commandToClient( %obj.client, 'OpenBombTimer' );
      
   }
   else
      Parent::onThrowGrenade(%data, %obj, %slot);
}

//-----------------------------------------------------------------------------

function TimeBombAmmo::onInventory(%data, %obj, %amount)
{
   if ( !%obj.isMemberOfClass( "Player" ) )
      return;

   LogEcho("\c3TimeBombAmmo::onInventory(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %amount SPC ")");

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

function TimeBombThrown::onCollision( %data, %obj, %col )
{
   // Spam suppression
}

function TimeBombThrown::detonate(%data, %gren)
{
   if(!%gren.armed)
      return;

   if(isEventPending(%gren.detThread))
      cancel(%gren.detThread);

   %gren.sourceObject.bombThrown = 0;
   %gren.setDamageState(Destroyed);
}

function TimeBombThrown::onDestroyed(%data, %obj, %prevState)
{
   radiusDamage(%obj, %obj.sourceObject, %obj.getPosition(), %data.damageRadius, %data.radiusDamage, %data.damageType, %data.areaImpulse);
   %obj.schedule(500, "delete");
}

//-----------------------------------------------------------------------------
// SMS Inventory
SmsInv.AllowGrenade("Soldier");
SmsInv.AddGrenade(TimeBomb, "Time Bomb");
//             |Item| |InvGrenade| |AmmoIncrement|

SmsInv.AllowAmmo("armor\tSoldier\t1");
SmsInv.AddAmmo(TimeBombAmmo, 1);

