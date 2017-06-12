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

addMessageCallback( 'MsgAmmoCnt', SyncAmmoHud );
addMessageCallback( 'MsgSpecialCnt', SyncSpecialHud );
addMessageCallback( 'MsgGrenadeCnt', SyncGrenadeHud );
addMessageCallback( 'MsgSyncClock', SyncSystemClock );
addMessageCallback( 'MsgVehiclesLeft', SetVehicleCount );
addMessageCallback( 'MsgSyncClock', SyncSystemClock );

function SyncAmmoHud(%msgType, %msgString, %weapon, %slot, %ammo, %amountInClips, %a5, %a6)
{
   %name = deTag(%weapon);
   %amount = deTag(%ammo);
   %magCount = deTag(%amountInClips);
   WeaponText.setValue(%name);
/*
   if (!%amount)
      AmmoAmount.setVisible(false);
   else
   {
      AmmoAmount.setVisible(true);
      AmmoAmount.setText(%amount @ "/" @ %magCount);
   }
*/
//make ammo hud always visible, even when out of ammo
AmmoAmount.setVisible(true);
AmmoAmount.setText(%amount @ "/" @ %magCount);

   //AmmoAmount.setValue(%amount);
   //ClipAmount.setValue(%magCount);
}

function SyncSpecialHud(%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6)
{
   %name = deTag(%a1);
   %amount = deTag(%a2);
   SpecialText.setValue(%name);
   SpecialAmount.setValue(%amount);
}

function SyncGrenadeHud(%msgType, %msgString, %a1, %a2, %a3, %a4, %a5, %a6)
{
   %name = deTag(%a1);
   %amount = deTag(%a3);
   GrenadeText.setValue(%name);
   GrenadeAmount.setValue(%amount);
}

//-----------------------------------------------------------------------------
// Taken from the Pausing/Countdown Gui Clock resource by Sebastien Bourgon:
// http://www.garagegames.com/index.php?sec=mg&mod=resource&page=view&qid=4978
//
// GuiClock.setReverseTime(600); Clock counts down from 10 minutes
// GuiClock.getReverseTime(); Get current time
//
// GuiClock.pauseTime(true); Pause Clock
// GuiClock.pauseTime(false); Unpause Clock
//
// GuiClock.setTime(600); Clock counts up from 10 minutes
// GuiClock.getTime(); Get current time
//-----------------------------------------------------------------------------
function SyncSystemClock(%msgType, %msgString, %timeLeft, %reverse, %pause)
{
   if(%reverse)
      HudClock.setReverseTime((%timeLeft * 0.5));
   else
      HudClock.setTime(%timeLeft);

   //if(%pause) // Have to add to engine
   //   HudClock.pauseTime(1);
   //else
   //   HudClock.pauseTime(0);

}

//-----------------------------------------------------------------------------
// Deploy callbacks
//-----------------------------------------------------------------------------
addMessageCallback( 'msgDeploySensorRed', clientDeploySensorRed );
addMessageCallback( 'msgDeploySensorGrn', clientDeploySensorGrn );
addMessageCallback( 'msgDeploySensorOff', clientDeploySensorOff );

function clientDeploySensorRed()
{
   deploySensor.setBitmap( "art/gui/deploy_red.png" );
   //deploySensor.setText("Error");
   deploySensor.setVisible(true);
}

function clientDeploySensorGrn()
{
   deploySensor.setBitmap( "art/gui/deploy_green.png" );
   //deploySensor.setText("Ok");
   deploySensor.setVisible(true);
}

function clientDeploySensorOff()
{
   deploySensor.setVisible(false);
}

//-----------------------------------------------------------------------------
// Vehicle callbacks
//-----------------------------------------------------------------------------
function SetVehicleCount(%msgType, %msgString, %vehicle, %left)
{
   if ( isObject( vehicleHud ) )
      VehicleCount.setText( %left SPC "Remaining" );
}

//-----------------------------------------------------------------------------
// Game callbacks
//-----------------------------------------------------------------------------
addMessageCallback( 'MsgEnterGameInfo', handleGameInfoMessage );

function handleGameInfoMessage(%msgType, %msgString, %map, %game, %gameName, %server)
{
   %mis = deTag(%map);
   %srv = deTag(%server);
   %gameType = deTag(%gameName);
   %game = deTag(%game);
   $Client::ServerName = %srv;
   $Client::MissionName = %mis;
   $Client::MissionTypeName = %gameType;

   // Setup the objective hud
   if ( isObject( objectiveHud ) )
   {
      clearObjHudMSG(); // objectiveHud specific message callback
      objectiveHud.configure(%game);
   }
}

//-----------------------------------------------------------------------------
// Team callbacks
//-----------------------------------------------------------------------------
addMessageCallback( 'MsgTeamList', handleTeamListMessage );

function handleTeamListMessage( %msgType, %msgString, %teamCount, %teamList )
{
   // Save off the team names:
   $Client::TeamCount = %teamCount;
   for ( %i = 0; %i < %teamCount; %i++ )
      $Client::TeamName[%i + 1, 0] = getRecord( deTag(%teamList), %i );
}

//-----------------------------------------------------------------------------
// Vote menu callbacks
//-----------------------------------------------------------------------------
addMessageCallback( 'MsgPlayerPopupItem', handlePlayerPopupMessage );
addMessageCallback( 'MsgVoteItem', handleVoteItemMessage );
addMessageCallback( 'MsgPlayerMuted', handlePlayerMuted );

function handlePlayerPopupMessage(%msgType, %msgString, %key, %voteName, %voteActionMsg, %voteText, %popupEntryId)
{
   if(%key != PlayerPopupMenu.key)
      return;

   %option = stripChars(detag(getTaggedString(%voteText)), "\cp\co\c6\c7\c8\c9");

   //error("handlePlayerPopupMessage(" SPC deTag(%msgType) SPC %msgString SPC %key SPC %voteName SPC %voteActionMsg SPC deTag(%voteText) SPC %popupEntryId SPC ")");
   PlayerPopupMenu.add("     " @ %option, %popupEntryId);
}

function handleVoteItemMessage(%msgType, %msgString, %key, %voteName, %voteText, %sort)
{
   if(%key != ESC_VoteMenu.key)
      return;

   %index = ESC_VoteMenu.rowCount();
   ESC_VoteMenu.addRow(%index, detag(%voteText));
   if(%sort)
   {
      ESC_VoteMenu.sort(0);
      ESC_VoteMenu.sortNumerical(1, false);
   }
   $Client::VoteCmd[%index] = detag(%voteName);
}

function handlePlayerMuted(%msgType, %msgString, %name, %client, %mute)
{
   if(isObject($PlayerList[%client]))
      $PlayerList[%client].chatMuted = %mute;
}

//-----------------------------------------------------------------------------
// Vote hud callbacks
//-----------------------------------------------------------------------------
addMessageCallback( 'clearVoteHud', clearVoteHud );
addMessageCallback( 'closeVoteHud', closeVoteHud );
addMessageCallback( 'addYesVote', addYesVote );
addMessageCallback( 'addNoVote', addNoVote );
addMessageCallback( 'MsgToggleVoteHud', handleToggleVoteHud );
addMessageCallback( 'VoteStarted', initVote );

function initVote(%msgType, %msgString)
{
   if(!$centerPrintActive)
   {
      %yBind = strUpr(getField(moveMap.getBinding(voteYes), 1));
      %nBind = strUpr(getField(moveMap.getBinding(voteNo), 1));
      %message = detag(%msgString) @ "\nPress " @ %yBind @ " to vote YES or " @ %nBind @ " to vote NO.";
      clientCmdCenterPrint(%message, 10, 2);
   }
}

function handleToggleVoteHud(%msgType, %msgString, %numClients, %passPercent, %toggle)
{
   if(Canvas.getContent() != PlayGui.getId())
      return;

   switch(%toggle)
   {
      case 1:
         //alxPlay(VoteInitiatedSound, 0, 0, 0);
         //Canvas.pushDialog(mainVoteHud);
         voteHud.yesCount = 0;
         voteHud.noCount = 0;
         voteHud.totalVotes = 0;
         voteHud.voting = true;
         PlayGui.add(mainVoteHud);

      case 2:
         //Canvas.popDialog(mainVoteHud);
         voteHud.yesCount = 0;
         voteHud.noCount = 0;
         voteHud.totalVotes = 0;
         voteHud.voting = false;
         PlayGui.remove(mainVoteHud);
   }
}

function closeVoteHud(%msgType, %msgString, %a1)
{
   PlayGui.remove(mainVoteHud);
}

function addYesVote(%msgType, %msgString)
{
   voteHud.yesCount++;
   voteHud.totalVotes++;
   if(mainVoteHud.isAwake())
      voteHudYes.setText(voteHud.yesCount);
}

function addNoVote(%msgType, %msgString)
{
   voteHud.noCount++;
   voteHud.totalVotes++;
   if(mainVoteHud.isAwake())
      voteHudNo.setText(voteHud.noCount);
}

function clearVoteHud(%msgType, %msgString)
{
   voteHudYes.setText(0);
   voteHudNo.setText(0);
}
