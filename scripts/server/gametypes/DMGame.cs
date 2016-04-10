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

// DisplayName = Deathmatch

//--- GAME RULES BEGIN ---
$HostGameRules["DM", 0] = "Eliminate the competition.";
$HostGameRules["DM", 1] = "Player with the best kill to death ratio wins!";
//--- GAME RULES END ---

package DMGame
{
   function DMdummy()
   {
      echo("All game types MUST have a package to activate! Even if all it contains is a dummy function.");
   }
};

function DMGame::setupGameParams(%game)
{
   //LogEcho("DMGame::setupGameParams(" SPC %game.class SPC ")");
   
   %game.playerType = "DefaultSoldier";
   
   CoreGame::setupGameParams(%game);

   %game.SCORE_PER_KILL = 10;
   %game.SCORE_PER_DEATH = -10;
   %game.SCORE_PER_SUICIDE = -10;
}

function DMGame::getTeamName(%game, %team)
{
   if(%team == 0)
      return addTaggedString($pref::Server::teamName[%team]);
   else
      return 'Game';
}

function DMGame::setUpTeams(%game)
{
   //LogEcho("DMGame::setUpTeams(" SPC %game.class SPC ")");
   %group = nameToID("MissionGroup/Teams");
   if(%group == -1)
      return;
   
   // create a team0 if it does not exist
   %team = nameToID("MissionGroup/Teams/Team0");
   if(%team == -1)
   {
      %team = new SimGroup("Team0");
      %group.add(%team);
   }

   %dropSet = new SimSet("TeamDrops0");
   MissionCleanup.add(%dropSet);
   %spawns = nameToID("MissionGroup/Teams/team0/SpawnSpheres0");
   if(%spawns != -1)
   {
      %count = %spawns.getCount();
      for(%i = 0; %i < %count; %i++)
         %dropSet.add(%spawns.getObject(%i));
   }

   // set the 'team' field for all the objects in this team
   %team.assignTeam(0);

   %game.numTeams = 1;
}

function DMGame::onClientEnterGame(%game, %client)
{
   //LogEcho("DMGame::onClientEnterGame(" SPC %game.class @", "@ %client.nameBase SPC ")");

   CoreGame::onClientEnterGame(%game, %client);

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

function DMGame::onClientLeaveGame(%game, %client)
{
   //LogEcho("DMGame::onClientLeaveGame(" SPC %game.class @", "@ %client.nameBase SPC ")");
   %game.clearClientVaribles(%client);
   %game.updateScore(%client);

   CoreGame::onClientLeaveGame(%game, %client);
}

function DMGame::getClientIndex(%game, %client)
{
   // Find our index in the client group...
   for( %i = 0; %i < ClientGroup.getCount(); %i++ )
   {
      if(ClientGroup.getObject(%i) == %client)
         return %i;
   }
   return -1;
}

function DMGame::assignClientTeam(%game, %client)
{
   LogEcho("DMGame::assignClientTeam(" SPC %game.class @", "@ %client.nameBase SPC ")");

   // This is pretty simple, just get the clients index from the client group
   // plus four, we plus four because the index starts at zero.
   // Team zero is always spectators.
   // However, this may cause a problem if a gametype has more then x teams and we swicth to it..
   %client.team = (%game.getClientIndex(%client) + 4);
   %client.lastTeam = %client.team;

   // Let everybody know this client joined the game
   messageAll( 'MsgClientJoinTeam', '\c1%4 has joined the %3.', %client, %client.team, %game.getTeamName(%client.team), %client.playerName );
   
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

function DMGame::clientChangeTeam(%game, %client, %team, %fromObs)
{
   //LogEcho("DMGame::clientChangeTeam(" SPC %game.class @", "@ %client.nameBase @", "@ %team @", "@ %fromObs SPC ")");

   if(%fromObs)
   {
      %game.assignClientTeam(%client);
      %game.spawnPlayer(%client, false);
   }
}

function DMGame::clientJoinTeam(%game, %client, %team, %respawn)
{
   LogEcho("DMGame::clientJoinTeam(" SPC %game.class @", "@ %client.nameBase @", "@ %team @", "@ %respawn SPC ")");
   if ( %team != 0 )
      return;

   %game.assignClientTeam(%client);
   %game.spawnPlayer(%client, %respawn);
}

function DMGame::createPlayer(%game, %client, %spawnPoint, %respawn)
{
   %player = CoreGame::createPlayer(%game, %client, %spawnPoint, %respawn);
   
   // CoreGame does the actual work, we want to setup a few extras

   if (!isDefined("%client.skin"))
   {
      // Determine which character skins are not already in use
      %availableSkins = %player.getDatablock().availableSkins;             // TAB delimited list of skin names
      %count = ClientGroup.getCount();
      for (%cl = 0; %cl < %count; %cl++)
      {
         %other = ClientGroup.getObject(%cl);
         if (%other != %client)
         {
            %availableSkins = strreplace(%availableSkins, %other.skin, "");
            %availableSkins = strreplace(%availableSkins, "\t\t", "");     // remove empty fields
         }
      }

      // Choose a random, unique skin for this client
      %count = getFieldCount(%availableSkins);
      %client.skin = addTaggedString( getField(%availableSkins, getRandom(%count)) );
   }
   %player.setSkinName( %client.skin );
}

function DMGame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc)
{
   LogEcho("DMGame::onDeath(" SPC %game.class @", "@ %player.getClassName() @", "@ %client.nameBase @", "@ %sourceObject @", "@ %sourceClient @", "@ %damageType @", "@ %damLoc SPC ")");

   // Call the default to handle the basics
   CoreGame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc);

   // Did a client controlled object kill this player?
   if( isObject( %client ) )
   {
      // Did a client controlled object kill this player?
      if( isObject( %sourceClient ) )
      {
         if( %sourceClient.team != %client.team && %sourceClient != %client )
         {
            messageClient(%sourceClient, 'MsgYourKills', "", %sourceClient.kills);
         }
      }
   }

   messageClient(%client, 'MsgYourDeaths', "", %client.deaths + %client.suicides);
}

function DMGame::updateScore(%game, %cl)
{
   LogEcho("DMGame::updateScore(" SPC %game.class @", "@ %cl.nameBase SPC ")");
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

function DMGame::endGame(%game)
{
   LogEcho("DMGame::endGame(" SPC %game SPC ")");
   if($Game::Running)
   {
      %winner = getWord(%game.findTopScorer(), 0);
      %tie = getWord(%game.findTopScorer(), 1);
      if(%tie > 0)
         messageAll( 'MsgGameOver', 'Match has ended in a tie.');
      else
         messageAll( 'MsgGameOver', 'Match has ended. %1 wins!', %winner.playerName );
   }
   CoreGame::endGame(%game);
}

function DMGame::pushChooseTeamMenu(%game, %client)
{
   // This list MUST be sent in order so that it is sync with the clients drop down menu.
   %list = strupr($pref::Server::teamName[0] TAB "AUTOMATIC");
   commandToClient(%client, 'PushTeamMenu', addTaggedString(%list));
}

function DMGame::pushChooseSpawnMenu(%game, %client)
{
   %list = "Firebase";
   commandToClient( %client, 'PushSpawnMenu', %list );
}

function DMGame::clientChooseSpawn(%game, %client, %option, %value)
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
