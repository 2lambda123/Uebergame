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

// Sync the Camera and the EditorGui
function clientCmdSyncEditorGui()
{
   if (isObject(EditorGui))
      EditorGui.syncCameraGui();
}

//----------------------------------------------------------------------------
// Game start / end events sent from the server
//----------------------------------------------------------------------------

function clientCmdGameStart(%seq)
{
   PlayerListGui.zeroScores();
}

function clientCmdGameEnd(%seq)
{
   // Stop local activity... the game will be destroyed on the server
   sfxStopAll();

   if ((!EditorIsActive() && !GuiEditorIsActive()))
   {
   // Copy the current scores from the player list into the
   // end game gui (bit of a hack for now).
   EndGameGuiListLabel.clear();
   %header = "NAME" TAB "TEAM" TAB "SCORE";
   EndGameGuiListLabel.addRow(0, %header);
   EndGameGuiListLabel.setSelectedRow(0);

   EndGameGuiList.clear();
   for (%i = 0; %i < PlayerListGuiList.rowCount(); %i++)
   {
      //error("PlayerListGuiList.rowCount loop at: " @ %i);
      %text = PlayerListGuiList.getRowText(%i);
      %id = PlayerListGuiList.getRowId(%i);
      EndGameGuiList.addRow(%id, %text);
   }
   EndGameGuiList.sortNumerical(2, false);

   // Display the end-game screen
   Canvas.setContent(EndGameGui);
}
}
//-----------------------------------------------------------------------------
// Damage Direction Indicator
//-----------------------------------------------------------------------------

function clientCmdSetDamageDirection(%direction)
{
   eval("%ctrl = DamageHUD-->damage_" @ %direction @ ";");
   if (isObject(%ctrl))
   {
      // Show the indicator, and schedule an event to hide it again
      cancelAll(%ctrl);
      %ctrl.setVisible(true);
      %ctrl.schedule(500, setVisible, false);
   }
}

//-----------------------------------------------------------------------------
// Show/Hide specific gui elements

// Default to Spectator mode
$HudMode = "Spectator";

function clientCmdSetHudMode(%mode)
{
   $HudMode = detag( %mode );

   echo("\c2clientCmdSetHudMode(" SPC $HudMode SPC ")");

   // only update key maps if playGui is current content (default.bind.cs)
   if( Canvas.getContent() == PlayGui.getId() )
      tge.updateKeyMaps();

   // Kill all of the huds
   ClusterHud.setVisible(false);
   HudClock.setVisible(false);
   clientCmdHideReticle();
   clientDeploySensorOff();
   objectiveHud.setVisible(false);
   //HudRadar.setVisible(false);

   switch$( $HudMode )
   {
      case "Spectator":
         HudClock.setVisible(true);

      case "HudTest":
         ClusterHud.setVisible(true);
         HudClock.setVisible(true);
         clientCmdShowReticle();
         objectiveHud.setVisible(true);
         mainVoteHud.setVisible(true);
         LagIcon.setVisible(true);
         toggleNetGraph(1);
         metrics("fps");

      case "Play":
         ClusterHud.setVisible(true);
         HudClock.setVisible(true);
         clientCmdShowReticle();
         objectiveHud.setVisible(true);
         //HudRadar.setVisible(true);
         if(voteHud.voting)
            mainVoteHud.setVisible(1);
         else
            mainVoteHud.setVisible(0);

      case "Corpse":
         HudClock.setVisible(true);
         objectiveHud.setVisible(true);
         if(voteHud.voting)
            mainVoteHud.setVisible(1);
         else
            mainVoteHud.setVisible(0);

      case "Pilot":
         ClusterHud.setVisible(true);
         HudClock.setVisible(true);
         clientCmdShowReticle();
         objectiveHud.setVisible(true);
         //HudRadar.setVisible(true);
         if(voteHud.voting)
            mainVoteHud.setVisible(1);
         else
            mainVoteHud.setVisible(0);

      default:
         HudClock.setVisible(true);
         clientCmdShowReticle();
         objectiveHud.setVisible(true);
         //HudRadar.setVisible(true);
         if(voteHud.voting)
            mainVoteHud.setVisible(1);
         else
            mainVoteHud.setVisible(0);
   }
}
//-----------------------------------------------------------------------------
// Show/Hide reticle

function clientCmdHideReticle()
{
   reticle.setVisible(false);
   $ZoomOn = false;
   toggleZoomFOV();
   zoomReticle.setVisible(false);
}

function clientCmdShowReticle()
{
   reticle.setVisible(true);
}

function clientCmdDoZoomReticle(%val)
{
   if( %val )
   {
      zoomReticle.setVisible(true);
   }
}

// ----------------------------------------------------------------------------
// splatter Support
// ----------------------------------------------------------------------------
function clientCMDSpatter(%Decalposition, %splatterNorm, %splatterScaling)
{
    decalManagerAddDecal(%Decalposition, %splatterNorm, 0, %splatterScaling, bloodDecalData, false);
}