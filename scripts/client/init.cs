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
// Variables used by client scripts & code.  The ones marked with (c)
// are accessed from code.  Variables preceeded by Pref:: are client
// preferences and stored automatically in the prefs/prefs.cs file
// in between sessions.
//
//    (c) Client::MissionFile             Mission file name
//    ( ) Client::Password                Password for server join

//    (?) Pref::Player::CurrentFOV
//    (?) Pref::Player::DefaultFov
//    ( ) Pref::Input::KeyboardTurnSpeed

//    (c) pref::Master[n]                 List of master servers
//    (c) pref::Net::RegionMask
//    (c) pref::Client::ServerFavoriteCount
//    (c) pref::Client::ServerFavorite[FavoriteCount]
//    .. Many more prefs... need to finish this off

// Moves, not finished with this either...
//    (c) firstPerson
//    $mv*Action...

//-----------------------------------------------------------------------------
// These are variables used to control the shell scripts and
// can be overriden by mods:
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
function Torque::initClient(%this)
{
   echo("\n--------- Initializing " @ $appName @ ": Client Scripts ---------");

   // Set a background image for the main guis
   if ( $pref::MainGui::LastBackground $= "" || $pref::MainGui::LastBackground >= 2 )
      $pref::MainGui::LastBackground = 0;

   switch( $pref::MainGui::LastBackground )
   {
      case 0:
         $MainGuiBackground = "art/gui/space_background_big.dds";
      case 1:
         $MainGuiBackground = "art/gui/space_background_big.dds";
      default:
         $MainGuiBackground = "art/gui/space_background_big.dds";
   }
   $pref::MainGui::LastBackground++;

   // Make sure this variable reflects the correct state.
   // Obviously the gui is loading/loaded, so no way should we be dedicated.
   $Server::Dedicated = $pref::Server::Dedicated = false;

   // Game information used to query the master server
   $Client::GameTypeQuery = $appName;
   $Client::MissionTypeQuery = "Any";
   $Client::GameType = "";

   // These should be game specific GuiProfiles.  Custom profiles are saved out
   // from the Gui Editor.  Either of these may override any that already exist.
   exec("scripts/gui/defaultGameProfiles.cs"); 

   // Default player key bindings
   exec("./default.bind.cs");

   if (isFile( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/bindings.config.cs" ) )
         exec( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/bindings.config.cs" );

   // Base client functionality
   exec( "./message.cs" );
   exec( "./mission.cs" );
   exec( "./missionDownload.cs" );
   exec( "./onMissionDownload.cs" );
   exec( "./renderManager.cs" );
   exec( "./lighting.cs" );
   exec( "./keyRemaps.cs" );

   // Client scripts
   exec("./serverConnection.cs");
   exec("./callbacks.cs");
   exec("scripts/gui/chatHud.cs");
   exec("scripts/gui/messageHud.cs");

   initRenderManager();
   initLightingSystems();

   // Use our prefs to configure our Canvas/Window
   configureCanvas();

   // Load up the Game GUIs
   exec("scripts/gui/adminDlg.gui");
   exec("scripts/gui/armoryHud.gui");
   exec("scripts/gui/defaultGameProfiles.cs");
   exec("scripts/gui/playGui.gui");
   exec("scripts/gui/chatHud.gui");
   exec("scripts/gui/playerList.gui");
   exec("scripts/gui/hudlessGui.gui");
   exec("scripts/gui/missionAreaWarningHud.gui");

   // Load up the shell GUIs
   exec("scripts/gui/mainMenuGui.gui");
   exec("scripts/gui/joinServerDlg.gui");
   exec("scripts/gui/endGameGui.gui");
   exec("scripts/gui/exitGameGui.gui");
   exec("scripts/gui/chooseLevelDlg.gui");
   exec("scripts/gui/loadingGui.gui");
   exec("scripts/gui/optionsDlg.gui");
   exec("scripts/gui/remapDlg.gui");
   exec("scripts/gui/extrasDlg.gui");
   exec("scripts/gui/helpDlg.gui");
   
   // Gui scripts
   exec("scripts/gui/adminDlg.cs");
   exec("scripts/gui/armoryHud.cs");
   exec("scripts/gui/playerList.cs");
   exec("scripts/gui/chatHud.cs");
   exec("scripts/gui/messageHud.cs");
   exec("scripts/gui/playGui.cs");
   exec("scripts/gui/chooseLevelDlg.cs");
   exec("scripts/gui/loadingGui.cs");
   exec("scripts/gui/optionsDlg.cs");
   exec("scripts/gui/helpDlg.cs");

   // Client scripts
   exec("./client.cs");
   exec("./missionDownload.cs");
   exec("./serverConnection.cs");

   loadMaterials();

   // Really shouldn't be starting the networking unless we are
   // going to connect to a remote server, or host a multi-player
   // game.
   setNetPort(0);

   // Copy saved script prefs into C++ code.
   setDefaultFov( $pref::Player::defaultFov );
   setZoomSpeed( $pref::Player::zoomSpeed );

   //if( isScriptFile( expandFilename("./audioData.cs") ) )
   //   exec( "./audioData.cs" );

   // Start up the main menu... this is separated out into a
   // method for easier mod override.

   if ($startWorldEditor || $startGUIEditor) {
      // Editor GUI's will start up in the primary main.cs once
      // engine is initialized.
      return;
   }

   // Connect to server if requested.
   if ($JoinGameAddress !$= "")
   {
      // If we are instantly connecting to an address, load the
      // loading GUI then attempt the connect.
      tge.loadLoadingGui();
      connect($JoinGameAddress, $Client::Password, getField($pref::Player, 0), getField($pref::Player, 1));
   }
   else
   {
      // Otherwise go to the splash screen.
      Canvas.setCursor("DefaultCursor");
      tge.loadMainMenu();
      //loadStartup();
   }   
}

//-----------------------------------------------------------------------------

function Torque::loadMainMenu(%this)
{
   // Startup the client with the Main menu...
   if (isObject( MainMenuGui ))
      Canvas.setContent( MainMenuGui );
   
   Canvas.setCursor("DefaultCursor");

   // first check if we have a level file to load
   if ($levelToLoad !$= "")
   {
      %levelFile = "levels/";
      %ext = getSubStr($levelToLoad, strlen($levelToLoad) - 3, 3);
      if(%ext !$= "mis")
         %levelFile = %levelFile @ $levelToLoad @ ".mis";
      else
         %levelFile = %levelFile @ $levelToLoad;

      // Clear out the $levelToLoad so we don't attempt to load the level again
      // later on.
      $levelToLoad = "";
      
      // let's make sure the file exists
      %file = findFirstFile(%levelFile);

      if(%file !$= "")
         createAndConnectToLocalServer("SinglePlayer", %file, $pref::Server::MissionType);
   }
}

function Torque::loadLoadingGui(%this)
{
   Canvas.setContent("LoadingGui");
   LoadingProgress.setValue(1);

   LoadingProgressTxt.setValue("WAITING FOR SERVER");

   Canvas.repaint();
}