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

function ArmoryDlg::configure(%this)
{
   %this.childGui = Armory_Root;
   %this.childGui.parent = %this;

   LoadoutMenu.clear();

   // ------------------------------------------------------------ Loadout list
   for(%i = 0; %i < 10; %i++)
      LoadoutMenu.add($pref::Player::LoadoutName[%i], %i);

   %favId = LoadoutMenu.findText(getField($pref::Player::LoadoutName[$pref::Player::SelectedLoadout], 0));
   if(%favId == -1)
      %favId = "Empty";

   LoadoutMenu.setSelected(%favId);
   LoadoutMenu.onSelect(%favId, "");

   %this.configured = 1;
}

function clientCmdToggleArmory( %val )
{
   if( ArmoryDlg.isAwake() )
      commandToServer( 'HideArmoryHud' );
   else
   {
      commandToServer( 'ShowArmoryHud' );
      commandToServer( 'PushTeamChooseMenu' );
      commandToServer( 'PushSpawnChooseMenu' );
   }
}

function clientCmdOpenArmoryHud()
{
   if( !ArmoryDlg.isAwake() )
      Canvas.pushDialog( ArmoryDlg );

   if(!ArmoryDlg.configured)
      ArmoryDlg.configure();
}

function clientCmdCloseArmoryHud()
{
   if( ArmoryDlg.isAwake() )
      Canvas.popDialog( ArmoryDlg );
}

function clientCmdClearArmoryHud(%line)
{
   %hud = ArmoryDlg.getId();
   while ( isObject(%hud.data[%line, 0]) )
   {
      for( %i = 0; %i < %hud.numCol; %i++ )
      {
         //remove and delete the hud line
         %obj = %hud.data[%line, %i];
         %obj.clear();
         %hud.childGui.remove(%obj);
         %obj.delete();
         %hud.data[%line, %i] = "";
      }
      %line++;
   }
}

function clientCmdRemoveArmoryLine(%line)
{
   //error("clientCmdRemoveArmoryLine(" SPC %line SPC ")");
   %hud = ArmoryDlg.getId();
   if( %hud.data[%line, 0] !$= "" )
   {
      for( %i = 0; %i < %hud.numCol; %i++ )
      {
         %obj = %hud.data[%line, %i];
         %obj.clear();
         %hud.childGui.remove(%obj);
         %obj.delete();
         %hud.data[%line, %i] = "";
      }
   }
}

function clientCmdSetArmoryLine(%line, %a0, %a1, %a2, %a3)
{
   // Line | Text | List | Type | FavCount
   //LogEcho("clientCmdSetArmoryLine(" SPC %line @", "@ %a0 @", "@ %a1 @", "@ %a3 SPC")");
   %hud = ArmoryDlg.getId();

   if( !isObject( %hud.data[%line, 0] ) )
   {
      %hud.numCol = makeEntry(%hud, %line, %a0, %a1, %a2, %a3);

      for(%i = 0; %i < %hud.numCol; %i++)
         %hud.childGui.add( %hud.data[%line, %i] );
   }

   for(%i = 0; %i < %hud.numCol; %i++)
      %hud.data[%line, %i].hudSetValue(detag(%a[%i]), %a4);   
}

function GuiButtonCtrl::hudSetValue(%this, %text)
{
   %this.setValue(%text);
}

function GuiTextCtrl::hudSetValue(%this, %text)
{
   %this.setValue(%text);
}

function GuiMLTextCtrl::hudSetValue(%this, %text)
{
   %this.setValue(%text);
}

function GuiPopUpMenuCtrl::hudSetValue(%this, %text, %textOverFlow)
{
   //LogEcho("GuiPopUpMenuCtrl::hudSetValue(" SPC %this.getName() SPC %text SPC %textOverFlow SPC ")");
   if(%textOverFlow !$= "")
      %text = %text @ %textOverFlow;

   %this.clear();
   %value = getField(%text, 0);
   %startVal = 1;
   if(%value $= "noSelect")
   {
      %this.replaceText(false);
      %value = getField(%text, 1);
      %startVal = 2;
   }
   else
      %this.replaceText(true);

   %this.setValue(%value);
   if(getFieldCount(%text) > 1)
   {
      %this.setActive(true);
      for(%i = %startVal; %i < getFieldCount(%text); %i++)
         %this.add(getField(%text, %i), %i);
   }
   else
      %this.setActive(false);
}

function makeEntry(%hud, %lineNum, %a0, %a1, %a2, %a3)
{
   //error("makeEntry(" SPC %hud @", "@ %lineNum @", "@ %a0 @", "@ %a1 @", "@ %a2 @", "@ %a3 SPC ")");
   %colNum = 0;
   if( isObject( %hud ) )
      %colNum = %hud.addLine(%lineNum, detag(%a2), detag(%a3));

   return %colNum;
}

//------------------------------------------------------------------------------

function ArmoryDlg::addLine( %this, %lineNum, %type, %count )
{
   //Logecho("ArmoryDlg::addLine(" SPC %this.getName() @", "@ %lineNum @", "@ %type @", "@ %count SPC ")");

   %this.count = %count;

   // Add label:
   %yOffset = ( %lineNum * 30 ) + 68;
   %this.data[%lineNum, 0] = new GuiTextCtrl("Armory_Menu_txt" @ %lineNum) 
   {
      profile = "GuiTxtRightWhtProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "250 " @ %yOffset -10;
      extent = "150 20";
      minExtent = "8 8";
      visible = "1";
      setFirstResponder = "0";
      modal = "1";
      helpTag = "0";
      text = "";
   };

   // Add drop menu:
   %this.data[%lineNum, 1] = new GuiPopUpMenuCtrl("Armory_Menu" @ %lineNum)
   {
      profile = "GuiPopUpMenuProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "448 " @ %yOffset - 10;
      extent = "150 20";
      minExtent = "8 2";
      visible = "1";
      helpTag = "0";
      maxPopupHeight = "200";
      type = %type;
   };

   return 2;
}

function Armory_Menu0::onSelect( %obj, %index, %text )
{
   //LogEcho("Armory_Menu::onSelect(" SPC %obj.getName() @", "@ %index @", "@ %text SPC ")");
   %hud = ArmoryDlg.getId();
   if ( isObject( %hud.data[0, 1] ) )
      %list = %hud.data[0, 1].type TAB %hud.data[0, 1].getValue();
   else
      %list = "";

   for ( %i = 1; %i < %hud.count; %i++ )
   {
      if(isObject(%hud.data[%i, 1]))
         %list = %list TAB %hud.data[%i, 1].type TAB %hud.data[%i, 1].getValue();
   }
   LogEcho("Armory_Menu::onSelect - Sending server inventory list:" SPC %list);
   commandToServer( 'setClientLoadout', addTaggedString(%list) );
}

function Armory_Menu1::onSelect( %obj, %index, %text )
{
   Armory_Menu0::onSelect( %obj, %index, %text );
}

function Armory_Menu2::onSelect( %obj, %index, %text )
{
   Armory_Menu0::onSelect( %obj, %index, %text );
}

function Armory_Menu3::onSelect( %obj, %index, %text )
{
   Armory_Menu0::onSelect( %obj, %index, %text );
}

function Armory_Menu4::onSelect( %obj, %index, %text )
{
   Armory_Menu0::onSelect( %obj, %index, %text );
}

function Armory_Menu5::onSelect( %obj, %index, %text )
{
   Armory_Menu0::onSelect( %obj, %index, %text );
}

function Armory_Menu6::onSelect( %obj, %index, %text )
{
   Armory_Menu0::onSelect( %obj, %index, %text );
}

//------------------------------------------------------------------------------
// Loadout - Loading and saving

function setUpFavPrefs()
{
   if($pref::Player::SelectedLoadout $= "")
      $pref::Player::SelectedLoadout = 0;   
   for(%i = 0; %i < 10; %i++)
   {
      if($pref::Player::LoadoutName[%i] $= "")
         $pref::Player::LoadoutName[%i] = "Loadout " @ %i + 1;
      if($pref::Player::Loadout[%i] $= "")
         $pref::Player::Loadout[%i] = "armor\tSoldier\tWeapon\tLurker rifle\tSpecial\tMunitions\tGrenade\tGrenade";
   }
   if($pref::Player::FavCurrentList $= "")
      $pref::Player::FavCurrentList = 0;
}
setUpFavPrefs();

function ArmoryDlg::onWake(%this)
{
   %this.pushed = 1;
   if ( isObject( hudMap ) )
   {
      hudMap.pop();
      hudMap.delete();
   }
   new ActionMap( hudMap );
   hudMap.blockBind( moveMap, bringUpOptions );
   hudMap.blockBind( moveMap, showPlayerList );
   hudMap.blockBind( moveMap, showScoreBoard );
   // If escape is mapped to moveMap this will work fine else it will crash :)
   //hudMap.bindCmd( keyboard, "escape", "", "Canvas.popDialog(%this);" );
   hudMap.push();
}

function ArmoryDlg::onSleep(%this)
{
   %this.pushed = 0;

   // Store our currently selected loadout so we can reload it on level change
   %list = $pref::Player::Loadout[$pref::Player::SelectedLoadout];

   for ( %i = 1; %i < %this.count; %i++ )
   {
      if( isObject( %this.data[%i, 1] ) )
      {
         if(%this.data[%i, 1].getValue() $= invalid)
            %list = %list TAB %this.data[%i, 1].type TAB "EMPTY";
         else
            %list = %list TAB %this.data[%i, 1].type TAB %this.data[%i, 1].getValue();
      }
   }
   $pref::Player::CurrentLoadout = %list;

   // Make sure the proper key maps are pushed
   tge.updateKeyMaps(); 
}

function clientCmdInitLoadFavorite()
{
   loadLoadout( $pref::Player::SelectedLoadout, 0 );
}

function LoadoutMenu::onSelect(%this, %id, %text)
{
   // Only select if it was the mouse that called this
   if(%text $= "")
      return;

   // Update all of the Armory buttons with the selected Loadout
   loadLoadout( %id, 1 );
}

function loadLoadout( %index, %echo )
{
   $pref::Player::SelectedLoadout = %index;
   %list = mFloor( %index / 20 );
   %hud = ArmoryDlg.getId();

   if ( isObject( %hud ) )
   {
      // Update the Edit Name field:
      LoadoutInput.setValue( $pref::Player::LoadoutName[%index] );
      SaveLoadout.setActive(strlen(trim($pref::Player::LoadoutName[%index])) >= 3);
   }

   if ( %echo )
      onServerMessage( "Loadout: \c2\"" @ $pref::Player::LoadoutName[%index] @ "\" \c0selected." );

   //LogEcho("loadLoadout - Sending server inventory list:" SPC $pref::Player::Loadout[%index]);
   commandToServer( 'setClientLoadout', addTaggedString($pref::Player::Loadout[%index]) );
}

function LoadoutInput::setField(%this)
{
   // called when you type in text input field
   %name = %this.getValue();
   %this.setValue(%name);
   SaveLoadout.setActive(strlen(trim(%name)) >= 3);
}

function ArmoryDlg::saveLoadout(%this)
{
   if ( $pref::Player::SelectedLoadout !$= "" )
   {
      // First get the string typed into the GuiTextEditCtrl
      LoadoutInput.setField();
      %name = LoadoutInput.getValue();

      // Now we have to add this to the Loadout array
      $pref::Player::LoadoutName[$pref::Player::SelectedLoadout] = %name;

      // This must be in proper order or bad evil things will happen!
      %list = %this.data[0, 1].type TAB %this.data[0, 1].getValue();
      for ( %i = 1; %i < %this.count; %i++ )
      {
         if(isObject(%this.data[%i, 1]))
         {
            %text = %this.data[%i, 1].getValue();
            if ( %text $= "Invalid" )
               %text = "Empty";

            %list = %list TAB %this.data[%i, 1].type TAB %text;
         }
      }

      $pref::Player::Loadout[$pref::Player::SelectedLoadout] = %list;
      echo("exporting pref::* to client.config.cs");
      export("$pref::*", ( GetUserHomeDirectory() @ "/My Games/" @ $AppName @ "/client.config.cs" ), False);
   }
   // Clear the text input field and disable send button
   SaveLoadout.setActive(0);
   LoadoutInput.setValue("");

   // ------------------------------------------------------------ re-populate the list
   LoadoutMenu.clear();
   for(%i = 0; %i < 10; %i++)
      LoadoutMenu.add($pref::Player::LoadoutName[%i], %i);

   %favId = LoadoutMenu.findText(getField($pref::Player::LoadoutName[$pref::Player::SelectedLoadout], 0));
   if(%favId == -1)
      %favId = "Empty";

   LoadoutMenu.setSelected(%favId);
   LoadoutMenu.onSelect(%favId, "");
}

function clientCmdPushTeamMenu(%list)
{
   TeamMenu.clear();
   for(%i = 0; %i < getFieldCount(deTag(%list)); %i++)
   {
      if(getField(deTag(%list), %i) $= "Empty")
         TeamMenu.add("", %i);
      else
         TeamMenu.add(getField(deTag(%list), %i), %i);
   }
   TeamMenu.setValue("CHOOSE TEAM");
}

function clientCmdProcessChooseTeam(%option)
{
   if(%option !$= "" && %option <= 5)
      commandToServer('clientChooseTeam', %option);
   else if(%option !$= "" && %option == 6)     
      escapeFromGame();
   
   clientCmdToggleArmory( 1 );
}

function TeamMenu::onSelect(%this, %id, %text)
{
   // Only select if it was the mouse that called this
   if(%text $= "")
      return;

   clientCmdprocessChooseTeam(%id);
}

function clientCmdPushSpawnMenu(%list)
{
   SpawnMenu.clear();
   for(%i = 0; %i < getFieldCount(deTag(%list)); %i++)
   {
      SpawnMenu.add(getField(deTag(%list), %i), %i);
   }
   SpawnMenu.setValue("CHOOSE SPAWN");
}

function SpawnMenu::onSelect(%this, %id, %text)
{
   // Only select if it was the mouse that called this
   if(%text $= "")
      return;

   commandToServer('clientChooseSpawn', %id, %text );
}
