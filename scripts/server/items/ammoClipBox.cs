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

datablock ItemData(AmmoClipBox : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/Lurker/Lurker_clip.dts";
   computeCRC = false;
   mass = 10;
   maxVelocity = 12;
   pickUpName = 'ammo clip crate';
};

function AmmoClipBox::onCollision(%data, %obj, %col)
{
   if ( %col.getDataBlock().className $= Armor && %col.getState() !$= "Dead" ) 
   {
      // Do not allow the thrower to pick up the ammo tossed and do not 
      // pick up ammo while in a vehicle or turret.
      if( %col.isMounted() )//|| %obj.sourceObject == %col )
         return;

      %gotSomething = 0;

      for(%i = 0; %i < %col.weaponSlotCount; %i++)
      {
         %weapon = %col.weaponSlot[%i];
         // See if the weapon needs ammo
         if ( %weapon.image.clip !$= "" )
         {
            %increase = %col.incInventory( %weapon.image.clip, 1 );
            if ( %increase > 0 )
            {
               %gotSomething = 1;
            }
         }
      }

      if ( %gotSomething )
      {
         serverPlay3D( %data.pickupSound, %col.getTransform() );
         if(%obj.isStatic())
            %obj.respawn();
         else
            %obj.delete();
      }
   }
}

function ShapeBase::tossAmmoClipCrate(%this)
{
   //error("ShapeBase::tossAmmoCrate(" SPC %this.client.nameBase SPC ")");
   if(!isObject(%this))
      return;

   %item = ItemData::create(AmmoClipBox);
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
