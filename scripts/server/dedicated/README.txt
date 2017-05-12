//-----------------------------------------------------------------------------
// Copyright (c) 2015 Max Gaming Technologies, LLC
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

In this folder are 2 files dedconfig.cs (Server and rotation config file) and dedicated.cs (main framework file).

INSTRUCTIONS

1.  dedicated.cs must be in the location scripts/server/dedicated.cs.  If you want to put it elsewhere then make sure to edit exec("scripts/server/dedicated.cs"); on line 6 of dedconfig.cs.

2.  dedconfig.cs should go in the same folder as the executable for easy access. In main.cs make sure to add an exec("dedconfig.cs"); with the correct path to the file so it gets executed on startup.  It needs to be executed before parseArgs is called.

3.  in main.cs in the function parseArgs after the case for -mission you need to add the following:

		case "-config":
            $argUsed[%i]++;
            if (%hasNextArg) {
               $DedicatedConfiguration = getDedicatedConfigurationByName(%nextArg);
               setupDedicatedMissionArgsFromConfig();
               $DedicatedConfig = true;
               $argUsed[%i+1]++;
               %i++;
            }
            else
               error("Error: Missing Command Line argument. Usage: -config <configname>");

	
	This code will add the command line option of -config so you can do like TorqueDed -dedicated -config Example1 and it will load the Example1 config from dedconfig.cs.  -mission is no longer required with this framework.
	
4.  As you can see in number 3 above, the config command sets up a simobject var called $DedicatedConfiguration that contains the configuration you loaded.  This can be used throughout your game to access the data from the configuration.  For instance originally -mission would set the $missionArg variable with the path to the map.  Now in your code to initialize the dedicated server you can set these variables from the config like so:  $missionArg = $DedicatedConfiguration.getDedMap();  It is also used to access server specific non map specific items like port number.  In stock the port is hardcoded in script.  So you can keep that there, but on initializing the ded before you start it up you would need to add $Pref::Server::Port = $DedicatedConfiguration.getDedPort(); so the port is set correctly to what is in the dedconfig.cs configuration.  You can see a full list of these accessor functions in the dedicated.cs file.

In the framework there are functions for Timelimit, respawns, kills, mission type, map, bots.  So if you need something else just checkout how one of those entries works in dedicated.cs and add your own.

5.  Outside of the accessor functions like getDedMap(), there is one other function you will need to support map rotation.  When your game is ready to rotate the map, you simple call:

$DedicatedConfiguration.nextMapCycle(); 

Then from there you can reset your variables using the accessor functions like getDedMap().

CONFIGURATION FILE

The dedconfig.cs is where you can place configurations for different servers as well as setup map rotations, etc.  The layout of this file is:

//The follow settings are server specific and not map specific so we only set them once for this config.
//This create a new config and sets the name you will pass into -config at command line to load it
%config = createDedicatedConfiguration("Example1");  //name must not contain spaces
//name that shows up in the server browser in game
%config.setDedName("Server 1"); 
//port must be unique per config.  use port 28000 for lan games. last 3 ports are for steam and must be unique.  If you do not use steam master server you can either remove these 3 ports from this call or just pass in 0 for them as they will be unused.  The first port is the one your server will use.
%config.setDedPort("28000", "28100", "28101", "28102");
//password.  You can leave this command out or set it to "" if you do not want a password
%config.setDedPassword("");
//max players.  Set how many players can join this server
%config.setDedMaxPlayers(16);

//For the map cycle you need to setup the map specific parameters for each map in your rotation.  The call to nextMapCycle mentioned above basically increments the current index your are on ( first paramater passed into each of these functions below ) and will loop back around to 0 once you reach the end.

//Map cycle setup.  If you only setup 1 map then rotation will just keep rotating it.
%config.createMapCycle(0, "GameType", "levels/test/test.mis", "test");
%config.createMapCycle(1, "GameType", "levels/test/test2.mis", "test2");
%config.createMapCycle(2, "GameType", "levels/test/test3.mis", "test3");
%config.createMapCycle(3, "GameType", "levels/test/test4.mis", "test4");
//Time limit, you can use the same settings that are available when you host a game
%config.setDedTimeLimit(0, "30");
%config.setDedTimeLimit(1, "30");
%config.setDedTimeLimit(2, "30");
%config.setDedTimeLimit(3, "30");
//Max spawns, you can use the same settings that are available when you host a game
%config.setDedMaxRespawns(0, "Unlimited");
%config.setDedMaxRespawns(1, "Unlimited");
%config.setDedMaxRespawns(2, "Unlimited");
%config.setDedMaxRespawns(3, "Unlimited");
//Max number of bots, you can use the same settings that are available when you host a game
%config.setDedMaxBots(0, "8");
%config.setDedMaxBots(1, "8");
%config.setDedMaxBots(2, "8");
%config.setDedMaxBots(3, "8");
//Maz number of kills, you can use the same settings that are available when you host a game
%config.setDedMaxKills(0, "16");
%config.setDedMaxKills(1, "16");
%config.setDedMaxKills(2, "16");
%config.setDedMaxKills(3, "16");


You can have as many of these configurations above as you want in dedconfig.cs but you can only load one per server.  Having them all here lets you just copy the game files and run with a different config command so you dont have to alter anything else before doing so.
