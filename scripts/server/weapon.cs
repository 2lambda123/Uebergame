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

// This file contains Weapon and Ammo Class/"namespace" helper methods
// as well as hooks into the inventory system. These functions are not
// attached to a specific C++ class or datablock, but define a set of
// methods which are part of dynamic namespaces "class". The Items
// include these namespaces into their scope using the  ItemData and
// ItemImageData "className" variable.

// All ShapeBase images are mounted into one of 8 slots on a shape.
// This weapon system assumes all primary weapons are mounted into
// specified slot 0:

//-----------------------------------------------------------------------------
// Image properties
//    emap
//    preload
//    shapeFile
//    mountPoint
//    offset
//    rotation
//    firstPerson
//    mass
//    usesEnergy
//    minEnergy
//    accuFire
//    lightType
//    lightTime
//    lightRadius
//    lightColor

// Image state variables
//stateName[] = TypeCaseString
//stateTransitionOnLoaded[] = TypeString
//stateTransitionOnNotLoaded[] = TypeString
//stateTransitionOnAmmo[] = TypeString
//stateTransitionOnNoAmmo[] = TypeString
//stateTransitionOnTarget[] = TypeString
//stateTransitionOnNoTarget[] = TypeString
//stateTransitionWet[] = TypeString
//stateTransitionNotWet[] = TypeString
//stateTransitionOnTriggerUp[] = TypeString
//stateTransitionOnTriggerDown[] = TypeString
//stateTransitionOnTimeout[] = TypeString
//stateTimeoutValue[] = TypeF32
//stateWaitForTimeout[] = TypeBool
//stateFire[] = TypeBool
//stateEjectShell[] = TypeBool
//stateEnergyDrain[] = TypeF32
//stateAllowImageChange[] = TypeBool
//stateDirection[] = TypeBool
//stateLoadedFlag[] = Ignore or Loaded or Empty
//stateSpinThread[] = FullSpeed or SpinDown or SpinUp or Stop or Ignore
//stateRecoil[] = NoRecoil or LightRecoil or MediumRecoil or HeavyRecoil
//stateSequence[] = TypeString
//stateSequenceRandomFlash[] = TypeBool
//stateSound[] = TypeAudioProfilePtr
//stateScript[] = TypeCaseString
//stateEmitter[] = TypeParticleEmitterDataPtr
//stateEmitterTime[] = TypeF32
//stateEmitterNode[] = TypeF32
//stateIgnoreLoadedForReady[] = TypeBool
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Weapon Class 
//-----------------------------------------------------------------------------

function Weapon::onUse(%data, %obj)
{
   // Default behavior for all weapons is to mount it into the object's weapon
   // slot, which is currently assumed to be slot 0
   if (%obj.getMountedImage($WeaponSlot) != %data.image.getId())
   {
      //serverPlay3D(WeaponUseSound, %obj.getTransform());

      %obj.mountImage(%data.image, $WeaponSlot);
/*
      if ( %obj.client )
      {
         if( %data.isField( "pickUpName" ) )
            messageClient(%obj.client, 'MsgWeaponUsed', '\c0%1 selected.', %data.pickUpName);
         else
            messageClient(%obj.client, 'MsgWeaponUsed', '\c0Weapon selected');
      }
 */
      // If this is a Player class object then allow the weapon to modify allowed poses
      if ( %obj.isInNamespaceHierarchy("Player") )
      {
         // Start by allowing everything
         %obj.allowAllPoses();
         
         // Now see what isn't allowed by the weapon
         %image = %data.image;
         
         if (%image.jumpingDisallowed)
            %obj.allowJumping(false);
         
         if (%image.jetJumpingDisallowed)
            %obj.allowJetJumping(false);
         
         if (%image.sprintDisallowed)
            %obj.allowSprinting(false);
         
         if (%image.crouchDisallowed)
            %obj.allowCrouching(false);
         
         if (%image.proneDisallowed)
            %obj.allowProne(false);
         
         if (%image.swimmingDisallowed)
            %obj.allowSwimming(false);
      }
   }
}

function Weapon::onPickup(%this, %obj, %shape, %amount)
{
   //echo("\c3Weapon::onPickup(" SPC %this.getName() @", "@ %obj.getDataBlock().getName() @", "@ %shape.client.nameBase @", "@ %amount SPC ")");
   // The parent Item method performs the actual pickup.
   // For player's we automatically use the weapon if the
   // player does not already have one in hand.
   if (Parent::onPickup(%this, %obj, %shape, %amount))
   {
      if ( %shape.getClassName() $= "Player" || %shape.getClassName() $= "AIPlayer" && %shape.getMountedImage($WeaponSlot) == 0)
         %shape.use(%this);
   }
}

function Weapon::incCatagory(%data, %obj)
{
   // Certain items are not added to the weapon count
   if ( !$SMS::ShowInInv[%data] )
      %obj.weaponCount++;   
}

function Weapon::decCatagory(%data, %obj)
{
   // Certain items are not removed from the weapon count
   if ( !$SMS::ShowInInv[%data] )
      %obj.weaponCount--;   
}

//-----------------------------------------------------------------------------
// Weapon Image Class
//-----------------------------------------------------------------------------

function WeaponImage::onMount(%this, %obj, %slot)
{
   //echo("\c3WeaponImage::onMount( " @ %this.getName() @ ", " @ %obj.client.nameBase @ ", " @ %slot @ " )");
   // Called from within the C++ source
   // Images assume a false ammo state on load.  We need to set the state according to the current inventory.

   // Basically setting this to false would change the state to NoAmmo, thus disabling any firing, setting 
   // it back to true would enable changing to the fire state again.
   // Images assume a false ammo state on load.  We need to
   // set the state according to the current inventory.
   
   //player just switched to a new weapon, so he is not reloading anymore
   %obj.isReloading = false;

   if( %this.isField("clip") )
   {
      // Use the clip system for this weapon.  Check if the player already has some ammo in a clip.
      if ( %obj.getInventory(%this.ammo) )
      {
         %obj.setImageAmmo( %slot, true );
         %currentAmmo = %obj.getInventory( %this.ammo );
      }
      /*else if( %obj.getInventory( %this.clip ) > 0 )
      {
         // Fill the weapon up from the first clip
         %obj.setInventory( %this.ammo, %this.ammo.maxInventory );
         %obj.setImageAmmo( %slot, true );
         
         // Add any spare ammo that may be "in the player's pocket"
         %currentAmmo = %this.ammo.maxInventory;
         %amountInClips += %obj.getFieldValue( "remaining" @ %this.ammo.getName());
      }*/ //above code is not working and we use a hard clip system
      else
      {
         %currentAmmo = 0 + %obj.getFieldValue( "remaining" @ %this.ammo.getName());
      }
      
      %amountInClips = %obj.getInventory(%this.clip);
      //%amountInClips *= %this.ammo.maxInventory;

      if ( isObject(%obj.client) && !%obj.client.isAiControlled() )
         messageClient( %obj.client, 'MsgAmmoCnt', "", addTaggedString($DataToName[%this.item]), %slot, addTaggedString(%currentAmmo), addTaggedString(%amountInClips) );
   }
   else if( %this.ammo !$= "" )
   {
      // Use the ammo pool system for this weapon
      if ( %obj.getInventory(%this.ammo) )
      {
         %obj.setImageAmmo(%slot, true);
         %currentAmmo = %obj.getInventory(%this.ammo);
      }
      else
         %currentAmmo = 0;

      if ( isObject(%obj.client) && !%obj.client.isAiControlled() )
         messageClient( %obj.client, 'MsgAmmoCnt', "", addTaggedString($DataToName[%this.item]), %slot, addTaggedString(%currentAmmo), '0' );
   }
}

function WeaponImage::onUnmount(%this, %obj, %slot)
{
   //echo("\c3WeaponImage::onUnmount( " @ %this.getName() @ ", " @ obj.client.nameBase @ ", " @ %slot @ " )");
   // Store the last weapon used so we can switch back to it if we wish
   %obj.setArmThread(looknw);
   %obj.lastWeapon = %this.item;

   // Assuming we have nothing mounted update the hud
   if ( isObject( %obj.client ) && !%obj.client.isAiControlled() )
   {
      //commandToClient(%obj.client, 'HideReticle');
      messageClient( %obj.client, 'MsgAmmoCnt', "", 'Empty', %slot, '0', '0' );
   }

   Parent::onUnmount(%this, %obj, %slot);
}

//-----------------------------------------------------------------------------
// Grenade Class 
//-----------------------------------------------------------------------------

function GrenadeImage::onMount(%data, %player, %slot)
{
   if ( %data.ammo !$= "" )
   {
      if( %player.getInventory(%data.ammo) )
         %player.setImageAmmo(%slot, true);

      // Send the ammo amount
      if ( isObject( %player.client ) )
         messageClient(%player.client, 'MsgGrenadeCnt', "", addTaggedString($DataToName[%data.item]), %player.getInventory(%data.ammo));
   }
   else // Energy based weapon
   {
      if ( isObject( %player.client ) )
         messageClient(%player.client, 'MsgGrenadeCnt', "", addTaggedString($DataToName[%data.item]), '*');
   }
}

function GrenadeImage::onUnmount(%data, %player, %slot)
{
   // Assuming we have nothing mounted update the hud
   if ( isObject( %player.client ) )
      messageClient(%player.client, 'MsgGrenadeCnt', "", 'Empty', '0');

   Parent::onUnmount(%this, %player, %slot);
}

//-----------------------------------------------------------------------------
// Clip Management
//-----------------------------------------------------------------------------
function WeaponImage::startReloadAmmoClip(%this, %obj, %slot)
{
    //Transition to the state that plays the animation
    %obj.setManualImageState(%slot, "ReloadClip");  
}

function WeaponImage::clearAmmoClip( %this, %obj, %slot )
{
   //echo("WeaponImage::clearAmmoClip: " SPC %this SPC %obj SPC %slot);
   
   // if we're not empty put the remaining bullets from the current clip
   // in to the player's "pocket".

   if ( %this.isField( "clip" ) )
   {
      // Commenting out this line will use a "hard clip" system, where
      // A player will lose any ammo currently in the gun when reloading.
      //%pocketAmount = %this.stashSpareAmmo( %obj );
      
      if ( %obj.getInventory( %this.clip ) > 0) // || %pocketAmount != 0 )
         %obj.setImageAmmo(%slot, false);
   }

}

function WeaponImage::reloadAmmoClip(%this, %obj, %slot)
{
   // Make sure we're indeed the currect image on the given slot
   if ( %this != %obj.getMountedImage( %slot ) )
      return;
   
   //Does this weapon use clips
   if ( %this.isField("clip") )
   {
      //Are there any clips left in inventory
      if ( %obj.getInventory(%this.clip) > 0 )
      {
         //remove a clip
         %obj.decInventory( %this.clip, 1 );
         //reset our current ammo back to a full clip size
         %obj.setInventory( %this.ammo, %obj.maxInventory(%this.ammo), 1 );
         //reset the state machine so firing can happen again.
         %obj.setImageAmmo(%slot, true);
      }
   }
}
 
function WeaponImage::onReloadFinish(%this, %obj, %slot)
{
    %obj.isReloading = false;
	%obj.allowSprinting(true);
    %this.schedule( 0, "reloadAmmoClip", %obj, %slot );
}

function WeaponImage::onWeaponActivate(%this, %obj, %slot)
{
    if ( %obj.getInventory( %this.ammo ) <= 0 )
        %obj.setManualImageState(%slot, "NoAmmo"); 
}

function WeaponImage::stashSpareAmmo( %this, %player )
{
   // If the amount in our pocket plus what we are about to add from the clip
   // Is over a clip, add a clip to inventory and keep the remainder
   // on the player
   if (%player.getInventory( %this.ammo ) < %this.ammo.maxInventory )
   {
      %nameOfAmmoField = "remaining" @ %this.ammo.getName();
      
      %amountInPocket = %player.getFieldValue( %nameOfAmmoField );
      
      %amountInGun = %player.getInventory( %this.ammo );
      
      %combinedAmmo = %amountInGun + %amountInPocket;
      
      // Give the player another clip if the amount in our pocket + the 
      // Amount in our gun is over the size of a clip.
      if ( %combinedAmmo >= %this.ammo.maxInventory )
      {
         %player.setFieldValue( %nameOfAmmoField, %combinedAmmo - %this.ammo.maxInventory );
         %player.incInventory( %this.clip, 1 );
      }
      else if ( %player.getInventory(%this.clip) > 0 )// Only put it back in our pocket if we have clips.
         %player.setFieldValue( %nameOfAmmoField, %combinedAmmo );
         
      return %player.getFieldValue( %nameOfAmmoField );
   }
   
   return 0;
}

//-----------------------------------------------------------------------------
// Clip Class
//-----------------------------------------------------------------------------

function AmmoClip::onPickup(%this, %obj, %shape, %amount)
{
   // The parent Item method performs the actual pickup.
   if (Parent::onPickup(%this, %obj, %shape, %amount))
      serverPlay3D(AmmoPickupSound, %shape.getTransform());

   // The clip inventory state has changed, we need to update the
   // current mounted image using this clip to reflect the new state.
   if ((%image = %shape.getMountedImage($WeaponSlot)) > 0)
   {
      // Check if this weapon uses the clip we just picked up and if
      // there is no ammo.
      if (%image.isField("clip") && %image.clip.getId() == %this.getId())
      {
         %outOfAmmo = !%shape.getImageAmmo($WeaponSlot);
         
         %currentAmmo = %shape.getInventory(%image.ammo);

         if ( isObject( %image.clip ) )
            %amountInClips = %shape.getInventory(%image.clip);
            
         %amountInClips *= %image.ammo.maxInventory;
         %amountInClips += %obj.getFieldValue( "remaining" @ %this.ammo.getName() );

         if ( isObject(%shape.client) && !%shape.client.isAiControlled() )
            messageClient(%shape.client, 'MsgAmmoCnt', "", addTaggedString($DataToName[%image.item]), %i, addTaggedString(%currentAmmo), addTaggedString(%amountInClips));

         if (%outOfAmmo)
         {
            %image.onClipEmpty(%shape, $WeaponSlot);
         }
      }
   }
}
