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

// Called from server.cs function Torque::buildServer() before loadMission()
function connectBots(%num)
{
   %numTotal = (%num + $Server::PlayerCount + $Server::BotCount);
   //error("connectAiClients(" SPC %num SPC ")");
   if ( %num $= "" || %num < 1 )
      return;

   // Prevent overfilling of the server.
   if ( %numTotal > $pref::Server::MaxPlayers)
      %num = $pref::Server::MaxPlayers - ($Server::PlayerCount + $Server::BotCount);
  
   // This in turn in C++ code calls aiConnect which sets up a new AIConnection
   for ( %i = 1; %i <= %num; %i++ )
   {
      aiAddPlayer( getRandomBotName() );
   }
}

function chooseBotSkin (%client)
{
   // Choose a random, unique skin for this client
   %count = getFieldCount(%availableSkins);
   %client.skin = addTaggedString( getField(%availableSkins, getRandom(%count)) );
}

// This is called from loadMissionStage2()
function AIConnection::onConnect(%client, %name)
{
   //echo("\c5AIClient::onConnect(" SPC %client SPC %name SPC ")");
   %client.isAdmin = false;
   %client.isSuperAdmin = false;
   %client.isConnected = true; //playerCount fix
   // Get the client's unique id:
   %client.guid = $Server::BotCount;//%client.getAddress();
   addToServerGuidList( %client.guid );

   // Save client preferences on the connection object for later use.
   %client.armor = "Soldier";
   //%client.skin = addTaggedString("Base");
   %client.skin = chooseBotSkin();
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
   //echo ( "game Mode = " @ $gameMode );
   switch$ ($gameMode)
   {
   case "DMGame":
      %client.setBotFav( %client.getRandomLoadout() );
   case "BRDMGame":
      %client.setBotFav( %client.getRandomLoadout() );      
   case "TDMGame":
      %client.setBotFav( %client.getRandomLoadout() );
   case "BRTDMGame":
      %client.setBotFav( %client.getRandomLoadout() );
   case "RtFGame":
      %client.setBotFav( %client.getRandomLoadout() );
   case "MfDGame":
      %client.setBotFav( %client.getRandomLoadout() );
   case "PBDMGame":
      %client.setBotFav( %client.getRandomLoadout2() );
   case "BRPBDMGame":
      %client.setBotFav( %client.getRandomLoadout2() );
   case "PBTDMGame":
      %client.setBotFav( %client.getRandomLoadout2() );
   case "BRPBTDMGame":
      %client.setBotFav( %client.getRandomLoadout2() );
   case "PBRtFGame":
      %client.setBotFav( %client.getRandomLoadout2() );
   case "PBMfDGame":
      %client.setBotFav( %client.getRandomLoadout2() );
   default:
      %client.setBotFav( %client.getRandomLoadout() );
   }
   // A bit of random delay for bots to join the game to prevent crashes.
   %randomJoinTime = ( getRandom ( 1000, 20000 ) + ( $pref::Server::warmupTime * 1000) );   
   Game.schedule( %randomJoinTime, "onClientEnterGame", %client );

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
   if(%client.isConnected) //playerCount fix
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
   
   // Now give them the stuff thats not listed in the armory (flagged).
   // These are default weapons. Inventory restrictions will keep it clean
   for ( %i = 0; %i < $SMS::MaxWeapons; %i++ )
   {
      %default = $SMS::Weapon[%i];

      // Filter out what isnt flagged..
      if ( $SMS::ShowInInv[%default] == 0 )
      {
         %player.incInventory( %default, 1 );

         if ( %default.image.clip !$= "" )
            %player.incInventory( %default.image.clip, %player.maxInventory( %default.image.clip ) );

         if ( %default.image.ammo !$= "" )
            %player.incInventory( %default.image.ammo, %player.maxInventory( %default.image.ammo ) );
      }
      // We dont want these counting against maxWeapons or we will hit the ceiling instantly.
   }

   for ( %i = 0; %i < getFieldCount( %client.weaponIndex ); %i++ )
   {
      %WInv = $NameToData[%client.loadout[getField( %client.weaponIndex, %i )]];
      if(%WInv !$= "")
      {
         // increment weapon count if current armor can hold this weapon
         if ( %player.incInventory( %WInv, 1 ) > 0 )
         {
            %weapCount++;

            if ( %WInv.image.clip !$= "" )
               %player.incInventory( %WInv.image.clip, %player.maxInventory( %WInv.image.clip ) );

            if ( %WInv.image.ammo !$= "" )
               %player.incInventory( %WInv.image.ammo, %player.maxInventory( %WInv.image.ammo ) );
         }
      }

      if( %weapCount >= %player.getDatablock().maxWeapons )
         break;
   }

   //echo("Weapon Count:" SPC %weapCount);
   %player.weaponCount = %weapCount;

   // Specials
   //-----------------------------------------------------------------------------
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

   // Grenades
   //-----------------------------------------------------------------------------
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
   
   //echo ("has ai clip? : " @ %obj.getInventory(%obj.getMountedImage($WeaponSlot).clip));
   //echo ("has ai clip? : " @ %player.getInventory(%player.getMountedImage($WeaponSlot).clip));
}

// Called from the Game object
function AIClient::onEndGame(%client)
{
   %client.missionCycleCleanup();
}
