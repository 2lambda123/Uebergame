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

$TeamDeployableMax[DeployedTurret] = mFloor($pref::Server::MaxPlayers * 0.25);

//-----------------------------------------------------------------------------

datablock AITurretShapeData(DeployedTurret : SentryTurret)
{
   category = "Deployable";
   nameTag = 'Sentry Turret';
   renderWhenDestroyed = false;

   item = TurretDeployable; // UnDeploy flag
   deployedObject = true; // UnDeploy and power flag
};

datablock ItemData(TurretDeployable)
{
   category = "Specials"; // Not only used in mission editor but also for cleaning up deploy counts
   className = "Special";
   shapeFile = "art/editor/invisible.dts";
   //shapeFile = "art/shapes/weapons/Turret/TP_Turret.dts";
   computeCRC = false;
   mass = 2;
   drag = 0.5;
   density = 2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   maxVelocity = 20;
   dynamicType = $TypeMasks::ItemObjectType;

   image = "TurretDeployableImage";
   pickUpName = 'Sentry Turret';
   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;

   lightType = "NoLight";
};

datablock ShapeBaseImageData(TurretDeployableImage)
{
   // Basic Item properties
   shapeFile = "art/shapes/weapons/Turret/TP_Turret.dts";
   shapeFileFP = "art/editor/invisible.dts";
   emap = true;
   computeCRC = false;
   cloakable  = true;

   imageAnimPrefix = "Turret";
   imageAnimPrefixFP = "Turret";

   item = TurretDeployable;
   deployed = DeployedTurret;
   mountPoint = 1;
   mass       = 2;
   firstPerson = true;
   useEyeNode = true;
   offset     = "0.15 -0.4 0"; // L/R - F/B - T/B
   rotation   = "1 0 0 0";

   lightType = "NoLight";

   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "1 1 1";
   camShakeAmp = "6 6 6";
   camShakeDuration = "1.5";
   camShakeRadius = "1.2";

   usesEnergy = true;
   minEnergy = 20;
   fireEnergy = 20;

   stateName[0]                    = "Preactivate";
   stateSequence[0]                = "activation";
   stateTransitionOnTriggerDown[0] = "Activate";

   stateName[1]                    = "Activate";
   stateTransitionOnTimeout[1]      = "Activate2";
   stateTimeoutValue[1]             = 0.66;
   stateFire[1]                     = true;
   stateAllowImageChange[1]         = false;
   stateSequence[1]                = "fire";
   stateSequenceNeverTransition[1]  = true;
   stateShapeSequence[1]            = "Recoil";
   stateScript[1]                  = "onActivate";

   stateName[2]                    = "Activate2";
   //stateTransitionOnTriggerUp[2]   = "Deactivate";
   stateTimeoutValue[2]             = 0.1;
   stateTransitionOnTimeout[2]     = "Deactivate";
   stateAllowImageChange[2]         = false;
   stateScript[2]                  = "onActivateDone";
   stateShapeSequence[2]            = "Fire_Release";

   stateName[3]                    = "Deactivate";
   stateScript[3]                  = "onDeactivate";
   stateTimeoutValue[3]            = 0.2;
   stateTransitionOnTimeout[3]     = "Preactivate";
};

//-----------------------------------------------------------------------------

function TurretDeployableImage::onDeactivate(%data, %obj, %slot)
{
   %obj.setImageTrigger(%slot, false);

   %obj.client.deploySpecial = true;

   if ( %obj.inv[%obj.lastWeapon] )
      %obj.use( %obj.lastWeapon );
   else
      %obj.use( %obj.weaponSlot[0] );
}

function TurretDeployableImage::onUnmount(%data, %obj, %slot)
{
   %obj.setImageTrigger(%slot, false);
}

function TurretDeployableImage::onActivate(%data, %obj, %slot)
{
   LogEcho("TurretDeployableImage::onActivate(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");
   %obj.client.deploySpecial = false;

   // Release the main weapon trigger and unmount the weapon
   if ( %obj.getMountedImage($WeaponSlot) != 0 )
   {
      %obj.setImageTrigger($WeaponSlot, false);
      %obj.unmountImage($WeaponSlot);
   }
}

function TurretDeployableImage::onActivateDone(%data, %obj, %slot)
{
   LogEcho("TurretDeployableImage::onActivateDone(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");
   if( $TeamDeployedCount[%obj.team, %data.deployed.getName()] >= $TeamDeployableMax[%data.deployed.getName()] )
   {
      messageClient(%obj.client, 'MsgDeployFailed', '\c2Your team has reached it\'s capacity for this item. %1', %data.deployed.nameTag);
      return %obj.setImageTrigger(%slot, false);
   }

   if ( %obj.getEnergyLevel() < %data.minEnergy )
      return %obj.setImageTrigger(%slot, false);

   %obj.setEnergyLevel( %obj.getEnergyLevel() - %data.fireEnergy );

   // To fire a deployable turret is to throw it.  Schedule the throw
   // so that it doesn't happen during this ShapeBaseImageData's state machine.
   // If we throw the last one then we end up unmounting while the state machine
   // is still being processed.
   %obj.schedule( 50, "throwTurret", %data.deployed );
}

function ShapeBase::throwTurret( %this, %data )
{
   LogEcho("ShapeBase::throwTurret(" SPC %this.getClassName() @", "@ %data.getName() SPC ")");

   if ( !isObject( %data ) )
      return false;

   // Create the item to throw first...
   %item = %data.onThrow(%this);
   if(%item)
   {
      %this.throwObject(%item);
      return true;
   }
   return false;
}

function DeployedTurret::onThrow(%data, %user)
{
   LogEcho( "DeployedTurret::onThrow(" SPC %user.client.nameBase @", "@ %data.getName() SPC ")" );

   %client = %user.client;

   // Construct the actual object in the world, and add it to
   // the mission group so it's cleaned up when the mission is
   // done.  The turret's rotation matches the player's.
   %rot = %user.getEulerRotation();
   %turret = new AITurretShape()
   {
      datablock = %data;
      rotation = "0 0 1 " @ getWord(%rot, 2);
      count = 1;
      sourceObject = %user;
      origin = %user;
      client = isObject(%client) ? %client : %user;
      owner = isObject(%client) ? %client : %user;
      isAiControlled = true;
   };
   addToDeploySet( %turret );

   LogEcho( "Turret Typemask:" SPC getObjectTypeMask(%turret) SPC "GetType:" SPC %turret.getType() );

   %turret.team = %user.team;
   %turret.setTeamId( %user.team );
   %turret.addToIgnoreList(%user);

   // increment the team count for this deployed object
   $TeamDeployedCount[%user.team, %data.getName()]++;

   if ( isObject( %client ) )
   {
      if (!%client.ownedTurrets)
      {
         %client.ownedTurrets = new SimSet();
      }
      // Add ourselves to the client's owned list.
      %client.ownedTurrets.add(%turret);

      // Go through the client's owned turret list.  Make sure we're
      // a friend of every turret and every turret is a friend of ours.
      // Commence hugging!
      for ( %i=0; %i<%client.ownedTurrets.getCount(); %i++ )
      {
         %obj = %client.ownedTurrets.getObject(%i);
         %obj.addToIgnoreList(%turret);
         %turret.addToIgnoreList(%obj);
      }

      // take the deployable out of inventory, this will unmount the image as well
      %user.setInventory( %data.item, 0 );

      %max = $TeamDeployableMax[%data.getName()];
      messageClient( %client, 'MsgAmmoCnt', "", addTaggedString($DataToName[%data.item.getName()]), $SpecialSlot, addTaggedString(%max - $TeamDeployedCount[%user.team, %data.getName()]), '0' );
   }

   return %turret;
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowItem("armor\tSoldier\t1");
SmsInv.AddItem(TurretDeployable, "Sentry Turret", 1);
