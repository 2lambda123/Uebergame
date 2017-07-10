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

// DisplayName = Marked For Death

//--- GAME RULES BEGIN ---
$HostGameRules["PBMfD", 0] = "Kill the enemy and don't get killed.";
$HostGameRules["PBMfD", 1] = "Team with the most kills wins!";
//--- GAME RULES END ---

package PBMfDgame
{
   function ArmoryCrate::onCollision(%data, %obj, %col)
   {
      if ( %col.getType() & ( $TypeMasks::PlayerObjectType ) )
      {
         if ( %col.getState() $= "Dead" || %col.isMounted() )
            return;

         if ( isObject( %col.client ) && %col.client.isMarked )
         {
            SMSInv.ReplenishLoadoutAmmo(%col);
            if ( %col.getDamageLevel() > 0 )
               %col.applyRepair( %data.repairAmount );

            serverPlay3D( %data.pickupSound, %col.getTransform() );
            %obj.delete();
         }
         else
            Parent::onCollision(%data, %obj, %col);
      }
   }

   // Used for scoring
   function resetThreatTimer(%client)
   {
      %client.dmgdObjective = false;
   }
};

function PBMfDgame::setupGameParams(%game)
{
   //echo("PBMfDgame::setupGameParams(" SPC %game.class SPC ")");
   
   %game.playerType = "Paintball";
   $gameMode = PBMfDgame;
   
   CoreGame::setupGameParams(%game);

   %game.SCORE_PER_KILL = 10;
   %game.SCORE_PER_MARK_DEFEND = 10;
   %game.SCORE_PER_DEATH = -10;
   %game.SCORE_PER_SUICIDE = -10;
   %game.SCORE_PER_TEAMKILL = -10;
   %game.SCORE_PER_PLYR_MARK_KILL = 30;
}

function PBMfDgame::onMissionLoaded(%game)
{
   //echo("PBMfDgame::onMissionLoaded(" SPC %game.class SPC ")");
   CoreGame::onMissionLoaded(%game);
   for(%i = 1; %i <= %game.numTeams; %i++)
   {
      $TeamScore[%i] = 0;
      $TeamMark[%i] = "";
   }
}

function PBMfDgame::setUpTeams(%game)
{
   //echo("PBMfDgame::setUpTeams(" SPC %game.class SPC ")");
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

function PBMfDgame::getTeamName(%game, %team)
{
   return addTaggedString($pref::Server::teamName[%team]);
}

function PBMfDgame::onClientEnterGame(%game, %client)
{
   //echo("PBMfDgame::onClientEnterGame(" SPC %game.class @", "@ %client.nameBase SPC ")");
   CoreGame::onClientEnterGame(%game, %client);

   // Set clients score and stats to zero
   for(%i = 1; %i <= %game.numTeams; %i++)
   {
      messageClient(%client, 'MsgMfDAddTeam', "", %i, %game.getTeamName(%i));
      messageClient(%client, 'MsgTeamScoreIs', "", %i, $TeamScore[%i]);
      messageClient(%client, 'MsgMfDMarkName', "", %i, $TeamMark[%i]);
   }

   %game.clearClientVaribles(%client);
   for(%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %cl = ClientGroup.getObject(%i);
      messageClient(%client, 'MsgClientScoreChanged', "", %cl, %cl.score, %cl.kills, %cl.deaths, %cl.suicides, %cl.teamKills);
   }
}

function PBMfDgame::onClientLeaveGame(%game, %client)
{
   //echo("PBMfDgame::onClientLeaveGame(" SPC %game.class @", "@ %client.nameBase SPC ")");
   %game.clearClientVaribles(%client);
   %game.updateScore(%client);
   
   // If client was marked and leaves the game, pick a new one
   if (( %client.player.isMarked ) && (%client.player.team == 1)) {
      %client.player.isMarked = 0;
      $Team1HasMarked = 0;
      objectiveHud.markName[1].setValue("GONE");
   }
   if (( %client.player.isMarked ) && (%client.player.team == 2)) {
      %client.player.isMarked = 0;
      $Team2HasMarked = 0;
      objectiveHud.markName[2].setValue("GONE");
   }

   CoreGame::onClientLeaveGame(%game, %client);
}

//-----------------------------------------------------------------------------

function PBMfDgame::assignClientTeam(%game, %client, %respawn)
{
   //echo("PBMfDgame::assignClientTeam(" SPC %game.class @", "@ %client.nameBase @", "@ %respawn SPC ")");

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

function PBMfDgame::clientJoinTeam(%game, %client, %team, %respawn)
{
   //echo("PBMfDgame::clientJoinTeam(" SPC %game.class @", "@ %team @", "@ %respawn SPC ")");
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

function PBMfDgame::loadOut(%game, %player)
{
   //LogEcho("\c4CoreGame::loadOut(" SPC %game.class SPC %player.client.nameBase SPC ")");
   %client = %player.client;
   trace(1);
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
   trace(0);
   if ((%player.team == 1) && ($Team1HasMarked == 0) && (%player.client.wasMarked == 0)) {
      %player.isMarked = 1;
      $TeamMark[1] = %client.playerName;
      messageClient( %client, 'MsgYourMarked', '\c2You have been marked for death!' );
      messageAllExcept( %client, -1, 'MsgMfDNewMark', '\c5%1 has been choosen as the target for %2.', $TeamMark[1], %game.getTeamName(1) );
      messageAll( 'MsgMfDMarkName', "", 1, $TeamMark[1] ); // Silent, update the hud

      %client.player.setInventory( BlueFlagImage, 1 );
      %client.player.mountImage( BlueFlagImage, $FlagSlot, true );

      $Team1HasMarked = 1;
   }
   
   if ((%player.team == 2) && ($Team2HasMarked == 0) && (%player.client.wasMarked == 0)) {
      %player.isMarked = 1;
      $TeamMark[2] = %client.playerName;
      messageClient( %client, 'MsgYourMarked', '\c2You have been marked for death!' );
      messageAllExcept( %client, -1, 'MsgMfDNewMark', '\c5%1 has been choosen as the target for %2.', $TeamMark[2], %game.getTeamName(2) );
      messageAll( 'MsgMfDMarkName', "", 2, $TeamMark[2] ); // Silent, update the hud

      %client.player.setInventory( RedFlagImage, 1 );
      %client.player.mountImage( RedFlagImage, $FlagSlot, true );

      $Team2HasMarked = 1;
   }
   
   if ( %player.client.wasMarked == 1)
      %player.client.wasMarked = 0; 
}

function PBMfDgame::pickTeamMark(%game, %team)
{
   //warn("PBMfDgame::pickTeamMark(" SPC %game.class SPC ")");
   %playerCount[%team] = 0;

   // Get a count of the clients assigned to this team that haven't been selected as a target
   %count = ClientGroup.getCount();
   for ( %i = 0; %i < %count; %i++ )
   {
      %cl = ClientGroup.getObject(%i);
      if ( %cl.team == %team ) // Don't add previously marked clients
      {
         %playerCount[%team]++; // Increase team size
         %client[%playerCount[%team]] = %cl;
      }
   }

   //warn("Free client count is" SPC %playerCount[%team] SPC "for team" SPC %team);

   %marked = %client[%playerCount[%team]];

   if ( isObject( %marked ) )
   {
      %marked.isMarked = 1;
      $TeamMark[%team] = %marked.playerName;
      messageClient( %client[%playerCount[%team]], 'MsgYourMarked', '\c2You have been marked for death!' );
      messageAllExcept( %client[%playerCount[%team]], -1, 'MsgMfDNewMark', '\c5%1 has been choosen as the target for %2.', $TeamMark[%team], %game.getTeamName(%team) );
      messageAll( 'MsgMfDMarkName', "", %team, $TeamMark[%team] ); // Silent, update the hud

      if ( isObject( %marked.player ) )
      {
         %marked.player.clearInventory();
         %marked.player.setInventory( Ryder, 1, 1 );
         %marked.player.setInventory( RyderClip, %marked.player.maxInventory(RyderClip), 1 );
         %marked.player.setInventory( RyderAmmo, %marked.player.maxInventory(RyderAmmo), 1 );
         %marked.player.weaponCount = %marked.player.getDataBlock().maxWeapons;
         %marked.player.use("Ryder");
         
         if ( %marked.player.team == 1 )
         {
         %marked.player.setInventory( BlueFlagImage, 1 );
         %marked.player.mountImage( BlueFlagImage, $FlagSlot, true );
         }
         if ( %marked.player.team == 2 )
         {
         %marked.player.setInventory( RedFlagImage, 1 );
         %marked.player.mountImage( RedFlagImage, $FlagSlot, true );
         }
      }
      return( %marked );
   }
}

//-----------------------------------------------------------------------------

function PBMfDgame::onLeaveMissionArea(%game, %player)
{
   if ( %player.getState() !$= "Dead" )
   {
      %player.alertThread = %game.schedule(1000, "AlertPlayer", 3, %player);
      messageClient(%player.client, 'MsgLeaveMissionArea', '\c1Return to mission area or take damage.', %game, %playerData, %player);
   }
}

function PBMfDgame::onEnterMissionArea(%game, %player)
{
   cancel(%player.alertThread);
   %player.alertThread = "";
}

function PBMfDgame::AlertPlayer(%game, %count, %player)
{
   if(%count > 1)
      %player.alertThread = %game.schedule(1000, "AlertPlayer", %count - 1, %player);
   else
      %player.alertThread = %game.schedule(1000, "MissionAreaDamage", %player);
}

function PBMfDgame::MissionAreaDamage(%game, %player)
{
   if ( %player.getState() !$= "Dead" )
   {                                   
      %player.setDamageFlash(0.1);
      %prevHurt = %player.getDamageLevel();
      %player.setDamageLevel(%prevHurt + 0.5);

      if ( %player.getState() $= "Dead" )
         %game.onDeath( %player, %player.client, 0, 0, $DamageType::OutOfBounds, %player.getPosition );
      else
         %player.alertThread = %game.schedule(1000, "MissionAreaDamage", %player);
   }
   else
   {
      %game.onDeath( %player, %player.client, 0, 0, "OOB", %player.getPosition );
   }  
}

//-----------------------------------------------------------------------------

function PBMfDgame::onDamaged(%game, %clVictim, %clAttacker, %sourceObject, %damageType)
{
   CoreGame::onDamaged(%game, %clVictim, %clAttacker, %sourceObject, %damageType);
   // If the victim is Marked for Death and is not on the attackers team, mark the attacker as a threat for x seconds
   if ((%clVictim.isMarked !$= "") && (%clVictim.team != %clAttacker.team))
   {
      %clAttacker.dmgdObjective = true;
      cancel(%clAttacker.threatTimer);  
      //%clAttacker.threatTimer = schedule(3000, %clAttacker, eval, "%clAttacker.dmgdObjective = false;");
      %clAttacker.threatTimer = schedule(3000, %clAttacker, "resetThreatTimer", %clAttacker );
   }
}

function PBMfDgame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc)
{
   //echo("PBMfDgame::onDeath(" SPC %game.class @", "@ %player.getClassName() @", "@ %client.nameBase @", "@ %sourceObject @", "@ %sourceClient @", "@ %damageType @", "@ %damLoc SPC ")");

   // Call the parent.
   CoreGame::onDeath(%game, %player, %client, %sourceObject, %sourceClient, %damageType, %damLoc);

   if ( isObject( %client ) )
   {
      // Did a client controlled object kill this player?
      if( isObject( %sourceClient ) )
      {
         if( %sourceClient.team != %client.team && %sourceClient != %client )
         {
            if ( %game.testMarkDefend( %client ) )
               %game.awardScoreMarkDefend( %sourceClient );

            if ( %player.isMarked )
            {
               %sourceClient.markKills++;
               $TeamScore[%sourceClient.team]++;

               // Update everyones objective hud with team scores
               for ( %i = 1; %i <= %game.numTeams; %i++ )
                  messageAll('MsgTeamScoreIs', "", %i, $TeamScore[%i]);  
            }
            %game.checkScoreLimit( %client.team );
         }
      }  

      if (( %player.isMarked ) && ( %client.team == 1 )) {
         %player.isMarked = 0;
         %player.client.wasMarked = 1;
         $Team1HasMarked = 0;
         objectiveHud.markName[1].setValue("DEAD");
      }
      if (( %player.isMarked ) && ( %client.team == 2 )) {
         %player.isMarked = 0;
         %player.client.wasMarked = 1;
         $Team2HasMarked = 0;
         objectiveHud.markName[2].setValue("DEAD");
      }
   }
}

function PBMfDgame::testMarkDefend(%game, %victim, %killer)
{
   return (%victimID.dmgdObjective); 
}

function PBMfDgame::awardScoreMarkDefend(%game, %client)
{
   %client.markDefends++;
   %game.updateScore(%client);
   messageClient(%client, 'MsgObjDef', '\c1You received a %1 point bonus for defending your teams mark.', %game.SCORE_PER_MARK_DEFEND);
   messageTeamExcept(%client, 'MsgObjDef', '\c1Teammate %1 received a %2 point bonus for defending your teams mark.', %client.playerName, %game.SCORE_PER_MARK_DEFEND);
}

function PBMfDgame::updateScore(%game, %cl)
{
   //echo("PBMfDgame::updateScore(" SPC %game.class @", "@ %cl.nameBase SPC ")");
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
                      %cl.markKills * %game.SCORE_PER_PLYR_MARK_KILL;

   %cl.defenseScore = %cl.markDefends * %game.SCORE_PER_MARK_DEFEND;

   %cl.score = mFloatLength((%cl.offenseScore + %cl.defenseScore),1);

   messageAll('MsgClientScoreChanged', "", %cl, %cl.score, %cl.kills, %cl.deaths, %cl.suicides, %cl.teamKills);
   messageClient(%cl, 'MsgYourScoreIs', "", %cl.score);
}

function PBMfDgame::checkScoreLimit(%game, %team)
{
   %markKillLimit = $pref::Server::MfDScoreLimit;
   if(%markKillLimit !$= "")
      %scoreLimit = %markKillLimit;
   else
      %scoreLimit = 5;

   // There is probably a better solution, but I'm lazy
   if ( $TeamScore[1] >= %scoreLimit )
      %game.onGameScoreLimit();
   if ( $TeamScore[2] >= %scoreLimit )
      %game.onGameScoreLimit();
}

function PBMfDgame::onGameScoreLimit(%game)
{
   //echo("PBMfDgame::onGameScoreLimit(" SPC %game.class SPC ")");
   echo("Game over (scorelimit)");
   %game.cycleGame();
}

function PBMfDgame::startGame(%game)
{
   CoreGame::startGame(%game); // Must call parent first otherwise pick targets will be nullified
   $Team1HasMarked = 0;
   $Team2HasMarked = 0;
   $Team1RespawnLocked = 0;
   $Team2RespawnLocked = 0;
}

function PBMfDgame::endGame(%game)
{
   //echo("PBMfDgame::endGame(" SPC %game SPC ")");
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

function PBMfDgame::clearClientVaribles(%game, %client)
{
   //echo("PBMfDgame::clearClientVaribles(" SPC %game.class SPC %client.nameBase SPC ")");
   CoreGame::clearClientVaribles(%game, %client);

   %client.dmgdObjective = false;
   %client.markKills = 0;
   %client.markDefends = 0;
   %client.teamKills = 0;
   %player.isMarked = 0;
}

function PBMfDgame::pushChooseTeamMenu(%game, %client)
{
   // This list MUST be sent in order so that it is sync with the clients drop down menu.
   %list = strupr($pref::Server::teamName[0] TAB "AUTOMATIC" TAB $pref::Server::teamName[1] TAB $pref::Server::teamName[2]);
   commandToClient(%client, 'PushTeamMenu', addTaggedString(%list));
}

function PBMfDgame::pushChooseSpawnMenu(%game, %client)
{
   %list = "Firebase";
   commandToClient( %client, 'PushSpawnMenu', %list );
}

function PBMfDgame::clientChooseSpawn(%game, %client, %option, %value)
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

// RtF game specific spectator function to drop the flag when becoming spectator
function PBMfDgame::forceSpectator(%game, %client, %reason)
{
   //LogEcho("\c4CoreGame::forceSpectator(" SPC %game.class SPC %client.nameBase SPC %reason SPC ")");
   //make sure we have a valid client...
   if (%client <= 0)
      return;

   if(!$Game::Running) // Make sure the game has started
      return;
      
   // If client was marked and leaves the game, pick a new one
   if (( %client.player.isMarked ) && (%client.player.team == 1)) {
      %client.player.isMarked = 0;
      $Team1HasMarked = 0;
      objectiveHud.markName[1].setValue("SPECTATING");
   }
   if (( %client.player.isMarked ) && (%client.player.team == 2)) {
      %client.player.isMarked = 0;
      $Team2HasMarked = 0;
      objectiveHud.markName[2].setValue("SPECTATING");
   }

   // first delete this player
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