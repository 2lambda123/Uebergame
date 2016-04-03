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

//-----------------------------------------------------------------------------
// Health Patchs cannot be picked up and are not meant to be added to
// inventory.  Health is applied automatically when an objects collides
// with a patch.
//-----------------------------------------------------------------------------

datablock SFXProfile(HealthPatchSound)
{
   filename = "art/sound/items/health_mono_01.ogg";
   description = AudioClosest3D;
   preload = true;
};

datablock ItemData(Medpack_medium)
{
   // Mission editor category, this datablock will show up in the
   // specified category under the "shapes" root category.
   category = "Health";

   className = "HealthPatch";

   // Basic Item properties
   shapeFile = "art/shapes/items/health/medpack_01.dts";
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
   repairAmount = 50;
   repairInstant = 30;

   alwaysAmbient = true;

   lightType = "NoLight";

   pickupSound = HealthPatchSound;
   throwSound = WeaponThrowSound;
};

function HealthPatch::onAdd(%this, %obj)
{
   //echo("HealthPatch::onAdd(" SPC %this @", "@ %obj SPC ")");
   Parent::onAdd(%this, %obj);
   %obj.playThread(0, "Ambient");
}

function HealthPatch::onCollision(%this, %obj, %col, %vec, %speed)
{
   // Now I am thinking we can expand this to include other shapebase derivatives such as turrets staticshapes etc.
   if ( %col.getType() & ( $TypeMasks::PlayerObjectType ) )
   {
      if ( %col.getState() $= "Dead" || %col.isMounted() )
         return;

      // Apply health to colliding object if it needs it. Works for all shapebase objects.
      if ( %col.getDamageLevel() > 0 )
      {
         %col.applyRepair(%this.repairAmount);
		 
		 %dam = %col.getDamageLevel();
         %dam = %dam - (%this.repairInstant); //instantly added health
           if(%dam < 0)
              %dam = 0;
		  
		 %col.setDamageLevel(%dam);
		 
         serverPlay3D( HealthPatchSound, %col.getTransform());
         //if ( isObject( %col.client ) )
            //messageClient(%col.client, 'MsgHealthPatchUsed', '\c2Health Applied');
		
         if(%obj.isStatic())
            %obj.respawn();
         else
            %obj.delete();
      }
   }
   else
   {
      if ( %col.getType() & ( $TypeMasks::StaticShapeObjectType | $TypeMasks::TurretObjectType | $TypeMasks::VehicleObjectType ) )
      {
         if ( %col.getDamageLevel() > 0 )
         {
            %col.setDamageLevel( %col.getDamageLevel() - %this.repairAmount );
            if(%obj.isStatic())
               %obj.respawn();
            else
               %obj.delete();
         }
      }
   }
}

function ShapeBase::tossPatch(%this)
{
   //error("ShapeBase::tossPatch(" SPC %this.client.nameBase SPC ")");
   if(!isObject(%this))
      return;

   %item = ItemData::create(Medpack_medium);
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
   %item.setCollisionTimeout(%this);
   serverPlay3D(%item.getDataBlock().throwSound, %item.getTransform());
   %item.schedulePop();

   return %item;
}
