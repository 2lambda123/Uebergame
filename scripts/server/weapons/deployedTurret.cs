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
// Deployable AI Turret
//-----------------------------------------------------------------------------
datablock AITurretShapeData(DeployedTurret : SentryTurret)
{
   // Mission editor category
   category = "Deployable";
   renderWhenDestroyed = false;

   // Dynamic properties defined by the scripts
   item = TurretDeployable; // Need this for Ammo count message to client

   nameTag = 'Sentry Turret';
   deployedObject = true;
};

datablock ItemData(TurretDeployable : DefaultWeapon)
{
   shapeFile = "art/shapes/weapons/Turret/TP_Turret.dts";

   image = TurretDeployableImage;
   pickUpName = 'Sentry Turret';
};

datablock ShapeBaseImageData(TurretDeployableImage)
{
   class = "WeaponImage";
   className = "WeaponImage";

   // Basic Item properties
   shapeFile = "art/editor/invisible.dts";
   //shapeFile = "art/shapes/weapons/Turret/TP_Turret.dts";
   //shapeFileFP = "art/shapes/weapons/Turret/FP_Turret.dts";
   emap = true;
   computeCRC = false;
   cloakable = true;

   imageAnimPrefix = "Turret";
   imageAnimPrefixFP = "Turret";

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   firstPerson = true;
   useEyeNode = true;

   // Don't allow a player to sprint with a turret
   sprintDisallowed = true;

   // Projectiles and Ammo.
   item = TurretDeployable;
   deployed = DeployedTurret;

   usesEnergy = true;
   minEnergy = 20;
   fireEnergy = 20;

   lightType = "NoLight";

   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "1 1 1";
   camShakeAmp = "6 6 6";
   camShakeDuration = "1.5";
   camShakeRadius = "1.2";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
   stateName[0]                     = "Preactivate";
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNoAmmo[0]       = "Activate";

   // Activating the gun.  Called when the weapon is first
   // mounted and there is ammo.
   stateName[1]                     = "Activate";
   stateTransitionGeneric0In[1]     = "SprintEnter";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 0.66;
   stateSequence[1]                 = "switch_in";
   stateSound[1]                    = TurretSwitchinSound;

   // Ready to fire, just waiting for the trigger
   stateName[2]                     = "Ready";
   stateTransitionGeneric0In[2]     = "SprintEnter";
   stateTransitionOnMotion[2]       = "ReadyMotion";
   stateTransitionOnTriggerDown[2]  = "Fire";
   stateScaleAnimation[2]           = false;
   stateScaleAnimationFP[2]         = false;
   stateSequence[2]                 = "idle";

   // Ready to fire with player moving
   stateName[3]                     = "ReadyMotion";
   stateTransitionGeneric0In[3]     = "SprintEnter";
   stateTransitionOnNoMotion[3]     = "Ready";
   stateScaleAnimation[3]           = false;
   stateScaleAnimationFP[3]         = false;
   stateSequenceTransitionIn[3]     = true;
   stateSequenceTransitionOut[3]    = true;
   stateTransitionOnTriggerDown[3]  = "Fire";
   stateSequence[3]                 = "run";

   // Wind up to throw the Turret
   stateName[4]                     = "Fire";
   stateTransitionGeneric0In[4]     = "SprintEnter";
   stateTransitionOnTimeout[4]      = "Fire2";
   stateTimeoutValue[4]             = 0.66;
   stateFire[4]                     = true;
   stateRecoil[4]                   = "";
   stateAllowImageChange[4]         = false;
   stateSequence[4]                 = "Fire";
   stateSequenceNeverTransition[4]  = true;
   stateShapeSequence[4]            = "Recoil";

   // Throw the actual Turret
   stateName[5]                     = "Fire2";
   stateTransitionGeneric0In[5]     = "SprintEnter";
   stateTransitionOnTriggerUp[5]    = "Reload";
   stateTimeoutValue[5]             = 0.1;
   stateAllowImageChange[5]         = false;
   stateScript[5]                   = "onFire";
   stateShapeSequence[5]            = "Fire_Release";

   // Play the reload animation, and transition into
   stateName[6]                     = "Reload";
   stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnTimeout[6]      = "Ready";
   stateWaitForTimeout[6]           = true;
   stateTimeoutValue[6]             = 0.66;
   stateSequence[6]                 = "switch_in";

   // Start Sprinting
   stateName[7]                     = "SprintEnter";
   stateTransitionGeneric0Out[7]    = "SprintExit";
   stateTransitionOnTimeout[7]      = "Sprinting";
   stateWaitForTimeout[7]           = false;
   stateTimeoutValue[7]             = 0.25;
   stateWaitForTimeout[7]           = false;
   stateScaleAnimation[7]           = true;
   stateScaleAnimationFP[7]         = true;
   stateSequenceTransitionIn[7]     = true;
   stateSequenceTransitionOut[7]    = true;
   stateAllowImageChange[7]         = false;
   stateSequence[7]                 = "sprint";

   // Sprinting
   stateName[8]                     = "Sprinting";
   stateTransitionGeneric0Out[8]    = "SprintExit";
   stateWaitForTimeout[8]           = false;
   stateScaleAnimation[8]           = false;
   stateScaleAnimationFP[8]         = false;
   stateSequenceTransitionIn[8]     = true;
   stateSequenceTransitionOut[8]    = true;
   stateAllowImageChange[8]         = false;
   stateSequence[8]                 = "sprint";
   
   // Stop Sprinting
   stateName[9]                     = "SprintExit";
   stateTransitionGeneric0In[9]     = "SprintEnter";
   stateTransitionOnTimeout[9]      = "Ready";
   stateWaitForTimeout[9]           = false;
   stateTimeoutValue[9]             = 0.5;
   stateSequenceTransitionIn[9]     = true;
   stateSequenceTransitionOut[9]    = true;
   stateAllowImageChange[9]         = false;
   stateSequence[9]                 = "sprint";
};

// ----------------------------------------------------------------------------
// Player deployable turret
// ----------------------------------------------------------------------------

// Cannot use the Weapon class for deployable turrets as it is already tied
// to ItemData.

function TurretDeployableImage::onMount(%this, %obj, %slot)
{
   %max = $TeamDeployableMax[DeployedTurret];
   %used = $TeamDeployedCount[%obj.team, DeployedTurret];

   if ( isObject( %obj.client ) )
      messageClient(%obj.client, 'MsgAmmoCnt', "", addTaggedString($DataToName[%this.item]), %slot, addTaggedString(%max - %used), '0');
}

function TurretDeployableImage::onUnmount(%this, %obj, %slot)
{
   %obj.setImageTrigger(%slot, false);
}

//-----------------------------------------------------------------------------

function TurretDeployableImage::onFire(%this, %obj, %slot)
{
   echo("TurretDeployableImage::onFire( "@%this.getName()@", "@%obj.client.nameBase@", "@%slot@" )");

   if( $TeamDeployedCount[%user.team, %this.deployed.getName()] >= $TeamDeployableMax[%this.deployed.getName()] )
   {
      messageClient(%user.client, 'MsgDeployFailed', '\c2Your team has reached it\'s capacity for this item. %1', %this.deployed.nameTag);
      return;
   }

   if ( %obj.getEnergyLevel() < %this.minEnergy )
      return;

   %obj.setEnergyLevel( %obj.getEnergyLevel() - %this.fireEnergy );

   // To fire a deployable turret is to throw it.  Schedule the throw
   // so that it doesn't happen during this ShapeBaseImageData's state machine.
   // If we throw the last one then we end up unmounting while the state machine
   // is still being processed.
   %obj.schedule( 50, "throwTurret", %this.deployed );
}

function ShapeBase::throwTurret( %this, %data )
{
   //echo("ShapeBase::throwTurret(" SPC %this.getClassName() @", "@ %data.getName() SPC ")");

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
   echo( "DeployedTurret::onThrow(" SPC %user.client.nameBase @", "@ %data.getName() SPC ")" );

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
      %user.decInventory( %data.item, 1 );

      %max = $TeamDeployableMax[%data.getName()];
      messageClient( %client, 'MsgAmmoCnt', "", addTaggedString($DataToName[%data.item.getName()]), $WeaponSlot, addTaggedString(%max - $TeamDeployedCount[%user.team, %data.getName()]), '0' );
   }

   if ( %user.inv[%obj.lastWeapon] )
      %user.use( %obj.lastWeapon );
   else
      %user.use( %use.weaponSlot[0] );

   return %turret;
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowWeapon("Soldier");
SmsInv.AddWeapon(TurretDeployable, "Sentry Turret", 1);
