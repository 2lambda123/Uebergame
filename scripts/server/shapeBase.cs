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

// This file contains ShapeBase methods used by all the derived classes

function ShapeBase::throw( %this, %data )
{
   //echo("ShapeBase::throw(" SPC %this.getName() @", "@ %data SPC ")");

   if ( !isObject(%data) || !$Game::Running )
      return false;

   if ( %this.hasInventory( %data ) )
   {
      // Create the item to throw first...
      %item = %this.throwItem(%data);
      if(%item)
      {
         %this.throwObject(%item);
         return true;
      }
      return false;
   }
   return false;
}

function ShapeBase::throwItem( %this, %data )
{
   //echo("ShapeBase::throwItem(" SPC %this.getName() @", "@ %data SPC ")");

   // save off the ammo count on this item
   if ( $AmmoIncrement[%data.getName()] !$="" )
   {
      if ( %this.getInventory( %data ) < $AmmoIncrement[%data.getName()] )
         %ammoStore = %this.getInventory(%data);
      else
         %ammoStore = $AmmoIncrement[%data.getName()];
   }
   else
      %ammoStore = %this.getInventory( %data );

   %item = %data.create();
   %item.static = false;
   %item.ammoStore = %ammoStore;
   %this.decInventory( %data, %ammoStore );
   MissionCleanup.add( %item );
   %item.schedulePop();
   return %item;
}

// This is used for everything being tossed or thrown.
function ShapeBase::throwObject( %this, %obj )
{
   //echo("\c3ShapeBase::throwObject(" SPC %this.getName() SPC %obj.client.nameBase SPC "force:" SPC %this.throwStrength SPC ")");
   // Throw the given object in the direction the shape is looking.
   // The force value is hardcoded according to the current default
   // object mass and mission gravity (20m/s^2).
   //%throwForce = %this.throwStrength;

   // Might be a hidden object such as a Flag, unhide
   if ( %obj.isHidden() )
      %obj.setHidden(false);

   // Object was tossed not thrown must tweak the force based on mass of object
   if ( %this.throwStrength <= 0 )
      %throwForce = ( %obj.getDataBlock().mass * 0.75 );
   else
      %throwForce = ( %obj.getDataBlock().mass * %this.throwStrength );

   // Thrown by a corpse
   %srcCorpse = ( %this.getState() $= "Dead" );
   if ( %srcCorpse )
   {
      %vec = (-1.0 + getRandom() * 2.0) SPC (-1.0 + getRandom() * 2.0) SPC getRandom();
      %vec = VectorScale(%vec, 10);
   }
   else
   {
      // Start with the shape's eye vector...
      %eye = %this.getEyeVector();
      %vec = VectorScale( %eye, ( %throwForce * 20 ) );
   }

   // Add a vertical component to give the object a better arc
   %dot = VectorDot("0 0 1", %eye);
   if (%dot < 0)
      %dot = -%dot;

   %vec = vectorAdd( %vec, VectorScale( "0 0 " @ %throwForce * 8, 1 - %dot ) );

   // Add shape's velocity
   %vec = VectorAdd( %vec, %this.getVelocity() );

   // Set the object's position and initial velocity
   %pos = getBoxCenter(%this.getWorldBox());
   %obj.setTransform(%pos);
   %obj.applyImpulse(%pos, %vec);

   // Since the object is thrown from the center of the
   // shape, the object needs to avoid colliding with it's
   // thrower.
   %obj.setCollisionTimeout(%this);

   serverPlay3D( ThrowSnd, %this.getTransform() );

   // Reset the throw strength.
   %this.throwStrength = 0;
   return %obj;
}

//-----------------------------------------------------------------------------
// ShapeBase inventory support

function ShapeBase::use(%player, %data)
{
   if ( isObject(%player) && %data !$= "" )
   {
      // Need to prevent weapon changing when zooming, but only shapes
      // that have a connection.
      %conn = %player.getControllingClient();
      if (%conn)
      {
         %defaultFov = ($pref::Player::Fov);
         %fov = %conn.getControlCameraFov();
         if (%fov != %defaultFov)
            return false;
      }

      // Cannot use anything while in a station
      if( %player.inStation )
         return false;

      //echo("\c3ShapeBase::use( " @ %player.client.nameBase @ ", " @ %data @ " )");

      if ( %player.hasInventory( %data ) )
      {
         %data.onUse(%player);
         return true;
      }
      return false;
   }
   return false;
}

function ShapeBase::pickup(%this, %obj, %amount)
{
   // This method is called to pickup an object and add it to the inventory.
   // The datablock onPickup method is actually responsible for doing all the
   // work, including incrementing the inventory.

   %data = %obj.getDatablock();

   // Try and pickup the max if no value was specified
   if (%amount $= "")
      %amount = %this.maxInventory(%data) - %this.getInventory(%data);

   // The datablock does the work...
   if (%amount < 0)
      %amount = 0;
   if (%amount)
      return %data.onPickup(%obj, %this, %amount);
   return false;
}

function ShapeBase::hasInventory(%this, %data)
{
   //echo("\c3ShapeBase::hasInventory( " @ %this.client.nameBase @ ", " @ %data @ " )");
   if ( isObject( %data ) )
      return( %this.inv[%data.getName()] > 0 );
   else
      return( 0 );
}

function ShapeBase::maxInventory(%this, %data)
{
   if ( isObject( %data ) )
   {
      if ( $Server::TestCheats )
         return 999;
      else
         return %this.getDatablock().maxInv[%data.getName()];
   }
}

function ShapeBase::incInventory(%this, %data, %amount)
{
   if ( %data $="" )
      return 0;

   //echo("\c3ShapeBase::incInventory(" SPC %this.getClassName() SPC %data.getName() SPC %amount SPC ")");
   // Increment the inventory by the given amount.  The return value
   // is the amount actually added, which may be less than the
   // requested amount due to inventory restrictions.

   %max = %this.maxInventory( %data );
   %total = %this.inv[%data.getName()];
   if ( %total < %max )
   {
      if ( %total + %amount > %max )
         %amount = %max - %total;

      %this.setInventory( %data, %total + %amount );
      %data.incCatagory( %this ); // Inc the players weapon count
      return %amount;
   }
   return 0;
}

function ShapeBase::decInventory(%this, %data, %amount)
{
   if ( %data $="" )
      return 0;

   //echo("\c3ShapeBase::decInventory(" SPC %this.getClassName() SPC %data.getName() SPC %amount SPC ")");
   // Decrement the inventory by the given amount. The return value
   // is the amount actually removed.

   %total = %this.inv[%data.getName()];
   if ( %total > 0 )
   {
      if ( %total < %amount )
         %amount = %total;

      %this.setInventory( %data, %total - %amount, true );
      %data.decCatagory( %this ); // Dec the players weapon count
      return %amount;
   }
   return 0;
}

function ShapeBase::setInventory(%this, %data, %value, %force)
{
   if ( !isObject( %data ) )
      return;

   %name = %data.getName();
   //echo("\c3ShapeBase::setInventory(" @ %this.getClassName() SPC %name SPC %value SPC %force @ ")");
   //echo("Classname =" SPC %data.getClassName());

   if ( %value < 0 )
      %value = 0;
   else 
   {
      if ( !%force ) 
      {
         // Impose inventory limits
         %max = %this.maxInventory(%data);
         if ( %value > %max )
            %value = %max;
      }
   }

   // Only if values have changed
   if ( %this.inv[%name] != %value ) 
   {
      %this.inv[%name] = %value;
      %data.onInventory( %this, %value );

      if ( %data.className $= "Weapon" || %data.getClassName() $= "ProximityMineData" || %data.getClassName() $= "AITurretShapeData")
      {
         if ( %this.weaponSlotCount $= "" )
            %this.weaponSlotCount = 0;

         %cur = -1;
         for ( %slot = 0; %slot < %this.weaponSlotCount; %slot++ )
         {
            if ( %this.weaponSlot[%slot] $= %name )
            {
               %cur = %slot;
               break;
            }
         }

         if ( %cur == -1 )
         {
            // Put this weapon in the next weapon slot:
            //error("Put this weapon " @ %name @ " in the next weapon slot: " @ %this.weaponSlotCount);

            if ( %this.weaponSlot[%this.weaponSlotCount - 1] $= "Grenade" )
            {
               %this.weaponSlot[%this.weaponSlotCount - 1] = %name;
               %this.weaponSlot[%this.weaponSlotCount] = "Grenade";
            }
            else
               %this.weaponSlot[%this.weaponSlotCount] = %name;

            %this.weaponSlotCount++;
         }
         else
         {
            // Remove the weapon from the weapon slot:
            //error("Remove this weapon " @ %name @ " from the weapon slot: " @ (%this.weaponSlotCount -1));
            for ( %i = %cur; %i < %this.weaponSlotCount - 1; %i++ )
               %this.weaponSlot[%i] = %this.weaponSlot[%i + 1];

            %this.weaponSlot[%i] = "";
            %this.weaponSlotCount--;
         }

         //error("WeaponSlotCount" SPC %this.weaponSlotCount);
      }
      %this.getDataBlock().onInventory( %data, %value );
   }
   return %value;
}

function ShapeBase::getInventory(%this, %data)
{
   if ( isObject( %data ) )
      return( %this.inv[%data.getName()] );
   else
      return( 0 );
}

function ShapeBase::hasAmmo(%this, %item)
{
   if ( %item.image.usesEnergy )
      return( true );

   if( %this.getInventory( %item.image.ammo ) > 0 || %this.getInventory( %item.image.clip ) > 0 )
      return( true );

   return( false );
}

function ShapeBase::clearInventory(%this)
{
   // Loop through our arrays and set our inventory of each to zero
   //echo("\c3ShapeBase::clearInventory(" @ %this.client.nameBase @ ")");

   // Weapons
   for( %i = 0; %i < $SMS::MaxWeapons; %i++ )
      %this.setInventory( $SMS::Weapon[%i], 0 );

   // Clips
   for( %i = 0; %i < $SMS::MaxClips; %i++ )
      %this.setInventory( $SMS::Clip[%i], 0 );

   // Ammo
   for( %i = 0; %i < $SMS::MaxAmmos; %i++ )
      %this.setInventory( $SMS::AmmoName[%i], 0 );

   // Specials
   for( %i = 0; %i < $SMS::MaxItems; %i++ )
      %this.setInventory( $SMS::Item[%i], 0 );
}

//commandToServer('GiveAll');
function serverCmdGiveAll(%client)
{
   // all weapons
   if(%client.isSuperAdmin && $Server::TestCheats)
   {
      %player = %client.player;

      for( %i = 0; %i < $SMS::MaxWeapons; %i++ )
         %player.setInventory( $SMS::Weapon[%i], 1 );

      for( %i = 0; %i < $SMS::MaxClips; %i++ )
         %player.setInventory( $SMS::Clip[%i], 999 );

      for( %i = 0; %i < $SMS::MaxAmmos; %i++ )
         %this.setInventory( $SMS::AmmoName[%i], 999 );
   }
}

//-----------------------------------------------------------------------------
// ShapeBase DataBlock
//-----------------------------------------------------------------------------

function ShapeBaseData::onUse(%this, %user)
{
   // Invoked when the object uses this datablock, should return
   // true if the item was used.
   return false;
}

function ShapeBaseData::onThrow(%this, %user, %amount)
{
   // Invoked when the object is thrown.  This method should
   // construct and return the actual mission object to be
   // physically thrown.  This method is also responsible for
   // decrementing the user's inventory.

   return 0;
}

function ShapeBaseData::onPickup(%this, %obj, %user, %amount)
{
   // Invoked when the user attempts to pickup this datablock object.
   // The %amount argument is the space in the user's inventory for
   // this type of datablock.  This method is responsible for
   // incrementing the user's inventory is something is addded.
   // Should return true if something was added to the inventory.

   return false;
}

function ShapeBaseData::onInventory(%this, %user, %value)
{
   // Invoked whenever an user's inventory total changes for
   // this datablock.
}

//-----------------------------------------------------------------------------
// ShapeBase object
//-----------------------------------------------------------------------------

function ShapeBase::damage(%this, %source, %position, %damage, %damageType)
{
   // All damage applied by one object to another should go through this
   // method. This function is provided to allow objects some chance of
   // overriding or processing damage values and types.  As opposed to
   // having weapons call ShapeBase::applyDamage directly.
   // Damage is redirected to the datablock, this is standard proceedure
   // for many built in callbacks.

   //echo("\c3ShapeBase::damage(" SPC %this @", "@ %source @", "@ %position @", "@ %damage @", "@ %damageType SPC ")");
   if(isObject(%this))
      %this.getDataBlock().damage(%this, %source, %position, %damage, %damageType);
}

// This is called by function ::damage. Used to remove energy when damage is inflicted instead
// of removing health, unless energy is drained to a point at which it cannot absorb the damage.
function ShapeBase::imposeShield(%obj, %position, %amount, %damageType)
{
   //echo("\c3ShapeBase::imposeShield(" SPC %obj.getClassName() @", "@ %obj @", "@ %position @", "@ %amount @", "@ %damageType SPC ")");
   %energy = %obj.getEnergyLevel();
   %data = %obj.getDataBlock();
   %strength = %energy / %data.energyPerDamagePoint;

   %shieldScale = %data.shieldDamageScale[%damageType];
   if ( %shieldScale $= "" )
      %shieldScale = 1;

   if ( %amount * %shieldScale <= %strength )
   {
      // Shield absorbs all
      %lost = %amount * %data.energyPerDamagePoint;
      %energy -= %lost;
      %obj.setEnergyLevel(%energy);

      return 0;
   }
   // Shield exhausted
   %obj.setEnergyLevel(0);
   return %amount - %strength;
}

function ShapeBase::setDamageDt(%this, %time, %damageAmount, %damageType)
{
   //echo("ShapeBase::setDamageDt(" SPC %this.client.nameBase @", "@ %time @", "@ %damageAmount @", "@ %damageType SPC ")");

   // This function is used to apply damage over time.
   %this.checkDamageDt(%time, %damageAmount, %damageType);
}

function ShapeBase::checkDamageDt(%this, %time, %damageAmount, %damageType)
{
   if ( %this.getDamageLevel() < %this.getDataBlock().maxDamage )
   {
      %this.damage(0, %this.getPosition(), %damageAmount, %damageType);
      %this.damageSchedule = %this.schedule(%time, "checkDamageDt", %time, %damageAmount, %damageType);
   }
   else
      %this.clearDamageDt();
}

function ShapeBase::clearDamageDt(%this)
{
   cancel(%this.damageSchedule);
   %this.damageSchedule = "";
}

function GameBase::damage(%this, %sourceObject, %position, %damage, %damageType)
{
   // All damage applied by one object to another should go through this method.
   // This function is provided to allow objects some chance of overriding or
   // processing damage values and types.  As opposed to having weapons call
   // ShapeBase::applyDamage directly. Damage is redirected to the datablock,
   // this is standard procedure for many built in callbacks.
      
   %datablock = %this.getDataBlock();
   if ( isObject( %datablock ) )
      %datablock.damage(%this, %sourceObject, %position, %damage, %damageType);
}

// ZOD: Avoid engine crashes by setting a schedule to unmount images
function ShapeBase::dismountImage(%this, %slot)
{
   //LogEcho("ShapeBase::dismountImage(" SPC %this.client.nameBase @", "@ %slot SPC ")");
   if( !isObject( %this ) || %this.getMountedImage( %slot ) == 0 )
   {
      return( false );
   }
   else
   {
      %this.unmountImage( %slot );
      return( true );
   }
}

//-----------------------------------------------------------------------------
// ZOD: Team functions, should be in the engine
//-----------------------------------------------------------------------------
/* //removing this should make the turrets shoot again
function ShapeBase::setTeamId(%obj, %team)
{
   if ( %team < 0 )
      %team = 0;

   %obj.teamdId = %team;
}

function ShapeBase::getTeamId(%obj)
{
   return %obj.teamId;
}
*/
//-----------------------------------------------------------------------------
// ShapeBaseImage datablock
//-----------------------------------------------------------------------------

function ShapeBaseImageData::onActivate(%data, %obj, %slot)
{
   // Reset the image trigger 
   %obj.setImageTrigger(%slot, false);
}

function ShapeBaseImageData::onDeactivate(%data, %obj, %slot)
{
   %obj.setImageTrigger(%slot, false);
}

function ShapeBaseImageData::onMount(%data, %obj, %slot)
{
   %obj.thrownChargeId = 0;
}

function ShapeBaseImageData::onUnmount(%data, %obj, %slot)
{
   if (%data.deleteLastProjectile && isObject(%obj.lastProjectile))
   {
      %obj.lastProjectile.delete();
      %obj.lastProjectile = "";
   }
}

function ShapeBaseImageData::onDeconstruct(%data, %obj, %slot)
{
   if (%data.deleteLastProjectile && isObject(%obj.lastProjectile))
   {
      %obj.lastProjectile.delete();
      %obj.lastProjectile = "";
   }
}

function ShapeBaseImageData::onFire(%data, %obj, %slot)
{
   //LogEcho("ShapeBaseImageData::onFire(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");

   if ( %data.ammo !$="" && !%obj.client.isAiControlled() ) // Ai has unlimited ammo, cause.. lazy bones
   {
      if ( %obj.getInventory( %data.ammo ) <= 0 )
         return;

      // Decrement inventory ammo. The image's ammo state is update
      // automatically by the ammo inventory hooks.
      %obj.decInventory( %data.ammo, 1 );
   }

   if ( %data.usesEnergy )
   {
      if ( %obj.getEnergyLevel() < %data.minEnergy )
         return;

      %obj.setEnergyLevel( %obj.getEnergyLevel() - %data.fireEnergy );
   }

   %data.lightStart = $Sim::Time;

   //if ( %obj.getClassname() $= "Player" || %obj.getClassname() $= "AiPlayer" )
   if ( %obj.isMemberOfClass( "Player" ) )
      %obj.setInvincible( false ); // fire your weapon and your invincibility goes away.

   if( %obj.inStation $= "" && %obj.isCloaked() )
   {
      if( %obj.respawnCloakThread !$= "" )
      {
         cancel(%obj.respawnCloakThread);
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
   if (isObject(%obj.lastProjectile) && %obj.deleteLastProjectile)
      %obj.lastProjectile.delete();

   if( %data.projectileSpread > 0 )
   {
      %vec = %obj.getMuzzleVector(%slot);
      %x = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %y = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %z = (getRandom() - 0.5) * 2 * 3.1415926 * %data.projectileSpread;
      %mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
      %muzzleVector = MatrixMulVector(%mat, %vec);
   }
   else
   {
      //%muzzleVector = %obj.getMuzzleVector(%slot);
      %muzzleVector = MatrixMulVector("0 0 0 0 0 1 0", %obj.getMuzzleVector(%slot));
   }

   // Determin initial projectile velocity based on the 
   // gun's muzzle point and the object's current velocity
   %objectVelocity = %obj.getVelocity();
   %muzzleVelocity = VectorAdd(VectorScale(%muzzleVector, %data.projectile.muzzleVelocity), VectorScale(%objectVelocity, %data.projectile.velInheritFactor));

   // Create the projectile object
   %p = new (%data.projectileType)() {
      dataBlock        = %data.projectile;
      initialVelocity  = %muzzleVelocity;
      initialPosition  = %obj.getMuzzlePoint(%slot);
      // This parameter is deleted about 7 ticks into the projectiles flight
      sourceObject     = %obj;
      sourceSlot       = %slot;
      // We use this for the source object when applying damage because it isn't deleted
      origin           = %obj;
      client           = %obj.client;
   };

   %obj.lastProjectile = %p;
   %obj.deleteLastProjectile = %data.deleteLastProjectile;
   if(%obj.client)
      %obj.client.projectile = %p;

   MissionCleanup.add(%p);

   //%p.mountPointLight( 10, "LightFlareExample1", 1, "FireLightAnim", 1, "DualParaboloidSinglePass" );

   return %p;
}

function Projectile::mountPointLight(%this, %radius, %flare, %anim, %animtype, %shdw, %shdwtype)
{
   %light = new PointLight() {
      radius = %radius;
      isEnabled = 1;
      color = "1 0.905882 0 1";
      brightness = 0.5;
      castShadows = %shdw;
      priority = 1;
      animate = %anim;
      animationType = %animtype;
      animationPeriod = 1;
      animationPhase = 1;
      flareScale = 0.15;
      flareType = %flare;
      attenuationRatio = "0 1 1";
      shadowType = %shdwtype;
      texSize = "256";
      overDarkFactor = "2000 1000 500 100";
      shadowDistance = "100";
      shadowSoftness = "0.15";
      numSplits = 1;
      logWeight = "0.91";
      fadeStartDistance = 0;
      lastSplitTerrainOnly = 0;
      splitFadeDistances = "10 20 30 40";
      representedInLightmap = 0;
      shadowDarkenColor = "0 0 0 -1";
      includeLightmappedGeometryInShadow = 0;
   };

   %this.mountObject(%light, 0, "0 0 0");
   %this.light = %light;
   %light.origin = %this;
   MissionCleanup.add(%light);
}

// ----------------------------------------------------------------------------
// A "generic" weaponimage onAltFire handler for most weapons.  Can be
// overridden with an appropriate namespace method for any weapon that requires
// a custom firing solution.
// ----------------------------------------------------------------------------

function ShapeBaseImageData::onAltFire(%data, %obj, %slot)
{
   //echo("\c4WeaponImage::onAltFire("@%data.getName()@", "@%obj.client.nameBase@", "@%slot@")");
   %data.onFire(%obj, %slot);
}

//-----------------------------------------------------------------------------
// Special cases for throwing items via mounted image

function ShapeBaseImageData::chargeStart(%data, %obj, %slot)
{
   %obj.startTime = getSimTime();

   // Release the main weapon trigger and unmount the weapon
   if ( %obj.getMountedImage($WeaponSlot) != 0 )
   {
      %obj.setImageTrigger($WeaponSlot, false);
      %obj.unmountImage($WeaponSlot);
   }
}

function ShapeBaseImageData::onThrowGrenade(%data, %obj, %slot)
{
   // Make sure we meet requirments
   if( %data.ammo !$="" )
   {
      if( %obj.getInventory( %data.ammo ) <= 0 )
         return %obj.setImageTrigger( %slot, false );

      %obj.decInventory(%data.ammo, 1);
   }

   if ( %data.usesEnergy )
   {
      if(%obj.getEnergyLevel() < %data.minEnergy)
         return %obj.setImageTrigger(%slot, false);

      %obj.setEnergyLevel(%obj.getEnergyLevel() - %data.fireEnergy);
   }

   // Work out the force of the throw
   %throwStrength = (getSimTime() - %obj.startTime) / 1000;
   if( %throwStrength < 1 ) // Realy really short hold?
      %throwStrength = 1;

   if( %throwStrength > 3 )
      %throwStrength = 3;

   // Bots just toss them with no charge up so lets change this up a little..
   %client = %obj.client;
   if ( ( isObject( %client ) && %client.isAiControlled() ) || %obj.isBot )
      %throwStrength = 2;

   %obj.throwStrength = %throwStrength;

   %data.lightStart = $Sim::Time;
   %obj.setInvincible( false ); // Throw a grenade and your invincibility goes away.

   if( %obj.inStation $= "" && %obj.isCloaked() )
   {
      if( %obj.respawnCloakThread !$= "" )
      {
         cancel(%obj.respawnCloakThread);
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

   // Create the grenade to throw
   %thrownItem = %data.thrownItem.create();
   %thrownItem.static = false;
   %thrownItem.rotate = false;
   %thrownItem.sourceObject = %obj;
   %thrownItem.team = %obj.team;
   MissionCleanup.add(%thrownItem);

   // Throw it
   %thrownItem.playThread(0, "Deploy");
   %obj.throwObject(%thrownItem);

   %thrownItem.detThread = %thrownItem.getDataBlock().schedule(%data.thrownItem.detonationTime, "detonate", %thrownItem);

   if ( %obj.inv[%obj.lastWeapon] )
      %obj.use( %obj.lastWeapon );
   else
      %obj.use( %obj.weaponSlot[0] );

   return %thrownItem;
}

function GrenadeImage::onDryFire(%this, %obj, %slot)
{
   if ( %obj.inv[%obj.lastWeapon] )
      %obj.use( %obj.lastWeapon );
   else
      %obj.use( %obj.weaponSlot[0] );
}

