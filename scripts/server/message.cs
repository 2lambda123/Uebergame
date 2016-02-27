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

//-----------------------------------------------------------------------------
// Server side message commands
//-----------------------------------------------------------------------------

function messageClient(%client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
   //if ( !%client.isAiControlled() )
      commandToClient(%client, 'ServerMessage', %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
}

function messageTeam(%team, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
   %count = ClientGroup.getCount();
   for(%cl= 0; %cl < %count; %cl++)
   {
      %recipient = ClientGroup.getObject(%cl);
	if(%recipient.team == %team)
	   messageClient(%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
   }
}

function messageTeamExcept(%client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
   %team = %client.team;
   %count = ClientGroup.getCount();
   for(%cl= 0; %cl < %count; %cl++)
   {
      %recipient = ClientGroup.getObject(%cl);
	  if((%recipient.team == %team) && (%recipient != %client))
	      messageClient(%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
   }
}

function messageAll(%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{
   %count = ClientGroup.getCount();
   for(%cl = 0; %cl < %count; %cl++)
   {
      %client = ClientGroup.getObject(%cl);
      messageClient(%client, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
   }
}

function messageAllExcept(%client, %team, %msgtype, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13)
{  
   //can exclude a client, a team or both. A -1 value in either field will ignore that exclusion, so
   //messageAllExcept(-1, -1, $Mesblah, 'Blah!'); will message everyone (since there shouldn't be a client -1 or client on team -1).
   %count = ClientGroup.getCount();
   for(%cl= 0; %cl < %count; %cl++)
   {
      %recipient = ClientGroup.getObject(%cl);
      if((%recipient != %client) && (%recipient.team != %team))
         messageClient(%recipient, %msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13);
   }
}

//---------------------------------------------------------------------------
// Server side client chat'n
//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// silly spam protection...
$SPAM_PROTECTION_PERIOD     = 10000;
$SPAM_MESSAGE_THRESHOLD     = 4;
$SPAM_PENALTY_PERIOD        = 10000;
$SPAM_MESSAGE               = '\c3FLOOD PROTECTION:\cr You must wait another %1 seconds.';

function GameConnection::spamMessageTimeout(%this)
{
   if(%this.spamMessageCount > 0)
      %this.spamMessageCount--;
}

function GameConnection::spamReset(%this)
{
   %this.isSpamming = false;
}

function spamAlert(%client)
{
   if($Pref::Server::FloodProtectionEnabled != true)
      return(false);

   if(!%client.isSpamming && (%client.spamMessageCount >= $SPAM_MESSAGE_THRESHOLD))
   {
      %client.spamProtectStart = getSimTime();
      %client.isSpamming = true;
      %client.schedule($SPAM_PENALTY_PERIOD, spamReset);
   }

   if(%client.isSpamming)
   {
      %wait = mFloor(($SPAM_PENALTY_PERIOD - (getSimTime() - %client.spamProtectStart)) / 1000);
      messageClient(%client, "", $SPAM_MESSAGE, %wait);
      return(true);
   }

   %client.spamMessageCount++;
   %client.schedule($SPAM_PROTECTION_PERIOD, spamMessageTimeout);
   return(false);
}

//---------------------------------------------------------------------------

function chatMessageClient( %client, %sender, %voiceTag, %voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 )
{
   //see if the client has muted the sender
   if ( !%client.muted[%sender] )
      commandToClient( %client, 'ChatMessage', %sender, %voiceTag, %voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 );
}

function chatMessageTeam( %sender, %team, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 )
{
   if ( ( %msgString $= "" ) || spamAlert( %sender ) )
      return;
	
   %count = ClientGroup.getCount();
	
   for ( %i = 0; %i < %count; %i++ )
   {
      %obj = ClientGroup.getObject( %i );
      if ( %obj.team == %sender.team )
         chatMessageClient( %obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 );
   }

   if($pref::Server::EchoChat && !$pref::Server::TournamentMode)
      echo( "SAYTEAM: " @ %sender.nameBase @ ": ", %a2 );
}

function chatMessageAll( %sender, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 )
{
   if ( ( %msgString $= "" ) || spamAlert( %sender ) )
	return;
	   
   %count = ClientGroup.getCount();
	
   for ( %i = 0; %i < %count; %i++ )
   {
      %obj = ClientGroup.getObject( %i );
	   
	if(%sender.team != 0)
	   chatMessageClient( %obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 );
	else
	{
	   // message sender is an observer -- only send message to other observers
	   if(%obj.team == %sender.team)
	      chatMessageClient( %obj, %sender, %sender.voiceTag, %sender.voicePitch, %msgString, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10 );
	}
   }
   if($pref::Server::EchoChat)
      echo( "SAYGLOBAL: " @ %sender.nameBase @ ": ", %a2 );
}

//-----------------------------------------------------------------------------
// Server chat message handlers
//-----------------------------------------------------------------------------

function serverCmdTeamMessageSent(%client, %text)
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
   chatMessageTeam(%client, %client.team, '\c6%1:\c3 %2', %client.playerName, %text);
}

function serverCmdMessageSent(%client, %text)
{
   if ( %client.isGagged ) // Check for admin gag
      return;

   // We created a filter via scripting
   // Allow admins to control language on server
   if ( $pref::Server::BadWordFilter )
      %text = filterChat( %text );

   if ( strstr(%text, "\n") != -1 ) // Disallow \n chat spam
   {
      //%text = stripchars(%text, "\n");
      messageClient(%client, 'MsgSpam', '\c2Newline characters are not allowed in chat.');
      echo(%client.nameBase @ " tried to spam the server with new lines on global channel.");

      // Mute the offender
      %client.spamMessageCount = $SPAM_MESSAGE_THRESHOLD;
	
      return;
   }

   if ( strlen(%text) >= $pref::Server::MaxChatLen )
      %text = getSubStr( %text, 0, $pref::Server::MaxChatLen );

   // server/message.cs
   chatMessageAll(%client, '\c6%1:\c4 %2', %client.playerName, %text);
}

function serverCmdPrivateMessageSent(%client, %target, %text)
{
   // Client side:
   //commandToServer('PrivateMessageSent', %target, %text);

   // We want this client to be able to talk to admins regardless of gag status
   if ( ( %text $= "" ) || spamAlert( %client ) )
      return;

   if ( $pref::Server::BadWordFilter )
      %text = filterChat( %text );

   if( strstr(%text, "\n") != -1 ) // Disallow \n chat spam
   {
      //%text = stripchars(%text, "\n");
      messageClient(%client, 'MsgSpam', '\c2Newline characters are not allowed in chat.');
      echo(%client.nameBase @ " tried to spam the server with new lines on global channel.");	
      return;
   }
   if ( strlen(%text) >= $pref::Server::MaxChatLen )
      %text = getSubStr( %text, 0, $pref::Server::MaxChatLen );

   chatMessageClient( %target, %client, %client.voiceTag, %client.voicePitch, '\c5Message from %1: \c3%2', %client.playerName, %text);
   //messageClient(%target, 'MsgPrivate', '\c5Message from %1: \c3%2', %client.playerName, %text);
}

//-----------------------------------------------------------------------------
// Bad language filter
//-----------------------------------------------------------------------------

function filterChat(%string)
{
   // Looks like They added this to TGE 1.5
   // Lets take advantage..
   if ( containsBadWords( %string ) )
      return( filterString( %string, "-" ) );
   else
      return( %string );
}

// Bad words are stored in a file so the admin can add or remove from the list
exec(GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/badWordList.cs");