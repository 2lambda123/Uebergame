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

// DisplayName = Team Deathmatch

//--- GAME RULES BEGIN ---
$HostGameRules["PBTDM", 0] = "Kill the enemy and don't get killed.";
$HostGameRules["PBTDM", 1] = "Team with the most kills wins!";
//--- GAME RULES END ---

package PBTDMGame
{
   function PBTDMdummy()
   {
      echo("All game types MUST have a package to activate! Even if all it contains is a dummy function.");
   }
};

function PBTDMGame::setupGameParams(%game)
{
   //echo("PBTDMGame::setupGameParams(" SPC %game.class SPC ")");
   
   %game.playerType = "Paintball";
   
   CoreGame::setupGameParams(%game);

   %game.SCORE_PER_KILL = 10; 
   %game.SCORE_PER_DEATH = -10;
   %game.SCORE_PER_SUICIDE = -10;
   %game.SCORE_PER_TEAMKILL = -10;
}

function PBTDMGame::onMissionLoaded(%game)
{
   //echo("PBTDMGame::onMissionLoaded(" SPC %game.class SPC ")");
   CoreGame::onMissionLoaded(%game);
   for(%i = 1; %i <= %game.numTeams; %i++)
   {
      $TeamScore[%i] = 0;
      $TeamDeaths[%i] = 0;
   }
}

function PBTDMGame::setUpTeams(%game)
{
   //echo("PBTDMGame::setUpTeams(" SPC %game.class SPC ")");
   %group = nameToID("MissionGroup/Teams");
   if ( %group == -1 )
      return;

   // create a team0 if it does not exist
   %team = nameToID("MissionGroup/Teams/Team0");
   if(%team == -1)
   {
      %team = new SimGroup("Team0");
      %group.add(%team);
   }

   // 'team0' is not counted as a team here
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

function PBTDMGame::getTeamName(%game, %team)
{
   return addTaggedString($pref::Server::teamName[%team]);
}

function PBTDMGame::onClientEnterGame(%game, %client)
{
   //echo("PBTDMGame::onClientEnterGame(" SPC %game.class @", "@ %client.nameBase SPC ")");
   CoreGame::onClientEnterGame(%game, %client);

   // Set clients score and stats to zero
   for(%i = 1; %i <= %game.numTeams; %i++)
   {
      messageClient(%client, 'MsgTDMAddTeam', "", %i, %game.getTeamName(%i));
      messageClient(%client, 'MsgTeamScoreIs', "", %i, $TeamScore[%i]);
      messageClient(%client, 'MsgTDMTeamDeaths', "", %i, $TeamDeaths[%i]);
   }

   %game.clearClientVaribles(%client);
   for(%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %cl = ClientGroup.getObject(%i);
      messageClient(%client, 'MsgClientScoreChanged', "", %cl, %cl.score, %cl.kills, %cl.deaths, %cl.suicides, %cl.teamKills);
   }
}

function PBTDMGame::onClientLeaveGame(%game, %client)
{
   //echo("PBTDMGame::onClientLeaveGame(" SPC %game.class @", "@ %client.nameBase SPC ")");
   %game.clearClientVaribles(%client);
   %game.updateScore(%client);

   CoreGame::onClientLeaveGame(%game, %client);
}

//-----------------------------------------------------------------------------

function PBTDMGame::assignClientTeam(%game, %client, %respawn)
{
   //echo("PBTDMGame::assignClientTeam(" SPC %game.class @", "@ %client.nameBase @", "@ %respawn SPC ")");

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
   
   //clear scores when client changes team
   %game.clearClientVaribles(%client);

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

function PBTDMGame::clientJoinTeam(%game, %client, %team, %respawn)
{
   //echo("PBTDMGame::clientJoinTeam(" SPC %game.class @", "@ %team @", "@ %respawn SPC ")");
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

//-----------------------------------------------------------------------------

function PBTDMGame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc)
{
   //echo("PBTDMGame::onDeath(" SPC %game.class @", "@ %player.getClassName() @", "@ %client.nameBase @", "@ %sourceObject @", "@ %sourceClient @", "@ %damageType @", "@ %damLoc SPC ")");

   // Call the parent.
   CoreGame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc);

   if ( isObject( %client ) )
   {
      // Adjust team scores
      if( isObject( %sourceClient ) )
      {
         if( %sourceClient.team != %client.team && %sourceClient != %client )
         {
            $TeamScore[%sourceClient.team]++;
            $TeamDeaths[%client.team]++;

            // Update everyones objective hud with team scores
            for ( %i = 1; %i <= %game.numTeams; %i++ )
            {
               messageAll('MsgTeamScoreIs', "", %i, $TeamScore[%i]);
               messageAll('MsgTDMTeamDeaths', "", %i, $TeamDeaths[%i]);
            }
            %game.checkScoreLimit( %sourceClient.team );
         }
      }
   }
}

function PBTDMGame::updateScore(%game, %cl)
{
   //echo("PBTDMGame::updateScore(" SPC %game.class @", "@ %cl.nameBase SPC ")");
   %killValue = %cl.kills * %game.SCORE_PER_KILL;
   %deathValue = %cl.deaths * %game.SCORE_PER_DEATH;
   %suicideValue = %cl.suicides * %game.SCORE_PER_SUICIDE;

   if (%killValue - %deathValue == 0)
      %cl.efficiency = %suicideValue;
   else
      %cl.efficiency = ((%killValue * %killValue) / (%killValue - %deathValue)) + %suicideValue;

   %cl.score = mFloatLength(%cl.efficiency, 1);

   messageAll('MsgClientScoreChanged', "", %cl, %cl.score, %cl.kills, %cl.deaths, %cl.suicides, %cl.teamKills);
   messageClient(%cl, 'MsgYourScoreIs', "", %cl.score);
}

function PBTDMGame::getScoreLimit(%game)
{
   %scoreLimit = MissionGroup.scoreLimit;
   if(%scoreLimit $= "")
   {
      %count = ClientGroup.getCount();
      %players = 0;
      for ( %i = 0; %i < %count; %i++ )
      {
         %cl = ClientGroup.getObject(%i);
         if ( %cl.team == 0 )
            continue;

         %players++;
      }
      return( %players * 5 );
   }
}

function PBTDMGame::checkScoreLimit(%game, %team)
{
   %scoreLimit = %game.getScoreLimit();
   %over = false;
   for(%i = 0; %i <= %game.numTeams; %i++)
   {
      if ( $TeamScore[%i] >= %scoreLimit )
      {
         %over = true;
         break;
      }
   }

   if ( %over )
      %game.onGameScoreLimit();
}

function PBTDMGame::onGameScoreLimit(%game)
{
   //echo("PBTDMGame::onGameScoreLimit(" SPC %game.class SPC ")");
   echo("Game over (scorelimit)");
   %game.cycleGame();
}

function PBTDMGame::endGame(%game)
{
   //echo("PBTDMGame::endGame(" SPC %game SPC ")");
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
         messageAll( 'MsgGameOver', 'Match has ended. %1 wins with %2 kills!', %winnerName, $TeamScore[%team] );
      else
         messageAll( 'MsgGameOver', 'Match has ended in a tie. %1 - %2', $TeamScore[1], $TeamScore[2] );
   }

   CoreGame::endGame(%game);
}

function PBTDMGame::clearClientVaribles(%game, %client)
{
   //echo("PBTDMGame::clearClientVaribles(" SPC %game.class SPC %client.nameBase SPC ")");
   CoreGame::clearClientVaribles(%game, %client);
}

function PBTDMGame::pushChooseTeamMenu(%game, %client)
{
   // This list MUST be sent in order so that it is sync with the clients drop down menu.
   %list = strupr($pref::Server::teamName[0] TAB "AUTOMATIC" TAB $pref::Server::teamName[1] TAB $pref::Server::teamName[2]);
   commandToClient(%client, 'PushTeamMenu', addTaggedString(%list));
}

function PBTDMGame::pushChooseSpawnMenu(%game, %client)
{
   %list = "Firebase";
   commandToClient( %client, 'PushSpawnMenu', %list );
}

function PBTDMGame::clientChooseSpawn(%game, %client, %option, %value)
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
   messageClient( %client, 'MsgDropZone', %msg );
}
