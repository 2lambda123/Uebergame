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
// Loading info is text displayed on the client side while the mission
// is being loaded.  This information is extracted from the mission file
// and sent to each the client as it joins.
//------------------------------------------------------------------------------

// Clears the mission info stored
function clearLoadInfo()
{
   for( %line = 0; %line < $HostGameRulesCount; %line++ )
      $HostGameRules[%line] = "";

   $HostGameRulesCount = 0;
}

// Extract the map description from the .mis file
function buildLoadInfo(%mission, %missionType)
{
   clearLoadInfo();
   $MissionDisplayName = fileBase(%mission);
   $MissionTypeDisplayName = %missionType;

   for( %i = 0; %i < $HostTypeCount; %i++ )
   {
      if( $HostTypeName[%i] $= %missionType )
         break;
   }

   $MissionDisplayName = $MissionDisplayName[%mission];
   $MissionTypeDisplayName = $HostTypeDisplayName[%i];
   for(%line = 0; $HostGameRules[%missionType, %line] !$= ""; %line++)
   {
      $HostGameRules[$HostGameRulesCount] = $HostGameRules[%missionType, %line];
      $HostGameRulesCount++;
   }
}

// Sends mission description to the client
function sendLoadInfoToClient(%client)
{
   //echo("sendLoadInfoToClient(" SPC %client.nameBase SPC ")");
   messageClient( %client, 'MsgLoadInfo', "", $Server::MissionFile, $MissionDisplayName, $MissionTypeDisplayName, $Server::MissionType );

   for( %line = 0; $HostMissionDesc[$Server::MissionFile, %line] !$= ""; %line++ )
   {
      messageClient(%client, 'MsgLoadDescLine', "", $HostMissionDesc[$Server::MissionFile, %line]);
   }

   for(%line = 0; %line < $HostGameRulesCount; %line++ )
   {
      if( $HostGameRules[%line] !$= "" )
         messageClient( %client, 'MsgLoadRulesLine', "", $HostGameRules[%line] );
   }

   for(%line = 0; $pref::Server::Message[%line] !$= ""; %line++ )
   {
      messageClient( %client, 'MsgLoadServerInfoLine', "", $pref::Server::Message[%line] );
   }
   
   if ($pref::Server::Hints)
   {
   messageClient( %client, 'MsgLoadHintTitleLine', "", getRandomHintTitle() );
   messageClient( %client, 'MsgLoadHintTextLine', "", getRandomHintText() );
   }
   messageClient( %client, 'MsgLoadInfoDone' );
}

function getRandomHintTitle(%text)
{
   %list = new ArrayObject();
   %list.add("0", "Hint:");
   %list.add("1", "Tip:");
   %list.add("2", "Note:");
   %list.add("3", "Clue:");
   %list.add("4", "Advice:");
   %list.add("5", "Pro Tip:");
   %list.add("6", "For those who do not read the manual:");
   %list.add("7", "Duion says:");
   %list.add("8", "Sidenote:");
   %list.add("9", "Read:");
   %list.add("10", "Read that:");
   %list.add("11", "Read this:");
   %list.add("12", "Some information while the game loads:");
   %list.add("13", "Fun fact:");
   %list.add("14", "Interesting fact:");
   %list.add("15", "Read this to kill the time until its loaded:");
   %list.add("16", "Reading makes you smart:");
   %list.add("17", "Hinweis:");
   %list.add("18", "Tipp:");
   %list.add("19", "Readme:");
   %list.add("20", "Bro Tip:");
   
   %random = getRandom(0, 20);
   %text = %list.getValue(%random);
   
   return (%text);
}

function getRandomHintText(%text)
{
   %list = new ArrayObject();
   %list.add("0", "You can host games yourself by pressing 'Host' in the main menu");
   %list.add("1", "Uebergame now has hints in the loading screen");
   %list.add("2", "If the game you hosted is not showing on the server list, check if your router or firewall is blocking the ports");
   %list.add("3", "You can add or vote in bots through the admin menu, hotkey 'o' or bottom left corner button in the ingame menu");
   %list.add("4", "The Armory dialog, hotkey 'i' or bottom right button lets you change your weapons and other equipment");
   %list.add("5", "Check out the AdminMenu, it lets you control your server directly or other servers through voting");
   %list.add("6", "Right clicking on Player's names in the AdminMenu list allows you to send private messages and more");
   %list.add("7", "There is a Fire Team feature, right click on Player's Names in the AdminMenu to invite them, then you can use Chat to fireteam function, hotkey 'u'");
   %list.add("8", "You can vote to change the level and the game type in the admin menu");
   %list.add("9", "There is a Tournament mode, it has a warmup time, where players can join a team and after that the teams get locked until round ends, all other players can only spectate");
   %list.add("10", "It may be practical to save your loadouts in the Armory, so you can quickly switch to a different gear");
   %list.add("11", "If the game runs slow, try lowering the graphics settings, the higher settings are intended for moderately up to date hardware, normal settings look fine as well");
   %list.add("12", "If you need to reduce graphic quality settings for performance, try disabling SSAO and/or HDR postFX since they use up the most performance");
   %list.add("13", "In games with many players and/or long playtime the decals can add up and remove performance a lot, try finding a good decal liftime setting then");
   %list.add("14", "V-Sync limits the framerate to 60fps, to save power on your GPU, so only activate if you have significantly more FPS ingame, otherwise it may reduce performance");
   %list.add("15", "You can re-bind most controls to other keys");
   %list.add("16", "F1 shows you the current FPS");
   %list.add("17", "You can press 'alt h' to hide the HUD, for example to make screenshots without a HUD");
   %list.add("18", "Pressing 'esc' in the main menu while you have the 3D realtime background enabled will hide the main menu, so you have some kind of screensaver or meditative ambience background");
   %list.add("19", "The demo recoding feature lets you record your games, press 'alt F8' to toggle demo recording, later you can view demos under 'Extras' and then 'Recordings' ");
   %list.add("20", "Under 'Extras' and then 'Credits' you can view the authors, sources and licenses for all assets used in the game");
   %list.add("21", "Under 'Extras' and then 'Dev-Tools' will open another Menu with buttons to several developer related features");
   %list.add("22", "For error/bug reports always provide the console.log file in your game directory, it often has valuable information about what happened");
   %list.add("23", "In case you do not know any further how something works, just read the manual found under 'Help' menu point, almost everything is explained there");
   %list.add("24", "Pressing 'Editor' will open the editor obviously, when you press it from main menu, it will give you a list of templates to chose from, but when you hosted a game the editor will open directly");
   %list.add("25", "Try out the amazing ingame Editor, it easily lets you build your own levels, the workflow is not that end-user friendly yet, but feel free to ask for help if you have problems");
   %list.add("26", "The meaning behind 'Uebergame' is that it is a framework that contains many under games, since 'ueber' is german and means 'above' or 'over'");
   %list.add("27", "'Uebergame' and 'Übergame' are both spelled correctly 'ue' is the same as 'ü' and it is used for people or language who do not have so called 'Umlauts'");
   %list.add("28", "When hosting a game, check out the 'Server Options' there are lots of things you can change to host your own custom matches");
   %list.add("29", "The game runs sluggish even though you have a powerful PC? Then your ping may be too high, this is when you live too far away from the location the server is in, this is a physical limitation and not the game's fault");
   %list.add("30", "You can create server sided mods with Uebergame edit files in scripts/server and when you host a server, the changes will be in effect for all players, no need to download any files for the others, take care to not break the game though");
   %list.add("31", "Right click will aim your weapon, you will shoot more accurate in that mode, you get some zoom, but you cannot sprint, reload, jump etc it also takes a while dependend on weapon until you are ready to fire again");
   %list.add("32", "Pressing 'ctrl k' and you will commit suicide, helpful if you are stuck or fallen into the void alternatively there is also a button in the Armory dialog for it ");
   %list.add("33", "The editor allows you to edit levels in realtime, even when people are playing on it on your server, but be careful, changing some things may crash the game, but most features are relatively safe to use.");
   %list.add("34", "If you are bad at the game, you should train more, practice makes a master");
   %list.add("35", "If you can beat the Bots then you qualify as skilled");
   %list.add("36", "Uebergame is primarily a multiplayer game, 'multi' comes from latin and means 'many', so it means it is a game played by many, so invite some friends for the best experience");
   %list.add("37", "Game experience my change during online play");
   %list.add("38", "Gameplay experience depends primarily on the people you play with");
   %list.add("39", "Game will need players to work");
   %list.add("40", "If you are out of ammo in battle, sometimes it may be quicker to switch to the pistol and finishing of the enemy before reloading");
   %list.add("41", "Try out the different kind of grenades you can chose in the armory");
   %list.add("42", "Try out the different kind special abilities in the armory 'Munitions' lets you drop ammo packs and 'Medical' lets you drop medpacks. Those abilities are meant to be some kind of class system, but there are only two yet");
   %list.add("43", "Game will need players to work");
   %list.add("44", "No hipsters were used in the production of Uebergame");
   %list.add("45", "Anything is possible in Uebergame, the only limit is yourself");
   %list.add("46", "Anything is possible in Uebergame, the only limit is your imagination");
   
   %random = getRandom(0, 46);
   %text = %list.getValue(%random);
   
   return (%text);
}