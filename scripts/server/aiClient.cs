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
// AIClient -> AIConnection -> GameConnection -> NetConnection -> SimGroup -> SimSet -> SimObject
//-----------------------------------------------------------------------------
// Varibles
//-----------------------------------------------------------------------------

//%client.setMoveSpeed(10);
//%client.stop();
//%client.setAimLocation(%position);
//%client.setMoveDestination(%position);
//%client.getAimLocation();
//%client.getMoveDestination();
//%client.setTargetObject();
//%client.getTargetObject();
//%client.missionCycleCleanup();
//%client.move();
//%client.getLocation();
//%client.moveForward(); // Test function

$Bot::ThinkTime = 500;         // The time frequency at which the AI scans for player targets
$Bot::DetectionDistance = 50;   // The distance from the AI that it will search for other players
$Bot::LoseTargetDistance = 75; // The distance from the AI that it will break off its chase
$Bot::FireDistance = 20;        // Max distance from the AI that will cause it to fire
$Bot::MeleeDistance = 2;        // Durr
$Bot::FireDelay = 1000;         // Default Time AI can start shooting again after its fired a shot
$Bot::GrenadeDelay = 3000;      // Default Time AI can start throwing a grenade again after its tossed one
$Bot::MeleeDelay = 2000;        // Default Time AI can start melee again after its done its last
$Bot::FireTimeout = 500;        // Default time after pressing fire that the AI stops firing
$Bot::MoveSpeed = 1;

//-----------------------------------------------------------------------------
// Client naming functions, used for aiConnection::onConnect()
//-----------------------------------------------------------------------------

$RandomBotNameCount = 0;
function addBotName(%name)
{
   $RandomBotName[$RandomBotNameCount] = %name;
   $RandomBotNameCount++;
}

addBotName("Bullseye");
addBotName("Casualty");
addBotName("Fodder");
addBotName("Grunt");
addBotName("Bait");
addBotName("Meat");
addBotName("Tardo");
addBotName("Roadkill");
addBotName("SkidMark");
addBotName("Flatline");
addBotName("Spud");
addBotName("WormChow");
addBotName("Endangered");
addBotName("Squidloaf");
addBotName("Gimp");
addBotName("Masochist");
addBotName("Terminal");
addBotName("KickMe");
addBotName("Fred");
addBotName("Fluffy Bunny");
addBotName("Carcass");
addBotName("Spastic");
addBotName("Bumpkin");
addBotName("Mad Cow");
addBotName("Oblivious");

function getRandomBotName()
{
   %index = getRandom( $RandomBotNameCount-1 );
   return($RandomBotName[%index]); 
}

//-----------------------------------------------------------------------------

// Called from server.cs function Torque::buildServer() before loadMission()
function connectAiClients(%num)
{
   //error("connectAiClients(" SPC %num SPC ")");
   if ( %num $= "" || %num < 1 )
      return;

   if ( %num > 16 || %num > $pref::Server::MaxPlayers - 1 )
      %num = 16;

   // This in turn in C++ code calls aiConnect which sets up a new AIConnection
   for ( %i = 1; %i <= %num; %i++ )
   {
      aiAddPlayer( getRandomBotName() );
   }
}

// This is called from loadMissionStage2()
function AIConnection::onConnect(%client, %name)
{
   //echo("\c5AIClient::onConnect(" SPC %client SPC %name SPC ")");
   %client.isAdmin = false;
   %client.isSuperAdmin = false;

   // Get the client's unique id:
   %client.guid = $Server::BotCount;//%client.getAddress();
   addToServerGuidList( %client.guid );

   // Save client preferences on the connection object for later use.
   %client.armor = "Soldier";
   %client.skin = addTaggedString("Base");
   %client.setPlayerName(%name);
   %client.team = 0;
   %client.lastTeam = 0;
   %client.justConnected = true; // Set the flag they just joined, put in spec.
   %client.isWaiting = false; // Team change wait flag
   %client.isReady = false; // For tournament mode play
   %client.SadAttempts = 0; // Start off with 0 SAD access attempts.

   // Setup for voice
   %client.voiceTag = "default";
   %client.voicePitch = 1;

   echo("CADD: " @ %client.nameBase @ " " @ %client.getAddress());

   // Inform all the other clients of the new guy
   messageAllExcept(%client, -1, 'MsgClientJoin', '\c5%1 joined the game.',
      %client.playerName,
      %client,
      %client.guid,
      %client.isAiControlled(),
      %client.isAdmin,
      %client.isSuperAdmin,
      %client.team,
	  0);

   // Set the bot up with some inventory selection
   %client.setBotFav( %client.getRandomLoadout() );

   Game.schedule( 2000, "onClientEnterGame", %client );

   $Server::BotCount++; // Master server gets this
}

function AIConnection::setPlayerName(%client, %name)
{
   //echo( "AIClient::setPlayerName(" SPC %client @", "@ %name SPC ")" );
   %client.nameBase = %name;
   %client.playerName = addTaggedString("\cp\c8" @ %name @ "\co");
}

function AIConnection::onDrop(%client, %reason)
{
   if ( !isObject(%client) )
      return;

   if ( isObject(Game) )
      Game.onClientLeaveGame( %client );

   %client.missionCycleCleanup();
   removeFromServerGuidList( %client.guid );
   messageAllExcept(%client, -1, 'MsgClientDrop', '\c1%1 has left the game.', %client.playerName, %client);

   removeTaggedString( %client.playerName );
   removeTaggedString( %client.skin );

   echo("CDROP: " @ %client @ " " @ %client.getAddress());

   $Server::BotCount--; // Master server gets this
}

function AIConnection::isMounted(%client)
{
   return false;
   %vehicle = %client.getControlObject();
   %className = %vehicle.getDataBlock().className;
   if ( %className $= WheeledVehicleData || %className $= FlyingVehicleData || %className $= HoverVehicleData ) 
      return true;
   else
      return false;
}

//-----------------------------------------------------------------------------
// Ai Inventory
//-----------------------------------------------------------------------------
function AIClient::setBotFav(%client, %list)
{
   //echo("\c5AIClient::setBotFav(" SPC %client.nameBase @", "@ %list SPC ")");
   // Make sure there is a favortite else use a default
   if ( %list $= "" )
      %list = %client.getRandomLoadout();

   %client.loadout[0] = getField( %list, 1 );
   %armor = $NameToData[%client.loadout[0]];
   %weaponCount = 0;
   %specialCount = 0;
   %grenadeCount = 0;
   %count = 1;
   %client.weaponIndex = "";
   %client.specialIndex = "";
   %client.grenadeIndex = "";

   for ( %i = 1; %i < getFieldCount( %list ); %i++ )
   {
      %setItem = false;
      switch$ ( getField(%list, %i-1 ) )
      {
         case Weapon:
            if ( %weaponCount < %armor.maxWeapons )
            {
               if ( !%weaponCount )
                  %client.weaponIndex = %count;
               else
                  %client.weaponIndex = %client.weaponIndex TAB %count;

               %weaponCount++;
               %setItem = true;   
            } 

         case Special:
            if ( %specialCount < %armor.maxSpecials )
            {
               if ( !%specialCount )
                  %client.specialIndex = %count;
               else
                  %client.specialIndex = %client.specialIndex TAB %count;

               %specialCount++;
               %setItem = true;
            }

         case Grenade:
            if ( %grenadeCount < %armor.maxGrenades )
            {
               if ( !%grenadeCount )
                  %client.grenadeIndex = %count;
               else
                  %client.grenadeIndex = %client.grenadeIndex TAB %count;

               %grenadeCount++;
               %setItem = true;
            }
      }

      if ( %setItem )
      {
         %client.loadout[%count] = getField( %list, %i );
         %count++;
      }
   }
   %client.numFavs = %count;
   %client.numFavsCount = 0;
}

function AIClient::ProcessLoadout(%client)
{
   //echo("\c5AIClient::ProcessLoadout(" SPC %client.nameBase SPC ")");
   %player = %client.player;
   if ( !isObject( %player ) )
      return;

   %player.clearInventory();
   %weapCount = 0;

   for ( %i = 0; %i < $SMS::MaxWeapons; %i++ )
   {
      %default = $SMS::Weapon[%i];
      if ( $SMS::ShowInInv[%default] == false )
      {
         %player.incInventory( %default, 1 );
         if ( %default.image.ammo !$= "" )
            %player.incInventory( %default.image.ammo, 999, true );
      }
   }

   for ( %i = 0; %i < getFieldCount( %client.weaponIndex ); %i++ )
   {
      %WInv = $NameToData[%client.loadout[getField( %client.weaponIndex, %i )]];
      if(%WInv !$= "")
      {
         if ( %player.incInventory( %WInv, 1 ) > 0 )
            %weapCount++;

         if ( %WInv.image.ammo !$= "" )
            %player.incInventory( %WInv.image.ammo, 999, true );
      }

      if(%weapCount >= %player.getDatablock().maxWeapons)
         break;
   }
   %player.weaponCount = %weapCount;

   //-----------------------------------------------------------------------------
   // Specials
   %specCount = 0;
   for ( %i = 0; %i < getFieldCount( %client.specialIndex ); %i++ )
   {
      %SInv = $NameToData[%client.loadout[getField( %client.specialIndex, %i )]];
      if(%SInv !$= "")
      {
         //warn("Trying to give player" SPC %SInv SPC "Sarcina");
         // increment special count if current armor can hold this special
         if ( %player.incInventory( %SInv, %player.maxInventory(%SInv) ) > 0 )
            %specCount++;
      }

      if( %specCount >= %player.getDatablock().maxSpecials )
         break;
   }
   %player.specialCount = %specCount;

   //-----------------------------------------------------------------------------
   // Grenades
   %grenCount = 0;
   for ( %i = 0; %i < getFieldCount( %client.grenadeIndex ); %i++ )
   {
      %GInv = $NameToData[%client.loadout[getField( %client.grenadeIndex, %i )]];
      if(%GInv !$= "")
      {
         // increment grenade count if current armor can hold this grenade
         if ( %player.incInventory( %GInv, %player.maxInventory(%GInv) ) > 0 )
         {
            //%player.lastGrenade = %GInv;
            %player.incInventory( %GInv.image.ammo, %player.maxInventory( %GInv.image.ammo ) );
            %grenCount++;
         }
      }

      if( %grenCount >= %player.getDatablock().maxGrenades )
         break;
   }
   %player.grenadeCount = %grenCount;

   %player.setInventory( HealthKit, 1 );
   %player.setInventory( Ryder, 1, 1 );
   %player.setInventory( RyderClip, %player.maxInventory(RyderClip), 1 );
   %player.setInventory( RyderAmmo, %player.maxInventory(RyderAmmo), 1 );
   %player.weaponCount++;
}

//-----------------------------------------------------------------------------
// Game object uses this to start the bots on their merry way
//-----------------------------------------------------------------------------

function AIClient::setUpTasks(%client)
{
   echo("\c5AIClient::setUpTasks(" SPC %client.nameBase SPC ")");

   if ( isEventPending( %this.thinkSchedule ) )
      cancel( %this.thinkSchedule );

   %bot = %client.player;
   if ( !isObject( %bot ) || %bot.getState() $= "Dead" )
      return;

   %client.clearTasks();

   // Wandering deathmatch bot or objective seeking?
   %random = mFloor( getRandom( 0, 2 ) );
   switch( %random )
   {
      case 0:
         %client.taskMode = "Wander";
      //case 1:
      //   %client.taskMode = "Guard";
      //case 1:
      //   %client.taskMode = "Wingman";
      default:
         %client.taskMode = "Wander";
   }

   //      %client.taskMode = "Wander";

   //if ( !%bot.getNavMesh() )
   //   error( "No Nav Mesh found for" SPC %client.nameBase );

   // Run a looping function
   %client.schedule( mFloor( getRandom(0.5, 2) ) * 1000, "think" );
}

// Called from the Game object
function AIClient::onEndGame(%client)
{
   %client.clearTasks();
   %client.missionCycleCleanup();
}

//-----------------------------------------------------------------------------
// AIClient methods 
//-----------------------------------------------------------------------------

function AIClient::clearTasks(%this)
{
   //echo("\c5AIClient::clearTasks(" SPC %this.nameBase SPC ")");
   if ( isEventPending( %this.thinkSchedule ) )
      cancel( %this.thinkSchedule );

   %this.thinkSchedule = "";

   %this.stop();
   //%this.clearAim();

   %player = %this.player;
   if ( isObject( %player ) )
   {
      %player.clearMoveTriggers();
      %player.setMoveSpeed( $Bot::MoveSpeed );
      %player.clearAim();
      %player.stop();
      %player.pathSet = 0;
      %player.path = "";
      %player.fireTrigger = "";
      %player.grenadeTrigger = "";
      %player.firing = 0;
      %player.lastFireTime = 0;
      %player.lastMeleeTime = 0;
      %player.lastGrenadeTime = 0;
      %player.scanCheck = 0;
      %player.gotHealth = 0;
   }

   %this.taskMode = "";
   %this.destination = "";
   %this.stuckTick = 0;
   %this.wingman = 0;
}

//-----------------------------------------------------------------------------

function AIClient::done(%this, %time)
{
   //echo("\c5AIConnection::done(" SPC %this.nameBase SPC %done SPC ")");
}

function AIClient::animate(%this, %seq)
{
   //echo("\c5AIConnection::animate(" SPC %this.nameBase SPC %seq SPC ")");
   %this.player.setActionThread(%seq);
}

//-----------------------------------------------------------------------------
// C++ Callbacks
//-----------------------------------------------------------------------------
/*
function AIClient::onAdd(%client)
{
   // Do nothing
}

function AIClient::onReachDestination(%client)
{
   //echo("\c5AIClient::onReachDestination(" SPC %client.nameBase SPC ")");
   // Do nothing
}

function AIClient::onStuck(%client)
{
   //echo("\c5" @ %client.nameBase @ "::onStuck()");
   // Do Nothing
}

function AIClient::onUnStuck(%client)
{
   //echo("\c5" @ %client.nameBase @ "::onUnStuck()");
}

function AIClient::onMove(%client)
{
   //echo("\c5" @ %client.nameBase @ "::onMove()");
}

function AIClient::onStop(%client)
{
   //echo("\c5" @ %client.nameBase @ "::onStop()");
}

function AIClient::onTargetEnterLOS(%client)
{
   //echo("\c5" @ %client.nameBase @ "::onTargetEnterLOS(" SPC %client.getTargetObject().getClassName() SPC ")" );
}

function AIClient::onTargetExitLOS(%client)
{
   //echo("\c5" @ %client.nameBase @ "::onTargetExitLOS(" SPC %client.getTargetObject().getClassName() SPC ")" );
}
*/
//-----------------------------------------------------------------------------
/*
// This function runs as a continuous loop on the client.
function AIClient::think(%client)
{
   if ( isEventPending( %client.thinkSchedule ) )
      cancel( %client.thinkSchedule );

   %bot = %client.player;

   //if the bot is dead or doesnt exist then exit the scan Loop. But only after the schedule is canceled
   if ( !isObject( %bot ) || %bot.getState() $= "Dead" )
      return( false );

   // No point in going further until the game starts
   if ( !$Game::Running )
   {
      echo("\c5AIClient::SoldierThink(" SPC %client.nameBase SPC " Game is not running )");
      %client.thinkSchedule = %client.schedule( $Bot::ThinkTime, "SoldierThink" );
      return( false );
   }

   // Are we damaged?
   %level = %bot.getDamageLevel();
   if ( %level >= ( %bot.getDataBlock().maxDamage * 0.7 ) && %client.taskMode !$= "Health" )
   {
      // Find a med pack
      %client.prevMode = %client.taskMode;
      %client.taskMode = "Health";
   }
   else
   {
      // Feeling better
      if ( %client.taskMode $= "Health" && %level < %bot.getDataBlock().maxDamage * 0.5 )
      {
         %client.taskMode = %client.prevMode;
      }
   }

   if ( VectorLen( %bot.getVelocity() ) < 0.5 )
   {
      %bot.stuckTick++;
      if ( %bot.stuckTick >= 8 && %bot.pathSet )
      {
         %bot.moveRandom();
         %bot.stuckTick = 0;
         %bot.thinkSchedule = %bot.schedule( ($Bot::ThinkTime * 2), "think" );
         return;
      }
   }

   //echo("AIClient::SoldierThink(" SPC %client.nameBase SPC ")");

   // Decide what to do based on the task at hand
   switch$ ( %client.taskMode )
   {
      case "Wander":
         if ( !%bot.pathSet )
            %bot.doWanderTask();

      //case "Guard":
      //   %bot.doGuardTask();

      case "Wingman":
         if ( !%bot.pathSet )
            %bot.doWingmanTask();

      case "Health":
         %bot.doHealthTask();

      case "Attack":
         %target = %bot.getAimObject();
         if ( %target == -1 || %target.getState() $= "Dead" || %target.isCloaked() )
         {
            %client.setUpTasks();
         }
         else
         {
            //warn( %player.namebase SPC "has target" SPC %player.getAimObject().player.nameBase);
            %targPos = %target.GetBoxCenter();
            %dist = vectorDist( %bot.getPosition(), %targPos );

            if ( %dist >= $Bot::LoseTargetDistance )
            {
               // Target gained too much distance on us, break off
               %client.setUpTasks();
            }
            else
            {
               if ( !%bot.pathSet || %bot.getPathDestination() $= "0 0 0" )
               {
                  %bot.pathSet = 1;
                  %bot.followObject( %target, 2 );
                  %bot.setMoveTrigger(5);
               }

               if ( %dist < $Bot::FireDistance )
               {
                  if ( %bot.getMoveTrigger(5) )
                     %bot.clearMoveTrigger(5);

                  //if ( %dist <= $Bot::MeleeDistance )
                  //{
                  //   %bot.doMelee( %target );
                  //}
                  //else
                  //{
                     //%bot.setAimLocation( %targPos );

                     if ( %bot.getMountedImage( $GrenadeSlot ) != 0 && %bot.hasAmmo( %bot.getMountedImage( $GrenadeSlot ).item.getId() ) )
                     {
                        //%bot.triggerGrenade( $GrenadeSlot );
                     }
                  //}
               }
               else
               {
                  if( %bot.checkInLos(%target, false, true) )
                  {
                     if ( !%bot.firing )
                     {
                        %client.chooseWeapon( %target, %dist );
                        %bot.triggerFire( $WeaponSlot, 1 );
                     }
                  }
               }
            }
         }
   }

   // Loop it!
   %client.thinkSchedule = %client.schedule( $Bot::ThinkTime, "think" );
}

function AIClient::chooseWeapon(%client, %target, %dist)
{
   %bot = %client.player;
   if ( !isObject( %bot ) || %bot.getState() $= "Dead" )
      return;

   // Some environment considerations

   // Lets see what weapons we have to choose from
   %hasPistol = ( %bot.inv["Ryder"] > 0 ) && %bot.hasAmmo( Ryder.getId() );
   %hasRifle = ( %bot.inv["Lurker"] > 0 ) && %bot.hasAmmo( Lurker.getId() ); 
   %hasSniper = ( %bot.inv["SniperRifle"] > 0 ) && %bot.hasAmmo( SniperRifle.getId() );
   %hasThump = ( %bot.inv["GrenadeLauncher"] > 0 ) && %bot.hasAmmo( GrenadeLauncher.getId() );
   %hasShotgun = ( %bot.inv["Shotgun"] > 0 ) && %bot.hasAmmo( Shotgun.getId() );

   %useWeapon = "Ryder";
   if (%dist > 40 && %target.isMounted() > 0 && %hasThump)
      %useWeapon = "GrenadeLauncher";
   //else if (%dist < 10 && %hasPistol)
   //   %useWeapon = "Ryder";
   else if (%dist < 20)
   {
      if ( %hasShotgun )
         %useWeapon = "Shotgun";
      else if ( %hasRifle )
         %useWeapon = "Lurker";
      else if ( %hasPistol )
         %useWeapon = "Ryder";
   }
   else if (%dist < 40)
   {
      if (%hasThump)
         %useWeapon = "GrenadeLauncher";
      else if ( %hasRifle )
         %useWeapon = "Lurker";
      else if ( %hasPistol )
         %useWeapon = "Ryder";
   }
   else if ( %dist > 40 )
   {
      if (%hasSniper)
         %useWeapon = "SniperRifle";
      else if (%hasThump)
         %useWeapon = "GrenadeLauncher";
	  else if ( %hasRifle )
         %useWeapon = "Lurker";
      else if (%hasPistol)
         %useWeapon = "Ryder";
   }

   %bot.use( %useWeapon );
}

//-----------------------------------------------------------------------------

function AIClient::onDamaged(%client, %bot, %delta)
{
   // Try and heal ourselves..
   if ( %bot.getDamageLevel() >= ( %bot.getDataBlock().maxDamage * 0.4 ) )
   {
      if ( %bot.hasInventory( HealthKit ) )
         %bot.use( "HealthKit" );
   }
}

function AIClient::choosePlayerTarget(%client)
{
   echo("AIClient::choosePlayerTarget(" SPC %client.nameBase SPC ")");
   %bot = %client.player;
   if ( !isObject( %bot ) || %bot.getState() $= "Dead" )
      return;

   %target = -1;
   // Loop through the client group and find players to shoot at.
   for(%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %tgt = ClientGroup.getobject(%i).player;
      if ( !isObject(%tgt) || %tgt.isCloaked() || %tgt.getState() $= "Dead" || %tgt.team == %client.team )
         continue;

      %dist = vectorDist(%bot.getPosition(), %tgt.getPosition());
      if ( ( %dist <= $Bot::DetectionDistance && %bot.checkInFoV( %tgt, 65 ) ) || $Bot::MeleeDistance )
      {
         %target = %tgt;
         break;
      }
   }

   if ( %target != -1 )
   {
      %bot.pathSet = 0;
      %bot.setAimObject( %target, "0 0 1.5" );
      %client.taskMode = "Attack";
   }

   return( %target );
}

function AIClient::chooseWingman(%client)
{
   %bot = %client.player;
   if ( !isObject( %bot ) || %bot.getState() $= "Dead" )
      return;

   %wingman = -1;

   // Loop through the client group and find the closest potential wingman
   for ( %i = 0; %i < ClientGroup.getCount(); %i++ )
   {
      %cl = ClientGroup.getobject(%i);
      %potWingman = %cl.player;

      // Weed out a few, dead guys, enemies.. yourself!
      if ( !isObject( %potWingman ) || %potWingman.getState() $= "Dead" || %potWingman.team != %client.team || %potWingman == %bot )
         continue;

      // Don't pair up with a guy who is paired up with a guy
      if ( %cl.taskMode !$="" && %cl.taskMode $= "Wingman" )
         continue;

      // Lets try and narrow it down to the closest one
      %tempDist = vectorDist( %bot.getPosition(), %potWingman.getPosition() );

      if ( %i == 0 ) // First guy we find becomes the base distance
      {
         %reference = %tempDist;
         %wingman = %potWingman;
      }
      else
      {
         if ( %tempDist < %reference ) // If the next guy is closer than the last, he becomes the base distance
         {
            %reference = %tempDist;
            %wingman = %potWingman;
         }
      }
   }

   // Don't want to forget, even if it's NULL
   %client.wingman = %wingman;

   // Hey, we found one
   if ( %wingman != -1 )
   {
      //serverCmdTeamMessageSent( %client, "I'm pairing up with" SPC  %wingman.client.nameBase );
      serverCmdPrivateMessageSent( %client, %wingman.client, "I'll follow you" );
      echo( %client.nameBase SPC "has" SPC %wingman.client.nameBase SPC "assigned as wingman" );
   }

   return( %wingman );
}

//-----------------------------------------------------------------------------
// Helper functions

// Return the angle of a vector in relation to world origin
function AIClient::getAngleofVector(%client, %vec)
{
   %vector = vectorNormalize(%vec);
   %vecx = getWord(%vector, 0);
   %vecy = getWord(%vector, 1);

   if ( %vecx >= 0 && %vecy >= 0 )
      %quad = 1;
   else if ( %vecx >= 0 && %vecy < 0 )
      %quad = 2;
   else if ( %vecx < 0 && %vecy < 0 )
      %quad = 3;
   else
      %quad = 4;

   %angle = mATan(%vecy/%vecx, -1);
   %degangle = mRadToDeg(%angle);
   switch(%quad)
   {
      case 1:
         %angle = %degangle-90;
      case 2:
         %angle = %degangle+270;
      case 3:
         %angle = %degangle+90;
      case 4:
         %angle = %degangle+450;
   }
   if (%angle < 0)  %angle = %angle + 360;
      return %angle;
}

//This is another function taken from code off of garagegames.
//The only mods I made to it was to add the extra check to ensure that the
//angle is within the 0-360 range.
function AIClient::check2DAngletoTarget(%client, %tgt)
{
   %bot = %client.player;
   if ( !isObject( %bot ) || %bot.getState() $= "Dead" )
      return;

   %eyeVec = vectorNormalize(%bot.getEyeVector());
   %eyeangle = %client.getAngleofVector(%eyeVec);
   %posVec = vectorSub(%tgt.GetBoxCenter(), %bot.GetBoxCenter());
   %posangle = %client.getAngleofVector(%posVec);
   %angle = %posangle - %eyeAngle;
   %angle = %angle ? %angle : %angle * -1;

   if (%angle < 0)  %angle = %angle + 360;
      return %angle;
}

//This function checks to see if the target supplied is within the bots FOV
function AIClient::isTargetInView(%client, %tgt, %fov)
{
   %bot = %client.player;
   if ( !isObject( %bot ) || %bot.getState() $= "Dead" )
      return;

   %ang = %client.check2dangletotarget( %tgt );
   %visleft = 360 - (%fov/2);
   %visright = %fov/2;
   if ( %ang > %visleft || %ang < %visright )
      return( true );

   return( false );
}

// Check to see if we hit target with scan instead of checking if we hit obstruction.
// This check prevents shooting teammates in the back
function AIClient::hasLOStoTarget(%client, %target)
{
   %bot = %client.player;
   if ( !isObject( %bot ) || %bot.getState() $= "Dead" || !isObject( %target ) )
      return( false );

   %eyePos = posFromTransform(%bot.getEyeTransform());
   %nEyeVec = vectorNormalize(%bot.getEyeVector());
   %dist = vectorDist(%eyePos, %target.GetBoxCenter());
   %scEyeVec = vectorScale(%nEyeVec, %dist);
   %eyeEnd = vectorAdd(%eyePos, %scEyeVec);

   %mask = ( $TypeMasks::StaticTSObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::ShapeBaseObjectType );

   //%scan = containerRayCast(%eyePos, %eyeEnd, %mask, %bot);
   %scan = containerRayCast(%eyePos, %target.GetBoxCenter(), %mask, %bot);
   %result = firstWord( %scan );

   if ( %result == %target )
      return( true );

   return( false );
}

// Here we NEED to check for obstructions as we are not looking for an object
function AIClient::hasLOStoPosition(%client, %endPos)
{
   %bot = %client.player;
   if ( !isObject( %bot ) || %bot.getState() $= "Dead" )
      return( false );

   %startPos = %bot.posFromTransform( %bot.getEyeTransform() );

   %mask = ( $TypeMasks::StaticObjectType | $TypeMasks::ShapeBaseObjectType | 
             $TypeMasks::StaticShapeObjectType | $TypeMasks::DynamicShapeObjectType );

   %result = containerRayCast( %startPos, %endPos, %mask, %bot );

   if ( !%result )
      return( true );

   return( false );
}

function AIClient::findHealth(%client)
{
   %target = -1;
   InitContainerRadiusSearch( %client.player.GetBoxCenter(), $Bot::DetectionDistance, $TypeMasks::ItemObjectType );
   while ((%tgt = containerSearchNext()) != 0)
   {
      if ( %tgt.getDataBlock().getName() $= "Medpack_medium" )
      {
         //if( %client.isTargetInView( %tgt, "120" ) )
         //{
            %target = %tgt;
            break;
         //}
      }
   }
   return( %target );
}
*/
//-----------------------------------------------------------------------------
// Inventory lists. Choose one at random

function AIClient::getRandomLoadout(%client)
{
   %index = getRandom( 1, $BotInventoryIndex );
   return( $BotInventorySet[%index] );
}

$BotInventoryIndex = 0;
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tLurker rifle\tSpecial\tMunitions\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tLurker rifle\tSpecial\tMedical\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tShotgun\tSpecial\tMunitions\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tShotgun\tSpecial\tMedical\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tGrenade Launcher\tSpecial\tMunitions\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tSniper Rifle\tSpecial\tMunitions\tGrenade\tGrenade";

