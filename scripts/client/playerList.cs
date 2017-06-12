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
// Hook into the client update messages to maintain our player list
// and scoreboard.
//-----------------------------------------------------------------------------
addMessageCallback( 'MsgYourInfo', handleGetMyInfo ); // Sent in ::onConnect
addMessageCallback( 'MsgClientJoin', handleClientJoin );
addMessageCallback( 'MsgClientDrop', handleClientDrop );
addMessageCallback( 'MsgClientScoreChanged', handleClientScoreChanged );
addMessageCallback( 'MsgClientNameChanged', handleClientNameChanged );
addMessageCallback( 'MsgClientJoinTeam', handleClientTeamChanged );
addMessageCallback( 'MsgAdminPlayer', handleClientAdminChanged );
addMessageCallback( 'MsgStripAdmin', handleClientAdminStripped );

function handleGetMyInfo(%msgType, %msgString, %clientName, %clientId)
{
   //LogEcho(" handleGetMyInfo(" SPC %clientName @", "@ %clientId SPC ")");
   $MyClient = %clientId;
}

function handleClientJoin(%msgType, %msgString, %clientName, %clientId, %guid, %isAI, %isAdmin, %isSuperAdmin, %team, %fireTeam, %ping, %pl)
{
   //%name = stripChars(detag(getTaggedString(%clientName)), "\cp\co\c6\c7\c8\c9");

   %name = StripMLControlChars(%clientName);
   //LogEcho("handleClientJoin(" SPC %name @", "@ %clientId @", "@ %guid SPC ")");

   // Create the player list group, and add it to the ClientConnectionGroup...
   if(!isObject("PlayerListGroup"))
   {
      %newGroup = new SimGroup("PlayerListGroup");
      ClientConnectionGroup.add(%newGroup);
   }

   // Create the player object and place it in the group
   $PlayerList[%clientId] = new ScriptObject()
   {
      className = "PlayerRep";
      playerName = %name;
      guid = %guid;
      clientId = %clientId;
      teamId = %team;
      fireTeamId = %fireTeam;
      score = 0;
      kills = 0;
      deaths = 0;
      suicides = 0;
      teamkills = 0;
      ping = %ping;
      packetLoss = %pl;
      chatMuted = false;
      isBot = %isAI;
      isAdmin = %isAdmin;
      isSuperAdmin = %isSuperAdmin;
      chatMuted = false;
   };
   PlayerListGroup.add($PlayerList[%clientId]);

   if ( !%isAI )
      PlayerListGui.updatePlayerInfo($PlayerList[%clientId]);
}

function handleClientDrop(%msgType, %msgString, %clientName, %clientId)
{
   // Find the player in the player array and remove the object from the group
   %player = $PlayerList[%clientId];
   if ( isObject(%player) )
   {
      PlayerListGuiList.removeRowById(%clientId);
      %player.delete();
      $PlayerList[%clientId] = "";
   }
}

function handleClientScoreChanged(%msgType, %msgString, %clientId, %score, %kills, %deaths, %suicides, %tks)
{
   // Find the player in the player array and update it's objects score
   %player = $PlayerList[%clientId];
   if ( isObject(%player) )
   {
      %player.kills = %kills;
      %player.score = %score;
      %player.deaths = %deaths;
      %player.suicides = %suicides;
      %player.teamkills = %tks;
      PlayerListGui.updatePlayerInfo(%player);
   }
}

function handleClientTeamChanged(%msgType, %msgString, %clientId, %team, %teamName, %clientName)
{
   // Look for this player in the player array and update it's objects team
   %player = $PlayerList[%clientId];
   if ( isObject(%player) )
   {
      %player.teamId = %team;
      PlayerListGui.updatePlayerInfo(%player);
   }
}

function handleClientNameChanged( %msgType, %msgString, %oldName, %newName, %clientId )
{
   %player = $PlayerList[%clientId];
   if ( isObject(%player) )
   {
      //%player.playerName = stripChars(detag(getTaggedString(%newName)), "\cp\co\c6\c7\c8\c9");
      %player.playerName = StripMLControlChars(%newName);
      PlayerListGui.updatePlayerInfo(%player);
   }
}

// Entered SAD or was awarded admin
function handleClientAdminChanged(%msgType, %msgString, %a1, %clientId, %clientName, %isSuperAdmin)
{
   // Find the player in the player array and remove the object from the group
   %player = $PlayerList[%clientId];
   if ( isObject(%player) )
   {
      if ( %isSuperAdmin )
         %player.isSuperAdmin = true;
      else
         %player.isAdmin = true;

      PlayerListGui.updatePlayerInfo(%player);
   }
}

// Admin privileges revoked by superadmin
function handleClientAdminStripped(%msgType, %msgString, %clientId)
{
   %player = $PlayerList[%clientId];
   if ( isObject(%player) )
   {
      %player.isSuperAdmin = false;
      %player.isAdmin = false;
      PlayerListGui.updatePlayerInfo(%player);
   }
}

function PlayerListGui::updatePlayerInfo(%this, %player)
{
   //error("PlayerListGui::updatePlayerInfo(" SPC %player.clientId @", "@ %player.playerName SPC ")");

   if ( !isObject( %player ) )
      return;

   // Build the text:
   %tag = %player.isSuperAdmin ? "[UeberAdmin]" : (%player.isAdmin ? "[Admin]" : (%player.isBot ? "[Bot]" : ""));

   if ( $Client::TeamCount > 1 )
   {
      if ( %player.teamId == 0 )
         %teamName = "Spectator";
      else
         %teamName = $Client::TeamName[%player.teamId, 0] $= "" ? "-" : $Client::TeamName[%player.teamId, 0];

      %text = %player.playerName SPC %tag TAB %teamName TAB %player.score;
   }
   else
   {
      if( %player.teamId == 0 )
         %text = %player.playerName SPC %tag TAB "Spectator" TAB %player.score;
      else
         %text = %player.playerName SPC %tag TAB "Combatant" TAB %player.score;
   }

   // Update or add the player to the control
   if (PlayerListGuiList.getRowNumById(%player.clientId) == -1)
      PlayerListGuiList.addRow(%player.clientId, %text);
   else
      PlayerListGuiList.setRowById(%player.clientId, %text);

   //Perform a numerical sort on the values in the specified column.
   PlayerListGuiList.sortNumerical(2, false);
}

function PlayerListGui::clear(%this)
{
   // Override to clear the list.
   PlayerListGuiList.clear();
}

function PlayerListGui::zeroScores(%this)
{
   for ( %i = 0; %i < PlayerListGroup.getCount(); %i++ )
   {
      %player = PlayerListGroup.getObject(%i);
      %player.score = 0;
      %player.kills = 0;
      %player.deaths = 0;
      %player.suicides = 0;
      %player.teamkills = 0;
      PlayerListGui.updatePlayerInfo(%player);
   }

   for ( %j = 0; %j < PlayerListGuiList.rowCount(); %j++ )
   {
      %text = PlayerListGuiList.getRowText(%j);
      %text = setField(%text, 2, "0");
      PlayerListGuiList.setRowById(PlayerListGuiList.getRowId(%j), %text);
   }
   PlayerListGuiList.clearSelection();
}

function echoPlayerList()
{
   for ( %i = 0; %i < PlayerListGroup.getCount(); %i++ )
   {
      %player = PlayerListGroup.getObject( %i );
      echo( "Player Name:" SPC %player.playerName SPC "Index:" SPC %i );
   }
}

function PlayerListGui::GetPlayerListUpdate(%this)
{
   cancel($PlayerListUpdate);
   if ( %this.isAwake() )
      commandToServer('SendPlayerListUpdate');
}

addMessageCallback( 'MsgPlyrListUpd', handlePlayerListUpdate );
//This just updates the clients network info. Might want to dump this for performance reasons.
function handlePlayerListUpdate(%msgType, %msgString, %clientId, %ping, %pl)
{
   PlayerListGui.updateNetwork(%clientId, %ping, %pl);
   $PlayerListUpdate = PlayerListGui.schedule(1000, "GetPlayerListUpdate");
}

function PlayerListGui::updateNetwork(%this, %clientId, %ping, %pl)
{
   %player = PlayerListGuiList.getRowTextById(%clientId);

   %netOne = setField(%player, 3, %ping);
   PlayerListGuiList.setRowById(%clientId, %netOne);
   %netTwo = setField(%player, 4, %pl);
   PlayerListGuiList.setRowById(%clientId, %netTwo);
   //Perform a numerical sort on the values in the specified column.
   PlayerListGuiList.sortNumerical(2, false);
}

function isClientChatMuted(%client)
{
   %player = $PlayerList[%client];
   if ( isObject(%player) )
      return(%player.chatMuted ? true : false);

   return(true);
}
