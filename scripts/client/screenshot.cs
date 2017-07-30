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
// formatImageNumber
// Preceeds a number with zeros to make it 6 digits long.
//---------------------------------------------------------------------------------------------
function formatImageNumber(%number)
{
   if(%number < 10)
      %number = "0" @ %number;
   if(%number < 100)
      %number = "0" @ %number;
   if(%number < 1000)
      %number = "0" @ %number;
   //if(%number < 10000)
   //   %number = "0" @ %number;
   return %number;
}

//---------------------------------------------------------------------------------------------
// formatSessionNumber
// Preceeds a number with zeros to make it 4 digits long.
//---------------------------------------------------------------------------------------------
function formatSessionNumber(%number)
{
   if(%number < 10)
      %number = "0" @ %number;
   if(%number < 100)
      %number = "0" @ %number;
   return %number;
}

//---------------------------------------------------------------------------------------------
// recordMovie
// Records a movie file from the Canvas content using the specified fps.
// Possible encoder values are "PNG" and "THEORA" (default).
//---------------------------------------------------------------------------------------------
$movieNumber = 0;

$MovieEncodeActive = false;
function makeMovie(%movieName, %fps, %encoder)
{
   // If the canvas doesn't exist yet, setup a flag so it'll 
   // start capturing as soon as it's created
   if ( !isObject( Canvas ) )   
      return;

   if ( $pref::Video::movieSession $= "" )
      $pref::Video::movieSession = 0;
   
   if ( $movieNumber == 0 )
      $pref::Video::movieSession++;
            
   if ( $pref::Video::movieSession > 999 )
      $pref::Video::movieSession = 1;

   if ( %fps $= "" )
      %fps = 30;

   if ( %encoder $= "" ) 
      %encoder = "THEORA";

   %movieName = $HomePath @ "/movies/" @ "movie_" @ $Client::MissionName @ "_" @ formatSessionNumber($pref::Video::movieSession) @ "-" @ formatImageNumber($movieNumber);
   %movieName = expandFileName( %movieName );
   
   $movieNumber++;

   %resolution = Canvas.getVideoMode();
   startVideoCapture(Canvas, %movieName, %encoder, %fps);
   $MovieEncodeActive = true;
}

function stopMovie()
{
   stopVideoCapture();
   $MovieEncodeActive = false;
}

/// This is bound in initializeCommon() to take a screenshot on a keypress.
function doScreenShot( %val )
{
   // This can be bound, so skip key up events.
   if (%val)
   {
      _screenShot( 1 );
   }
}

/// A counter for screen shots used by _screenShot().
$screenshotNumber = 0;

/// Internal function which generates unique filename and triggers a screenshot capture.
function _screenShot( %tiles, %overlap )
{
   if ( $pref::Video::screenShotSession $= "" )
      $pref::Video::screenShotSession = 0;
            
   if ( $screenshotNumber == 0 )
      $pref::Video::screenShotSession++;
            
   if ( $pref::Video::screenShotSession > 999 )
      $pref::Video::screenShotSession = 1;
                  
   %name = "screenshot_" @ $Client::MissionName @ "_" @ formatSessionNumber($pref::Video::screenShotSession) @ "-" @ formatImageNumber($screenshotNumber);
   %name = expandFileName( %name );
   
   $screenshotNumber++;
   
   if ( $pref::Video::screenShotFormat $= "1" )
      screenShot( $HomePath @ "/screenshots/" @%name, "jpeg", %tiles, %overlap );      
   else
      screenShot( $HomePath @ "/screenshots/" @%name, "png", %tiles, %overlap );
}

/// This will close the console and take a large format screenshot by tiling the current backbuffer
/// and save it to the root game folder.
/// For instance a tile setting of 4 with a window set to 800x600 will output a 3200x2400 screenshot.
function tiledScreenShot( %tiles, %overlap )
{
   // Pop the console off before we take the shot.
   Canvas.popDialog( ConsoleDlg );
   
   _screenShot( %tiles, %overlap );
}
