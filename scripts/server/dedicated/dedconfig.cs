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

//Dedicated Server Preferences file
//This file can be used to setup a single map config or map rotation configs
//Failure to set a config option will result in the dedicated server not working correctly

//Load up dedicated parsing API
exec("scripts/server/dedicated/dedicated.cs");

//Example configs/////////////////////////////////////////////////////////////////
//Here is are some example configuration.  The second and third one are using the same settings as our current live dedicated servers
//To run this config you would do the following at command line ./TorqueDed -config Example1
//name passed to dedicated server via command line 
%config = createDedicatedConfiguration("Example1");  //name must not contain spaces
//name that shows up in the server browser in game
%config.setDedName("Server 1"); 
//port must be unique per config.  use port 28000 for lan games. last 3 ports are for steam and must be unique, can ignore last 3 if you do not need them
%config.setDedPort("28000", "28100", "28101", "28102");
//password.  You can leave this command out or set it to "" if you do not want a password
%config.setDedPassword("");
//max players.  Set how many players can join this server
%config.setDedMaxPlayers(16);

//Map cycle setup.  If you only setup 1 map then rotation will just keep rotating it.
//Map 0 settings 
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

//Add custom configurations below///////////////////////////////////////////////////
//////////////////EXAMPLE 2////////////////////////////////////////
//Here is are some example configuration.  The second and third one are using the same 
//settings as our current live dedicated servers
//To run this config you would do the following at command line ./TorqueDed -config Example2
//name passed to dedicated server via command line 
%config = createDedicatedConfiguration("Example2");  //name must not contain spaces
//name that shows up in the server browser in game
%config.setDedName("Server 2"); 
//port must be unique per config.  use port 28000 for lan games. last 3 ports are for steam and must be unique
%config.setDedPort("28000", "28100", "28101", "28102");
//password.  You can leave this command out or set it to "" if you do not want a password
%config.setDedPassword("");
//max players.  Set how many players can join this server
%config.setDedMaxPlayers(16);

//Map cycle setup.  If you only setup 1 map then rotation will just keep rotating it.
//Map 0 settings 
//As we add more maps, more details will follow.  Currently only beach (Coastal Flood Plains)
//and mis7(Arctic Battlefield) are valid
%config.createMapCycle(0, "GameType", "levels/test/test6.mis", "test6");
%config.createMapCycle(1, "GameType", "levels/test/test7.mis", "test7");
%config.createMapCycle(2, "DifferentGameType", "levels/test/test8.mis", "test8");
%config.createMapCycle(3, "DifferentGameType", "levels/test/test9.mis", "test9");

//Time limit, you can use the same settings that are available when you host a game
%config.setDedTimeLimit(0, "30");
%config.setDedTimeLimit(1, "30");
%config.setDedTimeLimit(2, "20");
%config.setDedTimeLimit(3, "20");
//Max spawns, you can use the same settings that are available when you host a game
%config.setDedMaxRespawns(0, "Unlimited");
%config.setDedMaxRespawns(1, "Unlimited");
%config.setDedMaxRespawns(2, "3");
%config.setDedMaxRespawns(3, "3");
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
