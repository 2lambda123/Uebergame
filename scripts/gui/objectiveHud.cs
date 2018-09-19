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

// Called from handleGameInfoMessage which is sent every mission load
// by the Common game type. It deletes all elements first.
function objectiveHud::configure(%this, %gameType)
{
   %this.gameType = %gameType;

   switch$ (%gameType)
   {
      case "DMGame":
         %this.scoreLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "30 25";
            extent = "50 16";
            visible = "1";
            text = "SCORE";
         };
         %this.yourScore = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "120 25";
            extent = "30 16";
            visible = "1";
         };
         %this.killsLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "30 45";
            extent = "50 16";
            visible = "1";
            text = "KILLS";
         };
         %this.yourKills = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "120 45";
            extent = "30 16";
            visible = "1";
         };
         %this.deathsLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "195 35";
            extent = "50 16";
            visible = "1";
            text = "DEATHS";
         };
         %this.yourDeaths = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "260 35";
            extent = "30 16";
            visible = "1";
         };
         %this.add(%this.scoreLabel);
         %this.add(%this.yourScore);
         %this.add(%this.killsLabel);
         %this.add(%this.yourKills);
         %this.add(%this.deathsLabel);
         %this.add(%this.yourDeaths);
         
      case "BRDMGame":
         %this.scoreLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "30 25";
            extent = "50 16";
            visible = "1";
            text = "SCORE";
         };
         %this.yourScore = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "120 25";
            extent = "30 16";
            visible = "1";
         };
         %this.killsLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "30 45";
            extent = "50 16";
            visible = "1";
            text = "KILLS";
         };
         %this.yourKills = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "120 45";
            extent = "30 16";
            visible = "1";
         };
         %this.deathsLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "195 35";
            extent = "50 16";
            visible = "1";
            text = "DEATHS";
         };
         %this.yourDeaths = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "260 35";
            extent = "30 16";
            visible = "1";
         };
         %this.add(%this.scoreLabel);
         %this.add(%this.yourScore);
         %this.add(%this.killsLabel);
         %this.add(%this.yourKills);
         %this.add(%this.deathsLabel);
         %this.add(%this.yourDeaths);
		 
	   case "PBDMGame":
         %this.scoreLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "30 25";
            extent = "50 16";
            visible = "1";
            text = "SCORE";
         };
         %this.yourScore = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "120 25";
            extent = "30 16";
            visible = "1";
         };
         %this.killsLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "30 45";
            extent = "50 16";
            visible = "1";
            text = "KILLS";
         };
         %this.yourKills = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "120 45";
            extent = "30 16";
            visible = "1";
         };
         %this.deathsLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "195 35";
            extent = "50 16";
            visible = "1";
            text = "DEATHS";
         };
         %this.yourDeaths = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "260 35";
            extent = "30 16";
            visible = "1";
         };
         %this.add(%this.scoreLabel);
         %this.add(%this.yourScore);
         %this.add(%this.killsLabel);
         %this.add(%this.yourKills);
         %this.add(%this.deathsLabel);
         %this.add(%this.yourDeaths);
         
	   case "BRPBDMGame":
         %this.scoreLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "30 25";
            extent = "50 16";
            visible = "1";
            text = "SCORE";
         };
         %this.yourScore = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "120 25";
            extent = "30 16";
            visible = "1";
         };
         %this.killsLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "30 45";
            extent = "50 16";
            visible = "1";
            text = "KILLS";
         };
         %this.yourKills = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "120 45";
            extent = "30 16";
            visible = "1";
         };
         %this.deathsLabel = new GuiTextCtrl() {
            profile = "ObjTextTeamLeftProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "195 35";
            extent = "50 16";
            visible = "1";
            text = "DEATHS";
         };
         %this.yourDeaths = new GuiTextCtrl() {
            profile = "ObjTextTeamCenterProfile";
            horizSizing = "relative";
            vertSizing = "relative";
            position = "260 35";
            extent = "30 16";
            visible = "1";
         };
         %this.add(%this.scoreLabel);
         %this.add(%this.yourScore);
         %this.add(%this.killsLabel);
         %this.add(%this.yourKills);
         %this.add(%this.deathsLabel);
         %this.add(%this.yourDeaths);

      case "TDMGame":
         %this.teamName[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 25";
		extent = "70 16";
		visible = "1";
         };
         %this.teamName[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 45";
		extent = "70 16";
		visible = "1";
         };
         %this.teamScore[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 25";
		extent = "25 16";
		visible = "1";
         };
         %this.teamScore[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 45";
		extent = "25 16";
		visible = "1";
         };
         %this.deathsLabel[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "165 25";
		extent = "70 16";
		visible = "1";
		text = "Deaths:";
         };
         %this.deathsLabel[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "165 45";
		extent = "70 16";
		visible = "1";
		text = "Deaths:";
         };
         %this.deaths[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "245 25";
		extent = "43 16";
		visible = "1";
         };
         %this.deaths[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "245 45";
		extent = "43 16";
		visible = "1";
         };

         for(%i = 1; %i <= 2; %i++)
         {
            %this.add(%this.teamName[%i]);
            %this.add(%this.teamScore[%i]);
            %this.add(%this.deathsLabel[%i]);
            %this.add(%this.deaths[%i]);
         }
         
      case "BRTDMGame":
         %this.teamName[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 25";
		extent = "70 16";
		visible = "1";
         };
         %this.teamName[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 45";
		extent = "70 16";
		visible = "1";
         };
         %this.teamScore[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 25";
		extent = "25 16";
		visible = "1";
         };
         %this.teamScore[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 45";
		extent = "25 16";
		visible = "1";
         };
         %this.deathsLabel[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "165 25";
		extent = "70 16";
		visible = "1";
		text = "Deaths:";
         };
         %this.deathsLabel[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "165 45";
		extent = "70 16";
		visible = "1";
		text = "Deaths:";
         };
         %this.deaths[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "245 25";
		extent = "43 16";
		visible = "1";
         };
         %this.deaths[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "245 45";
		extent = "43 16";
		visible = "1";
         };

         for(%i = 1; %i <= 2; %i++)
         {
            %this.add(%this.teamName[%i]);
            %this.add(%this.teamScore[%i]);
            %this.add(%this.deathsLabel[%i]);
            %this.add(%this.deaths[%i]);
         }

		case "PBTDMGame":
         %this.teamName[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 25";
		extent = "70 16";
		visible = "1";
         };
         %this.teamName[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 45";
		extent = "70 16";
		visible = "1";
         };
         %this.teamScore[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 25";
		extent = "25 16";
		visible = "1";
         };
         %this.teamScore[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 45";
		extent = "25 16";
		visible = "1";
         };
         %this.deathsLabel[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "165 25";
		extent = "70 16";
		visible = "1";
		text = "Deaths:";
         };
         %this.deathsLabel[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "165 45";
		extent = "70 16";
		visible = "1";
		text = "Deaths:";
         };
         %this.deaths[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "245 25";
		extent = "43 16";
		visible = "1";
         };
         %this.deaths[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "245 45";
		extent = "43 16";
		visible = "1";
         };

         for(%i = 1; %i <= 2; %i++)
         {
            %this.add(%this.teamName[%i]);
            %this.add(%this.teamScore[%i]);
            %this.add(%this.deathsLabel[%i]);
            %this.add(%this.deaths[%i]);
         }
         
		case "BRPBTDMGame":
         %this.teamName[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 25";
		extent = "70 16";
		visible = "1";
         };
         %this.teamName[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 45";
		extent = "70 16";
		visible = "1";
         };
         %this.teamScore[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 25";
		extent = "25 16";
		visible = "1";
         };
         %this.teamScore[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 45";
		extent = "25 16";
		visible = "1";
         };
         %this.deathsLabel[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "165 25";
		extent = "70 16";
		visible = "1";
		text = "Deaths:";
         };
         %this.deathsLabel[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "165 45";
		extent = "70 16";
		visible = "1";
		text = "Deaths:";
         };
         %this.deaths[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "245 25";
		extent = "43 16";
		visible = "1";
         };
         %this.deaths[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "245 45";
		extent = "43 16";
		visible = "1";
         };

         for(%i = 1; %i <= 2; %i++)
         {
            %this.add(%this.teamName[%i]);
            %this.add(%this.teamScore[%i]);
            %this.add(%this.deathsLabel[%i]);
            %this.add(%this.deaths[%i]);
         }
		 
      case "MfDGame":
         %this.teamName[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 25";
		extent = "70 16";
		visible = "1";
         };
         %this.teamName[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 45";
		extent = "70 16";
		visible = "1";
         };
         %this.teamScore[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 25";
		extent = "25 16";
		visible = "1";
         };
         %this.teamScore[2] = new GuiTextCtrl() {
		profile = "ObjTextCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 45";
		extent = "25 16";
		visible = "1";
         };
         %this.markLabel[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "155 25";
		extent = "70 16";
		visible = "1";
		text = "Mark:";
         };
         %this.markLabel[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "155 45";
		extent = "70 16";
		visible = "1";
		text = "Mark:";
         };
         %this.markName[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "195 25";
		extent = "125 16";
		visible = "1";
         };
         %this.markName[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "195 45";
		extent = "135 16";
		visible = "1";
         };

         for(%i = 1; %i <= 2; %i++)
         {
            %this.add(%this.teamName[%i]);
            %this.add(%this.teamScore[%i]);
            %this.add(%this.markLabel[%i]);
            %this.add(%this.markName[%i]);
         }
         
      case "PBMfDGame":
         %this.teamName[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 25";
		extent = "70 16";
		visible = "1";
         };
         %this.teamName[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 45";
		extent = "70 16";
		visible = "1";
         };
         %this.teamScore[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 25";
		extent = "25 16";
		visible = "1";
         };
         %this.teamScore[2] = new GuiTextCtrl() {
		profile = "ObjTextCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 45";
		extent = "25 16";
		visible = "1";
         };
         %this.markLabel[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "155 25";
		extent = "70 16";
		visible = "1";
		text = "Mark:";
         };
         %this.markLabel[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "155 45";
		extent = "70 16";
		visible = "1";
		text = "Mark:";
         };
         %this.markName[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "195 25";
		extent = "125 16";
		visible = "1";
         };
         %this.markName[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "195 45";
		extent = "135 16";
		visible = "1";
         };

         for(%i = 1; %i <= 2; %i++)
         {
            %this.add(%this.teamName[%i]);
            %this.add(%this.teamScore[%i]);
            %this.add(%this.markLabel[%i]);
            %this.add(%this.markName[%i]);
         }

      case "RtFGame":
         %this.teamName[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 25";
		extent = "70 16";
		visible = "1";
         };
         %this.teamName[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 45";
		extent = "70 16";
		visible = "1";
         };
         %this.teamScore[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 25";
		extent = "25 16";
		visible = "1";
         };
         %this.teamScore[2] = new GuiTextCtrl() {
		profile = "ObjTextCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 45";
		extent = "25 16";
		visible = "1";
         };
         %this.flagStatusLabel = new GuiTextCtrl() {
		profile = "ObjTextTeamCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "155 25";
		extent = "135 16";
		visible = "1";
		text = "Flag Status";
         };
         %this.flagStatus = new GuiTextCtrl() {
		profile = "ObjTextTeamCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "155 45";
		extent = "135 16";
		visible = "1";
		text = "Home";
         };

         for(%i = 1; %i <= 2; %i++)
         {
            %this.add(%this.teamName[%i]);
            %this.add(%this.teamScore[%i]);
         }
         %this.add(%this.flagStatusLabel);
         %this.add(%this.flagStatus);
      
      case "PBRtFGame":
         %this.teamName[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 25";
		extent = "70 16";
		visible = "1";
         };
         %this.teamName[2] = new GuiTextCtrl() {
		profile = "ObjTextLeftProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "30 45";
		extent = "70 16";
		visible = "1";
         };
         %this.teamScore[1] = new GuiTextCtrl() {
		profile = "ObjTextTeamCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 25";
		extent = "25 16";
		visible = "1";
         };
         %this.teamScore[2] = new GuiTextCtrl() {
		profile = "ObjTextCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "120 45";
		extent = "25 16";
		visible = "1";
         };
         %this.flagStatusLabel = new GuiTextCtrl() {
		profile = "ObjTextTeamCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "155 25";
		extent = "135 16";
		visible = "1";
		text = "Flag Status";
         };
         %this.flagStatus = new GuiTextCtrl() {
		profile = "ObjTextTeamCenterProfile";
		horizSizing = "relative";
		vertSizing = "relative";
		position = "155 45";
		extent = "135 16";
		visible = "1";
		text = "Home";
         };

         for(%i = 1; %i <= 2; %i++)
         {
            %this.add(%this.teamName[%i]);
            %this.add(%this.teamScore[%i]);
         }
         %this.add(%this.flagStatusLabel);
         %this.add(%this.flagStatus);
   }
}

addMessageCallback('MsgClearObjHud', clearObjHudMSG);

function clearObjHudMSG(%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6)
{
   //remove everything in the objective Hud
   while (objectiveHud.getCount() > 0)
      objectiveHud.getObject(0).delete();
}

addMessageCallback('MsgCheckTeamLines', checkTeamLines);

function checkTeamLines(%msgType, %msgString, %team, %a2, %a3, %a4, %a5, %a6)
{
   if(%team > 2 || %team < 1)
      return;

   %other = %team == 1 ? 2 : 1;
   if(isObject(objectiveHud.teamName[1]))
   {
      %tY = getWord(objectiveHud.teamName[%team].position, 1);
      %oY = getWord(objectiveHud.teamName[%other].position, 1);

      // if player's team is lower on objective hud than other team is, switch them
      if(%tY > %oY)
         swapTeamLines();
   }
}

function swapTeamLines()
{
   %bLeft = "ObjTextLeftProfile";
   %bCenter = "ObjTextCenterProfile";
   %gLeft = "ObjTextTeamLeftProfile";
   %gCenter = "ObjTextTeamCenterProfile";

   // Make sure our team is always at the top of the hud
   %teamOneY = getWord(objectiveHud.teamName[1].position, 1);
   %teamTwoY = getWord(objectiveHud.teamName[2].position, 1);

   %newTop = %teamOneY > %teamTwoY ? 1 : 2;
   %newBottom = %teamOneY > %teamTwoY ? 2 : 1;

   %nameX = firstWord(objectiveHud.teamName[1].position);
   objectiveHud.teamName[1].position = %nameX SPC %teamTwoY;
   objectiveHud.teamName[2].position = %nameX SPC %teamOneY;
   objectiveHud.teamName[%newTop].setProfile(%gLeft);
   objectiveHud.teamName[%newBottom].setProfile(%bLeft);

   %scoreX = firstWord(objectiveHud.teamScore[1].position);
   objectiveHud.teamScore[1].position = %scoreX SPC %teamTwoY;
   objectiveHud.teamScore[2].position = %scoreX SPC %teamOneY;
   objectiveHud.teamScore[%newTop].setProfile(%gCenter);
   objectiveHud.teamScore[%newBottom].setProfile(%bCenter);

   if(isObject(objectiveHud.deaths[1])) // TDM game type
   {
      %locatX = firstWord(objectiveHud.deaths[1].position);
      objectiveHud.deaths[1].position = %locatX SPC %teamTwoY;
      objectiveHud.deaths[2].position = %locatX SPC %teamOneY;
      objectiveHud.deaths[%newTop].setProfile(%gLeft);
      objectiveHud.deaths[%newBottom].setProfile(%bLeft);
   }

   if(isObject(objectiveHud.markName[1])) // MfD game type
   {
      %locatX = firstWord(objectiveHud.markName[1].position);
      objectiveHud.markName[1].position = %locatX SPC %teamTwoY;
      objectiveHud.markName[2].position = %locatX SPC %teamOneY;
      objectiveHud.markName[%newTop].setProfile(%gCenter);
      objectiveHud.markName[%newBottom].setProfile(%bCenter);
   }
}

//Generic msg callbacks...
//-----------------------------------------------------------------------------
addMessageCallback('MsgYourRankIs', yourRankIs);

function yourRankIs(%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6)
{
   %rank = detag(%a1);
   error("Your rank is:" SPC %rank);
}

addMessageCallback('MsgTeamScoreIs', teamScoreIs);

function teamScoreIs(%msgType, %msgString, %teamNum, %score, %a3, %a4, %a5, %a6)
{
   if(%score $= "")
      %score = 0;

   if(isObject(objectiveHud.teamScore[%teamNum]))
      objectiveHud.teamScore[%teamNum].setValue(%score);
}

addMessageCallback('MsgYourScoreIs', handleYourScore);

function handleYourScore(%msgType, %msgString, %score)
{
   if(isObject(objectiveHud.yourScore))
      objectiveHud.yourScore.setValue(%score);
}

addMessageCallback('MsgYourDeaths', handleYourDeaths);

function handleYourDeaths(%msgType, %msgString, %deaths)
{
   if(%deaths $= "")
      %deaths = 0;

   if(isObject(objectiveHud.yourDeaths))
      objectiveHud.yourDeaths.setValue(%deaths);
}

addMessageCallback('MsgYourKills', handleYourKills);

function handleYourKills(%msgType, %msgString, %kills)
{
   if(%kills $= "")
      %kills = 0;

   if(isObject(objectiveHud.yourKills))
      objectiveHud.yourKills.setValue(%kills);
}

// Team Deathmatch - Hud Messages
//-----------------------------------------------------------------------------
function tdmAddTeam(%msgType, %msgString, %teamNum, %team)
{
   %teamName = detag(%team);
   if(isObject(objectiveHud.teamName[%teamNum]))
      objectiveHud.teamName[%teamNum].setValue(%teamName);
}

function tdmTeamDeaths(%msgType, %msgString, %teamNum, %deaths, %a3, %a4, %a5, %a6)
{
   if(isObject(objectiveHud.deaths[%teamNum]))
      objectiveHud.deaths[%teamNum].setValue(%deaths);
}

addMessageCallback('MsgTDMAddTeam', tdmAddTeam);
addMessageCallback('MsgTDMTeamDeaths', tdmTeamDeaths);

// Marked for Death - Hud Messages
//-----------------------------------------------------------------------------
function mfdAddTeam(%msgType, %msgString, %teamNum, %team)
{
   %teamName = detag(%team);
   if(isObject(objectiveHud.teamName[%teamNum]))
      objectiveHud.teamName[%teamNum].setValue(%teamName);
}

function mfdMarkName(%msgType, %msgString, %teamNum, %mark, %a3, %a4, %a5, %a6)
{
   %markName = stripTaggedVar(%mark);
   if(isObject(objectiveHud.markName[%teamNum]))
      objectiveHud.markName[%teamNum].setValue(%markName);
}

addMessageCallback('MsgMfDAddTeam', mfdAddTeam);
addMessageCallback('MsgMfDMarkName', mfdMarkName);

// Retrieve the Flag - Hud Messages
//-----------------------------------------------------------------------------
function rtfAddTeam(%msgType, %msgString, %teamNum, %team)
{
   %teamName = detag(%team);
   if(isObject(objectiveHud.teamName[%teamNum]))
      objectiveHud.teamName[%teamNum].setValue(%teamName);
}

function rtfFlagStatus(%msgType, %msgString, %name, %status, %a3, %a4, %a5, %a6)
{
   if(isObject(objectiveHud.flagStatus))
      objectiveHud.flagStatus.setValue(%status);
}

addMessageCallback('MsgRtFAddTeam', rtfAddTeam);
addMessageCallback('MsgRtFFlagStatus', rtfFlagStatus);
