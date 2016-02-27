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

// A very simple video player.

//---------------------------------------------------------------------------------------------
// Prerequisites.

if( !isObject( GuiVideoPlayer ) )
   exec( "./guiVideoPlayer.gui" );

//---------------------------------------------------------------------------------------------
// Functions.

function toggleVideoPlayer(%val)
{
   if ( %val )
   {
      if( !GuiVideoPlayer.isAwake() )
      {
         GuiVideoPlayer.setExtent( Canvas.getExtent() );
         GuiVideoPlayer.setPosition( 0, 0 );

         Canvas.pushDialog( GuiVideoPlayer );
      }
      else
         Canvas.popDialog( GuiVideoPlayer );
   }
}

//---------------------------------------------------------------------------------------------
// Methods.

function GuiVideoPlayer::onWake( %this )
{
   GuiMusicPlayerMusicList.load();

   //TheoraVideo.setFile( "art/movies/file.ogv);
   //TheoraVideo.play();
   //TheoraVideo.pause();
   //TheoraVideo.stop();
   //TheoraVideo.getCurrentTime();
   //TheoraVideo.isPlaybackDone();

   if ( isObject( hudMap ) )
   {
      hudMap.pop();
      hudMap.delete();
   }
   new ActionMap( hudMap );
   hudMap.blockBind( moveMap, bringUpOptions );
   hudMap.blockBind( moveMap, toggleTeamChoose );
   hudMap.blockBind( moveMap, showScoreBoard );
   hudMap.push();
}

function GuiMusicPlayer::onSleep(%this)
{
   TheoraVideo.stop();
   // Make sure the proper key maps are pushed
   tge.updateKeyMaps();
}
