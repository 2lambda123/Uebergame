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

//--- GAME RULES BEGIN ---
$HostGameRules["TEMPLATE", 0] = "Template Game.";
$HostGameRules["TEMPLATE", 1] = "Kill the bad guy";
//--- GAME RULES END ---

$DM:TeamCount = 5; //start off DM team count at 5, so we can use 1-4 for teams
$TEMPLATEGame::Matches = 3; //Number of matches we want

package TEMPLATEGame
{
   function TEMPLATEGamedummy()
   {
      echo("All game types MUST have a package to activate! Even if all it contains is a dummy function.");
   }
};

function TEMPLATEGame::setupGameParams(%game)
{
   //LogEcho("TEMPLATEGame::setupGameParams(" SPC %game.class SPC ")");
   
   %game.playerType = "DefaultPlayerData";
   $gameMode = TEMPLATEGame;
   
   CoreGame::setupGameParams(%game);

   %game.SCORE_PER_KILL = 10;
   %game.SCORE_PER_DEATH = -10;
   %game.SCORE_PER_SUICIDE = -10;
}

function TEMPLATEGame::getTeamName(%game, %team)
{
   if(%team == 0)
      return addTaggedString($pref::Server::teamName[%team]);
   else
      return 'Match';
}

function TEMPLATEGame::setUpTeams(%game)
{
   //LogEcho("TEMPLATEGame::setUpTeams(" SPC %game.class SPC ")");
   %group = nameToID("MissionGroup/Teams");
   if(%group == -1)
      return
   
   // create a Team0 if it does not exist
   %team = nameToID("MissionGroup/Teams/Team0");
   if(%team == -1)
   {
      %team = new SimGroup("Team0");
      %group.add(%team);
   }

   %dropSet = new SimSet("TeamDrops0");
   MissionCleanup.add(%dropSet);
   %spawns = nameToID("MissionGroup/Teams/Team0/SpawnSpheres0");
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

function TEMPLATEGame::onClientEnterGame(%game, %client)
{
   //LogEcho("TEMPLATEGame::onClientEnterGame(" SPC %game.class @", "@ %client.nameBase SPC ")");

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

function TEMPLATEGame::onClientLeaveGame(%game, %client)
{
   //LogEcho("TEMPLATEGame::onClientLeaveGame(" SPC %game.class @", "@ %client.nameBase SPC ")");
   %game.clearClientVaribles(%client);
   %game.updateScore(%client);

   CoreGame::onClientLeaveGame(%game, %client);
}

function TEMPLATEGame::getClientIndex(%game, %client)
{
   // Find our index in the client group...
   for( %i = 0; %i < ClientGroup.getCount(); %i++ )
   {
      if(ClientGroup.getObject(%i) == %client)
         return %i;
   }
   return -1;
}

function TEMPLATEGame::assignClientTeam(%game, %client)
{
   //LogEcho("TEMPLATEGame::assignClientTeam(" SPC %game.class @", "@ %client.nameBase SPC ")");

   // This is pretty simple, just get the clients index from the client group
   // plus four, we plus four because the index starts at zero.
   // Team zero is always spectators.
   // However, this may cause a problem if a gametype has more then x teams and we swicth to it..
   //%client.team = (%game.getClientIndex(%client) + 4);

   %client.team = 1; //Assign each client to the same team, co-op mode
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

function TEMPLATEGame::clientChangeTeam(%game, %client, %team, %fromObs)
{
   //LogEcho("TEMPLATEGame::clientChangeTeam(" SPC %game.class @", "@ %client.nameBase @", "@ %team @", "@ %fromObs SPC ")");

   if(%fromObs)
   {
      %game.assignClientTeam(%client);
      %game.spawnPlayer(%client, false);
   }
}

function TEMPLATEGame::clientJoinTeam(%game, %client, %team, %respawn)
{
   //LogEcho("TEMPLATEGame::clientJoinTeam(" SPC %game.class @", "@ %client.nameBase @", "@ %team @", "@ %respawn SPC ")");
   if ( %team != 0 )
      return;

   %game.assignClientTeam(%client);
   %game.spawnPlayer(%client, %respawn);
}

function TEMPLATEGame::createPlayer(%game, %client, %spawnPoint, %respawn)
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

function TEMPLATEGame::updateScore(%game, %cl)
{
   //LogEcho("TEMPLATEGame::updateScore(" SPC %game.class @", "@ %cl.nameBase SPC ")");
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

/* //This is left commented as the template game mode doesn't use a max score.
function TEMPLATEGame::checkScoreLimit(%game, %sourceClient)
{
   if ( %sourceClient.score >= $pref::Server::DMScoreLimit )
      %game.onGameScoreLimit();
}*/
/*
function TEMPLATEGame::onGameScoreLimit(%game)
{
   echo("TEMPLATEGame::onGameScoreLimit(" SPC %game.class SPC ")");
   echo("Game over (scorelimit)");
   %game.cycleGame();
}*/

function TEMPLATEGame::pushChooseTeamMenu(%game, %client)
{
   // This list MUST be sent in order so that it is sync with the clients drop down menu.
   %list = strupr($pref::Server::teamName[0] TAB "AUTOMATIC");
   commandToClient(%client, 'PushTeamMenu', addTaggedString(%list));
}

function TEMPLATEGame::pushChooseSpawnMenu(%game, %client)
{
   %list = "Firebase";
   commandToClient( %client, 'PushSpawnMenu', %list );
}

function TEMPLATEGame::clientChooseSpawn(%game, %client, %option, %value)
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


/////////////// Most relevant functions for custom game mode /////////////////////////////////

//Here's the main function for the game mode, this is called once the map is created
function TEMPLATEGame::startGame(%game)
{
   CoreGame::startGame(%game);
  
   //Here we will be eventually spawning traps, NPCs and procedurally generate contents.
   %game.spawnDummyBoss();
}

function TEMPLATEGame::endGame(%game)
{
   //LogEcho("TEMPLATEGame::endGame(" SPC %game SPC ")");
   if($Game::Running)
   {
       messageAll( 'MsgGameOver', 'Match has ended.');
   }
   CoreGame::endGame(%game);
}

function TEMPLATEGame::spawnDummyBoss(%game)
{
    //Actually spawn the dummy enemy
   %npc = new AiPlayer(EndBossExample)
    {
         dataBlock = DummyBossData;
         class = "BadBot";
         client = -1;
         team = -1; //Team 1 is assigned as we want it to be an enemy of the clients
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
   MissionGroup.add(%npc);
   //We could now arm the NPC and such.
   
   %spawnPoint = %game.pickSpawnPoint(1); //we will change this to a custom spawning function
   %npc.setPosition(%spawnPoint); 
   %npc.tetherPoint = %spawnPoint;
}

//This function is important to assign scores, check winning conditions and such.
function TEMPLATEGame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc)
{
   //LogEcho("TEMPLATEGame::onDeath(" SPC %game.class @", "@ %player.getClassName() @", "@ %client.nameBase @", "@ %sourceObject @", "@ %sourceClient @", "@ %damageType @", "@ %damLoc SPC ")");
      
   // Call the default to handle the basics
   CoreGame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc);

   // Did a client controlled object kill this player?
   if( isObject( %client ) ) //Note: this will always fail when %player.getID() == EndBossExample.getID() as it has no client.
   {
      // Did a client controlled object kill this player?
      if( isObject( %sourceClient ) )
      {
         if( %sourceClient.team != %client.team && %sourceClient != %client )
         {
            messageClient(%sourceClient, 'MsgYourKills', "", %sourceClient.kills);
            //%game.checkScoreLimit(%sourceClient);
         }
      }
   }
   
   messageClient(%client, 'MsgYourDeaths', "", %client.deaths + %client.suicides);
   
   
   //Our winning condition is below
   
   //Check winning, additional score and losing conditions here
   if(%player.getID() == EndBossExample.getID()) //is it our boss who died?
   {
      %game.awardScoreKill(%sourceClient, %damageType);//our server-side object doesn't have a client, so the above isObject(%client) alsways failed in this case.
      messageAll( 'MsgDeath', '%1 was killed by %2\c2 [%3]',"Banana boss", %sourceClient.playerName, $DamageText[%damageType] );
      messageClient(%sourceClient, 'MsgYourKills', "", %sourceClient.kills);
      %game.onFinalConditionMet();
      %player.schedule(2000,"delete");
   }
}


function TEMPLATEGame::onFinalConditionMet(%game)
{
    $TEMPLATEGame::Matches--; //decrease the matches number
    
    if($TEMPLATEGame::Matches > 0)
    {
        %game.schedule(6000,"rematch"); //here we can use a variable to count down and eventually respawn to have many matches
        return;
    }
    else
    {
       centerPrintAll("\n<color:ff0000><font:Arial:24>The banana monster is dead, the world is a safer place now...", 5, 3);
       %game.schedule(5000,"endGame");
    }
   
}


//Get called by onFinalConditionMet() when $TEMPLATEGame::Matches > 0
function TEMPLATEGame::rematch(%game)
{
    centerPrintAll("\n<color:ff0000><font:Arial:24>The banana monster is back!", 5, 3);
    %game.spawnDummyBoss();
}
