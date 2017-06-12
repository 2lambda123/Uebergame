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
// These are variables used to control the shell scripts and
// can be overriden by mods:
//-----------------------------------------------------------------------------
function Torque::initClient(%this)
{
   echo("\n--------- Initializing " @ $appName @ ": Client Scripts ---------");

   // Set a background image for the main guis
   if ( $pref::MainGui::LastBackground $= "" || $pref::MainGui::LastBackground >= 2 )
      $pref::MainGui::LastBackground = 0;

   // create more backgrounds later #newfeature
   switch( $pref::MainGui::LastBackground )
   {
      case 0:
         $MainGuiBackground = "art/gui/core/space_background_big.dds";
      case 1:
         $MainGuiBackground = "art/gui/core/space_background_big.dds";
      default:
         $MainGuiBackground = "art/gui/core/space_background_big.dds";
   }
   $pref::MainGui::LastBackground++;
     
   // Make sure this variable reflects the correct state.
   // Obviously the gui is loading/loaded, so no way should we be dedicated.
   $Server::Dedicated = $pref::Server::Dedicated = false;

   // Game information used to query the master server
   $Client::GameTypeQuery = $appName;
   $Client::MissionTypeQuery = "Any";
   $Client::GameType = "";

   exec("./audioGui.cs"); //menu button sounds etc, load early so they work properly
   
   // These should be game specific GuiProfiles.  Custom profiles are saved out
   // from the Gui Editor.  Either of these may override any that already exist.
   exec("scripts/gui/defaultGameProfiles.cs"); 

   // Default player key bindings
   exec("./default.bind.cs");

   if (isFile($HomePath @ "/bindings.cs"))
      exec($HomePath @ "/bindings.cs");
	 
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
   exec("./playerList.cs");
   exec("~/gui/chatHud.cs");
   exec("./messageHud.cs");

   initRenderManager();
   initLightingSystems();

   // Use our prefs to configure our Canvas/Window
   configureCanvas();

   loadMaterials();
   
   // Load up the shell GUIs
   exec("~/gui/startupGui.gui");
   exec("~/gui/startupGui.cs");
   exec("~/gui/backgroundGui.gui");
   exec("~/gui/backgroundLevelGui.gui");
   exec("~/gui/mainMenuGui.gui");
   exec("~/gui/mainMenuGui.cs");
   exec("~/gui/mainMenuAdsGui.gui");
   exec("~/gui/mainMenuAdsGui.cs");
   exec("~/gui/gameMenuGui.gui");
   exec("~/gui/gameMenuGui.cs");
   exec("~/gui/chooseLevelDlg.gui");
   exec("~/gui/chooseLevelDlg.cs");
	exec("~/gui/startEditorGui.gui");
   exec("~/gui/startEditorGui.cs");
   exec("~/gui/serverOptionsDlg.gui");
   exec("~/gui/serverOptionsDlg.cs");
   exec("~/gui/joinServerDlg.gui");
   exec("~/gui/joinServerDlg.cs");
   exec("~/gui/ipJoinDlg.gui");
   exec("~/gui/optionsDlg.gui");
   exec("~/gui/optionsDlg.cs");
   exec("~/gui/loadingGui.gui");
   exec("~/gui/loadingGui.cs");
   exec("~/gui/remapDlg.gui");
   exec("~/gui/endGameGui.gui");
   exec("~/gui/endGameGui.cs");
   exec("~/gui/extrasDlg.gui");
   exec("~/gui/helpDlg.gui");
   exec("~/gui/creditsDlg.gui");
   exec("~/gui/devToolsDlg.gui");

   // Load up the Game GUI
   exec("~/gui/playGui.gui");
   exec("~/gui/playGui.cs");
   exec("~/gui/hudlessGui.gui");
   exec("~/gui/hudlessGui.cs");
   exec("~/gui/chatHud.gui");
   exec("~/gui/messageHud.gui");
   exec("~/gui/scoreHudFFA.gui");
   exec("~/gui/ScoreHudFFA.cs");
   exec("~/gui/scoreHud.gui");
   exec("~/gui/scoreHud.cs");
   exec("~/gui/voteHudDlg.gui");
   exec("~/gui/voteHudDlg.cs");
   exec("~/gui/quickChatHud.gui");
   exec("~/gui/quickChatHud.cs");
   exec("~/gui/adminDlg.gui");
   exec("~/gui/adminDlg.cs");
   exec("~/gui/armoryHud.gui");
   exec("~/gui/armoryHud.cs");
   exec("~/gui/vehicleHud.gui");
   exec("~/gui/vehicleHud.cs");
   exec("~/gui/bombTimerDlg.gui");
   exec("~/gui/bombTimerDlg.cs");
   exec("~/gui/fireTeamHud.gui");
   exec("~/gui/fireTeamHud.cs");
   exec("~/gui/missionAreaWarningHud.gui");
   exec("~/gui/recordingsDlg.gui");
   exec("~/gui/recordingsDlg.cs");
   exec("~/gui/guiMusicPlayer.gui");
   exec("~/gui/guiMusicPlayer.cs");
   exec("~/gui/guiVideoPlayer.gui");
   exec("~/gui/guiVideoPlayer.cs");
   exec("~/gui/objectiveHud.cs");

   //update user config files, if he has an old version
   if($pref::Version !$= 1050)
   updatePrefs();
   
   // Really shouldn't be starting the networking unless we are
   // going to connect to a remote server, or host a multi-player game.
   setNetPort(0);

   // Copy saved script prefs into C++ code.
   setDefaultFov( $pref::Player::defaultFov );
   setZoomSpeed(500);

   if( isScriptFile( expandFilename("./audioData.cs") ) )
      exec( "./audioData.cs" );

   // Start up the main menu... this is separated out into a method for easier mod override.
   if ($startWorldEditor || $startGUIEditor) {
      // Editor GUI's will start up in the primary main.cs once engine is initialized.
      return;
   }

   // Connect to server if requested.
   if ($JoinGameAddress !$= "")
   {
      // If we are instantly connecting to an address, load the loading GUI then attempt the connect.
      tge.loadLoadingGui();
      connect($JoinGameAddress, $Client::Password, getField($pref::Player, 0), getField($pref::Player, 1));
   }
   else
   {
      // Otherwise go to the splash screen.
      Canvas.setCursor("DefaultCursor");
      tge.loadMainMenu();
   }   
}

function Torque::loadMainMenu(%this)
{   
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

      // Clear out the $levelToLoad so we don't attempt to load the level again later on.
      $levelToLoad = "";
      
      // let's make sure the file exists
      %file = findFirstFile(%levelFile);

      if(%file !$= "")
         createAndConnectToLocalServer("SinglePlayer", %file, $pref::Server::MissionType);
   }
   else
   {
      if ($pref::Menu::Level)
      {
         if( $UsingMainMenuLevel )
            disconnect(); 

         $UsingMainMenuLevel = true;
         schedule(50,0, "createAndConnectToLocalServer","SinglePlayer", getMainMenuLevel() ); 
      }
      else
      {
         Canvas.setContent("backgroundGui");
         Canvas.pushDialog( MainMenuGui );
         
         if ($pref::Menu::Ads)
            Canvas.pushDialog( MainMenuAdsGui );
      }       
   }
}

function Torque::loadLoadingGui(%this)
{
   if($UsingMainMenuLevel )
      return; // no level loading gui for loading the main menu
 
   Canvas.setContent("LoadingGui");
   LoadingProgress.setValue(1);

   LoadingProgressTxt.setValue("WAITING FOR SERVER");

   Canvas.repaint();
}