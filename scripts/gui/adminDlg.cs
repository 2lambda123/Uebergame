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

AdminDlg.initialized = false; //set variable to fix non-initiated PlayerPopupMenu

function AdminDlg::toggle(%this, %val)
{
   if(%this.isAwake())
      Canvas.popDialog(%this);
   else
      Canvas.pushDialog(%this);
}

function clientCmdToggleAdminDlg(%val)
{
   AdminDlg.toggle(%val);
}

function AdminDlg::onWake(%this)
{
   // Add the Player admin feature menu:
   if ( !%this.initialized )
   {
      // Add the Player popup menu:
      new GuiControl(PlayerAdminActionDlg) {
         profile = "GuiModelessDialogProfile";
         horizSizing = "relative";
         vertSizing = "relative";
         position = "0 0";
         extent = "640 480";
         minExtent = "8 8";
         visible = "0";
         setFirstResponder = "0";
         modal = "1";

         new GuiPopUpMenuCtrl(PlayerPopupMenu) {
            profile = "GuiPopUpMenuProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "0 0";
            extent = "200 25";
            minExtent = "200 25";
            visible = "1";
            maxLength = "255";
            maxPopupHeight = "200";
            noButtonStyle = "1";
            key = "0";
         };
      };
      %this.initialized = true;
   }

   %headerStyle = "<font:Arial:14><color:7777ff>";
   %statusText = "<spop><spush>" @ %headerStyle @ "SERVER NAME:<spop>" SPC $Client::ServerName
         NL "<spush>" @ %headerStyle @ "MISSION TYPE:<spop>" SPC $Client::MissionTypeName 
         NL "<spush>" @ %headerStyle @ "MISSION NAME:<spop>" SPC $Client::MissionName
         NL "<spush>" @ %headerStyle @ "DESCRIPTION:<spop>";

   for(%i = 0; %this.qLine[%i] !$= ""; %i++)
      %statusText = %statusText NL "<lmargin:10>* <lmargin:24>" @ %this.qLine[%i];

   AdminStatusText.setText(%statusText);

   // Fill the action menu
   ESC_VoteMenu.fillVoteMenu();

   if($PlayerList[$MyClient].isAdmin)
   {
      // Fill the drop down for special admin functions
      ESC_CmdMenu.setVisible(true);
      ESC_CmdInput.setVisible(true);
      ESC_SendBtn.setVisible(true);
      commandToServer('GetRemoteCmdMenu');
   }

   if ( isObject( hudMap ) )
   {
      hudMap.pop();
      hudMap.delete();
   }
   new ActionMap( hudMap );
   hudMap.blockBind( moveMap, bringUpOptions );
   hudMap.blockBind( moveMap, toggleTeamChoose );
   hudMap.blockBind( moveMap, showScoreBoard );
   hudMap.push();
}

function AdminDlg::onSleep(%this)
{
   // Make sure the proper key maps are pushed
   tge.updateKeyMaps();
}

// Player pop-up stuffs
//-----------------------------------------------------------------------------
function PlayerListGuiList::onSelect(%this, %id, %text)
{
   //LogEcho("PlayerListGuiList::onSelect(" SPC %this.getName() SPC %id SPC %text SPC ")");
   // Get the client id from the player list array
   PlayerPopupMenu.player = $PlayerList[%id];
   PlayerPopupMenu.key++;
   PlayerPopupMenu.clear();

   // For looks only
   PlayerPopupMenu.setValue(PlayerPopupMenu.player.playerName);
   //PlayerPopupMenu.setSelected(0);
   //PlayerPopupMenu.onSelect(0, "");

   // Get the list of commands we can perform on this selected client
   commandToServer('SendPlayerPopupMenu', PlayerPopupMenu.player.clientId, PlayerPopupMenu.key);
}

function PlayerListGuiList::onRightMouseDown(%this, %column, %row, %mousePos)
{
   //LogEcho("PlayerListGuiList::onRightMouseDown(" SPC %this.getName() SPC %column SPC %row SPC %mousePos SPC ")");
   //PlayerPopupMenu.player = $PlayerList[ %this.getRowId(%row) ];
   // We are getting the client id from the lists onSelect because if we do it here it screws up the button
   if ( PlayerPopupMenu.player.playerName !$= "" )
   {
      // Place the button where our mouse is and make it visible
      PlayerPopupMenu.position = %mousePos;
      Canvas.pushDialog(PlayerAdminActionDlg);

      // For looks only
      PlayerPopupMenu.setValue(PlayerPopupMenu.player.playerName);

      // Force the opening of the drop down, this is for ease of use
      PlayerPopupMenu.forceOnAction();
   }
}

function PlayerAdminActionDlg::onWake(%this)
{

}

function LobbyPlayerActionDlg::onSleep(%this)
{

}

function PlayerPopupMenu::onSelect(%this, %id, %text)
{
   //LogEcho("PlayerPopupMenu::onSelect(" SPC %this.getName() SPC %id SPC %text SPC ")");
   switch(%id)
   {
      case 1: // Personal mute target
         commandToServer('togglePlayerMute', %this.player.clientId);

      case 2: // Vote to make target admin
         commandToServer('InitVote', "VoteAdminPlayer", %this.player.clientId, 0, 0, 0, $MyClient.isAdmin);

      case 3: // Vote to kick target
         commandToServer('InitVote', "VoteKickPlayer", %this.player.clientId, 0, 0, 0, $MyClient.isAdmin);

      case 4: // Vote to ban target
         commandToServer('InitVote', "VoteBanPlayer", %this.player.clientId, 0, 0, 0, $MyClient.isAdmin);

      case 5: // force spectator
         commandToServer('InitAdminCommand' , "forceClientToSpectator", %this.player.clientId);

      case 6: //change team 1
         commandToServer('ChangePlayersTeam', %this.player.clientId, 1);
         //commandToServer('InitAdminCommand', "changePlayersTeam", %this.player.clientId, 1);
      
      case 7: //change team 2
         commandToServer('ChangePlayersTeam', %this.player.clientId, 2);
         //commandToServer('InitAdminCommand', "changePlayersTeam", %this.player.clientId, 2);

      case 8:
         commandToServer('clientAddToGame', %this.player.clientId );

      case 9: // Add to admin list
         commandToServer('InitAdminCommand', "AddToAdminList", %this.player.clientId);

      case 10: // Add to super admin list
         commandToServer('InitAdminCommand', "AddToSuperList", %this.player.clientId);

      case 11: // Text warn target
         commandToServer('InitAdminCommand', "WarnPlayer", %this.player.clientId);

      case 12: // Strip targets admin rights
         commandToServer('InitAdminCommand', "RemoveAdmin", %this.player.clientId);

      case 13: // Print targets client information
         commandToServer('InitAdminCommand', "GetClientInfo", %this.player.clientId);

      case 14: // Gag target
         commandToServer('InitAdminCommand', "GagPlayer", %this.player.clientId);

      case 15: // Send private message
         PrivateMessage(%this.player.clientId);

      case 16: // Invite to fire team
         commandToServer('inviteFireTeam', %this.player.clientId);
         echo("Invite" SPC $PlayerList[%this.player.clientId].playerName SPC "to fire team");

      case 17: // Kick from fire team
         commandToServer('kickFireTeam', %this.player.clientId);
         echo("Kick" SPC $PlayerList[%this.player.clientId].playerName SPC "from fire team");

      case 18: // Leave fire team
         commandToServer('leaveFireTeam', %this.player.clientId);
         echo($PlayerList[%this.player.clientId].playerName SPC "leaving fire team");
   }
   Canvas.popDialog(PlayerAdminActionDlg);
}

function PlayerPopupMenu::onCancel(%this)
{
   Canvas.popDialog(PlayerAdminActionDlg);
}

function ESC_ServerListPane::onWake(%this)
{
   ESC_VoteMenu.clear();
}

function ESC_VoteMenu::onSelect(%this, %id, %text)
{
}

function ESC_VoteMenu::onRightMouseDown(%this, %column, %row, %mousePos)
{
   %id = %this.getSelectedId();
   %text = %this.getRowTextById(%id);

   //error("ESC_VoteMenu::onRightMouseDown(" SPC %this.type @", "@  $Client::VoteCmd[%id] @", "@  %text SPC ")");
   switch$ (%this.type)
   {
      case "":
         switch$ ($Client::VoteCmd[%id])
         {
            case "VoteTournamentMode":
               %this.tourneyChoose = 1;
               %this.fillMissionTypeMenu();
               return; 

            case "VoteMatchStart":
               %this.startVote("VoteMatchStart", %text);
               return;

            case "VoteChangeMission":
               %this.fillMissionTypeMenu();
               return;

            case "VoteSkipMission":
               %this.startVote("VoteSkipMission", %text);
               return;

            case "VoteChangeTimeLimit":
               %this.fillTimeLimitMenu();
               return;
			   
			case "VoteAddBots":
               %this.fillAddBotsMenu();
               return;

            //case "VoteResetServer":

            //case "VoteClearServer":

            case "CreateFireteam":
               %this.fillFireTeamMenu();
               return;

            case "JoinFireteam":
               %this.fillJoinFireTeamMenu();
               return;
         }

      case "team":
         commandToServer('ClientJoinTeam', %id++);
         return;

      case "type":
         %this.fillMissionMenu($Client::VoteCmd[%id], %text);
         return;

      case "mission":
         if( !%this.tourneyChoose )
         {
            %this.startVote( "VoteChangeMission", 
                  %text,                 // Mission display name 
                  %this.typeName,        // Mission type display name 
                  $Client::VoteCmd[%id], // Mission id                              
                  %this.missionType );   // Mission type id
         }
         else
         {
            %this.startVote( "VoteTournamentMode", 
                  %text,                 // Mission display name
                  %this.typeName,        // Mission type display name
                  $Client::VoteCmd[%id], // Mission id
                  %this.missionType );   // Mission type id

            %this.tourneyChoose = 0;
         }
         %this.reset();
         return;

      case "timeLimit":
         %this.startVote("VoteChangeTimeLimit", $Client::VoteCmd[%id]);
         %this.reset();
         return;
		 
      case "addBots":
         %this.startVote("VoteAddBots", $Client::VoteCmd[%id]);
         %this.reset();
         return;

      case "fireteamcreate":
         commandToServer('createFireTeam', $Client::VoteCmd[%id]);
         %this.reset();
         return;

      case "fireteamjoin":
         commandToServer('joinFireTeam', %text);
         %this.reset();
         return;

   }
   %this.fillVoteMenu();
   %this.startVote($Client::VoteCmd[%id]);
}

function ESC_VoteMenu::reset(%this)
{
   // Clear the vote menu options list and repopulate it
   %this.type = "";
   VoteCancelBtn.setVisible(false);
   %this.fillVoteMenu();
}

function ESC_VoteMenu::fillVoteMenu(%this)
{
   // Get the list of vote options from the server
   %this.key++;
   %this.clear();
   VoteCancelBtn.setVisible(false);
   commandToServer('GetVoteMenu', %this.key);
}

function ESC_VoteMenu::fillMissionTypeMenu(%this)
{
   // Get the list of gametypes from the server for selection
   %this.key++;
   %this.clear();
   %this.type = "type";
   commandToServer('GetMissionTypeList', %this.key);
   VoteCancelBtn.setVisible(true);
}

function ESC_VoteMenu::fillMissionMenu(%this, %type, %name)
{
   // Get the list of missions from the server for the selected gametype
   %this.key++;
   %this.clear();
   %this.type = "mission";
   %this.missionType = %type;
   %this.typeName = %name;
   commandToServer('GetMissionList', %this.key, %type);
}

function ESC_VoteMenu::fillTimeLimitMenu(%this)
{
   // Get the time limit list from the server
   %this.key++;
   %this.clear();
   %this.type = "timeLimit";
   commandToServer('GetTimeLimitList', %this.key);
   VoteCancelBtn.setVisible(true);
}

function ESC_VoteMenu::fillAddBotsMenu(%this)
{
   // Get the add bots list from the server
   %this.key++;
   %this.clear();
   %this.type = "addBots";
   commandToServer('GetAddBotsList', %this.key);
   VoteCancelBtn.setVisible(true);
}

function ESC_VoteMenu::fillFireTeamMenu(%this)
{
   // Get the list of fire teams from the server
   %this.key++;
   %this.clear();
   %this.type = "fireteamcreate";
   commandToServer('GetFireTeamMenu', %this.key);
}

function ESC_VoteMenu::fillJoinFireTeamMenu(%this)
{
   // Get the list of fire teams from the server
   %this.key++;
   %this.clear();
   %this.type = "joinfireteam";
   commandToServer('GetJoinFireTeamMenu', %this.key);
   VoteCancelBtn.setVisible(true);
}

function ESC_VoteMenu::startVote(%this, %name, %val1, %val2, %val3, %val4, %playerVote)
{
   //error("ESC_VoteMenu::startVote("@%this.getName() @", "@ %name @", "@ %val1 @", "@ %val2 @", "@ %val3 @", "@ %val4 @", "@ %playerVote @")");

   if(%val1 $="") %val1 = 0;
   if(%val2 $="") %val2 = 0;
   if(%val3 $="") %val3 = 0;
   if(%val4 $="") %val4 = 0;
   if(%playerVote $="") %playerVote = 0;

   commandToServer('InitVote', %name, %val1, %val2, %val3, %val4, %admin);
}

function setPlayerVote(%vote)
{
   commandToServer('setPlayerVote', %vote);
}

function ClientCmdVoteSubmitted(%type)
{
   clientCmdClearBottomPrint();
   
   // #investigage and decide if these sound effects should be used or not
   //if(%type)
   //   alxPlay(VoteNoSound, 0, 0, 0);
   //else
   //   alxPlay(VoteYesSound, 0, 0, 0);
}

function PrivateMessage(%clientId)
{
   $PrivMsgTarget = %clientId;
   %notice = "\c2Next message you send will be private to: " @ $PlayerList[%clientId].playerName;
   onChatMessage(%notice);
}

function ClientCmdFillCmdMenuDropdown(%text)
{
   %list = deTag(%text);
   //LogEcho("ClientCmdFillCmdMenuDropdown(" SPC %text SPC ")");

   // Populate the cmd drop down
   ESC_CmdMenu.clear();
   for(%i = 0; %i < getFieldCount(%list); %i++)
   {
      //error(getField(%list, %i));
      ESC_CmdMenu.add(getField(%list, %i), %i);
   }
   ESC_CmdMenu.setSelected(0);
   ESC_CmdMenu.onSelect(0, "");
}

function ESC_CmdMenu::onSelect(%this, %id, %text)
{
   //error("ESC_CmdMenu::onSelect(" SPC %this.getName() SPC %id SPC %text SPC ")");
   if(%text $= "")
      return;

   $AdminID = %id;
   $AdminMenu = %text;
}

function ESC_CmdInput::setField(%this)
{
   // called when you type in text input field
   %value = %this.getValue();
   %this.setValue(%value);
   $AdminInput = %value;
   ESC_SendBtn.setActive(strlen(trim(%value)) >= 1);
}

function ESC_CmdInput::processEnter(%this)
{
   // Called when you press enter in text input field
}

function ESC_SendBtn::adminCommand(%this)
{
   //error("ESC_SendBtn::adminCommand(" SPC %this.getName() SPC ")");
   // Called when you press the send button

   // Update the global from the text input field
   ESC_CmdInput.setField();

   // Send the current menu selection and text to the server
   commandToServer('InitAdminCommand', $AdminId, $AdminInput);

   // Clear the text input field and disable send button
   ESC_SendBtn.setActive(0);
   ESC_CmdInput.setValue("");

   // Fill the drop down for special admin functions
   commandToServer('GetRemoteCmdMenu');
   $AdminInput = "";
   $AdminMenu = "";
}
