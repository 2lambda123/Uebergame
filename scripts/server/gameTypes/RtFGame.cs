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

// DisplayName = Retrieve the Flag

//--- GAME RULES BEGIN ---
$HostGameRules["RtF", 0] = "Flag may be positioned on the map in randomly selected areas.";
$HostGameRules["RtF", 1] = "Find the flag and retrieve it for your team by placing it on your teams flag stand.";
$HostGameRules["RtF", 2] = "Team with the most retrieves wins!";
//--- GAME RULES END ---

package RtFGame
{
   function Flag::initializeObjective(%data, %flag)
   {
      // This function is called after the mission file is executed.

      // Some setup for the flag.
      %flag.client = ""; // Start empty, one of our linkers
      $Game::FlagStatus = "<Home>"; // Objective hud uses this
      $Game::FlagHomeTransform = %flag.getTransform(); // IF bad things happen this is a fallback
      %flag.lastTransForm = %flag.getTransform();
      %flag.static = true;
   }
};

function RtFGame::setupGameParams(%game)
{
   //echo("RtFGame::setupGameParams(" SPC %game.class SPC ")");
   
   %game.playerType = "DefaultPlayerData";
   $gameMode = RtFGame;
   
   CoreGame::setupGameParams(%game);

   // Setup some scoring and timing
   %game.SCORE_PER_KILL = 10;
   %game.SCORE_PER_DEATH = -10;
   %game.SCORE_PER_SUICIDE = -10;
   %game.SCORE_PER_TEAMKILL = -10;
   %game.SCORE_PER_CARRIER_KILL = 10;
   %game.SCORE_PER_PLYR_FLAG_CAP = 20;
   %game.SCORE_PER_TEAM_FLAG_CAP = 1;

   %game.FADE_FLAG_TIME = 2000;
   %game.FLAG_RETURN_DELAY = 30 * 1000;
   %game.flagReturnTimer = "";
}

function RtFGame::onMissionLoaded(%game)
{
   //echo("RtFGame::onMissionLoaded(" SPC %game.class SPC ")");
   CoreGame::onMissionLoaded(%game);

   // Start the team scores at zero
   for(%i = 1; %i <= %game.numTeams; %i++)
      $TeamScore[%i] = 0;
}

function RtFGame::setUpTeams(%game)
{
   //echo("RtFGame::setUpTeams(" SPC %game.class SPC ")");

   // Need this group! This is how we create teams.
   %group = nameToID("MissionGroup/Teams");
   if ( %group == -1 )
      return;

   // create a Team0 if it does not exist
   %team = nameToID("MissionGroup/Teams/Team0");
   if(%team == -1)
   {
      %team = new SimGroup("Team0");
      %group.add(%team);
   }

   // 'Team0' is not counted as a team here
   %game.numTeams = 0;
   while( %team != -1 )
   {
      // create drop set and add all spawnsphere objects into it
      %dropSet = new SimSet("TeamDrops" @ %game.numTeams);
      MissionCleanup.add(%dropSet);
      //echo("DROPSET CREATED: " @ %dropSet);
      %spawns = nameToID("MissionGroup/Teams/team" @ %game.numTeams @ "/SpawnSpheres" @ %game.numTeams);
      if ( %spawns != -1 )
      {
         %count = %spawns.getCount();
         for(%i = 0; %i < %count; %i++)
            %dropSet.add(%spawns.getObject(%i));
      }
      // set the 'team' field for all the objects in this team
      %team.assignTeam( %game.numTeams );

      // get next group
      %team = nameToID("MissionGroup/Teams/team" @ %game.numTeams + 1);
      if ( %team != -1 )
         %game.numTeams++;
   }
   //echo("NUMBER OF TEAMS: " @ %game.numTeams);

   // Send the clients the names of the teams and set the scores to zero
   for ( %i = 0; %i < %game.numTeams; %i++ )
   {
      $TeamScore[%i] = 0;
      $TeamDeaths[%i] = 0;

      if ( %i > 0 )
         messageAll( 'MsgTeamNames', "", %count, %game.getTeamName(%i+1));
   }
}

function RtFGame::getTeamName(%game, %team)
{
   // Send the client the friendly name of this team id
   return addTaggedString($pref::Server::teamName[%team]);
}

//-----------------------------------------------------------------------------

function RtFGame::onClientEnterGame(%game, %client)
{
   //echo("RtFGame::onClientEnterGame(" SPC %game.class @", "@ %client.nameBase SPC ")");
   CoreGame::onClientEnterGame(%game, %client);

   // setup client score etc.
   %game.clearClientVaribles(%client);

   // Let the client know everyones score etc. For playerlist and score gui mainly
   for(%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %cl = ClientGroup.getObject(%i);
      messageClient(%client, 'MsgClientScoreChanged', "", %cl, %cl.score, %cl.kills, %cl.deaths, %cl.suicides, %cl.teamKills);
   }

   // Send team names and score down the pipe
   for(%i = 1; %i <= %game.numTeams; %i++)
   {
      messageClient(%client, 'MsgRtFAddTeam', "", %i, %game.getTeamName(%i));
      messageClient(%client, 'MsgTeamScoreIs', "", %i, $TeamScore[%i]);
   }

   // Notify of the flags status. For objective hud
   messageAll( 'MsgRtFFlagStatus', "", 0, $Game::FlagStatus );
}

function RtFGame::onClientLeaveGame(%game, %client)
{
   //echo("RtFGame::onClientLeaveGame(" SPC %game.class @", "@ %client.nameBase SPC ")");
   %game.clearClientVaribles(%client); // Just cause I said so.
   %game.updateScore(%client); // Read above
   
   %player = %client.player;

   // Remove the flag if we had it. Let the game know
   if ( %player.holdingFlag !$= "" )
   {
      // Should handle un-hide of item
      %player.throwObject( %player.holdingFlag );

      // We won't be needing this anymore..
      %player.unmountImage( $FlagSlot );

      // Tell the game
      %game.onFlagDropped( %player, %player.holdingFlag );
   }

   CoreGame::onClientLeaveGame(%game, %client);
}

function RtFGame::assignClientTeam(%game, %client, %respawn)
{
   //echo("RtFGame::assignClientTeam(" SPC %game.class @", "@ %client.nameBase @", "@ %respawn SPC ")");

   // Pick a team so as not to be uneven sides
   %numPlayers = ClientGroup.getCount();
   for(%i = 0; %i <= %game.numTeams; %i++)
      %numTeamPlayers[%i] = 0;

   for(%i = 0; %i < %numPlayers; %i = %i + 1)
   {
      %cl = ClientGroup.getObject(%i);
      if(%cl != %client)
         %numTeamPlayers[%cl.team]++;
   }
   %leastPlayers = %numTeamPlayers[1];
   %leastTeam = 1;

   for(%i = 2; %i <= %game.numTeams; %i++)
   {
      if((%numTeamPlayers[%i] < %leastPlayers) || 
         ((%numTeamPlayers[%i] == %leastPlayers) && 
         ($TeamScore[%i] < $TeamScore[%leastTeam])))
      {
         %leastTeam = %i;
         %leastPlayers = %numTeamPlayers[%i];
      }
   }
   %client.team = %leastTeam;
   %client.lastTeam = %team;

   messageAll( 'MsgClientJoinTeam', '%4 has joined team %3.', %client, %client.team, %game.getTeamName(%client.team), %client.playerName );
   messageClient(%client, 'MsgCheckTeamLines', "", %client.team);

   echo(%client.nameBase @ " (cl " @ %client @ ") joined team " @ %client.team);
}

function RtFGame::clientJoinTeam(%game, %client, %team, %respawn)
{
   //echo("RtFGame::clientJoinTeam(" SPC %game.class @", "@ %team @", "@ %respawn SPC ")");
   // Probbly client request to join from spectator
   if ( %team < 1 || %team > %game.numTeams )
      return;

   if ( %respawn $= "" && $Game::Running )
      %respawn = 1;
   else if (%respawn $= "" && !$Game::Running )
      %respawn = 0;

   %client.team = %team;
   %client.lastTeam = %team;

   // Spawn the player:
   %game.spawnPlayer(%client, %respawn);

   messageAllExcept(%client, -1, 'MsgClientJoinTeam', '\c1%4 joined team %3.', %client, %client.team, %game.getTeamName(%team), %client.playerName);
   messageClient(%client, 'MsgClientJoinTeam', '\c1You joined team %3.', %client, %client.team, %game.getTeamName(%team), %client.playerName);
   messageClient(%client, 'MsgCheckTeamLines', "", %client.team);

   echo(%client.nameBase@" (cl "@%client@") joined team "@%client.team);
}

function RtFGame::pushChooseTeamMenu(%game, %client)
{
   // Send client the list of available team choices. This is not dynamic
   // This list MUST be sent in order so that it is sync with the clients drop down menu.
   %list = strupr($pref::Server::teamName[0] TAB "AUTOMATIC" TAB $pref::Server::teamName[1] TAB $pref::Server::teamName[2]);
   commandToClient(%client, 'PushTeamMenu', addTaggedString(%list));
}

//-----------------------------------------------------------------------------

function RtFGame::createPlayer(%game, %client, %spawnPoint, %respawn)
{
   %player = CoreGame::createPlayer(%game, %client, %spawnPoint, %respawn);

   // CoreGame does the actual work, we want to setup a few extras

   %player.flagTossWait = false; // Can throw the flag
   %player.holdingFlag = ""; // Doesn't have the flag
}

function RtFGame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc)
{
   //echo("RtFGame::onDeath(" SPC %game.class @", "@ %player.getClassName() @", "@ %client.nameBase @", "@ %sourceObject @", "@ %sourceClient @", "@ %damageType @", "@ %damLoc SPC ")");

   if ( isObject( %client ) )
   {
      // Check for carier kill
      if( isObject( %sourceClient ) )
      {
         if( %sourceClient.team != %client.team && %sourceClient != %client )
         {
            // This guy earned a bonus!
            if ( %player.holdingFlag !$= "" )
               %game.awardScoreCarrierKill( %sourceClient );
         }
      }
   }

   // Remove the flag if we had it. Let the game know
   if ( %player.holdingFlag !$= "" )
   {
      // Should handle un-hide of item
      %player.throwObject( %player.holdingFlag );

      // We won't be needing this anymore..
      %player.unmountImage( $FlagSlot );

      // Tell the game
      %game.onFlagDropped( %player, %player.holdingFlag );
   }

   // Call the common.
   CoreGame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc);
}

function RtFGame::updateScore(%game, %cl)
{
   //echo("RtFGame::updateScore(" SPC %game.class @", "@ %cl.nameBase SPC ")");
   %killValue = %cl.kills * %game.SCORE_PER_KILL;
   %deathValue = %cl.deaths * %game.SCORE_PER_DEATH;
   %suicideValue = %cl.suicides * %game.SCORE_PER_SUICIDE;

   if (%killValue - %deathValue <= 0)
      %killPoints = 0;
   else
      %killPoints = (%killValue * %killValue) / (%killValue - %deathValue);

   %cl.offenseScore = %killPoints +
                      %cl.suicides  * %game.SCORE_PER_SUICIDE +
                      %cl.teamKills * %game.SCORE_PER_TEAMKILL +
                      %cl.vehicleDestroys * %game.SCORE_PER_DESTROY_VEHICLE +
                      %cl.shapeDestroys * %game.SCORE_PER_DESTROY_SHAPE +
                      %cl.turretDestroys * %game.SCORE_PER_DESTROY_TURRET +
                      %cl.flagRetrieves * %game.SCORE_PER_PLYR_FLAG_CAP;

   %cl.defenseScore = %cl.carrierKills * %game.SCORE_PER_CARRIER_KILL;

   %cl.score = mFloatLength((%cl.offenseScore + %cl.defenseScore),1);

   messageAll('MsgClientScoreChanged', "", %cl, %cl.score, %cl.kills, %cl.deaths, %cl.suicides, %cl.teamKills);
   messageClient(%cl, 'MsgYourScoreIs', "", %cl.score);
}

function RtFGame::awardScoreCarrierKill(%game, %client)
{
   %client.carrierKills++;
   messageClient(%client, 'MsgCarrierKill', '\c1You received a %1 point bonus for killing the flag carier.', %game.SCORE_PER_CARRIER_KILL);
   messageTeamExcept(%client, 'MsgCarrierKill', '\c1Teammate %1 received a %2 point bonus for killing the flag carrier.', %client.playerName, %game.SCORE_PER_CARRIER_KILL);
}

//-----------------------------------------------------------------------------

function RtFGame::onTouchFlag(%game, %player, %flag)
{
   // If the flag was in the middle of being reset, cancel it
   if ( isEventPending( %game.flagReturnTimer ) )
      cancel( %game.flagReturnTimer );

   // Make sure nothing else is mounted first
   if ( %player.getMountedImage( $FlagSlot ) != 0 )
      %player.unmountImage( $FlagSlot );

   %client = %player.client; // Get the client id
   %flag.client = %client; // link the id to the flag
   %player.holdingFlag = %flag; // link the flag to the player id

   // Mount the flag image
   %player.mountImage( %flag.getDataBlock().image, $FlagSlot, true );

   // Hide the flag item ;)
   %flag.setHidden(true);
   %flag.startFade(0, 0, false);

   // flag is moving.. no really ;)
   $Game::FlagStatus = "<"@%client.nameBase@">";

   // Notify everyone of who took it etc.
   messageAll( 'MsgRtFFlagStatus', '\c2%1 took the flag.', %flag.client.playerName, $Game::FlagStatus );
   serverPlay3D( WeaponUseSound, %player.getTransform());
}

function RtFGame::onFlagDropped(%game, %player, %flag)
{
   // Let the objective hud know the deal
   $Game::FlagStatus = "<Dropped>";

   // Notify everyone of who dropped/threw it etc.
   messageAll( 'MsgRtFFlagStatus', '\c2%1 dropped the flag.', %flag.client.playerName, $Game::FlagStatus );
   //serverPlay3D( WeaponUseSound, %player.getTransform()); //no need for sound, since it already does the throw sound by default for all items thrown

   // Guess theres no carrier anymore..
   %flag.static = false; // Really need to clear this
   %player.holdingFlag = ""; // clear the link
   %flag.client = ""; // clear the client link

   // Give some time for someone to grab again and make a nice visual fade
   if ( !isEventPending( %game.flagReturnTimer ) )
      %game.flagReturnTimer = %game.schedule( %game.FLAG_RETURN_DELAY - %game.FADE_FLAG_TIME, "flagReturnFade", %flag, true);
}

function RtFGame::flagReturnFade(%game, %flag, %dropped)
{
   // Cancel our call to this function if it is still pending
   if ( isEventPending( %game.flagReturnTimer ) )
      cancel( %game.flagReturnTimer );

   messageAll( 'MsgRtFFlagStatus', '\c2Flag returning.', 0, $Game::FlagStatus );

   // Reset the flag shortly
   %game.flagReturnTimer = %game.schedule( %game.FADE_FLAG_TIME, "resetFlag", %flag, %dropped );
   %flag.startFade(%game.FADE_FLAG_TIME, 0, true);
}

function RtFGame::resetFlag(%game, %flag, %dropped)
{
   // Cancel our call to this function if it is still pending
   if ( isEventPending( %game.flagReturnTimer ) )
      cancel( %game.flagReturnTimer );

   if ( !%dropped )
   {
      %group = nameToID("MissionGroup/Teams/Team0/RtF0/Flagstands");
      if( %group != -1 )
      {
         %count = %group.getCount();
         if ( %count != 0 )
         {
            // Pick a stand in the flag group at random
            %index = getRandom( %count-1 );
            %stand = %group.getObject( %index );
            %trans = VectorAdd( %stand.getTransform(), "0 0 0.2" );
            %flag.lastTransForm = %trans;
         }
         else
         {
           %trans = $Game::FlagHomeTransform;
           %flag.lastTransForm = %trans;
         }
      }
      else
      {
         %trans = $Game::FlagHomeTransform;
         %flag.lastTransForm = %trans;
      }
   }
   else
     %trans = %flag.lastTransForm;

   %flag.setVelocity("0 0 0");
   %flag.setTransform( %trans );
   %flag.client = "";
   $Game::FlagStatus = "<Home>";
   %flag.setHidden(false);
   %flag.startFade( %game.FADE_FLAG_TIME, 0, false );
   %flag.static = true;

   // Notify everyone the flag was reset.
   messageAll( 'MsgRtFFlagStatus', "", 0, $Game::FlagStatus );
}

function RtFGame::resetFlagTossWait(%game, %player)
{
   %player.flagTossWait = false;
}

//-----------------------------------------------------------------------------

function RtFGame::onEnterTrigger(%game, %data, %trigger, %colObj)
{
   //echo("RtFGame::onEnterTrigger(" SPC %game.class @", "@ %data @", "@ %trigger @", "@ %colObj SPC ")");

   // Make sure we deserve to trigger it
   if ( ( %colObj.getClassName() !$= "Player" && %colObj.getClassName() !$= "AiIPlayer" ) || 
          %colObj.getState() $= "Dead" || %colObj.isMounted() )
      return;

   // Hand it off for processing..
   %game.onFlagStandCollision( %trigger.parent, %colObj );
}

function RtFGame::onFlagStandCollision(%game, %stand, %player)
{
   // Make sure player actually has a flag
   if ( %player.team != %stand.team || %player.holdingFlag $= "" )
      return;

   // Do scoring
   %client = %player.client;
   $TeamScore[%client.team] += %game.SCORE_PER_TEAM_FLAG_CAP;
   %client.flagRetrieves++;
   %game.updateScore( %client );
   messageClient( %client, 'MsgFlagRetrieve', '\c1You received a %1 point bonus for retrieving the flag.', %game.SCORE_PER_PLYR_FLAG_CAP );
   messageAll('MsgTeamScoreIs', '\c2%3 retrieved the flag for %4', %client.team, $TeamScore[%client.team], %client.playerName, %game.getTeamName(%client.team) );
   serverPlay3D( HealthPatchSound, %player.getTransform());
   %player.unMountImage( $FlagSlot );
   %game.resetFlag( %player.holdingFlag, false );
   %player.holdingFlag = "";
   %game.checkScoreLimit( %client.team );
}

function RtFGame::onLeaveMissionArea(%game, %player)
{
   if ( %player.holdingFlag !$= "" )
   {
      if ( %player.holdingFlag.client == %player.client )
      {
         // We won't be needing this anymore..
         %player.unmountImage( $FlagSlot );

         // Get the actual flag item
         %flag = %player.holdingFlag;
         %flag.setVelocity("0 0 0");
         %flag.setTransform(%player.getWorldBoxCenter());
         %flag.setCollisionTimeout(%player);
         %flag.static = false;

         // Player cant pick it up again right away..
         %player.flagTossWait = true;
         %game.schedule( 1000, resetFlagTossWait, %player );

         if ( %flag.isHidden() )
            %flag.setHidden(false);

         // now for the tricky part -- throwing the flag back into the mission area
         // let's try throwing it back towards its "home"
         %home = $Game::FlagHomeTransform;
         %vecx =  firstWord(%home) - firstWord(%player.getWorldBoxCenter());
         %vecy = getWord(%home, 1) - getWord(%player.getWorldBoxCenter(), 1);
         %vecz = getWord(%home, 2) - getWord(%player.getWorldBoxCenter(), 2);
         %vec = %vecx SPC %vecy SPC %vecz;

         // normalize the vector, scale it, and add an extra "upwards" component
         %vecNorm = VectorNormalize(%vec);
         %vec = VectorScale(%vecNorm, 750);
         %vec = vectorAdd(%vec, "0 0 250");

         // apply the impulse to the flag object
         %flag.applyImpulse(%player.getWorldBoxCenter(), %vec);

         // Tell the game
         %game.onFlagDropped( %player, %player.holdingFlag );

         messageClient(%player.client, 'MsgFlagDropped', '\c1You have lost the flag.');
      }
   }
}

function RtFGame::onEnterMissionArea(%game, %player)
{

}

//-----------------------------------------------------------------------------

function RtFGame::checkScoreLimit(%game, %team)
{
   //echo("RtFGame::checkScoreLimit(" SPC %game.class @", "@ %team SPC ")");
   %retrieveLimit = $pref::Server::RtFScoreLimit;
   if(%retrieveLimit !$= "")
      %scoreLimit = %retrieveLimit * %game.SCORE_PER_TEAM_FLAG_CAP;
   else
      %scoreLimit = 5 * %game.SCORE_PER_TEAM_FLAG_CAP;

   if ( $TeamScore[%team] >= %scoreLimit )
      %game.onGameScoreLimit();
}

function RtFGame::onGameScoreLimit(%game)
{
   //echo("RtFGame::onGameScoreLimit(" SPC %game.class SPC ")");
   echo("Game over (scorelimit)");
   %game.cycleGame();
}

function RtFGame::endGame(%game)
{
   //echo("RtFGame::endGame(" SPC %game SPC ")");
   if ( $Game::Running )
   {
      // send the winner message.
      // I know there is a better way to figure, I am lazy bones today
      %winner = 0;
      if ( $TeamScore[1] > $TeamScore[2] )
      {
         %winnerName = %game.getTeamName(1);
         %team = 1;
         %winner = 1;
      }
      else if ( $TeamScore[2] > $TeamScore[1] )
      {
         %winnerName = %game.getTeamName(2);
         %team = 2;
         %winner = 1;
      }

      if ( %winner )
         messageAll( 'MsgGameOver', 'Match has ended. %1 wins with %2 flag retrieves!', %winnerName, $TeamScore[%team] );
      else
         messageAll( 'MsgGameOver', 'Match has ended in a tie. %1 - %2', $TeamScore[1], $TeamScore[2] );
   }

   // If the flag was in the middle of being reset, cancel it
   if ( isEventPending( %game.flagReturnTimer ) )
      cancel( %game.flagReturnTimer );

   %game.flagReturnTimer = "";

   CoreGame::endGame(%game);
}

function RtFGame::clearClientVaribles(%game, %client)
{
   //echo("RtFGame::clearClientVaribles(" SPC %game.class SPC %client.nameBase SPC ")");
   CoreGame::clearClientVaribles(%game, %client);

   %client.flagRetrieves = 0;
   %client.carrierKills = 0;
}

function RtFGame::pushChooseTeamMenu(%game, %client)
{
   // This list MUST be sent in order so that it is sync with the clients drop down menu.
   %list = strupr($pref::Server::teamName[0] TAB "AUTOMATIC" TAB $pref::Server::teamName[1] TAB $pref::Server::teamName[2]);
   commandToClient(%client, 'PushTeamMenu', addTaggedString(%list));
}

function RtFGame::pushChooseSpawnMenu(%game, %client)
{
   %list = "Castra";
   commandToClient(%client, 'PushSpawnMenu', addTaggedString(%list));
}

function DnHGame::clientChooseSpawn(%game, %client, %option, %value)
{
   switch$ ( %option )
   {
      case 0:
         %client.spawnZone = "";
         %msg = '\c2Drop zone: Castra.';

      default:
         %client.spawnZone = "";
         %msg = '\c2Drop zone: Castra.';
   }
   messageClient( %client, 'MsgDropZone', %msg, "", %value);
}

// RtF game specific spectator function to drop the flag when becoming spectator
function RtFGame::forceSpectator(%game, %client, %reason)
{
   //LogEcho("\c4CoreGame::forceSpectator(" SPC %game.class SPC %client.nameBase SPC %reason SPC ")");
   //make sure we have a valid client...
   if (%client <= 0)
      return;

   if(!$Game::Running) // Make sure the game has started
      return;
      
   %player = %client.player;
   
   // Remove the flag if we had it. Let the game know
   if ( %player.holdingFlag !$= "" )
   {
      // Should handle un-hide of item
      %player.throwObject( %player.holdingFlag );

      // We won't be needing this anymore..
      %player.unmountImage( $FlagSlot );

      // Tell the game
      %game.onFlagDropped( %player, %player.holdingFlag );
   }

   // first kill this player
   if(%client.player)
      %client.player.schedule(50,"delete"); //better solution

   // place them in spectator mode
   %game.clearRespawnWait(%client);
   %client.lastObserverSpawn = -1;
   %client.observerStartTime = $Sim::Time;
   %adminForce = 0;

   // switch client to team 0 (spectator) and save off the last team they were on
   %client.lastTeam = %client.team;
   %client.team = 0;
   //%client.player.team = 0;
   //%client.player.setTeam(0);
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

   // set their control to the obs. cam
   %client.setControlObject( %client.camera );

   // display the hud and clear any previous prints
   clearBottomPrint(%client);
   clearCenterPrint(%client);
   //commandToClient(%client, 'setHudMode', 'Spectator');
   updateSpectatorHud(%client);

   // message everyone about this event
   if(!%adminForce)
      messageAllExcept(%client, -1, 'MsgClientJoinTeam', '\c2%4 has become a %3.', %client, %client.team, %game.getTeamName(0), %client.playerName );
   else
      messageAllExcept(%client, -1, 'MsgClientJoinTeam', '\c2The admin has forced %4 to become an spectator.', %client, %client.team, %game.getTeamName(0), %client.playerName );

   %game.onClientBecomeSpectator(%client);
}
