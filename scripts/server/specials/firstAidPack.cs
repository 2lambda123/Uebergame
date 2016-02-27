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

datablock ItemData(FirstAidPack)
{
   // Mission editor category, this datablock will show up in the
   // specified category under the "shapes" root category.
   category = "Specials";
   className = "Special";

   // Basic Item properties
   shapeFile = "art/shapes/items/ammo/futuristic_ammo_box_01.dts";
   computeCRC = false;
   mass = 2;
   drag = 0.5;
   density = 2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   maxVelocity = 20;
   dynamicType = $TypeMasks::ItemObjectType;

   image = FirstAidPackImage;

   pickUpName = 'Medical';
   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;

   lightType = "NoLight";
};

datablock ShapeBaseImageData(FirstAidPackImage)
{
   shapeFile = "art/editor/invisible.dts";
   computeCRC = false;
   cloakable = true;
   item = FirstAidPack;
   mountPoint = 2;
   mass = 2;
   offset = "0 0 0";

   throwTimeout = 800;
   usesEnergy = true;
   minEnergy = 20;
   fireEnergy = 20;

   lightType = "NoLight"; // NoLight, ConstantLight, PulsingLight, WeaponFireLight.

   stateName[0]                    = "Preactivate";
   stateSequence[0]                = "activation";
   stateTransitionOnTriggerDown[0] = "Activate";

   stateName[1]                    = "Activate";
   stateScript[1]                  = "onActivate";
   stateSequence[1]                = "fire";
   stateTransitionOnTriggerUp[1]   = "Deactivate";
   stateTransitionOnNoAmmo[1]      = "Deactivate";

   stateName[2]                    = "Deactivate";
   stateScript[2]                  = "onDeactivate";
   stateTimeoutValue[2]            = 0.5;
   stateTransitionOnTimeout[2]     = "Preactivate";
};

function FirstAidPackImage::onMount(%data, %obj, %slot)
{
}

function FirstAidPackImage::onUnmount(%data, %obj, %slot)
{
   %obj.setImageTrigger(%slot, false);
}

function FirstAidPackImage::onActivate(%data, %obj, %slot)
{
   //echo("FirstAidPackImage::onActivate(" SPC %data.getName() @", "@ %obj.client.nameBase @" , "@ %slot SPC ")");

   %obj.setEnergyLevel(%obj.getEnergyLevel() - %data.fireEnergy);

   // Release the main weapon trigger and unmount the weapon
   if ( %obj.getMountedImage($WeaponSlot) != 0 )
   {
      %obj.setImageTrigger($WeaponSlot, false);
      %obj.unmountImage($WeaponSlot);
   }

   // Throw a health patch
   %item = ItemData::create(Medpack_medium);
   %item.sourceObject = %obj;
   %item.static = false;
   MissionCleanup.add(%item);
   %obj.throwObject(%item);

   // Delete it after a while. Dont want to litter up the mission
   %item.schedulePop();
}

function FirstAidPackImage::onDeactivate(%data, %obj, %slot)
{
   %obj.setImageTrigger(%slot, false);

   if ( %obj.inv[%obj.lastWeapon] )
      %obj.use( %obj.lastWeapon );
   else
      %obj.use( %obj.weaponSlot[0] );
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowItem("armor\tSoldier\t1");
SmsInv.AddItem(FirstAidPack, "Medical", 1);
