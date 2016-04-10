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

//$QuickChatMenu::titleColor  = "<font:" @ "Arial Bold" @ ":14><color:7FFFD4>";
//$QuickChatMenu::groupColor  = "<font:" @ "Arial Bold" @ ":14><color:00CED1>";
//$QuickChatMenu::chatColor   = "<font:" @ "Arial Bold" @ ":14><color:FFFFFF>";
//$QuickChatMenu::escapeColor = "<font:" @ "Arial Bold" @ ":14><color:00CED1>";

function toggleQuickChatHud( %val )
{
   if ( %val )
      canvas.pushdialog(QuickChatMenuHud);
}

if( isObject(ChatMenuActionMapGroup) )
   ChatMenuActionMapGroup.delete();

new SimSet(ChatMenuActionMapGroup);
ActionMapGroup.add(ChatMenuActionMapGroup);

new ActionMap(ChatMenuGroupAction);
ChatMenuActionMapGroup.add(ChatMenuGroupAction);
ChatMenuGroupAction.bindCmd(keyboard, "c", "canvas.popdialog(QuickChatMenuHud);", "");

if( isObject(ChatMenuGroup) )
   ChatMenuGroup.delete();

$CurrentChatMenu = new SimSet(ChatMenuGroup) {
                      title = "Quick Chat Menu";
                      isGroup = true;
                      childCount = 0;
                      parent = ChatMenuGroup;
                      actionMap = ChatMenuGroupAction; };

function openChatRoot()
{
   // reset the 'current' pointer to the root object
   $CurrentChatMenu = ChatMenuGroup;
}

function openChatGroup( %key, %name )
{
   // create a new group object and set the 'current' pointer
   %parent = $CurrentChatMenu;
   %childActionMap = new ActionMap(){};
   ChatMenuActionMapGroup.add(%childActionMap);
   %child = new SimGroup() {
      key = %key;
      title = %name;
      parent = %parent;
      isGroup = true;
      childCount = 0;
      actionMap = %childActionMap;
   };
   %parent.add(%child);

   %parent.actionMap.bindCmd(keyboard, %key, "QuickChatMenuHud.selectObject("@ %child @");", "");
   %parent.child[%parent.childCount] = %child;
   %parent.childCount++;
    
   %child.actionMap.bindCmd(keyboard, "c", "parentChatGroup();", "");
    
   $CurrentChatMenu = %child;
}

function closeChatGroup()
{
   // set the 'current' pointer to the parent of the current target
   $CurrentChatMenu = $CurrentChatMenu.parent;
}

function addChatItem( %key, %teamOnly, %name, %text, %audioFile, %animation, %play3D )
{
   // add a chat object to the current 'current' pointer target
   %parent = $CurrentChatMenu;
   %child = new SimObject() {
      key = %key;
      title = %name;
      parent = %parent;
      isGroup = false;
      teamOnly = %teamOnly;
      text = %text;
      audioFile = %audioFile;
      animation = %animation;
      play3D = %play3D;
   };
   %parent.add(%child);

   %parent.actionMap.bindCmd(keyboard, %key, "QuickChatMenuHud.selectObject("@ %child @");", "");
   %parent.child[%parent.childCount] = %child;
   %parent.childCount++;
}

function parentChatGroup()
{
   $CurrentChatMenu.actionMap.pop();
   QuickChatMenuHud.selectObject($CurrentChatMenu.parent);
}

// -----------------------------------------------------------------------------

function QuickChatMenuHud::onWake( %this )
{
   %this.readObject(ChatMenuGroup);
}

function QuickChatMenuHud::readObject( %this, %object )
{
   %displayString = "<font:" @ "Arial Bold" @ ":14><color:7FFFD4>" @ %object.title;

   for ( %i = 0; %i < %object.childCount; %i++ )
   {
      %child = %object.child[%i];

      // add this item to the display string
      if( %child.isGroup )
         %displayString = %displayString @ "<font:" @ "Arial Bold" @ ":14><color:00CED1>" @ "\n";
      else
         %displayString = %displayString @ "<font:" @ "Arial Bold" @ ":14><color:FFFFFF>" @ "\n";

      %displayString = %displayString @ %child.key @ ": " @ %child.title;
   }

   %displayString = %displayString @ "\n" @ "<font:" @ "Arial Bold" @ ":14><color:00CED1>" @ "C: leave chat menu";

   QCMenuText.setText(%displayString);
   QuickChatBase.extent = firstWord(QuickChatBase.extent) SPC 30 + (15*%object.childCount);

   %object.actionMap.push();
   $CurrentChatMenu = %object;
}

function QuickChatMenuHud::onSleep( %this )
{
   $CurrentChatMenu.actionMap.pop();
}

function QuickChatMenuHud::selectObject( %this, %object )
{
   %object.parent.actionMap.pop();

   if ( %object.isGroup )
        %this.readObject(%object);
   else
   {
      //if ( %object.animation !$="" )
         commandToServer( 'PlayCel', %object.animation );

      if ( %object.audioFile )
         commandToServer( %object.teamOnly ? 'teamMessageSent' : 'MessageSent', %object.text @ "~w" @ %object.audioFile );
      else
         commandToServer( %object.teamOnly ? 'teamMessageSent' : 'MessageSent', %object.text );

      canvas.popdialog(%this);
   }
}

// -----------------------------------------------------------------------------

if ( isFile( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/quickChat.cs" ) )
   exec( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/quickChat.cs" );
else
   exec("./quickChat.cs");
