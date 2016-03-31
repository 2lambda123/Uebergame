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
// Global Non-remapable binds
//-----------------------------------------------------------------------------
GlobalActionMap.bindCmd(keyboard, "F1", "", "contextHelp();");
//GlobalActionMap.bindCmd(keyboard, "escape", "", "handleEscape();");
GlobalActionMap.bind(keyboard, "F9", toggleConsole);
GlobalActionMap.bindCmd(keyboard, "alt k", "cls();","");
GlobalActionMap.bindCmd(keyboard, "alt enter", "", "Canvas.attemptFullscreenToggle();");

//GlobalActionMap.bind(keyboard, "F5", doProfile); // Debug mode only
GlobalActionMap.bind(keyboard, "F4", showMetrics);
GlobalActionMap.bind(keyboard, "ctrl F4", showUeberMetrics);
GlobalActionMap.bind( keyboard, "alt F6", startRecordingDemo );
GlobalActionMap.bind( keyboard, "alt F7", stopRecordingDemo );
//GlobalActionMap.bind( keyboard, "F9", toggleDemoRecording );
GlobalActionMap.bind( keyboard, "F5", doScreenShot );
GlobalActionMap.bind( keyboard, "alt F5", doScreenShotHudless);
//GlobalActionMap.bind( keyboard, "F12", recordMovie );

//------------------------------------------------------------------------------
// Utility remap functions:
//------------------------------------------------------------------------------

function ActionMap::copyBind( %this, %otherMap, %command )
{
   if ( !isObject( %otherMap ) )
   {
      error( "ActionMap::copyBind - \"" @ %otherMap @ "\" is not an object!" );
      return;
   }

   %bind = %otherMap.getBinding( %command );
   if ( %bind !$= "" )
   {
      %device = getField( %bind, 0 );
      %action = getField( %bind, 1 );
      %flags = %otherMap.isInverted( %device, %action ) ? "SDI" : "SD";
      %deadZone = %otherMap.getDeadZone( %device, %action );
      %scale = %otherMap.getScale( %device, %action );
      %this.bind( %device, %action, %flags, %deadZone, %scale, %command );
   }
}

//------------------------------------------------------------------------------
function ActionMap::blockBind( %this, %otherMap, %command )
{
   if ( !isObject( %otherMap ) )
   {
      error( "ActionMap::blockBind - \"" @ %otherMap @ "\" is not an object!" );
      return;
   }

   %bind = %otherMap.getBinding( %command );
   if ( %bind !$= "" )
      %this.bind( getField( %bind, 0 ), getField( %bind, 1 ), "" );
}

//------------------------------------------------------------------------------
function ActionMap::clearBind(%this, %command)
{
   if ( !isObject( %this ) )
   {
      error( "ActionMap::clearBind - \"" @ %this @ "\" is not an object!" );
      return;
   }

   %fullMapString = %this.getBinding( %command );
   if(%fullMapString $= "")
      return;

   %mapCount = getRecordCount( %fullMapString );
   for ( %i = 0; %i < %mapCount; %i++ )
   {
      %temp = getRecord( %fullMapString, %i );
      %this.unbind( getField( %temp, 0 ), getField( %temp, 1 ) );
   }
}

//------------------------------------------------------------------------------
// Command functions
//------------------------------------------------------------------------------

function escapeFromGame()
{
   if ( $Server::ServerType $= "SinglePlayer" )
      //MessageBoxYesNo( "Exit", "Exit from this Mission?", "disconnect();", "");
	  Canvas.pushDialog(exitGameGui);
   else
      //MessageBoxYesNo( "Disconnect", "Disconnect from the server?", "disconnect();", "");
	  Canvas.pushDialog(exitGameGui);
}

// Opens a Gui ingame that displays all metrics in one window
// #issue: still needs function to be toggled off when pressing the key again
// currently the gui does close when pressing esc
function showMetrics(%val)
{
   if(%val)
      metrics("fps");
}

function showUeberMetrics(%val)
{
   if(%val)
      metrics("fps gfx shadow sfx terrain groundcover forest net");
}

// Only if debug build
function doProfile(%val)
{
   if (%val)
   {
      // key down -- start profile
      echo("Starting profile session...");
      profilerReset();
      profilerEnable(true);
   }
   else
   {
      // key up -- finish off profile
      echo("Ending profile session...");

      profilerDumpToFile("profilerDumpToFile" @ getSimTime() @ ".txt");
      profilerEnable(false);
   }
}

//------------------------------------------------------------------------------
// GUI Hud binds
//------------------------------------------------------------------------------

function showPlayerList(%val)
{
   if(%val)
      AdminDlg.toggle(%val);
}

function showScoreBoard(%val)
{
   if ( ( $Client::TeamCount > 1 ) && (%val) )
     clientCmdToggleScoreHud(%val);

   else if(%val)
     ScoreHudFFA.toggle(%val); 
}

function toggleArmoryDlg( %val )
{
   if ( %val )
      clientCmdToggleArmory( %val );
}

function toggleVehicleHud( %val )
{
   if ( %val )
   {
      if( VehicleHud.isAwake() )
         Canvas.popDialog( VehicleHud );
      else
         Canvas.pushDialog( VehicleHud );
   }
}

function hideHUDs(%val)
{
   if (%val)
      HudlessPlayGui.toggle();
}

function doScreenShotHudless(%val)
{
   if(%val)
   {
      canvas.setContent(HudlessPlayGui);
      //doScreenshot(%val);
      schedule(10, 0, "doScreenShot", %val);
   }
   else
      canvas.setContent(PlayGui);
}

//------------------------------------------------------------------------------
// Movement Keys
//------------------------------------------------------------------------------

$movementSpeed = 1; // m/s

function setSpeed(%speed)
{
   if(%speed)
      $movementSpeed = %speed;
}

function moveleft(%val)
{
   $mvLeftAction = %val * $movementSpeed;
}

function moveright(%val)
{
   $mvRightAction = %val * $movementSpeed;
}

function moveforward(%val)
{
   $mvForwardAction = %val * $movementSpeed;
}

function movebackward(%val)
{
   $mvBackwardAction = %val * $movementSpeed;
}

function moveup(%val)
{
   %object = ServerConnection.getControlObject();
   
   if(%object.isInNamespaceHierarchy("Camera"))
      $mvUpAction = %val * $movementSpeed;
}

function movedown(%val)
{
   %object = ServerConnection.getControlObject();
   
   if(%object.isInNamespaceHierarchy("Camera"))
      $mvDownAction = %val * $movementSpeed;
}

function turnLeft( %val )
{
   $mvYawRightSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function turnRight( %val )
{
   $mvYawLeftSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function panUp( %val )
{
   $mvPitchDownSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function panDown( %val )
{
   $mvPitchUpSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function getMouseAdjustAmount(%val)
{
   // based on a default camera FOV of 90'
   return(%val * ($cameraFov / 90) * 0.01) * $pref::Input::LinkMouseSensitivity;
}

function getGamepadAdjustAmount(%val)
{
   // based on a default camera FOV of 90'
   return(%val * ($cameraFov / 90) * 0.01) * 10.0;
}

function yaw(%val)
{
   %yawAdj = getMouseAdjustAmount(%val);
   if(ServerConnection.isControlObjectRotDampedCamera())
   {
      // Clamp and scale
      %yawAdj = mClamp(%yawAdj, -m2Pi()+0.01, m2Pi()-0.01);
      %yawAdj *= 0.5;
   }

   $mvYaw += %yawAdj;
}

function pitch(%val)
{
   %pitchAdj = getMouseAdjustAmount(%val);
   if(ServerConnection.isControlObjectRotDampedCamera())
   {
      // Clamp and scale
      %pitchAdj = mClamp(%pitchAdj, -m2Pi()+0.01, m2Pi()-0.01);
      %pitchAdj *= 0.5;
   }

   $mvPitch += %pitchAdj;
}

function gamePadMoveX( %val )
{
   $mvXAxis_L = %val;
}

function gamePadMoveY( %val )
{
   $mvYAxis_L = %val;
}

function gamepadYaw(%val)
{
   %yawAdj = getGamepadAdjustAmount(%val);
   if(ServerConnection.isControlObjectRotDampedCamera())
   {
      // Clamp and scale
      %yawAdj = mClamp(%yawAdj, -m2Pi()+0.01, m2Pi()-0.01);
      %yawAdj *= 0.5;
   }

   if(%yawAdj > 0)
   {
      $mvYawLeftSpeed = %yawAdj;
      $mvYawRightSpeed = 0;
   }
   else
   {
      $mvYawLeftSpeed = 0;
      $mvYawRightSpeed = -%yawAdj;
   }
}

function gamepadPitch(%val)
{
   %pitchAdj = getGamepadAdjustAmount(%val);
   if(ServerConnection.isControlObjectRotDampedCamera())
   {
      // Clamp and scale
      %pitchAdj = mClamp(%pitchAdj, -m2Pi()+0.01, m2Pi()-0.01);
      %pitchAdj *= 0.5;
   }

   if(%pitchAdj > 0)
   {
      $mvPitchDownSpeed = %pitchAdj;
      $mvPitchUpSpeed = 0;
   }
   else
   {
      $mvPitchDownSpeed = 0;
      $mvPitchUpSpeed = -%pitchAdj;
   }
}

//------------------------------------------------------------------------------
// Triggers
//------------------------------------------------------------------------------

function mouseFire(%val)
{
   $mvTriggerCount0 += (($mvTriggerCount0 & 1) == %val) ? 2 : 1;
}

function mouseJet(%val)
{
   $mvTriggerCount1 += (($mvTriggerCount1 & 1) == %val) ? 2 : 1;
}

function jump(%val)
{
   $mvTriggerCount2 += (($mvTriggerCount2 & 1) == %val) ? 2 : 1;
}

function doCrouch(%val)
{
   $mvTriggerCount3 += (($mvTriggerCount3 & 1) == %val) ? 2 : 1;
}

function doProne(%val)
{
   $mvTriggerCount4 += (($mvTriggerCount4 & 1) == %val) ? 2 : 1;
}

function doSprint(%val)
{
   $mvTriggerCount5 += (($mvTriggerCount5 & 1) == %val) ? 2 : 1;
}

function throwGrenade( %val )
{
   commandToServer( 'ThrowGrenade', %val );

   //$mvTriggerCount6 += (($mvTriggerCount6 & 1) == %val) ? 2 : 1;
}

function throwFlag( %val )
{
   commandToServer( 'ThrowFlag', %val );
   //$mvTriggerCount7 += (($mvTriggerCount7 & 1) == %val) ? 2 : 1;
}
//------------------------------------------------------------------------------
// Gamepad Trigger
//------------------------------------------------------------------------------

function gamepadFire(%val)
{
   if(%val > 0.1 && !$gamepadFireTriggered)
   {
      $gamepadFireTriggered = true;
      $mvTriggerCount0++;
   }
   else if(%val <= 0.1 && $gamepadFireTriggered)
   {
      $gamepadFireTriggered = false;
      $mvTriggerCount0++;
   }
}

function gamepadAltTrigger(%val)
{
   if(%val > 0.1 && !$gamepadAltTriggerTriggered)
   {
      $gamepadAltTriggerTriggered = true;
      $mvTriggerCount1++;
   }
   else if(%val <= 0.1 && $gamepadAltTriggerTriggered)
   {
      $gamepadAltTriggerTriggered = false;
      $mvTriggerCount1++;
   }
}

//------------------------------------------------------------------------------
// Zoom and FOV functions
//------------------------------------------------------------------------------

if($Player::CurrentFOV $= "")
   $Player::CurrentFOV = $pref::Player::DefaultFOV / 2;

// toggleZoomFOV() works by dividing the CurrentFOV by 2.  Each time that this
// toggle is hit it simply divides the CurrentFOV by 2 once again.  If the
// FOV is reduced below a certain threshold then it resets to equal half of the
// DefaultFOV value.  This gives us 4 zoom levels to cycle through.

function toggleZoomFOV(%val)
{
   if ( %val )
   {
      $Player::CurrentFOV = $Player::CurrentFOV / 2;

      if($Player::CurrentFOV < 5)
         resetCurrentFOV();

      if(ServerConnection.zoomed)
         setFOV($Player::CurrentFOV);
      else
      {
         setFov($pref::Player::Fov);
      }
   }
}

function resetCurrentFOV()
{
   $Player::CurrentFOV = ($pref::Player::Fov) / 2;
}

function turnOffZoom()
{
   ServerConnection.zoomed = false;
   setFov($pref::Player::Fov);

   // Rather than just disable the DOF effect, we want to set it to the level's
   // preset values.
   //DOFPostEffect.disable();
   ppOptionsUpdateDOFSettings();
   reticle.setVisible(true);
   zoomReticle.setVisible(false);
}

function toggleZoom(%val)
{
   if ( Canvas.getContent() != PlayGui.getId() )
      return;

   if ( %val )
   {
      ServerConnection.zoomed = true;
      setFov($Player::CurrentFOV);

      //zoomReticle.setBitmap( "art/gui/weaponHud/zoomHair12.png" );
      zoomReticle.setBitmap( "art/gui/weaponHud/bino.png" );
      reticle.setVisible(false);
      zoomReticle.setVisible(true);

      DOFPostEffect.setAutoFocus( true );
      DOFPostEffect.setFocusParams( 0.5, 0.5, 50, 500, -5, 5 );
      DOFPostEffect.enable();
   }
   else
   {
      turnOffZoom();
   }
}

//------------------------------------------------------------------------------
// Iron sights functions
// Scripted by: ZOD, idea by deepscratch
// http://www.garagegames.com/account/profile/138071
//------------------------------------------------------------------------------

function toggleIronSights( %val )
{
   if ( Canvas.getContent() != PlayGui.getId() )
      return;
  
   if ( %val )
   {
      ServerConnection.zoomed = true;
      setFov($Player::CurrentFOV);
      //DOFPostEffect.setAutoFocus( true );
      //DOFPostEffect.setFocusParams( 0.5, 0.5, 50, 500, -5, 5 );
      //DOFPostEffect.enable();
      commandToServer( 'DoIronSights' );
      reticle.setVisible(false);
   }
   else
   {
      ServerConnection.zoomed = false;
      setFov($pref::Player::Fov);
      //ppOptionsUpdateDOFSettings();
      commandToServer( 'UndoIronSights' );
      reticle.setVisible(true);
      zoomReticle.setVisible(false);
   }
}

//------------------------------------------------------------------------------
// Camera & View functions
//------------------------------------------------------------------------------

function toggleFreeLook( %val )
{
   if ( %val )
      $mvFreeLook = true;
   else
      $mvFreeLook = false;
}

function toggleFirstPerson(%val)
{
   if (%val)
   {
      ServerConnection.setFirstPerson(!ServerConnection.isFirstPerson());
   }
}

function toggleCamera(%val)
{
   if (%val)
      commandToServer('ToggleCamera');
}

//------------------------------------------------------------------------------
// Misc. Player stuff
//------------------------------------------------------------------------------

function celebrationWave(%val)
{
   if(%val)
      commandToServer('playCel', "wave");
}

function celebrationSalute(%val)
{
   if(%val)
      commandToServer('playCel', "salute");
}

function doSuicide(%val)
{
   if(%val)
      commandToServer('suicide');
}

//------------------------------------------------------------------------------
// Item manipulation
//------------------------------------------------------------------------------

function triggerSpecial(%val)
{
   commandToServer( 'UseSpecial', %val );
}

function tossSpecial( %val )
{
   if ( %val )
      commandToServer( 'Throw', "Special" );
}

function tossGrenade( %val )
{
   if ( %val )
      commandToServer( 'Throw', "Grenade" );
}

function tossAmmo( %val )
{
   if ( %val )
      commandToServer( 'Throw', "Ammo" );
}

//------------------------------------------------------------------------------
// Message HUD functions
//------------------------------------------------------------------------------

function pageMessageHudUp( %val )
{
   if ( %val )
      pageUpMessageHud();
}

function pageMessageHudDown( %val )
{
   if ( %val )
      pageDownMessageHud();
}

function resizeMessageHud( %val )
{
   if ( %val )
      cycleMessageHudSize();
}

//------------------------------------------------------------------------------
// Weapon functions
//------------------------------------------------------------------------------
/*
function melee( %val )
{
   if ( %val )
      commandToServer( 'DoMelee' );
}
*/
function reloadWeapon( %val )
{
   if ( %val )
      commandToServer( 'reloadWeapon' );
}

function throwWeapon( %val )
{
   if ( %val )
      commandToServer( 'Throw', "Weapon" );
}

function cycleWeaponAxis( %val )
{
   if ( %val < 0 )
      commandToServer( 'cycleWeapon', "next" );
   else
      commandToServer( 'cycleWeapon', "prev" );
}

function nextWeapon( %val )
{
   if ( %val )
      commandToServer( 'cycleWeapon', "next" );
}

function prevWeapon( %val )
{
   if ( %val )
      commandToServer( 'cycleWeapon', "prev" );
}

function cycleNextWeaponOnly( %val )
{
   if ( %val < 0 )
      commandToServer( 'cycleWeapon', "next" );
}

function useFirstWeaponSlot( %val )
{
   if ( %val )
      commandToServer( 'selectWeaponSlot', 0 );
}

function useSecondWeaponSlot( %val )
{
   if ( %val )
      commandToServer( 'selectWeaponSlot', 1 );
}

function useThirdWeaponSlot( %val )
{
   if ( %val )
      commandToServer( 'selectWeaponSlot', 2 );
}

function useFourthWeaponSlot( %val )
{
   if ( %val )
      commandToServer( 'selectWeaponSlot', 3 );
}

function useFifthWeaponSlot( %val )
{
   if ( %val )
      commandToServer( 'selectWeaponSlot', 4 );
}

function useSixthWeaponSlot( %val )
{
   if ( %val )
      commandToServer( 'selectWeaponSlot', 5 );
}

function useSeventhWeaponSlot( %val )
{
   if ( %val )
      commandToServer( 'selectWeaponSlot', 6 );
}

function useEighthWeaponSlot( %val )
{
   if ( %val )
      commandToServer( 'selectWeaponSlot', 7 );
}

//-----------------------------------------------------------------------------
// Vehicle related

function useVehicleWeaponOne(%val)
{
   if(%val)
      commandToServer('setVehicleWeapon', 1);
}

function useVehicleWeaponTwo(%val)
{
   if(%val)
      commandToServer('setVehicleWeapon', 2);
}

function useVehicleWeaponThree(%val)
{
   if(%val)
      commandToServer('setVehicleWeapon', 3);
}

function nextVehicleWeapon(%val)
{
   if ( %val )
      commandToServer('switchVehicleWeapon', "next");
}

function prevVehicleWeapon(%val)
{
   if ( %val )
      commandToServer('switchVehicleWeapon', "prev");
}

function cycleVehicleWeapon( %val )
{
   if ( %val < 0 )
      commandToServer( 'switchVehicleWeapon', "next" );
   else
      commandToServer( 'switchVehicleWeapon', "prev" );
}

function cycleNextVehicleWeaponOnly( %val )
{
   if ( %val < 0 )
      commandToServer( 'switchVehicleWeapon', "next" );
}

//------------------------------------------------------------------------------
// Demo recording functions
//------------------------------------------------------------------------------

function toggleDemoRecording(%val)
{
   if ( %val )
   {
      if(ServerConnection.isDemoRecording())
      {
         stopDemoRecord();
      }
      else
      {
         startDemoRecord();
      }
   }
}

function startRecordingDemo( %val )
{
   if ( %val )
      startDemoRecord();
}

function stopRecordingDemo( %val )
{
   if ( %val )
      stopDemoRecord();
}

function recordMovie(%val)
{
   if ( %val )
   {
      if( !$MovieEncodeActive )
      {
         makeMovie();
      }
      else
      {
         stopMovie();
      }
   }
}

//------------------------------------------------------------------------------
// Helper Functions
//------------------------------------------------------------------------------

function dropCameraAtPlayer(%val)
{
   if (%val)
      commandToServer('dropCameraAtPlayer');
}

function dropPlayerAtCamera(%val)
{
   if (%val)
      commandToServer('DropPlayerAtCamera');
}

function bringUpOptions(%val)
{
   if (%val)
      Canvas.pushDialog(OptionsDlg);
}

//------------------------------------------------------------------------------
// VOTING FUNCTIONS:
//------------------------------------------------------------------------------

function voteYes(%val)
{
   if(%val)
      setPlayerVote(true);
}

function voteNo(%val)
{
   if(%val)
      setPlayerVote(false);
}

//------------------------------------------------------------------------------
// Inventory functions
//------------------------------------------------------------------------------

function cycleLoadoutNext(%val) { if(%val) CycleLoadout(1); }
function cycleLoadoutPrev(%val) { if(%val) CycleLoadout(2); }

function cycleLoadout(%val)
{
   switch$($pref::Player::SelectedLoadout)
   {
      case 0: if(%val == 1) %next = 1; else %next = 9;
      case 1: if(%val == 1) %next = 2; else %next = 0;
      case 2: if(%val == 1) %next = 3; else %next = 1;
      case 3: if(%val == 1) %next = 4; else %next = 2;
      case 4: if(%val == 1) %next = 5; else %next = 3;
      case 5: if(%val == 1) %next = 6; else %next = 4;
      case 6: if(%val == 1) %next = 7; else %next = 5;
      case 7: if(%val == 1) %next = 8; else %next = 6;
      case 8: if(%val == 1) %next = 9; else %next = 0;
      case 9: if(%val == 1) %next = 0; else %next = 8;
      default: %next = 0;
   }
   loadLoadout(%next, 1);
}

//------------------------------------------------------------------------------
// Misc.
//------------------------------------------------------------------------------

function autoMountVehicle(%val)
{
   if ( %val )
      commandToServer( 'MountVehicle' );
}

function createVehicle(%val)
{
   if ( %val )
      commandToServer( 'CreateVehicle', $pref::Player::SelectedVehicle );
}


//------------------------------------------------------------------------------
// Default actionmap
//------------------------------------------------------------------------------

if ( isObject( moveMap ) )
   moveMap.delete();

new ActionMap(moveMap);

//-----------------------------------------------------------------------------
// Movment
moveMap.bindCmd(keyboard, "escape", "", "escapeFromGame();");

moveMap.bind( mouse, xaxis, yaw );
moveMap.bind( mouse, yaxis, pitch );
moveMap.bind( keyboard, "w", moveforward );
moveMap.bind( keyboard, "a", moveleft );
moveMap.bind( keyboard, "s", movebackward );
moveMap.bind( keyboard, "d", moveright );
moveMap.bind( keyboard, "up", moveup );
moveMap.bind( keyboard, "down", movedown );
moveMap.bind( keyboard, "left", turnLeft );
moveMap.bind( keyboard, "right", turnRight );
moveMap.bind( keyboard, "home", panUp );
moveMap.bind( keyboard, "end", panDown );
moveMap.bind( keyboard, "space", jump );
moveMap.bind( keyboard, "lcontrol", doCrouch );
moveMap.bind( keyboard, "x", doProne );
moveMap.bind( keyboard, "lshift", doSprint );
//moveMap.bind( keyboard, "e", mouseJet ); //no jetpacking yet
//-----------------------------------------------------------------------------
// Weapons
moveMap.bind( mouse, button0, mouseFire );
//moveMap.bind( keyboard, "q", melee ); //melee not working correctly yet
moveMap.bind( keyboard, "r", reloadWeapon );
//moveMap.bindCmd(keyboard, "e", "commandToServer('PickupFacing');", "");
moveMap.bind( mouse, zaxis, cycleWeaponAxis );
moveMap.bind( keyboard, "+", nextWeapon );
moveMap.bind( keyboard, "minus", prevWeapon );
moveMap.bind( keyboard, "1", useFirstWeaponSlot );
moveMap.bind( keyboard, "2", useSecondWeaponSlot );
moveMap.bind( keyboard, "3", useThirdWeaponSlot );
moveMap.bind( keyboard, "4", useFourthWeaponSlot );
moveMap.bind( keyboard, "5", useFifthWeaponSlot );
moveMap.bind( keyboard, "6", useSixthWeaponSlot );
moveMap.bind( keyboard, "7", useSeventhWeaponSlot );
moveMap.bind( keyboard, "8", useEighthWeaponSlot );
moveMap.bind( keyboard, "alt w", throwWeapon );
moveMap.bind( keyboard, "alt a", tossAmmo );

moveMap.bind( keyboard, "b", triggerSpecial );
moveMap.bind( keyboard, "ctrl b", tossSpecial );
moveMap.bind( keyboard, "g", throwGrenade );
moveMap.bind( keyboard, "alt g", tossGrenade );

//-----------------------------------------------------------------------------
// Vehicle
//moveMap.bind( keyboard, "x", createVehicle );
moveMap.bind( keyboard, "m", autoMountVehicle );

//-----------------------------------------------------------------------------
// Camera and View
//moveMap.bind( mouse, button1, toggleZoom );
moveMap.bind( mouse, button1, toggleIronSights );
moveMap.bind( keyboard, "z", toggleZoomFOV );
moveMap.bind( keyboard, "v", toggleFreeLook );
moveMap.bind( keyboard, "F3", toggleFirstPerson );
//moveMap.bind( keyboard,"F6", toggleCamera );
//moveMap.bind( keyboard, "F8", dropCameraAtPlayer );
//moveMap.bind( keyboard, "F7", dropPlayerAtCamera );

//-----------------------------------------------------------------------------
// Misc
//moveMap.bind( keyboard, "ctrl w", celebrationWave );
//moveMap.bind( keyboard, "ctrl s", celebrationSalute );
moveMap.bind( keyboard, "ctrl k", doSuicide );
moveMap.bind( keyboard, "ctrl f", throwFlag );

//-----------------------------------------------------------------------------
// Huds
moveMap.bind( keyboard, "F2", showPlayerList );
moveMap.bind( keyboard, "tab", showScoreBoard );
moveMap.bind( keyboard, "u", toggleMessageHud );
moveMap.bind( keyboard, "y", teamMessageHud );
moveMap.bind( keyboard, "o", fireTeamMessageHud );
moveMap.bind( keyboard, "pageUp", pageMessageHudUp );
moveMap.bind( keyboard, "pageDown", pageMessageHudDown );
moveMap.bind( keyboard, "p", resizeMessageHud );
moveMap.bind( keyboard, "c", toggleQuickChatHud );
moveMap.bind( keyboard, "F8", bringUpOptions );
moveMap.bind( keyboard, "insert", voteYes );
moveMap.bind( keyboard, "delete", voteNo );
moveMap.bind( keyboard, "i", toggleArmoryDlg );
moveMap.bind( keyboard, "=", cycleLoadoutNext );
moveMap.bind( keyboard, "-", cycleLoadoutPrev );
moveMap.bind( keyboard, "ctrl n", toggleNetGraph );
//moveMap.bind( keyboard, "/", toggleVehicleHud ); //not in use yet
//moveMap.bind( keyboard, "F8", toggleOverheadMap ); //???
moveMap.bind( keyboard, "F6", toggleMusicPlayer );
moveMap.bind(keyboard, "alt h", hideHUDs);

moveMap.bind( gamepad, thumbrx, "D", "-0.23 0.23", gamepadYaw );
moveMap.bind( gamepad, thumbry, "D", "-0.23 0.23", gamepadPitch );
moveMap.bind( gamepad, thumblx, "D", "-0.23 0.23", gamePadMoveX );
moveMap.bind( gamepad, thumbly, "D", "-0.23 0.23", gamePadMoveY );

moveMap.bind( gamepad, btn_a, jump );
moveMap.bind( gamepad, btn_back, showPlayerList );

moveMap.bind( gamepad, dpadl, nextWeapon);
moveMap.bind( gamepad, dpadu, prevWeapon);
moveMap.bindCmd( gamepad, dpadd, "commandToServer('reloadWeapon');", "");
moveMap.bind( gamepad, dpadr, doSprint);

moveMap.bind( gamepad, triggerr, gamepadFire );
moveMap.bind( gamepad, triggerl, gamepadAltTrigger );
moveMap.bind( gamepad, btn_b, toggleZoomFOV );
moveMap.bind( gamepad, btn_back, toggleCamera );

//------------------------------------------------------------------------------
// Spectator actionmap
//------------------------------------------------------------------------------

if(isObject(spectatorMap))
   spectator.delete();

new ActionMap(spectatorMap);
spectatorMap.bindCmd(keyboard, "escape", "", "escapeFromGame();");
spectatorMap.copyBind( moveMap, moveup );
spectatorMap.copyBind( moveMap, movedown );
spectatorMap.copyBind( moveMap, jump );
spectatorMap.copyBind( moveMap, mouseFire );
spectatorMap.copyBind( moveMap, mouseJet);

//------------------------------------------------------------------------------
// Vehicle actionmap
//------------------------------------------------------------------------------

// Starting vehicle action map code
if ( isObject( vehicleMap ) )
   vehicleMap.delete();

new ActionMap(vehicleMap);

function brake(%val)
{
   commandToServer('toggleBrakeLights');
   $mvTriggerCount2++;
}

vehicleMap.bindCmd( keyboard, "escape", "", "escapeFromGame();" );
vehicleMap.copyBind( moveMap, moveforward );
vehicleMap.copyBind( moveMap, movebackward );
vehicleMap.copyBind( moveMap, moveleft );
vehicleMap.copyBind( moveMap, moveright );
vehicleMap.copyBind( moveMap, moveup );
vehicleMap.copyBind( moveMap, movedown );
vehicleMap.copyBind( moveMap, mouseJet);
vehicleMap.copyBind( moveMap, yaw );
vehicleMap.copyBind( moveMap, pitch );
vehicleMap.copyBind( moveMap, mouseFire );
vehicleMap.copyBind( moveMap, altTrigger );
vehicleMap.bind(keyboard, space, brake);

vehicleMap.copyBind( moveMap, autoMountVehicle );
vehicleMap.copyBind( moveMap, toggleFirstPerson );
vehicleMap.copyBind( moveMap, toggleFreeLook );

vehicleMap.bind( mouse, zaxis, cycleVehicleWeapon );
vehicleMap.bind(keyboard, "+", nextVehicleWeapon );
vehicleMap.bind(keyboard, "minus", prevVehicleWeapon );
vehicleMap.bind( keyboard, "1", useVehicleWeaponOne );
vehicleMap.bind( keyboard, "2", useVehicleWeaponTwo );
vehicleMap.bind( keyboard, "3", useVehicleWeaponThree );

// The key command for flipping the car
vehicleMap.bindCmd(keyboard, "ctrl x", "commandToServer(\'flipCar\');", "");

// Bind the chat keys as well:
vehicleMap.copyBind( moveMap, toggleMessageHud );
vehicleMap.copyBind( moveMap, teamMessageHud );
vehicleMap.copyBind( moveMap, resizeMessageHud );
vehicleMap.copyBind( moveMap, pageMessageHudUp );
vehicleMap.copyBind( moveMap, pageMessageHudDown );

// Miscellaneous other binds:
vehicleMap.copyBind( moveMap, suicide );
vehicleMap.copyBind( moveMap, voteYes );
vehicleMap.copyBind( moveMap, voteNo );
vehicleMap.copyBind( moveMap, showPlayerList );
vehicleMap.copyBind( moveMap, showScoreBoard );
vehicleMap.copyBind( moveMap, toggleNetGraph );
vehicleMap.copyBind( moveMap, toggleArmoryDlg );
vehicleMap.copyBind( moveMap, bringUpOptions );


//------------------------------------------------------------------------------
// Push the proper key bind map. This is called by clientCmdSetHudMode
//------------------------------------------------------------------------------

function Torque::updateKeyMaps(%this)
{
   echo( "Torque::updateKeyMaps(" SPC $HudMode SPC ")" );

   //pop the action maps...
   if (isObject(moveMap))
      moveMap.pop();

   if (isObject(spectatorBlockMap))
      spectatorBlockMap.pop();

   if (isObject(spectatorMap))
      spectatorMap.pop();

   if (isObject(vehicleMap))
      vehicleMap.pop();

   if (isObject(hudMap))
   {
      hudMap.pop();
      hudMap.delete();
   }

   if(isObject(stationMap))
   {
      stationMap.pop();
      stationMap.delete(); 
   }

   // push the proper map
   switch$($HudMode)
   {
      case "Spectator":
         moveMap.push();
         if(isObject(spectatorBlockMap))
            spectatorBlockMap.delete();

         new ActionMap(spectatorBlockMap);
         //spectatorBlockMap.blockBind( moveMap, jump );
         spectatorBlockMap.blockBind( moveMap, mouseFire );
         spectatorBlockMap.blockBind( moveMap, mouseJet );
         spectatorBlockMap.blockBind( moveMap, toggleQuickChatHud );
         spectatorBlockMap.blockBind( moveMap, toggleZoom );
         spectatorBlockMap.blockBind( moveMap, setZoomFOV );
         spectatorBlockMap.blockBind( moveMap, toggleIronSights );
         spectatorBlockMap.push();
         spectatorMap.push();

         if(spectatorMap.getBinding(mouseFire) $= "")
            spectatorMap.copyBind( moveMap, mouseFire );

      case "Pilot":
         if ( $pref::Vehicle::InvertYAxis )
         {
            %bind = moveMap.getBinding( pitch );
            %device = getField( %bind, 0 );
            %action = getField( %bind, 1 );
            %flags = moveMap.isInverted( %device, %action ) ? "SD" : "SDI";
            %deadZone = moveMap.getDeadZone( %device, %action );
            %scale = moveMap.getScale( %device, %action );
            vehicleMap.bind( %device, %action, %flags, %deadZone, %scale, pitch );
         }
         else
            vehicleMap.copyBind( moveMap, pitch );

         vehicleMap.push();

      case "Passenger":
         moveMap.push();

      default:
         moveMap.push();
   }
}
