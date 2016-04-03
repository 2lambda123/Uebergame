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

$TeamDeployableMax[TripMineDeployed] = 12;

datablock SFXProfile(MineDeploySound)
{
   filename = "art/sound/weapons/mine_armed.ogg";
   description = AudioClose3D;
   preload = true;
};

datablock SFXProfile( MineArmedSound )
{
   filename = "art/sound/weapons/mine_armed";
   description = AudioClose3d;
   preload = true;
};

datablock SFXProfile( MineSwitchinSound )
{
   filename = "art/sound/weapons/wpn_proximitymine_switchin";
   description = AudioClosest3D;
   preload = true;
};

datablock SFXProfile( MineTriggeredSound )
{
   filename = "art/sound/weapons/mine_trigger";
   description = AudioClose3d;
   preload = true;
};

//-----------------------------------------------------------------------------
// Item and Image

datablock ItemData(tripMineDeployed : DefaultAmmo)
{
   category = "Deployable";
   shapeFile = "art/shapes/weapons/grenade/grenade.dts";
   mass = 2;
   density = 20;
   elasticity = 0.2;
   friction = 1;
   pickupRadius = 2;
   dynamicType = $TypeMasks::DamagableItemObjectType;

   maxDamage = 0.21;
   destroyedLevel = 0.2;

   radiusDamage = 100;
   damageRadius = 3.0;
   damageType = $DamageType::Mine;
   areaImpulse = 1500;

   explosion = MineExplosion;
   underwaterExplosion = UnderwaterMineExplosion;

   simpleServerCollision = false;

   spacing = 0;        // How close together (radius) fields can be placed
   maxCheckCount = 0;  // How many times the fields attempts to deploy
   armTime = 1000;      // How long before mine is ready to detonate
   proximity = 0.5;       // Distance from mine that causes it to explode
   sourceItem = TripMine;
};

datablock ItemData(TripMine : DefaultWeapon)
{
   category = "Handheld";
   className = "HandInventory";
   shapeFile = "art/shapes/weapons/grenade/grenade.dts";
   computeCRC = false;

   image = TripMineImage;
   thrownItem = TripMineDeployed;
   pickUpName = 'Trip Mine';
   throwTimeout = 800;
   isGrenade = true;
};

datablock ItemData(TripMineAmmo : DefaultAmmo)
{
   shapeFile = "art/shapes/weapons/ProxMine/TP_ProxMine.dts";
   pickUpName = 'TripMine';
};

datablock ShapeBaseImageData(TripMineImage)
{
   class = "GrenadeImage";
   className = GrenadeImage;

   shapeFile = "art/editor/invisible.dts";
   //shapeFile = "art/shapes/weapons/ProxMine/TP_ProxMine.dts";
   //shapeFileFP = "art/shapes/weapons/ProxMine/FP_ProxMine.dts";
   emap = true;
   computeCRC = false;

   imageAnimPrefix = "ProxMine";
   imageAnimPrefixFP = "ProxMine";

   //mountPoint = 3;
   firstPerson = false;
   useEyeNode = true;
   animateOnServer = true;
   cloakable = true;
   mass = 2;
   //rotation = "1 0 0 180";
   //offset = "0 -0.5 1"; // L/R - F/B - T/B

   throwTimeout = 800;
   item = TripMine;
   ammo = TripMineAmmo;
   thrownItem = TripMineDeployed;

   //usesEnergy = false;
   //minEnergy = 50;
   //fireEnergy = 40;
   
   // Shake camera while firing.
   shakeCamera = true;
   camShakeFreq = "1 1 1";
   camShakeAmp = "6 6 6";
   camShakeDuration = "1.5";
   camShakeRadius = "1.2";

   stateName[0]                    = "Preactivate";
   stateTransitionOnLoaded[0]      = "Activate";
   stateTransitionOnNoAmmo[0]      = "NoAmmo";
   stateSound[0]                   = WeaponSwitchSound;

   stateName[1]                    = "Activate";
   stateTransitionOnTimeout[1]     = "Ready";
   stateTimeoutValue[1]            = 0.2;
   stateSequence[1]                = "switch_in";
   stateShapeSequence[1]           = "Reload";

   stateName[2]                    = "Ready";
   stateTransitionOnNoAmmo[2]      = "NoAmmo";
   stateTransitionOnTriggerDown[2] = "Fire";
   stateSequence[2]                = "idle";

   stateName[3]                    = "Fire";
   stateTransitionOnTimeout[3]     = "Reload";
   stateTimeoutValue[3]            = 0.2;
   stateFire[3]                    = true;
   stateAllowImageChange[3]        = false;
   stateScript[3]                  = "onThrowGrenade";
   stateSequence[3]                = "Fire";

   stateName[4]                    = "Reload";
   stateTransitionOnNoAmmo[4]      = "NoAmmo";
   stateTransitionOnTimeout[4]     = "Activate";
   stateTimeoutValue[4]            = 0.2;
   stateAllowImageChange[4]        = false;
   stateSequence[4]                = "Reload";

   stateName[5]                    = "NoAmmo";
   stateTransitionOnAmmo[5]        = "Reload";
   stateSequence[5]                = "NoAmmo";
   stateTransitionOnTriggerDown[5] = "DryFire";

   stateName[6]                    = "DryFire";
   stateSound[6]                   = WeaponEmptySound;
   stateTimeoutValue[6]            = 0.2;
   stateTransitionOnTimeout[6]     = "NoAmmo";
   stateScript[6]                  = "onDryFire";
};

//-----------------------------------------------------------------------------

function TripMineDeployed::detonate(%data, %gren)
{

}

function TripMine::onInventory(%data, %player, %amount)
{
   Parent::onInventory(%data, %player, %amount);

   %max = $TeamDeployableMax[TripMineDeployed];
   %used = $TeamDeployedCount[%player.team, TripMineDeployed];


   // Now send the client a silent message containing the ammo amount and the short name of the weapon its for.
   // The client can stuff this info into an array so we can order it properly.
   if ( isObject( %player.client ) )
      messageClient( %player.client, 'MsgGrenadeCnt', "", addTaggedString($DataToName[%data.getName()]), $GrenadeSlot, addTaggedString(%max - %used) );
}

function TripMineImage::onThrowGrenade(%data, %obj, %slot)
{
   LogEcho("TripMineImage::onThrowGrenade(" SPC %data.getName() @", "@ %obj.client.nameBase SPC ")");

   if(%obj.getEnergyLevel() < %data.minEnergy)
      return %obj.setImageTrigger(%slot, false);

   %obj.setEnergyLevel(%obj.getEnergyLevel() - %data.fireEnergy);

   if ( %obj.isInWater )
   {
      messageClient(%obj.client, 'MsgError', '\c2Cannot deploy a mine underwater.');
      return;
   }

   if ( %obj.outOfBounds )
   {
      messageClient(%obj.client, 'MsgError', '\c2Cannot deploy a mine outside of the mission area.');
      return;
   }

   // Release the main weapon trigger and unmount the weapon
   if ( %obj.getMountedImage($WeaponSlot) != 0 )
   {
      %obj.setImageTrigger($WeaponSlot, false);
      %obj.unmountImage($WeaponSlot);
   }

   %max = $TeamDeployableMax[TripMineDeployed];
   %used = $TeamDeployedCount[%obj.team, TripMineDeployed]; // How many will we have left
   %name = $DataToName[%data];
   if ( Game.numTeams > 1 )
      %txt = "Your team has deployed" SPC %used SPC "of" SPC %max SPC "mines"@"s.";
   else
      %txt = "You have deployed" SPC %used SPC "of" SPC %max SPC "mines"@"s.";

   if ( %used >= %max )
   {
      messageClient(%obj.client, 'MsgTeamDeployCount', '\c2Your team has the max mines\s (%1) allowed deployed', %maxDep);
      return;
   }
   else
      messageClient(%obj.client, 'MsgGrenadeCnt', addTaggedString(%txt), addTaggedString(%name), $GrenadeSlot, addTaggedString(%max - %used));


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

   // Create the item
   %item = %data.thrownItem.create();
   %item.static = false;
   %item.rotate = false;
   %item.armed = false;
   %item.sourceObject = %obj;
   %item.team = %obj.team;
   %item.damaged = 0;
   %item.detonated = false;
   %item.checkCount = 0;
   %item.noDamage = false;
   %item.boom = false;
   %obj.decInventory( %data, 1 );

   // Throw the item
   %obj.throwObject(%item);

   // Schedule a deploy of the mine field
   %item.getDataBlock().schedule(1750, "onDeploy", %item, %obj);

   if ( %obj.inv[%obj.lastWeapon] )
      %obj.use( %obj.lastWeapon );
   else
      %obj.use( %obj.weaponSlot[0] );

   return %item;
}

function TripMineDeployed::onDeploy(%data, %mine, %player)
{
   LogEcho("TripMineDeployed::onDeploy(" SPC %data.getName() SPC %mine.checkCount SPC %player.client.nameBase SPC ")");
   if(%mine.checkCount > %data.maxCheckCount)
      explodeMine(%mine, true);

   // wait until the mine comes to rest
   if ( %mine.isAtRest() )
   {
      // check for other deployed mine fields in the vicinity
      InitContainerRadiusSearch(%mine.getWorldBoxCenter(), %data.spacing, $TypeMasks::DamagableItemObjectType);
      while( ( %item = containerSearchNext() ) != 0 )
      {
         if(%item == %mine)
            continue;

         %ioType = %item.getDatablock().getName();
         if ( %ioType $= "TripMineDeployed" )
         {
            schedule(100, %mine, "explodeMine", %mine, true);
            return;
         }
      }
      %mine.playThread(0, "Activate");
      %data.schedule(2000, "initMine", %mine, %player);
   }
   else
   {
      //schedule this deploy check again a little later
      %mine.checkCount++;
      %data.schedule(500, "onDeploy", %mine, %player);
   }
}

function TripMineDeployed::initMine(%data, %mine, %player)
{
   LogEcho("TripMineDeployed::initMine(" SPC %data @", "@ %mine @", "@ %player.client.nameBase SPC ")");

   serverPlay3D( MineDeploySound, %mine.getTransform() );
   %mine.playThread(0, "deploy");

   MissionCleanup.add( %item[%i] );
   %mine.getDataBlock().schedule( 1500, "CheckVicinity", %mine );
   %mine.getDataBlock().schedule( %data.armTime, "ArmMine", %mine, %player );
   $TeamDeployedCount[%player.team, TripMineDeployed]++;

   // Update the hud
   %max = $TeamDeployableMax[TripMineDeployed];
   %used = $TeamDeployedCount[%player.team, TripMineDeployed];
   messageClient(%player.client, 'MsgGrenadeCnt', "", addTaggedString($DataToName[%data.sourceItem]), $GrenadeSlot, addTaggedString(%max - %used));
}

function TripMineDeployed::ArmMine(%data, %mine, %player)
{
   %mine.armed = true;
}

function TripMineDeployed::CheckVicinity(%data, %mine)
{
   //LogEcho("TripMineDeployed::CheckVicinity(" SPC %data.getName() SPC %mine SPC ")");
   if ( %mine.armed )
   {
      if ( !%mine.boom )
      {
         %masks = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;
         InitContainerRadiusSearch(%mine.getWorldBoxCenter(), %data.proximity, %masks);
         while( ( %tgt = containerSearchNext()) != 0 ) 
         {
            if ( !$FriendlyFire && %tgt.team == %mine.team )
               continue;

            %mine.detonated = true;
            schedule(50, %mine, "explodeMine", %mine, false);
            break;
         }
      }
   }

   if ( !%mine.detonated )
      %data.schedule(300, "CheckVicinity", %mine);
}

function TripMineDeployed::onCollision(%data, %mine, %col)
{
   LogEcho("TripMineDeployed::onCollision(" SPC %data.getName() SPC %mine SPC %col.client.nameBase SPC ")");
   // don't detonate if mine isn't armed yet
   if ( !%mine.armed )
      return;

   // don't detonate if mine is already detonating
   if ( %mine.boom )
      return;

   //check to see what it is that collided with the mine
   if ( %col.getType() & ( $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType ) )
   {
      //error("Mine detonated due to collision with #"@%col@" ("@%struck@"); armed = "@%obj.armed);
      if ( !$FriendlyFire && %mine.team == %col.team )
         return;
      else
         explodeMine(%mine, false);
   }
}

function explodeMine(%mine, %noDamage)
{
   LogEcho("explodeMine(" SPC %mine.getDataBlock().getName() SPC %noDamage SPC ")");
   %mine.noDamage = %noDamage;
   %mine.setDamageState(Destroyed);
}

function TripMineDeployed::damage(%data, %mine, %sourceObject, %position, %amount, %damageType)
{
   LogEcho("TripMineDeployed::damage(" SPC %data.getName() SPC %mine SPC %sourceObject SPC %position SPC %amount SPC %damageType SPC ")");

   // This function is called AFTER ::onDestroyed if it is struck
   if ( !%mine.armed )
      return;

   if ( %mine.boom )
      return;

   %mine.damaged += %amount;

   if ( %mine.damaged >= %data.maxDamage )
   {
      %mine.setDamageState(Destroyed);
   }
}

function TripMineDeployed::onDestroyed(%data, %mine, %prevState)
{
   LogEcho("TripMineDeployed::onDestroyed(" SPC %data.getName() SPC %mine SPC %prevState SPC ")");
   %mine.boom = true;

   // %noDamage is a boolean flag -- don't want to set off all other mines in
   // vicinity if there's a "mine overload", so apply no damage/impulse if true
   if(!%mine.noDamage)
      radiusDamage(%mine, %mine.sourceObject, %mine.getPosition(), %data.damageRadius, %data.radiusDamage, %data.damageType, %data.areaImpulse);

   %mine.schedule(600, "delete");
   $TeamDeployedCount[%mine.sourceObject.team, %data.getName()]--;

   // Update the hud
   %max = $TeamDeployableMax[TripMineDeployed];
   %used = $TeamDeployedCount[%mine.sourceObject.team, %data.getName()];
   messageClient(%mine.sourceObject.client, 'MsgGrenadeCnt', "", addTaggedString($DataToName[%data.sourceItem]), addTaggedString(%max - %used));
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowGrenade("Soldier");
SmsInv.AddGrenade(TripMine, "Trip Mine");

SmsInv.AllowAmmo("armor\tSoldier\t2");
SmsInv.AddAmmo(TripMineAmmo, 2);
