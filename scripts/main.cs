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

function Torque::getHomePath(%this)
{
   echo("no config in game directory - Set directory to users");
   %temp = getUserHomeDirectory();
   echo(%temp);
   if( !isDirectory( %temp ) )
   {
      echo("c2cannot find home user document folder");
      %temp = getUserDataDirectory();
      echo(%temp);
      if( !isDirectory( %temp ) )
      {
         echo("c2cannot find home user appdata roaming folder");
         $HomePath = "configs";
      }
      else
      {
         //put it in appdata/roaming
         $HomePath = %temp @ "/" @ $appName;
      }
   }
   else
   {
      //put it in user/documents
      $HomePath = %temp @ "/My Games/" @ $appName;
   }
   return( $HomePath );
}

/// Used to get a random main menu level
function getMainMenuLevel(%levelName)
{
   %list = new ArrayObject();
   %list.add("0", "levels/Ambience/ambience_space_ocean_01.mis");
   %list.add("1", "levels/Ambience/ambience_space_ocean_02.mis");
   %list.add("2", "levels/Ambience/ambience_space_ocean_03.mis");
   %list.add("3", "levels/Ambience/ambience_pazifik_island_01_dawn.mis");
   %list.add("4", "levels/Ambience/ambience_pazifik_island_01_day.mis");
   %list.add("5", "levels/Ambience/ambience_pazifik_island_01_night.mis");
   %list.add("6", "levels/Ambience/ambience_pazifik_island_01_rain.mis");
   
   %random = getRandom(0, 6);
   %levelName = %list.getValue(%random);
   
   return (%levelName);
}

/// Used to get a random 2D background image if 3D background is disabled
function getRandomBackground(%this)
{
   %list = new ArrayObject();
   %list.add("0", "art/gui/backgrounds/deep_sky_01.jpg");
   %list.add("1", "art/gui/backgrounds/deep_space_01.jpg");
   %list.add("2", "art/gui/backgrounds/eye_nebula_01.jpg");
   %list.add("3", "art/gui/backgrounds/ez_space_lite_01.jpg");
   %list.add("4", "art/gui/backgrounds/meyespace_v1_01.jpg");
   %list.add("5", "art/gui/backgrounds/milky_way_01.jpg");
   %list.add("6", "art/gui/backgrounds/space_nebula_01.jpg");
   %list.add("7", "art/gui/backgrounds/spacegen_01.jpg");
   
   %random = getRandom(0, 7);
   %backgroundName = %list.getValue(%random);
   
   return (%backgroundName);
}

//---------------------------------------------------------------------------------------------
// CorePackage
// Adds functionality for this mod to some standard functions.
//---------------------------------------------------------------------------------------------
package scripts
{
//---------------------------------------------------------------------------------------------
// onStart
// Called when the engine is starting up. Initializes this mod.
//---------------------------------------------------------------------------------------------
function Torque::onStart(%this)
{
   echo("\n--------- Starting "@ $appName @" ---------");

   Parent::onStart(%this);

   $GameLaunched = 0;
   // Set our saves directory
   %this.getHomePath();
   
   // Load up default settings
   exec( "./gui/profiles.cs" );
   exec( "./client/defaults.cs" );
   exec( "./server/defaults.cs" );
   
   //config update function
   exec( "./client/updatePrefs.cs" );
   
   // Load up our user saved settings, if existing
   if (isFile($HomePath @ "/config.cs"))
      exec( $HomePath @ "/config.cs" );

   $ScriptGroup = new SimGroup(ScriptClassGroup);

   // Build the mission listing for server and client
   echo("----- Adding missions to list -----");
   %this.buildMissionList();

   // Initialise stuff.
   exec("./client/core.cs");
   %this.initializeCore();

   exec("./client/init.cs");

   // Init the physics plugin.
   physicsInit();
      
   // Start up the audio system.
   sfxStartup();

   exec("./client/client.cs");
   exec("./server/server.cs");

   exec("./gui/guiTreeViewCtrl.cs");
   exec("./gui/messageBoxes/messageBox.ed.cs");
   
   // Server gets loaded for all sessions, since clients
   // can host in-game servers.
   %this.initServer();

   // Start up in either client, or dedicated server mode
   if ( $isDedicated || $pref::Server::Dedicated )
   {
      %this.initDedicated();
   }
   else
      
      %this.initClient();
}

//---------------------------------------------------------------------------------------------
// onExit
// Called when the engine is shutting down. Shutdowns this mod.
//---------------------------------------------------------------------------------------------
function onExit()
{   
   // Stop file change events.
   stopFileChangeNotifications();
   
   sfxShutdown();

   // Ensure that we are disconnected and/or the server is destroyed.
   // This prevents crashes due to the SceneGraph being deleted before
   // the objects it contains.
   if ($Server::Dedicated)
      tge.destroyServer();
   else
      disconnect();
   
   // Destroy the physics plugin.
   physicsDestroy();

   echo("Exporting client control configuration");
   if (isObject(moveMap))
   {
      moveMap.save($HomePath @ "/bindings.cs", false);

      // Append the other action maps:
      spectatorMap.save($HomePath @ "/bindings.cs", true);
      vehicleMap.save($HomePath @ "/bindings.cs", true);
   }

   echo("Exporting configs");
   export("$pref::*", $HomePath @ "/config.cs", false);
   BanList::Export($HomePath @ "/banlist.cs");

   Parent::onExit();
}

//---------------------------------------------------------------------------------------------
// displayHelp
// Prints the command line options available for this mod.
//---------------------------------------------------------------------------------------------
function Torque::displayHelp(%this)
{
   // Let the parent do its stuff.
   Parent::displayHelp();

   error("Command line options:\n" @
         "  -fullscreen            Starts game in full screen mode\n" @
         "  -windowed              Starts game in windowed mode\n" @
         "  -autoVideo             Auto detect video, but prefers OpenGL\n" @
         "  -openGL                Force OpenGL acceleration\n" @
         "  -directX               Force DirectX acceleration\n" @
         "  -voodoo2               Force Voodoo2 acceleration\n" @
         "  -prefs <configFile>    Exec the config file\n" @
         "  -dedicated             Start as dedicated server\n"@
         "  -connect <address>     For non-dedicated: Connect to a game at <address>\n" @
         "  -level <filename>      For dedicated: Load the mission\n" @
         "  -type <game_type>      For dedicated: Use the specified gametype\n"
	);
}

//---------------------------------------------------------------------------------------------
// parseArgs
// Parses the command line arguments and processes those valid for this mod.
//---------------------------------------------------------------------------------------------
function Torque::parseArgs(%this)
{
   // Let the parent grab the arguments it wants first.
   Parent::parseArgs(%this);

   // Loop through the arguments.
   for (%i = 1; %i < $Game::argc; %i++)
   {
      %arg = $Game::argv[%i];
      %nextArg = $Game::argv[%i+1];
      %hasNextArg = $Game::argc - %i > 1;
   
      switch$ (%arg)
      {
         case "-fullscreen":
            $cliFullscreen = true;
            $argUsed[%i]++;

         case "-windowed":
            $cliFullscreen = false;
            $argUsed[%i]++;

         case "-openGL":
            $pref::Video::displayDevice = "OpenGL";
            $argUsed[%i]++;

         case "-directX":
            $pref::Video::displayDevice = "D3D";
            $argUsed[%i]++;

         case "-voodoo2":
            $pref::Video::displayDevice = "Voodoo2";
            $argUsed[%i]++;

         case "-autoVideo":
            $pref::Video::displayDevice = "";
            $argUsed[%i]++;

         case "-prefs":
            $argUsed[%i]++;
            if (%hasNextArg) {
               exec(%nextArg, true, true);
               $argUsed[%i+1]++;
               %i++;
            }
            else
               error("Error: Missing Command Line argument. Usage: -prefs <path/script.cs>");

         case "-dedicated":
            $pref::Server::Dedicated = true;
            enableWinConsole(true);
            $argUsed[%i]++;

         case "-connect":
            $argUsed[%i]++;
            if (%hasNextArg) {
               $JoinGameAddress = $nextArg;
               $argUsed[%i+1]++;
               %i++;
            }
            else
               error("Error: Missing Command Line argument. Usage: -connect <ip_address>");

         case "-level":
            $argUsed[%i]++;
            if (%hasNextArg)
            {
               %hasExt = strpos(%nextArg, ".mis");
               if(%hasExt == -1)
               {
                  $levelToLoad = %nextArg @ " ";
                  
                  for(%j = %i + 2; %j < $Game::argc; %j++)
                  {
                     %arg = $Game::argv[%j];
                     %hasExt = strpos(%arg, ".mis");
                     
                     if(%hasExt == -1)
                     {
                        $levelToLoad = $levelToLoad @ %arg @ " ";
                     }
                     else
                     {
                        $levelToLoad = $levelToLoad @ %arg;
                        break;
                     }
                  }
               }
               else
               {
                  $levelToLoad = %nextArg;
               }
               $argUsed[%i+1]++;
               %i++;
            }
            else
               error("Error: Missing Command Line argument. Usage: -level <level file name (no path), with or without extension>");

         case "-type":
            $argUsed[%i]++;
            if (%hasNextArg)
            {
               $missionTypeArg = %nextArg;
               $argUsed[%i+1]++;
               %i++;
            }
            else
               error("Error: Missing Command Line argument. Usage: -type <game_type>");
      }
   }
}
};

function LogEcho(%string)
{
   if( $pref::LogEchoEnabled )
   {
/*
      %file = $HomePath @"/"@ "echos.txt";
      %log = new FileObject();
      %log.openForAppend(%file);
      %log.writeLine("\"" @ %string );
      %log.close();
      %log.delete();
*/
      echo(%string);
   }
}

//-----------------------------------------------------------------------------
// Mission List Creation
//-----------------------------------------------------------------------------

// This can be easily overloaded for more types.
function Torque::getMissionTypeDisplayNames(%this)
{
   //maybe this can be done better though a switch case statement or so
   for ( %type = 0; %type < $HostTypeCount; %type++ )
   {
      if ( $HostTypeName[%type] $= DM )
         $HostTypeDisplayName[%type] = "Deathmatch";
      else if ( $HostTypeName[%type] $= TDM )
         $HostTypeDisplayName[%type] = "Team Deathmatch";
      else if ( $HostTypeName[%type] $= RtF )
         $HostTypeDisplayName[%type] = "Retrieve the Flag";
      else if ( $HostTypeName[%type] $= MfD )
         $HostTypeDisplayName[%type] = "Marked For Death";
	   else if ( $HostTypeName[%type] $= PBDM )
         $HostTypeDisplayName[%type] = "Paintball Deathmatch";
	   else if ( $HostTypeName[%type] $= PBTDM )
         $HostTypeDisplayName[%type] = "Paintball Team Deathmatch";
      else if ( $HostTypeName[%type] $= PBRtF )
         $HostTypeDisplayName[%type] = "Paintball Retrieve the Flag";
      else if ( $HostTypeName[%type] $= PBMfD )
         $HostTypeDisplayName[%type] = "Paintball Marked For Death";
      else if ( $HostTypeName[%type] $= BRDM )
         $HostTypeDisplayName[%type] = "Battle Royale Deathmatch";
      else if ( $HostTypeName[%type] $= BRTDM )
         $HostTypeDisplayName[%type] = "Battle Royale Team Deathmatch";
      else if ( $HostTypeName[%type] $= BRPBDM )
         $HostTypeDisplayName[%type] = "Battle Royale Paintball Deathmatch";
      else if ( $HostTypeName[%type] $= BRPBTDM )
         $HostTypeDisplayName[%type] = "Battle Royale Paintball Team Deathmatch";
	 
      else
         $HostTypeDisplayName[%type] = $HostTypeName[%type];
   }
}

function Torque::buildMissionList(%this)
{
   %search = "levels/*.mis";
   $HostTypeCount = 0;
   $HostMissionCount = 0;

   %i = 0;
   for ( %file = findFirstFile(%search); %file !$= ""; %file = findNextFile(%search) )
   {
      // Skip our new level/mission if we arent choosing a level
      // to launch in the editor.
      %name = fileBase(%file); // get the name

      // Do not add the new mission template into rotation.
      if ( !$startWorldEditor && %name $= NewLevel )
         continue;

      %this.addMissionFile( %file );
   }
}

function Torque::addMissionFile( %this, %file )
{
   %idx = $HostMissionCount;
   $HostMissionCount++;
   $HostMissionFile[%idx] = %file;
   $HostMissionName[%idx] = fileBase(%file);

   %LevelInfoObject = %this.getLevelInfo(%file);

   if ( %LevelInfoObject != 0 )
   {
      if( %LevelInfoObject.levelName !$= "" )
         $HostMissionName[%idx] = %LevelInfoObject.levelName;

      if ( %LevelInfoObject.MissionTypes !$= "" )
         %typeList = %LevelInfoObject.MissionTypes;

      for ( %word = 0; ( %misType = getWord( %typeList, %word ) ) !$= ""; %word++ )
      {
         for ( %i = 0; %i < $HostTypeCount; %i++ )
         {
            if ( $HostTypeName[%i] $= %misType )
               break;
         }

         if ( %i == $HostTypeCount )
         {
            $HostTypeCount++;
            $HostTypeName[%i] = %misType;
            $HostMissionCount[%i] = 0;
         }

         // add the mission to the type
         %ct = $HostMissionCount[%i];
         $HostMission[%i, $HostMissionCount[%i]] = %idx;
         $HostMissionCount[%i]++;
      }

      for( %i = 0; %LevelInfoObject.desc[%i] !$= ""; %i++ )
      {
         $HostMissionDesc[$HostMissionFile[%idx], %i] = %LevelInfoObject.desc[%i];
         //echo($HostMissionDesc[$HostMissionFile[%idx], %i]);
      }

      $MissionDisplayName[$HostMissionFile[%idx]] = $HostMissionName[%idx];

      %LevelInfoObject.delete();

      %this.getMissionTypeDisplayNames();

      echo($HostMissionName[%idx] SPC "added to mission list");
   }
}

// Read the level file and find the level object strings, then create the action object via eval so we
// can extract the params from it and load them into global vars
function Torque::getLevelInfo( %this, %missionFile ) 
{
   %file = new FileObject();
   
   %LevelInfoObject = "";
   
   if ( %file.openForRead( %missionFile ) )
   {
      %inInfoBlock = false;

      while ( !%file.isEOF() )
      {
         %line = %file.readLine();
         %line = trim( %line );
         if( %line $= "new ScriptObject(LevelInfo) {" )
            %inInfoBlock = true;
         else if( %line $= "new LevelInfo(theLevelInfo) {" )
            %inInfoBlock = true;
         else if( %inInfoBlock && %line $= "};" ) {
            %inInfoBlock = false;

            %LevelInfoObject = %LevelInfoObject @ %line;
            break;
         }

         if( %inInfoBlock )
            %LevelInfoObject = %LevelInfoObject @ %line @ " ";
      }

      %file.close();
   }
   %file.delete();

   if( %LevelInfoObject !$= "" )
   {
      %LevelInfoObject = "%LevelInfoObject = " @ %LevelInfoObject;
      eval( %LevelInfoObject );

      return %LevelInfoObject;
   }
	
   // Didn't find our LevelInfo
   return 0; 
}

function Torque::getMissionCount(%this, %type)
{
   // Find out how many mission files this gametype has associated with it
   for ( %count = 0; %count < $HostTypeCount; %count++ )
   {
      if ( $HostTypeName[%count] $= %type )
         break;
   }
   return $HostMissionCount[%count];
}
