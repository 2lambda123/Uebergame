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

//------------------------------------------------------------------------------
function LoadingGui::onAdd(%this)
{
   %this.qLineCount = 0;
}

//------------------------------------------------------------------------------
function LoadingGui::onWake(%this)
{
   CloseMessagePopup();
   %this.reset();
   LOAD_MessageVector.attach(HudMessageVector);
   LOAD_ScrollHud.scrollToBottom();
}

//------------------------------------------------------------------------------
function LoadingGui::onSleep(%this)
{
   %this.reset();
}

function LoadingGui::reset(%this)
{
   // Set the load bitmap to default
   LOAD_MapPic.setBitmap( $MainGuiBackground );

   // Clear the load info:
   LOAD_MapName.setText("");
   LOAD_MapDescription.setText("");
   LOAD_MissionType.setText("");
   LOAD_GameText.setText("");
   LOAD_ServerInfo.setText("");
   LoadingProgress.setValue(0);
   LoadingProgress.setText("WAITING FOR SERVER");

   // Stop sound...
}

function LOAD_MessageVector::addLine(%this, %text)
{
   ChatHud::addLine(%this, %text);
}

//------------------------------------------------------------------------------
// Before downloading a mission, the server transmits the mission
// information through these messages.
//------------------------------------------------------------------------------

addMessageCallback('MsgLoadInfo', handleLoadInfoMessage);
addMessageCallback('MsgLoadDescLine', handleLoadDescriptionMessage);
addMessageCallback('MsgLoadRulesLine', handleLoadRulesLineMessage);
addMessageCallback('MsgLoadServerInfoLine', handleLoadServerInfoMessage);
addMessageCallback('MsgLoadInfoDone', handleLoadInfoDoneMessage);
addMessageCallback('MsgLoadFailed', handleLoadFailedMessage);

//------------------------------------------------------------------------------

function handleLoadInfoMessage(%msgType, %msgString, %misFile, %mapName, %typeName, %missionType)
{
   // Need to pop up the loading gui to display this stuff.
   // But if we are using a lobby, this would overload that so we rem it.
   // Show the loading screen immediately if it isn't current already.
   if ( isObject( LoadingGui ) )
   {
      if(Canvas.getContent() != LoadingGui.getId())
      {
         Canvas.setContent("LoadingGui");
      }
      LoadingProgress.setValue(1);
      LoadingProgress.setValue("LOADING MISSION FILE");
      Canvas.repaint();
   }

   $Client::MissionType = %missionType;

   // Clear the mission description text from the admin menu hud
   for(%i = 0; %i < AdminDlg.qLineCount; %i++)
      AdminDlg.qLine[%i] = "";

   AdminDlg.qLineCount = 0;

   // Set the load bitmap which is the mission name.jpg
   %image = filePath(%misFile) @ "/" @ fileBase(%misFile) @ "_preview";
   // Test against all of the different image formats
   // This should probably be moved into an engine function
   if (isFile(%image @ ".png") ||
       isFile(%image @ ".jpg") ||
       isFile(%image @ ".bmp"))
   {
      LOAD_MapPic.setBitmap( %image );
   }
   else
   {
      LOAD_MapPic.setBitmap( "levels/load_mission.jpg" );
   }

   LOAD_MapName.setText( "<color:ffffff><shadowcolor:000000><shadow:3:3><font:Arial Bold:24>" @ %mapName );
   LOAD_MissionType.setText( "<color:ffffff><shadowcolor:000000><shadow:3:3><font:Arial Bold:24>" @ %typeName );
}

//------------------------------------------------------------------------------

function handleLoadDescriptionMessage(%msgType, %msgString, %line)
{
   %text = "<color:ffffff><shadowcolor:000000><shadow:2:2><font:Arial Bold:18>" @ %line;
   // Use the bool TRUE to cause the text to reformat after each line being added.
   LOAD_MapDescription.addtext( %text @ "\n\n", true );

   // Fill the mission description text for the admin menu hud
   AdminDlg.qLine[AdminDlg.qLineCount] = %line;
   AdminDlg.qLineCount++;
}

//------------------------------------------------------------------------------

function handleLoadRulesLineMessage(%msgType, %msgString, %line)
{
   %text = "<color:ffffff><shadowcolor:000000><shadow:2:2><font:Arial Bold:18>" @ %line;
   // Use the bool TRUE to cause the text to reformat after each line being added.
   LOAD_GameText.addtext( %text @ "\n\n", true );
}

//------------------------------------------------------------------------------

function handleLoadServerInfoMessage(%msgType, %msgString, %line)
{
   %text = "<color:ffffff><shadowcolor:000000><shadow:2:2><font:Arial Bold:18>" @ %line;
   // Use the bool TRUE to cause the text to reformat after each line being added.
   LOAD_ServerInfo.addtext( %text @ "\n\n", true );
}

//------------------------------------------------------------------------------

function handleLoadInfoDoneMessage(%msgType, %msgString)
{
   // This will get called after the last description line is sent.
}

//------------------------------------------------------------------------------

function handleLoadFailedMessage( %msgType, %msgString )
{
   MessageBoxOK( "Mission Load Failed", %msgString NL "Press OK to return to the Main Menu", "disconnect();" );
}