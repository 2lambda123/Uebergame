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

datablock SFXProfile(ShapeChargeActivateSound)
{
   filename    = "art/sound/weapons/mine_armed";
   description = AudioClose3d;
   preload = true;
};

datablock SFXProfile(ShapeChargeTriggerSound)
{
   filename = "art/sound/weapons/mine_trigger";
   description = AudioClose3d;
   preload = true;
};

datablock StaticShapeData(ShapeChargeDeployed : StaticShapeDamageScale)
{
   //category = "Deployable";

   shapeFile = "art/shapes/weapons/explosives/plastique/plastique.dts";
   emap = false;
   computeCRC = false;
   dynamicType = $TypeMasks::StaticShapeObjectType;

   isShielded = false;
   repairRate = 0;
   energyPerDamagePoint = 75;
   maxEnergy = 50;
   rechargeRate = 0;

   isInvincible = false;
   maxDamage = 50; // Must be higher then destroyed level
   destroyedLevel = 49;
   directDamage = 0;
   radiusDamage = 250.0;
   damageRadius = 7;
   damageType = $DamageType::ShapeCharge;

   renderWhenDestroyed = false;
   areaImpulse = 3000;
   canImpulse = false;
   deployedObject = false;

   explosion = LargeExplosion;
   underwaterExplosion = LargeWaterExplosion;

   armDelay = 1000;
   detonateRange = 75;
};

datablock ItemData(ShapeChargeTossed : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/explosives/plastique/plastique.dts";
   computeCRC = false;
   mass = 1;
   density = 10;
   elasticity = 0.1;
   friction = 0.9;
   maxVelocity = 100;
   sticky = true;

   lightType = "NoLight";
};

datablock ItemData(ShapeCharge : DefaultWeapon)
{
   category = "Handheld";
   className = "HandInventory";
   shapeFile = "art/shapes/weapons/explosives/plastique/plastique.dts";
   computeCRC = false;
   
   mass = 3;
   drag = 0.5;
   density = 2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   maxVelocity = 20;
   dynamicType = $TypeMasks::ItemObjectType;

   // Script varibles
   image = ShapeChargeImage;
   thrownItem = ShapeChargeTossed;
   pickUpName = 'Shape Charge';
   throwTimeout = 5000;
   isGrenade = true;

   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;
};

datablock ItemData(ShapeChargeAmmo : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/explosives/plastique/plastique.dts";
   pickUpName = 'Shape Charge';
};

datablock ShapeBaseImageData(ShapeChargeImage)
{
   className = GrenadeImage;
   shapeFile = "art/editor/invisible.dts";
   computeCRC = false;
   cloakable = true;

   item = ShapeCharge;
   ammo = ShapeChargeAmmo;
   thrownItem = ShapeChargeTossed;
   throwTimeout = 800;
   
   mass = 2;
   mountPoint = 2;
   //offset     = "0 0 -0.1"; // L/R - F/B - T/B
   //offset = "0 +0.01 0";
   //rotation = "1 0 0 180";

   lightType = "NoLight";
   
   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "1 1 1";
   camShakeAmp = "6 6 6";
   camShakeDuration = "1.5";
   camShakeRadius = "1.2";

   stateName[0]                    = "Preactivate";
   stateSequence[0]                = "activation";
   stateTransitionOnTriggerDown[0] = "Activate";

   stateName[1]                    = "Activate";
   stateScript[1]                  = "onActivate";
   stateSequence[1]                = "fire";
   stateTransitionOnTriggerUp[1]   = "Deactivate";

   stateName[2]                    = "Deactivate";
   stateScript[2]                  = "onDeactivate";
   stateTimeoutValue[2]            = 0.5;
   stateTransitionOnTimeout[2]     = "Preactivate";
};

//-----------------------------------------------------------------------------

function ShapeChargeImage::onUnmount(%data, %obj, %slot)
{
   %obj.setImageTrigger(%slot, false);
   %obj.setInventory( %data.item, 0 );

   // If we have placed a charge delete it.
   if( isObject( %obj.thrownChargeId ) )
   {
      %obj.thrownChargeId.schedule(250, "delete");
      %obj.thrownChargeId = 0;
   }
}

function ShapeChargeImage::onActivate(%data, %obj, %slot)
{
   //LogEcho("ShapeChargeImage::onActivate(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");
   
   // Only one per customer!
   if ( isObject( %obj.thrownChargeId ) )
   {
      // If it's armed see how far from the player it is.
      if ( %obj.thrownChargeId.armed )
      {
         %range = %obj.thrownChargeId.getDataBlock().detonateRange;
         %dist = vectorDist(%obj.getWorldBoxCenter(), %obj.thrownChargeId.getWorldBoxCenter());
         if ( %dist < %range )
         {
            ServerPlay3D( ShapeChargeTriggerSound, %obj.thrownChargeId.getTransform() );
            %obj.thrownChargeId.getDataBlock().detonate( %obj.thrownChargeId, %obj );

			%obj.isReloading = false; //fix iron sight after shapeCharge detonate
            // Unmount the image, this will also remove inventory, we have to schedule this to avoid game crash
            %obj.schedule( 100, "dismountImage", %slot );
            //%obj.setInventory( %data.item, 0 );
         } 
         else
            messageClient( %obj.client, 'MsgError', 'You are %1 meters away from the charge. Need to be within %2 meters to detonate.', mFloor(%dist), %range);
      }
      return;
   }

   // Release the main weapon trigger and unmount the weapon
   if ( %obj.getMountedImage($WeaponSlot) != 0 )
   {
      %obj.setImageTrigger($WeaponSlot, false);
      %obj.unmountImage($WeaponSlot);
   }

   // Throw a charge
   %item = ItemData::create(ShapeChargeTossed);
   %obj.thrownChargeId = %item;
   %item.sourceObject = %obj;
   %item.static = false;
   %item.rotate = false;
   %item.armed = false;
   %item.checkCount = 0;
   MissionCleanup.add(%item);
   %obj.throwObject(%item);

   // Schedule a check to see if the ITEM is at rest but not stuck to anything
   %item.velocCheck = %item.getDataBlock().schedule(1000, "checkVelocity", %item);
}

function ShapeChargeImage::onDeactivate(%data, %obj, %slot)
{
   %obj.setImageTrigger(%slot, false);

   if ( %obj.inv[%obj.lastWeapon] )
      %obj.use( %obj.lastWeapon );
   else
      %obj.use( %obj.weaponSlot[0] );
}

//-----------------------------------------------------------------------------

function ShapeChargeTossed::onInventory(%data, %obj, %amount)
{
   if ( !%obj.isMemberOfClass( "Player" ) )
      return;

   //LogEcho("\c3ShapeChargeTossed::onInventory(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %amount SPC ")");

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

//-----------------------------------------------------------------------------

function ShapeChargeTossed::onCollision(%data, %obj, %col)
{
   //LogEcho("ShapeChargeTossed::onCollision(" SPC %data.getName() @", "@ %obj.getName() @", "@ %col.getClassName() SPC ")");

   // Lets keep thing from floating mid air, the check velocity should handle it afterwards
   if ( %col.getType() & ( $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::StaticShapeObjectType ) )
   {
      %vec = (-1.0 + getRandom() * 2.0) SPC (-1.0 + getRandom() * 2.0) SPC getRandom();
      %vec = vectorScale(%vec, 15);
      %pos = %col.getWorldBoxCenter();
      %obj.applyImpulse(%pos, %vec);
   }
}

function ShapeChargeTossed::checkVelocity(%data, %item)
{
   //LogEcho("ShapeChargeTossed::checkVelocity(" SPC %data.getName() @", "@ %item.getClassName() SPC ")");

   %item.checkCount++;
   if ( VectorLen( %item.getVelocity() ) < 0.1 )
   {
      // Item has come to rest but not activated, probably on a 
      // staticshape or vehicle etc. Lets force activation
      cancel(%item.velocCheck);
      %data.activateCharge( %item, posFromTransform( %item.getTransform() ), rotFromTransform( %item.getTransform() ) );
      return;
   }

   if(%item.checkCount >= 6)
   {
      // Items's still moving but it's been checked several times,
      // probably thrown off face of earth, delete it
      cancel( %item.velocCheck );
      if ( isObject( %item.sourceObject ) )
         %item.sourceObject.thrownChargeId = 0;

      %item.schedule( 100, "delete" );
   }
   else
   {
      // check back in a little while
      %item.velocCheck = %data.schedule( 1000, "checkVelocity", %item );
   }
}

function ShapeChargeTossed::onStickyCollision(%data, %item)
{
   //LogEcho("ShapeChargeTossed::onStickyCollision(" SPC %data.getName() @", "@ %item.sourceObject.client.nameBase SPC ")");
   // We have sticky! Lets setup for the actual charge
   cancel(%item.velocCheck);
   %pos = %item.getLastStickyPos();
   %norm = %item.getLastStickyNormal();
   %intAngle = getTerrainAngle(%norm);
   %rotAxis = vectorNormalize(vectorCross(%norm, "0 0 1"));
   if (getWord(%norm, 2) == 1 || getWord(%norm, 2) == -1)
      %rotAxis = vectorNormalize(vectorCross(%norm, "0 0 1"));

   %rot = %rotAxis @ " " @ %intAngle;
   %data.activateCharge(%item, %pos, %rot);
}

function ShapeChargeTossed::activateCharge(%data, %item, %pos, %rot)
{
   //LogEcho( "ShapeChargeTossed::activateCharge(" SPC %data.getName() @", "@ %item.getDataBlock().getName() @", "@ %pos @", "@ %rot SPC ")");
   %source = %item.sourceObject;

   // Create the charge and schedule arming
   %charge = new StaticShape() {
      dataBlock = ShapeChargeDeployed;
      sourceObject = %source;
      position = %pos;
      rotation = %rot;
   };
   MissionCleanup.add(%charge);
   %source.thrownChargeId = %charge;
   %charge.armed = false;
   %charge.damaged = 0;
   %charge.thwart = false;
   //messageClient(%source.client, 'MsgShapeChargePlaced', "\c2Shape charge deployed.");

   %item.schedule(50, "delete"); // Safe to remove the item now

   // arm itself 2.5 seconds after creation
   if ( %source.thrownChargeId != %charge )
      %charge.schedule( 100, "delete" );
   else
   {
      // "deet deet deet" sound, also need to play animation
      //%charge.playThread(0, "deploy");
      //%charge.playThread(1, "activate");
      schedule( %charge.getDatablock().armDelay, %charge, "armShapeCharge", %charge );
   }
}

function armShapeCharge(%item)
{
   //LogEcho("armShapeCharge(" SPC %item SPC ")");
   %item.playAudio( 1, ShapeChargeActivateSound );
   %item.armed = true;
}

function ShapeChargeDeployed::detonate(%data, %item, %player)
{
   //LogEcho("ShapeChargeDeployed::detonate(" SPC %data.getName() @", "@ %item @", "@ %player.client.nameBase SPC ")");
   // No boom boom unless armed!
   if ( !%item.armed )
      return;

   if ( %item.getDamageState() !$= Destroyed )
   {
      %item.setDamageState(Destroyed);
   }
}

function ShapeChargeDeployed::onDestroyed(%data, %obj, %prevState)
{
   // Doesn't matter if it was armed or not may have been destroyed by a weapon instead of source detonation.
   radiusDamage(%obj, %obj.sourceObject, %obj.getPosition(), %data.damageRadius, %data.radiusDamage, %data.damageType, %data.areaImpulse);

   // Free up the source to throw another
   if ( isObject( %obj.sourceObject ) )
      if ( %obj.sourceObject.thrownChargeId == %obj )
         %obj.sourceObject.thrownChargeId = 0;

   %obj.schedule(50, "delete");
}

//-----------------------------------------------------------------------------
// SMS Inventory
SmsInv.AllowGrenade("Soldier");
SmsInv.AddGrenade(ShapeCharge, "Shape Charge");
//             |Item| |InvGrenade| |AmmoIncrement|

SmsInv.AllowAmmo("armor\tSoldier\t1");
SmsInv.AddAmmo(ShapeChargeAmmo, 1);
