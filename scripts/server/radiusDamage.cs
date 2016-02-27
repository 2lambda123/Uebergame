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

function ProjectileData::onCollision(%data, %proj, %col, %fade, %pos, %normal)
{
   //echo("ProjectileData::onCollision(" SPC %data.getName() @", "@ %proj @", "@ %col.getClassName() @", "@ %fade @", "@ %pos @", "@ %normal SPC ")");

   // Apply damage to the object all shape base objects
   if ( %data.directDamage > 0 )
   {
      if ( %col.getType() & ( $TypeMasks::ShapeBaseObjectType ) )
          %col.damage( %proj.origin, %pos, %data.directDamage, %data.damageType );
   }

   if ( isObject( %proj.light ) )
      %proj.light.delete();
}

function ProjectileData::onExplode(%data, %proj, %position, %mod)
{
   //echo("ProjectileData::onExplode(" SPC %data.getName() @", "@ %proj @", "@ %position @", "@ %mod SPC ")");

   // Damage objects within the projectiles damage radius
   if ( %data.damageRadius > 0 )
      radiusDamage( %proj, %proj.origin, %position, %data.damageRadius, %data.radiusDamage, %data.damageType, %data.areaImpulse );
}

// Non stock C++ callback
function ProjectileData::onRemove(%data, %proj)
{
   //warn("ProjectileData::onRemove(" SPC %data.getName() @", "@ %proj.getClassName() SPC ")");
}

// Support function which applies damage to objects within the radius of
// some effect, usually an explosion.  This function will also optionally 
// apply an impulse to each object.
function radiusDamage(%source, %sourceParent, %position, %radius, %damage, %damageType, %impulse)
{
   //echo("radiusDamage(" SPC %sourceParent @", "@ %position @", "@ %radius @", "@ %damage @", "@ %damageType @", "@ %impulse SPC ")");
   // Use the container system to iterate through all the objects
   // within our explosion radius.  We'll apply damage to all ShapeBase
   // objects.
   InitContainerRadiusSearch( %position, %radius, $TypeMasks::ShapeBaseObjectType );

   // Create an array of targets to damage and or impulse
   %count = 0;
   %halfRadius = %radius / 2;
   %sourceBlock = %source.getDataBlock(); // Used further down

   while( ( %targetObject = containerSearchNext() ) != 0 )
   {
      // Certain explosions need to ignore the source. Filter them out right away.
      if ( %targetObject == %sourceParent && %damageType $= "Vehicle Explosion" )
         continue;

      // Calculate how much exposure the current object has to
      // the explosive force.  The object types listed are objects
      // that will block an explosion.  If the object is totally blocked,
      // then no damage is applied.
      %coverage = calcExplosionCoverage(%position, %targetObject, $TypeMasks::InteriorObjectType |
                                                                  $TypeMasks::TerrainObjectType |
                                                                  $TypeMasks::VehicleObjectType);


      if ( %coverage <= 0 )
         continue;

      // Radius distance subtracts out the length of smallest bounding
      // box axis to return an appriximate distance to the edge of the
      // object's bounds, as opposed to the distance to it's center.
      %dist = containerSearchCurrRadiusDist();

      // Calculate a distance scale for the damage and the impulse.
      // Full damage is applied to anything less than half the radius away,
      // linear scale from there.
      %distScale = (%dist < %halfRadius) ? 1.0 : 1.0 - ((%dist - %halfRadius) / %halfRadius);
      //%amount = (1.0 - (%dist / %radius)) * %coverage * %damage;
      //error("Amount:" SPC %amount);

      if (%distScale > %radius)
         continue;

      // See if the shape is protected
      if ( %targetObject.isMounted() )
      {
         %mount = %targetObject.getObjectMount();
         %found = -1;
         for ( %i = 0; %i < %mount.getDataBlock().numMountPoints; %i++ )
         {
            if ( %mount.getMountNodeObject(%i) == %targetObject )
            {
               %found = %i;
               break;
            }
         }

         if ( %found != -1 )
         {
            if ( %mount.getDataBlock().isProtectedMountPoint[%found] )
            {
               continue;
            }
         }
      }

      // Stuff each target into an array.
      %target[%count] = %targetObject;
      %distance[%count] = %distScale;
      %damage[%count] = %damage * %coverage * %distScale;
      %count++;
   }

   // Loop through our target array and apply the damage etc.
   for( %i = 0; %i < %count; %i++)
   {
      %targetObject = %target[%i];
      if ( isObject( %targetObject ) )
      {
         %data = %targetObject.getDataBlock();
         %className = %targetObject.getClassName();

         // Apply the damage directly to the objects datablock
         if ( %distance[%i] > 0 )
            %targetObject.damage( %sourceParent, %position, %damage[%i], %damageType );

         if ( %className $= "Player" || %className $= "AiPlayer" )
         {
            // Now we will apply different affects based on parameters in the sources datablock
            if ( %sourceBlock.applyEmp && %sourceParent != %targetObject && %sourceParent.team != %targetObject.team )
               %targetObject.setEnergyLevel(0);
            if ( %sourceBlock.applyConcussion && %sourceParent != %targetObject && %sourceParent.team != %targetObject.team )
               %data.applyConcussion( %targetObject );
         }

         // Apply the impulse to objects that specifically allow it.
         // We do this because if the source object gets an impulse applied it will hang the game engine.
         // Also it is nice to have options for gameplay reasons.
         if ( %impulse > 0 && %data.canImpulse == true )
         {
            // We want a different impulse for certain projectiles if the source hit themslves
            if ( %sourceParent == %targetObject )
               %kick = %sourceBlock.sourceImpulse $= "" ? %impulse : %sourceBlock.sourceImpulse;
            else
               %kick = %impulse;

            %vec = VectorSub( %targetObject.getWorldBoxCenter(), %position );
            %vec = VectorNormalize( %vec );
            %vec = VectorScale( %vec, %kick * %distance[%i] );
            %targetObject.applyImpulse( %position, %vec );
         }
      }
   }
}
