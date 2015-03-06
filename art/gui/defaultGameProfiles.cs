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
//GuiMenuButtonProfile.soundButtonOver = "AudioButtonOver";
//GuiMenuButtonProfile.soundButtonDown = "AudioButtonDown";

//-----------------------------------------------------------------------------
// Chat Hud profiles


singleton GuiControlProfile (ChatHudEditProfile)
{
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

singleton GuiControlProfile (ChatHudTextProfile)
{
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

singleton GuiControlProfile ("ChatHudMessageProfile")
{
   fontType = "Arial";
   fontSize = "16";
   fontColor = "255 255 255 255";      // default color (death msgs, scoring, inventory)
   fontColors[1] = "255 255 255 255";   // client join/drop, tournament mode
   fontColors[2] = "255 255 255 255"; // gameplay, admin/voting, pack/deployable
   fontColors[3] = "255 255 255 255";   // team chat, spam protection message, client tasks
   fontColors[4] = "255 255 255 255";  // global chat
   fontColors[5] = "255 255 255 255";  // used in single player game
   // WARNING! Colors 6-9 are reserved for name coloring
   autoSizeWidth = true;
   autoSizeHeight = true;
   fontColors[0] = "255 255 255 255";
   fontColorHL = "255 255 255 255";
   fontColorNA = "255 255 255 255";
   fontColorSEL = "255 255 255 255";
   fontColorLink = "255 255 255 255";
   fontColorLinkHL = "255 255 255 255";
   bevelColorHL = "255 0 255 255";
   fontColors[6] = "255 0 255 255";
   fontColors[7] = "255 0 255 255";
};

singleton GuiControlProfile ("ChatHudScrollProfile")
{
   opaque = false;
   borderThickness = 0;
   borderColor = "0 255 0";
   bitmap = "core/art/gui/images/scrollBar";
   hasBitmapArray = true;
};


//-----------------------------------------------------------------------------
// Core Hud profiles

singleton GuiControlProfile ("HudScrollProfile")
{
   opaque = false;
   border = true;
   borderColor = "0 255 0";
   bitmap = "core/art/gui/images/scrollBar";
   hasBitmapArray = true;
};

singleton GuiControlProfile ("HudTextProfile")
{
   opaque = false;
   fillColor = "128 128 128";
   fontColor = "0 255 0";
   border = true;
   borderColor = "0 255 0";
};

singleton GuiControlProfile ("ChatHudBorderProfile")
{
   bitmap = "core/art/gui/images/chatHudBorderArray";
   hasBitmapArray = true;
   opaque = false;
};


//-----------------------------------------------------------------------------
// Center and bottom print

singleton GuiControlProfile ("CenterPrintProfile")
{
   opaque = false;
   fillColor = "128 128 128";
   fontColor = "0 255 0";
   border = true;
   borderColor = "0 255 0";
};

singleton GuiControlProfile ("CenterPrintTextProfile")
{
   opaque = false;
   fontType = "Arial";
   fontSize = 12;
   fontColor = "0 255 0";
};

// -----------------------------------------------------------------------------
// HUD text
// -----------------------------------------------------------------------------

singleton GuiControlProfile (HudTextNormalProfile)
{
   opaque = false;
   fontType = "Arial";
   fontSize = "16";
   fontColor = "210 210 210 255";
   fontColors[0] = "210 210 210 255";
};

singleton GuiControlProfile (HudTextItalicProfile : HudTextNormalProfile)
{
   fontType = "ArialItalic";
   fontSize = "26";
   fontColors[0] = "200 200 200 255";
   fontColor = "200 200 200 255";
};

singleton GuiControlProfile (HudTextBoldProfile : HudTextNormalProfile)
{
   fontType = "ArialBold";
   fontSize = "16";
   fontColors[0] = "210 210 210 255";
   fontColor = "210 210 210 255";
};

// -----------------------------------------------------------------------------
// Numerical health text
// -----------------------------------------------------------------------------

singleton GuiControlProfile (NumericHealthProfile)
{
   opaque = true;
   justify = "center";
   fontType = "ArialBold";
   fontSize = 32;
   fontColor = "255 255 255";
};
