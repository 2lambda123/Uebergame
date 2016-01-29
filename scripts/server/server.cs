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

function Torque::initServer(%this)
{
   echo("\n--------- Initializing " @ $appName @ ": Server Scripts ---------");
   $Server::BotCount = 0;
   $Server::GameType = $appName;
   $Server::TestCheats = 0; // Dev purposes for testing and editing

   // Server::Status is returned in the Game Info Query and represents the
   // current status of the server. This string sould be very short.
   $Server::Status = "Unknown";

   // Turn on testing/debug script functions
   $Server::TestCheats = false;

   // Specify where the mission files are.
   $Server::MissionFileSpec = "levels/*.mis";

   // Base server functionality
   if (isFile( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/banlist.cs" ) )
   exec( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/banlist.cs" );

   exec("./audio.cs");
   exec("./message.cs");
   exec("./commands.cs");
   exec("./missionInfo.cs");
   exec("./missionLoad.cs");
   exec("./missionDownload.cs");
   exec("./gameConnection.cs");
   exec("./admin.cs");
   exec("./kickban.cs");
   exec("./camera.cs");
   exec("./centerPrint.cs");
   exec("./library/library.cs");
}

//-----------------------------------------------------------------------------

function Torque::initDedicated(%this)
{
   echo("\n--------- Starting Dedicated Server ---------");

   echo("$levelToLoad:" SPC $levelToLoad SPC "$missionTypeArg" SPC $missionTypeArg);

   // Make sure this variable reflects the correct state.
   $Server::Dedicated = true;

   // The server isn't started unless a mission and type has been specified.
   if ( $levelToLoad $= "" || $missionTypeArg $= "" )
   {
      error( "No level or game type specified. Creation of server failed." );
      return false;
   }

   %level = "";
   for( %file = findFirstFile( $Server::MissionFileSpec ); %file !$= ""; %file = findNextFile( $Server::MissionFileSpec ) )
   {
      if ( fileName( %file ) $= $levelToLoad )
      {
         %level = %file;
         break;
      }
   }

   if ( !isFile( %level ) )
      return false;

   tge.createServer( "MultiPlayer", %level, $missionTypeArg );
}

/// Attempt to find an open port to initialize the server with
function portInit(%port)
{
   %failCount = 0;
   while(%failCount < 10 && !setNetPort(%port))
   {
      echo("Port init failed on port " @ %port @ " trying next port.");
      %port++; %failCount++;
   }
}

/// Create a server of the given type, load the given level, and then
/// create a local client connection to the server.
//
/// @return true if successful.
function createAndConnectToLocalServer( %serverType, %level, %missionType )
{
   if( !tge.createServer( %serverType, %level, %missionType ) )
      return false;
   
   %conn = new GameConnection( ServerConnection );
   RootGroup.add( ServerConnection );

   %conn.setConnectArgs( getField($pref::Player, 0), getField($pref::Player, 1));
   %conn.setJoinPassword( $Client::Password );
   
   %result = %conn.connectLocal();
   if( %result !$= "" )
   {
      %conn.delete();
      tge.destroyServer();
      
      return false;
   }

   return true;
}

/// Create a server with either a "SinglePlayer" or "MultiPlayer" type
/// Specify the level to load on the server
function Torque::createServer(%this, %serverType, %level, %missionType)
{
   // Increase the server session number.  This is used to make sure we're
   // working with the server session we think we are.
   $Server::Session++;

   if (%level $= "")
   {
      error("createServer(): level name unspecified");
      return false;
   }

   // Make sure our level name is relative so that it can send
   // across the network correctly
   //%relPath = makeRelativePath(%level, getWorkingDirectory());
   //echo( "c4\Relative Path:" SPC %relPath );

   tge.destroyServer();

   $missionSequence = 0;
   $Server::PlayerCount = 0;
   $Server::ServerType = %serverType;
   // Server::GameType is sent to the master server.

   // Server::MissionType sent to the master server.  Clients can
   // filter servers based on mission type.
   $Server::MissionType = %missionType;
   $Server::LoadFailMsg = "";

   $Physics::isSinglePlayer = true;
   
   // Setup for multi-player, the network must have been
   // initialized before now.
   if (%serverType $= "MultiPlayer")
   {
      $Physics::isSinglePlayer = false;
            
      echo("Starting multiplayer mode");

      // Make sure the network port is set to the correct pref.
      portInit($Pref::Server::Port);
      allowConnections(true);

      if ($pref::Net::DisplayOnMaster !$= "Never" )
         schedule(0,0,startHeartbeat);
   }

   // Create the ServerGroup that will persist for the lifetime of the server.
   $ServerGroup = new SimGroup(ServerGroup);

   // Let the game initialize some things now that the the server has been created

   // GameStartTime is the sim time the game started. Used to calculate game elapsed time.
   $Game::StartTime = 0;
   
   exec("./antispam.cs");
   exec("./damageTypes.cs");
   exec("./shapeBase.cs");
   exec("./cameraData.cs");
   
   // Create the server physics world.
   physicsInitWorld( "server" );
   
   // Load SMS Utilities  -Yardik-
   echo("<>>>> LOADING SMS <<<<>");
   exec("./sms.cs");

   // Exec server scripts
   
   exec("./camera.cs");
   exec("./triggers.cs");
   exec("./shapeBase.cs");
   exec("./items/item.cs");
   exec("./items/health.cs");
   exec("./weapons/projectile.cs");
   exec("./radiusDamage.cs");
   exec("./teleporter.cs");
   exec("./weapons/weapon.cs");
   exec("./weapons/proximityMine.cs");
   exec("./player.cs");
   exec("./aiClient.cs");
   exec("./aiPlayer.cs");
   exec("./vehicles/vehicle.cs");
   exec("./vehicles/vehicleWheeled.cs");
   exec("./vehicles/cheetah.cs");
   exec("./weapons/turret.cs");
   
   
   // Exec Datablocks
   exec("./cameraData.cs");
   exec("./markers.cs");
   exec("./defaultparticle.cs");
   
   exec("./lights.cs");
   exec("./particles.cs");
   exec("./environment.cs");
   exec("./rigidShape.cs");
   exec("./items/healthData.cs");
   exec("./weapons/sfx/weaponfx.cs");
   exec("./weapons/sfx/grenadefx.cs");
   exec("./weapons/sfx/weapons/rocketfx.cs");
   exec("./weapons/sfx/weapons/bulletImpact.cs");
   exec("./weapons/default/Lurker.cs");
   exec("./weapons/default/Ryder.cs");
   exec("./weapons/default/ProxMine.cs");
   exec("./weapons/default/Turret.cs");
   exec("./weapons/paintball/PaintballMarkerBlue.cs");
   exec("./weapons/paintball/PaintballMarkerRed.cs");
   exec("./weapons/paintball/PaintballMarkerGreen.cs");
   exec("./weapons/paintball/PaintballMarkerYellow.cs");
   exec("./playerData.cs");
   exec("./aiPlayer.cs");
   exec("./vehicles/defaultCar.cs");
   exec("./vehicles/cheetahCar.cs");

   %datablockFiles = new ArrayObject();
   %datablockFiles.add( "art/particles/managedParticleData.cs" );
   %datablockFiles.add( "art/particles/managedParticleEmitterData.cs" );
   %datablockFiles.add( "art/decals/managedDecalData.cs" );
   %datablockFiles.add( "scripts/server/managedDatablocks.cs" );
   %datablockFiles.add( "art/forest/managedItemData.cs" );   
   loadDatablockFiles( %datablockFiles, true );

   // Init the SMS functions -Yardik-
   SmsInv.SetupMaxWeapons();
   SmsInv.SetupMaxClips();
   SmsInv.SetupMaxAmmos();
   SmsInv.SetupMaxItems();
   SmsInv.SetupMaxGrenades();
   SmsInv.SetupMaxMines();

   exec("./inventory.cs");
   // Game files
   exec("./gameTypes/CoreGame.cs"); // Parent to all, want this loaded first

   %search = "./gameTypes/*Game.cs";
   for(%file = findFirstFile(%search); %file !$= ""; %file = findNextFile(%search))
   {
     %type = fileBase(%file);
     if(%type !$= CoreGame)
        exec("./gameTypes/" @ %type @ ".cs");
   }

   // Mission scripting support. Auto-execute files - Temporary until I figure out how to just exe the mapscript in the proper dir itself
   %path = "levels/*.cs";
   for( %file = findFirstFile( %path ); %file !$= ""; %file = findNextFile( %path ) )
   {
       if( fileBase(%file) $= fileBase(%level) )
          exec( %file );
   }

   // Load the level
   %this.loadMission(%level, %missionType, true);
   
   return true;
}

/// Shut down the server
function Torque::destroyServer(%this)
{
   $Server::ServerType = "";
   allowConnections(false);
   stopHeartbeat();
   $missionRunning = false;
   $Game::Running = false;

   // Destroy the server physcis world
   physicsDestroyWorld( "server" );

   // Delete all the server objects
   if (isObject(MissionGroup))
      MissionGroup.delete();
   if (isObject(MissionCleanup))
      MissionCleanup.delete();

   if(isObject(Game)) // Clean up the Game object
   {
      Game.deactivatePackages();
      Game.delete();
   }

   // Delete all the server objects
   if (isObject($ServerGroup))
      $ServerGroup.delete();

   // Delete all the connections:
   // Something is wrong with aiClient that causes an infinite loop when you try to delete
   //while ( ClientGroup.getCount() )
   //{
   //   %client = ClientGroup.getObject(0);
   //   %client.delete();
   //}
   for (%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      ClientGroup.getObject(%i).delete();
   }

   $Server::GuidList = "";

   // Delete all the data blocks...
   deleteDataBlocks();
   
   // Save any server settings
   echo( "Exporting server prefs..." );
   export("$Pref::Server::*", GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/server.config.cs", False);

   // Increase the server session number.  This is used to make sure we're
   // working with the server session we think we are.
   $Server::Session++;
}

/// Reset the server's default prefs
function resetServerDefaults()
{
   echo( "Resetting server defaults..." );
   $resettingServer = true;
   if(isObject(Game))
      Game.endGame();

   exec( "./defaults.cs" );
   exec( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/server.config.cs" );

   allowConnections(true); // ZOD: Open up the server for connections again.

   tge.loadMission( $pref::Server::MissionFile, $pref::Server::MissionType, false );
   $resettingServer = false;
   echo( "Server reset complete." );
}

/// Guid list maintenance functions
function addToServerGuidList( %guid )
{
   %count = getFieldCount( $Server::GuidList );
   for ( %i = 0; %i < %count; %i++ )
   {
      if ( getField( $Server::GuidList, %i ) == %guid )
         return;
   }
   $Server::GuidList = $Server::GuidList $= "" ? %guid : $Server::GuidList TAB %guid;
}

function removeFromServerGuidList( %guid )
{
   %count = getFieldCount( $Server::GuidList );
   for ( %i = 0; %i < %count; %i++ )
   {
      if ( getField( $Server::GuidList, %i ) == %guid )
      {
         $Server::GuidList = removeField( $Server::GuidList, %i );
         return;
      }
   }

   // Huh, didn't find it.
}

/// When the server is queried for information, the value of this function is
/// returned as the status field of the query packet.  This information is
/// accessible as the ServerInfo::State variable.
function onServerInfoQuery()
{
   return "Doing Ok";
}

function listClients()
{
   for(%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %cl = ClientGroup.getObject(%i);
      %type = "";
      if(%cl.isAiControlled())
         %type = "Bot ";
      if(%cl.isAdmin)
         %type = %status @ "Admin ";
      if(%cl.isSuperAdmin)
         %type = %status @ "SuperAdmin ";
      if(%type $= "")
         %type = "<normal>";

      echo("client: " @ %cl @ " player: " @ %cl.player @ " name: " @ %cl.nameBase @ " team: " @ %cl.team @ " status: " @ %status);
   }
}
