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

//base simObject and SimGroup
new SimGroup(DedicatedConfigGroup);

//Creates a dedicated configuration object and returns it.
//Note: Name must not contain spaces

function createDedicatedConfiguration(%configName) {
   //create a unique object inherited from DedicatedConfiguration
   %config = new SimObject();
   %config.configurationName = %configName;

   //init vars
   %config.indexCount = 0;
   %config.nameBase = "Torque Dedicated Server";
   %config.port = "28000";
   %config.steamPort = "28100";
   %config.gamePort = "28101";
   %config.queryPort = "28102";
   %config.mapRotation = 0;
   %config.password = "";
   %config.maxPlayers = 8;

   //Add it to list
   DedicatedConfigGroup.add(%config);
   //return a reference to it
	return %config;
}

function getDedicatedConfigurationByName(%configName) {
   for (%i = 0; %i < DedicatedConfigGroup.getCount(); %i++) {
      %config = DedicatedConfigGroup.getObject(%i);
      if (%config.configurationName $= %configName) {
         return %config;
      }
   }
   return 0;
}

function setupDedicatedMissionArgsFromConfig() {
   //Load up values from first cycle
   $timeLimitArg = $DedicatedConfiguration.getDedTimeLimit();
   $maxRespawnArg = $DedicatedConfiguration.getDedRespawns();
   $maxKillsArg = $DedicatedConfiguration.getDedKills();
   $missiontypeArg = $DedicatedConfiguration.getDedMissionType();
   $missionArg = $DedicatedConfiguration.getDedMap();
   $minAIPlayersArg = $DedicatedConfiguration.getDedBots();
}

//Sets the server name of this configuration
function SimObject::setDedName(%this, %name) {
   %this.nameBase = %name;
}

//sets the port of this configuration
function SimObject::setDedPort(%this, %port, %steamPort, %gamePort, %queryPort) {
   %this.port = %port;
   %this.steamPort = %steamPort;
   %this.gamePort = %gamePort;
   %this.queryPort = %queryPort;
}

//sets the password of this configuration
function SimObject::setDedPassword(%this, %password) {
   %this.password = %password;
}

//sets the maximum number of players for this configuration
function SimObject::setDedMaxPlayers(%this, %players) {
   %this.maxPlayers = %players;
}

//create a map cycle with missiontype and map //Note: if you set a map you must set a mission type
function SimObject::createMapCycle(%this, %index, %missionType, %mapName, %mapDisplayName) {
   %this.map[%index] = %mapName;
   %this.mapName[%index] = %mapDisplayName;
   %this.missionType[%index] = %missionType;

   //setup defaults
   %this.time[%index] = "Unlimited";
   %this.respawns[%index] = "Unlimited";
   %this.kills[%index] = "5";
   %this.bots[%index] = "0";

   %this.indexCount++;  //keep a count of index
}

//sets the time limit for this map cycle //Note: Optional
function SimObject::setDedTimeLimit(%this, %index, %time) {
   %this.time[%index] = %time;
}

//sets the maximum number of respawns for this map cycle //Note: Optional
function SimObject::setDedMaxRespawns(%this, %index, %respawns) {
   %this.respawns[%index] = %respawns;
}

//sets the maximum number of bots for this map cycle //Note: Optional
function SimObject::setDedMaxBots(%this, %index, %bots) {
   %this.bots[%index] = %bots;
}

//sets the maximum kills for this map cycle //Note: Optional
function SimObject::setDedMaxKills(%this, %index, %kills) {
   %this.kills[%index] = %kills;
}

//Utility functions to retrieve data //increments the map rotation
function SimObject::nextMapCycle(%this) {
   %this.mapRotation++;
   if (%this.mapRotation >= %this.indexCount) {
      %this.mapRotation = 0;
   }
}

//Returns the server name
function SimObject::getDedName(%this) {
   return %this.nameBase;
}

//Returns the server port number
function SimObject::getDedPort(%this) {
   return %this.port;
}

//Returns the server steam port number
function SimObject::getDedSteamPort(%this) {
   return %this.steamPort;
}

//Returns the server steam game port number
function SimObject::getDedGamePort(%this) {
   return %this.gamePort;
}

//Returns the server steam query port number
function SimObject::getDedQueryPort(%this) {
   return %this.queryPort;
}

//Returns the server password
function SimObject::getDedPassword(%this) {
   return %this.password;
}

//Returns the server max players
function SimObject::getDedMaxPlayers(%this) {
   return %this.maxPlayers;
}

//gets the current rotation map
function SimObject::getDedMap(%this) {
   return %this.map[%this.mapRotation];
}

function SimObject::getDedMapName(%this) {
   return %this.mapName[%this.mapRotation];  
}

//gets the current rotation mission type
function SimObject::getDedMissionType(%this) {
   return %this.missionType[%this.mapRotation];
}

//gets the current rotation time limit
function SimObject::getDedTimeLimit(%this) {
   return %this.time[%this.mapRotation];
}

//gets the current rotation max respawns
function SimObject::getDedRespawns(%this) {
   return %this.respawns[%this.mapRotation];
}

//gets the current rotation min bots
function SimObject::getDedBots(%this) {
   return %this.bots[%this.mapRotation];
}

//gets the current rotation max kills
function SimObject::getDedKills(%this) {
   return %this.kills[%this.mapRotation];
}