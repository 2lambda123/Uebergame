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
function ScoreGui::configure(%this, %gameType)
{
   ScoreHud.PlayerScore[0] = new GuiTextListCtrl() {
      profile = "ScoreHudTextWhiteProfile";
      horizSizing = "center";
      vertSizing = "bottom";
      position = "56 315";
      extent = "500 160";
      minExtent = "500 160";
      visible = "1";
      enumerate = "1";
      resizeCell = "1";
      columns = "0 250";
      fitParentWidth = "0";
      clipColumnText = "0";
      noDuplicates = "true";
   };
   ScoreHud.PlayerScore[1] = new GuiTextListCtrl() {
      profile = "ScoreHudTextBlueProfile";
      horizSizing = "relative";
      vertSizing = "bottom";
      position = "20 60";
      extent = "230 250";
      minExtent = "230 250";
      visible = "1";
      enumerate = "1";
      resizeCell = "1";
      columns = "0 120";
      fitParentWidth = "0";
      clipColumnText = "0";
      noDuplicates = "true";
   };
   ScoreHud.PlayerScore[2] = new GuiTextListCtrl() {
      profile = "ScoreHudTextRedProfile";
      horizSizing = "relative";
      vertSizing = "bottom";
      position = "342 60";
      extent = "230 250";
      minExtent = "230 250";
      visible = "1";
      enumerate = "1";
      resizeCell = "1";
      columns = "0 120";
      fitParentWidth = "0";
      clipColumnText = "0";
      noDuplicates = "true";
   };

   for(%i = 0; %i <= 2; %i++)
      ScoreHud.add(ScoreHud.PlayerScore[%i]);

   %this.configured = 1;
}

function ScoreGui::onWake(%this)
{
   if ( isObject( hudMap ) )
   {
      hudMap.pop();
      hudMap.delete();
   }
   new ActionMap( hudMap );
   hudMap.blockBind( moveMap, bringUpOptions );
   hudMap.blockBind( moveMap, toggleTeamChoose );
   hudMap.blockBind( moveMap, showPlayerList );
   hudMap.push();

   if(!%this.configured)
      %this.configure();
}

function ScoreGui::onSleep(%this)
{
   // Make sure the proper key maps are pushed
   tge.updateKeyMaps(); 
}

//------------------------------------------------------------------------------

function clientCmdToggleScoreHud(%val)
{
   %hud = ScoreGui.getId();
   if( %hud.isAwake() )
      commandToServer( 'HideScoreHud' );
   else
      commandToServer( 'ShowScoreHud' );
}

function clientCmdOpenScoreHud()
{
   //error("clientCmdOpenScoreHud()");
   %hud = ScoreGui.getId();
   if( !%hud.isAwake() )
      Canvas.pushDialog( %hud );
}

function clientCmdCloseScoreHud()
{
   //error("clientCmdCloseScoreHud()");
   %hud = ScoreGui.getId();
   if( %hud.isAwake() )
      Canvas.popDialog( %hud );
}

//------------------------------------------------------------------------------
addMessageCallBack( 'SetTeamScores', handleSetTeamScores );

function handleSetTeamScores(%msgType, %msgString, %a1, %a2)
{
   %list = deTag(%a1);
   TeamOneHeader.clear();
   TeamTwoHeader.clear();
   if(%a2 > 1)
   {
      %header[1] = getField(%list, 0) SPC
                   getField(%list, 1);

      %header[2] = getField(%list, 2) SPC
                   getField(%list, 3);

      TeamOneHeader.setText(%header[1]);
      TeamTwoHeader.setText(%header[2]);
   }
   else
   {
      %header = getField(%list, 0) SPC
                getField(%list, 1);

      TeamOneHeader.setText(%header);
      TeamTwoHeader.setText("");
   }
   updatePlayerScoreList();
}

function updatePlayerScoreList(%this, %player)
{
   ScoreHud.PlayerScore[0].clear();
   ScoreHud.PlayerScore[1].clear();
   ScoreHud.PlayerScore[2].clear();
   for(%i = 0; %i < PlayerListGroup.getCount(); %i++)
   {
      %player = PlayerListGroup.getObject(%i);
      ScoreHud.PlayerScore[%player.teamId].addRow(%player.clientId, %player.playerName TAB %player.score);
      ScoreHud.PlayerScore[%player.teamId].sortNumerical(1, false);
   }
}

