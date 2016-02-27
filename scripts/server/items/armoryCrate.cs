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

datablock ItemData(ArmoryCrate)
{
   category = "Stations";
   shapeFile = "art/shapes/storage/crates/weapons/weapon_box_01.dts";
   scale = "0.2 0.2 0.2";
   computeCRC = false;
   emap = 1;
   mass = 20;
   drag = 0.5;
   density = 5;
   elasticity = 0.3;
   friction = 0.6;
   sticky = false;
   gravityMod = 1;
   maxVelocity = 30;
   dynamicType = $TypeMasks::ItemObjectType;

   pickupRadius = 5;
   pickUpName = 'armory crate';

   lightType = "NoLight";

   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;
   repairAmount = 0.2;
};

function ArmoryCrate::onCollision(%data, %obj, %col)
{
   if ( %col.getType() & ( $TypeMasks::PlayerObjectType ) )
   {
      if ( %col.getState() $= "Dead" || %col.isMounted() )
         return;

      if ( isObject( %col.client ) )
      {
         if ( %col.client.isAiControlled() )
            %client.ProcessLoadout();
         else
            SmsInv.ProcessLoadout(%col.client);

         // Mount a weapon
         if ( %col.inv[%col.lastWeapon] )
            %col.use( %col.lastWeapon );
         else
         {
            if ( %col.getMountedImage($WeaponSlot) == 0 ) 
               %col.use( %col.weaponSlot[0] );
         }
      }

      if ( %col.getDamageLevel() > 0 )
         %col.applyRepair( %data.repairAmount );

      serverPlay3D( %data.pickupSound, %col.getTransform() );
      %obj.delete();
   }
}
