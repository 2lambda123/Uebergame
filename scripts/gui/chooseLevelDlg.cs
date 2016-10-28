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

function ChooseLevelDlg::onWake(%this)
{
   buildMissionTypePopup(CL_LevelType);

   // Select the saved-off prefs:
   if ( $pref::Server::MissionType !$= "" )
   {
      // Find the last selected type:
      for ( %type = 0; %type < $HostTypeCount; %type++ )
      {
         if($HostTypeName[%type] $= $pref::Server::MissionType)
            break;
      }

      if ( %type != $HostTypeCount )
      {
         CL_LevelType.setSelected(%type);
         CL_LevelType.onSelect(%type, "");
         if ( $pref::Server::MissionFile !$= "" )
         {
            // Find the last selected mission:
            for(%index = 0; %index < $HostMissionCount[%type]; %index++)
            {
               if($HostMissionFile[$HostMission[%type, %index]] $= $pref::Server::MissionFile)
                  break;
            }

            if(%index != $HostMissionCount[%type])
               CL_LevelList.setSelectedById($HostMission[%type, %index]);
         }
      }
   }
   else
   {
      CL_LevelType.setSelected(0);
      CL_LevelType.onSelect(0, "");
   }
}

function ChooseLevelDlg::onSleep( %this )
{
   // This is set from the outside, only stays true for a single wake/sleep
   // cycle.
   %this.launchInEditor = false;
}

function buildMissionTypePopup(%popup)
{
   %popup.clear();
   for(%type = 0; %type < $HostTypeCount; %type++)
      %popup.add($HostTypeDisplayName[%type], %type);

   %popup.sort();
}

// Do this onMouseUp not via Command which occurs onMouseDown so we do

// not have a lingering mouseUp event lingering in the ether.

function ChooseLevelDlgGoBtn::onMouseUp( %this )

{
   // if we are in a game and try to join another, quit the current game
   if (PlayGui.isAwake())
   disconnect();

   // So we can't fire the button when loading is in progress.

   if ( isObject( ServerGroup ) )

      return;

   %id = CL_LevelList.getSelectedId();
   %mission = $HostMissionFile[%id];

   // Launch the chosen level with the editor open?

   if ( ChooseLevelDlg.launchInEditor )

   {

      activatePackage( "BootEditor" );

      ChooseLevelDlg.launchInEditor = false;
 
     StartLevel( %mission, "SinglePlayer" );

   }

   else

   {

      StartLevel( %mission, "" );
 
  }

}

function StartLevel(%mission, %serverType)
{
	
   if( %mission $= "" )
   {
      %id = CL_LevelList.getSelectedId();
      %mission = $HostMissionFile[%id];
   }

   if ( %serverType $= "" )
   {
      if( $pref::Server::Multiplayer )
         %serverType = "MultiPlayer";
      else
         %serverType = "SinglePlayer";
   }

   //LogEcho("CL_StartLevel selected id:" SPC %id SPC "Mission:" SPC %mission);

   if ( $pref::Server::Password !$= "" )
      $Client::Password = $pref::Server::Password;

   // Show the loading screen immediately.
   if ( isObject( LoadingGui ) )
   {
      Canvas.setContent("LoadingGui");
      LoadingProgress.setValue(1);
      LoadingProgress.setValue("LOADING LEVEL FILE");
      Canvas.repaint();
   }

   createAndConnectToLocalServer( %serverType, %mission, $pref::Server::MissionType );
}

function CL_LevelList::onSelect(%this, %row)
{
   //LogEcho("CL_LevelList::onSelect( "@%this.getName()@", "@%row@" )");
   // Get the mission file, this includes path information.
   %mission = $HostMissionFile[%this.getSelectedId()];

   // Find the preview image
   %levelPreview = filePath(%mission) @ "/" @ fileBase(%mission) @ "_preview";

   // Test against all of the different image formats
   // This should probably be moved into an engine function
   if (isFile(%levelPreview @ ".png") ||
       isFile(%levelPreview @ ".jpg") ||
       isFile(%levelPreview @ ".dds"))
   {
      CL_Preview.setBitmap( %levelPreview );
   }
   else
   {
      CL_Preview.setBitmap( "levels/load_mission.jpg" );
   }
/*
   // Set the preview bitmap which is the mission name.jpg
   %image = filePath(%mission) @ "/load_" @ fileBase(%mission) @ ".jpg";
   if ( isFile( %image ) )
      CL_Preview.setBitmap( %image );
   else
      CL_Preview.setBitmap( "levels/load_mission.jpg" );
*/
   // Extract mission description from the mission file and stuff into a global array.
   %this.getMissionInfo(%mission);

   CL_Description.setText("");
   %text = "<color:ffffff><shadowcolor:000000><shadow:1:1><font:ArialBold:12>";
   for(%line = 0; %line < $SMInfoLineCount; %line++)
      CL_Description.addtext( %text @ $SMInfoLine[%line] @ "\n\n", true );
}

function CL_LevelType::onSelect(%this, %id, %text)
{
   //LogEcho("CL_LevelType::onSelect( "@%this.getName()@", "@%id@", "@%text@" )");
   // Fill the mission list:
   CL_LevelList.clear();
   %lastAdded = 0;
   for(%i = 0; %i < $HostMissionCount[%id]; %i++)
   {
      %misId = $HostMission[%id, %i];
      CL_LevelList.addRow(%misId, $HostMissionName[%misId]);
      %lastAdded = %misId;
   }
   CL_LevelList.sort(0);
   CL_LevelList.sortNumerical(1, false);

   // Select the last mission added:
   CL_LevelList.setSelectedById(%lastAdded);
   $pref::Server::MissionType = $HostTypeName[%id];
}

function CL_LevelList::getMissionInfo(%this, %mission)
{
   //LogEcho("CL_LevelList::getMissionInfo(" SPC %this.getName() @", "@ %mission SPC ")");

   // Clear out the old
   for ( %i = 0; %i < %SMInfoLineCount; %i++ )
      $SMInfoLine[%i] = "";

   $SMInfoLineCount = 0;

   for( %line = 0; $HostMissionDesc[%mission, %line] !$= ""; %line++ )
   {
      $SMInfoLine[$SMInfoLineCount] = $HostMissionDesc[%mission, %line];
      $SMInfoLineCount++;
   }
}
