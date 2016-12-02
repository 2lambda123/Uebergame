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

//Functions which allow you to query the LAN or a master server for online games
//queryLanServers
//queryMasterServer
//querySingleServer(Address, Flags)
//cancelServerQuery
//stopServerQuery
//startHeartbeat
//stopHeartbeat
//getServerCount
//setServerInfo(index)

// Filter Flags:
//Dedicated      (0)
//NotPassworded  (1)
//Linux          (2)
//CurrentVersion (7)

function JoinServerDlg::onWake()
{
   // Add the server option menu:
   if(!%this.initialized)
   {
      // Add the server popup menu:
      new GuiControl(ServerActionDlg) {
         profile = "GuiModelessDialogProfile";
         horizSizing = "relative";
         vertSizing = "relative";
         position = "0 0";
         extent = "640 480";
         minExtent = "8 8";
         visible = "0";
         setFirstResponder = "0";
         modal = "1";

         new GuiPopUpMenuCtrl(ServerPopupMenu) {
            profile = "GuiPopUpMenuProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "0 0";
            extent = "150 25";
            minExtent = "150 25";
            visible = "1";
            maxLength = "255";
            maxPopupHeight = "200";
            noButtonStyle = "1";
         };
      };
      // Label the popup button and add the options
      ServerPopupMenu.setValue("Server Menu");
      ServerPopupMenu.add("Refresh Server", 0);
      ServerPopupMenu.add("Server Info", 1);
      ServerPopupMenu.add("Mark Favorite", 2);
      %this.initialized = true;
   }

   // Double check the status. Tried setting this the control
   // inactive to start with, but that didn't seem to work.
   JS_joinServer.setActive(JS_serverList.rowCount() > 0);

   JS_cancelQuery.setActive(0);

   JS_listLabels.clear();
   %text = "PASS" TAB "NAME" TAB "PING" TAB "PLAYERS" TAB "VERSION" TAB "LEVEL" TAB "TYPE" TAB "FAV";
   JS_listLabels.addRow(0, %text);
   JS_listLabels.setSelectedRow(0);
   //JS_listLabels.scrollVisible(0);

   JS_sort.clear();
   for ( %i = 0; %i < getFieldCount(%text); %i++ )
      JS_sort.add( getField( %text, %i ), %i );

   JS_sort.setSelected(1);
}

//----------------------------------------
function JoinServerDlg::query(%this)
{
   JS_cancelQuery.setActive(1);
   queryMasterServer(
      0,                            // Query flags
      $Client::GameTypeQuery,       // gameTypes
      $Client::MissionTypeQuery,    // missionType
      0,                            // minPlayers
      255,                          // maxPlayers
      254,                           // maxBots
      $pref::Net::RegionMask,       // regionMask
      0,                            // maxPing
      0,                          // minCPU
      0                             // filterFlags
      //0,                            // buddy count
      //0                             // buddy list
      );
}

//----------------------------------------
function JoinServerDlg::queryLan(%this)
{
   JS_cancelQuery.setActive(1);
   queryLANServers(
      $pref::Net::Port,             // lanPort for local queries
      0,                            // Query flags
      $Client::GameTypeQuery,       // gameTypes
      $Client::MissionTypeQuery,    // missionType
      0,                            // minPlayers
      255,                          // maxPlayers
      254,                           // maxBots
      0,                            // regionMask
      0,                            // maxPing
      0,                          // minCPU
      0                             // filterFlags
      );
}

//----------------------------------------
function JoinServerDlg::cancel(%this)
{
   cancelServerQuery();
   JS_statusBar.setValue(1);
   JS_statusText.setText("Query Canceled");
}

//----------------------------------------
function JoinServerDlg::join(%this)
{
   // if we are in a game and try to join another, quit the current game
   if (PlayGui.isAwake())
   disconnect();

   cancelServerQuery();
   %id = JS_serverList.getSelectedId();
   // The server info index is stored in the row along with the
   // rest of displayed info.
   %index = getField(JS_serverList.getRowTextById(%id), 8);
   if ( setServerInfo( %index ) )
   {
      if ( $ServerInfo::Password )
      {
         Canvas.pushDialog( IpJoinDlg );
         IP_AddressEdit.setValue( $ServerInfo::Address );
      }
      else
      {
         %conn = new GameConnection(ServerConnection);
         %conn.setConnectArgs( getField($pref::Player, 0), getField($pref::Player, 1));
         %conn.setJoinPassword( $Client::Password );
         %conn.connect( $ServerInfo::Address );
      }
   }
   else
      MessageBoxOk("No Server Selected","You need to select an available server. Press the Query button to list available servers.");
}

//----------------------------------------
function JoinServerDlg::exit(%this)
{
   cancelServerQuery();
   Canvas.popDialog(JoinServerDlg);
}

//----------------------------------------
function JoinServerDlg::update(%this)
{
   // Copy the servers into the server list.
   //JS_queryStatus.setVisible(false);
   JS_serverList.clear();
   %sc = getServerCount();
   for (%i = 0; %i < %sc; %i++)
   {
      setServerInfo(%i);
      JS_serverList.addRow(%i,
         ($ServerInfo::Password? "Yes": "No") TAB 
         $ServerInfo::Name TAB
         $ServerInfo::Ping TAB
         ($ServerInfo::PlayerCount + $ServerInfo::BotCount) @ "/" @ $ServerInfo::MaxPlayers TAB
         $ServerInfo::Version TAB
         //$ServerInfo::GameType TAB
		 $ServerInfo::MissionName TAB
		 $ServerInfo::MissionType TAB
         ($ServerInfo::Favorite? "Yes": "No") TAB
         %i);  // ServerInfo index stored also This index is used by setServerInfo
   }
   JS_serverList.sort(0, true);
   JS_serverList.setSelectedRow(0);
   JS_serverList.scrollVisible(0);

   JS_joinServer.setActive(JS_serverList.rowCount() > 0);
   JS_cancelQuery.setActive(0);
} 

function JS_serverList::onSelect(%this, %id, %text)
{
   //error("JS_serverList::onSelect(" SPC %this.getName() @ ", " @ %id @ ", " @ %text SPC ")");

   // Label the popup button with the server name
   ServerPopupMenu.setValue(getField(%text, 1));
}

function JS_serverList::onRightMouseDown(%this, %column, %row, %mousePos)
{
   //error("JS_serverList::onRightMouseDown( " @ %this.getName() @ ", " @ %column @ ", " @ %row @ ", " @ %mousePos @ " )");
   cancelServerQuery();

   ServerPopupMenu.position = %mousePos;
   Canvas.pushDialog(ServerActionDlg);

   // Force the opening of the drop down, this is for ease of use
   ServerPopupMenu.forceOnAction();
}
//queryFavoriteServers(0);
function ServerPopupMenu::onSelect(%this, %id, %text)
{
   switch(%id)
   {
      case 0: // Refresh selected server
         %id = JS_serverList.getSelectedId();
         %index = getField(JS_serverList.getRowTextById(%id), 8);
         if(setServerInfo(%index))
            querySingleServer( $ServerInfo::Address, 0 );
         else
            MessageBoxOk("No Server Selected", "You need to select an available server. Press the Query button to list available servers.");

      case 1: // Show selected servers information
         %id = JS_serverList.getSelectedId();
         %index = getField(JS_serverList.getRowTextById(%id), 8);
         if ( setServerInfo( %index ) )
         {
            MessageBoxOK( "SERVER INFO",
                   "STATUS:" SPC $ServerInfo::Status @ "\n" @
                   $ServerInfo::Address @ "\n" @
                   "NAME:" SPC $ServerInfo::Name @ "\n" @
                   "GAMETYPE:" SPC $ServerInfo::GameType @ "\n" @
                   "MISSION:" SPC $ServerInfo::MissionName @ "\n" @
                   "MISSION TYPE:" SPC $ServerInfo::MissionType @ "\n" @
                   "STATE:" SPC $ServerInfo::State @ "\n" @
                   "INFO:" SPC $ServerInfo::Info @ "\n" @
                   "PLAYERS:" SPC $ServerInfo::PlayerCount @ "\n" @
                   "MAX PLAYERS:" SPC $ServerInfo::MaxPlayers @ "\n" @
                   "BOTS:" SPC $ServerInfo::BotCount @ "\n" @
                   "VERSION:" SPC $ServerInfo::Version @ "\n" @
                   "PING:" SPC $ServerInfo::Ping @ "\n" @
                   "CPU:" SPC $ServerInfo::CPUSpeed @ "\n" @
                   "FAVORITE:" SPC $ServerInfo::Favorite @ "\n" @
                   "DEDICATED:" SPC $ServerInfo::Dedicated @ "\n" @
                   "PASSWORD:" SPC $ServerInfo::Password );
         }
         else
            MessageBoxOk("No Server Selected", "You need to select an available server. Press the Query button to list available servers.");
 
      case 2: // Set as favorite
         %id = JS_serverList.getSelectedId();
         %index = getField(JS_serverList.getRowTextById(%id), 8);
         if ( setServerInfo( %index ) )
         {
            if ( $pref::Client::ServerFavoriteCount $= "" )
               $pref::Client::ServerFavoriteCount = 0;

            $pref::Client::ServerFavorite[$pref::Client::ServerFavoriteCount++] = $ServerInfo::Address;
         }
  }
   Canvas.popDialog(ServerActionDlg);
}

function ServerPopupMenu::onCancel(%this)
{
   Canvas.popDialog(ServerActionDlg);
}

function JS_sort::onSelect(%this, %id, %text)
{
   switch(%id)
   {
      case 0:
         JS_serverList.sortNumerical(0, true);
      case 1:
         JS_serverList.sort(1, true);
      case 2:
         JS_serverList.sortNumerical(2, true);
      case 3:
         JS_serverList.sortNumerical(3, false);
      case 4:
         JS_serverList.sortNumerical(4, false);
      case 5:
         JS_serverList.sortNumerical(5, true);
      case 6:
         JS_serverList.sort(6, true);
   }
}

//----------------------------------------
function onServerQueryStatus(%status, %msg, %value)
{
   // Update query status
   // States: start, update, ping, query, done
   // value = % (0-1) done for ping and query states

   switch$ (%status)
   {
      case "start":
         JS_joinServer.setActive(false);
         JS_queryMaster.setActive(false);
         JS_statusText.setText(%msg);
         JS_statusBar.setValue(0);
         JS_serverList.clear();

      case "ping":
         JS_statusText.setText("Pinging Servers");
         JS_statusBar.setValue(%value);

      case "query":
         JS_statusText.setText("Querying Servers");
         JS_statusBar.setValue(%value);

      case "done":
         JS_queryMaster.setActive(true);
         JS_statusText.setText(%msg);
         JoinServerDlg.update();
   }
}

function IP_AddressEdit::setField(%this)
{
   %value = %this.getValue();
   %this.setValue(%value);
}

function IP_AddressEdit::processEnter(%this)
{
   %ip = IP_AddressEdit.getValue();
   if(isCleanIP(%ip))
   {
      if(strstr(%ip, ":") != -1)
      {
         connect(%ip, $Client::Password, getField($pref::Player, 0), getField($pref::Player, 1));
         echo( "Trying to join server " @ %ip @ "..." );
      }
      else
      {
         IP_AddressEdit.setValue("");
         MessageBoxOK( "ERROR", "Wrong syntax, must enter IP:PORT. \n\nExample: 192.168.0.1:28000" );
      }
   }
   else
   {
      IP_AddressEdit.setValue("");
      MessageBoxOK( "ERROR", "Wrong syntax, must enter IP:PORT. \n\nExample: 192.168.0.1:28000" );
   }
}

function isCleanIP(%ip)
{
   %dot = 0;
   %op = 0;
   for(%i = 0; (%ip = getSubStr(%ip, 0, %i)) !$= ""; %i++)
   {
      switch$(%ip)
      {
         case "0" or "1" or "2" or "3" or "4" or "5" or "6" or "7" or "8" or "9":
            continue;

         case ".":
            if(%dot > 3)
               return false;

            %dot++;
            continue;

         case ":":
            if(%op > 1)
               return false;

            %op++;
            continue;

         default:
            return false;
      }
   }
   return true;
}

function connect(%ip, %pwd, %name, %skin)
{
   %conn = new GameConnection(ServerConnection);
   %conn.setConnectArgs(%name, %skin);
   %conn.setJoinPassword(%pwd);
   %conn.connect(%ip);
}

function JS_listLabels::onSelect(%this, %id, %text)
{
   // So annoying you cannot get the selected column only the whole row :\
   //error("JS_listLabels::onSelect(" SPC %this.getName() @ ", " @ %id @ ", " @ %text SPC ")");
}
