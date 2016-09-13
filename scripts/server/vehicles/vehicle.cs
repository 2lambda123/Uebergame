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

function WheeledVehicleData::create(%block, %team, %marker)
{
   if(%marker $= "")
   {
      %obj = new WheeledVehicle() {
         dataBlock = %block;
         resetPos = "1";
         respawn = "1";
         respawnTime = "60000";
         mountable = true;
         team = %team;
      };
      return(%obj);
   }
   else
   {
      %marker.obj = new WheeledVehicle() {
         dataBlock = %block;
         team = %team;
         mountable = %marker.mountable;
         disableMove = %marker.disableMove;
         resetPos = %marker.resetPos;
         respawn = %marker.respawn;
         respawnTime = %marker.respawnTime;
         marker = %marker;
      };
      return(%marker.obj);
   }
}

function FlyingVehicleData::create(%block, %team, %marker)
{
   if(%marker $= "")
   {
      %obj = new FlyingVehicle() 
      {
         dataBlock = %block;
         resetPos = "1";
         respawn = "1";
         respawnTime = "60000";
         mountable = true;
         team = %team;
      };
      return(%obj);
   }      
   else
   {
      %marker.obj = new FlyingVehicle() 
      {
         dataBlock  = %block;
         team = %team;
         mountable = %marker.mountable;
         disableMove = %marker.disableMove;
         resetPos = %marker.resetPos;
         respawn = %marker.respawn;
         respawnTime = %marker.respawnTime;
         marker = %marker;
      };
      return(%marker.obj);
   }
}

function HoverVehicleData::create(%block, %team, %marker)
{
   if(%marker $= "")
   {
      %obj = new HoverVehicle() 
      {
         dataBlock = %block;
         resetPos = "1";
         respawn = "1";
         respawnTime = "60000";
         mountable = true;
         team = %team;
      };
      return(%obj);
   }
   else
   {
      %marker.obj = new HoverVehicle()
      {
         dataBlock  = %block;
         team = %team;
         mountable = %marker.mountable;
         disableMove = %marker.disableMove;
         resetPos = %marker.resetPos;
         respawn = %marker.respawn;
         respawnTime = %marker.respawnTime;
         marker = %marker;
      };
      return(%marker.obj);
   }
}

function VehicleData::onNewDataBlock(%data, %obj)
{
   //echo("VehicleData::onNewDataBlock( " @ %data @ ", " @ %obj @ " )");
   //%obj.dump();
}

function VehicleData::onAdd(%data, %obj)
{
   %obj.setRechargeRate(%data.rechargeRate);
   %obj.setEnergyLevel(%data.maxEnergy);
   %obj.setRepairRate(0);
   if ( %obj.disableMove )
      %obj.immobilized = true;

   // IF a vehicle has turrets mounted to it or some other non imagedata shape, set it up on a schedule.
   //if ( %vehicle.deployed )
   //{
   //   if ( $pref::Server::warmupTime >= 2 )
   //      %data.schedule( ( $pref::Server::warmupTime * 1000 ) / 2, "vehicleDeploy", %vehicle, 0, 1 );
   //   else
   //      %data.schedule( 2000, "vehicleDeploy", %vehicle, 0, 1 );
   //}

   if(%obj.mountable || %obj.mountable $= "")
      %data.isMountable(%obj, true);
   else
      %data.isMountable(%obj, false);

   %obj.setTeamId(%obj.team);
   %obj.setShapeName(%data.nameTag);
   %obj.setSelfPowered();
}

function VehicleData::onRemove(%data, %obj)
{
   // if there are passengers/driver, kick them out
   for(%i = 0; %i < %obj.getDatablock().numMountPoints; %i++)
   {
      if ( %obj.getMountNodeObject(%i) ) 
      {
         %passenger = %obj.getMountNodeObject(%i);
         %passenger.getDataBlock().doDismount(%passenger, true);
      }
   }
   if(%obj.lastPilot.lastVehicle == %obj)
      %obj.lastPilot.lastVehicle = "";
}

function VehicleData::hasDismountOverrides(%data, %obj)
{
   return false;
}

function VehicleData::onCollision(%data, %vehicle, %col, %vec, %speed)
{
   //:ogEcho("VehicleData::onCollision( " @ %data.getName() @ ", " @ %vehicle.getClassName() @ ", " @ %col.getClassName() @ ", " @ %vec @ ", " @ %speed @ " )");

   // Zod: Killing this stuff for now, causing crashes.. Works fine TGE 1.5

   // Collision with other objects, including items and itself?!?!?!
   if ( !isObject( %vehicle ) || !isObject( %col ) || %col == %vehicle || %vehicle.getDamageState() $= "Destroyed" )
      return;

   if ( %col.isMemberOfClass("Player") )
   {
      if ( %col.getState() $= "Dead" )
         return;

      %colData = %col.getDataBlock();
      if ( %speed > %colData.minImpactSpeed )
      {
         %colData.onImpact( %col, %vehicle, %vec, %speed );
         %vehicle.playAudio( 0, %data.hardImpactSound );
      }
      else
         %vehicle.playAudio( 0, %data.softImpactSound );
   }
}

function VehicleData::onImpact(%data, %obj, %col, %vec, %vecLen)
{
   //error("VehicleData::onImpact(" SPC %data.getName() @", "@ %obj.getClassName() @", "@ %col @", "@ %vec @", "@ %vecLen SPC ")");

   if ( %vecLen > %data.minImpactSpeed )
      %obj.damage( 0, VectorAdd(%obj.getPosition(), %vec), %vecLen * %data.speedDamageScale, $DamageType::Impact );

   // associated "crash" sounds
   if(%vecLen > %data.hardImpactSpeed)
      %obj.playAudio(0, %data.hardImpactSound);
   else if(%vecLen > %data.softImpactSpeed)
      %obj.playAudio(0, %data.softImpactSound);
}

function VehicleData::onFlipped(%this, %vehicle, %flipped)
{
   //LogEcho("VehicleData::onFlipped(" SPC %this.getName() @", "@ %vehicle.getClassName() @", "@ %flipped SPC ")");
   %rot =  %vehicle.rotFromTransform();
   %newRot = "0 0" SPC getwords(%rot, 2, 3);
   %vehicle.setTransform(%vehicle.getPosition() SPC %newRot);
}

function VehicleData::damage(%data, %obj, %sourceObject, %position, %damage, %damageType)
{
   //echo("VehicleData::damage( " @ %data.getName() @ ", " @ %obj @ ", " @ %sourceObject @ ", " @ %position @ ", " @ %damage @ ", " @ %damageType @ " )");
   if ( %obj.isDestroyed() || %data.isInvincible )
      return;

   %obj.damageTimeMS = GetSimTime();

   if ( isObject(%sourceObject) )
      %obj.lastDamagedBy = %sourceObject;
   else
      %obj.lastDamagedBy = 0;

   %obj.damageTimeMS = GetSimTime();

   if ( %data.isShielded )
      %amount = %obj.imposeShield(%position, %damage, %damageType); // Resides in server/shapeBase.cs

   %damageScale = %data.damageScale[%damageType];
   if(%damageScale !$= "")
      %amount *= %damageScale;

   if ( %damage > 0 )
      %obj.applyDamage(%damage);
}

function VehicleData::onDamage(%this, %obj)
{
   //echo("VehicleData::onDamage( " @ %this @ ", " @ %obj @ " )");
   if ( %obj.getDamageState() $= "Destroyed" )
      return;

   %damage = %obj.getDamageLevel();
   if ( %damage >= %this.destroyedLevel )
   {
      if ( %obj.getDamageState() !$= "Destroyed" )
      {
         if ( %obj.respawnTime !$= "" )
            %obj.marker.schedule = %this.schedule(%obj.respawnTime, "respawn", %obj.marker); 

         // Let the game object have a shot at doing something with this information..
         // We put this here so we have some time before the vehicle is deleted.
         if ( isObject( %obj.lastDamagedBy ) && isObject( Game ) )
            Game.vehicleDestroyed( %obj, %obj.lastDamagedBy );

         %obj.setDamageState( Destroyed );
      }
   }
   else
   {
      if ( %obj.getDamageState() !$= "Enabled" )
         %obj.setDamageState( Enabled );
   }
}

function VehicleData::onDisabled(%data, %obj, %state)
{
   //echo("VehicleData::onDisabled( " @ %data.getName() @ ", " @ %obj @ ", " @ %state @ " )");
   // Increase source of the damage ranking points
}

function VehicleData::onDestroyed(%data, %obj, %prevState)
{
   //echo("VehicleData::onDestroyed( " @ %data.getName() @ ", " @ %obj @ ", " @ %prevState @ " )");

   radiusDamage( %obj, %obj, %obj.getPosition(), "8", "0.5", $DamageType::Explosion, 0 );

   for(%i = 0; %i < %obj.getDatablock().numMountPoints; %i++)
   {
      if ( %obj.getMountNodeObject( %i ) > 0 )
      {
         %flingee = %obj.getMountNodeObject(%i);
         if ( %flingee.isMemberOfClass("Player") )
         {
            %flingee.getDataBlock().doDismount( %flingee, true );
            %xVel = 150.0 - (getRandom() * 300.0);
            %yVel = 150.0 - (getRandom() * 300.0);
            %zVel = (getRandom() * 50.0) + 50.0;
            %flingVel = %xVel @ " " @ %yVel @ " " @ %zVel;
            %flingee.applyImpulse( %flingee.getTransform(), %flingVel );
            %flingee.damage( %obj, %obj.getPosition(), 0.4, $DamageType::Crash );
         }
      }
   }

   // Make sure we decrement the count of this vehicle
   $VehicleTotalCount[%obj.team, %data.getName()]--;

   %obj.schedule( 2000, "delete" );
}

function VehicleData::onTrigger(%this, %obj, %trigger, %val)
{
   // trigger = 0 for "fire", 1 for "jump", 3 for "thrust"
   // state = 1 for firing, 0 for not firing
   //echo("VehicleData::onTrigger( " @ %this.getName() @ ", " @ %obj.getDataBlock().getName() @ ", " @ %trigger @ ", " @ %val @ " )");
}

function VehicleData::onEnterLiquid(%data, %obj, %coverage, %type)
{
   switch(%type)
   {
      case 0: //Water
         %obj.isInWater = 1;
         //%obj.setDamageDt(1000, 2, $DamageType::Water);
      case 1: //Ocean Water
         %obj.isInWater = 1;
         //%obj.setDamageDt(1000, 2, $DamageType::Water);
      case 2: //River Water
         %obj.isInWater = 1;
         //%obj.setDamageDt(1000, 2, $DamageType::Water);
      case 3: //Stagnant Water
         %obj.isInWater = 1;
         //%obj.setDamageDt(1000, 2, $DamageType::Water);
      case 4: //Lava
         %obj.isInWater = 1;
         %obj.setDamageDt(1000, 4, $DamageType::Lava);
      case 5: //Hot Lava
         %obj.isInWater = 1;
         %obj.setDamageDt(1000, 5, $DamageType::Lava);
      case 6: //Crusty Lava
         %obj.isInWater = 1;
         %obj.setDamageDt(1000, 6, $DamageType::Lava);
      case 7: //Quick Sand
         %obj.setDamageDt(1000, 2, $DamageType::QuickSand);
   }
}

function VehicleData::onLeaveLiquid(%data, %obj, %type)
{
   %obj.clearDamageDt();
   %obj.isInWater = 0;
}

function VehicleData::onLeaveMissionArea(%this, %vehicle)
{
   // Let everyone know we are leaving mission area
   for(%i = 0; %i < %vehicle.getDatablock().numMountPoints; %i++)
   {
      if ( %vehicle.getMountNodeObject( %i ) > 0 )
      {
         %mounted = %vehicle.getMountNodeObject(%i);
         if ( %mounted.getClassName() $= "Player" )
         {
            messageClient(%mounted.client, 'LeaveMissionArea', '\c2You have left the mission area.');
            %mounted.outOfBounds = true;
         }
      }
   }
   //%vehicle.setDamageDt(1000, 2, "MissionAreaDamage");
}

function VehicleData::onEnterMissionArea(%this, %vehicle)
{
   // Let everyone know we are returning to mission area
   for(%i = 0; %i < %vehicle.getDatablock().numMountPoints; %i++)
   {
      if ( %vehicle.getMountNodeObject( %i ) > 0 )
      {
         %mounted = %vehicle.getMountNodeObject(%i);
         if ( %mounted.getClassName() $= "Player" )
         {
            messageClient(%mounted.client, 'EnterMissionArea', '\c2You are back in the mission area.');
            %mounted.outOfBounds = false;
         }
      }
   }
   //%vehicle.clearDamageDt();
}

function VehicleData::playerMounted(%data, %vehicle, %player, %node)
{
   if ( %player.lastVehicle.lastPilot == %player && %player.lastVehicle != %vehicle )
   {
      schedule(15000, %player.lastVehicle, "abandonTimeOut", %player.lastVehicle );
      %player.lastVehicle.lastPilot = "";
   }

   if ( %vehicle.lastPilot !$= "" && %vehicle.lastPilot.lastVehicle == %vehicle )
      %vehicle.lastPilot.lastVehicle = "";
            
   %vehicle.lastPilot = %player;
   %player.lastVehicle = %vehicle;

   // update spectators who are following this guy...
   if( %player.client.observeCount > 0 )
      resetSpectatorFollow( %player.client, false );
}

function VehicleData::playerDismounted(%data, %obj, %player)
{
   // update spectators who are following this guy...
   if( %player.client.observeCount > 0 )
      resetSpectatorFollow( %player.client, true );
}

function VehicleData::checkIfPlayersMounted(%data, %obj)
{
   for(%i = 0; %i < %obj.getDatablock().numMountPoints; %i++)
      if (%obj.getMountNodeObject(%i))
         return true;

   return false;
}

//-----------------------------------------------------------------------------
// Vehicle creation and respawning

function SimGroup::setupPositionMarkers(%group, %create)
{
   for (%i = 0; %i < %group.getCount(); %i++)
   {               
      %obj = %group.getObject(%i);
      if ( %obj.getClassName() $= SimGroup )
         %obj.setupPositionMarkers(%create);
      else
      {
         if ( %obj.getType() & ( $TypeMasks::VehicleObjectType ) )
         {
            if ( %obj.resetPos || %obj.respawnTime !$= "" )
            {
               if ( %create )
                  %marker = %obj.getDataBlock().createPositionMarker(%obj);
               else
                  %obj.delete();
            }
         }
      }
   }               
}

function VehicleData::createPositionMarker(%data, %vehicle)
{
   //LogEcho("VehicleData::createPositionMarker( " @ %data.getName() @ ", " @ %vehicle.getClassName() @ " )");
   %marker = new MissionMarker()
   {
      dataBlock = VehicleMarker;
      mountable = %vehicle.mountable;
      disableMove = %vehicle.disableMove;
      resetPos = %vehicle.resetPos;
      data = %data.getName();
      deployed = %vehicle.deployed;
      team = %vehicle.team;
      respawn = %vehicle.respawn;
      respawnTime = %vehicle.respawnTime;
   };
   %vehicle.marker = %marker;
   %marker.setTransform(%vehicle.getTransform());
   %vehicle.getGroup().add(%marker);
   return %marker;
}

function VehicleData::respawn(%data, %marker)
{
   //LogEcho("VehicleData::respawn( " @ %data.getName() @ ", " @ %marker @ " )");
   %mask = $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;
   InitContainerRadiusSearch(%marker.getWorldBoxCenter(), %data.checkRadius, %mask);
   if ( containerSearchNext() == 0 )
   {
      %vehicle = %data.create(%marker.team, %marker);
      %vehicle.setTeamId(%marker.team);
      %vehicle.startFade(1000, 0, false);
      %vehicle.setTransform(%marker.getTransform());
      %marker.getGroup().add(%vehicle);
   }
   else
   {
      %data.schedule(3000, "respawn", %marker);
   }
}

function abandonTimeOut(%vehicle)
{
   //error("Can abandon?" SPC %vehicle.getDatablock().cantAbandon SPC "Last pilot?" SPC %vehicle.lastPilot );
   if ( %vehicle.getDatablock().cantAbandon $= "" && %vehicle.lastPilot $= "")
   {
      //error("abandonTimeOut( " @ %vehicle.getDataBlock().getName() @ ", " @ %vehicle.getClassName() @ " )");
      for ( %i = 0; %i < %vehicle.getDatablock().numMountPoints; %i++ )
      {
         if ( %vehicle.getMountNodeObject(%i) )
         {
            %passenger = %vehicle.getMountNodeObject(%i);
            if ( !(%passenger.getType() & $TypeMasks::TurretObjectType) )
            {
               if ( %passenger.lastVehicle !$= "" )
                  schedule(15000, %passenger.lastVehicle, "abandonTimeOut", %passenger.lastVehicle);

               %passenger.lastVehicle = %vehicle;   
               %vehicle.lastPilot = %passenger;   
               return;
            }
         }
      }

      if ( %vehicle.respawnTime !$= "" )
         %vehicle.marker.data.schedule(%vehicle.respawnTime, "respawn", %vehicle.marker); 

      %vehicle.getDatablock().isMountable(%vehicle, false);

      // Make sure we decrement the count of this vehicle
      $VehicleTotalCount[%vehicle.team, %vehicle.getDataBlock().getName()]--;
      %vehicle.startFade(1000, 0, true);
      %vehicle.schedule(1001, "delete");
   }
}

//-----------------------------------------------------------------------------
// Vehicle player mounting and dismounting

function VehicleData::isMountable(%data, %obj, %val)
{
   %obj.mountable = %val;
}

function VehicleData::mountPlayer(%data, %vehicle, %player)
{
   if ( isObject( %vehicle ) && %vehicle.getDamageState() !$= "Destroyed" )
   {
      %player.startFade( 1000, 0, true );
      %data.schedule( 1000, "setMountVehicle", %player, %vehicle );
      %player.schedule( 1500, "startFade", 1000, 0, false );
   }
}

function VehicleData::setMountVehicle(%data, %player, %vehicle)
{
   //echo( "VehicleData::setMountVehicle(" SPC %data.getName() @", "@ %player.client.nameBase @", "@ deTag(%vehicle.nameTag) SPC ")" );
   if ( isObject( %vehicle ) && %vehicle.getDamageState() !$= "Destroyed" )
   {
      %node = %data.findEmptySeat( %vehicle, %player );
      if ( %node >= 0 )
      {
         //error("Mount Node:" SPC %node);
         %vehicle.mountObject( %player, %node );
         %player.playAudio( 0, MountVehicleSound );
         %player.mVehicle = %vehicle;
         %data.playerMounted( %vehicle, %player, %node );
      }
   }
}

function VehicleData::findEmptySeat(%data, %vehicle, %player)
{
   //echo("This vehicle has " @ %data.numMountPoints @ " mount points.");
   for ( %i = 0; %i < %data.numMountPoints; %i++ )
   {
      %node = %vehicle.getMountNodeObject(%i);
      if ( %node == 0 )
      {
         return %i;
      }
   }
   return -1;
}

function VehicleData::switchSeats(%data, %vehicle, %player)
{
   echo("VehicleData::switchSeats(" SPC %data.getName() @", "@ %vehicle @", "@ %player.client.nameBase SPC ")");
   for ( %i = 0; %i < %data.numMountPoints; %i++ )
   {
      %node = %vehicle.getMountNodeObject(%i);
      error("Node:" SPC %node);
      if ( %node == %player || %node > 0 )
         continue;

      if ( %node == 0 )
         return %i;
   }
   return -1;
}

//-----------------------------------------------------------------------------
// Vehicle server commands

function serverCmdSwitchVehicleWeapon(%client, %dir)
{
   //error("serverCmdSwitchVehicleWeapon(" SPC %client.nameBase @", "@ %dir @", "@  %vehicle.getDataBlock().getName() SPC ")");
   //%vehicle = %client.player.getControlObject();
   %vehicle = %client.player.mVehicle;

   if ( !isObject( %vehicle ) || %vehicle.getDataBlock().numWeapons <= 1 )
      return;

   %weaponNum = %vehicle.selectedWeapon;
   if(%dir $= "next")
   {
      if(%weaponNum++ > %vehicle.getDataBlock().numWeapons)
         %weaponNum = 1;
   }
   else
   {
      if(%weaponNum-- < 1)
         %weaponNum = %vehicle.getDataBlock().numWeapons;
   }

   if ( %vehicle.getDataBlock().numWeapons < %weaponNum )
      return;

   %vehicle.selectedWeapon = %weaponNum;
   messageClient( %client, 'MsgVWeapon', 'Weapon %1 selected.', %weaponNum );
}

function serverCmdMountVehicle(%client)
{
   %player = %client.player;
   if ( !isObject( %player ) || %player.getState() $= "Dead" || !$Game::Running )
      return( false );

   // Exit vehicle
   if ( isObject( %player.mVehicle ) && %player.isMounted() )
   {
      %player.getDataBlock().doDismount( %player, 0 );
      return( true );
   }

   // Switch seats
   if ( isObject( %player.mVehicle ) && %player.isMounted() )
   {
      %seat = %player.mVehicle.getDatablock().switchSeats( %player.mVehicle, %player );
      if ( %seat >= 0 )
      {
         //error("Switching seats to node:" SPC %seat );
         commandToClient(%client, 'setHudMode', 'Play');

         // Dismount
         %player.unmount();

         // Reset control
         %player.setControlObject(%player);

         // Let the vehicle know
         %player.mVehicle.getDatablock().playerDismounted( %player.mVehicle, %player );

         // Mount the new node
         %player.mVehicle.mountObject( %player, %seat );
         %player.playAudio( 0, MountVehicleSound );
         %player.mVehicle.getDatablock().playerMounted( %player.mVehicle, %player, %seat );
         return( true );
      }
   }

   InitContainerRadiusSearch( %player.getPosition(), 20, ( $TypeMasks::VehicleObjectType ) );
   while ( ( %vehicle = containerSearchNext()) != 0 )
   {
      if ( %player.mountVehicle )
      {
         if ( !%vehicle.mountable )
         {
            messageClient( %client, 'MsgMountFailed', '\c2Vehicle is not ready.');
            return;
         }

         if ( %vehicle.team == %player.team || %vehicle.team <= 0 )
         {
            %vel = vectorDot(%vel, vectorNormalize(%vehicle.getVelocity()));
            if ( %vel <= %vehicle.getDataBlock().maxMountSpeed )
            {
               if ( %vehicle.getType() & ( $TypeMasks::VehicleObjectType ) )
               {
                  // Unflip flipped vehicles....
                  %rot =  %vehicle.rotFromTransform();
                  %check[1] = firstWord( %rot );
                  %check[2] = getWord( %rot, 1 );
                  %flipped = 0;
                  for ( %i = 0; %i < 2; %i++ )
                  {
                     if ( ( %check[%i] > 0 || %check[%i] < 0 )&& getWord( %rot, 2 ) > 10 )
                        %flipped = 1;
                  }

                  if ( %flipped )
                  {
                     %newRot = "0 0" SPC getwords(%rot, 2, 3);
                     %vehicle.setTransform(%vehicle.getPosition() SPC %newRot);
                     messageClient( %client, 'MsgMountFailed', '\c2Unflipping Vehicle.');
                  }
               }

               messageClient( %client, 'MsgMountSuccess', '\c2Entering %1.', %vehicle.getDataBlock().nameTag);
               %player.mountVehicles(false); // Safety
               %vehicle.getDataBlock().mountPlayer(%vehicle, %player);
               return;
            }
            else
            {
               messageClient( %client, 'MsgMountFailed', '\c2Cannot enter a moving vehicle.');
               return;
            }
         }
         else
         {
            messageClient( %client, 'MsgMountFailed', '\c2Cannot enter an enemy vehicle.');
            return;
         }
      }
   }

   messageClient( %client, 'MsgMountFailed', '\c2No vehicle in range.');
}

function serverCmdUpRightVehicle(%client)
{
   %vehicle = %client.player.getControlObject();
   //%vehicle = %client.player.lastVehicle;
   
   %rot =  %vehicle.rotFromTransform();
   %newRot = "0 0" SPC getwords(%rot, 2, 3);
   %vehicle.setTransform(%vehicle.getPosition() SPC %newRot);
}

function serverCmdSelectVehicle(%client, %vehicle)
{
   // Send the vehicle choice back to the client.
   commandToClient(%client, 'SelectedVehicle', %vehicle);

   %invName = deTag(%vehicle);
   %client.selectedVehicle = %invName;
   %left = $Game::VehicleMax[$NameToVehicle[%invName]] - $VehicleTotalCount[%client.team, $NameToVehicle[%invName]];
   messageClient( %client, 'MsgVehiclesLeft', '\c2%2 %1\s remaining.', %vehicle, %left);
}
// Spawn a vehicle on terrain or interior
/*
function serverCmdCreateVehicle(%client, %vehicle)
{
   %player = %client.player;
   if(!isObject( %player ) || %player.getState() $= "Dead" || !$Game::Running)
      return( false );

   // Limit vehicle creation to player with special vehicle creation pack
   if ( !%player.hasInventory( VehiclePack ) )
   {
      return( false );
   }

   %data = $NameToVehicle[%vehicle];

   // See if there is a valid creation surface in range
   %range = 20;
   %mask = ( $TypeMasks::TerrainObjectType );
   %searchResult = %player.doRaycast(%range, %mask); // Found in library.cs

   if(!%searchResult)
   {
      messageClient(%obj.client, 'MsgNoSurface', '\c2Cannot create %1. No valid surface found.', %vehicle);
      return;
   }

   // We need this for creation position
   %resultPos = getWord(%searchResult, 1) @ " " @ getWord(%searchResult, 2) @ " " @ getWord(%searchResult, 3);

   //%test = ($TypeMasks::VehicleObjectType | $TypeMasks::MoveableObjectType |
   //         $TypeMasks::StaticShapeObjectType | $TypeMasks::PlayerObjectType );
   %test = ($TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType);

   InitContainerRadiusSearch( %eyeEnd, 10, %test );
   %result = containerSearchNext();
   if(%result)
   {
      messageClient(%client, 'MsgNoSurface', '\c2You cannot create %1 so close to another object.', %vehicle);
      return;
   }
   else
   {
      if ( ( $Game::VehicleMax[%data.getName()] - $VehicleTotalCount[%client.team, %data.getName()] ) > 0 )
      {
         %obj = %data.create(%client.team);
         if(isObject(%obj))
         {
            %obj.getDataBlock().isMountable(%obj, false);
            %obj.getDataBlock().schedule(5000, "isMountable", %obj, true);

            MissionCleanup.add(%obj);
            %x = getword(%resultPos, 0);
            %y = getword(%resultPos, 1);
            %z = getword(%resultPos, 2) + %data.createHoverHeight;
            %obj.setTransform(%x SPC %y SPC %z SPC rotFromTransForm(%player.getTransform()));
            %obj.setCloaked(true);
            %obj.schedule(4000, "setCloaked", false);
            //messageClient(%client, 'MsgNoSurface', '\c2%2 %1\s left.', %vehicle, ( $Game::VehicleMax[%data.getName()] - $VehicleTotalCount[%client.team, %data.getName()] ));
         }
      }
      else
      {
         messageClient(%client, 'MsgNoSurface', '\c2Cannot create %1, limit reached.', %vehicle);
         return;
      }
   }
}
*/