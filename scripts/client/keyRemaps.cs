//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// moveMap
$RemapCount = 0;
//-----------------------------------------------------------------------------
// Movment
$RemapName[$RemapCount] = "Forward";
$RemapCmd[$RemapCount] = "moveforward";
$RemapCount++;
$RemapName[$RemapCount] = "Backward";
$RemapCmd[$RemapCount] = "movebackward";
$RemapCount++;
$RemapName[$RemapCount] = "Strafe Left";
$RemapCmd[$RemapCount] = "moveleft";
$RemapCount++;
$RemapName[$RemapCount] = "Strafe Right";
$RemapCmd[$RemapCount] = "moveright";
$RemapCount++;
$RemapName[$RemapCount] = "Turn Left";
$RemapCmd[$RemapCount] = "turnLeft";
$RemapCount++;
$RemapName[$RemapCount] = "Turn Right";
$RemapCmd[$RemapCount] = "turnRight";
$RemapCount++;
$RemapName[$RemapCount] = "Look Up";
$RemapCmd[$RemapCount] = "panUp";
$RemapCount++;
$RemapName[$RemapCount] = "Look Down";
$RemapCmd[$RemapCount] = "panDown";
$RemapCount++;
$RemapName[$RemapCount] = "Jump";
$RemapCmd[$RemapCount] = "jump";
$RemapCount++;
//$RemapName[$RemapCount] = "Thrust / Jump Jets";
//$RemapCmd[$RemapCount] = "mouseJet";
//$RemapCount++;
$RemapName[$RemapCount] = "Sprint";
$RemapCmd[$RemapCount] = "doSprint";
$RemapCount++;
$RemapName[$RemapCount] = "Crouch";
$RemapCmd[$RemapCount] = "doCrouch";
$RemapCount++;
$RemapName[$RemapCount] = "Prone";
$RemapCmd[$RemapCount] = "doProne";
$RemapCount++;

//-----------------------------------------------------------------------------
// Weapons

$RemapName[$RemapCount] = "Fire Weapon";
$RemapCmd[$RemapCount] = "mouseFire";
$RemapCount++;
$RemapName[$RemapCount] = "Melee Attack";
$RemapCmd[$RemapCount] = "melee";
$RemapCount++;
$RemapName[$RemapCount] = "Reload Weapon";
$RemapCmd[$RemapCount] = "reloadWeapon";
$RemapCount++;
$RemapName[$RemapCount] = "Toss Weapon";
$RemapCmd[$RemapCount] = "throwWeapon";
$RemapCount++;
$RemapName[$RemapCount] = "Select Next Weapon";
$RemapCmd[$RemapCount] = "nextWeapon";
$RemapCount++;
$RemapName[$RemapCount] = "Select Previous Weapon";
$RemapCmd[$RemapCount] = "prevWeapon";
$RemapCount++;
$RemapName[$RemapCount] = "Weapon Slot One";
$RemapCmd[$RemapCount] = "useFirstWeaponSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Weapon Slot Two";
$RemapCmd[$RemapCount] = "useSecondWeaponSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Weapon Slot Three";
$RemapCmd[$RemapCount] = "useThirdWeaponSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Weapon Slot Four";
$RemapCmd[$RemapCount] = "useFourthWeaponSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Weapon Slot Five";
$RemapCmd[$RemapCount] = "useFifthWeaponSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Weapon Slot Six";
$RemapCmd[$RemapCount] = "useSixthWeaponSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Weapon Slot Seven";
$RemapCmd[$RemapCount] = "useSeventhWeaponSlot";
$RemapCount++;
$RemapName[$RemapCount] = "Weapon Slot Eight";
$RemapCmd[$RemapCount] = "useEighthWeaponSlot";
$RemapCount++;

//-----------------------------------------------------------------------------
// Ammo

//$RemapName[$RemapCount] = "Toss Ammo";
//$RemapCmd[$RemapCount] = "tossAmmo";
//$RemapCount++;

//-----------------------------------------------------------------------------
// Specials

$RemapName[$RemapCount] = "Use Special";
$RemapCmd[$RemapCount] = "triggerSpecial";
$RemapCount++;
//$RemapName[$RemapCount] = "Toss Special";
//$RemapCmd[$RemapCount] = "tossSpecial";
//$RemapCount++;

//-----------------------------------------------------------------------------
// Grenades

$RemapName[$RemapCount] = "Throw Grenade";
$RemapCmd[$RemapCount] = "throwGrenade";
$RemapCount++;
//$RemapName[$RemapCount] = "Toss Grenade Ammo";
//$RemapCmd[$RemapCount] = "tossGrenade";
//$RemapCount++;

//-----------------------------------------------------------------------------
// Vehicles

$RemapName[$RemapCount] = "Enter Vehicle";
$RemapCmd[$RemapCount] = "autoMountVehicle";
$RemapCount++;
//$RemapName[$RemapCount] = "Spawn Vehicle";
//$RemapCmd[$RemapCount] = "createVehicle";
//$RemapCount++;

//-----------------------------------------------------------------------------
// Inventory

//$RemapName[$RemapCount] = "Choose Next Loadout";
//$RemapCmd[$RemapCount] = "cycleLoadoutNext";
//$RemapCount++;
//$RemapName[$RemapCount] = "Choose Previous Loadout";
//$RemapCmd[$RemapCount] = "cycleLoadoutPrev";
//$RemapCount++;

//-----------------------------------------------------------------------------
// Camera and View

$RemapName[$RemapCount] = "Aim Down Sights";
$RemapCmd[$RemapCount] = "toggleIronSights";
$RemapCount++;
$RemapName[$RemapCount] = "Adjust Zoom";
$RemapCmd[$RemapCount] = "toggleZoomFOV";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle Zoom";
$RemapCmd[$RemapCount] = "toggleZoom";
$RemapCount++;
$RemapName[$RemapCount] = "Free Look";
$RemapCmd[$RemapCount] = "toggleFreeLook";
$RemapCount++;
$RemapName[$RemapCount] = "Switch 1st/3rd person view";
$RemapCmd[$RemapCount] = "toggleFirstPerson";
$RemapCount++;
//$RemapName[$RemapCount] = "Toggle Camera";
//$RemapCmd[$RemapCount] = "toggleCamera";
//$RemapCount++;
//$RemapName[$RemapCount] = "Drop Camera at Player";
//$RemapCmd[$RemapCount] = "dropCameraAtPlayer";
//$RemapCount++;
//$RemapName[$RemapCount] = "Drop Player at Camera";
//$RemapCmd[$RemapCount] = "dropPlayerAtCamera";
//$RemapCount++;

//-----------------------------------------------------------------------------
// Messaging

$RemapName[$RemapCount] = "Quick Chat Menu";
$RemapCmd[$RemapCount] = "toggleQuickChatHud";
$RemapCount++;
$RemapName[$RemapCount] = "Chat to Everyone";
$RemapCmd[$RemapCount] = "toggleMessageHud";
$RemapCount++;
$RemapName[$RemapCount] = "Chat to Team";
$RemapCmd[$RemapCount] = "teamMessageHud";
$RemapCount++;
$RemapName[$RemapCount] = "Message Hud PageUp";
$RemapCmd[$RemapCount] = "pageMessageHudUp";
$RemapCount++;
$RemapName[$RemapCount] = "Message Hud PageDown";
$RemapCmd[$RemapCount] = "pageMessageHudDown";
$RemapCount++;
$RemapName[$RemapCount] = "Resize Message Hud";
$RemapCmd[$RemapCount] = "resizeMessageHud";
$RemapCount++;

//-----------------------------------------------------------------------------
// Huds

$RemapName[$RemapCount] = "Bring up Options Dialog";
$RemapCmd[$RemapCount] = "bringUpOptions";
$RemapCount++;
$RemapName[$RemapCount] = "Show Player list/Admin";
$RemapCmd[$RemapCount] = "showPlayerList";
$RemapCount++;
$RemapName[$RemapCount] = "Show Scores";
$RemapCmd[$RemapCount] = "showScoreBoard";
$RemapCount++;
$RemapName[$RemapCount] = "Show/Hide Armory Screen";
$RemapCmd[$RemapCount] = "toggleArmoryDlg";
$RemapCount++;
$RemapName[$RemapCount] = "Show/Hide Vehicle Screen";
$RemapCmd[$RemapCount] = "toggleVehicleHud";
$RemapCount++;
$RemapName[$RemapCount] = "Vote Yes";
$RemapCmd[$RemapCount] = "voteYes";
$RemapCount++;
$RemapName[$RemapCount] = "Vote No";
$RemapCmd[$RemapCount] = "voteNo";
$RemapCount++;

//-----------------------------------------------------------------------------
// Misc

$RemapName[$RemapCount] = "Commit Suicide";
$RemapCmd[$RemapCount] = "doSuicide";
$RemapCount++;
$RemapName[$RemapCount] = "Animation - Wave";
$RemapCmd[$RemapCount] = "celebrationWave";
$RemapCount++;
$RemapName[$RemapCount] = "Animation - Salute";
$RemapCmd[$RemapCount] = "celebrationSalute";
$RemapCount++;
$RemapName[$RemapCount] = "Toggle Demo Recording";
$RemapCmd[$RemapCount] = "toggleDemoRecording";
$RemapCount++;
$RemapName[$RemapCount] = "Throw Flag";
$RemapCmd[$RemapCount] = "throwFlag";
$RemapCount++;

//-----------------------------------------------------------------------------
// Spectator Map
$SpecRemapCount = 0;
$SpecRemapName[$SpecRemapCount] = "Move Up";
$SpecRemapCmd[$SpecRemapCount] = "moveup";
$SpecRemapCount++;
$SpecRemapName[$SpecRemapCount] = "Move Down";
$SpecRemapCmd[$SpecRemapCount] = "movedown";
$SpecRemapCount++;
$SpecRemapName[$SpecRemapCount] = "Toggle Specerver Mode";
$SpecRemapCmd[$SpecRemapCount] = "jump";
$SpecRemapCount++;
$SpecRemapName[$SpecRemapCount] = "Spawn/Previous";
$SpecRemapCmd[$SpecRemapCount] = "mouseFire";
$SpecRemapCount++;
$SpecRemapName[$SpecRemapCount] = "Cycle Camera/Next";
$SpecRemapCmd[$SpecRemapCount] = "mouseJet";
$SpecRemapCount++;

//-----------------------------------------------------------------------------
// Vehicle Map
$VehRemapCount = 0;
$VehRemapName[$VehRemapCount] = "Forward";
$VehRemapCmd[$VehRemapCount] = "moveforward";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Backward";
$VehRemapCmd[$VehRemapCount] = "movebackward";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Strafe Left";
$VehRemapCmd[$VehRemapCount] = "moveleft";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Strafe Right";
$VehRemapCmd[$VehRemapCount] = "moveright";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Turn Left";
$VehRemapCmd[$VehRemapCount] = "turnLeft";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Turn Right";
$VehRemapCmd[$VehRemapCount] = "turnRight";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Move Up";
$VehRemapCmd[$VehRemapCount] = "moveup";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Move Down";
$VehRemapCmd[$VehRemapCount] = "movedown";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Yaw";
$VehRemapCmd[$VehRemapCount] = "yaw";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Pitch";
$VehRemapCmd[$VehRemapCount] = "pitch";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Exit Vehicle";
$VehRemapCmd[$VehRemapCount] = "autoMountVehicle";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Thrust";
$VehRemapCmd[$VehRemapCount] = "mouseJet";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Free Look";
$VehRemapCmd[$VehRemapCount] = "toggleFreeLook";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Select Next Weapon";
$VehRemapCmd[$VehRemapCount] = "nextVehicleWeapon";
$VehRemapCount++;
$VehRemapName[$VehRemapCount] = "Select Previous Weapon";
$VehRemapCmd[$VehRemapCount] = "prevVehicleWeapon";
$VehRemapCount++;
