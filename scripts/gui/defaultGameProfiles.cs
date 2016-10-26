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
// Override base controls
GuiMenuButtonProfile.soundButtonOver = "AudioButtonOverSoundList";
GuiMenuButtonProfile.soundButtonDown = "AudioButtonAcceptSoundList"; //positive sound for all else
GuiMenuButtonProfile2.soundButtonOver = "AudioButtonOverSoundList";
GuiMenuButtonProfile2.soundButtonDown = "AudioButtonCancelSoundList"; //negative sound for exit, cancel etc

//-----------------------------------------------------------------------------
// Chat Hud profiles

singleton GuiControlProfile (ChatHudEditProfile)
{
   fontType = "Arial";
   fontSize = 16;
   opaque = false;
   fillColor = "255 255 255";
   fillColorHL = "128 128 128";
   border = false;
   borderThickness = 0;
   borderColor = "40 231 240";
   fontColor = "40 231 240";
   fontColorHL = "40 231 240";
   fontColorNA = "128 128 128";
   textOffset = "0 2";
   autoSizeWidth = false;
   autoSizeHeight = true;
   tab = true;
   canKeyFocus = true;
};

singleton GuiControlProfile(GuiBevelLoweredProfile)
{
   opaque = true;
   bevelColorHL = "200 200 200";
   bevelColorLL = "64 64 64";

   fillColor = "105 105 105 128";
   border = true;
   borderColor = "0 0 0 80";
   bitmap = "art/gui/osxScroll";
   hasBitmapArray = true;
};

singleton GuiControlProfile (ChatHudTextProfile)
{
   fontType = "Arial";
   fontSize = 16;
   opaque = false;
   fillColor = "255 255 255";
   fillColorHL = "128 128 128";
   border = false;
   borderThickness = 0;
   borderColor = "40 231 240";
   fontColor = "40 231 240";
   fontColorHL = "40 231 240";
   fontColorNA = "128 128 128";
   textOffset = "0 0";
   autoSizeWidth = true;
   autoSizeHeight = true;
   tab = true;
   canKeyFocus = true;
};
/*
singleton GuiControlProfile ("ChatHudMessageProfile")
{
   fontType = "Arial";
   fontSize = 16;
   fontColor = "44 172 181";      // default color (death msgs, scoring, inventory)
   fontColors[1] = "4 235 105";   // client join/drop, tournament mode
   fontColors[2] = "219 200 128"; // gameplay, admin/voting, pack/deployable
   fontColors[3] = "77 253 95";   // team chat, spam protection message, client tasks
   fontColors[4] = "40 231 240";  // global chat
   fontColors[5] = "200 200 50 200";  // used in single player game
   // WARNING! Colors 6-9 are reserved for name coloring
   autoSizeWidth = true;
   autoSizeHeight = true;
};
*/
singleton GuiControlProfile(ChatHudMessageProfile)
{
   fontType = "Arial";
   fontSize = 16;
   fontColor = "255 224 128"; // default color (death msgs, scoring, inventory)
   fontColors[1] = "127 255 212";   // client join/drop, tournament mode
   fontColors[2] = "240 230 140"; // gameplay, admin/voting, pack/deployable

   fontColors[3] = "0 224 0"; //"255 255 255"; // team chat, spam protection message, client tasks

   fontColors[4] = "255 255 255"; //"255 255 0";  // global chat

   fontColors[5] = "200 200 50"; // used in single player game

   // WARNING! Colors 6-9 are reserved for name coloring
   fontColors[6] = "200 200 200"; // Player name color
   fontColors[7] = "220 220 20"; // Tag color
   fontColors[8] = "0 206 209"; // Smurf color
   fontColors[9] = "186 0 255"; // Bot name color

   autoSizeWidth = true;
   autoSizeHeight = true;
};

singleton GuiControlProfile ("HudScrollProfile")
{
   opaque = false;
   borderThickness = 0;
   borderColor = "128 0 0";
   bitmap = "art/gui/scrollBar";
   hasBitmapArray = true;
};

singleton GuiControlProfile ("HudBorderProfile")
{
   bitmap = "art/gui/borderArray";
   hasBitmapArray = true;
   opaque = false;
};

singleton GuiControlProfile(GuiShapeNameProfile)
{
   fontType = "Arial";
   fontSize = "14";
};

//-----------------------------------------------------------------------------

singleton GuiControlProfile(GuiTextNoKeyEditProfile)
{
   opaque = true;
   fillColor = "255 255 255";
   fillColorHL = "128 128 128";
   border = 3;
   borderThickness = 2;
   borderColor = "0 0 0";
   fontColor = "0 0 0";
   fontColorHL = "255 255 255";
   fontColorNA = "128 128 128";
   textOffset = "0 2";
   autoSizeWidth = false;
   autoSizeHeight = true;
   tab = false;
   canKeyFocus = true;
};

// -----------------------------------------------------------------------------
// Load Screen
// -----------------------------------------------------------------------------

singleton GuiControlProfile(GuiLoadPaneProfile)
{
   opaque = true;
   bevelColorHL = "255 255 128";
   bevelColorLL = "105 105 128";

   fillColor = "128 0 0 200";
};

//-----------------------------------------------------------------------------
// Score Hud profiles

singleton GuiControlProfile(GameOverHeaderProfile)
{
   opaque = false;
   fontSize = 24;
   fontColor = "255 255 255";
   justify = "center";
};

singleton GuiControlProfile(ScoreHeaderTextProfile)
{
   opaque = false;
   fontSize = 24;
   fontColor = "255 255 255";
   justify = "center";
};

singleton GuiControlProfile(TeamScoreTextLProfile : GuiTextProfile)
{
   fontSize = 18;
   fontColor = "255 255 255";
   fontColorHL = "255 100 100";
   fillColorHL = "200 200 200";
   justify = "left";
};

singleton GuiControlProfile(TeamScoreTextRProfile : GuiTextProfile)
{
   fontSize = 18;
   fontColor = "255 255 255";
   fontColorHL = "255 100 100";
   fillColorHL = "200 200 200";
   justify = "right";
};

singleton GuiControlProfile(BloodStreakProfile)
{
   opaque = false;
   fillColor = "128 0 0";
};

//-----------------------------------------------------------------------------
// Objective Hud profiles

//Bottom
singleton GuiControlProfile(ObjTextLeftProfile)
{
   fontType = "ArialBold";
   fontSize = "16";
   fontColor = "224 0 0";
   justify = "left";
   fontColors[0] = "224 0 0 255";
};
// Bottom
singleton GuiControlProfile(ObjTextCenterProfile : ObjTextLeftProfile)
{
   justify = "center";
   fontSize = "16";
};
//Top
singleton GuiControlProfile(ObjTextTeamLeftProfile : ObjTextLeftProfile)
{
   fontColor = "0 224 0";
   justify = "left";
   fontColors[0] = "0 224 0 255";
   fontColors[8] = "Magenta";
};
//Top
singleton GuiControlProfile(ObjTextTeamCenterProfile : ObjTextLeftProfile)
{
   fontColor = "0 224 0";  
   justify = "center";
   fontColors[0] = "0 224 0 255";
};

//-----------------------------------------------------------------------------
// Name coloring for optionsDlg

singleton GuiControlProfile(FriendNameTextProfile : GuiTextProfile)
{
   fontColor = "0 224 0";
   justify = "center";
};

singleton GuiControlProfile(EnemyNameTextProfile : GuiTextProfile)
{
   fontColor = "224 0 0";
   justify = "center";
};

//-----------------------------------------------------------------------------
// ScoreHud

singleton GuiControlProfile(ScoreHudHeaderProfile)
{
   fontType = "ArialBold";
   fontSize = 20;
   fontColor = "255 255 255";
};

singleton GuiControlProfile(ScoreHudHeaderBlueProfile)
{
   fontType = "ArialBold";
   fontSize = 18;
   fontColor = "150 200 255";
};

singleton GuiControlProfile(ScoreHudHeaderRedProfile)
{
   fontType = "ArialBold";
   fontSize = 18;
   fontColor = "255 200 150";
};

singleton GuiControlProfile(ScoreHudTextWhiteProfile)
{
   fontType = "ArialBold";
   fontSize = 14;
   fontColor = "240 240 240";
};

singleton GuiControlProfile(ScoreHudTextBlueProfile)
{
   fontType = "ArialBold";
   fontSize = 14;
   fontColor = "150 200 255";
};

singleton GuiControlProfile(ScoreHudTextRedProfile)
{
   fontType = "ArialBold";
   fontSize = 14;
   fontColor = "255 200 150";
};
