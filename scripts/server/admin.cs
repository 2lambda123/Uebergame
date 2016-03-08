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

function serverCmdSAD(%client, %password)
{
   if(%password $= "")
   {
      messageClient(%client, 'MsgPasswordFailed', '\c2You did not supply a PW.');
      return;
   }

   switch$ (%password)
   {
      case $pref::Server::SuperAdminPassword:
         if(!%client.isSuperAdmin)
         {
            if(%password $= "changeme")
            {
               messageClient(%client, 'MsgPasswordFailed', '\c2Illegal SAD PW. You need to change the default \"$pref::Server::SuperAdminPassword\" value in \"server.config.cs\"!');
               return;
            }
            %client.isAdmin = true;
            %client.isSuperAdmin = true;
            MessageAll( 'MsgAdminPlayer', '\c2%2 has become a Super Admin by force.', 0, %client, %client.playerName, 1);
            echo(%client.nameBase @ " has become a Super Admin by force.");
         }

      case $pref::Server::AdminPassword:
         if(!%client.isAdmin)
         {
            if(%password $= "changethis")
            {
               messageClient(%client, 'MsgPasswordFailed', '\c2Illegal Admin PW. You need to change the default \"$pref::Server::AdminPassword\" value in \"server.config.cs\"!');
               return;
            }
            %client.isAdmin = true;
            %client.isSuperAdmin = false;
            MessageAll( 'MsgAdminPlayer', '\c2%2 has become a Admin by force.', 0, %client, %client.playerName, 0);
            echo(%client.nameBase @ " has become an Admin by force.");
         }

      default:
         messageClient(%client, 'MsgPasswordFailed', '\c2Illegal SAD PW.');
         %client.SadAttempts++;
         if(%client.SadAttempts >= 6 && !%client.isSuperAdmin)
         {
            %client.getAddress();
            %client.getAuthInfo();
            messageClient(%client, 'onClientBanned', '\c2For attempting to exploit SAD to gain unauthorized Admin by entering\ntoo many passwords, you are being Banned');
            if( isObject(%client.player) )
            {
               %client.player.kill($DamageType::ScriptDamage);
               %client.schedule(700, "delete");
            }
            schedule(10, %client, "ResetSadAttp", %client);
            %client.setDisconnectReason( 'For attempting to exploit SAD to gain unauthorized Admin by entering\ntoo many passwords, you are being Banned.' );
            %client.schedule(700, "delete");
            BanList::add(%client.guid, %client.getAddress(), $pref::Server::BanTime);
            echo(%client.nameBase @ " " @ %client.guid @ " has been banned for excessive use of SAD");
         }
   }
}

function ResetSadAttp(%client)
{
   %client.SadAttempts = 0;
}

function serverCmdSADSetPassword(%client, %password)
{
   if(%client.isSuperAdmin)
      $pref::Server::AdminPassword = %password;
}

function serverCmdGetRemoteCmdMenu(%client)
{
   // Only Admins have access to these
   if(%client.isAdmin)
   {
      // Populate the drop down menu with options seperated by \t (tab deliniated list).
      %setOne = "Set Max Players\tSend Bottomprint Message\tSend Centerprint Message\tConsole Command";
      %setTwo = "\tEnter Admin Password\tEnter Super Admin Password\tSet Join Password\tSet Admin Password\tSet Super Admin Password";
      %list = %setOne @ %setTwo;
      commandToClient(%client, 'FillCmdMenuDropdown', addTaggedString(%list));
   }
   else
      commandToClient(%client, 'FillCmdMenuDropdown', 'For Administrators only');
}

function serverCmdsendPlayerPopupMenu(%client, %targetClient, %key)
{
   if( isObject( Game ) )
      Game.sendPlayerVoteMenu(%client, %targetClient, %key);
}

function serverCmdGetVoteMenu(%client, %key)
{
   if( isObject( Game ) )
      Game.sendServerVoteMenu(%client, %key);
}

function serverCmdGetMissionTypeList(%client, %key)
{
   for(%type = 0; %type < $HostTypeCount; %type++)
      messageClient( %client, 'MsgVoteItem', "", %key, %type, $HostTypeDisplayName[%type], true);
}

function serverCmdGetMissionList(%client, %key, %type)
{
   // Find the last selected type: - ZOD, maybe not..
   //for(%type = 0; %type < $HostTypeCount; %type++)
   //{
   //   if($HostTypeName[%type] $= $Server::MissionType)
   //      break;
   //}
   //for(%i = $HostMissionCount[%type] - 1; %i >= 0; %i--)
   //{
   //   %idx = $HostMission[%type, %i];
   //   messageClient(%client, 'MsgVoteItem', "", %key, %idx, $HostMissionName[%idx], true);
   //}

   if(%type < 0 || %type >= $HostTypeCount)
      return;

   for(%i = $HostMissionCount[%type] - 1; %i >= 0; %i--)
   {
      %idx = $HostMission[%type, %i];
      messageClient(%client, 'MsgVoteItem', "", %key, %idx, $HostMissionName[%idx], true);
   }
}

function serverCmdGetTimeLimitList(%client, %key)
{
   messageClient(%client, 'MsgVoteItem', "", %key, 5, '5 minutes');
   messageClient(%client, 'MsgVoteItem', "", %key, 10, '10 minutes');
   messageClient(%client, 'MsgVoteItem', "", %key, 15, '15 minutes');
   messageClient(%client, 'MsgVoteItem', "", %key, 20, '20 minutes');
   messageClient(%client, 'MsgVoteItem', "", %key, 25, '25 minutes');
   messageClient(%client, 'MsgVoteItem', "", %key, 30, '30 minutes');
   messageClient(%client, 'MsgVoteItem', "", %key, 45, '45 minutes');
   messageClient(%client, 'MsgVoteItem', "", %key, 60, '60 minutes');
   messageClient(%client, 'MsgVoteItem', "", %key, 999, 'No time limit');
}

function AdminCommand(%client, %typename, %val1, %val2, %val3, %val4)
{
   if(!%client.isAdmin)
      return;

   $AdminCl = %client; // Need this for kicks and bans

   if(Game.scheduleVote !$= "" && Game.voteType $= %typeName)
   {
      messageAll('closeVoteHud', "");
      cancel(Game.scheduleVote);
      Game.scheduleVote = "";
   }
   Game.evalVote(%client, %typeName, true, %val1, %val2, %val3, %val4);
}

function PlayerVote(%client, %typename, %arg1, %arg2, %arg3, %arg4, %clientsVoting, %teamSpecific)
{
   //error("PlayerVote(" SPC %client.nameBase SPC %typename SPC %arg1 SPC %arg2 SPC %arg3 SPC %arg4 SPC %clientsVoting SPC %teamSpecific SPC ")");
   // open the vote hud for all clients that will participate in this vote
   if(%teamSpecific)
   {
       for(%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
       {
           %cl = ClientGroup.getObject(%clientIndex);
           if(%cl.team == %client.team && !%cl.isAIControlled())
              messageClient(%cl, 'MsgToggleVoteHud', "", %clientsVoting, ($pref::Server::VotePassPercent / 100), 1);
       }
   }
   else
   {
       for (%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
       {
           %cl = ClientGroup.getObject(%clientIndex);
           if(!%cl.isAIControlled())
              messageClient(%cl, 'MsgToggleVoteHud', "", %clientsVoting, ($pref::Server::VotePassPercent / 100), 1);
       }
   }

   clearVotes();
   Game.voteType = %typeName;
   Game.scheduleVote = schedule(($pref::Server::VoteTime * 1000), Game, "calcVotes", %client, %typeName, %arg1, %arg2, %arg3, %arg4);
   %client.vote = true;
   %client.canVote = false;
   messageAll('addYesVote', "");
   clearCenterPrint(%client);

   %client.rescheduleVote = schedule(($pref::Server::voteSpread * 1000) + ($pref::Server::voteTime * 1000) , %client, "resetVotePrivs", %client);
}

function resetVotePrivs(%client)
{
   if(isEventPending(%client.rescheduleVote))
      cancel(%client.rescheduleVote);

   %client.rescheduleVote = "";
   %client.canVote = true;
}

function serverCmdSetPlayerVote(%client, %vote)
{
   // players can only vote once
   if( %client.vote $= "" )
   {
      %client.vote = %vote;
      if(%client.vote == 1)
         messageAll('addYesVote', "");
      else
         messageAll('addNoVote', "");

      commandToClient(%client, 'voteSubmitted', %vote);
   }
}

function calcVotes(%client, %typeName, %arg1, %arg2, %arg3, %arg4)
{
   //error("calcVotes(" SPC %client.nameBase SPC %typeName SPC %arg1 SPC %arg2 SPC %arg3 SPC %arg4 SPC ")");
   if(%typeName $= "VoteMatchStart")
   {
      if($Game::Running || $countdownStarted)
         return;
   }
   for(%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++) 
   {
      %cl = ClientGroup.getObject(%clientIndex);
      messageClient(%cl, 'closeVoteHud', "");

      if(%cl.vote !$= "") 
      {
         if(%cl.vote) 
         {
            Game.votesFor[%cl.team]++;
            Game.totalVotesFor++;
         } 
         else 
         {
            Game.votesAgainst[%cl.team]++;
            Game.totalVotesAgainst++;
         }
      }
      else 
      {
         Game.votesNone[%cl.team]++;
         Game.totalVotesNone++;
      }
   }
   Game.evalVote(%client, %typeName, false, %arg1, %arg2, %arg3, %arg4);
   Game.scheduleVote = "";
   Game.kickClient = "";
   clearVotes();
}

function clearVotes()
{
   for(%clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++)
   {   
      %client = ClientGroup.getObject(%clientIndex);
      %client.vote = "";
      messageClient(%client, 'clearVoteHud', "");
      messageClient(%client, 'MsgToggleVoteHud', "", 0, 0, 0);
   }

   for(%team = 1; %team < 5; %team++) 
   {
      Game.votesFor[%team] = 0;
      Game.votesAgainst[%team] = 0;
      Game.votesNone[%team] = 0;
      Game.totalVotesFor = 0;
      Game.totalVotesAgainst = 0;
      Game.totalVotesNone = 0;
   }
}

function serverCmdInitVote(%client, %typeName, %val1, %val2, %val3, %val4, %playerVote)
{
   //error("serverCmdInitVote(" SPC %client.nameBase SPC %typeName SPC %val1 SPC %val2 SPC %val3 SPC %val4 SPC %playerVote SPC ")");
   if(!%client.canVote && (!%client.isAdmin && !%client.isSuperAdmin))
      return;

   if(%typeName $= "")
      return;

   if( isObject( Game ) )
      Game.InitVote(%client, %typeName, %val1, %val2, %val3, %val4, %playerVote);
}

function serverCmdInitAdminCommand(%client, %typeName, %target)
{
   //error("serverCmdInitAdminCommand(" SPC %client.nameBase SPC %typeName SPC %target SPC ")");
   // Admin only actions on players, no voting here
   if(%client.isAdmin)
   {
      // Two client functions send to this, one sends a number ID, the other a string
      // The player pop-up menu sends a string
      switch$(%typeName)
      {
         case 0: // Server player limit
            if(%client.isSuperAdmin)
            {
               if ( isCleanNumber(%target) && %target <= 128 && %target > 0 )
               {
                  $pref::Server::MaxPlayers = %target;
                  messageClient(%client, 'MsgAdmin', '\c2Max players set to %1.', %target);
               }
               else
                  messageClient(%client, 'MsgError', '\c2%1 is invalid, must be between 1 and 128.', %target);
            }
            else
               messageClient(%client, 'MsgError', '\c2Only Super Admins can use this command.');

         case 1: // Bottom print all
            bottomPrintAll(%client.nameBase @ ": " @ %target, 8, 3);

         case 2: // center print all
            centerPrintAll(%client.nameBase @ ": " @ %target, 8, 3);

         case 3: // Remote console access
            if(%client.isSuperAdmin)
            {
               if($pref::Server::AllowRemoteConsole)
               {
                  eval(%target);
                  messageClient(%client, 'MsgAdmin', '\c2Command %1 sent to server console.', %target);
                  echo(%client.nameBase @ "Sent the console command " @ %target @ " to the server.", 1);
               }
               else
                  messageClient(%client, 'MsgError', '\c2Remote console access is disabled.');
            }
            else
               messageClient(%client, 'MsgError', '\c2Only Super Admins can use this command.');

         case 4: // Enter admin password
            serverCmdSAD(%client, %target);

         case 5: // Enter super admin password
            serverCmdSAD(%client, %target);

         case 6: // Set server password
            $pref::Server::Password = %target;
            messageClient(%client, 'MsgAdmin', '\c2Server is locked, password is %1.', %target);

         case 7: // Set admin password
            serverCmdSADSetPassword(%client, %target);

         case 8: // Set super admin password
            serverCmdSADSetPassword(%client, %target);

         case "forceClientToSpectator":
            serverCmdForceClientToSpectator(%client, %target);

         case "changePlayersTeam":
            serverCmdChangePlayersTeam(%client, %target);

         case "AddToAdminList":
            if(%client.isSuperAdmin)
               AddToAdminList(%client, %target);

         case "AddToSuperList":
            if(%client.isSuperAdmin)
               AddToSuperAdminList(%client, %target);

         case "WarnPlayer":
            messageClient(%target, 'MsgWarn', '\c2%1 is warning you for inappropriate behavior. Straighten up your act or you may be kicked from the server.', %client.playerName);
            centerprint(%target, "You are recieving this warning for inappropriate conduct.\nBehave or you will be kicked.", 10, 2);

         case "RemoveAdmin":
            if ( %client.isSuperAdmin )
            {
               if ( %target.isAdmin && !%target.isSuperAdmin )
               {
                  %target.isAdmin = false;
                  %target.isSuperAdmin = false;
                  messageAll('MsgStripAdmin', "", %target );
                  messageClient(%target, 'MsgAdminRevoke', '\c2Your admin status has been revoked by %1.', %client.playerName);
               }
               else
                  messageClient(%client, 'MsgError', '\c2You cannot remove admin from %1.', %target.playerName);
            }

         case "GetClientInfo":
            MessageClient(%client, "", '---------------------------------------------------------------');
            MessageClient(%client, 'ClientInfo', '\c3Client Info for %1\nClientID: %2\nIP: %3', %target.playerName, %target, %target.getAddress());
            MessageClient(%client, "", '---------------------------------------------------------------');

         case "GagPlayer":
            if(!%target.isGagged && !%target.isAdmin)
            {
               %target.isGagged = true;
               messageClient(%client, 'MsgAdmin', 'You have Gagged %1.', %target.playerName);
               messageAllExcept(%target, -1, 'MsgAdminForce', '%1 has been Gagged by %2 for talking too much crap.', %target.playerName, %client.playerName);
               messageClient(%target, 'MsgAdminAction', 'You have Been Gagged by %1, quit talking trash and play.', %client.playerName);
               echo(%client.nameBase @ " gagged " @ %target.nameBase);
            }
            else if (%target.isGagged)
            {
               %target.isGagged = false;
               messageClient(%client, 'MsgAdmin', 'You have UnGagged %1.', %target.playerName);
               messageAllExcept(%target, -1, 'MsgAdminAction', '%1 has been UnGagged by %2.', %target.playerName, %client.playerName);
               messageClient(%target, 'MsgAdminAction', 'You have Been UnGagged by %1, quit talking trash and play.', %client.playerName);
               echo(%client.nameBase @ " ungagged " @ %target.nameBase);
            }

         case "BootToRear":
            if ( %client.isSuperAdmin )
            {
               if ( !$Game::Running )
               {
                  messageClient(%client, 'MsgError', 'You must wait for the match to start!');
                  return;
               }
               if(isObject(%target.player) && !%target.isSuperAdmin)
               {
                  %time = getTime();
                  %vec = "0 0 10";
                  %target.player.applyKick(10, 200, "up");
                  messageAllExcept(%target, -1, 'MsgAdminForce', '%1 has been given a boot to the rear by %2.', %target.playerName, %client.playerName);
                  messageClient(%target, 'MsgAdminAction', 'You have Been given a boot to the rear by %1, now behave.', %client.playerName);
                  echo(%client.nameBase @ " gave " @ %target.nameBase @ " a boot to the rear");
               }
               else
                  messageClient(%client, 'MsgError', 'You must wait for the player to spawn!');
            }

         case "Explode":
            if(%client.isSuperAdmin)
            {
               if(!$Game::Running)
               {
                  messageClient(%client, 'MsgError', 'You must wait for the match to start!');
                  return;
               }
               if(isObject(%target.player) && !%target.isSuperAdmin)
               {
                  %target.player.Kill($DamageType::Explosion);
                  messageAllExcept(%target, -1, 'MsgAdminForce', '%1 found some explosives in his pants planted by %2.', %target.playerName, %client.playerName);
                  messageClient(%target, 'MsgAdminAction', 'You have Been dissasembled for inspection by the Super Admin %1, now behave.', %client.playerName);
                  echo(%client.nameBase @ " exploded " @ %target.nameBase);
               }
               else
                  messageClient(%client, 'MsgError', 'You must wait for the player to spawn!');	
            }
      }
   }
   else
      messageClient(%client, 'MsgError', '\c2Only Admins can use this command.');
}

function clearserver(%client)
{
   if(isEventPending($ClearSchedule))
   {
      cancel($ClearSchedule);
      $ClearSchedule = "";
   }
   echo(%client.nameBase @ " has cleared the server.", 1);

   for(%i = 0; %i < ClientGroup.getCount(); %i++)
   {
      %cl = ClientGroup.getObject(%i);
      if(!%cl.isAdmin)
      {
         messageClient(%cl, 'onClientKicked', "");
         if(%cl.isAIControlled())
         {
            $HostGameBotCount--;
            %cl.drop();
         }
         else
         {
            if(isObject(%cl.player))
               %cl.player.kill($DamageType::ScriptDamage);

            if(isObject(%cl))
            {
               %cl.setDisconnectReason( "Clearing server." );
	         %cl.schedule(700, "delete");
            }
	      BanList::add(%cl.guid, "0", 120);
         }
      }
   }
}

function serverCmdTogglePlayerMute(%client, %who)
{
   if (%client.muted[%who])
   {
      %client.muted[%who] = false;
      messageClient(%client, 'MsgPlayerMuted', '\c2%1 has been unmuted.', %who.playerName, %who, false);
   }
   else
   {
      %client.muted[%who] = true;
      messageClient(%client, 'MsgPlayerMuted', '\c2%1 has been muted.', %who.playerName, %who, true);
   }
}

function isOnAdminList(%client)
{
   if(!%totalRecords = getFieldCount($pref::Server::AdminList))
   {   
      return false;
   }

   for(%i = 0; %i < %totalRecords; %i++)
   {
      %record = getField( getRecord($pref::Server::AdminList, 0), %i);
      if(%record == %client.guid)
         return true;
   }
   return false;
}   

function isOnSuperAdminList(%client)
{
   if(!%totalRecords = getFieldCount($pref::Server::superAdminList))
   {   
      return false;
   }

   for(%i = 0; %i < %totalRecords; %i++)
   {
      %record = getField(getRecord($pref::Server::superAdminList, 0), %i);
      if(%record == %client.guid)
         return true;
   }
   return false;
}

function AddToAdminList(%admin, %client)
{
   %count = getFieldCount($pref::Server::AdminList);
   for(%i = 0; %i < %count; %i++)
   {
      %id = getField($pref::Server::AdminList, %i);
      if(%id == %client.guid)
         return;  // They're already there!
   }

   if(%count == 0)
      $pref::Server::AdminList = %client.guid;
   else
      $pref::Server::AdminList = $pref::Server::AdminList TAB %client.guid;

   export("$pref::*", ( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/server.config.cs" ), false);
   echo(%client.nameBase @ " " @ %client.guid @ " added to Admin list.", 1);
}

function AddToSuperAdminList(%client)
{
   %count = getFieldCount($pref::Server::SuperAdminList);
   for(%i = 0; %i < %count; %i++)
   {
      %id = getField($pref::Server::SuperAdminList, %i);
      if (%id == %client.guid)
         return; // They're already there!
   }

   if(%count == 0)
      $pref::Server::SuperAdminList = %client.guid;
   else
      $pref::Server::SuperAdminList = $pref::Server::SuperAdminList TAB %client.guid;

   export("$pref::*", ( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/client.config.cs" ), false);
   echo(%client.nameBase @ " " @ %client.guid @ " added to Super Admin list.", 1);
}

function getAdmin()
{
   %admin = 0;
   for ( %clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++ ) 
   {
      %cl = ClientGroup.getObject( %clientIndex );
      if(%cl.isAdmin || %cl.isSuperAdmin)
      {
         %admin = %cl;
         break;
      }
   }
   return %admin;   
}
