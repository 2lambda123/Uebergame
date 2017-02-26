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

/// Returns true if the current quality settings equal
/// this graphics quality level.
function GraphicsQualityLevel::isCurrent( %this )
{
   // Test each pref to see if the current value
   // equals our stored value.
   
   for ( %i=0; %i < %this.count(); %i++ )
   {
      %pref = %this.getKey( %i );
      %value = %this.getValue( %i );
      
      if ( getVariable( %pref ) !$= %value )
         return false;
   }
   
   return true;
}

/// Applies the graphics quality settings and calls 
/// 'onApply' on itself or its parent group if its 
/// been overloaded.
function GraphicsQualityLevel::apply( %this )
{
   for ( %i=0; %i < %this.count(); %i++ )
   {
      %pref = %this.getKey( %i );
      %value = %this.getValue( %i );
      setVariable( %pref, %value );
   }
   
   // If we have an overloaded onApply method then
   // call it now to finalize the changes.
   if ( %this.isMethod( "onApply" ) )   
      %this.onApply();
   else
   {
      %group = %this.getGroup();      
      if ( isObject( %group ) && %group.isMethod( "onApply" ) )
         %group.onApply( %this );
   }   
}

function GraphicsQualityPopup::init( %this, %qualityGroup )
{
   assert( isObject( %this ) );
   assert( isObject( %qualityGroup ) );
            
   // Clear the existing content first.   
   %this.clear();
    
   // Fill it.
   %select = -1;
   for ( %i=0; %i < %qualityGroup.getCount(); %i++ )
   {
      %level = %qualityGroup.getObject( %i );
      if ( %level.isCurrent() )
         %select = %i;
         
      %this.add( %level.getInternalName(), %i );
   }
   
   // Setup a default selection.
   if ( %select == -1 )
      %this.setText( "Custom" );
   else
      %this.setSelected( %select );      
}

function GraphicsQualityPopup::apply( %this, %qualityGroup, %testNeedApply )
{
   assert( isObject( %this ) );
   assert( isObject( %qualityGroup ) );
   
   %quality = %this.getText();
   
   %index = %this.findText( %quality );
   if ( %index == -1 )
      return false;

   %level = %qualityGroup.getObject( %index );
   if ( isObject( %level ) && !%level.isCurrent() )
   {
      if ( %testNeedApply )
         return true;

      %level.apply();
   }
   
   return false;
}

function OptionsDlg::setPane(%this, %pane)
{
   %this-->OptPlayerPane.setVisible(false);
   %this-->OptAudioPane.setVisible(false);
   %this-->OptGraphicsPane.setVisible(false);
   %this-->OptAdvGraphicsPane.setVisible(false);
   %this-->OptNetworkPane.setVisible(false);
   %this-->OptControlsPane.setVisible(false);
   
   %this.findObjectByInternalName( "Opt" @ %pane @ "Pane", true ).setVisible(true);
   
   // Update the state of the apply button.
   %this._updateApplyState();
}

// New replacment for above
function Opt_Tabs::onTabSelected(%this, %tab)
{
   //echo("Opt_Tabs::onTabSelected(" SPC %this.getName() @", "@ %tab SPC ")");
   OptionsDlg.pane = %tab;
   
   if( %tab $="Controls" )
      OptionsDlg.fillRemapList();
   //update mouse slider values again so it does not get reset
   MouseXSlider.setValue( moveMap.getScale( mouse, xaxis ) );
   MouseYSlider.setValue( moveMap.getScale( mouse, yaxis ) );
}

function OptionsDlg::onWake(%this)
{
   Opt_Tabs.selectPage(Graphics);

   // Player Pane
   OptPlayerNameInput.setValue(getField($pref::Player, 0));
   OP_FovSlider.setValue( $pref::Player::Fov );
   OptPlayerSkinMenu.init();

   // KeyMaps and Mouse Pane

   // Initialize the Control Options controls:
   OptBindListMenu.init();

   MouseXSlider.setValue( moveMap.getScale( mouse, xaxis ) );
   MouseYSlider.setValue( moveMap.getScale( mouse, yaxis ) );
   MouseXText.setValue( moveMap.getScale( mouse, xaxis ) );
   MouseYText.setValue( moveMap.getScale( mouse, yaxis ) );

   MouseZActionMenu.clear();
   MouseZActionMenu.add( "Nothing", 1 );
   MouseZActionMenu.add( "Cycle Weapon", 2 );
   MouseZActionMenu.add( "Next Weapon Only", 3 );
   MouseZActionMenu.add( "Cycle Zoom Level", 4 );

   %bind = moveMap.getCommand( "mouse", "zaxis" );
   %selId = 1;
   switch$ ( %bind )
   {
      case "cycleWeaponAxis":
         %selId = 2;
      case "cycleNextWeaponOnly":
         %selId = 3;
      case "toggleZoomFOV":
         %selId = 4;
   }
   MouseZActionMenu.setSelected( %selId );

   //------------------------------------------

   if ( isFunction("getWebDeployment") && getWebDeployment() )
   {
      // Cannot enable full screen under web deployment
      %this-->OptGraphicsFullscreenToggle.setStateOn( false );
      %this-->OptGraphicsFullscreenToggle.setVisible( false );
   }
   else
   {
      %this-->OptGraphicsFullscreenToggle.setStateOn( Canvas.isFullScreen() );
   }
   %this-->OptGraphicsVSyncToggle.setStateOn( !$pref::Video::disableVerticalSync );

   %this-->Tgl_7.setStateOn( $pref::Decals::enabled );
   
   %this-->Tgl_9.setStateOn( $pref::LightManager::Enable::sunBokeh );
   %this-->Tgl_10.setStateOn( $pref::LightManager::Enable::Vignette );
   %this-->Tgl_11.setStateOn( $pref::LightManager::Enable::HDR );
   %this-->Tgl_12.setStateOn( $pref::LightManager::Enable::SSAO );
   %this-->Tgl_13.setStateOn( $pref::LightManager::Enable::LightRay );
   %this-->Tgl_14.setStateOn( $pref::LightManager::Enable::DOF );
   
   %this-->Tgl_DisplayMaster.setStateOn($pref::Net::DisplayOnMaster);

   OptionsDlg.initResMenu();
   %resSelId = OptionsDlg-->OptGraphicsResolutionMenu.findText( _makePrettyResString( $pref::Video::mode ) );
   if( %resSelId != -1 )
      OptionsDlg-->OptGraphicsResolutionMenu.setSelected( %resSelId );
   
   OptGraphicsDriverMenu.clear();
   
   %buffer = getDisplayDeviceList();
   %count = getFieldCount( %buffer );   
   for(%i = 0; %i < %count; %i++)
      OptGraphicsDriverMenu.add(getField(%buffer, %i), %i);

   %selId = OptGraphicsDriverMenu.findText( getDisplayDeviceInformation() );
   if ( %selId == -1 )
      OptGraphicsDriverMenu.setFirstSelected();
   else
      OptGraphicsDriverMenu.setSelected( %selId );

   // Setup the graphics quality dropdown menus.
   %this-->OptMeshQualityPopup.init( MeshQualityGroup );
   %this-->OptTextureQualityPopup.init( TextureQualityGroup );
   %this-->OptLightingQualityPopup.init( LightingQualityGroup );
   %this-->OptShaderQualityPopup.init( ShaderQualityGroup );
   
   %this-->OptDecalQualityPopup.init( DecalQualityGroup );

   // Setup the anisotropic filtering menu.
   %ansioCtrl = %this-->OptAnisotropicPopup;
   %ansioCtrl.clear();
   %ansioCtrl.add( "Off", 0 );
   %ansioCtrl.add( "4X", 4 );
   %ansioCtrl.add( "8X", 8 );
   %ansioCtrl.add( "16X", 16 );
   %ansioCtrl.setSelected( $pref::Video::defaultAnisotropy, false );
            
   // set up the Refresh Rate menu.
   %refreshMenu = %this-->OptRefreshSelectMenu;
   %refreshMenu.clear();
   // %refreshMenu.add("Auto", 60);
   %refreshMenu.add("60", 60);
   %refreshMenu.add("75", 75);
   %refreshMenu.add("120", 120);
   %refreshMenu.setSelected( getWord( $pref::Video::mode, $WORD::REFRESH ) );
   
   // Audio
   //OptAudioHardwareToggle.setStateOn($pref::SFX::useHardware);
   //OptAudioHardwareToggle.setActive( true );
   
   %this-->OptAudioVolumeMaster.setValue( $pref::SFX::masterVolume );
   %this-->OptAudioVolumeShell.setValue( $pref::SFX::channelVolume[ $GuiAudioType] );
   %this-->OptAudioVolumeSim.setValue( $pref::SFX::channelVolume[ $SimAudioType ] );
   %this-->OptAudioVolumeMusic.setValue( $pref::SFX::channelVolume[ $MusicAudioType ] );
   
   OptAudioProviderList.clear();
   %buffer = sfxGetAvailableDevices();
   %count = getRecordCount( %buffer );   
   for(%i = 0; %i < %count; %i++)
   {
      %record = getRecord(%buffer, %i);
      %provider = getField(%record, 0);
      
      if ( OptAudioProviderList.findText( %provider ) == -1 )
            OptAudioProviderList.add( %provider, %i );
   }
   
   OptAudioProviderList.sort();

   %selId = OptAudioProviderList.findText($pref::SFX::provider);
   if ( %selId == -1 )
      OptAudioProviderList.setFirstSelected();
   else
      OptAudioProviderList.setSelected( %selId );

   OptAudioUpdate();
	   
   // Populate the Anti-aliasing popup.
   %aaMenu = %this-->OptAAQualityPopup;
   %aaMenu.clear();
   %aaMenu.Add( "Off", 0 );
   %aaMenu.Add( "1x", 1 );
   %aaMenu.Add( "2x", 2 );
   %aaMenu.Add( "4x", 4 );
   %aaMenu.setSelected( getWord( $pref::Video::mode, $WORD::AA ) );
   OptMouseSensitivity.value = $pref::Input::LinkMouseSensitivity;
}

function OptionsDlg::onSleep(%this)
{  
         // Player
         OptPlayerNameInput.setField();
         %playerName = OptPlayerNameInput.getValue();
         %skin = OptPlayerSkinMenu.getTextById(OptPlayerSkinMenu.getSelected());
         $pref::Player = %playerName TAB %skin;

	  	 // Mouse
         %xSens = MouseXSlider.getValue();
         %ySens = MouseYSlider.getValue();
         moveMap.bind( mouse, xaxis, "S", %xSens, "yaw" );
         %yFlags = InvertMouseTgl.getValue() ? "SI" : "S";
         moveMap.bind( mouse, yaxis, %yFlags, %ySens, "pitch" );
	 
         switch ( MouseZActionMenu.getSelected() )
         {
            case 2:
               moveMap.bind( mouse, zaxis, cycleWeaponAxis );
            case 3:
               moveMap.bind( mouse, zaxis, cycleNextWeaponOnly );
            case 4:
               moveMap.bind( mouse, zaxis, toggleZoomFOV );
            default:
               moveMap.unbind( mouse, zaxis );
         }
		 
   // write out the control config files into the user home directory

   moveMap.save($HomePath @ "/bindings.cs", false);
   spectatorMap.save($HomePath @ "/bindings.cs", true);
   vehicleMap.save($HomePath @ "/bindings.cs", true);
   
   //export settings directly in case the game is not exited regularly and settings are lost
   export("$pref::*", $HomePath @ "/config.cs", false);
}

function OptGraphicsDriverMenu::onSelect( %this, %id, %text )
{
   // Attempt to keep the same resolution settings:
   %resMenu = OptionsDlg-->OptGraphicsResolutionMenu;	
   %currRes = %resMenu.getText();
   
   // If its empty the use the current.
   if ( %currRes $= "" )
      %currRes = _makePrettyResString( Canvas.getVideoMode() );
                  
   // Fill the resolution list.
   OptionsDlg.initResMenu();

   // Try to select the previous settings:
   %selId = %resMenu.findText( %currRes );
   if ( %selId == -1 )	
      %selId = 0;
	   
   %resMenu.setSelected( %selId );
	
   OptionsDlg._updateApplyState();
}

function _makePrettyResString( %resString )
{
   %width = getWord( %resString, $WORD::RES_X );
   %height = getWord( %resString, $WORD::RES_Y );
   
   %aspect = %width / %height;
   %aspect = mRound( %aspect * 100 ) * 0.01;            
   
   switch$( %aspect )
   {
      case "1.33":
         %aspect = "4:3";
      case "1.6":
         %aspect = "16:10";
      case "1.78":
         %aspect = "16:9";
      default:
         %aspect = "";
   }
   
   %outRes = %width @ " x " @ %height;
   if ( %aspect !$= "" )
      %outRes = %outRes @ "  (" @ %aspect @ ")";
      
   return %outRes;   
}

function OptionsDlg::initResMenu( %this )
{
   // Clear out previous values
   %resMenu = %this-->OptGraphicsResolutionMenu;	   
   %resMenu.clear();
   
   // If we are in a browser then we can't change our resolution through
   // the options dialog
   if (getWebDeployment())
   {
      %count = 0;
      %currRes = getWords(Canvas.getVideoMode(), $WORD::RES_X, $WORD::RES_Y);
      %resMenu.add(%currRes, %count);
      %count++;

      return;
   }
   
   // Loop through all and add all valid resolutions
   %count = 0;
   %resCount = Canvas.getModeCount();
   for (%i = 0; %i < %resCount; %i++)
   {
      %testResString = Canvas.getMode( %i );
      %testRes = _makePrettyResString( %testResString );
                     
      // Only add to list if it isn't there already.
      if (%resMenu.findText(%testRes) == -1)
      {
         %resMenu.add(%testRes, %i);
         %count++;
      }
   }
   
   %resMenu.sort();
}

function OptionsDlg::applyGraphics( %this, %testNeedApply )
{
   %newAdapter    = OptGraphicsDriverMenu.getText();
   %numAdapters   = GFXInit::getAdapterCount();
   %newDevice     = $pref::Video::displayDevice;
							
   for( %i = 0; %i < %numAdapters; %i ++ )
   {
      if( GFXInit::getAdapterName( %i ) $= %newAdapter )
      {
         %newDevice = GFXInit::getAdapterType( %i );
         break;
      }
   }
   // Change the device.
   if ( %newDevice !$= $pref::Video::displayDevice )
   {
      if ( %testNeedApply )
         return true;
         
      $pref::Video::displayDevice = %newDevice;
      if( %newAdapter !$= getDisplayDeviceInformation() )
         MessageBoxOK( "Change requires restart", "Please restart the game for a display device change to take effect." );
   }

   // Gather the new video mode.
   if ( isFunction("getWebDeployment") && getWebDeployment() )
   {
      // Under web deployment, we use the custom resolution rather than a Canvas
      // defined one.
      %newRes = %this-->OptGraphicsResolutionMenu.getText();
   }
   else
   {
      %newRes = getWords( Canvas.getMode( %this-->OptGraphicsResolutionMenu.getSelected() ), $WORD::RES_X, $WORD::RES_Y ); 
   }

   %newBpp        = 32; // ... its not 1997 anymore.
   %newFullScreen = %this-->OptGraphicsFullscreenToggle.getValue() ? "true" : "false";
   %newRefresh    = %this-->OptRefreshSelectMenu.getSelected();
   %newVsync = !%this-->OptGraphicsVSyncToggle.getValue();	
   %newFSAA = %this-->OptAAQualityPopup.getSelected();

   // Under web deployment we can't be full screen.
   if ( isFunction("getWebDeployment") && getWebDeployment() )
   {
      %newFullScreen = false;
   }
   else if ( %newFullScreen $= "false" )
   {
      // If we're in windowed mode switch the fullscreen check
      // if the resolution is bigger than the desktop.
      %deskRes    = getDesktopResolution();      
      %deskResX   = getWord(%deskRes, $WORD::RES_X);
      %deskResY   = getWord(%deskRes, $WORD::RES_Y);
      if ( getWord( %newRes, $WORD::RES_X ) > %deskResX || getWord( %newRes, $WORD::RES_Y ) > %deskResY )
      {
         %newFullScreen = "true";
         %this-->OptGraphicsFullscreenToggle.setStateOn( true );
      }
   }

   // Build the final mode string.
   %newMode = %newRes SPC %newFullScreen SPC %newBpp SPC %newRefresh SPC %newFSAA;
	
   // Change the video mode.   
   if (  %newMode !$= $pref::Video::mode || %newVsync != $pref::Video::disableVerticalSync )
   {
      if ( %testNeedApply )
         return true;

      $pref::Video::mode = %newMode;
      $pref::Video::disableVerticalSync = %newVsync;      
      configureCanvas();
   }

   // Test and apply the graphics settings.
   if ( %this-->OptMeshQualityPopup.apply( MeshQualityGroup, %testNeedApply ) ) return true;            
   if ( %this-->OptTextureQualityPopup.apply( TextureQualityGroup, %testNeedApply ) ) return true;            
   if ( %this-->OptLightingQualityPopup.apply( LightingQualityGroup, %testNeedApply ) ) return true;            
   if ( %this-->OptShaderQualityPopup.apply( ShaderQualityGroup, %testNeedApply ) ) return true;     

   if ( %this-->OptDecalQualityPopup.apply( DecalQualityGroup, %testNeedApply ) ) return true;
   
   // Check the anisotropic filtering.   
   %aniLevel = %this-->OptAnisotropicPopup.getSelected();
   if ( %aniLevel != $pref::Video::defaultAnisotropy )
   {
      if ( %testNeedApply )
         return true;
                                 
      $pref::Video::defaultAnisotropy = %aniLevel;
   }
   
   // If we're applying the state then recheck the 
   // state to update the apply button.
   if ( !%testNeedApply )
      %this._updateApplyState();
      
   return false;
}

function OptionsDlg::_updateApplyState( %this )
{
   %applyCtrl = %this-->Apply;
   %graphicsPane = %this-->OptGraphicsPane;
         
   assert( isObject( %applyCtrl ) );
   assert( isObject( %graphicsPane ) );

   %applyCtrl.active = %graphicsPane.isVisible() && %this.applyGraphics( true );   
}

function OptionsDlg::_autoDetectQuality( %this )
{
   %msg = GraphicsQualityAutodetect();
   %this.onWake();
   
   if ( %msg !$= "" )
   {
      MessageBoxOK( "Notice", %msg );
   }
}

//------------------------------------------------------------------------------
// Keymaps

function restoreDefaultMappings()
{
   moveMap.delete();
   spectatorMap.delete();
   vehicleMap.delete();
   exec( "~/client/default.bind.cs" );
   OptionsDlg.fillRemapList();
}

function getMapDisplayName( %device, %action )
{
	if ( %device $= "keyboard" )
		return( %action );		
	else if ( strstr( %device, "mouse" ) != -1 )
	{
		// Substitute "mouse" for "button" in the action string:
		%pos = strstr( %action, "button" );
		if ( %pos != -1 )
		{
			%mods = getSubStr( %action, 0, %pos );
			%object = getSubStr( %action, %pos, 1000 );
			%instance = getSubStr( %object, strlen( "button" ), 1000 );
			return( %mods @ "mouse" @ ( %instance + 1 ) );
		}
		//else
		//	error( "Mouse input object other than button passed to getDisplayMapName!" );
	}
	else if ( strstr( %device, "joystick" ) != -1 )
	{
		// Substitute "joystick" for "button" in the action string:
		%pos = strstr( %action, "button" );
		if ( %pos != -1 )
		{
			%mods = getSubStr( %action, 0, %pos );
			%object = getSubStr( %action, %pos, 1000 );
			%instance = getSubStr( %object, strlen( "button" ), 1000 );
			return( %mods @ "joystick" @ ( %instance + 1 ) );
		}
		else
	   { 
	      %pos = strstr( %action, "pov" );
         if ( %pos != -1 )
         {
            %wordCount = getWordCount( %action );
            %mods = %wordCount > 1 ? getWords( %action, 0, %wordCount - 2 ) @ " " : "";
            %object = getWord( %action, %wordCount - 1 );
            switch$ ( %object )
            {
               case "upov":   %object = "POV1 up";
               case "dpov":   %object = "POV1 down";
               case "lpov":   %object = "POV1 left";
               case "rpov":   %object = "POV1 right";
               case "upov2":  %object = "POV2 up";
               case "dpov2":  %object = "POV2 down";
               case "lpov2":  %object = "POV2 left";
               case "rpov2":  %object = "POV2 right";
               default:       %object = "??";
            }
            return( %mods @ %object );
         }
         //else
         //   error( "Unsupported Joystick input object passed to getDisplayMapName!" );
      }
	}
		
	return( "??" );		
}

function OptBindListMenu::init(%this)
{
   %selId = %this.getSelected();
   %this.clear();
   %this.add( "Main", 0 );
   %this.add( "Spectator", 1 );
   %this.add( "Vehicle", 2 );
   %this.setSelected( %selId );
   %this.onSelect( %selId, %this.getTextById( %selId ) );
}

function OptBindListMenu::onSelect( %this, %id, %text )
{
   OptControlsPane.group = %text;
   OptionsDlg.fillRemapList();
}

function buildFullMapString( %index )
{
   switch$ ( OptControlsPane.group )
   {
      case "Spectator":
         %actionMap  = spectatorMap;
         %name       = $SpecRemapName[%index];
         %cmd        = $SpecRemapCmd[%index];

      case "Vehicle":
         %actionMap  = vehicleMap;
         %name       = $VehRemapName[%index];
         %cmd        = $VehRemapCmd[%index];

      default:
         %actionMap  = moveMap;
         %name       = $RemapName[%index];
         %cmd        = $RemapCmd[%index];
   }

   %temp = %actionMap.getBinding( %cmd );
   if ( %temp $= "" )
      return %name TAB "";

   %mapString = "";

   %count = getFieldCount( %temp );
   for ( %i = 0; %i < %count; %i += 2 )
   {
      if ( %mapString !$= "" )
         %mapString = %mapString @ ", ";

      %device = getField( %temp, %i + 0 );
      %object = getField( %temp, %i + 1 );
      %mapString = %mapString @ getMapDisplayName( %device, %object );
   }

   return( %name TAB %mapString ); 
}

function OptionsDlg::fillRemapList( %this )
{
   switch$ ( OptControlsPane.group )
   {
      case "Spectator":
         %count = $SpecRemapCount;
      case "Vehicle":
         %count = $VehRemapCount;
      default:
         %count = $RemapCount;
   }

   %remapList = %this-->OptRemapList;

   %remapList.clear();
   for ( %i = 0; %i < %count; %i++ )
      %remapList.addRow( %i, buildFullMapString( %i ) );
}

function OptionsDlg::doRemap( %this )
{
   %remapList = %this-->OptRemapList;
   %selId = %remapList.getSelectedId();

   switch$ ( OptControlsPane.group )
   {
      case "Spectator":
         %name = $SpecRemapName[%selId];
      case "Vehicle":
         %name = $VehRemapName[%selId];
      default:
         %name = $RemapName[%selId];
   }

   //RemapDlg-->OptRemapText.setValue( "Re-bind \"" @ %name @ "\" to..." );
   RemapDlg-->OptRemapText.addtext("Re-bind \"" @ %name @ "\""
                        @ "\n\nPressing backspace will clear the current key map for that control."
                        @ "\n\nPressing escape will return you to the control menu without changing anything.");

   OptRemapInputCtrl.index = %selId;
   Canvas.pushDialog( RemapDlg );
}

function redoMapping( %device, %action, %cmd, %oldIndex, %newIndex )
{
   switch$ ( OptControlsPane.group )
   {
      case "Spectator":
         %actionMap = spectatorMap;

      case "Vehicle":
         %actionMap = vehicleMap;

      default:
         %actionMap = moveMap;
   }

   //%actionMap.bind( %device, %action, $RemapCmd[%newIndex] );
   %actionMap.bind( %device, %action, %cmd );
	
   //those 3 lines cause a crash, so I commented them out, hope they were not too important
   //%remapList = %this-->OptRemapList;
   //%remapList.setRowById( %oldIndex, buildFullMapString( %oldIndex ) );
   //%remapList.setRowById( %newIndex, buildFullMapString( %newIndex ) );

   OptionsDlg.fillRemapList();
}

function findRemapCmdIndex( %command )
{
   switch$ ( OptControlsPane.group )
   {
      case "Spectator":
         for ( %i = 0; %i < $SpecRemapCount; %i++ )
         {
            if ( %command $= $SpecRemapCmd[%i] )
               return( %i );
         }

      case "Vehicle":
         for ( %i = 0; %i < $VehRemapCount; %i++ )
         {
            if ( %command $= $VehRemapCmd[%i] )
               return( %i );
         }

      default:
         for ( %i = 0; %i < $RemapCount; %i++ )
         {
            if ( %command $= $RemapCmd[%i] )
            return( %i );			
         }
   }

   return( -1 );	
}

/// This unbinds actions beyond %count associated to the
/// particular moveMap %commmand.
function unbindExtraActions( %command, %count )
{
   switch$ ( OptControlsPane.group )
   {
      case "Spectator":
         %actionMap = spectatorMap;

      case "Vehicle":
         %actionMap = vehicleMap;

      default:
         %actionMap = moveMap;
   }

   %temp = %actionMap.getBinding( %command );
   if ( %temp $= "" )
      return;

   %count = getFieldCount( %temp ) - ( %count * 2 );
   for ( %i = 0; %i < %count; %i += 2 )
   {
      %device = getField( %temp, %i + 0 );
      %action = getField( %temp, %i + 1 );
      
      %actionMap.unbind( %device, %action );
   }
}

function OptRemapInputCtrl::onInputEvent( %this, %device, %action )
{
   //error( "** onInputEvent called - device = " @ %device @ ", action = " @ %action @ " **" );
   Canvas.popDialog( RemapDlg );

   switch$ ( OptControlsPane.group )
   {
      case "Spectator":
         %actionMap = spectatorMap;
         %cmd = $SpecRemapCmd[%this.index];
         %name = $SpecRemapName[%this.index];

      case "Vehicle":
         %actionMap = vehicleMap;
         %cmd = $VehRemapCmd[%this.index];
         %name = $VehRemapName[%this.index];

      default:
         %actionMap = moveMap;
         %cmd  = $RemapCmd[%this.index];
         %name = $RemapName[%this.index];
   }

   //%actionMap = moveMap;
   //%cmd  = $RemapCmd[%this.index];
   //%name = $RemapName[%this.index];

   // Test for the reserved keystrokes:
   if ( %device $= "keyboard" )
   {
      // Cancel...
      if ( %action $= "escape" )
      {
         // Do nothing...
         return;
      }
      // Taken from this resource by OneST8
      // http://www.garagegames.com/index.php?sec=mg&mod=resource&page=view&qid=7311
      if ( %action $= "backspace" ) // clear the current binding.
      {
         //get the current binding, device and action
         %bind = %actionMap.getBinding( %cmd );
         %device = getWord( %bind, 0 );
         %action = getWord( %bind, 1 );

         //remove the binding
         unbindExtraActions( %cmd, 1 );
         %actionMap.unbind( %device, %action );

         //refresh the options dialog to reflect the change
         OptionsDlg-->OptRemapList.setRowById( %this.index, buildFullMapString( %this.index ) );
         return;
      }
   }

   // Grab the friendly display name for this action
   // which we'll use when prompting the user below.
   %mapName = getMapDisplayName( %device, %action );
   
   // Get the current command this action is mapped to.
   %prevMap = %actionMap.getCommand( %device, %action );

   // If nothing was mapped to the previous command 
   // mapping then it's easy... just bind it.
   if ( %prevMap $= "" )
   {
      unbindExtraActions( %cmd, 1 );
      %actionMap.bind( %device, %action, %cmd );
      OptionsDlg-->OptRemapList.setRowById( %this.index, buildFullMapString( %this.index ) );
      return;
   }

   // If the previous command is the same as the 
   // current then they hit the same input as what
   // was already assigned.
   if ( %prevMap $= %cmd )
   {
      unbindExtraActions( %cmd, 0 );
      %actionMap.bind( %device, %action, %cmd );
      OptionsDlg-->OptRemapList.setRowById( %this.index, buildFullMapString( %this.index ) );
      return;   
   }

   // Look for the index of the previous mapping.
   %prevMapIndex = findRemapCmdIndex( %prevMap );
   
   // If we get a negative index then the previous 
   // mapping was to an item that isn't included in
   // the mapping list... so we cannot unmap it.
   if ( %prevMapIndex == -1 )
   {
      MessageBoxOK( "Remap Failed", "\"" @ %mapName @ "\" is already bound to a non-remappable command!" );
      return;
   }

   // Setup the forced remapping callback command.
   %callback = "redoMapping(" @ %device @ ", \"" @ %action @ "\", \"" @
                              %cmd @ "\", " @ %prevMapIndex @ ", " @ %this.index @ ");";
   
   // Warn that we're about to remove the old mapping and
   // replace it with another.
   switch$ ( OptControlsPane.group )
   {
      case "Spectator":
         %prevCmdName = $SpecRemapName[%prevMapIndex];
      case "Vehicle":
         %prevCmdName = $VehRemapName[%prevMapIndex];
      default:
         %prevCmdName = $RemapName[%prevMapIndex];
   }

   MessageBoxYesNo( "Warning",
      "\"" @ %mapName @ "\" is already bound to \""
      @ %prevCmdName @ "\"!\nDo you wish to replace this mapping?",
       %callback, "" );
}

//------------------------------------------
// Mouse

function MouseXSlider::sync( %this )
{
   %thisValue = %this.getValue();
   MouseXText.setValue( "(" @ getSubStr( %thisValue, 0, 4 ) @ ")" );
   if ( $pref::Input::LinkMouseSensitivity )
   {
      if ( MouseYSlider.getValue() != %thisValue )
         MouseYSlider.setValue( %thisValue );
   }
}

function MouseYSlider::sync( %this )
{
   %thisValue = %this.getValue();
   MouseYText.setValue( "(" @ getSubStr( %thisValue, 0, 4 ) @ ")" );
   if ( $pref::Input::LinkMouseSensitivity )
   {
      if ( MouseXSlider.getValue() != %thisValue )
         MouseXSlider.setValue( %thisValue );
   }
}

function toggleInvertYAxis()
{
   // Catch the case where this is toggled in-game while in a vehicle:
   if ( isObject( vehicleMap ) )
   {
      %bind = vehicleMap.getBinding( pitch );
      if ( %bind !$= "" )
      {
         %device = getField( %bind, 0 );
         %action = getField( %bind, 1 );
         %flags = $pref::Vehicle::InvertYAxis ? "SDI" : "SD";
         %deadZone = vehicleMap.getDeadZone( %device, %action );
         %scale = vehicleMap.getScale( %device, %action );
         vehicleMap.bind( %device, %action, %flags, %deadZone, %scale, pitch );
      }
   }
}

//------------------------------------------------------------------------------
// Audio
$AudioTestHandle = 0;
// Description to use for playing the volume test sound.  This isn't
// played with the description of the channel that has its volume changed
// because we know nothing about the playback state of the channel.  If it
// is paused or stopped, the test sound would not play then.
$AudioTestDescription = new SFXDescription()
{
   sourceGroup = AudioChannelMaster;
};

function OptAudioUpdateMasterVolume( %volume )
{
   if( %volume == $pref::SFX::masterVolume )
      return;
      
   sfxSetMasterVolume( %volume );
   $pref::SFX::masterVolume = %volume;
   
   if( !isObject( $AudioTestHandle ) )
      $AudioTestHandle = sfxPlayOnce( AudioChannel, "art/sound/gui/volumeTest.wav" );
}

function OptAudioUpdateChannelVolume( %description, %volume )
{
   %channel = sfxGroupToOldChannel( %description.sourceGroup );
   
   if( %volume == $pref::SFX::channelVolume[ %channel ] )
      return;

   sfxSetChannelVolume( %channel, %volume );
   $pref::SFX::channelVolume[ %channel ] = %volume;
   
   if( !isObject( $AudioTestHandle ) )
   {
      $AudioTestDescription.volume = %volume;
      $AudioTestHandle = sfxPlayOnce( $AudioTestDescription, "art/sound/gui/volumeTest.wav" );
   }
}

function OptAudioProviderList::onSelect( %this, %id, %text )
{
   // Skip empty provider selections.   
   if ( %text $= "" )
      return;
      
   $pref::SFX::provider = %text;
   OptAudioDeviceList.clear();
   
   %buffer = sfxGetAvailableDevices();
   %count = getRecordCount( %buffer );   
   for(%i = 0; %i < %count; %i++)
   {
      %record = getRecord(%buffer, %i);
      %provider = getField(%record, 0);
      %device = getField(%record, 1);
      
      if (%provider !$= %text)
         continue;
            
       if ( OptAudioDeviceList.findText( %device ) == -1 )
            OptAudioDeviceList.add( %device, %i );
   }

   // Find the previous selected device.
   %selId = OptAudioDeviceList.findText($pref::SFX::device);
   if ( %selId == -1 )
      OptAudioDeviceList.setFirstSelected();
   else
      OptAudioDeviceList.setSelected( %selId );

   OptAudioUpdate();
}

function OptAudioDeviceList::onSelect( %this, %id, %text )
{
   // Skip empty selections.
   if ( %text $= "" )
      return;
      
   $pref::SFX::device = %text;

   if ( !sfxCreateDevice(  $pref::SFX::provider, 
                           $pref::SFX::device, 
                           $pref::SFX::useHardware,
                           -1 ) )                              
      error( "Unable to create SFX device: " @ $pref::SFX::provider 
                                             SPC $pref::SFX::device 
                                             SPC $pref::SFX::useHardware );
}

function OptMouseSetSensitivity(%value)
{  
   $pref::Input::LinkMouseSensitivity = %value;
}  

/*
function OptAudioHardwareToggle::onClick(%this)
{
   if (!sfxCreateDevice($pref::SFX::provider, $pref::SFX::device, $pref::SFX::useHardware, -1))
      error("Unable to create SFX device: " @ $pref::SFX::provider SPC $pref::SFX::device SPC $pref::SFX::useHardware);
}
*/

function OptAudioUpdate()
{
   // set the driver text
   OptAudioInfo.setText("");
   %text = sfxGetDeviceInfo();

   %count = getFieldCount( %text );
   for( %i=0; %i < %count; %i++ )
   {
      %line = getField( %text, %i );
      OptAudioInfo.addtext(%line @ "\n", true);
   }
}

//-----------------------------------------------------------------------------

function Tgl_9::onAction(%this)
{
   if ( %this.getValue() )
   {     
      SunBokehPostFX.enable();
      $pref::LightManager::Enable::SunBokeh = "1";
   }
   else
   {
      SunBokehPostFX.disable();
      $pref::LightManager::Enable::SunBokeh = "0";
   }
}

function Tgl_10::onAction(%this)
{
   if ( %this.getValue() )
   {     
      VignettePostFX.enable();
      $pref::LightManager::Enable::Vignette = "1";
   }
   else
   {
      VignettePostFX.disable();
      $pref::LightManager::Enable::Vignette = "0";
   }
}

function Tgl_11::onAction(%this)
{
   if ( %this.getValue() )
   {    
      HDRPostFX.enable();
      $pref::LightManager::Enable::HDR = "1";
   }
   else
   {
      HDRPostFX.disable();
      $pref::LightManager::Enable::HDR = "0";
   }
}

function Tgl_12::onAction(%this)
{
   if ( %this.getValue() )
   {
      SSAOPostFX.enable();
      $pref::LightManager::Enable::SSAO = "1";
   }
   else
   {    
      SSAOPostFX.disable();
      $pref::LightManager::Enable::SSAO = "0";
   }
}

function Tgl_13::onAction(%this)
{
   if ( %this.getValue() )
   {     
      LightRayPostFX.enable();
      $pref::LightManager::Enable::LightRay = "1";
   }
   else
   {
      LightRayPostFX.disable();
      $pref::LightManager::Enable::LightRay = "0";
   }
}

function Tgl_14::onAction(%this)
{
   if ( %this.getValue() )
   {
      DOFPostFX.enable();
      $pref::LightManager::Enable::DOF = "1";
   }
   else
   {
      DOFPostFX.disable();
      $pref::LightManager::Enable::DOF = "0";
   }
}

//------------------------------------------------------------------------------
// Player Pane

function OptPlayerNameInput::setField(%this)
{
   // called when you type in text input field
   %value = %this.getValue();
   %this.setValue(%value);
}

function OptPlayerSkinMenu::init(%this)
{
   PlayerPreview.setSkin( getField($pref::Player, 1) );
   PlayerPreview.setMountSkin( "Lurker_D" );

   %this.clear();
   %list = "base	olive	urban	desert	swamp	water	blue	red	green	yellow";

   %count = getFieldCount( %list );
   for( %i=0; %i < %count; %i++ )
   {
      %skin = getField( %list, %i );
      %this.add( %skin, %i );
   }

   %selId = %this.findText( getField( $pref::Player, 1) );

   if( %selId != -1 )
   {
      %this.setSelected( %selId );
      %this.onSelect( %selId, %this.getTextById( %selId ) );
   }
}

function OptPlayerSkinMenu::onSelect(%this, %id, %text)
{
   // Only select if it was the mouse that called this
   if(%text $= "")
      return( false );

   OptPlayerNameInput.setField();
   %playerName = OptPlayerNameInput.getValue();
   $pref::Player = %playerName TAB %text;

   PlayerPreview.setSkin( %text );
   PlayerPreview.setMountSkin( "Lurker_D" );
   //PlayerPreview.setSeq( "Celebrate_01" );
}

function updateFieldOfView()
{
   %value = OP_FovSlider.getValue();
   //warn("updateFieldOfView" SPC mFloor(%value));
   $pref::Player::Fov = mFloor( %value );

   if ( Canvas.getContent() == PlayGui.getId() )
      setFov( mFloor( %value ) );
}
