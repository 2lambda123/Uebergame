function ScoreHudFFA::onWake(%this)
{
   ScoreHudFFAListLabel.clear();
   %header = "NAME" TAB "TEAM" TAB "SCORE";
   ScoreHudFFAListLabel.addRow(0, %header);
   ScoreHudFFAListLabel.setSelectedRow(0);

      ScoreHudFFAList.clear();
      for (%i = 0; %i < PlayerListGuiList.rowCount(); %i++)
      {
         %text = PlayerListGuiList.getRowText(%i);
         %id = PlayerListGuiList.getRowId(%i);
         ScoreHudFFAList.addRow(%id, %text);
      }
      ScoreHudFFAList.sortNumerical(1, false);
	  
	  ScoreHudFFA.GetScoreHudFFAUpdate();  
}

function ScoreHudFFA::toggle(%this, %val)
{
   if(%this.isAwake())
      Canvas.popDialog(%this);
   else
      Canvas.pushDialog(%this);
}

function ScoreHudFFA::refresh(%this)
{
      ScoreHudFFAList.clear();
      for (%i = 0; %i < PlayerListGuiList.rowCount(); %i++)
      {
         %text = PlayerListGuiList.getRowText(%i);
         %id = PlayerListGuiList.getRowId(%i);
         ScoreHudFFAList.addRow(%id, %text);
      }
      ScoreHudFFAList.sortNumerical(1, false);
}

function ScoreHudFFA::GetScoreHudFFAUpdate(%this)
{
   //cancel refresh in case the Hud was closed
   cancel($ScoreHudFFAUpdate);
   //enter a loop that refreshes the Hud every 3 seconds
   if ( %this.isAwake() )
      ScoreHudFFA.refresh();
  
  $ScoreHudFFAUpdate = ScoreHudFFA.schedule(3000, "GetScoreHudFFAUpdate");
}