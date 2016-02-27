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

//----------------------------------------------------------------------------
// Mission Loading & Mission Info
// The mission loading server handshaking is handled by the
// scripts/client/missingLoading.cs.  This portion handles the interface
// with the game GUI.
//----------------------------------------------------------------------------

//----------------------------------------------------------------------------
// Loading Phases:
// Phase 1: Download Datablocks
// Phase 2: Download Ghost Objects
// Phase 3: Scene Lighting

//----------------------------------------------------------------------------
// Phase 1
//----------------------------------------------------------------------------

function onMissionDownloadPhase1(%missionName, %musicTrack)
{
   %path = "levels/" @ fileBase( %missionName ) @ $PostFXManager::fileExtension;
   %found = 0;
   for( %file = findFirstFile( %path ); %file !$= ""; %file = findNextFile( %path ) )
   {
      //echo("--------> FILE:" SPC %file SPC "<--------");
      if ( isScriptFile( %file ) )
      {
         %found = 1;
         postFXManager::loadPresetHandler( %file );
         break;
      }
   }

   if( !%found )
      PostFXManager::settingsApplyDefaultPreset();

/*
   // Load the post effect presets for this mission.
   %path = "levels/" @ fileBase( %missionName ) @ $PostFXManager::fileExtension;
   if ( isScriptFile( %path ) )
      postFXManager::loadPresetHandler( %path ); 
   else
      PostFXManager::settingsApplyDefaultPreset();
*/

   // Close and clear the message hud (in case it's open)
   if ( isObject( MessageHud ) )
      MessageHud.close();

   // Reset the loading progress controls:
   if ( isObject( LoadingProgress ) )
   {
      LoadingProgress.setValue(0);
      LoadingProgress.setValue("LOADING DATABLOCKS");
      Canvas.repaint();
   }

   if ( %musicTrack !$= "" )
      clientCmdPlayMusic(%musicTrack);
   else
      clientCmdStopMusic();
}

function onPhase1Progress(%progress)
{
   if ( isObject( LoadingProgress ) )
   {
      LoadingProgress.setValue(%progress);
      Canvas.repaint(33);
   }
}

function onPhase1Complete()
{
   if ( isObject( LoadingProgress ) )
   {
      LoadingProgress.setValue( 1 );
      Canvas.repaint();
   }
}

//----------------------------------------------------------------------------
// Phase 2
//----------------------------------------------------------------------------

function onMissionDownloadPhase2(%missionFile)
{
   // Clear the console
   if(!$pref::Console::NoClear)
      cls();

   // Reset the loading progress controls:
   if ( isObject( LoadingProgress ) )
   {
      LoadingProgress.setValue(0);
      LoadingProgress.setText("LOADING OBJECTS");
      Canvas.repaint();
   }
}

function onPhase2Progress(%progress)
{
   if ( isObject( LoadingProgress ) )
   {
      LoadingProgress.setValue(%progress);
      Canvas.repaint(33);
   }
}

function onPhase2Complete()
{
   // Make the clients next selection the last values stored or last loadout selected.
   if( $pref::Player::CurrentLoadout !$= "" )
      commandToServer( 'setClientLoadout', addTaggedString($pref::Player::CurrentLoadout) );
   else
      commandToServer( 'setClientLoadout', addTaggedString($pref::Player::Loadout[$pref::Player::SelectedLoadout]) );

   // Send selected vehicle
   commandToServer('SelectVehicle', addTaggedString($pref::Player::SelectedVehicle));

   if ( isObject( LoadingProgress ) )
   {
      LoadingProgress.setValue( 1 );
      Canvas.repaint();
   }
}   

function onFileChunkReceived(%fileName, %ofs, %size)
{
   if ( isObject( LoadingProgress ) )
   {
      LoadingProgress.setValue(%ofs / %size);
      LoadingProgress.setText("Downloading " @ %fileName @ "...");
   }
}

//----------------------------------------------------------------------------
// Phase 3
//----------------------------------------------------------------------------

function onMissionDownloadPhase3(%missionFile)
{
   if ( isObject( LoadingProgress ) )
   {
      LoadingProgress.setValue(0);
      LoadingProgress.setText("LIGHTING MISSION");
      Canvas.repaint();
   }
}

function onPhase3Progress(%progress)
{
   if ( isObject( LoadingProgress ) )
   {
      LoadingProgress.setValue(%progress);
	  Canvas.repaint(33);
   }
}

function onPhase3Complete()
{
   $lightingMission = false;

   if ( isObject( LoadingProgress ) )
   {
      LoadingProgress.setValue("STARTING MISSION");
      LoadingProgress.setValue( 1 );
      Canvas.repaint();
   }
}

//----------------------------------------------------------------------------
// Mission loading done!
//----------------------------------------------------------------------------

function onMissionDownloadComplete()
{
   // Client will shortly be dropped into the game, so this is
   // good place for any last minute gui cleanup.
   if ( isObject( ArmoryDlg ) )
      clientCmdClearArmoryHud(0);
}