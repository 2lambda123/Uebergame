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

//----------------------------------------------------------------------------
// Debug commands
//----------------------------------------------------------------------------

function serverCmdNetSimulateLag( %client, %msDelay, %packetLossPercent )
{
   if ( %client.isAdmin )
      %client.setSimulatedNetParams( %packetLossPercent / 100.0, %msDelay );   
}

function serverCmdDoMelee(%client)
{
   %player = %client.player;
   if ( !isObject( %player ) || %player.getState() $= "Dead" || !$Game::Running )
      return;

   if ( %player.getEnergyLevel() <= 15 )
      return;

   // Tiring
   %player.setEnergyLevel( %player.getEnergyLevel() - 15 );
   %player.setActionThread("ProxMine_Fire"); // Rigged DAE model

   // Library.cs
   %scan = %player.doRaycast( 2, $TypeMasks::ShapeBaseObjectType );
   %hitObj = firstWord( %scan );

   if ( %hitObj )
   {
      %type = %hitObj.getClassName();
      if ( %type $= "TerrainBlock" || %type $= "Forest" )
      {
         return;
      }

      %hitObj.damage( %player, %player.getPosition(), 25, $DamageType::Melee );

      if ( %hitObj.isMemberOfClass( "Player" ) )
      {
         %hitObj.repulse( %obj );
      }
   }
}

//----------------------------------------------------------------------------
// Server admin
//----------------------------------------------------------------------------

function serverCmdSAD( %client, %password )
{
   if( %password !$= "" && %password $= $Pref::Server::AdminPassword)
   {
      %client.isAdmin = true;
      %client.isSuperAdmin = true;
      %name = getTaggedString( %client.playerName );
      MessageAll( 'MsgAdminForce', "\c2" @ %name @ " has become Admin by force.", %client );   
   }
}

function serverCmdSADSetPassword(%client, %password)
{
   if(%client.isSuperAdmin)
      $Pref::Server::AdminPassword = %password;
}

//-----------------------------------------------------------------------------
// Misc. server commands avialable to clients
//-----------------------------------------------------------------------------

function serverCmdShowScoreHud(%client)
{
   commandToClient(%client, 'OpenScoreHud');
   updateScoreHudThread(%client);
}

function serverCmdHideScoreHud(%client)
{
   commandToClient(%client, 'CloseScoreHud');
   cancel(%client.scoreHudThread);
   %client.scoreHudThread = "";
}

function updateScoreHudThread(%client, %tag)
{
   Game.updateScoreHud(%client);
   cancel(%client.scoreHudThread);
   %client.scoreHudThread = schedule(1000, %client, "updateScoreHudThread", %client);
}

function serverCmdSuicide(%client)
{
   %player = %client.player;
   if ( !isObject(%player) || %player.getState() $= "Dead" || !$Game::Running )
      return;

   %player.kill($DamageType::Suicide);
}

function serverCmdPlayCel(%client, %anim)
{
   %player = %client.player;
   if( %anim $= "Death1" || %anim $= "Death2" || %anim $= "Death3" || %anim $= "Death4" || %anim $= "Death5" ||
       %anim $= "Death6" || %anim $= "Death7" || %anim $= "Death8" || %anim $= "Death9" || %anim $= "Death10" ||
       %anim $= "Death11" || %anim $= "sitting" || %anim $= "scoutRoot" || %anim $= "look" || %anim $= "lookms" ||
       %anim $= "looknw" || %anim $= "head" || %anim $= "headSide" || %anim $= "standjump" || %anim $= "light_recoil" ||
       %anim $= "" )
      return;

   if ( !isObject( %player ) || %player.getState() $= "Dead" || %player.isMounted() )
      return;

   %player.playCelAnimation(%anim);
}

function serverCmdPlayDeath(%client)
{
   %player = %client.player;
   if ( !isObject(%player) || %player.getState() $= "Dead" )
      return;

   %player.playDeathAnimation();
}

//-----------------------------------------------------------------------------
// Inventory server commands
//-----------------------------------------------------------------------------

function serverCmdUse(%client, %data)
{
   //%player = %client.getControlObject(); // might be a camera
   %player = %client.player;
   if ( !isObject( %player ) || %player.getState() $= "Dead" || !$Game::Running )
      return;

   %player.use( %data );
}

function serverCmdThrow(%client, %data)
{
   %player = %client.player;
   if ( !isObject( %player ) || %player.getState() $= "Dead" || !$Game::Running )
      return;

   switch$ (%data)
   {
      case "Weapon":
         %item = (%player.getMountedImage($WeaponSlot) == 0 ) ? "" : %player.getMountedImage($WeaponSlot).item;
         if ( %item !$="" && $SMS::ShowInInv[%item] )
            %player.throw(%item);

      case "Special":
         %item = (%player.getMountedImage($SpecialSlot) == 0 ) ? "" : %player.getMountedImage($SpecialSlot).item;
         if ( %item !$="" )
            %player.throw(%item);

      case "Ammo":
         %weapon = (%player.getMountedImage($WeaponSlot) == 0 ) ? "" : %player.getMountedImage($WeaponSlot);
         if ( %weapon !$= "" )
         {
            if ( %weapon.ammo !$= "" )
               %player.throw(%weapon.ammo);
         }

      case "Clip":
         %weapon = (%player.getMountedImage($WeaponSlot) == 0 ) ? "" : %player.getMountedImage($WeaponSlot);
         if ( %weapon !$= "" )
         {
            if ( %weapon.clip !$= "" )
               %player.throw(%weapon.clip);
         }

      case "Grenade":
         %item = (%player.getMountedImage($GrenadeSlot) == 0 ) ? "" : %player.getMountedImage($GrenadeSlot).item;
         if ( %item !$="" )
            %player.throw(%item);

      default:
         if( %player.hasInventory( %data.getName() ) )
            %player.throw(%data);
   }
}

function serverCmdSelectWeaponSlot(%client, %data)
{
   //LogEcho("serverCmdSelectWeaponSlot(" SPC %client.nameBase @", "@ %data SPC ")");
   %player = %client.player;
   if ( isObject( %player ) )
   {
      // Little hack to allow pilot to switch vehicles weapons
      if ( %player.isPilot() )
      {
         serverCmdSwitchVehicleWeapon( %client, %data );
         return;
      }

      if ( %data < 0 || %data > %player.weaponSlotCount || %player.weaponSlot[%data] $= "" )// || %this.weaponSlot[%data] $= "Grenade" )
         return;

      %player.use( %player.weaponSlot[%data] );
   }
}

function serverCmdCycleWeapon(%client, %data)
{
   %player = %client.player;
   if ( isObject( %player ) )
   {
      // Little hack to allow pilot to switch vehicles weapons
      if ( %player.isPilot() )
      {
         serverCmdSwitchVehicleWeapon( %client, %data );
         return;
      }

      if ( %player.weaponSlotCount == 0 )
         return;

      %slot = -1;
      if ( %player.getMountedImage($WeaponSlot) != 0 )
      {
         %curWeapon = %player.getMountedImage($WeaponSlot).item;
         %player.prevWeapon = %curWeapon;
         for ( %i = 0; %i < %player.weaponSlotCount; %i++ )
         {
            //error("CURRENT WEAPON: " @ %curWeapon @ " IN SLOT: " @ %player.weaponSlot[%i]);
            if ( %curWeapon $= %player.weaponSlot[%i] )
            {
               %slot = %i;
               break;
            }
         }
      }

      if ( %data $= "prev" )
      {
         // Previous weapon...
         if ( %slot == 0 || %slot == -1 )
         {
            %i = %player.weaponSlotCount - 1;
            %slot = 0;
         }
         else
            %i = %slot - 1;
      }
      else
      {
         // Next weapon...
         if ( %slot == ( %player.weaponSlotCount - 1 ) || %slot == -1 )
         {
            %i = 0;
            %slot = ( %player.weaponSlotCount - 1 );
         }
         else
            %i = %slot + 1;
      }

      %newSlot = -1;
      while ( %i != %slot )
      {
         // Don't cycle to Grenade, we select that specifically
         if ( %player.weaponSlot[%i] !$= "" && %player.hasInventory( %player.weaponSlot[%i] ) && %player.hasAmmo( %player.weaponSlot[%i] ) )
         // Allow switch to empty weapon
         //if ( %player.weaponSlot[%i] !$= "" && %player.hasInventory( %player.weaponSlot[%i] ) && %player.weaponSlot[%i] !$= "Grenade" )
         {
            // player has this weapon and it has ammo or uses energy
            %newSlot = %i;
            break;
         }

         if ( %data $= "prev" )
         {
            if ( %i == 0 )
               %i = %player.weaponSlotCount - 1;
            else
               %i--;
         }
         else
         {
            if ( %i == ( %player.weaponSlotCount - 1 ) )
               %i = 0;
            else
               %i++;
         }
      }

      if ( %newSlot != -1 )
         %player.use( %player.weaponSlot[%newSlot] );
   }
}

function serverCmdReloadWeapon(%client)
{
   %player = %client.getControlObject();
   %image = %player.getMountedImage( $WeaponSlot );
   
   // Don't reload if the weapon's full.
   if ( %player.getInventory(%image.ammo) == %image.ammo.maxInventory )
      return;

   if ( %image > 0 )
   {
      %image.clearAmmoClip( %player, $WeaponSlot );
      //%image.reloadAmmoClip(%player, $WeaponSlot);
   }
}

function serverCmdUseSpecial(%client, %val)
{
   //%player = %client.getControlObject(); // might be a camera
   %player = %client.player;
   if ( !isObject( %player ) || %player.getState() $= "Dead" || !$Game::Running )
      return;

   if ( !%player.isMounted() )
   {
      if ( ( %player.getMountedImage( $SpecialSlot ) != 0 ) )
      {
         %player.setImageTrigger( $SpecialSlot, %val );
      }
      else
      {
         // Ok lets assume we wish to undeploy something we deployed if we have no special mounted
         if ( %val )
            %player.unDeployObject();
      }
   }
}

function serverCmdThrowGrenade(%client, %val)
{
   %player = %client.player;
   if ( !isObject( %player ) || %player.getState() $= "Dead" || !$Game::Running )
      return;

   if ( !%player.isMounted() )
   {
      if ( %player.getMountedImage( $GrenadeSlot ) != 0 )
         %player.setImageTrigger( $GrenadeSlot, %val );
   }
}

function serverCmdThrowFlag(%client, %val)
{
   %player = %client.player;
   if ( !isObject( %player ) || %player.getState() $= "Dead" || !$Game::Running )
      return;

   if ( !%player.isMounted() )
   {
      if ( %player.getMountedImage( $FlagSlot ) != 0 )
      {
         %player.setImageTrigger( $FlagSlot, %val );
         echo( "Throw Flag" );
      }
   }
}

//-----------------------------------------------------------------------------
// Team choose functions
//-----------------------------------------------------------------------------

// Client wants to see his choose team dialog.
function serverCmdPushTeamChooseMenu(%client)
{
   // Hand this off to the current game type
   if ( isObject( Game ) )
      Game.pushChooseTeamMenu( %client );
}

function serverCmdPushSpawnChooseMenu(%client)
{
   // Hand this off to the current game type
   if ( isObject( Game ) )
      Game.pushChooseSpawnMenu( %client );
}

function serverCmdclientChooseSpawn(%client, %option, %value)
{
   // Hand this off to the current game type
   if ( isObject( Game ) )
      Game.clientChooseSpawn( %client, %option, %value );
}

// function uses a waiting period when changing teams to prevent team change 
// exploit to crash servers. Added admin varible so admins can teamchange whoever
// they want.
function serverCmdClientJoinTeam(%client, %team, %admin)
{
   //warn("serverCmdClientJoinTeam(" SPC %client.nameBase @", "@ %team @", "@ %admin.nameBase SPC ")");
   // If the client does not enter a team, uses a team less than -1,
   // more than the number of teams for the gametype or zero, set his team to -1 (switch)
   if ( %team <= 0 || %team > Game.numTeams )
      %team = -1;

   if ( %team == -1 )
      %team = %client.team == 1 ? 2 : 1;

   if ( isObject(Game) && Game.kickClient != %client )
   {
      if ( ( !%admin.isAdmin && $pref::Server::TournamentMode ) &&  ( $Game::Running || $CountdownStarted ) )
         return false;

      if ( %client.team != %team )   
      {
         // Fair teams check.
         if ( !%client.isAdmin )
         {
            %otherTeam = %team == 1 ? 2 : 1;
            if ( !%admin.isAdmin && %team != 0 && ( Game.getTeamClientCount( %team )+1  > Game.getTeamClientCount( %otherTeam ) ) )
            {
               messageClient(%client, 'MsgFairTeams', '\c2Teams will be uneven, please choose another team.');
               return false;
            }
         }
         
         if ( !%client.isWaiting || %admin.isAdmin )
         {
            %client.isWaiting = true;
            %client.waitStart = getSimTime();
            //schedule(15000, %client, eval, "%client.isWaiting = false;");
            %client.schedule(15000, waitTimeout);

            %teamed = %client.team == 0 ? 0 : 1;

            if ( !%teamed )
               clearBottomPrint(%client);
            else
            {
               if ( isObject( %client.player ) )
                  %client.player.kill($DamageType::Default);
            }

            Game.clientJoinTeam( %client, %team, %teamed );
            return true;
         }
         else
         {
      	%wait = mFloor((15000 - (getSimTime() - %client.waitStart)) / 1000);
            messageClient(%client, "", '\c3WAIT MESSAGE:\cr You must wait another %1 seconds', %wait);
         }
      }
      else
         messageClient(%client, "", '\c2You allready on team %1.', Game.getTeamName(%team));
   }
   else
      messageClient(%client, "", '\c2Kicked players cannot change teams!');

   return false;
}

// Admin forces auto assign player to a team.
function serverCmdClientAddToGame(%client, %targetClient)
{
   //LogEcho("\c3serverCmdClientAddToGame(" SPC %client.nameBase SPC %targetClient.nameBase SPC ")");

   if ( %targetClient.team == 0 )
   {
      Game.assignClientTeam(%client, $Game::Running);
      Game.spawnPlayer(%client, false);

      messageClient( %targetClient, 'MsgClient', '\c2%1 has added you to the game.', %client.playerName);
      messageAllExcept( %targetClient, -1, 'MsgClient', '\c2%1 added %2 to the game.', %client.playerName, %targetClient.playerName);

      clearBottomPrint(%targetClient);
      if ( $pref::Server::TournamentMode && !$CountdownStarted )
      {   
         %targetClient.notReady = true;
         centerprint(%targetClient, "\nPress FIRE when ready.", 0, 3);
      }
   }
   else
      messageClient(%client, 'MsgError', '\c2Client is allready assigned to a team.');
}

// Admin forces player to a specific team
function serverCmdChangePlayersTeam(%clientRequesting, %client, %team)
{
   //LogEcho("\c3serverCmdChangePlayersTeam(" SPC %clientRequesting.nameBase SPC %client.nameBase SPC %team SPC ")");
   if ( isObject(Game) && %client != Game.kickClient && %clientRequesting.isAdmin )
   {
      serverCmdClientJoinTeam(%client, %team, %clientRequesting);

      if ( $pref::Server::TournamentMode && !$CountdownStarted )
      {
         %client.notReady = true;
         centerprint(%client, "\nPress FIRE when ready.", 0, 3);
      }

      %aname = %clientRequesting.playerName;
      %name = %client.playerName;
      %multiTeam = (Game.numTeams > 1);
      if ( %multiTeam )
      {
         messageClient( %client, 'MsgClient', '\c2%1 has changed your team.', %aname);
         messageAllExcept( %client, -1, 'MsgClient', '\c2%1 forced %2 to join the %3 team.', %aname, %name, game.getTeamName(%client.team) );
      }
      else
      {
         messageClient( %client, 'MsgClient', '\c2%1 has added you to the game.', %aname);
         messageAllExcept( %client, -1, 'MsgClient', '\c2%1 added %2 to the game.', %aname, %name);
      }
   }
}

// An admin forces or player chooses to become a spectator
function serverCmdForceClientToSpectator(%clientRequesting, %client)
{
   if ( isObject( Game ) )
   {
      if ( %clientRequesting.isAdmin )
      {
         // Always remove the player object
         if ( isObject( %client.player ) )
            %client.player.kill($DamageType::Default);

         Game.forceSpectator(%client, "adminForce");
      }
      else
      {
         if ( Game.kickClient != %client )
         {
            // Always remove the player object
            if ( isObject( %client.player ) )
               %client.player.kill($DamageType::Default);

            Game.forceSpectator( %client, "playerChoose" );
         }
      }
   }
}

// This is called from the staging screen. Players choose a team to join.
function serverCmdClientChooseTeam(%client, %option)
{
   //warn("serverCmdClientChooseTeam(" SPC %client.nameBase @", "@ %option SPC ")");

   // No game object?
   if( !isObject(Game) )
      return;

   // No switching to spectator in single player mode
   //if( Game.class $= SinglePlayerGame && %option <= 0  )
   //   return;

   %joined = false;
   switch( %option )
   {
      case 0:
         if ( %client.team != %option )
            Game.forceSpectator(%client, "playerChoose");

         return;

      case 1: // Server chooses team automatically
         Game.assignClientTeam(%client, $Game::Running);
         Game.spawnPlayer(%client, false);
         %joined = true;	

      default: // Join option sent
         // The client sends the request 1 number higher then the actual team
         if ( %client.team != (%option-1) )
            %joined = serverCmdClientJoinTeam(%client, (%option - 1), %client);
   }

   if ( !%joined )
      return;

   clearBottomPrint(%client);
   if ( $pref::Server::TournamentMode )
   {
      if ( !$Game::Running )
      {
         %client.notReady = true;
         %client.notReadyCount = "";
         %client.camera.getDataBlock().setMode(%client.camera, "pre-game", %client.player);
         %client.setControlObject(%client.camera);
         commandToClient(%client, 'setHudMode', 'Spectator');
         centerprint(%client, "\nPress FIRE when ready.", 0, 3);
      }
      else
      {
         %client.setControlObject(%client.player);
         commandToClient(%client, 'setHudMode', 'Play');
      }
   }
   else
   {
      if ( !$Game::Running )
      {
         %client.notReady = true;
         %client.camera.getDataBlock().setMode(%client.camera, "pre-game", %client.player);
         %client.setControlObject(%client.camera);
         commandToClient(%client, 'setHudMode', 'Spectator');
      }
      else
      {
         %client.setControlObject(%client.player);
         commandToClient(%client, 'setHudMode', 'Play');
      }
   }
}

//-----------------------------------------------------------------------------
// Player list commands
//-----------------------------------------------------------------------------

function serverCmdSendPlayerListUpdate(%client)
{
   %count = ClientGroup.getCount();
   for(%i = 0; %i < %count; %i++)
   {
      %cl = ClientGroup.getObject(%i);
      messageClient(%client, 'MsgPlyrListUpd', "", 
                    %cl,
                    %cl.getPing(),
                    %cl.getPacketLoss());
   }
}

//------------------------------------------------------------------------------
// Iron sights functions
//------------------------------------------------------------------------------

function serverCmdDoIronSights(%client, %val)
{
   %player = %client.player;
   if ( !isObject( %player ) || %player.getState() $= "Dead" )
      return;

   %curWeapon = %player.getMountedImage( $WeaponSlot );
   %image = %curWeapon.ironSight;

   // Unmount image and replace it with iron sighted version
   if ( isObject( %image ) )
   {
      %player.unmountImage( $WeaponSlot );
      %player.mountImage( %image, $WeaponSlot );
      %player.allowJumping(false);
      %player.allowJetJumping(false);
      %player.allowSprinting(false);
      %player.allowSwimming(false);
   }
   else
   {
      commandToClient( %client, 'DoZoomReticle', 1 );
   }
}

function serverCmdUndoIronSights(%client, %val)
{
   %player = %client.player;
   if ( !isObject( %player ) || %player.getState() $= "Dead" )
      return;

   %curWeapon = %player.getMountedImage($WeaponSlot);
   %image = %curWeapon.parentImage;

   // Unmount image and replace it with standard sighted version
   if ( isObject( %image ) )
   {
      %player.unmountImage( $WeaponSlot );
      %player.mountImage( %image, $WeaponSlot );
      %player.allowJumping(true);
      %player.allowJetJumping(true);
      %player.allowSprinting(true);
      %player.allowSwimming(true);
   }
}
