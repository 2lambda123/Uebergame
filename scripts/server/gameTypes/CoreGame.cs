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

// DisplayName = Master Gametype

// Game rules:
$HostGameRules["Core", 0] = "Eliminate the competition.";
$HostGameRules["Core", 1] = "Last player standing is declared the winner or";
$HostGameRules["Core", 2] = "player with most kills at timelimit.";

//-----------------------------------------------------------------------------
// <> EXECUTION ORDER <>
//
// CoreGame::activatePackages
// CoreGame::onMissionLoaded
// CoreGame::setupGameParams
// CoreGame::checkMatchStart
// CoreGame::Countdown
// CoreGame::setClientState
// CoreGame::onClientEnterGame
// CoreGame::sendClientTeamList
// CoreGame::spawnPlayer
// CoreGame::pickSpawnPoint
// CoreGame::createPlayer
// CoreGame::loadOut
// CoreGame::startGame
//
// CoreGame::EndCountdown
// CoreGame::onGameDurationEnd
// CoreGame::cycleGame
// CoreGame::endGame
// CoreGame::onCyclePauseEnd
// CoreGame::deactivatePackages
//
//-----------------------------------------------------------------------------

// Pause while looking over the end game screen (in secs)
$Game::EndGamePause = $pref::Server::EndGamePause * 1000;

function CoreGame::activatePackages(%game)
{
   //LogEcho("CoreGame::activatePackages(" SPC %game.class SPC ")");
   // activate the default package for the game type
   // ZOD: We don't have a default package as of yet if ever..
   // All other gametypes MUST have a package even if just a dummy!
   //activatePackage(CoreGame);
   if ( isPackage( %game.class ) && %game.class !$= CoreGame )
      activatePackage( %game.class );
}

function CoreGame::deactivatePackages(%game)
{
   //LogEcho("CoreGame::deactivatePackages(" SPC %game.class SPC ")");
   //deactivatePackage(CoreGame);
   if ( isActivePackage( %game.class ) && %game.class !$= CoreGame )
      deactivatePackage( %game.class );
}

function CoreGame::onMissionLoaded(%game)
{
   // Called by loadMissionStage2() once the mission is finished loading
   //LogEcho("\c4CoreGame::onMissionLoaded(" SPC %game.class SPC ")");

   //set up scoring variables and other game specific globals
   %game.setupGameParams();

   //set up the teams
   if ( !$UsingMainMenuLevel )
   %game.setUpTeams();

   %game.setupObjectCounts();

   MissionGroup.initializeObjective(); // Setup objectives
   MissionGroup.setupPositionMarkers(true); // Setup vehicle spawns

   // Setup protected objects
   if ( $pref::Server::TournamentMode )
      $BaseSacking = 1;
   else
      $BaseSacking = $pref::Server::BaseSacking;

   if ( !$Game::Running )
   {
      if($pref::Server::TournamentMode)
         %game.checkTournamentStart();
      else
         %game.checkMatchStart();
   }
}

function CoreGame::setupObjectCounts(%game)
{
   // The problem with this is that anything within the mission group that is teamed
   // gets its count set to zero here even though we may not wish that to be.

   // Clean up object counts.
   for ( %i = 0; %i <= %game.numTeams; %i++ )
   {
      $TeamDeployedCount[%i, ProxMine] = 0;
      $TeamDeployedCount[%i, TripMineDeployed] = 0;
      $TeamDeployedCount[%i, DeployedTurret] = 0;
	  
      for( %j = 0; %j < $SMS::MaxItems; %j++ )
      {
         %item = $SMS::Item[%j];
         if ( %item.category $= "Deployable" ) // Assume this is deployable and reset its count
            $TeamDeployedCount[%i, %item] = 0;
      }

      // Vehicles
      for ( %x = 0; %x < $SMS::MaxVehicles; %x++ )
      {
         $VehicleTotalCount[%i, $InvVehicle[%x]] = 0;
      }
   }
}

/// Setup scoring etc.
function CoreGame::setupGameParams(%game)
{
   //LogEcho("\c4CoreGame::setupGameParams(" SPC %game.class SPC ")");
   
   $Game::Schedule = "";

   %game.SCORE_PER_DESTROY_VEHICLE = 8;
   %game.SCORE_PER_DESTROY_SHAPE = 0;
   %game.SCORE_PER_DESTROY_TURRET = 6;

   // Setup max vehicles allowed
   for ( %i = 0; %i < $SMS::MaxVehicles; %i++ )
   {
      $Game::VehicleMax[$InvVehicle[%i]] = 4;
      //error("Vehicle max" SPC $InvVehicle[%i] SPC ":" SPC $Game::VehicleMax[$InvVehicle[%i]]);
   }
}

function SimGroup::assignTeam(%this, %team)
{
   for ( %i = 0; %i < %this.getCount(); %i++ )
   {
      %obj = %this.getObject(%i);
      //LogEcho("\c4SimGroup::assignTeam(" SPC %obj.getClassName() SPC %team SPC ")");
      switch$ ( %obj.getClassName() )
      {
         case SpawnSphere:
            if($Game::Running)
            {
               // find out what team the spawnsphere used to belong to
               %found = false;
               for(%l = 1; %l <= Game.numTeams; %l++)
               {
                  %drops = nameToId("MissionCleanup/TeamDrops" @ %l);
                  for(%j = 0; %j < %drops.getCount(); %j++)
                  {
                     %current = %drops.getObject(%j);
                     if(%current == %obj)
                        %found = %l;
                  }
               }

               if(%team != %found)
                  Game.claimSpawn(%obj, %team, %found);
               else
                  error("spawn "@%obj@" is already on team "@%team@"!");
            }
            else
               Game.claimSpawn(%obj, %team, "");

         case SimGroup:
            %obj.assignTeam( %team );

         default:
            %obj.team = %team;
            %className = %obj.getClassName();
            if ( %className $= "Waypoint" || %className $= "Turret" || %className $= "StaticShape" )
               %obj.setTeamId( %team );
      }
   }
}

function CoreGame::claimSpawn(%game, %obj, %newTeam, %oldTeam)
{
   if(%newTeam == %oldTeam)
      return;

   %newSpawnGroup = nameToId("MissionCleanup/TeamDrops" @ %newTeam);
   if(%oldTeam !$= "")
   {
      %oldSpawnGroup = nameToId("MissionCleanup/TeamDrops" @ %oldTeam);
      %oldSpawnGroup.remove(%obj);
   }
   %newSpawnGroup.add(%obj);   
}

function CoreGame::sendClientTeamList(%game, %client)
{
   //LogEcho("\c4CoreGame::sendClientTeamList(" SPC %game.class @", "@ %client.nameBase SPC ")");
   // Send the client the current team list:
   %teamCount = %game.numTeams;
   for ( %i = 0; %i < %teamCount; %i++ )
   {
      if ( %i > 0 )
         %teamList = %teamList @ "\n";

      %teamList = %teamList @ detag( getTaggedString( %game.getTeamName(%i + 1) ) );
   }
   messageClient( %client, 'MsgTeamList', "", %teamCount, addTaggedString(%teamList) );
}

function CoreGame::checkMatchStart(%game)
{
   //LogEcho("\c4CoreGame::checkMatchStart(" SPC %game.class SPC ")");
   if($CountdownStarted || $Game::Running)
      return;

   %count = ClientGroup.getCount();
   for( %i = 0; %i < %count; %i++ )
   {
      %cl = ClientGroup.getObject(%i);
      if(!%cl.isAiControlled())
      {
         clearCenterPrint(%cl);
         clearBottomPrint(%cl);
      }
   }
   %game.Countdown($pref::Server::warmupTime * 1000);
}

function CoreGame::checkTournamentStart(%game)
{
   //LogEcho("\c4CoreGame::checkTournamentStart(" SPC %game.class SPC ")");
   if($CountdownStarted || $Game::Running)
      return;

   // loop through all the clients and see if any are still notready
   %playerCount = 0;
   %notReadyCount = 0;

   %count = ClientGroup.getCount();
   for( %i = 0; %i < %count; %i++ )
   {
      %cl = ClientGroup.getObject(%i);
      if(%cl.camera.Mode $= "pre-game")
      {
         if(%cl.notready)
         {
            %notReady[%notReadyCount] = %cl;
            %notReadyCount++;
         }
         else
         {   
            %playerCount++;
         }
      }
   }

   if ( %notReadyCount )
   {
      if ( %notReadyCount == 1 )
         MessageAll( 'msgHoldingUp', '\c1%1 is holding things up!', %notReady[0].playerName);
      else if ( %notReadyCount < 4 )
      {
         for(%i = 0; %i < %notReadyCount - 2; %i++)
            %str = getTaggedString(%notReady[%i].playerName) @ ", " @ %str;

         %str = "\c2" @ %str @ getTaggedString(%notReady[%i].playerName) @ " and " @ getTaggedString(%notReady[%i+1].playerName) 
                     @ " are holding the Match up!";
         MessageAll( 'msgHoldingUp', %str );
      }
      return;
   }

   if ( %playerCount != 0 )
   {
      %count = ClientGroup.getCount();
      for ( %i = 0; %i < %count; %i++ )
      {
         %cl = ClientGroup.getObject(%i);
         %cl.notready = "";
         %cl.notReadyCount = "";
         clearCenterPrint(%cl);
         clearBottomPrint(%cl);
      }
      
      if ( Game.scheduleVote !$= "" && Game.voteType $= "VoteMatchStart") 
      {
         messageAll('closeVoteHud', "");
         cancel(Game.scheduleVote);
         %game.scheduleVote = "";
      }
      %game.Countdown(30 * 1000);
   }
}

function CoreGame::Countdown(%game, %timeMS)
{
   //LogEcho("\c4CoreGame::Countdown(" SPC %game.class SPC %timeMS SPC ")");
   if( $countdownStarted )
      return;

   $countdownStarted = true;
   %game.matchStart = %game.schedule(%timeMS, "startGame");

   if( %timeMS > 30000 )
      %game.notifyMatchStart(%timeMS);
   if( %timeMS >= 30000 )
      %game.thirtyCount = %game.schedule(%timeMS - 30000, "notifyMatchStart", 30000);
   if( %timeMS >= 15000 )
      %game.fifteenCount = %game.schedule(%timeMS - 15000, "notifyMatchStart", 15000);
   if( %timeMS >= 10000 )
      %game.tenCount = %game.schedule(%timeMS - 10000, "notifyMatchStart", 10000);
   if( %timeMS >= 5000 )
      %game.fiveCount = %game.schedule(%timeMS - 5000, "notifyMatchStart", 5000);
   if( %timeMS >= 4000 )
      %game.fourCount = %game.schedule(%timeMS - 4000, "notifyMatchStart", 4000);
   if( %timeMS >= 3000 )
      %game.threeCount = %game.schedule(%timeMS - 3000, "notifyMatchStart", 3000);
   if( %timeMS >= 2000 )
      %game.twoCount = %game.schedule(%timeMS - 2000, "notifyMatchStart", 2000);
   if( %timeMS >= 1000 )
      %game.oneCount = %game.schedule(%timeMS - 1000, "notifyMatchStart", 1000);
}

function CoreGame::notifyMatchStart(%game, %time)
{
   %seconds = mFloor(%time / 1000);
   if ( %seconds > 2 )
      messageAll('MsgMissionStart', 'Match starts in %1 seconds.', %seconds);
   else if ( %seconds == 2 )
      messageAll('MsgMissionStart', 'Match starts in 2 seconds.');
   else if ( %seconds == 1 )
      messageAll('MsgMissionStart', 'Match starts in 1 second.');

   messageAll('MsgSyncClock', "", mFloor(%seconds * 2), true, true);
}

function CoreGame::startGame(%game)
{
   //LogEcho("\c4CoreGame::startGame(" SPC %game.class SPC ")");
   
   // This is where the game play should start, the default onMissionLoaded function starts the game.
   if ( $Game::Running )
   {
      error("startGame: End the game first!");
      return;
   }

   $Game::StartTime = $Sim::Time; // Keep track of when the game started

   if (!$UsingMainMenuLevel) // No time limit for main menu levels
   {
      // Start the game timer
      if ( $pref::Server::TimeLimit > 0 )
         %curTimeLeftMS = ($pref::Server::TimeLimit * 60 * 1000);
      else
         %curTimeLeftMS = (15 * 60 * 1000);

      $Game::Schedule = %game.schedule(%curTimeLeftMS, "onGameDurationEnd");
      %game.EndCountdown(%curTimeLeftMS); //schedule the end of match countdown
    
      messageAll('MsgSyncClock', "", (%curTimeLeftMS / 1000), true, false);
   }

   $Game::Running = true;

   MessageAll('MsgGameStart', '\c1Match started!');

   // Inform the client we're starting up
   for ( %i = 0; %i < ClientGroup.getCount(); %i++ )
   {
      %cl = ClientGroup.getObject(%i);
      %game.clearClientVaribles(%cl);
      if ( !%cl.isAiControlled() )
      {
         commandToClient(%cl, 'GameStart');

         // Clear any prints
         if ( isObject( %cl.player ) )
         {
            clearBottomPrint(%cl);
            clearCenterPrint(%cl);
         }
      }

      // set all clients control to their player and anything else needed
      if ( !$pref::Server::TournamentMode && %cl.matchStartReady && %cl.camera.mode $= "pre-game" )
      {
         if ( isObject( %cl.player ) )
         {
            if ( %cl.getControlObject() != %cl.player )
               %cl.setControlObject( %cl.player );

            if ( !%cl.isAiControlled() )
               commandToClient(%cl, 'setHudMode', 'Play');
         }
         else
            error("can't set control of player for client: " @ %cl.nameBase @ ", no player object found!");
      }
      else
      {
         if ( %cl.matchStartReady && %cl.camera.mode $= "pre-game" )
         {
            if ( isObject( %cl.player ) )
            {
               if ( %cl.getControlObject() != %cl.player )
                  %cl.setControlObject( %cl.player );

               if ( !%cl.isAiControlled() )
                  commandToClient(%cl, 'setHudMode', 'Play');
            }
            else
               echo("can't set control of player for client: " @ %cl.nameBase @ ", no player object found!");
         }
      }
   }
}

/// Called from function serverCmdMissionStartPhase2Ack in commands.cs
function CoreGame::setClientState(%game, %client)
{
   //LogEcho("\c4CoreGame::setClientState(" SPC %game.class SPC %client.nameBase SPC ")");
   // create a new camera for this client
   %client.camera = new Camera() 
   {
      dataBlock = Spectator;
   };

   // we get automatic cleanup this way.
   %client.camera.scopeToClient(%client);
   MissionCleanup.add(%client.camera);

   if(isObject(%client.rescheduleVote))
      cancel(%client.rescheduleVote);

   %client.canVote = true;
   %client.rescheduleVote = "";

   %spectator = false;
   // Tournament mode is taken care of in ::onClientEnterGame
   if ( !$pref::Server::TournamentMode )
   {
      if ( %client.justConnected )
      {
         %client.justConnected = false;
         %client.camera.getDataBlock().setMode(%client.camera, "NewConnect");
      }
      else
      {
         // server just changed maps - this guy was here before
         if ( %client.lastTeam !$= "" )
         {   
            // see if this guy was an Spectator from last game
            if ( %client.lastTeam == 0 )
            {
               %client.team = 0;
               %client.camera.getDataBlock().setMode(%client.camera, "SpectatorFly");
            }
            else  // let this player join the team he was on last game
            {
               if ( %game.numTeams > 1 && %client.lastTeam <= %game.numTeams )
               {   
                  %game.clientJoinTeam(%client, %client.lastTeam, false);
               }
               else
               {   
                  %game.assignClientTeam(%client);
                  %game.spawnPlayer(%client, 0); // spawn the player
               }
            }
         }
         else
         {   
            %game.assignClientTeam(%client);
            %game.spawnPlayer(%client, 0); // spawn the player
         } 
      }
   }

   // Send mission information
   messageClient(%client, 'MsgEnterGameInfo', '\c1You are in mission %1 (%3)',
                                              addTaggedString($MissionDisplayName),
                                              addTaggedString(%game.class), 
                                              addTaggedString($MissionTypeDisplayName), 
                                              addTaggedString($pref::Server::Name));
}

// Called from function serverCmdMissionStartPhase3Ack in commands.cs
function CoreGame::onClientEnterGame(%game, %client)
{
   //LogEcho("\c4CoreGame::onClientEnterGame(" SPC %game.class SPC %client.nameBase SPC ")");
   
   // Setup zee bots
   if ( %client.isAiControlled() && !$UsingMainMenuLevel )
   {
      %game.assignClientTeam(%client);
      %game.spawnPlayer(%client, 0);
      %client.matchStartReady = true;
      %client.notReady = false;
      return;
   }

   //synchronize the clock HUD
   messageClient(%client, 'MsgSyncClock', "", $Sim::Time - $Game::StartTime);
   %game.sendClientTeamList(%client);

   if ( !$pref::Server::TournamentMode )
   {
      if ( %client.team == 0 )
      {
         %client.observerStartTime = getSimTime();
         if ( %client.getControlObject() != %client.camera )
            %client.setControlObject(%client.camera);

         commandToClient(%client, 'setHudMode', 'Spectator');
         updateSpectatorHud(%client);
      }
      else // Failsafe even though startgame should take care of this
      {
         if ( !$Game::Running )
         {
            if ( %client.getControlObject() != %client.camera )
               %client.setControlObject(%client.camera);

            commandToClient(%client, 'setHudMode', 'Spectator');
         }
         else
         {
            if ( isObject( %client.player ) )
            {
               if ( %client.getControlObject() != %client.player )
                  %client.setControlObject(%client.player);

               commandToClient(%client, 'setHudMode', 'Play');
            }
            else
               error("can't set control of player for client: " @ %client.nameBase @ ", no player object found!");
         }
      }  
   }
   else
   {
      // set all players into spectator mode. setting the control object will handle further procedures...
      commandToClient(%client, 'setHudMode', 'Spectator');
      %client.camera.getDataBlock().setMode(%client.camera, "SpectatorFly");
      %client.setControlObject(%client.camera);
      messageAll( 'MsgClientJoinTeam', "", %client, 0, %game.getTeamName(0));
      %client.team = 0;
      
      if ( !$Game::Running && !$CountdownStarted )
      {
         if ( $FriendlyFire )
            %damMess = "ENABLED";
         else
            %damMess = "DISABLED";
         
         BottomPrint(%client, "Server is Running in Tournament Mode.\nPick a Team\nFriendly fire is " @ %damMess, 0, 3 ); 
      }
      else
      {
         BottomPrint( %client, "\nServer is Running in Tournament Mode", 0, 3 ); 
      }   
   }

   if ( %client.team > 0 )
      messageClient(%client, 'MsgCheckTeamLines', "", %client.team);

   // new 3.9 entity feature
   %entityIds = parseMissionGroupForIds("Entity", "");
   %entityCount = getWordCount(%entityIds);
   
   for(%i=0; %i < %entityCount; %i++)
   {
      %entity = getWord(%entityIds, %i);
      
      for(%e=0; %e < %entity.getCount(); %e++)
      {
         %child = %entity.getObject(%e);
         if(%child.getCLassName() $= "Entity")
            %entityIds = %entityIds SPC %child.getID();  
      }
      
      for(%c=0; %c < %entity.getComponentCount(); %c++)
      {
         %comp = %entity.getComponentByIndex(%c);
         
         if(%comp.isMethod("onClientConnect"))
         {
            %comp.onClientConnect(%client);  
         }
      }
   }
  
   // were ready to go.
   %client.matchStartReady = true;
   echo("Client" SPC %client SPC "is ready.");
}

function CoreGame::pickObserverSpawn(%game, %client, %next)
{
   //LogEcho("\c4CoreGame::pickObserverSpawn(" SPC %game.class SPC %client.nameBase SPC %next SPC ")");
   %group = nameToID("MissionGroup/ObserverDropPoints");
   %count = %group.getCount();

   if ( !%count || %group == -1 )
   {
      echo("no observer spawn points found");
      return -1;
   }    
    
   if ( %client.lastObserverSpawn == -1 )
   {
      %client.lastObserverSpawn = 0;
      return(%group.getObject(%client.lastObserverSpawn));
   }

   if ( %next == true )
      %spawnIdx = %client.lastObserverSpawn + 1;
   else
      %spawnIdx = %client.lastObserverSpawn - 1;

   if ( %spawnIdx < 0 )
      %spawnIdx = %count - 1;
   else if ( %spawnIdx >= %count )
      %spawnIdx = 0;
      
   %client.lastObserverSpawn = %spawnIdx;
   return %group.getObject(%spawnIdx);
}

function CoreGame::pickSpawnPoint(%game, %team) 
{
   // Only have 1 team free for all, clamp spawngroup
   if ( %game.numTeams <= 1 && %team > 0 )
      %team = 0;

   //LogEcho("\c4CoreGame::pickSpawnPoint(" SPC %game.class SPC %team SPC ")");
   %spawngroup = "MissionCleanup/TeamDrops" @ %team;
   %group = nameToID(%spawngroup);
   if ( %group != -1 )
   {
      %count = %group.getCount();
      if ( %count > 0 )
      {
         %index = getRandom(%count-1);
         %spawn = %group.getObject(%index);

         // See if this spawn is setup for this game type
         %bypass = true;
         if( %spawn.gameTypesList !$= "" )
         {
            for ( %i = 0; (%allow = getWord(%spawn.gameTypesList, %i)) !$= ""; %i++ )
            {
               if ( %allow $= $Server::MissionType )
                  %bypass = false;
            }
         }
         else
            %bypass = false;

         if ( !%bypass )
         {
            if ( %spawn.sphereWeight > 0 )
            {
               %pos = VectorAdd( %game.pickPointInSpawnSphere( %spawn ), "0 0 0.1" );
               %rot = mDegToRad(getRandom(360));
               return( %pos SPC "0 0 1 " @ %rot );
            }
            else
            {
               InitContainerRadiusSearch( %spawn.position, 3.5, $TypeMasks::PlayerObjectType );
               %test = containerSearchNext();
               if ( !%test )
                  return %spawn.getTransform();
               else
                  return %game.pickSpawnPoint(%team); // Try try again.
            }
         }
         else
            return %game.pickSpawnPoint(%team); // Try try again.
      }
      else
         error("No spawn points found in " @ %spawngroup);
   }
   else
      error("Missing spawn points group " @ %spawngroup);

   // If we can't find any group or spawn points, stick the player at mission center
   return( bumpZ("0 0 800", 0.5) SPC "1 0 0 0" );
}

function CoreGame::pickPointInSpawnSphere(%game, %spawnSphere)
{
   %SpawnLocationFound = false;
   %attemptsToSpawn = 0;
   while( !%SpawnLocationFound && ( %attemptsToSpawn < 5 ) )
   {
      %spherePos = %spawnSphere.getPosition();

      // Attempt to spawn the player within the bounds of the spawnsphere.
      %angleY = mDegToRad( getRandom( 0, 100 ) * m2Pi() );
      %angleXZ = mDegToRad( getRandom( 0, 100 ) * m2Pi() );

      %sphereLocation = setWord( %spherePos, 0, getWord(%spherePos, 0) + (mCos(%angleY) * mSin(%angleXZ) * getRandom(-%spawnSphere.radius, %spawnSphere.radius)));
      %sphereLocation = setWord( %sphereLocation, 1, getWord(%sphereLocation, 1) + (mCos(%angleXZ) * getRandom(-%spawnSphere.radius, %spawnSphere.radius)));
      
      %SpawnLocationFound = true;

      // Now we need to make sure that the surface we want to spawn on is valid..
      %x = getWord( %sphereLocation, 0 );
      %y = getWord( %sphereLocation, 1 );
      %z = getWord( %sphereLocation, 2 );
      %start = %x SPC %y SPC ( %z + 10 );
      %end = %x SPC %y SPC "-1";
      %surface = containerRayCast( %start, %end, $TypeMasks::TerrainObjectType | $TypeMasks::StaticObjectType, 0 );
      if ( !%surface )
      {
         //warn( "No valid spawn surface could be found" );
         %SpawnLocationFound = false;
         break;
      }

      // Now have to check that another object doesn't already exist at this spot.
      initContainerRadiusSearch( %sphereLocation, 3.5, ( $TypeMasks::PlayerObjectType | $TypeMasks::PlayerObjectType ) );
      while ( (%objectNearExit = containerSearchNext()) != 0 )
      {
         // If any player is found within this radius, mark that we need to look for another spot.
         %SpawnLocationFound = false;
         break;
      }   
      // If the attempt at finding a clear spawn location failed try no more than 5 times.
      %attemptsToSpawn++;
   }
      
   // If we couldn't find a spawn location after 5 tries, spawn the object at the center of the sphere and give a warning.
   if ( !%SpawnLocationFound )
   {
      %sphereLocation = %spherePos;
      warn("WARNING: Could not spawn player after" SPC %attemptsToSpawn 
      SPC "tries in spawnsphere" SPC %spawnSphere SPC "without overlapping another player. Attempting spawn in center of sphere.");
   }
   
   return( %sphereLocation );
}

function CoreGame::spawnPlayer(%game, %client, %respawn)
{
   //LogEcho("\c4CoreGame::spawnPlayer(" SPC %game.class SPC %client.nameBase SPC %respawn SPC ")");
   // Combination create player and drop him somewhere
   %spawnPoint = %game.pickSpawnPoint(%client.team);
   %game.createPlayer(%client, %spawnPoint, %respawn);
   //echo ( "team= " @ %client.team ); //activate to check which team the player is in
}

function CoreGame::createPlayer(%game, %client, %spawnPoint, %respawn, %team)
{
   //LogEcho("\c4CoreGame::createPlayer(" SPC %game.class SPC %client.nameBase SPC %spawnPoint SPC 
   
   // The client should not have a player currently assigned. Assigning a new one could result in a player ghost.
   if( isObject(%client.player) && ( %client.player.getState() !$= "Dead" ) )
      %client.player.schedule(50,"delete");
	  
   // clients and cameras can exist in team 0, but players should not Force to a valid team
   if ( %client.team == 0 )
   {
      %game.assignClientTeam(%client, $Game::Running);
      %spawnPoint = %game.pickSpawnPoint(%client.team);
   }

   //error("Creating" SPC %client.nameBase SPC "\s' player on team" SPC %client.team);
   // Create the player object, AiClients get AiPlayer class
   if ( %client.isAiControlled() )
   {
	  switch$ (%game.playerType)
	  {
	   case "DefaultPlayerData":
	    %player = new AiPlayer()
		 {
         dataBlock = DefaultPlayerData;
         class = "BadBot";
         client = %client;
         team = %client.team;
         isBot = true;
         mMoveTolerance = 0.10;
         allowWalk = true;
         allowJump = true;
         allowDrop = true;
         allowSwim = true;
         allowLedge = true;
         allowClimb = true;
         allowTeleport = true;
         };
		 
	   case "Paintball":
		 %player = new AiPlayer()
		 {
         dataBlock = PaintballPlayerData;
         class = "BadBot";
         client = %client;
         team = %client.team;
         isBot = true;
         mMoveTolerance = 0.10;
         allowWalk = true;
         allowJump = true;
         allowDrop = true;
         allowSwim = true;
         allowLedge = true;
         allowClimb = true;
         allowTeleport = true;
         };
		 
	   default:
		   %player = new AiPlayer()
         {
         dataBlock = $NameToData[%client.loadout[0]];
         class = "BadBot";
         client = %client;
         team = %client.team;
         isBot = true;
         mMoveTolerance = 0.10;
         allowWalk = true;
         allowJump = true;
         allowDrop = true;
         allowSwim = true;
         allowLedge = true;
         allowClimb = true;
         allowTeleport = true;
         };
      }
      
   %player.setPosition(%spawnPoint); //BadBot
   %player.tetherPoint = %spawnPoint; // Tetherpoint will give the bot a place to call home
    
   }
   else
   {
	  switch$ (%game.playerType)
	  {
	   case "DefaultPlayerData":
	    %player = new Player() 
         {
           dataBlock = DefaultPlayerData;
           client = %client;
           team = %client.team;
           isBot = false;
         };
		 
	   case "Paintball":
		 %player = new Player() 
         {
           dataBlock = PaintballPlayerData;
           client = %client;
           team = %client.team;
           isBot = false;
         };
		 
	   default:
		 %player = new Player() 
         {
           dataBlock = $NameToData[%client.loadout[0]];
           client = %client;
           team = %client.team;
           isBot = false;
         };
	  }
   }
   MissionCleanup.add(%player);

   if(%respawn)
   {
      // TGE 1.5
      //setInvincibleMode( time , speed )

      //Purpose
      //Use the setInvincibleMode method to temporarily make this shape invincible. i.e. Not able to take damage. While the player is invincible, the screen will flicker blue with a varying rate and a varying intensity.

      //Syntax
      //time - A floating-point value specifying the time in seconds for this shape to remain invincible.
      //speed - A floating-point value between 0.0 and 1.0 controlling the rate at which the blue flickering effect occurs.

      //Notes
      //The flickering effect is used to indicated to a player that his (or her) avatar is invincible. Furthermore, this flicker rate will change and the flicker will become increasingly translucent as the time elapses. If speed set to 1.0, the flickering is a bit obscene. Generally, lower values are nicer.
      //%player.setInvincibleMode($InvincibleTime, 0.02);

      %client.respawns--;
      %player.setInvincible( true );
	   %player.schedule( $InvincibleTime, "setInvincibleOff" );	  
   }

   // Player setup...
   %client.player = %player;
   %player.setTeamId(%client.team);
   %player.setTransform(%spawnPoint);
   %player.setShapeName(%client.playerName);
   %player.isInIronSights = false; // init iron sight aiming variable
   %player.isReloading = false; // init reloading variable
   
   if ( %client.team == 0 )
      %player.setSkinName( %client.skin );
   if ( %client.team == 1 )
      %player.setSkinName( "blue" );
   if ( %client.team == 2 )
      %player.setSkinName( "red" );

   %player.setRechargeRate(%player.getDataBlock().rechargeRate);
   %player.setEnergyLevel(%player.getDataBlock().maxEnergy);
   %player.setRepairRate(%player.getDataBlock().repairRate);

   // Setup inventory - We could tie this into some object that has to be enabled in order to get your selection
   // Perhaps deployables would require their own object you get them from. So they would not be part of your loadout
   %game.loadOut(%player);

   if ( !%client.isAiControlled() )
   {
   switch$ (%game.playerType)
      {
      case "Paintball": 
         //loadout a red marker by default for red team and for blue team and deathmatch a blue marker as default
      if ( %player.weaponSlot[0] $= "" && %client.team == 2 )
      {
         %player.setInventory( PaintballMarkerRed, 1, 1 );
         %player.setInventory( PaintballClip, %player.maxInventory(PaintballClip), 1 );
         %player.setInventory( PaintballAmmo, %player.maxInventory(PaintballAmmo), 1 );
         %player.weaponCount++;
      }
      else if ( %player.weaponSlot[0] $= "" )
      {
         %player.setInventory( PaintballMarkerBlue, 1, 1 );
         %player.setInventory( PaintballClip, %player.maxInventory(PaintballClip), 1 );
         %player.setInventory( PaintballAmmo, %player.maxInventory(PaintballAmmo), 1 );
         %player.weaponCount++;
      }
      
      %player.use( %player.weaponSlot[0] ); //use slot 0 since this is our first weapon
	 
      case "DefaultPlayerData": 
	 //check if soldier did not equip a primary weapon and give Lurker Rifle as a default then
      if ( %player.weaponSlot[1] $= "" )
      {
         %player.setInventory( Lurker, 1, 1 );
         %player.setInventory( LurkerClip, %player.maxInventory(LurkerClip), 1 );
         %player.setInventory( LurkerAmmo, %player.maxInventory(LurkerAmmo), 1 );
         %player.weaponCount++;
      }
      //use slot 1 for the DefaultPlayerData, since his main weapon is on slot 1 instead of 0, since he has a pistol also
      %player.use( %player.weaponSlot[1] );
	 
      default: %player.use( %player.weaponSlot[0] ); //all others start with weapon slot 0 as default
      }
   }
   
   // Update the camera to start with the player.
   if ( !%client.isAiControlled() )
   {
      %client.camera.setTransform(%player.getEyeTransform());

      // This used to be done in the camera trigger but timing issues can cause crashes
      // Decide which object the client is in control of depending on game state
      if ( $Game::Running )
      {
         if ( %client.getControlObject() != %player )
         %client.setControlObject( %player );

         commandToClient(%client, 'setHudMode', 'Play');
      }
      else
      {
         // Should allready have control of the camera. Check just in case
         if ( %client.getControlObject() != %client.camera )
         %client.setControlObject( %client.camera );

         %client.camera.getDataBlock().setMode( %client.camera, "pre-game", %player );
      }
   }
   // Setup the bot
   if ( ( $Game::Running || %respawn ) && %client.isAiControlled() )
   {
      // Set the bot up with some random inventory selection
      //echo( "playerType: " @ %game.playerType );
      switch$ (%game.playerType)
      {
         case "DefaultPlayerData":
         %client.setBotFav(%client.getRandomLoadout());
         %player.use( %player.weaponSlot[1] );
         case "Paintball":
         %client.setBotFav(%client.getRandomLoadout2());
         %player.use( %player.weaponSlot[0] );
         default:
         %client.setBotFav(%client.getRandomLoadout());
         %player.use( %player.weaponSlot[1] );
      }
   }
   // update anyone spectating this client we dont worry about bots here because their cam mode would always be NULL.
   %count = ClientGroup.getCount();
   for (%i = 0; %i < %count; %i++)
   {
      %cl = ClientGroup.getObject(%i);
      if (%cl.camera.mode $= "spectatorFollow" && %cl.observeClient == %player.client)
      {
         %transform = %player.getTransform();
         %cl.camera.setOrbitMode(%player, %transform, 0.5, 4.5, 4.5);
         %cl.camera.targetObj = %player;
      }
   }
   return( %player );
}

function CoreGame::loadOut(%game, %player)
{
   //LogEcho("\c4CoreGame::loadOut(" SPC %game.class SPC %player.client.nameBase SPC ")");
   %client = %player.client;

   // Equip the player with their choosen weapons etc.
   if ( %player.client.isAiControlled() )
      %player.client.ProcessLoadout();
   else
   {
      SmsInv.ProcessLoadout( %client );
	  
	  if (%player.PlayerData = DefaultPlayerData)
      {
         //%player.setInventory( HealthKit, 1 );
         %player.setInventory( Ryder, 1, 1 );
         %player.setInventory( RyderClip, %player.maxInventory(RyderClip), 1 );
         %player.setInventory( RyderAmmo, %player.maxInventory(RyderAmmo), 1 );
      }
      %player.weaponCount++;
   }
}

function CoreGame::onClientLeaveGame(%game, %client)
{
   //LogEcho("\c4CoreGame::onClientLeaveGame(" SPC %game.class SPC %client.nameBase SPC ")");

   serverCmdleaveFireTeam(%client); // Leave fire team if any..

   // Remove any turrets this client owns
   if ( %client.ownedTurrets )
   {
      for ( %i=0; %i<%client.ownedTurrets.getCount(); %i++ )
      {
         %turret = %client.ownedTurrets.getObject(%i);
         %turret.damage( %turret.getDataBlock().maxDamage );
      }
   }

   if (isObject(%client.camera))
      %client.camera.delete();

   if (isObject(%client.player))
      %client.player.delete();
}

function CoreGame::onDamaged(%game, %clVictim, %clAttacker, %sourceObject, %damageType)
{
   //LogEcho("\c4CoreGame::onDamaged(" SPC %game.class @", "@ %clVictim.nameBase @", "@ %clAttacker.nameBase @", "@ %sourceObject @", "@ %damageType SPC ")");
   if ( %clAttacker && %clAttacker != %clVictim && %clVictim.team == %clAttacker.team )
   {
      if ( %clAttacker.player.causedRecentDamage != %clVictim.player )
      {
         %clAttacker.player.causedRecentDamage = %clVictim.player;
         if (!%sourceObject.isAiControlled) 
         %clAttacker.player.schedule(50, "causedTeamDamage", ""); //this needs to be fixed to work with AI players later, so they get punished for team kill
         %game.friendlyFireMessage(%clVictim, %clAttacker);          
      }    
   }

   if ( %clAttacker && %clAttacker != %clVictim )
   {
      %clVictim.lastDamaged = getSimTime();
      %clVictim.lastDamagedBy = %clAttacker;
   }
}

function CoreGame::friendlyFireMessage(%game, %damaged, %damager)
{
   messageClient(%damaged, 'MsgDamagedByTeam', '\c1You were harmed by teammate %1', %damager.playerName);
   messageClient(%damager, 'MsgDamagedTeam', '\c1You just harmed teammate %1.', %damaged.playerName);
}

function CoreGame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc)
{
    //LogEcho("\c4CoreGame::onDeath(" SPC %game.class @", "@ %player @", "@ %client.nameBase @", "@ %sourceObject @", "@ %sourceClient @", "@ %damageType @", "@ %damLoc SPC ")");

   // Switch the client over to the death cam and unhook the player object.
   if ( isObject( %client ) )
   {
      // Some default scoring etc..
      // Did a client controlled object kill this player?
      if( isObject( %sourceClient ) )
      {
         // Did this client kill itself?
         if ( %damageType == 10 || %sourceClient == %client )
         {
            %game.awardScoreSuicide( %client );
            messageAll('MsgDeath', '%1 committed suicide.', %client.playerName );
         }
         else if( %sourceClient.team == %client.team )
         {
            %game.awardScoreTeamkill( %client, %sourceClient );
            messageAll( 'MsgDeath', '%1 was killed by teammate %2.', %client.playerName, %sourceClient.playerName );
         }
         else
         {
            %game.awardScoreKill(%sourceClient, %damageType);
            %game.awardScoreDeath(%client, %damageType);
            messageAll( 'MsgDeath', '%1 was killed by %2\c2 [%3]', %client.playerName, %sourceClient.playerName, $DamageText[%damageType] );
         }

         // Send silent message with player stats
         messageClient(%sourceClient, 'MsgStatsKiller', "", addTaggedString($DamageText[%damageType]), addTaggedString(%sourceClient.kills[$DamageText[%damageType]]));
      }
      else
      {
         if ( isObject( %sourceObject ) )
         {
            %game.awardScoreDeath(%client, %damageType);

            // Death message
            switch$ ( $DamageText[%damageType] )
            {
               case "Impact":
                  messageAll( 'MsgDeath', '%1 flew well but landed poorly.', %client.playerName );

               case "Fire" or "Lava":
                  messageAll( 'MsgDeath', '%1 became the main cource at a barbecue.', %client.playerName );

               case "Lightning":
                  messageAll( 'MsgDeath', '%1 became a lightning rod.', %client.playerName );

               default:
                  messageAll( 'MsgDeath', '%1 was killed.', %client.playerName );
            }
         }
         else
         {
            %game.awardScoreDeath(%client, %damageType);
            messageAll( 'MsgDeath', '%1 died.', %client.playerName );
         }
      }

      // Deal with corpses, cameras etc.
      if ( %client.isAiControlled() )
      {
         %game.schedule($CorpseTimeoutValue, "spawnPlayer", %client, %true);
      }
      else
      {
         if ( isObject( %client.camera ) && isObject( %player ) )
         {
            %client.setControlObject( %client.camera );
            %client.camera.setMode( "Corpse", %player );
         }
         %client.respawnWait = 1;
         %game.schedule( $CorpseTimeoutValue, "clearRespawnWait", %client );
      }

      %client.player = 0;
   }
}

function CoreGame::clearRespawnWait(%game, %client)
{
   if(isObject(%client))
      %client.respawnWait = 0;
}

// Some defaults
function CoreGame::clearClientVaribles(%game, %client)
{
   %client.score = 0;
   %client.kills = 0;
   %client.teamKills = 0;
   %client.deaths = 0;
   %client.suicides = 0;
   %client.efficiency = 0.0;
   //%client.midairs = 0; //unused?!
   %client.vehicleDestroys = 0;
   %client.shapeDestroys = 0;
   %client.turretDestroys = 0;
}

//-----------------------------------------------------------------------------
// Games over, go home
//-----------------------------------------------------------------------------
function CoreGame::EndCountdown(%game, %timeMS)
{
   //LogEcho("\c4CoreGame::EndCountdown(" SPC %game.class SPC %timeMS SPC ")");

   if(%timeMS >= 60000)
      %game.endsixtyCount = %game.schedule(%timeMS - 60000, "notifyMatchEnd", 60000);
   if(%timeMS >= 30000)
      %game.endthirtyCount = %game.schedule(%timeMS - 30000, "notifyMatchEnd", 30000);
   if(%timeMS >= 10000)
      %game.endtenCount = %game.schedule(%timeMS - 10000, "notifyMatchEnd", 10000);
   if(%timeMS >= 5000)
      %game.endfiveCount = %game.schedule(%timeMS - 5000, "notifyMatchEnd", 5000);
   if(%timeMS >= 4000)
      %game.endfourCount = %game.schedule(%timeMS - 4000, "notifyMatchEnd", 4000);
   if(%timeMS >= 3000)
      %game.endthreeCount = %game.schedule(%timeMS - 3000, "notifyMatchEnd", 3000);
   if(%timeMS >= 2000)
      %game.endtwoCount = %game.schedule(%timeMS - 2000, "notifyMatchEnd", 2000);
   if(%timeMS >= 1000)
      %game.endoneCount = %game.schedule(%timeMS - 1000, "notifyMatchEnd", 1000);
}

function CoreGame::notifyMatchEnd(%game, %time)
{
   %seconds = mFloor(%time / 1000);
   if (%seconds > 1)
      messageAll('MsgMissionEnd', 'Match ends in %1 seconds.', %seconds);
   else if (%seconds == 1)
      messageAll('MsgMissionEnd', 'Match ends in 1 second.');
}

function CoreGame::onGameDurationEnd(%game)
{
   //LogEcho("\c4CoreGame::onGameDurationEnd(" SPC %game.class SPC ")");
   
   // This "redirect" is here so that we can abort the game cycle if the $Game::Duration variable
   // has been cleared, without having to have a function to cancel the schedule.

   echo("Game over (timelimit)");
   %game.cycleGame();
}

function CoreGame::cycleGame(%game)
{
   //LogEcho("\c4CoreGame::cycleGame(" SPC %game.class SPC ")");
   
   // This is setup as a schedule so that this function can be called directly from object callbacks. Object callbacks
   // have to be carefull about invoking server functions that could cause their object to be deleted.
   if ($Game::Running && (!EditorIsActive() && !GuiEditorIsActive()))
   {
      if(isEventPending($Game::Schedule))
         cancel($Game::Schedule);

      %game.endGame();
      $Game::Schedule = %game.schedule($Game::EndGamePause, "onCyclePauseEnd");
   }
}

function CoreGame::endGame(%game)
{
   //LogEcho("\c4CoreGame::endGame(" SPC %game SPC ")");
   if(!$Game::Running)
   {
      error("endGame: No game running!");
      return;
   }

   %game.cancelCountdown();
   %game.cancelEndCountdown();

   for (%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %cl = ClientGroup.getObject(%i);
      %cl.lastTeam = %client.team;
      if(!%cl.isAiControlled())
      {
         commandToClient(%cl, 'GameEnd'); // end the game
         commandToClient(%cl, 'setHudMode', 'Spectator'); // Turn the hud off
      }
      else // Cleanup the ai
         %cl.onEndGame();
   }

   messageAll('MsgGameOver', '\c2Match has ended.');
   messageAll('MsgClearObjHud', "");

   %game.setupClientTeams(); // Setup random teams
   $Game::Running = false;
}

function CoreGame::setupClientTeams(%game)
{
   //LogEcho("\c4CoreGame::setupClientTeams(" SPC %game SPC ")");
   if(!$pref::Server::RandomizeTeams || %game.numTeams == 1)
   {
      %count = ClientGroup.getCount();
      for (%i = 0; %i < %count; %i++)
      {
         %client = ClientGroup.getObject(%i);
         %client.lastTeam = %client.team;
         %client.setupTeam = 0;
      }
      return;
   }
   else
   {
      %numTeamPlayers = 0;
      %totalNumPlayers = ClientGroup.getCount();
      for(%i = 0; %i < %totalNumPlayers; %i++)
      {
         %cl = ClientGroup.getObject(%i);
         if(%cl.team == 0)
            %cl.lastTeam = %cl.team;
         else
         {
            %teamPlayer[%numTeamPlayers] = %cl;
            %numTeamPlayers++;
         }
      }

      %numPlayersLeft = %numTeamPlayers - 1;
      for(%j = 0; %j < %numTeamPlayers; %j++)
      {
         if(%numPlayersLeft > 0)
         {
            %r = 0;
            %val = mFloor(getRandom(0, %numPlayersLeft));
            if(%val > %numPlayersLeft)
               %val = %numPlayersLeft;

            %client = %teamPlayer[%val];
            %shuffledPlayersArray[%j] = %client;
            for(%y = 0; %y <= %numPlayersLeft; %y++)
            {
               %clplyr = %teamPlayer[%y];
               if(%clplyr != %client)
               {
                  %teamPlayer[%r] = %clplyr;
                  %r++;
               }
            }
            %numPlayersLeft--;
         }
         else
            %shuffledPlayersArray[%j] = %teamPlayer[%numPlayersLeft];
      }

      %thisTeam = 1;
      for(%k = 0; %k <= %numTeamPlayers; %k++)
      {
         if(%thisTeam == 1)
         {
            %shuffledPlayersArray[%k].lastTeam = 1;
            %thisTeam = 0;
         }
         else
         {
            %shuffledPlayersArray[%k].lastTeam = 2;
            %thisTeam = 1;
         }
      }
   }
}

function CoreGame::onCyclePauseEnd(%game)
{
   //LogEcho("\c4CoreGame::onCyclePauseEnd(" SPC %game.class SPC ")");
   $Game::Cycling = false;
   if(isEventPending($Game::Schedule))
      cancel($Game::Schedule);

   $Game::Schedule = "";

   %game.cycleMissions(); // Just cycle through the missions for now.
}

function CoreGame::getNextMission(%game, %mission, %misType)
{
   // First find the index of the mission in the list:
   for ( %mis = 0; %mis < $HostMissionCount; %mis++ )
   {
      if ( $HostMissionFile[%mis] $= %mission )
         break;
   }
   if ( %mis == $HostMissionCount )
      return "";

   // Now find the index of the mission type:
   for ( %type = 0; %type < $HostTypeCount; %type++ )
   {
      if ( $HostTypeName[%type] $= %misType )
         break;
   }
   if ( %type == $HostTypeCount )
      return "";

   // Now find the mission's index in the mission-type specific sub-list:
   for ( %i = 0; %i < $HostMissionCount[%type]; %i++ )
   {
      if($HostMission[%type, %i] == %mis)
         break;
   }

   if ( $pref::Server::RandomMissions )
   {
      %i = mFloor( getRandom( 0, ( $HostMissionCount[%type] - 1 ) ) );

      // If its same as last map, go back 1
      if ( $HostMissionFile[$HostMission[%type, %i]] $= %mission )
         %i--;

      // If its greater then or equal to count, set to zero
      %i = %i >= $HostMissionCount[%type] ? 0 : %i;
   }
   else
   {
      // Go BACKWARDS, because the missions are in reverse alphabetical order:
      if(%i == 0)
         %i = $HostMissionCount[%type] - 1;
      else
         %i--;
   }
   return $HostMission[%type, %i];
}

function CoreGame::findNextCycleMission(%game)
{
   %tempMission = $Server::MissionFile;
   %failsafe = 0;
   while(1)
   {
      %nextMissionIndex = %game.getNextMission( %tempMission, $Server::MissionType );
      %nextPotentialMission = $HostMissionFile[%nextMissionIndex];

      //just cycle to the next if we've gone all the way around...
      if ( %nextPotentialMission $= $Server::MissionFile || %failsafe >= 1000 )
      {
         %nextMissionIndex = %game.getNextMission( $Server::MissionFile, $Server::MissionType );
         return $HostMissionFile[%nextMissionIndex];
      }
      return %nextPotentialMission;
      %failsafe++;
   }
}

function CoreGame::cycleMissions(%game)
{
   //LogEcho("\c4CoreGame::cycleMissions(" SPC %game.class SPC ")");
   %nextMission = %game.findNextCycleMission();
   if ( %nextMission $= "" ) // Failsafe
      %nextMission = $Server::MissionFile;

   messageAll('MsgLoading', 'Loading %1 (%2)...', %nextMission, $MissionTypeDisplayName);
   tge.loadMission( %nextMission, $Server::MissionType, false );
}

//-----------------------------------------------------------------------------
// Misc game functions
//-----------------------------------------------------------------------------
function CoreGame::forceSpectator(%game, %client, %reason)
{
   //LogEcho("\c4CoreGame::forceSpectator(" SPC %game.class SPC %client.nameBase SPC %reason SPC ")");
   
   if (%client <= 0) //make sure we have a valid client...
      return;

   if(!$Game::Running) // Make sure the game has started
      return;

   if(%client.player)
      %client.player.schedule(50,"delete"); // delete player object if going spectator

   // place them in spectator mode
   %game.clearRespawnWait(%client);
   %client.lastObserverSpawn = -1;
   %client.observerStartTime = $Sim::Time;
   %adminForce = 0;

   // switch client to team 0 (spectator) and save off the last team they were on
   %client.lastTeam = %client.team;
   %client.team = 0;
   %client.notready = 1;
   %client.notReadyCount = "";

   switch$ (%reason)
   {
      case "playerChoose":
         %client.camera.getDataBlock().setMode( %client.camera, "SpectatorFly" );
         messageClient(%client, 'MsgClientJoinTeam', '\c1You have joined the %3.', %client, %client.team, %game.getTeamName(0) );
         echo(%client.nameBase@" (cl "@%client@") entered spectator mode");

      case "AdminForce":
         %client.camera.getDataBlock().setMode( %client.camera, "SpectatorFly" );
         messageClient(%client, 'MsgClientJoinTeam', '\c1You have been forced into spectator mode by the admin.', %client, %client.team, %game.getTeamName(0) );
         echo(%client.nameBase@" (cl "@%client@") was forced into spectator mode by admin");
         %adminForce = 1;

      case "spawnTimeout":
         %client.camera.getDataBlock().setMode( %client.camera, "SpectatorFly" );
         messageClient(%client, 'MsgClientJoinTeam', '\c1You have been placed in spectator mode due to delay in respawning.', %client, %client.team, %game.getTeamName(0) );
         echo(%client.nameBase@" (cl "@%client@") was placed in spectator mode due to spawn delay");
   }

   %client.setControlObject( %client.camera ); // set their control to the obs. cam

   // display the hud and clear any previous prints
   clearBottomPrint(%client);
   clearCenterPrint(%client);
   //commandToClient(%client, 'setHudMode', 'Spectator'); // #investigate if this is necessary or could be used as a feature
   updateSpectatorHud(%client);

   // message everyone about this event
   if(!%adminForce)
      messageAllExcept(%client, -1, 'MsgClientJoinTeam', '\c2%4 has become a %3.', %client, %client.team, %game.getTeamName(0), %client.playerName );
   else
      messageAllExcept(%client, -1, 'MsgClientJoinTeam', '\c2The admin has forced %4 to become an spectator.', %client, %client.team, %game.getTeamName(0), %client.playerName );

   %game.onClientBecomeSpectator(%client);
}

function CoreGame::onClientBecomeSpectator(%game, %client)
{
   serverCmdleaveFireTeam(%client); // Leave fire team if any..
   %game.clearClientVaribles(%client); //reset scores for spectators, they do not deserve scores

   for(%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %cl = ClientGroup.getObject(%i);
      messageClient(%client, 'MsgClientScoreChanged', "", %cl, %cl.score, %cl.kills, %cl.deaths, %cl.suicides, %cl.teamKills);
   }

   // Setup objective hud
   messageClient(%client, 'MsgYourScoreIs', "", 0);
   messageClient(%client, 'MsgYourDeaths', "", 0);
   messageClient(%client, 'MsgYourKills', "", 0);
}

function CoreGame::cancelCountdown(%game)
{
   if(%game.sixtyCount !$= "")
      cancel(%game.sixtyCount);
   if(%game.thirtyCount !$= "")
      cancel(%game.thirtyCount);
   if(%game.fifteenCount !$= "")
      cancel(%game.fifteenCount);
   if(%game.tenCount !$= "")
      cancel(%game.tenCount);
   if(%game.fiveCount !$= "")
      cancel(%game.fiveCount);
   if(%game.fourCount !$= "")
      cancel(%game.fourCount);
   if(%game.threeCount !$= "")
      cancel(%game.threeCount);
   if(%game.twoCount !$= "")
      cancel(%game.twoCount);
   if(%game.oneCount !$= "")
      cancel(%game.oneCount);

   cancel(%game.matchStart);
   
   %game.matchStart = "";
   %game.thirtyCount = "";
   %game.fifteenCount = "";
   %game.tenCount = "";
   %game.fiveCount = "";
   %game.fourCount = "";
   %game.threeCount = "";
   %game.twoCount = "";
   %game.oneCount = "";

   $countdownStarted = false;
}

function CoreGame::cancelEndCountdown(%game)
{
   //cancel the mission end countdown...
   if(%game.endsixtyCount !$= "")
      cancel(%game.endsixtyCount);
   if(%game.endthirtyCount !$= "")
      cancel(%game.endthirtyCount);
   if(%game.endtenCount !$= "")
      cancel(%game.endtenCount);
   if(%game.endfiveCount !$= "")
      cancel(%game.endfiveCount);
   if(%game.endfourCount !$= "")
      cancel(%game.endfourCount);
   if(%game.endthreeCount !$= "")
      cancel(%game.endthreeCount);
   if(%game.endtwoCount !$= "")
      cancel(%game.endtwoCount);
   if(%game.endoneCount !$= "")
      cancel(%game.endoneCount);
   
   %game.endmatchStart = "";
   %game.endthirtyCount = "";
   %game.endtenCount = "";
   %game.endfiveCount = "";
   %game.endfourCount = "";
   %game.endthreeCount = "";
   %game.endtwoCount = "";
   %game.endoneCount = "";
}

// Returns number of client objects on team
function CoreGame::getTeamClientCount(%game, %team)
{
   // Return client total for specified team
   %clientCount = 0;
   %count = ClientGroup.getCount();
   for ( %i = 0; %i < %count; %i++ )
   {
      %client = ClientGroup.getObject(%i);
      if ( %client.team == %team )
         %clientCount++;
   }
   return %clientCount;
}

// Returns number of player objects live or dead on team
function CoreGame::getTeamPlayerCount(%game, %team)
{
   %playerCount = 0;
   %count = ClientGroup.getCount();
   for ( %i = 0; %i < %count; %i++ )
   {
      %client = ClientGroup.getObject(%i);
      if ( %client.team == %team )
      {
         if ( isObject( %client.player ) )
            %playerCount++;
      }
   }
   return %playerCount;
}

// Returns the team with the most clients
function CoreGame::getLargestTeam(%game)
{
   %count = ClientGroup.getCount();
   for ( %i = 0; %i < %count; %i++ )
   {
      %client = ClientGroup.getObject( %i );
      if ( %client.team == 0 )
         continue;

      %teamClientCount[%client.team]++;
   }
   %largestPlayers = %teamClientCount[1];
   %largestTeam = 1;

   for(%j = 1; %j <= %game.numTeams; %j++)
   {
      if ( %teamClientCount[%j] > %largestPlayers || %teamClientCount[%j] == %largestPlayers )
      {
         %largestTeam = %j;
         %largestPlayers = %teamClientCount[%j];
      }
   }

   return( %largestTeam );
}

// Returns the highest scorer
function CoreGame::findTopScorer(%game)
{
   %score = 0;
   %tie = 0;
   for(%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %cl = ClientGroup.getObject(%i);
      if(%cl.score > %score)
      {
         %score = %cl.score;
         %top = %cl;
      }
      else if(%cl.score == %score)
         %tie++;
   }
   error("Top Scorer:" SPC %top.nameBase SPC "Tie:" SPC %tie);
   return( %top SPC %tie );
}

function CoreGame::findTopKiller(%game)
{
   %kills = 0;
   %tie = 0;
   for(%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %cl = ClientGroup.getObject(%i);
      if(%cl.kills > %kills)
      {
         %kills = %cl.kills;
         %top = %cl;
      }
      else if(%cl.kills == %kills)
         %tie++;
   }
   error("Top Gun:" SPC %top.nameBase SPC "Tie:" SPC %tie);
   return( %top SPC %tie );
}

//-----------------------------------------------------------------------------
// Score hud
//-----------------------------------------------------------------------------
function CoreGame::updateScoreHud(%game, %client)
{
   if(%game.numTeams > 1)
      %list = $pref::Server::teamName[1] TAB $teamScore[1] TAB $pref::Server::teamName[2] TAB $teamScore[2];
   else
      %list = %client.nameBase TAB %client.score;

   messageClient(%client, 'SetTeamScores', "", addTaggedString(%list), %game.numTeams);
}

//-----------------------------------------------------------------------------
// Camera functions
//-----------------------------------------------------------------------------
function CoreGame::SpectatorOnTrigger(%game, %data, %obj, %trigger, %state, %client)
{
   //LogEcho("\c4CoreGame::SpectatorOnTrigger(" SPC %game.class SPC %data.getName() SPC %obj.mode SPC %trigger SPC %state SPC %client.nameBase SPC ")");
   switch$ (%obj.mode)
   {
      case "NewConnect":
         switch(%trigger)
         {
            case 0:
               clearBottomPrint(%client);

               //assign a team and spawn the player
               commandToClient(%client, 'setHudMode', 'Play');
               %game.assignClientTeam(%client, 0);
               %game.spawnPlayer(%client, 0);

               if ( $Game::Running )
               {
                  %obj.setFlyMode();
               }

            case 2: //switch the spectator mode to spectating clients
               if (isObject(%client.observeFlyClient))
                  observeClient(%client, %client.observeFlyClient);
               else
                  observeClient(%client, -1);

               spectatorFollowUpdate(%client, %client.observeClient, false);
               displaySpectatorHud(%client, %client.observeClient);
               messageClient(%client.observeClient, 'Observer', '\c1%1 is now observing you.', %client.playerName);

            case 3: // cycle throw the static spectator spawn points
               %markerObj = Game.pickObserverSpawn(%client, true);
               %transform = %markerObj.getTransform();
               %obj.setTransform(%transform);
               %obj.setFlyMode();
         }

      case "SpectatorFly": // Free-flying observer camera
         switch(%trigger)
         {
            case 0:
               if ( !$pref::Server::TournamentMode && $Game::Running )
               {
                  clearBottomPrint(%client);
                  commandToClient(%client, 'setHudMode', 'Play');
                  if( %client.lastTeam !$= "" && %client.lastTeam != 0 && %game.numTeams > 1)
                  {
                     %game.clientJoinTeam(%client, %client.lastTeam, 1); 
                     %obj.setFlyMode();
                  }
                  else
                  {
                     //assign a team and spawn the player
                     %game.assignClientTeam(%client, 0);
                     %game.spawnPlayer(%client, 0);
                     %obj.setFlyMode();
                  }
               }
               else if ( !$pref::Server::TournamentMode )
               {
                  clearBottomPrint(%client);
                  %game.assignClientTeam(%client, 0);
                  %game.spawnPlayer(%client, 0);
               }
 
            case 2: //switch the spectator mode to spectating clients
               if (isObject(%client.observeFlyClient))
                  observeClient(%client, %client.observeFlyClient);
               else
                  observeClient(%client, -1);

               spectatorFollowUpdate(%client, %client.observeClient, false);
               displaySpectatorHud(%client, %client.observeClient);
               messageClient(%client.observeClient, 'Observer', '\c1%1 is now observing you.', %client.playerName);

            case 3: // cycle throw the static spectator spawn points
               %markerObj = Game.pickObserverSpawn(%client, true);
               %transform = %markerObj.getTransform();
               %obj.setTransform(%transform);
               %obj.setFlyMode();
         }

      case "Corpse": // Viewing dead corpse, so we probably want to respawn.
         if(!%client.respawnWait) // Wait for player to be deleted, clones are baaad.
         {
            commandToClient(%client, 'setHudMode', 'Play');
            %game.spawnPlayer(%client, 1);
            %obj.setFlyMode();
         }

      case "spectatorFollow":
         switch(%trigger)
         {
            case 0: //press FIRE - cycle to next client
               %nextClient = findNextObserveClient(%client);
               %prevObsClient = %client.observeClient;
               if (%nextClient > 0 && %nextClient != %client.observeClient)
               { 
                  spectatorFollowUpdate(%client, %nextClient, true); // update the observer list for this client
               
                  //set the new object
                  %transform = %nextClient.player.getTransform();
                  if(!%nextClient.isMounted())
                  {
                     %params = %nextClient.player.getDataBlock().observeParameters;
                     %obj.setOrbitMode(%nextClient.player, %transform, getWord(%params, 0), getWord(%params, 1), getWord(%params, 2));
                     %client.observeClient = %nextClient;
                  }
                  else
                  {
                     %mount = %nextClient.player.getObjectMount();
                     if(%mount.getDataBlock().observeParameters $= "")
                        %params = %transform;
                     else
                        %params = %mount.getDataBlock().observeParameters;

                     %obj.setOrbitMode(%mount, %mount.getTransform(), getWord(%params, 0), getWord(%params, 1), getWord(%params, 2));
                     %client.observeClient = %nextClient;
                  }
                  //send the message(s)
                  displaySpectatorHud(%client, %nextClient);
                  messageClient(%nextClient, 'Observer', '\c1%1 is now observing you.', %client.playerName);  
                  messageClient(%prevObsClient, 'ObserverEnd', '\c1%1 is no longer observing you.', %client.playerName);  
               }

            case 2: //press JUMP
               // update the observer list for this client
               spectatorFollowUpdate(%client, -1, false);
               //toggle back to observer fly mode
               %obj.mode = "SpectatorFly";
               %obj.setFlyMode();
               updateSpectatorHud(%client);
               messageClient(%client.observeClient, 'ObserverEnd', '\c1%1 is no longer observing you.', %client.playerName);  

            case 3: //press JET - cycle to prev client
               %prevClient = findPrevObserveClient(%client);
               %prevObsClient = %client.observeClient;
               if (%prevClient > 0 && %prevClient != %client.observeClient)
               {
                  // update the observer list for this client
                  spectatorFollowUpdate(%client, %prevClient, true);
               
                  //set the new object
                  %transform = %prevClient.player.getTransform();
                  if(!%prevClient.isMounted())
                  {
                     %params = %prevClient.player.getDataBlock().observeParameters;
                     %obj.setOrbitMode(%prevClient.player, %transform, getWord(%params, 0), getWord(%params, 1), getWord(%params, 2));
                     %client.observeClient = %prevClient;
                  }
                  else
                  {
                     %mount = %prevClient.player.getObjectMount();
                     if(%mount.getDataBlock().observeParameters $= "")
                        %params = %transform;
                     else
                        %params = %mount.getDataBlock().observeParameters;
            
                     %obj.setOrbitMode(%mount, %mount.getTransform(), getWord(%params, 0), getWord(%params, 1), getWord(%params, 2));
                     %client.observeClient = %prevClient;
                  }
                  //send the message(s)
                  displaySpectatorHud(%client, %prevClient);
                  messageClient(%prevClient, 'Observer', '\c1%1 is now observing you.', %client.playerName);  
                  messageClient(%prevObsClient, 'ObserverEnd', '\c1%1 is no longer observing you.', %client.playerName);  
               }
         }

      case "pre-game":
         switch(%trigger)
         {
            case 0:
               if ( !$pref::Server::TournamentMode || $CountdownStarted || $Game::Running )
                  return;

               if ( %client.notReady )
               {
                  %client.notReady = "";
                  MessageAll( 0, '\c1%1 is READY.', %client.playerName );
                  if(%client.notReadyCount < 3)
                     centerprint( %client, "\nWaiting for Match start (FIRE if not ready)", 0, 3);
                  else 
                     centerprint( %client, "\nWaiting for Match start", 0, 3);
               }
               else
               {
                  %client.notReadyCount++;
                  if ( %client.notReadyCount < 4 )
                  {
                     %client.notReady = true;
                     MessageAll( 0, '\c1%1 is not READY.', %client.playerName );
                     centerprint( %client, "\nPress FIRE when ready.", 0, 3 );
                  }
                  return;
               }
               %game.checkTournamentStart();
         }
   }
}

function CoreGame::SpectatorSetMode(%game, %data, %obj, %mode, %targetObj)
{
   //LogEcho("\c4CoreGame::SpectatorSetMode(" SPC %game.class SPC %data.getName() SPC %obj SPC %mode SPC %targetObj SPC ")");

   %client = %obj.getControllingClient();
   switch$ (%mode)
   {
      case "NewConnect": // Free-flying spectator camera basically
         displaySpectatorHud(%client, 0);
         commandToClient(%client, 'setHudMode', 'Spectator');
         %markerObj = %game.pickObserverSpawn(%client, true);
         %transform = %markerObj.getTransform();
         %obj.setTransform(%transform);
         %obj.setFlyMode();

      case "SpectatorFly": // Free-flying spectator camera
         displaySpectatorHud(%client, 0);
         commandToClient(%client, 'setHudMode', 'Spectator');
         %markerObj = %game.pickObserverSpawn(%client, true);
         %transform = %markerObj.getTransform();
         %obj.setTransform(%transform);
         %obj.setFlyMode();

      case "pre-game":
         commandToClient(%client, 'setHudMode', 'Spectator');
         // Clear any prints
         clearBottomPrint( %client );
         %params = %targetObj.getDataBlock().observeParameters;
         %obj.setOrbitMode( %targetObj, %targetObj.getTransform(), firstWord(%params), getWord(%params, 1), getWord(%params, 2), 1);

      case "spectatorFollow": // Observer attached to a moving object assume its a player
         %transform = %targetObj.getTransform();
         if( !%targetObj.isMounted() )
         {
            %params = %targetObj.getDataBlock().observeParameters;
            %obj.setOrbitMode(%targetObj, %transform, getWord(%params, 0), getWord(%params, 1), getWord(%params, 2));
         }
         else
         {
            %mount = %targetObj.getObjectMount();
            if( %mount.getDataBlock().observeParameters $= "" )
               %params = %transform;
            else
               %params = %mount.getDataBlock().observeParameters;

            %obj.setOrbitMode(%mount, %mount.getTransform(), getWord(%params, 0), getWord(%params, 1), getWord(%params, 2));
         }

      case "Corpse":
         // Lock the camera down in orbit around the corpse, which should be targetObj
         commandToClient(%client, 'setHudMode', 'Corpse');
         %transform = %targetObj.getTransform();
         %pos = posFromTransform(%transform);
         //%params = %targetObj.getDataBlock().observeParameters; // #investigage if this can be used instead the fixed values below
         %params = "0.5 8 1";
         %obj.setOrbitMode(%targetObj, %pos SPC "1 0 0 0", firstWord(%params), getWord(%params, 1), getWord(%params, 2), 1);
   }
   %obj.mode = %mode;
}

//-----------------------------------------------------------------------------
// Trigger functions
//-----------------------------------------------------------------------------
function CoreGame::onEnterTrigger(%game, %data, %obj, %colobj)
{
   //Do Nothing
}

function CoreGame::onLeaveTrigger(%game, %data, %obj, %colobj)
{
   //Do Nothing
}

function CoreGame::onTickTrigger(%game, %data, %obj)
{
   //Do Nothing
}

//-----------------------------------------------------------------------------
// Voting functions
//-----------------------------------------------------------------------------
function CoreGame::sendPlayerVoteMenu(%game, %client, %targetClient, %key)
{
   //LogEcho("\c4CoreGame::sendPlayerVoteMenu("@%game.class @", "@ %client.nameBase @", "@ %targetClient.nameBase @", "@ %key @")");
   if(!%targetClient.matchStartReady)
      return;

   %isAdmin = ( %client.isAdmin ?  1 : 0 );
   %isTargetSelf = ( %client == %targetClient ?  1 : 0 );
   %isTargetAdmin = ( %targetClient.isAdmin ?  1 : 0 );
   %isTargetBot = %targetClient.isAIControlled();
   %isTargetSpectator = ( %targetClient.team == 0 ? 1: 0 );
   %outrankTarget = false;

   if ( %client.isSuperAdmin )
      %outrankTarget = !%targetClient.isSuperAdmin;
   else if ( %client.isAdmin )
      %outrankTarget = !%targetClient.isAdmin;
   else
      %outrankTarget = false;

   if(!%isTargetSelf)
   {
      if(%client.muted[%targetClient])
         messageClient(%client, 'MsgPlayerPopupItem', "", %key, "MutePlayer", "", 'Unmute Text Chat', 1);
      else
         messageClient(%client, 'MsgPlayerPopupItem', "", %key, "MutePlayer", "", 'Mute Text Chat', 1);

      messageClient( %client, 'MsgPlayerPopupItem', "", %key, "SendMessage", "", 'Send Private Message', 15 );

      // Fire team
      messageClient( %client, 'MsgPlayerPopupItem', "", %key, "InviteToFireteam", "", 'Invite to Fire Team', 16 );
      messageClient( %client, 'MsgPlayerPopupItem', "", %key, "KickFromFireteam", "", 'Kick from Fire Team', 17 );
   }
   else
   {
      // Fire team
      messageClient( %client, 'MsgPlayerPopupItem', "", %key, "LeaveFireteam", "", 'Leave Fire Team', 18 );
   }

   if(!%client.canVote && !%isAdmin)
      return;

   // regular vote options on players
   if(%game.scheduleVote $= "" && !%isAdmin && !%isTargetAdmin)
   {
      if($pref::Server::AllowPlayerVoteAdmin)
         messageClient(%client, 'MsgPlayerPopupItem', "", %key, "VoteAdminPlayer", "", 'Vote to Make Admin', 2);

      if(!%isTargetSelf)
      {
         messageClient(%client, 'MsgPlayerPopupItem', "", %key, "VoteKickPlayer", "", 'Vote to Kick', 3);
      }
   }

   // Admin only options on players:
   if(%isAdmin)
   {
      if(%client.isSuperAdmin && %targetClient.guid != 0)
      {
         messageClient(%client, 'MsgPlayerPopupItem', "", %key, "addAdmin", "", 'Add to Server Admin List', 9);
         messageClient(%client, 'MsgPlayerPopupItem', "", %key, "addSuperAdmin", "", 'Add to Server SuperAdmin List', 10);
      }

      if( !%isTargetAdmin )
         messageClient(%client, 'MsgPlayerPopupItem', "", %key, "VoteAdminPlayer", "", 'Make Admin', 2);

      if(!%isTargetSelf && %outrankTarget)
      {
         messageClient(%client, 'MsgPlayerPopupItem', "", %key, "VoteKickPlayer", "", 'Kick', 3);

         messageClient(%client, 'MsgPlayerPopupItem', "", %key, "Warn", "", 'Warn player', 11);
         if(%isTargetAdmin)
            messageClient(%client, 'MsgPlayerPopupItem', "", %key, "StripAdmin", "", 'Strip admin', 12);

         if (!%isTargetSpectator)
            messageClient( %client, 'MsgPlayerPopupItem', "", %key, "ToSpectator", "", 'Force spectator', 5 );

         messageClient(%client, 'MsgPlayerPopupItem', "", %key, "PrintClientInfo", "", 'Client Info', 13);
         if(%client.isSuperAdmin)
         {
            messageClient(%client, 'MsgPlayerPopupItem', "", %key, "VoteBanPlayer", "", 'Ban', 4);

            if(%targetClient.isGagged)
               messageClient(%client, 'MsgPlayerPopupItem', "", %key, "UnGagPlayer", "", 'UnGag Player', 14);
            else
               messageClient(%client, 'MsgPlayerPopupItem', "", %key, "GagPlayer", "", 'Gag Player', 14);
         }
      }

      if(%isTargetSelf || %outrankTarget)
      {
         if(%isTargetAdmin)
            messageClient(%client, 'MsgPlayerPopupItem', "", %key, "StripAdmin", "", 'Strip admin', 12);

         if(%game.numTeams > 1)
         {   
            if (%isTargetSpectator)
            {
               %action = %isTargetSelf ? "Join" : "Change to";
               %str1 = %action SPC $pref::Server::teamName[1];
               %str2 = %action SPC $pref::Server::teamName[2];

               messageClient(%client, 'MsgPlayerPopupItem', "", %key, "ChangeTeam", "", addTaggedString(%str1), 6);
               messageClient(%client, 'MsgPlayerPopupItem', "", %key, "ChangeTeam", "", addTaggedString(%str2), 7);
            }
            else
            {
               %changeTo = %targetClient.team == 1 ? 2 : 1;   
               %str = "Switch to " @ $pref::Server::teamName[%changeTo];
               %caseId = 5 + %changeTo;

               messageClient(%client, 'MsgPlayerPopupItem', "", %key, "ChangeTeam", "", addTaggedString(%str), %caseId);
            }
         }
         else if(%isTargetSpectator)
         {
            %str = %isTargetSelf ? 'Join the Game' : 'Add to Game';
            messageClient(%client, 'MsgPlayerPopupItem', "", %key, "JoinGame", "", %str, 8);
         }
      }
   }
}

function CoreGame::sendServerVoteMenu(%game, %client, %key)
{
   //LogEcho("\c4CoreGame::sendServerVoteMenu("@%game.class @", "@ %client.nameBase @", "@ %key @")");

   // This function fills the ESC_VoteMenu in the AdminDlg
   %isAdmin = (%client.isAdmin || %client.isSuperAdmin);
   %multipleTeams = %game.numTeams > 1;

   // Fire team
   if ( %client.team != 0 && !isObject( %client.fireTeam ) )
   {
      messageClient(%client, 'MsgVoteItem', "", %key, 'CreateFireteam', 'Create a Fire Team');
      messageClient(%client, 'MsgVoteItem', "", %key, 'JoinFireteam', 'Join a Fire Team');
   }

   if ( !%client.canVote && !%isAdmin )
      return;

   if ( %game.scheduleVote $= "" )
   {
      if ( !%client.isAdmin )
      {
         // Actual vote options:
         messageClient(%client, 'MsgVoteItem', "", %key, 'VoteChangeMission', 'Vote to Change the Mission');
         messageClient(%client, 'MsgVoteItem', "", %key, 'VoteSkipMission', 'Vote to Skip Mission');
         messageClient(%client, 'MsgVoteItem', "", %key, 'VoteAddBots', 'Vote to add Bots');
         messageClient(%client, 'MsgVoteItem', "", %key, 'VoteKickAllBots', 'Vote to kick all Bots');

         if ( $pref::Server::TournamentMode )
         {   
            messageClient(%client, 'MsgVoteItem', "", %key, 'VoteTournamentMode', 'Vote Free For All Mode');
            
            if ( !$Game::Running && !$CountdownStarted )
               messageClient(%client, 'MsgVoteItem', "", %key, 'VoteMatchStart', 'Vote to Start the Match');
         }
         else
            messageClient(%client, 'MsgVoteItem', "", %key, 'VoteTournamentMode', 'Vote Tournament Mode');

         if ( %multipleTeams )
         {
            if ( $FriendlyFire )
               messageClient(%client, 'MsgVoteItem', "", %key, 'VoteFriendlyFire', 'Vote Disable Friendly Fire');
            else
               messageClient(%client, 'MsgVoteItem', "", %key, 'VoteFriendlyFire', 'Vote Enable Friendly Fire');

            if ( $BaseSacking )
               messageClient(%client, 'MsgVoteItem', "", %key, 'VoteBaseSacking', 'Vote Enable Protected Objects');
            else
               messageClient(%client, 'MsgVoteItem', "", %key, 'VoteBaseSacking', 'Vote Disable Protected Objects');
         }
      }
      else
      {
         // Actual vote options:
         messageClient(%client, 'MsgVoteItem', "", %key, 'VoteChangeMission', 'Change the Mission');
         messageClient(%client, 'MsgVoteItem', "", %key, 'VoteSkipMission', 'Skip the Mission');
         messageClient(%client, 'MsgVoteItem', "", %key, 'VoteAddBots', 'Add Bots');
         messageClient(%client, 'MsgVoteItem', "", %key, 'VoteKickAllBots', 'Kick all Bots');

         if ( $pref::Server::TournamentMode )
         {
            messageClient(%client, 'MsgVoteItem', "", %key, 'VoteTournamentMode', 'Free For All Mode');

            if(!$Game::Running && !$CountdownStarted)
               messageClient(%client, 'MsgVoteItem', "", %key, 'VoteMatchStart', 'Start Match');
         }
         else
            messageClient(%client, 'MsgVoteItem', "", %key, 'VoteTournamentMode', 'Tournament Mode');

         if ( %multipleTeams )
         {
            if ( $FriendlyFire )
               messageClient(%client, 'MsgVoteItem', "", %key, 'VoteFriendlyFire', 'Disable Friendly Fire');
            else
               messageClient(%client, 'MsgVoteItem', "", %key, 'VoteFriendlyFire', 'Enable Friendly Fire');

            if ( $BaseSacking )
               messageClient(%client, 'MsgVoteItem', "", %key, 'VoteBaseSacking', 'Enable Protected Objects');
            else
               messageClient(%client, 'MsgVoteItem', "", %key, 'VoteBaseSacking', 'Disable Protected Objects');
         }
      }
   }

   // Admin only options:
   if ( %client.isAdmin )
   {
      messageClient(%client, 'MsgVoteItem', "", %key, 'VoteChangeTimeLimit', 'Change the Time Limit');
      messageClient(%client, 'MsgVoteItem', "", %key, 'VoteResetServer', 'Reset the Server');
      messageClient(%client, 'MsgVoteItem', "", %key, 'VoteClearServer', 'Clear the Server');
      messageClient(%client, 'MsgVoteItem', "", %key, 'VoteCancelVote', 'Cancel Vote');
   }
}

$Vote::DisallowVote["VoteAdminPlayer"] = $pref::Server::DisallowVoteAdmin;
$Vote::DisallowVote["VoteChangeMission"] = $pref::Server::DisallowVoteMission;
$Vote::DisallowVote["VoteSkipMission"] = $pref::Server::DisallowVoteSkipMission;
$Vote::DisallowVote["VoteFriendlyFire"] = $pref::Server::DisallowVoteFriendlyFire;
$Vote::DisallowVote["VoteBaseSacking"] = $pref::Server::DisallowVoteBaseSacking;
$Vote::DisallowVote["VoteTournamentMode"] = $pref::Server::DisallowVoteServerMode;
$Vote::DisallowVote["VoteMatchStart"] = $pref::Server::DisallowVoteStartMatch;
$Vote::DisallowVote["VoteChangeTimeLimit"] = $pref::Server::DisallowVoteTimeLimit;
$Vote::DisallowVote["VoteAddBots"] = $pref::Server::DisallowAddBots;
$Vote::DisallowVote["VoteKickAllBots"] = $pref::Server::DisallowVoteKickAllBots;

$Vote::TypeToDesc["VoteAdminPlayer"] = "Admin Player";
$Vote::TypeToDesc["VoteChangeMission"] = "change the mission to";
$Vote::TypeToDesc["VoteSkipMission"] = "skip to next mission";
$Vote::TypeToDesc["VoteFriendlyFire"] = $FriendlyFire ? "disable friendly fire" : "enable friendly fire";
$Vote::TypeToDesc["VoteBaseSacking"] = $BaseSacking ? "enable protected objects" : "disable protected objects";
$Vote::TypeToDesc["VoteTournamentMode"] = $pref::Server::TournamentMode ? "change to free for all mode" : "change to tournament mode";
$Vote::TypeToDesc["VoteMatchStart"] = "start the match";
$Vote::TypeToDesc["VoteChangeTimeLimit"] = "change the time limit to";
$Vote::TypeToDesc["VoteAddBots"] = "Add bots:";
$Vote::TypeToDesc["VoteKickAllBots"] = "Kick all bots";

function CoreGame::InitVote(%game, %client, %typeName, %val1, %val2, %val3, %val4, %playerVote)
{
   //LogEcho("\c4CoreGame::InitVote("@%game.class @", "@ %client.nameBase @", "@ %typeName @", "@ %val1 @", "@ %val2 @", "@ %val3 @", "@ %val4 @", "@ %playerVote @")");
   %clientsVoting = 0;
   %adminAction = 0;

   switch$(%typeName)
   {
      case "VoteKickPlayer":
         if ( %client.isAdmin )
         {
             if ( !%outrankTarget )
                messageClient(%sender, 'MsgAdminCommand', '\c2Cannot kick %1 as you do not outrank %2.', %val1.playerName, %val1.sex $= "Male" ? 'him' : 'her');
             else
                AdminCommand(%client, %typename, %val1, %val2, %val3, %val4);
         }
         else
         {
            if ( %val1.isAdmin )
            {
               messageClient(%client, '', '\c2You can not %1 %2, %3 is a Admin!', "Kick Player", %val1.playerName, %val1.sex $= "Male" ? 'he' : 'she');
               return;
            }
            if ( %client.team != %val1.team && %val1.team != 0 )
            {
               messageClient(%client, '', "\c2Player votes must be team based.");
               return;
            }
            if ( %game.scheduleVote !$= "" )
            {
               messageClient(%client, 'voteAlreadyRunning', '\c2A vote is already in progress.');
               return;
            }
            %game.kickClient = %val1;
            %game.kickGuid = %val1.guid;
            %game.kickTeam = %val1.team;

            if ( %val1.team == 0 )
            {
               for ( %idx = 0; %idx < ClientGroup.getCount(); %idx++ )
               {
                  %cl = ClientGroup.getObject(%idx);
                  messageClient(%cl, 'VoteStarted', '\c2%1 initiated a vote to %2 %3.', %client.playerName, "Kick Player", %val1.playerName);
                  %clientsVoting++;
               }
               PlayerVote(%client, %typename, %val1, %val2, %val3, %val4, %clientsVoting);
            }
            else
            {
                for ( %idx = 0; %idx < ClientGroup.getCount(); %idx++ )
                {
                  %cl = ClientGroup.getObject(%idx);
                  if ( %cl.team == %client.team )
                  {
                     messageClient( %cl, 'VoteStarted', '\c2%1 initiated a vote to %2 %3.', %client.playerName, "Kick Player", %val1.playerName);
                     %clientsVoting++;
                  }
               }
               PlayerVote(%client, %typename, %val1, %val2, %val3, %val4, %clientsVoting, true);
            }
         }

      case "VoteBanPlayer":
         if ( %client.isSuperAdmin || (%client.isAdmin && $pref::Server::AllowAdminBan) )
         {
            if ( %client.isAdmin && %outrankTarget )
            {
               messageClient(%sender, 'MsgAdminCommand', '\c2Cannot ban %1 as you do not outrank %2.', %val1.playerName, %val1.sex $= "Male" ? 'him' : 'her');
               return;
            }
            AdminCommand(%client, %typename, %val1, %val2, %val3, %val4);
         }

      case "VoteCancelVote":
         if ( %client.isSuperAdmin || (%client.isAdmin && $pref::Server::AllowAdminCancelVote) )
         {
            AdminCommand(%client, %typename, %val1, %val2, %val3, %val4);
         }

      case "VoteResetServer":
         if ( %client.isSuperAdmin || (%client.isAdmin && $pref::Server::AllowAdminResetServer) )
         {
            AdminCommand(%client, %typename, %val1, %val2, %val3, %val4);
         }

      case "VoteClearServer":
         if ( %client.isSuperAdmin || (%client.isAdmin && $pref::Server::AllowAdminClearServer) )
         {
            if( isEventPending( $ClearSchedule ) )
               cancel($ClearSchedule);

            centerPrintAll("\n<color:ff0000><font:Arial:12>SERVER WILL BE CLEARING FOR TOURNAMENT IN 10 SECONDS.", 5, 3);
            messageAll('MsgAdminForce', '\c2The Admin %1 is clearing the server for a tournament, you will be kicked in 10 seconds.', $AdminCl.playerName);
            $ClearSchedule = schedule(10000, %game, "clearserver", %client);
         }

      default:
         if ( %client.isAdmin )
            AdminCommand(%client, %typename, %val1, %val2, %val3, %val4);
         else
         {
            if ( $Vote::DisallowVote[%typeName] )
            {
               messageClient(%client, 'VoteDisabled', '\c2This type of vote (%1) is dissallowed.', %typeName);
               return;
            }

            if ( %game.scheduleVote !$= "" )
            {
               messageClient(%client, 'voteAlreadyRunning', '\c2A vote is already in progress.');
               return;
            }

            for (%idx = 0; %idx < ClientGroup.getCount(); %idx++)
            {
               %cl = ClientGroup.getObject(%idx);

               if ( %val1 != 0 && %val1 !$="" )
                  messageClient(%cl, 'VoteStarted', '\c2%1 initiated a vote to %2 %3 ( %4 ).', %client.playerName, $Vote::TypeToDesc[%typeName], %val1, %val2);
               else
                  messageClient(%cl, 'VoteStarted', '\c2%1 initiated a vote to %2.', %client.playerName, $Vote::TypeToDesc[%typeName]);

               %clientsVoting++;
            }
            PlayerVote(%client, %typename, %val1, %val2, %val3, %val4, %clientsVoting);
         }
   }
}

function CoreGame::evalVote(%game, %client, %typeName, %admin, %val1, %val2, %val3, %val4)
{
   //LogEcho("\c4CoreGame::evalVote(" SPC %game.class SPC %client.nameBase SPC %typeName SPC %admin SPC %val1 SPC %val2 SPC %val3 SPC %val4 SPC ")");
   // %client is the admin who issued the cmd or the player who initiated the vote
   // %val1 holds true or false for if this is an admin cmd or player vote
   switch$ (%typeName)
   {
      case "VoteChangeMission":
         // Ok %val2 is the MissionTypeDisplayName. So we have to convert that to the actual gameTypeName
         // We will loop through all types and find the index of this DisplayName. Then use this to get the proper TypeName
         for( %type = 0; %type < $HostTypeCount; %type++ )
         {
            // If this returns null, Houston we got a problem!
            if($HostTypeDisplayName[%type] $= %val2) // Found the index, YEY!
               break;
         }

         if ( $HostTypeName[%type] $= "" )
         {
            error("Invalid mission type id passed to change mission!");
            return;
         }

         if ( $HostMissionFile[%val3] $= "" )
         {
            error("Invalid mission index passed to change mission!");
            return;
         }

         if ( %admin )
         {
            messageAll('MsgMissionCycle', '%1 changing mission to %2 ( %3 )', %client.playerName, %val1, $HostTypeDisplayName[%type]);
            if ( isEventPending( $Game::Schedule ) )
               cancel($Game::Schedule);

            %game.endGame();
            $Game::Cycling = false;
            tge.schedule($Game::EndGamePause, "loadMission", $HostMissionFile[%val3], $HostTypeName[%type], false);
            echo("mission changed to "@%val1@"/"@$HostTypeDisplayName[%type]@" (admin)");
         }
         else
         {
            %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
            if(%totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100))
            {
               messageAll('MsgVotePassed', '\c2The mission was changed to %1 ( %2 ) by vote.', %val1, $HostTypeDisplayName[%type]);
               if(isEventPending($Game::Schedule))
                  cancel($Game::Schedule);

               %game.endGame();
               $Game::Cycling = false;
               tge.schedule($Game::EndGamePause, "loadMission", $HostMissionFile[%val3], $HostTypeName[%type], false);
               echo("mission changed to "@%val1@"/"@$HostTypeDisplayName[%type]@" (vote)");
            }
            else
               messageAll('MsgVoteFailed', '\c2Change mission vote did not pass: %1 percent.', mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100));
         }

      case "VoteSkipMission":
         if ( %admin )
         {
            messageAll('MsgMissionCycle', '%1 cycling missions', %client.playerName);
            if ( isEventPending( $Game::Schedule ) )
               cancel($Game::Schedule);

            %game.endGame();
            $Game::Schedule = %game.schedule(250, "onCyclePauseEnd");
         }
         else
         {
            %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
            if(%totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100))
            {
               messageAll('MsgVotePassed', '\c2The mission was skipped by vote.');
               if(isEventPending($Game::Schedule))
                  cancel($Game::Schedule);

               %game.endGame();
               $Game::Schedule = %game.schedule(250, "onCyclePauseEnd");
               echo("mission changed to "@%val1@"/"@$HostTypeDisplayName[%type]@" (vote)");
            }
            else
               messageAll('MsgVoteFailed', '\c2Skip mission vote did not pass: %1 percent.', mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100));
         }

      case "VoteMatchStart":
         %cause = "";
         %ready = %game.forceMatchStart();
         if ( %admin )
         {
            if ( !%ready )
            {
               messageClient( %client, 'msgClient', '\c2No players are ready yet.');
               return;
            }
            else
            {
               messageAll('msgMissionStart', '\c2%1 has forced the match to start.', %client.playerName);
               %cause = "(admin)";
               %game.Countdown();
            }
         }
         else
         {
            if ( !%ready )
            {
               messageAll( 'msgClient', '\c2Vote passed to start match, but no players are ready yet.');
               return;
            }
            else
            {
               %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
               if(%totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100))
               {
                  messageAll('MsgVotePassed', '\c2The match has been started by vote: %1 percent.', mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100));
                  %game.Countdown();
               }
               else
                  messageAll('MsgVoteFailed', '\c2Start Match vote did not pass: %1 percent.', mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100));
            }
         }
         if ( %cause !$= "" )
            echo("start match "@%cause);

      case "VoteChangeTimeLimit":
         if( %val1 == 999 )
            %display = "unlimited";
         else
            %display = %val1;

         %cause = "";
         if ( %admin )
         {
            messageAll( 'MsgAdminForce', '\c2%1 changed the mission time limit to %2 minutes.', %client.playerName, %display );
            $pref::Server::TimeLimit = %val1;
            %cause = "(admin)";
         }
         else
         {
            %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
            if(%totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100))
            {
               messageAll('MsgVotePassed', '\c2The mission time limit was set to %1 minutes by vote.', %display);
               $pref::Server::TimeLimit = %val1;
               %cause = "(vote)";
            }
            else
               messageAll('MsgVoteFailed', '\c2The vote to change the mission time limit did not pass: %1 percent.', mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100));
         }

         //if the time limit was actually changed...
         if ( %cause !$= "" )
         {
            echo("time limit set to "@%display SPC %cause);

            //if the match has been started, reset the end of match countdown
            if ( $Game::Running )
            {
               //schedule the end of match countdown
               %elapsedTimeMS = getSimTime() - $Game::StartTime;
               %curTimeLeftMS = ($pref::Server::TimeLimit * 60 * 1000) - %elapsedTimeMS;

               //error("time limit="@$pref::Server::TimeLimit@", elapsed="@(%elapsedTimeMS / 60000)@", curtimeleftms="@%curTimeLeftMS);
               %game.CancelEndCountdown();
               %game.EndCountdown(%curTimeLeftMS);

               messageAll('MsgSyncClock', "", (%curTimeLeftMS / 1000), true, false);
               cancel($Game::Schedule);
               $Game::Schedule = %game.schedule(%curTimeLeftMS, "onGameDurationEnd");
            }
         }

      case "VoteResetServer":
         %cause = "";
         if ( %admin )
         {
            messageAll('AdminResetServer', '\c2The Admin %1 has reset the server.', %client.playerName);
            resetServerDefaults();
            %cause = "(admin)";
         }
         else
         {
            %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
            if ( %totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100) )
            {
               messageAll('MsgVotePassed', '\c2The Server has been reset by vote.' );
               resetServerDefaults();
               %cause = "(vote)";
            }
            else
               messageAll('MsgVoteFailed', '\c2The vote to reset Server to defaults did not pass: %1 percent.', mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100));
         }
         if ( %cause !$= "" )
            echo("server reset "@%cause);

      case "VoteKickPlayer":
         %cause = "";

         if ( %admin )
         {
            kick(%val1, %admin, %val1.guid );
            %cause = "(admin)";
         }
         else
         {
            %team = %client.team;
            %totalVotes = %game.votesFor[%game.kickTeam] + %game.votesAgainst[%game.kickTeam];
            if ( %totalVotes > 0 && (%game.votesFor[%game.kickTeam] / %totalVotes) > ($pref::Server::VotePassPercent / 100) )
            {
               kick(%val1, %admin, %game.kickGuid);
               %cause = "(vote)";
            }
            else
            {
               for ( %idx = 0; %idx < ClientGroup.getCount(); %idx++ )
               {
                  %cl = ClientGroup.getObject( %idx );

                  if ( %cl.team == %game.kickTeam )
                     messageClient( %cl, 'MsgVoteFailed', '\c2Kick player vote did not pass' );
               }
            }
         }

         %game.kickTeam = "";
         %game.kickGuid = "";
         %game.kickClientName = "";

         if ( %cause !$= "" )
            echo(%val1.nameBase@" (cl " @ %game.kickClient @ ") kicked " @ %cause);

      case "VoteBanPlayer":
         ban(%val1, %client);
         echo(%val1.nameBase@" (cl "@%val1@") was banned by "@%client.nameBase);

      case "VoteAdminPlayer":
         %cause = "";
         if ( %admin )
         {
            messageAll('MsgAdminPlayer', '\c2The Admin %1 made %3 an admin.', %client.playerName, %val1, %val1.playerName, 0);
            %val1.isAdmin = 1;
            %cause = "(admin)";
         }
         else
         {
            %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
            if ( %totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100) )
            {
               messageAll('MsgAdminPlayer', '\c2%3 was made an admin by vote.', %client, %val1, %val1.playerName, 0);
               %val1.isAdmin = 1;
               %cause = "(vote)";
            }
            else
               messageAll('MsgVoteFailed', '\c2Vote to make %1 an admin did not pass.', %val.playerName);
         }
         if ( %cause !$= "" )
            echo(%client.nameBase@" (cl "@%val1@") made admin "@%cause);

      case "VoteCancelVote":
         if ( %game.scheduleVote !$= "" )
         {
            messageAll('MsgVoteFailed', '\c2The admin %1 has cancelled the vote.', %client.playerName);

            clearVotes();
            messageAll('closeVoteHud', "");
            cancel(Game.scheduleVote);
            %game.scheduleVote = "";
            for( %i = 0; %i < ClientGroup.getCount(); %i++ )
            {
               %cl = ClientGroup.getObject(%i);
               resetVotePrivs(%cl);
            }
         }

      case "VoteFriendlyFire":
         %cause = "";
         %setto = "";
         if ( %admin ) 
         {
            if ( $FriendlyFire )
            {
               messageAll('MsgAdminForce', '\c2%1 has disabled friendly fire.', %client.playerName);   
               $pref::Server::FriendlyFire = $FriendlyFire = 0;
               %setto = "disabled";
            }
            else 
            {
               messageAll('MsgAdminForce', '\c2%1 has enabled friendly fire.', %client.playerName);   
               $pref::Server::FriendlyFire = $FriendlyFire = 1;
               %setto = "enabled";
            }
            %cause = "(admin)";
         }
         else 
         {
            %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
            if ( %totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100) )
            {
               if ( $FriendlyFire ) 
               {
                  messageAll('MsgVotePassed', '\c2Friendly fire was disabled by vote.'); 
                  $pref::Server::FriendlyFire = $FriendlyFire = 0;
                  %setto = "disabled";
               }
               else 
               {
                  messageAll('MsgVotePassed', '\c2Friendly fire was enabled by vote.');  
                  $pref::Server::FriendlyFire = $FriendlyFire = 1;
                  %setto = "enabled";
               }
               %cause = "(vote)";
            }
            else 
            {
               %pct = mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100);
               if ( $FriendlyFire )
                  messageAll('MsgVoteFailed', '\c2Disable friendly fire vote did not pass: %1 percent.', %pct);  
               else 
                  messageAll('MsgVoteFailed', '\c2Enable friendly fire vote did not pass: %1 percent.', %pct);   
            }
         }
         if ( %setto !$= "" )
            echo("Friendly fire "@%setto SPC %cause);

      case "VoteBaseSacking":
         %cause = "";
         %setto = "";
         if ( %admin ) 
         {
            if ( $BaseSacking )
            {
               messageAll('MsgAdminForce', '\c2%1 has enabled protected objects.', %client.playerName);   
               $pref::Server::BaseSacking = $BaseSacking = 0;
               %setto = "disabled";
            }
            else 
            {
               messageAll('MsgAdminForce', '\c2%1 has disabled protected objects.', %client.playerName);   
               $pref::Server::BaseSacking = $BaseSacking = 1;
               %setto = "enabled";
            }
            %cause = "(admin)";
         }
         else 
         {
            %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
            if ( %totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100))
            {
               if ( $BaseSacking ) 
               {
                  messageAll('MsgVotePassed', '\c2Protected objects were enabled by vote.'); 
                  $pref::Server::BaseSacking = $BaseSacking = 0;
                  %setto = "disabled";
               }
               else 
               {
                  messageAll('MsgVotePassed', '\c2Protected objects were disabled by vote.');  
                  $pref::Server::BaseSacking = $BaseSacking = 1;
                  %setto = "enabled";
               }
               %cause = "(vote)";
            }
            else 
            {
               %pct = mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100);
               if ( $BaseSacking )
                  messageAll('MsgVoteFailed', '\c2Enable protected objects vote did not pass: %1 percent.', %pct);  
               else 
                  messageAll('MsgVoteFailed', '\c2Disable protected objects vote did not pass: %1 percent.', %pct);   
            }
         }
         if ( %setto !$= "" )
            echo("Base Sacking "@%setto SPC %cause);

      case "VoteTournamentMode":
         for( %type = 0; %type < $HostTypeCount; %type++ )
         {
            // If this returns null, Houston we got a problem!
            if ( $HostTypeDisplayName[%type] $= %val2 ) // Found the index, YEY!
               break;
         }

         if ( $HostTypeName[%type] $= "" )
         {
            error("Invalid mission type id passed to change mission!");
            return;
         }

         if ( $HostMissionFile[%val3] $= "" )
         {
            error("Invalid mission index passed to change mission!");
            return;
         }

         %setting = $pref::Server::TournamentMode ? "Free for all" : "Tournament"; // For message.. What is it now?
         if ( %admin )
         {

            messageAll( 'MsgAdminForce', '\c2%1 has switched the server mode to %2 %3 ( %4 ).', %client.playerName, %setting, %val1, $HostTypeDisplayName[%type]);
            $pref::Server::TournamentMode = $pref::Server::TournamentMode ? 0 : 1;
            if ( isEventPending( $Game::Schedule ) )
               cancel($Game::Schedule);

            %game.endGame();
            $Game::Cycling = false;
            tge.schedule($Game::EndGamePause, "loadMission", $HostMissionFile[%val3], $HostTypeName[%type], false);
         }
         else
         {
            %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
            if ( %totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100) )
            {
               messageAll('MsgVotePassed', '\c2Server mode switched to %1 mode %2 ( %3 ) by vote.', %setting, %val1, $HostTypeDisplayName[%type]);
               if ( isEventPending( $Game::Schedule ) )
                  cancel($Game::Schedule);

               %game.endGame();
               $Game::Cycling = false;
               tge.schedule($Game::EndGamePause, "loadMission", $HostMissionFile[%val3], $HostTypeName[%type], false);
            }
            else
               messageAll('MsgVoteFailed', '\c2%1 mode vote did not pass: %2 percent.', %setting, mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100));
         }
		 
      case "VoteAddBots":
	     %display = %val1;
		 %playersTotal = $Server::PlayerCount + $Server::BotCount;
		 
         if ( %admin )
         {	
            if (%playersTotal >= $pref::Server::MaxPlayers) 
			{
		       messageClient( %client, 'MsgVoteFailed', '\c2Max Bots reached, try lower number.' );  
	              return; 	
			}			   
			messageAll( 'MsgAdminForce', '\c2%1 added %2 bots.', %client.playerName, %display );
            connectBots(%val1);
            $pref::Server::AiCount = ( $pref::Server::AiCount + (%val1) );
			
			   if($pref::Server::AiCount > $pref::Server::MaxPlayers - 1)
            $pref::Server::AiCount = $pref::Server::MaxPlayers - 1;
         }
         else
         {
            %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
            if ( %totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100) )
            {
		       if (%playersTotal >= $pref::Server::MaxPlayers) 
			   {
		          messageClient( %client, 'MsgVoteFailed', '\c2Max Bots reached, try lower number.' );  
	                 return; 	
			   }
               messageAll('MsgVotePassed', '\c2Vote passed to add %1 bots.' );
			   connectBots(%val1);
               $pref::Server::AiCount = ( $pref::Server::AiCount + (%val1) );
			   
			      if($pref::Server::AiCount > $pref::Server::MaxPlayers - 1)
               $pref::Server::AiCount = $pref::Server::MaxPlayers - 1;
            }
            else
               messageAll('MsgVoteFailed', '\c2The vote to add %1 bots did not pass: %1 percent.', mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100));
         }
		 
      case "VoteKickAllBots":
         if ( %admin )
         {
			messageAll( 'MsgAdminForce', '\c2%1 kicked all bots.', %client.playerName );
            kickAllBots();
            $pref::Server::AiCount = 0;
         }
         else
         {
            %totalVotes = %game.totalVotesFor + %game.totalVotesAgainst;
            if ( %totalVotes > 0 && (%game.totalVotesFor / (ClientGroup.getCount() - $Server::BotCount)) > ($pref::Server::VotePassPercent / 100) )
            {
               messageAll('MsgVotePassed', '\c2Vote passed to kick all bots.' );
               kickAllBots();
               $pref::Server::AiCount = 0;
            }
            else
               messageAll('MsgVoteFailed', '\c2The vote to kick all bots did not pass: %1 percent.', mFloor(%game.totalVotesFor/(ClientGroup.getCount() - $Server::BotCount) * 100));
         }
   }
}

//-----------------------------------------------------------------------------
// Scoring
//-----------------------------------------------------------------------------
function CoreGame::vehicleDestroyed(%game, %obj, %destroyer)
{
   //warn("CoreGame::vehicleDestroyed(" SPC %game.class @", "@ %obj.getDataBlock().getName() @", "@ %destroyer.client.nameBase SPC ")");
   %dataName = %obj.getDataBlock().getName();
   %shapeName = addTaggedString( %obj.getShapeName() );

   if ( isObject( %destroyer.client ) )
   {
      if ( %destroyer.team != %obj.team )
      {
         %destroyer.client.vehicleDestroys++;
         messageTeamExcept(%destroyer.client, 'MsgDestroyed', '\c0%1 destroyed an enemy %2', %destroyer.client.playerName, %shapeName);
         messageClient(%destroyer.client, %msgType, '\c0You received a %1 point bonus for destroying an enemy %2', %game.SCORE_PER_DESTROY_VEHICLE, %shapeName);
         %game.updateScore(%destroyer.client);
      }
   }
}

function CoreGame::staticShapeDestroyed(%game, %obj, %destroyer)
{
   //warn("CoreGame::staticShapeDestroyed(" SPC %game.class @", "@ %obj.getDataBlock().getName() @", "@ %destroyer.client.nameBase SPC ")");
   %dataName = %obj.getDataBlock().getName();
   %shapeName = %obj.getDataBlock().nameTag;

   if ( isObject( %destroyer.client ) )
   {
      if ( %destroyer.team != %obj.team )
      {
         %destroyer.client.shapeDestroys++;
         messageTeamExcept(%destroyer.client, 'MsgDestroyed', '\c0%1 destroyed an enemy %2', %destroyer.client.playerName, %shapeName);
         messageClient(%destroyer.client, %msgType, "", %game.SCORE_PER_DESTROY_SHAPE, %shapeName); //needs to be fixed (especially the shapecharge) and properly integrated
         %game.updateScore(%destroyer.client);
      }
   }
}

function CoreGame::turretDestroyed(%game, %obj, %destroyer)
{
   //warn("CoreGame::turretDestroyed(" SPC %game.class @", "@ %obj.getDataBlock().getName() @", "@ %destroyer.client.nameBase SPC ")");
   %dataName = %obj.getDataBlock().getName();
   %shapeName = addTaggedString(%obj.getShapeName());

   if ( isObject( %destroyer.client ) )
   {
      if ( %destroyer.team != %obj.team )
      {
         %destroyer.client.turretDestroys++;
         messageTeamExcept(%destroyer.client, 'MsgDestroyed', '\c0%1 destroyed an enemy %2', %destroyer.client.playerName, %shapeName);
         messageClient(%destroyer.client, %msgType, '\c0You received a %1 point bonus for destroying an enemy %2', %game.SCORE_PER_DESTROY_TURRET, %shapeName);
         %game.updateScore(%destroyer.client);
      }
   }
}

function CoreGame::playerTouchSwitch(%game, %switch, %player)
{
   %client = %player.client;
   if ( %switch.team == %client.team )
      return false;

   // Let everyone know whats going on
   messageAll( 'MsgSwitchClaimed', '\c2%1 claimed %2 for %3.', %client.playerName, %switch.nameTag, %game.getTeamName( %client.team ) );

   // Change teams
   %group = %switch.getGroup();
   %group.assignTeam( %client.team ); // This should do the switch as well

   return true;
}

function CoreGame::awardScoreDeath(%game, %victimID, %damageType)
{
   %victimID.deaths++;

   if ( %victimID.deaths[$DamageText[%damageType]] $="" )
      %victimID.deaths[$DamageText[%damageType]] = 0;

   %victimID.deaths[$DamageText[%damageType]]++;

   if ( %game.SCORE_PER_DEATH != 0 )
   {      
      %game.updateScore( %victimID );
   }
}

function CoreGame::awardScoreKill(%game, %killerID, %damageType)
{
   %killerID.kills++;

   if ( %killerID.kills[$DamageText[%damageType]] $="" )
      %killerID.kills[$DamageText[%damageType]] = 0;

    %killerID.kills[$DamageText[%damageType]]++;

   %game.updateScore( %killerID );      
} 

function CoreGame::awardScoreSuicide(%game, %victimID)
{
   %victimID.suicides++;
   %game.updateScore( %victimID );
}
   
function CoreGame::awardScoreTeamkill(%game, %victimID, %killerID)
{
   %killerID.teamKills++;
   if (%game.SCORE_PER_TEAMKILL != 0)   
      messageClient(%killerID, 'MsgScoreTeamkill', '\c0You have been penalized for killing teammate %1.', %victimID.playerName); 

   %game.updateScore( %killerID );

   // Start a kick vote
   if ( !$pref::Server::TournamentMode )
   {
      if( ( $pref::Server::TkLimit >= 3 && %killerID.teamKills >= $pref::Server::TkLimit ) && (getAdmin() == 0))
      {
         serverCmdStartNewVote(%victimID, "VoteKickPlayer", %killerID, 0, 0, 0, true);
         bottomPrintAll("<color:ff0000>" @ %killerID.nameBase @ " Has " @ %killerID.teamKills @ " team kills. Recommend voting yes.", 4, 2);
         //LogEcho(%killerID.nameBase @ " GUID: " @ %killerID.guid @ " TKS: " @ %killerID.teamKills, 1);
      }
   }
}
