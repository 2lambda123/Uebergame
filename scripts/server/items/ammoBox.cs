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

datablock ItemData(AmmoBox : DefaultAmmo)
{
   shapeFile = "art/shapes/items/ammo/futuristic_ammo_box_01.dts";
   computeCRC = false;
   mass = 10;
   maxVelocity = 50;
   pickUpName = 'ammo crate';
};

function AmmoBox::onCollision(%data, %obj, %col)
{
   if ( %col.getDataBlock().className $= Armor && %col.getState() !$= "Dead" ) 
   {
      // Do not allow the thrower to pick up the ammo tossed and do not 
      // pick up ammo while in a vehicle or turret.
      if( %col.isMounted() )//|| %obj.sourceObject == %col )
         return;

      %gotSomething = 0;

      // This will give the player the max ammo for weapons that are mounted.
      // We loop through the array to see if the player has a weapon listed (This is an array of itemdata).
      // Then we check to see if that weapons image has an ammo parameter.
      // If it does have an ammo parameter we increase the
      // players ammo for the found weapon to the players max load.
      for(%i = 0; %i < %col.weaponSlotCount; %i++)
      {
         %weapon = %col.weaponSlot[%i];
         // See if the weapon needs ammo
         if ( %weapon.image.ammo !$= "" )
         {
            %ammoName = %weapon.image.ammo;
            //%increase = %col.incInventory(%ammoName, %col.getDataBlock().maxInv[%ammoName]);
            // Or
            %increase = %col.incInventory( %ammoName, $AmmoIncrement[%ammoName] );
            if ( %increase > 0 )
            {
               %gotSomething = 1;
               //if ( isObject( %col.client ) )
                  //messageClient(%col.client, 'MsgItemPickup', '\c2You picked up %2 %1\s.', nameToID(%ammoName).pickUpName, %increase);
            }
         }
      }

      // Grenades
      %grenade = (%col.getMountedImage($GrenadeSlot) == 0 ) ? "" : %col.getMountedImage($GrenadeSlot).ammo;
      //%added = %grenade $= "" ? 0 : %col.incInventory( %grenade, %col.getDataBlock().maxInv[%grenade] );
      // Or
      %added = %grenade $= "" ? 0 : %col.incInventory( %grenade, $AmmoIncrement[%grenade] );

      if ( %added > 0 )
      {
         %gotSomething = 1;
         //if ( isObject( %col.client ) )
         //   messageClient(%col.client, 'MsgGrenadeCnt', "", addTaggedString($DataToName[%grenade]), addTaggedString(%col.getInventory(%grenade)));
      }

      if ( %gotSomething )
      {
         //if ( isObject( %col.client ) )
            //messageClient( %col.client, 'MsgItemPickup', '\c2Ammunition replenished.' );

         serverPlay3D( %data.pickupSound, %col.getTransform() );
         if(%obj.isStatic())
            %obj.respawn();
         else
            %obj.delete();
      }
   }
}

function ShapeBase::tossAmmoCrate(%this)
{
   //error("ShapeBase::tossAmmoCrate(" SPC %this.client.nameBase SPC ")");
   if(!isObject(%this))
      return;

   %item = ItemData::create(AmmoBox);
   %item.sourceObject = %this;
   %item.static = false;
   MissionCleanup.add(%item);

   %vec = (-1.0 + getRandom() * 2.0) SPC (-1.0 + getRandom() * 2.0) SPC getRandom();
   %vec = vectorScale(%vec, 20);
   %eye = %this.getEyeVector();
   %dot = vectorDot("0 0 1", %eye);
   if (%dot < 0)
      %dot = -%dot;

   %vec = vectorAdd(%vec, vectorScale("0 0 12", 1 - %dot));
   %vec = vectorAdd(%vec, %this.getVelocity());
   %pos = getBoxCenter(%this.getWorldBox());
   %item.setTransform(%pos);
   %item.applyImpulse(%pos, %vec);
   // This causes server crash
   %item.setCollisionTimeout(%this);
   serverPlay3D(%item.getDataBlock().throwSound, %item.getTransform());
   %item.schedulePop();

   return %item;
}
