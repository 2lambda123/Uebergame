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

datablock ItemData(HealthKit)
{
   category = "Specials";

   shapeFile = "art/shapes/items/kit/healthkit.dts";
   computeCRC = false;
   emap = 1;
   mass = 5;
   drag = 0.5;
   density = 2;
   elasticity = 0.2;
   friction = 0.6;
   sticky = false;
   gravityMod = 1;
   maxVelocity = 100;
   dynamicType = $TypeMasks::ItemObjectType;

   // Dynamic properties defined by the scripts
   pickupName = 'Health Kit';
   repairAmount = 50;

   lightType = "NoLight";

   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;
};

function HealthKit::onAdd(%this, %obj)
{
   //echo("HealthKit::onAdd(" SPC %this @", "@ %obj SPC ")");
   Parent::onAdd(%this, %obj);
   %obj.playThread(0, "Ambient");
}

function HealthKit::onUse(%this, %player)
{
   //echo("HealthKit::onUse( " @ %this.getName() @ ", " @ %player.client.nameBase @ " )");
   // Apply some health to whoever uses it, the health kit is only
   // used if the user is currently damaged.
   if(%player.getInventory(%this) > 0)
   {
      if(%player.getDamageLevel() > 0)
      {
         // Decrease the players special level
         %player.decreaseSpecialLevel(50);

         %player.decInventory(%this, 1);
         %player.applyRepair(%this.repairAmount);
      }
      else
      {
         if(%player.client)
            messageClient(%player.client, 'MsgHealthKitUsed', '\c2Repair not needed.');
      }
   }
}
//-----------------------------------------------------------------------------
// SMS

SmsInv.AllowItem("armor\tSoldier\t1");
//SmsInv.AddItem("HealthKit", "Health Kit", 1);
