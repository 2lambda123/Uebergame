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

//---------------------------------------------------------------------------------------------
// initializeCore
// Initializes core game functionality.
//---------------------------------------------------------------------------------------------
function Torque::initializeCore(%this)
{
   // Not Reentrant
   if( $coreInitialized == true )
      return;

   // Very basic functions used by everyone.
   exec("./audio.cs");
   exec("./canvas.cs");
   exec("scripts/gui/cursors.cs");
   exec("./cursor.cs");
   exec("./persistenceManagerTest.cs");
   
   exec( "./audioEnvironments.cs" );
   exec( "./audioDescriptions.cs" );
   exec( "./audioStates.cs" );
   exec( "./audioAmbiences.cs" );

   // Input devices
   exec("./oculusVR.cs");

   // Seed the random number generator.
   setRandomSeed();
   
   // Set up networking.
   setNetPort(0);
  
   // Initialize the canvas.
   initializeCanvas();

   // Start processing file change events.   
   startFileChangeNotifications();
      
   // Core Guis.
   exec("scripts/gui/console.gui");
   exec("scripts/gui/consoleVarDlg.gui");
   exec("scripts/gui/netGraphGui.gui");
   
   // Gui Helper Scripts.
   exec("scripts/gui/helpDlg.cs");
   exec("scripts/gui/console.cs");
   exec("scripts/gui/consoleVarDlg.cs");
   exec("scripts/gui/netGraphGui.cs");

   // Random Scripts.
   exec("./screenshot.cs");
   exec("./scriptDoc.cs");
   exec("./helperfuncs.cs");
   exec("./commands.cs");
   
   // Client scripts
   exec("./devHelpers.cs");
   exec("./metrics.cs");
   exec("./centerPrint.cs");

   // Materials and Shaders for rendering various object types
   loadCoreMaterials();

   exec("./commonMaterialData.cs");
   exec("./shaders.cs");
   exec("./materials.cs");
   exec("./terrainBlock.cs");
   exec("./water.cs");
   exec("./imposter.cs");
   exec("./scatterSky.cs");
   exec("./clouds.cs");
   
   // Initialize all core post effects.   
   exec("./postFx.cs");
   initPostEffects();
   
   // Initialize the post effect manager.
   exec("./postFx/postFXManager.gui");
   exec("./postFx/postFXManager.gui.cs");
   exec("./postFx/postFXManager.gui.settings.cs");
   exec("./postFx/postFXManager.persistance.cs");
   
   PostFXManager.settingsApplyDefaultPreset();  // Get the default preset settings   

   $coreInitialized = true;
}

//---------------------------------------------------------------------------------------------
// shutdownCore
// Shuts down core game functionality.
//---------------------------------------------------------------------------------------------
function shutdownCore()
{      
   // Stop file change events.
   stopFileChangeNotifications();
   
   sfxShutdown();
}

function handleEscape()
{
   if (isObject(EditorGui))
   {
      if (Canvas.getContent() == EditorGui.getId())
      {
         EditorGui.handleEscape();
         return;
      }
      else if ( EditorIsDirty() )
      {
         MessageBoxYesNoCancel( "Level Modified", "Level has been modified in the Editor. Save?",
                           "EditorDoExitMission(1);",
                           "EditorDoExitMission();",
                           "");
         return;
      }
   }

   if (isObject(GuiEditor))
   {
      if (GuiEditor.isAwake())
      {
         GuiEditCanvas.quit();
         return; 
      }
   }

   if (PlayGui.isAwake())	
      escapeFromGame();	
}

//-----------------------------------------------------------------------------
// loadMaterials - load all materials.cs files
//-----------------------------------------------------------------------------
function loadCoreMaterials()
{
   // Load any materials files for which we only have DSOs.

   for( %file = findFirstFile( "scripts/materials.cs.dso" );
        %file !$= "";
        %file = findNextFile( "scripts/materials.cs.dso" ))
   {
      // Only execute, if we don't have the source file.
      %csFileName = getSubStr( %file, 0, strlen( %file ) - 4 );
      if( !isFile( %csFileName ) )
         exec( %csFileName );
   }

   // Load all source material files.

   for( %file = findFirstFile( "scripts/materials.cs" );
        %file !$= "";
        %file = findNextFile( "scripts/materials.cs" ))
   {
      exec( %file );
   }
}

function reloadCoreMaterials()
{
   reloadTextures();
   loadCoreMaterials();
   reInitMaterials();
}

//-----------------------------------------------------------------------------
// loadMaterials - load all materials.cs files
//-----------------------------------------------------------------------------
function loadMaterials()
{
   // Load any materials files for which we only have DSOs.

   for( %file = findFirstFile( "*/materials.cs.dso" );
        %file !$= "";
        %file = findNextFile( "*/materials.cs.dso" ))
   {
      // Only execute, if we don't have the source file.
      %csFileName = getSubStr( %file, 0, strlen( %file ) - 4 );
      if( !isFile( %csFileName ) )
         exec( %csFileName );
   }

   // Load all source material files.

   for( %file = findFirstFile( "*/materials.cs" );
        %file !$= "";
        %file = findNextFile( "*/materials.cs" ))
   {
      exec( %file );
   }

   // Load all materials created by the material editor if
   // the folder exists
   if( IsDirectory( "materialEditor" ) )
   {
      for( %file = findFirstFile( "materialEditor/*.cs.dso" );
           %file !$= "";
           %file = findNextFile( "materialEditor/*.cs.dso" ))
      {
         // Only execute, if we don't have the source file.
         %csFileName = getSubStr( %file, 0, strlen( %file ) - 4 );
         if( !isFile( %csFileName ) )
            exec( %csFileName );
      }

      for( %file = findFirstFile( "materialEditor/*.cs" );
           %file !$= "";
           %file = findNextFile( "materialEditor/*.cs" ))
      {
         exec( %file );
      }
   }
}

function reloadMaterials()
{
   reloadTextures();
   loadMaterials();
   reInitMaterials();
}

// From Martin "Founder" Hoover.
// http://www.garagegames.com/my/home/view.profile.php?qid=5055
function cropXDecimals(%num, %count)
{
   %length = strlen(%num);
   %dot = 0;
   for ( %i = 0; %i < %length; %i++ )
   {
      if ( getSubStr( %num, %i, 1 ) $= "." )
      {
         %dot = %i;
         break;
      }
   }

   if ( %dot > 0 )
      %final = getSubStr( %num, 0, %dot + %count );
   else
      %final = %num;

   return %final;
}
