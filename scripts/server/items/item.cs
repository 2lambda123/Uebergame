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

// C++ side was increased from 4 to support a total of 6 mounted images
// C++ side of player to allow triggering of all slots except slot 2 for mounted images
// Some of these are hard coded -- 1 is jump jet, 2 is jump, 3 is crouch, 5 is sprint
$WeaponSlot = 0;
$AuxiliarySlot = 5;
$EffectsSlot = 4;
$GrenadeSlot = 2;
$SpecialSlot = 3;
$FlagSlot = 1;

// Respawntime is the amount of time it takes for a static "auto-respawn"
// object, such as an ammo box or weapon, to re-appear after it's been
// picked up.  Any item marked as "static" is automaticlly respawned.
$Item::RespawnTime = 60 * 1000;

// Poptime represents how long dynamic items (those that are thrown or
// dropped) will last in the world before being deleted.
$Item::PopTime = 30 * 1000;

//-----------------------------------------------------------------------------

function Item::respawn(%this)
{
   // This method is used to respawn static ammo and weapon items
   // and is usually called when the item is picked up.
   // Instant fade...
   %this.startFade(0, 0, true);
   %this.setHidden(true);

   // Shedule a reapearance
   %this.schedule($Item::RespawnTime, "setHidden", false);
   %this.schedule($Item::RespawnTime + 100, "startFade", 1000, 0, false);
}   

function Item::schedulePop(%this)
{
   // This method deletes the object after a default duration. Dynamic
   // items such as thrown or drop weapons are usually popped to avoid
   // world clutter.
   %this.schedule($Item::PopTime - 1000, "startFade", 1000, 0, true);
   %this.schedule($Item::PopTime, "delete");
}

function Item::schedulePopLong(%this)
{
   // This method deletes the object after a default duration. Dynamic
   // items such as thrown or drop weapons are usually popped to avoid
   // world clutter.
   %this.schedule(62000 - 1000, "startFade", 1000, 0, true);
   %this.schedule(62000, "delete");
}

function ItemData::onEnterLiquid(%data, %obj, %coverage, %type)
{    
   switch(%type)
   {
      case 0:
         //Water
      case 1:
         //Ocean Water
      case 2:
         //River Water
      case 3:
         //Stagnant Water
      case 4:
         //Lava
         %obj.delete();
      case 5:
         //Hot Lava
         %obj.delete();
      case 6:    
         //Crusty Lava
         %obj.delete();
      case 7:
         //Quick Sand
         %obj.delete();
   }
}

function ItemData::onLeaveLiquid(%data, %obj, %type)
{
   // dummy
}

//-----------------------------------------------------------------------------
// Callbacks to hook items into the inventory system

function ItemData::onInventory(%data, %obj, %amount)
{
   if ( !%obj.isMemberOfClass( "Player" ) )
      return;

   //LogEcho("\c3ItemData::onInventory(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %amount SPC ")");
   switch$ ( %data.className )
   {
      case "Weapon":
         if (!%amount && (%slot = %obj.getMountSlot(%data.image)) != -1)
            %obj.unmountImage(%slot);

      case "Special":
         // If we don't have any more of these items, make sure we don't have an image mounted.
         if ( %amount <= 0 )
         {
            // If we don't have any more of these items, make sure
            // we don't have an image mounted.
            %slot = %obj.getMountSlot( %data.image );
            if ( %slot != -1 )
               %obj.unmountImage( %slot );
         }
         else
         {
            // If we are adding this, remove anything that may be there and mount this
            if ( ( %oldImage = %obj.getMountedImage($SpecialSlot) ) != 0 )
               %obj.setInventory(%oldImage.item, 0);

            %obj.mountImage(%data.image, $SpecialSlot);
         }

          if ( isObject(%obj.client) && !%obj.client.isAiControlled() )
            messageClient(%obj.client, 'MsgSpecialCnt', "", addTaggedString($DataToName[%data.getName()]), addTaggedString(%amount));

      case "HandInventory":
         // If we don't have any more of these items, make sure we don't have an image mounted.
         if (!%amount && (%slot = %obj.getMountSlot(%data.image)) != -1)
            %obj.unmountImage(%slot);
         else
         {
            // If we are adding this, remove anything that may be there and mount this
            if ( ( %oldImage = %obj.getMountedImage($GrenadeSlot) ) != 0 )
               %obj.setInventory(%oldImage.item, 0);

            %obj.mountImage(%data.image, $GrenadeSlot);
         }

          if ( isObject(%obj.client) && !%obj.client.isAiControlled() )
            messageClient(%obj.client, 'MsgGrenadeCnt', "", addTaggedString($DataToName[%data.getName()]), addTaggedString(%obj.getInventory(%data.image.ammo)));

      case "Ammo" or "AmmoClip":
         // The ammo inventory state has changed, we need to update any
         // mounted images using this ammo to reflect the new state.
         for (%i = 0; %i < 8; %i++)
         {
            if ((%image = %obj.getMountedImage(%i)) > 0)
            {
               if (isObject(%image.ammo) && %image.ammo.getId() == %data.getId())
               {
                  %obj.setImageAmmo(%i, %amount != 0);
                  %currentAmmo = %obj.getInventory(%data);
            
                  if (%obj.getClassname() $= "Player")
                  {
                     if ( isObject( %image.clip ) )
                     {
                        %amountInClips = %obj.getInventory(%image.clip);
                        //%amountInClips *= %obj.maxInventory(%image.clip);
                        %amountInClips += %obj.getFieldValue( "remaining" @ %data.getName() );
                     }
                     else //Is a single fire weapon, like the grenade launcher.
                     {
                        %amountInClips = 0;
                     }
               
                     if ( isObject(%obj.client) && !%obj.client.isAiControlled() )
                        //messageClient(%obj.client, 'MsgAmmoCnt', "", addTaggedString($DataToName[%image.item]), %i, addTaggedString(%currentAmmo), addTaggedString(%amountInClips));
					    messageClient(%obj.client, 'MsgAmmoCnt', "", addTaggedString($DataToName[%image.item]), %i, %currentAmmo, %amountInClips); //removed tagged strings to prevent console spam
                  }
               }
            }
         }

      default:
         if ( %amount <= 0 )
         {
            // If we don't have any more of these items, make sure
            // we don't have an image mounted.
            %slot = %obj.getMountSlot( %data.image );
            if ( %slot != -1 )
               %obj.unmountImage( %slot );
         }
   }
}

function ItemData::onUse(%data, %obj)
{
   //echo("ItemData::onUse(" SPC %data.getName() @", "@ %obj.client.nameBase SPC ")");
   //if ( !%obj.getType() & ( $TypeMasks::PlayerObjectType ) )
   if ( !%obj.isMemberOfClass( "Player" ) )
      return;

   switch$ ( %data.className )
   {
      case "Weapon":
         if ( %obj.getMountedImage($WeaponSlot) != %data.image.getId() )
            %obj.mountImage( %data.image, $WeaponSlot );

         if ( isObject(%obj.client) ) // Send a callback
            messageClient(%obj.client, 'MsgWeaponUsed', "", %data.pickUpName);

         // Team skinned weapons
         //%data.image.skin = addTaggedString("team" @ %obj.team);
         //%obj.mountImage(%data.image, $WeaponSlot, false, %data.image.skin);
/*
      case "Special":
         if (%obj.getMountedImage($SpecialSlot) != %data.image.getId())
            %obj.mountImage(%data.image, $SpecialSlot);
         else
            %obj.setImageTrigger($SpecialSlot, !%obj.getImageTrigger($SpecialSlot));
*/
   }
}

// Default item pickup.
// If the player has max of the item or is not allowed the item or allready has the item, do nothing.
function ItemData::onCollision(%data, %obj, %col, %vec, %speed)
{
   //error("\c3ItemData::onCollision( " @ %data.getName() @ ", " @ %obj.getClassName() @ ", " @ %col.getClassName() @ ", " @ %vec @ ", " @ %speed @ " )");
   // Default behavior for items is to get picked up by the colliding object.
   //if ( !isObject(%obj) || (!%obj.getType() & ( $TypeMasks::PlayerObjectType )) || %col.getState() $= "Dead" || %col.isMounted() )
   if ( !isObject( %obj ) || !%col.isMemberOfClass( "Player" ) || %col.getState() $= "Dead" || %col.isMounted() )
   return;

   // We check the classname defined in the datablock for action
   switch$ ( %data.className )
   {
      case "Weapon":
         // Certain weapons are not checked against the weapon count
         //if( %data.getName() !$= "Grenade" && %data.getName() !$= "FlashGrenade" && 
         //  ( %col.weaponCount >= %col.getDatablock().maxWeapons ) )
         //   return 0;

         if ( %col.weaponCount >= %col.getDatablock().maxWeapons && $SMS::ShowInInv[%data] )
            return 0;

         %inv = %col.incInventory( %data, 1 );
         if ( %inv > 0 )
         {
            if ( %col.getMountedImage( $WeaponSlot ) == 0 )
               %col.use( %data.getName() );

            if ( %data.pickupSound !$= "" )
               serverPlay3D( %data.pickupSound, %col.getTransform() );

            if ( isObject(%col.client) )
               messageClient( %col.client, 'MsgItemPickup', "", %data.pickupName );

            if ( %obj.isStatic() )
               %obj.respawn();
            else
               %obj.delete();
         }

      case "Special":
         if ( %col.getMountedImage($SpecialSlot) != 0 )
            return 0;
         else
         {
            %inv = %col.incInventory( %data, 1 );
            if ( %inv > 0 )
            {
               if ( %data.pickupSound !$= "" )
                  serverPlay3D( %data.pickupSound, %col.getTransform() );

               if ( isObject( %col.client ) )
                  messageClient( %col.client, 'MsgItemPickup', "", %data.pickupName );

               if ( %obj.isStatic() )
                  %obj.respawn();
               else
                  %obj.delete();
            }
         }

      case "HandInventory":
         if ( %col.getMountedImage($GrenadeSlot) != 0 )
            return 0;
         else
         {
            %inv = %col.incInventory( %data, 1 );
            if ( %inv > 0 )
            {
               if ( %data.pickupSound !$= "" )
                  serverPlay3D( %data.pickupSound, %col.getTransform() );

               if ( isObject( %col.client ) )
                  messageClient( %col.client, 'MsgItemPickup', "", %data.pickupName );

               if ( %obj.isStatic() )
                  %obj.respawn();
               else
                  %obj.delete();
            }
         }

      case "AmmoClip":
         %ammoName = %data.getName();
         %aMax = %col.getDataBlock().maxInv[%ammoName];
         if(%obj.ammoStore $= "")
            %obj.ammoStore = $AmmoIncrement[ %ammoName ];

         %inv = %col.incInventory( %ammoName, %obj.ammoStore );
         if ( %inv > 0 )
         {
            if ( %data.pickupSound !$= "" )
               serverPlay3D( %data.pickupSound, %col.getTransform() );

            if ( isObject( %col.client ) )
               messageClient( %col.client, 'MsgItemPickup', "", %data.pickUpName, %inv );

            if ( %obj.isStatic() )
               %obj.respawn();
            else
               %obj.delete();
         }

      case "Ammo":

	  
      default:
         %inv = %col.incInventory(%data, 1);
         if ( %inv > 0 )
         {
            if ( %data.pickupSound !$= "" )
               serverPlay3D( %data.pickupSound, %col.getTransform() );

            if ( isObject( %col.client ) )
               messageClient( %col.client, 'MsgItemPickup', "", %data.pickUpName );

            if ( %obj.isStatic() )
               %obj.respawn();
            else
               %obj.delete();
         }
   }
}

//-----------------------------------------------------------------------------
// Hook into the mission editor. We also use this to spawn items via script.

function ItemData::create(%data)
{
   // The mission editor invokes this method when it wants to create
   // an object of the given datablock type.  For the mission editor
   // we always create "static" re-spawnable rotating objects.
   %obj = new Item() {
      dataBlock = %data;
      static = true;
      rotate = false;
      scale = %data.scale !$= "" ? %data.scale : "1 1 1";
   };
   return %obj;
}

//-----------------------------------------------------------------------------
// Default Weapon Item and Ammo for inheriting
//-----------------------------------------------------------------------------

datablock ItemData(DefaultWeapon)
{
   // Mission editor category, this datablock will show up in the
   // specified category under the "shapes" root category.
   category = "Weapon";

   // Add the Weapon namespace as a parent.  The weapon namespace provides
   // common weapon related functions and hooks into the inventory system.
   className = "Weapon";

   // Basic Item properties
   computeCRC = false;
   emap = true;
   mass = 5;
   drag = 0.5;
   density = 10;
   elasticity = 0.2;
   friction = 0.6;
   sticky = false;
   gravityMod = 1;
   maxVelocity = 20;
   dynamicType = $TypeMasks::ItemObjectType;

   lightType = "NoLight";

   simpleServerCollision = true;

   // Dynamic properties defined by the scripts
   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;
};

datablock ItemData(DefaultClip)
{
   category = "AmmoClip";
   className = "AmmoClip";
   shapeFile = "art/shapes/weapons/Lurker/Lurker_clip.dts";
   computeCRC = false;
   emap = true;
   renderWhenDestroyed = true;
   mass = 2;
   drag = 0.5;
   density = 2;
   elasticity = 0.2;
   friction = 0.6;
   sticky = false;
   gravityMod = 1;
   maxVelocity = 20;
   dynamicType = $TypeMasks::ItemObjectType;

   simpleServerCollision = true;

   lightType = "NoLight";

   pickUpName = 'Magazine';
   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;
};

datablock ItemData(DefaultAmmo)
{
   category = "Ammo";
   className = "Ammo";
   shapeFile = "art/shapes/weapons/Lurker/Lurker_clip.dts";
   computeCRC = false;
   emap = true;
   renderWhenDestroyed = true;
   mass = 1;
   drag = 0.5;
   density = 2;
   elasticity = 0.2;
   friction = 0.6;
   sticky = false;
   gravityMod = 1;
   maxVelocity = 20;
   dynamicType = $TypeMasks::ItemObjectType;

   simpleServerCollision = true;

   lightType = "NoLight";

   pickUpName = 'Ammo';
   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;
};

//-----------------------------------------------------------------------------

$RandomItemsAdded = 0;

// ZOD: example - placeRandomItem(Medpack_medium, 50, 10, 2000, "0 0 100");
function placeRandomItem(%data, %num, %spacing, %radius, %startPos)
{
   if(%spacing $= "" || %spacing == -1)
      %spacing = 5.0;

   %maxSlope = 45;
   %zOffset = 0.5;
   %slopeWithTerrain = 1;

   //set up folders in mis file
   $RandomItemsAdded++;
   if(!isObject(RandomItems))
   {
      %randomItemGroup = new simGroup(RandomItems);
      MissionCleanup.add(%randomItemGroup);
   }
   %groupName = "Additional" @ $RandomItemsAdded @ %data;
   %group = new simGroup(%groupName);
   RandomItems.add(%group);

   if(%startPos !$= "" && %startPos != -1)
      %ctr = %startPos;
   else 
      %ctr = "0 0 0";

   %areaX = getWord(%ctr, 0) - %radius;
   %areaY = getWord(%ctr, 1) - %radius;

   %itemCount = %num;
   while((%itemCount > 0) && (%retries < (15000 / %maxSlope)))
   {
      %x = (getRandom(mFloor(%areaX / 8), mFloor((%areaX + (%radius * 2)) / 8)) * 8) + 4;  //tile center
      %y = (getRandom(mFloor(%areaY / 8), mFloor((%areaY + (%radius * 2)) / 8)) * 8) + 4;

      %start = %x @ " " @ %y @ " 2000";
      %end = %x @ " " @ %y @ " -1";
      %ground = containerRayCast(%start, %end, $TypeMasks::TerrainObjectType, 0);
      %z = getWord(%ground, 3);
      %z += %zOffset;
      %position = %x @ " " @ %y @ " " @ %z;

      %start = %x + 2 @ " " @ %y @ " 2000";
      %end = %x + 2 @ " " @ %y @ " -1";
      %hit1 = containerRayCast(%start, %end, $TypeMasks::TerrainObjectType, 0);

      %start = %x - 2 @ " " @ %y @ " 2000";
      %end = %x - 2 @ " " @ %y @ " -1";
      %hit2 = containerRayCast(%start, %end, $TypeMasks::TerrainObjectType, 0);

      %norm1 = getWord(%hit1, 4) @ " " @ getWord(%hit1, 5) @ " " @ getWord(%hit1, 6);
      %norm2 = getWord(%hit2, 4) @ " " @ getWord(%hit2, 5) @ " " @ getWord(%hit2, 6);

      %angNorm1 = getTerrainAngle(%norm1);
      %angNorm2 = getTerrainAngle(%norm2);
      if ((getTerrainAngle(%norm1) > %maxSlope) || (getTerrainAngle(%norm2) > %maxslope))
      {
         %retries++;
         continue;
      }

      %terrainNormal = VectorAdd(%norm1, %norm2);
      %terrainNormal = VectorNormalize(%terrainNormal);

      InitContainerRadiusSearch(%position, %spacing,  $TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType |
                                $TypeMasks::MoveableObjectType | $TypeMasks::StaticShapeObjectType |
                                $TypeMasks::StaticTSObjectType | $TypeMasks::InteriorObjectType |
                                $TypeMasks::ItemObjectType);

      %this = containerSearchNext();
      if(%this)
      {
         if(%this.getClassName() !$= fxSunLight && %this.getClassName() !$= fxShapeReplicator)
         {
            //echo("Found an obstacle" SPC %this.getClassName() SPC "continuing");
            %retries++;
            continue;
         }
      }

      if(%slopeWithTerrain)
      {
         %rotAxis = vectorCross(%terrainNormal, "0 0 1");
         %rotAxis = vectorNormalize(%rotAxis);
         %rotation = %rotAxis @ " " @ getTerrainAngle(%terrainNormal);
      }
      else
         %rotation = "1 0 0 0";

      %randomAngle = getRandom(360);
      %zrot = MatrixCreate("0 0 0", "0 0 1 " @ %randomAngle);
      %orient = MatrixCreate(%position, %rotation);
      %finalXForm = MatrixMultiply(%orient, %zrot);

      %newItem = new Item() {
         dataBlock = %data;
         position  = %x SPC %y SPC (%z += %zoffset);
         rotation  = %rotation;
         static = true;
         rotate = true;
         collideable = true;
         scale  = %data.scale ? %data.scale : "1 1 1";
      };
      %group.add(%newItem);
      %newItem.setTransform(%finalXForm);

      %itemCount--;
      %retries = 0;
   }
   if (%itemCount > 0)
   {
      error("Unable to place all shapes, area saturated.");
      error("Looking for clear area " @ (%spacing * 2) @ " meters in diameter, with a max slope of " @ %maxSlope);
   }
   echo("Placed " @ %num - %itemCount @ " of " @ %num);
}
