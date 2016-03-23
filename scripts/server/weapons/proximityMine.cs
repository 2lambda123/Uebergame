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

// CoreGame deals with zeroing out the counts
$TeamDeployableMax[ProxMine] = 6; // Maximum mines allowed deployed per team

datablock SFXProfile( MineArmedSound )
{
   filename = "art/sound/weapons/mine_armed";
   description = AudioClose3d;
   preload = true;
};

datablock SFXProfile( MineSwitchinSound )
{
   filename = "art/sound/weapons/wpn_proximitymine_switchin";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile( MineTriggeredSound )
{
   filename = "art/sound/weapons/mine_trigger";
   description = AudioClose3d;
   preload = true;
};

datablock ItemData(ProxMineAmmo : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/ProxMine/TP_ProxMine.dts";
   pickUpName = 'Proximity Mine';
};

datablock ProximityMineData( ProxMine )
{
   // ShapeBaseData fields
   category = "Weapon";
   shapeFile = "art/shapes/weapons/ProxMine/TP_ProxMine.dts";
   explosion = MineExplosion;
   underwaterExplosion = UnderwaterMineExplosion;

   // ItemData fields
   sticky = true;
   mass = 2;
   elasticity = 0.2;
   friction = 0.6;

   simpleServerCollision = false;

   // ProximityMineData fields
   armingDelay = 3.5;
   armingSound = MineArmedSound;

   autoTriggerDelay = 0;
   triggerOnOwner = false;
   triggerRadius = 2.0;
   triggerDelay = 0.45;
   triggerSound = MineTriggeredSound;

   explosionOffset = 0.1;
   
   // dynamic fields
   pickUpName = 'Proximity Mine';
   description = 'Proximity Mine';
   image = ProxMineImage;

   damageType = $DamageType::Mine; // type of damage applied to objects in radius
   radiusDamage = 100;           // amount of damage to apply to objects in radius
   damageRadius = 6;            // search radius to damage objects when exploding
   areaImpulse = 2000;          // magnitude of impulse to apply to objects in radius
};

datablock ShapeBaseImageData( ProxMineImage )
{
   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";

   // Basic Item properties
   shapeFile = "art/shapes/weapons/ProxMine/TP_ProxMine.dts";
   shapeFileFP = "art/shapes/weapons/ProxMine/FP_ProxMine.dts";
   emap = true;
   computeCRC = false;

   imageAnimPrefix = "ProxMine";
   imageAnimPrefixFP = "ProxMine";

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   firstPerson = true;
   useEyeNode = true;
   animateOnServer = true;
   cloakable = true;

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.
   correctMuzzleVector = false;
   correctMuzzleVectorTP = false;

   // Projectiles and Ammo.
   item = ProxMine;
   ammo = ProxMineAmmo;

   usesEnergy = true;
   minEnergy = 45;
   fireEnergy = 40;

   lightType = "NoLight";

   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "1 1 1";
   camShakeAmp = "1 1 1";

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
   stateTimeoutValue[1]             = 3.0;
   stateSequence[1]                 = "switch_in";
   stateShapeSequence[1]            = "Reload";
   stateSound[1]                    = MineSwitchinSound;

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
   stateWaitForTimeout[3]           = false;
   stateScaleAnimation[3]           = false;
   stateScaleAnimationFP[3]         = false;
   stateSequenceTransitionIn[3]     = true;
   stateSequenceTransitionOut[3]    = true;
   stateTransitionOnTriggerDown[3]  = "Fire";
   stateSequence[3]                 = "run";

   // Wind up to throw the ProxMine
   stateName[4]                     = "Fire";
   stateTransitionGeneric0In[4]     = "SprintEnter";
   stateTransitionOnTimeout[4]      = "Fire2";
   stateTimeoutValue[4]             = 0.8;
   stateFire[4]                     = true;
   stateRecoil[4]                   = "";
   stateAllowImageChange[4]         = false;
   stateSequence[4]                 = "Fire";
   stateSequenceNeverTransition[4]  = true;
   stateShapeSequence[4]            = "Fire";

   // Throw the actual mine
   stateName[5]                     = "Fire2";
   stateTransitionGeneric0In[5]     = "SprintEnter";
   stateTransitionOnTriggerUp[5]    = "Reload";
   stateTimeoutValue[5]             = 0.7;
   stateAllowImageChange[5]         = false;
   stateSequenceNeverTransition[5]  = true;
   stateSequence[5]                 = "fire_release";
   stateShapeSequence[5]            = "Fire_Release";
   stateScript[5]                   = "onFire";

   // Play the reload animation, and transition into
   stateName[6]                     = "Reload";
   stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnTimeout[6]      = "Ready";
   stateWaitForTimeout[6]           = true;
   stateTimeoutValue[6]             = 3.0;
   stateSequence[6]                 = "switch_in";
   stateShapeSequence[6]            = "Reload";
   stateSound[6]                    = MineSwitchinSound;

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
   stateSequence[7]                 = "run2sprint";

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
   stateSequence[9]                 = "sprint2run";
};

//-----------------------------------------------------------------------------
// Functions

function ProximityMineData::create( %data )
{
   // Construct the actual object in the world, and add it to
   // the mission group so its cleaned up when the mission is
   // done.  The object is given a random z rotation.
   %mine = new ProximityMine()
   {
      datablock = %data;
      rotation = "0 0 1 "@ (getRandom() * 360);
      static = false;
      rotate = false;
      scale = %data.scale !$= "" ? %data.scale : "1 1 1";
   };
   return %mine;
}

function ProximityMineData::onTriggered( %this, %obj, %target )
{
   echo(%this.name SPC "triggered by " @ %target.getClassName());
}

function ProximityMineData::onExplode( %data, %mine, %position )
{
   // Damage objects within the mine's damage radius
   if ( %data.damageRadius > 0 )
      radiusDamage(%mine, %mine.sourceObject, %position, %data.damageRadius, %data.radiusDamage, %data.damageType, %data.areaImpulse);

   // Update the hud
   $TeamDeployedCount[%mine.sourceObject.team, ProxMine]--;
   %max = $TeamDeployableMax[ProxMine];
   %used = $TeamDeployedCount[%mine.sourceObject.team, ProxMine];
   messageClient(%mine.sourceObject.client, 'MsgAmmoCnt', "", addTaggedString($DataToName[%data.getName()]), $WeaponSlot, addTaggedString(%max - %used), '0');
}

function ProximityMineData::damage( %this, %obj, %position, %source, %amount, %damageType )
{
   // Explode if any damage is applied to the mine
   %obj.schedule(50 + getRandom(50), explode);
}

// ----------------------------------------------------------------------------
// Player deployable proximity mine
// ----------------------------------------------------------------------------

function ProxMine::onUse(%data, %obj)
{
   Weapon::onUse(%data, %obj);
}

function ProxMine::onPickup(%this, %obj, %shape, %amount)
{
   Weapon::onPickup(%this, %obj, %shape, %amount);
}

function ProxMine::onInventory( %data, %obj, %amount )
{
   if (!%amount && (%slot = %obj.getMountSlot(%data.image)) != -1)
      %obj.unmountImage(%slot);

   %max = $TeamDeployableMax[ProxMine];
   %used = $TeamDeployedCount[%obj.team, ProxMine];
   messageClient(%obj.client, 'MsgAmmoCnt', "", addTaggedString($DataToName[%data.getName()]), $WeaponSlot, addTaggedString(%max - %used), '0');
}

function ProxMine::incCatagory(%data, %obj)
{
   // Certain items are not added to the weapon count
   if ( !$SMS::ShowInInv[%data] )
      %obj.weaponCount++;   
}

function ProxMine::decCatagory(%data, %obj)
{
   // Certain items are not removed from the weapon count
   if ( !$SMS::ShowInInv[%data] )
      %obj.weaponCount--;   
}

function ProxMineImage::onFire( %data, %obj, %slot )
{
   //error("ProxMineImage::onFire(" SPC %data.getName() @", "@ %obj.client.nameBase SPC ")");

   if(%obj.getEnergyLevel() < %data.minEnergy)
      return %obj.setImageTrigger(%slot, false);

   %obj.setEnergyLevel(%obj.getEnergyLevel() - %data.fireEnergy);

   if ( %obj.isInWater )
   {
      messageClient(%obj.client, 'MsgError', '\c2Cannot deploy mines underwater.');
      return;
   }

   if ( %obj.outOfBounds )
   {
      messageClient(%obj.client, 'MsgError', '\c2Cannot deploy mines outside of the mission area.');
      return;
   }

   %max = $TeamDeployableMax[ProxMine];
   %used = $TeamDeployedCount[%obj.team, ProxMine]; // How many will we have left
   %name = $DataToName[%data.item.getName()];

   if ( %used >= %max )
   {
      messageClient(%obj.client, 'MsgTeamDeployCount', '\c2Your team has the max mines\s (%1) allowed deployed', %maxDep);
      return;
   }

   %data.lightStart = $Sim::Time;
   %obj.setInvincible( false ); // Throw a grenade and your invincibility goes away.

   if ( %obj.inStation $= "" && %obj.isCloaked() )
   {
      if ( %obj.respawnCloakThread !$= "" )
      {
         cancel( %obj.respawnCloakThread );
         %obj.setCloaked( false );
         %obj.respawnCloakThread = "";
      }
      else
      {
         if( %obj.getEnergyLevel() > 20 )
         {   
            %obj.setCloaked( false );
            %obj.reCloak = %obj.schedule( 1000, "setCloaked", true ); 
         }
      }
   }

   // Create the mine
   %mine = %data.item.create();
   %mine.sourceObject = %obj;
   %mine.client = %obj.client;
   %mine.team = %obj.team;
/*
   %mine.armed = false;
   %mine.damaged = 0;
   %mine.detonated = false;
   %mine.checkCount = 0;
   %mine.noDamage = false;
   %mine.boom = false;
*/
   $TeamDeployedCount[%obj.team, ProxMine]++;
   %obj.decInventory( %data, 1 );

   // Throw the item
   %obj.throwObject(%mine);

   // Update the hud
   if ( Game.numTeams > 1 )
      %txt = "Your team has deployed" SPC $TeamDeployedCount[%obj.team, ProxMine] SPC "of" SPC %max SPC %name @"s.";
   else
      %txt = "You have deployed" SPC $TeamDeployedCount[%obj.team, ProxMine] SPC "of" SPC %max SPC %name @"s.";

   messageClient(%obj.client, 'MsgTeamDeployCount', addTaggedString(%txt));
   messageClient(%obj.client, 'MsgAmmoCnt', "", addTaggedString(%name), $WEaponSlot, addTaggedString(%max - %used), '0');
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowWeapon("Soldier");
SmsInv.AddWeapon(ProxMine, "Proximity Mine", 1);

SmsInv.AllowAmmo("armor\tSoldier\t2");
SmsInv.AddAmmo(ProxMineAmmo, 2);
