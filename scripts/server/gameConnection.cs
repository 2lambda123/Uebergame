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


//-----------------------------------------------------------------------------
// This script function is called before a client connection
// is accepted.  Returning "" will accept the connection,
// anything else will be sent back as an error to the client.
// All the connect args are passed also to onConnectRequest
//
function GameConnection::onConnectRequest( %client, %netAddress, %name, %skin )
{
   echo("Connect request from: " @ %netAddress);

   %guid = %client.getCleanIP();
   if ( %client.isOnServerBanList( %guid ) )
   {
      return "CR_YOUAREBANNED";
   }

   if(($Server::PlayerCount + $Server::BotCount) >= $pref::Server::MaxPlayers)
      return "CR_SERVERFULL";

   return "";
}

//-----------------------------------------------------------------------------
// This script function is the first called on a client accept
//
function GameConnection::onConnect( %client, %name, %skin )
{
   //LogEcho("GameConnection::onConnect(" SPC %client @", "@ %name @", "@ %skin SPC ")");
   // Send down the connection error info, the client is
   // responsible for displaying this message if a connection
   // error occures.
   messageClient(%client,'MsgConnectionError',"",$pref::Server::ConnectionError);

   // Send mission information to the client
   sendLoadInfoToClient( %client );

   // Simulated client lag for testing...
   // %client.setSimulatedNetParams(0.1, 30);

   // Get the client's unique id:
   // %authInfo = %client.getAuthInfo();
   // %client.guid = getField( %authInfo, 3 );
   %client.guid = %client.getCleanIP();
   addToServerGuidList( %client.guid );
   
   // Set admin status
   if (%client.getAddress() $= "local")
   {
      %client.isAdmin = true;
      %client.isSuperAdmin = true;
   }
   else
   {
      if ( isOnSuperAdminList( %client ) )
      {
         %client.isAdmin = true;
         %client.isSuperAdmin = true;   
      }
      else if ( isOnAdminList( %client ) )
      {
         %client.isAdmin = true;
      }
      else
      {
         %client.isAdmin = false;
         %client.isSuperAdmin = false;
      }
   }

   // Save client preferences on the connection object for later use.
   //%client.gender = "Male";
   //%client.race = "Human";
   %client.armor = "Soldier";
   %client.skin = addTaggedString( %skin );
   %client.setPlayerName(%name);
   %client.ready = false;
   %client.team = 0;
   %client.lastTeam = 0;
   %client.justConnected = true; // Set the flag they just joined, put in spec.
   %client.isWaiting = false; // Team change wait flag
   %client.isReady = false; // For tournament mode play
   %client.SadAttempts = 0; // Start off with 0 SAD access attempts.

   // Setup for voice
   %client.voiceTag = "default";
   %client.voicePitch = 1;

   // 
   echo("CADD: " @ %client @ " " @ %client.getAddress());

  // Inform the client of all the other clients
   %count = ClientGroup.getCount();
   for (%cl = 0; %cl < %count; %cl++)
   {
      %other = ClientGroup.getObject(%cl);
      if ((%other != %client))
      {
         // These should be "silent" versions of these messages...
         messageClient(%client, 'MsgClientJoin', "",
               %other.playerName,
               %other,
               %other.guid,
               %other.isAIControlled(),
               %other.isAdmin,
               %other.isSuperAdmin,
               %other.team,
			   0,
               %other.getPing(),
               %other.getPacketLoss());
      }
   }

   // Inform the client we've joined up
   messageClient(%client,
      'MsgClientJoin', '\c1Welcome to Uebergame, %1.',
      %client.playerName,
      %client,
      %client.guid,
	  0,
      //%client.isAiControlled(),
      %client.isAdmin,
      %client.isSuperAdmin,
      %client.team,
	  0,
      %client.getPing(),
      %client.getPacketLoss());

   // Inform all the other clients of the new guy
   messageAllExcept(%client, -1, 'MsgClientJoin', '\c1%1 has joined the server.',
      %client.playerName,
      %client,
      %client.guid,
	  0,
      //%client.isAiControlled(),
      %client.isAdmin,
      %client.isSuperAdmin,
      %client.team,
	  0,
      %client.getPing(),
      %client.getPacketLoss());

   // ZOD: Send this player their client ID etc.
   messageClient(%client, 'MsgYourInfo', "", %client.nameBase, %client);

   // Setup the default player class
   SmsInv.setDefaultInventory(%client);

   %client.SadAttempts = 0; // Start off with 0 SAD access attempts.

   // If the mission is running, go ahead download it to the client
   if ($missionRunning)
   {
      %client.loadMission();
   }
   else if ($Server::LoadFailMsg !$= "")
   {
      messageClient(%client, 'MsgLoadFailed', $Server::LoadFailMsg);
   }
   $Server::PlayerCount++;

   if( $pref::Server::ConnectLog && $Server::Dedicated )
   {
      writeConnectionLog();
   }
}

function writeConnectionLog()
{
   %file = $HomePath @"/"@ "connections.csv";
   %conn = new FileObject();
   if ( %conn.openForAppend( %file ) )
   {
      %conn.writeLine("\"" @ %client.nameBase @ "," @ %client.getAddress() );
   }
   %conn.close();
   %conn.delete();
   echo( "exporting client info to connection.csv..." );
}

function readConnectionLog()
{
   %file = $HomePath @"/"@ "connections.csv";
   %conn = new FileObject();
   if( %conn.openForRead( %file ) )
   {	
      while( !%conn.isEOF() )
      {
         %line = %conn.readLine();
         echo(%line);
      }	
      %conn.close();
   }
   else
   {
      error("Failed to read connection log file. Does it exist?");  
   }    

   %conn.delete();
}

function GameConnection::getCleanIP(%client)
{
   return 0;

   // Get the client's unique transport address
   %ipString = %client.getAddress();

   // Find the char count of the address
   %count = strlen(%ipString);

   // Strip the "IP:" string from the address, first 3 char
   %address = getSubStr(%ipString, 3, %count);

   // Search the address for the port start ":"
   %port = strchr(%address, ":");

   // Rip the port out of the address and we are left with a clean ip
   %ip = getSubStr(%address, 0, %port);
   return %ip;
}

//-----------------------------------------------------------------------------
// A player's name could be obtained from the auth server, but for
// now we use the one passed from the client.
// %realName = getField( %authInfo, 0 );
//
function GameConnection::setPlayerName(%client, %name)
{
   //LogEcho("GameConnection::setPlayerName(" SPC %client @", "@ %name SPC ")");
   // Minimum length requirements
   %name = trim( strToPlayerName( %name ) );

   // Zod: Make use of the bad word filter ;)
   if ( $pref::enableBadWordFilter )
   {
      if ( containsBadWords( %name ) )
         %name = "Potty Mouth";
   }

   if ( strlen( %name ) < 3 )
      %name = "Poser";

   //LogEcho("After length checks and bad word check, name is:" SPC %name);

   // Make sure the alias is unique, we'll hit something eventually
   if (!isNameUnique(%name))
   {
      %isUnique = false;
      for (%suffix = 1; !%isUnique; %suffix++)
      {
         %nameTry = %name @ "." @ %suffix; 
         %isUnique = isNameUnique(%nameTry);
      }
      %client.nameBase = %nameTry;
      //%client.playerName = addTaggedString(%nameTry);
      %client.playerName = addTaggedString("\cp\c8" @ %nameTry @ "\co");
      MessageAll( 'MsgClientNameChanged', '\c2The smurf \"%1\" is now called \"%2\".', %name, %nameTry, %client );
   }
   else
   {
      // Tag the name with the "smurf" color:
      %client.nameBase = %name;
      //%client.playerName = addTaggedString(%name);
      %client.playerName = addTaggedString("\cp\c8" @ %name @ "\co");
      //LogEcho("After Unique check name is:" SPC StripMLControlChars( detag( getTaggedString( %client.playerName ) ) ) );
   }
}

function isNameUnique(%name)
{
   %count = ClientGroup.getCount();
   for ( %i = 0; %i < %count; %i++ )
   {
      %test = ClientGroup.getObject( %i );
      //%rawName = stripChars( detag( getTaggedString( %test.playerName ) ), "\cp\co\c6\c7\c8\c9" );
      %rawName = StripMLControlChars( detag( getTaggedString( %test.playerName ) ) );
      if ( strcmp( %name, %rawName ) == 0 )
         return false;
   }
   return true;
}

//-----------------------------------------------------------------------------
// This function is called when a client drops for any reason
//
function GameConnection::onDrop(%client, %reason)
{
   if(isObject(Game))
      Game.onClientLeaveGame(%client);
   
   removeFromServerGuidList( %client.guid );
   messageAllExcept(%client, -1, 'MsgClientDrop', '\c1%1 has left the game.', %client.playerName, %client);

   removeTaggedString(%client.playerName);
   removeTaggedString(%client.skin);

   echo("CDROP: " @ %client @ " " @ %client.getAddress());
   $Server::PlayerCount--;

   // Reset the server if everyone has left the game
   if( $Server::PlayerCount == 0 && $Server::Dedicated && !$resettingServer && !$LoadingMission )
      schedule(10, 0, "resetServerDefaults");
}

//-----------------------------------------------------------------------------
// Mission Loading
// The server portion of the client/server mission loading process
//-----------------------------------------------------------------------------

//--------------------------------------------------------------------------
// Loading Phases:
// Phase 1: Transmit Datablocks
//          Transmit targets
// Phase 2: Transmit Ghost Objects
// Phase 3: Start Game
//
// The server invokes the client MissionStartPhase[1-3] function to request
// permission to start each phase.  When a client is ready for a phase,
// it responds with MissionStartPhase[1-3]Ack.

function GameConnection::loadMission(%client)
{
   // Send over the information that will display the server info
   // when we learn it got there, we'll send the data blocks
   %client.currentPhase = 0;
   commandToClient(%client, 'MissionStartPhase1', $missionSequence, $Server::MissionFile, MissionGroup.musicTrack);
   echo("<>>>> Sending mission load to client: " @ $Server::MissionFile @ "  <<<<>");
}

function GameConnection::onDataBlocksDone( %client, %missionSequence )
{
   // Make sure to ignore calls from a previous mission load
   if (%missionSequence != $missionSequence)
      return;
   if (%client.currentPhase != 1)
      return;
   %client.currentPhase = 1.5;

   // On to the next phase
   commandToClient(%client, 'MissionStartPhase2', $missionSequence, $Server::MissionFile);
}

function GameConnection::clientWantsGhostAlwaysRetry(%client)
{
   if($MissionRunning)
      %client.activateGhosting();
}

function GameConnection::onGhostAlwaysFailed(%client)
{
   // Unused, console spam placeholder
}

function GameConnection::onGhostAlwaysObjectsReceived(%client)
{
   // Ready for next phase.
   commandToClient(%client, 'MissionStartPhase3', $missionSequence, $Server::MissionFile);

   // Send the list of available vehicles
   %client.SendVehicleList();
}

function GameConnection::startMission(%client)
{
   // Inform the client the mission starting
   commandToClient(%client, 'MissionStart', $missionSequence);
}

function GameConnection::endMission(%client)
{
   // Inform the client the mission is done.  Note that if this is
   // called as part of the server destruction routine, the client will
   // actually never see this comment since the client connection will
   // be destroyed before another round of command processing occurs.
   // In this case, the client will only see the disconnect from the server
   // and should manually trigger a mission cleanup.
   commandToClient(%client, 'MissionEnd', $missionSequence);
}

// This just keeps the player from changing teams too fast which could crash the server.
function GameConnection::waitTimeout(%client)
{
   %client.isWaiting = false;
}

// Sends the list of vehicles on the server for the vehicle hud
function GameConnection::SendVehicleList(%client)
{
   //error("GameConnection::SendVehicleList(" SPC %client.nameBase SPC ")");
   for ( %i = 0; %i < $SMS::MaxVehicles; %i++ )
   {
      if ( !$GameBanList[$VehicleToName[$InvVehicle[%i]]] )
      {
         %list = %list $="" ? $VehicleToName[$InvVehicle[%i]] : %list TAB $VehicleToName[$InvVehicle[%i]];
         %bitmap = %bitmap $="" ? fileBase( $InvVehicle[%i].shapeFile ) : %bitmap TAB fileBase( $InvVehicle[%i].shapeFile );
      }
   }

   //error("Server Vehicle List:" SPC %list);
   //error("Server Bitmap List:" SPC %bitmap);
   commandToClient(%client, 'GetVBitmapList', addTaggedString(%bitmap));
   commandToClient(%client, 'GetVehicleList', addTaggedString(%list));
}

//--------------------------------------------------------------------------
// Sync the clock on the client.

function GameConnection::syncClock(%client, %time)
{
   commandToClient(%client, 'syncClock', %time);
}

