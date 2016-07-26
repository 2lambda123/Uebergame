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

// Set the name of our application
$appName = "Uebergame";

// The directory it is run from
$defaultGame = "scripts";

// Set profile directory
$pref::Video::ProfilePath = "scripts/client/profile";

function createCanvas(%windowTitle)
{
   if ($isDedicated)
   {
      GFXInit::createNullDevice();
      return true;
   }

   // Create the Canvas
   %foo = new GuiCanvas(Canvas)
   {
      displayWindow = false;
   };
   
   // Set the window title
   if (isObject(Canvas))
      Canvas.setWindowTitle($appName);
   
   return true;
}

// Display the optional commandline arguements
$displayHelp = false;

// Use these to record and play back crashes
//saveJournal("editorOnFileQuitCrash.jrn");
//playJournal("editorOnFileQuitCrash.jrn", false);

// --------------------------------------------------------------------
// Packages Bug fix Package
// From Martin "Founder" Hoover.
// http://www.garagegames.com/community/forums/viewthread/19617/1#comment-151672


$TotalNumberOfPackages = 0;
package PackageFix
{
   // Fixes bug whereby parent functions are lost when packages are deactivated.
   function isActivePackage(%package)
   {
      for(%i = 0; %i < $TotalNumberOfPackages; %i++)
      {
         if($Package[%i] $= %package)
         {
            return true;
            break;
         }
      }
      return false;
   }

   function activatePackage(%this)
   {
      // This package name is allready active, so lets not activate it again.
      if(isActivePackage(%this))
      {
         error(%this SPC "is a currently active package. activatePackage() failed.");
         return;
      }
      Parent::ActivatePackage(%this);

      if($TotalNumberOfPackages $= "")
         $TotalNumberOfPackages = 0;

      $Package[$TotalNumberOfPackages] = %this;
      $TotalNumberOfPackages++;
   }

   function deactivatePackage(%this)
   {
      if(!isActivePackage(%this))
      {
         error(%this SPC "is not an active package. deactivatePackage() failed.");
         return;
      }
      %count = 0;
      %counter = 0;
      //find the index number of the package to deactivate
      for(%i = 0; %i < $TotalNumberOfPackages; %i++)
      {
         if($Package[%i] $= %this)
            %breakpoint = %i;
      }
      for(%j = 0; %j < $TotalNumberOfPackages; %j++)
      {
         if(%j < %breakpoint)
         {
            //go ahead and assign temp array, save code
            %tempPackage[%count] = $Package[%j];
            %count++;
         }
         else if(%j > %breakpoint)
         {
            %reactivate[%counter] = $Package[%j];
            $Package[%j] = "";
            %counter++;
         }
      }
      //deactivate all the packagess from the last to the current one
      for(%k = (%counter - 1); %k > -1; %k--)
         Parent::DeactivatePackage(%reactivate[%k]);

      //deactivate the package that started all this
      Parent::DeactivatePackage(%this);

      //don't forget this
      $TotalNumberOfPackages = %breakpoint;

      //reactivate all those other packages
      for(%l = 0; %l < %counter; %l++)
      ActivatePackage(%reactivate[%l]);
   }

   function listPackages()
   {
      echo("----------------------------------------");
      echo("Activated Packages:");
      for(%i = 0; %i < $TotalNumberOfPackages; %i++)
         echo($Package[%i]);

      echo("----------------------------------------");
   }
};
activatePackage(PackageFix);

//------------------------------------------------------------------------------
// This is our main game object, we use this so mod packages can be
// deactivated without loss of parent functions.
new ScriptObject(tge)
{
   class = Torque;
};

//------------------------------------------------------------------------------
// Check if a script file exists, compiled or not.
function isScriptFile(%path)
{
   if( isFile(%path @ ".dso") || isFile(%path) )
      return true;
   
   return false;
}

//------------------------------------------------------------------------------
// Process command line arguments
exec("scripts/parseArgs.cs");

$isDedicated = false;
$dirCount = 2;
$userDirs = $defaultGame @ ";art;levels";

// load tools scripts if we're a tool build
if (isToolBuild())
    $userDirs = "tools;" @ $userDirs;


// Parse the executable arguments with the standard
// function from scripts/parseArgs.cs
defaultParseArgs();


if($dirCount == 0) {
      $userDirs = $defaultGame;
      $dirCount = 1;
}

//-----------------------------------------------------------------------------
// Display a splash window immediately to improve app responsiveness before
// engine is initialized and main window created
 if (!$isDedicated)
 displaySplashWindow();


//-----------------------------------------------------------------------------
// The displayHelp, onStart, onExit and parseArgs function are overriden
// by mod packages to get hooked into initialization and cleanup.

function Torque::onStart(%this)
{
   // Default startup function
}

function onExit()
{
   // OnExit is called directly from C++ code, whereas onStart is
   // invoked at the end of this file.
}

function Torque::parseArgs(%this)
{
   // Here for mod override, the arguments have already
   // been parsed.
}

function compileFiles(%pattern)
{  
   %path = filePath(%pattern);

   %saveDSO    = $Scripts::OverrideDSOPath;
   %saveIgnore = $Scripts::ignoreDSOs;
   
   $Scripts::OverrideDSOPath  = %path;
   $Scripts::ignoreDSOs       = false;
   %mainCsFile = makeFullPath("main.cs");

   for (%file = findFirstFileMultiExpr(%pattern); %file !$= ""; %file = findNextFileMultiExpr(%pattern))
   {
      // we don't want to try and compile the primary main.cs
      if(%mainCsFile !$= %file)      
         compile(%file, true);
   }

   $Scripts::OverrideDSOPath  = %saveDSO;
   $Scripts::ignoreDSOs       = %saveIgnore;
}

if($compileAll)
{
   echo(" --- Compiling all files ---");
   compileFiles("*.cs");
   compileFiles("*.gui");
   compileFiles("*.ts");  
   echo(" --- Exiting after compile ---");
   quit();
}

if($compileTools)
{
   echo(" --- Compiling tools scritps ---");
   compileFiles("tools/*.cs");
   compileFiles("tools/*.gui");
   compileFiles("tools/*.ts");  
   echo(" --- Exiting after compile ---");
   quit();
}

package help
{
   function onExit()
   {
      // Override onExit when displaying help
   }
};

function Torque::displayHelp(%this)
{
   activatePackage(help);

      // Notes on logmode: console logging is written to console.log.
      // -log 0 disables console logging.
      // -log 1 appends to existing logfile; it also closes the file
      // (flushing the write buffer) after every write.
      // -log 2 overwrites any existing logfile; it also only closes
      // the logfile when the application shuts down.  (default)

   error(
      "Main command line options:\n"@
      "  -log <logmode>         Logging behavior; see main.cs comments for details\n"@
      "  -game <game_name>      Reset list of mods to only contain <game_name>\n"@
      "  <game_name>            Works like the -game argument\n"@
      "  -dir <dir_name>        Add <dir_name> to list of directories\n"@
      "  -console               Open a separate console\n"@
      "  -show <shape>          Deprecated\n"@
      "  -jSave  <file_name>    Record a journal\n"@
      "  -jPlay  <file_name>    Play back a journal\n"@
      "  -jDebug <file_name>    Play back a journal and issue an int3 at the end\n"@
      "  -help                  Display this help message\n"
   );
}


//--------------------------------------------------------------------------

// Default to a new logfile each session.
if( !$logModeSpecified )
{
   if( $platform !$= "xbox" && $platform !$= "xenon" )
      setLogMode(6);
}

// Get the first dir on the list, which will be the last to be applied... this
// does not modify the list.
nextToken($userDirs, currentMod, ";");

// Execute startup scripts for each mod, starting at base and working up
function Torque::loadDir(%this, %dir)
{
   pushback($userDirs, %dir, ";");

   if (isScriptFile(%dir @ "/main.cs"))
   exec(%dir @ "/main.cs");

   if ( isPackage( %dir ) )
      activatePackage( %dir );
}

echo("--------- Loading DIRS ---------");
function Torque::loadDirs(%this, %dirPath)
{
   %dirPath = nextToken(%dirPath, token, ";");
   if (%dirPath !$= "")
      %this.loadDirs(%dirPath);

   echo("Loading Directory:" SPC %dirPath);

   if(exec(%token @ "/main.cs") != true)
   {
      error("Error: Unable to find specified directory: " @ %token );
   }

   // Activate the directories package if any (This will not activate the core directory)
   if ( isPackage( %token ) )
      activatePackage( %token );
}
tge.loadDirs($userDirs);
echo("");

// Parse the command line arguments
echo("--------- Parsing Arguments ---------");
tge.parseArgs();

// Either display the help message or startup the app.
if ($displayHelp)
{
   enableWinConsole(true);
   tge.displayHelp();
   quit();
}
else
{
   tge.onStart();
   echo("Engine initialized...");

   if( !$isDedicated )
   {
      // As we know at this point that the initial load is complete,
      // we can hide any splash screen we have, and show the canvas.
      // This keeps things looking nice, instead of having a blank window
      closeSplashWindow();
      Canvas.showWindow();
   }
   
   // Auto-load on the 360
   if( $platform $= "xenon" )
   {
      %mission = "levels/Training_Grounds/TG_DesertRuins/TG_DesertRuins_day.mis";
      
      echo("Xbox360 Autoloading level: '" @ %mission @ "'");
      
      if ($pref::Server::Multiplayer)
         %serverType = "MultiPlayer";
      else
         %serverType = "SinglePlayer";

	  createAndConnectToLocalServer(%serverType, %mission, $pref::Server::MissionType);
   }
}

// Display an error message for unused arguments
for ($i = 1; $i < $Game::argc; $i++)
{
   if (!$argUsed[$i])
      error("Error: Unknown command line argument: " @ $Game::argv[$i]);
}

// Automatically start up the appropriate eidtor, if any
if ($startWorldEditor)
{
   Canvas.setCursor("DefaultCursor");
   Canvas.setContent(EditorChooseLevelGui);
}
else if ($startGUIEditor)
{
   Canvas.setCursor("DefaultCursor");
   Canvas.setContent(EditorChooseGUI);
}
