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

function PlayGui::onWake(%this)
{
   // Turn off any shell sounds...
   // alxStop( ... );

   $enableDirectInput = "1";
   activateDirectInput();

   // Message hud dialog
   if ( isObject( MainChatHud ) )
   {
      Canvas.pushDialog( MainChatHud );
      chatHud.attach(HudMessageVector);
   }

   // Add the cluster and objective huds.
   // Adding keeps them in the foreground in case we are zoomed.
   //%this.add(ClusterHud);

   // Add the FireTeam hud
   %this.add(FireTeamHud);

   // just update the action maps here
   tge.updateKeyMaps();

   // hack city - some controls are floating around and need to be clamped
   %this.schedule( 50, "refreshElements" );
}

function PlayGui::onSleep(%this)
{
   // pop the keymaps
   if (isObject(moveMap))
      moveMap.pop();

   if (isObject(spectatorBlockMap))
      spectatorBlockMap.pop();

   if (isObject(spectatorMap))
      spectatorMap.pop();

   if (isObject(vehicleMap))
      vehicleMap.pop();

   if (isObject(passengerKeys))
   {
      passengerKeys.pop();
      passengerKeys.delete();
   }

   if (isObject(hudMap))
   {
      hudMap.pop();
      hudMap.delete();
   }

   //chatHud.detach(HudMessageVector);
}

function PlayGui::clearHud( %this )
{
   Canvas.popDialog( MainChatHud );

   while ( %this.getCount() > 0 )
      %this.getObject( 0 ).delete();
}

//-----------------------------------------------------------------------------

function PlayGui::refreshElements(%this)
{
   // Find center
   %cX = getWord( PlayGui.getGlobalCenter(), 0 );
   %cY = getWord( PlayGui.getGlobalCenter(), 1 );
   //echo( "PLAYGUI CENTER:" SPC %cX SPC %cY );

   %rX = getWord( reticle.getExtent(), 0 ) * 0.5;
   %rY = getWord( reticle.getExtent(), 1 ) * 0.5;
   //echo( "RETICLE HALF EXTENT:" SPC %rX SPC %rY );

   %subX = %cX - %rX;
   %subY = %cY - $rY;
   //echo( "NEW RETICLE POSITION:" SPC %subX SPC %subY );
   //reticle.setPosition( %subX, %subY );


   //%x = ( getWord( Canvas.getVideoMode(), 0 ) * 0.5 );
   //%y = ( getWord( Canvas.getVideoMode(), 1 ) * 0.5 );
   //echo( "CANVAS CENTER:" SPC %x SPC %y );

   zoomReticle.setPosition( 0, 0 );
   zoomReticle.extent = getWord(Canvas.getVideoMode(), 0) SPC getWord(Canvas.getVideoMode(), 1);
   BottomPrintText.position = "0 0";
   CenterPrintText.position = "0 0";
   //HudRadar.position = "10 10";
   //LagIcon.position = "10 165";
}

function PlayGui::onMouseUp(%this){ }
function PlayGui::onMouseDown(%this){ }
function PlayGui::onMouseMove(%this){ }
function PlayGui::onMouseDragged(%this){ }
function PlayGui::onMouseEnter(%this){ }
function PlayGui::onMouseLeave(%this){ }

function PlayGui::onMouseWheelUp(%this){ }
function PlayGui::onMouseWheelDown(%this){ }
function PlayGui::onRightMouseDown(%this){ }
function PlayGui::onRightMouseUp(%this){ }
function PlayGui::onRightMouseDragged(%this){ }
function PlayGui::onMiddleMouseDown(%this){ }
function PlayGui::onMiddleMouseUp(%this){ }
function PlayGui::onMiddleMouseDragged(%this){ }
/*
function PlayGui::onRightMouseDown(%this, %pos, %start, %ray)
{   
   if( Canvas.isCursorOn() )
      Canvas.cursorOff();
   else
      Canvas.cursorOn();

   if( Canvas.isCursorShown() )
      Canvas.hideCursor();
   else
      Canvas.showCursor();
}
*/