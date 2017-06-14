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

// Spectators cannot be in a fire team
// Fire teams are attached to the client object
// Need a way to handle permission. So that a fire team leader can accept/reject joins etc.
// Unfinished
function Torque::setupFireTeams(%this)
{
   %max = 12;
   $FireTeam[0] = new ScriptObject() { class = Alpha; count = 0; maxPlayers = %max; };
   $FireTeam[1] = new ScriptObject() { class = Beta; count = 0; maxPlayers = %max; };
   $FireTeam[2] = new ScriptObject() { class = Gamma; count = 0; maxPlayers = %max; };
   $FireTeam[3] = new ScriptObject() { class = Delta; count = 0; maxPlayers = %max; };
   $FireTeam[4] = new ScriptObject() { class = Epsilon; count = 0; maxPlayers = %max; };
   $FireTeam[5] = new ScriptObject() { class = Zeta; count = 0; maxPlayers = %max; };

   for ( %i = 0; $FireTeam[%i] !$= ""; %i++ )
      ScriptClassGroup.add( $FireTeam[%i] );
}

tge.setupFireTeams();

//-----------------------------------------------------------------------------

function serverCmdcreateFireTeam(%client, %index)
{
   // First, is the client in one allready? If so they must leave it first.
   // Second, pick one that isn't in use. If there aren't any, return a message.
   // Third, create it or notify client there isn't one available.

   if ( %client.team == 0 )
      return;

   if ( !isObject( %client.fireTeam ) )
   {
      %fireTeam = $FireTeam[%index];
      if ( isObject( %fireteam ) )
      {
         if ( $FireTeam[%index].creator $= "" )
         {
            %fireTeam.creator = %client;
            %fireTeam.player[%fireTeam.count++] = %client;
            %client.fireTeam = %fireTeam;
            messageClient(%client, 'MsgFireTeam', '\c2Fire team %1 successfully created.', %fireTeam.class);
            commandToClient(%client, 'ToggleFireTeamHud', 1);

            // Update the hud with list of players
            for ( %i = 1; %fireTeam.player[%i] !$= ""; %i++ )
               messageFireTeam(%fireTeam, 'MsgFTPlayers', "", %fireTeam.class, %fireTeam.player[%i], %i);
         }
         else
            messageClient(%client, 'MsgFireTeam', '\c2Fire team %1 already created by %1.', %fireTeam.class, %fireTeam.creator.playerName);
      }
      else
         messageClient(%client, 'MsgFireTeam', '\c2Fire team %1 does not exist.', %fireTeam.class);
   }
   else
      messageClient(%client, 'MsgFireTeam', '\c2You are already in a fire team. You must leave it before joining another.');
}

function serverCmdjoinFireTeam(%client, %class)
{
   // First, is the client in one allready? If so they must leave it first.
   // Second, Does this fire team exist? If not, return a message.
   // Third, is the selected fire team full? If so, return a message.
   // Fourth, does this fire teams creator allow this? If not, return a message.

   if ( %client.team == 0 )
      return;

   if ( !isObject( %client.fireTeam ) )
   {
      // Find the fire team object based on the class passed
      for ( %i = 0; $FireTeam[%i] !$= ""; %i++ )
      {
         if ( $FireTeam[%i].class $= %class )
         {
            %fireteam = $FireTeam[%i];
            break;
         }
      }

      if ( isObject( %fireTeam ) )
      {
         if ( %fireTeam.count < %fireTeam.maxPlayers )
         {
            %fireTeam.player[%fireTeam.count++] = %client;
            %client.fireTeam = %fireTeam;
            messageClient(%client, 'MsgFireTeam', '\c2Welcome to fire team %1 %2.', %fireTeam.class, %client.playerName);
            commandToClient(%client, 'ToggleFireTeamHud', 1);

            // Update the hud with list of players
            for ( %i = 1; %fireTeam.player[%i] !$= ""; %i++ )
               messageFireTeam(%fireTeam, 'MsgFTPlayers', "", %fireTeam.class, %fireTeam.player[%i], %i);
         }
         else
            messageClient(%client, 'MsgFireTeam', '\c2Fire team %1 is full.', %fireTeam.class);
      }
      else
         messageClient(%client, 'MsgFireTeam', '\c2Fire team %1 does not exist.', %fireTeam.class);
   }
   else
      messageClient(%client, 'MsgFireTeam', '\c2You are already in fire team %1.', %client.fireTeam.class);
}

function serverCmdleaveFireTeam(%client)
{
   // First, is the client in a fire team? If not do nothing.
   // Second, is this client the creator? If so we need to hand ownership to someone else if any.
   if ( isObject( %client.fireTeam ) )
   {
      %fireTeam = %client.fireTeam;

      // Re-sort the fire teams clients which should remove this one in the process
      %count = 0;
      for ( %i = 1; %i <= %fireTeam.maxPlayers; %i++ )
      {
         if ( %fireTeam.player[%i] != %client )
         {
            %Temp[%count++] = %fireTeam.player[%i];
         }
      }

      %fireTeam.count = %count;
      for( %j = 0; %j < %count; %j++ )
      {
         %fireTeam.player[%j] = %Temp[%j];
      }

      // If this client creatd the team, hand it off to someone else
      if ( %fireTeam.creator == %client )
      {
         %fireTeam.creator = "";
         for ( %i = 1; %i <= %fireTeam.maxPlayers; %i++ )
         {
            if ( %fireTeam.player[%i] !$= "" )
            {
               %fireTeam.creator = %fireTeam.player[%i];
               break;
            }
         }
      }
      %client.fireTeam = "";
      commandToClient(%client, 'ToggleFireTeamHud', 0);
      messageClient(%client, 'MsgFireTeam', '\c2You have left fire team %1.', %fireTeam.class);
      messageFireTeam(%fireteam, 'MsgFireTeam', '\c2%1 has left the fire team.', %client.playerName);

      // Update the hud with list of players
      for ( %i = 1; %fireTeam.player[%i] !$= ""; %i++ )
         messageFireTeam(%fireTeam, 'MsgFTPlayers', "", %fireTeam.class, %fireTeam.player[%i], %i);
   }
}

function serverCmddisbandFireTeam(%client)
{
   // First, is the client in a fire team? If not do nothing.
   // Second, did this client create the team they are in? If not, return a message.
   if ( isObject( %client.fireTeam ) )
   {
      %fireTeam = %client.fireTeam;
      if ( %fireTeam.creator == %client )
      {
         messageFireTeam(%fireTeam, 'MsgFireTeam', '\c2%1 has disbanded the fire team', %client.nameBase);
         for ( %i = 1; %i <= %fireTeam.maxPlayers; %i++ )
         {
            commandToClient(%fireTeam.player[%i], 'ToggleFireTeamHud', 0);
            %fireTeam.player[%i].client.fireteam = "";
            %fireTeam.player[%i] = "";
         }
         %fireTeam.creator = "";
         %fireTeam.count = 0;
      }
   }
}

function serverCmdinviteFireTeam(%client, %target)
{
   // First, is the target in one allready? If so they must leave it first. Return a message.
   // Second, did this client create the team they are in? If not do nothing.
   if ( %client == %target ) // Can't invite yourself, accidents happen.
      return;

   if ( !isObject( %target.fireTeam ) )
   {
      if ( isObject( %client.fireTeam ) )
      {
         %fireTeam = %client.fireTeam;
         if ( %fireTeam.creator == %client )
         {
            commandToClient(%target, 'fireTeamInvitation', %client.playerName, addTaggedString(%fireTeam.class));
         }
      }
   }
}

function serverCmdkickFireTeam(%client, %target)
{
   // First, is the client in a fire team? If not do nothing.
   // Second, is the target in this clients fire team? If not do nothing.
   // Third, did this client create the team they are in? If not do nothing.

   if ( %client == %target ) // Can't kick yourself, accidents happen.
      return;

   if ( isObject( %client.fireTeam ) && isObject( %target.fireTeam ) )
   {
      if ( %client.fireTeam == %target.fireTeam )
      {
         %fireTeam = %client.fireTeam;
         if ( %fireTeam.creator == %client )
         {
            // Re-sort the fire teams clients which should remove this one in the process
            %count = 0;
            for ( %i = 1; %i <= %fireTeam.maxPlayers; %i++ )
            {
               if ( %fireTeam.player[%i] != %target )
               {
                  %Temp[%count++] = %fireTeam.player[%i];
               }
            }

            %fireTeam.count = %count;
            for( %j = 0; %j < %count; %j++ )
            {
               %fireTeam.player[%j] = %Temp[%j];
            }

            %target.fireTeam = "";
            commandToClient(%target, 'ToggleFireTeamHud', 0);
            messageClient(%target, 'MsgFireTeam', '\c2You have been kicked from fire team %1 by %2.', %fireTeam.class, %client.playerName);
            messageFireTeam(%fireteam, 'MsgFireTeam', '\c2%1 was kicked from the fire team.', %target.playerName);

            // Update the hud with list of players
            for ( %i = 1; %fireTeam.player[%i] !$= ""; %i++ )
               messageFireTeam(%fireTeam, 'MsgFTPlayers', "", %fireTeam.class, %fireTeam.player[%i], %i);
         }
      }
   }
}

function serverCmdacceptInvite(%client, %fireTeam)
{
   messageClient(%target, 'MsgFireTeam', '\c2Sorry, fire team invites are not implemented yet.');
}

function serverCmddeclineInvite(%client, %fireTeam)
{

}

//-----------------------------------------------------------------------------

function serverCmdGetFireTeamMenu(%client, %key)
{
   for ( %i = 0; $FireTeam[%i] !$= ""; %i++ )
   {
      if ( $FireTeam[%i].creator $= "" )
      {
         messageClient( %client, 'MsgVoteItem', "", %key, %i, $FireTeam[%i].class, true);
      }
   }
}

function serverCmdGetJoinFireTeamMenu(%client, %key)
{
   for ( %i = 0; $FireTeam[%i] !$= ""; %i++ )
   {
      if ( $FireTeam[%i].creator !$= "" )
      {
         messageClient( %client, 'MsgVoteItem', "", %key, %i, $FireTeam[%i].class, true);
      }
   }
}

//-----------------------------------------------------------------------------

function serverCmdFireTeamMessageSent(%client, %text)
{
   if ( %client.isGagged ) // Check for admin gag
      return;

   // We created a filter via scripting
   // Allow admins to control language on server
   if ( $pref::Server::BadWordFilter )
      %text = filterChat( %text );

   if ( strstr(%text, "\n") != -1 ) //Disallow \n chat spam
   {
      //%text = stripchars(%text, "\n");
      messageClient(%client, 'MsgSpam', '\c2Newline characters are not allowed in chat.');
      echo(%client.nameBase @ " tried to spam the server with new lines on teamchat channel.");

      // Mute the offender
      %client.spamMessageCount = $SPAM_MESSAGE_THRESHOLD;

      return;
   }

   if ( strlen(%text) >= $pref::Server::MaxChatLen )
      %text = getSubStr( %text, 0, $pref::Server::MaxChatLen );

   // server/message.cs
   chatMessageFireTeam(%client, %client.fireTeam, '\c6%1:\c3 %2', %client.playerName, %text);
}

function chatMessageFireTeam( %sender, %fireTeam, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 )
{
   if ( !isObject( %fireTeam ) )
      return;

   if ( ( %msgString $= "" ) || spamAlert( %sender ) ||  %sender.fireTeam != %fireTeam )
      return;
	
   %count = ClientGroup.getCount();
	
   for ( %i = 0; %i < %count; %i++ )
   {
      %obj = ClientGroup.getObject( %i );
      if ( %obj.fireTeam == %sender.fireTeam )
         chatMessageClient( %obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 );
   }

   if ( $pref::Server::EchoChat && !$pref::Server::TournamentMode )
      echo( "SAYFT: " @ %sender.nameBase @ ": ", %a2 );
}

function messageFireTeam(%fireTeam, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
   if ( !isObject( %fireTeam ) )
      return;

   %count = ClientGroup.getCount();
   for ( %cl= 0; %cl < %count; %cl++ )
   {
      %recipient = ClientGroup.getObject(%cl);
	if ( %recipient.fireTeam == %fireTeam )
	   messageClient(%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
   }
}
