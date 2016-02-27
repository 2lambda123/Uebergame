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

function vehicleHud::onWake( %this )
{
   VehiclesMenu.clear();
   for(%i = 0; %i < getFieldCount($Client::VehicleList); %i++)
      VehiclesMenu.add(getField($Client::VehicleList, %i), %i);

   %id = VehiclesMenu.findText($pref::Player::SelectedVehicle);
   if(%id == -1)
      %id = 0;

   VehiclesMenu.setSelected(%id);
   //VehiclesMenu.onSelect(%id, "");

   if ( isObject( hudMap ) )
   {
      hudMap.pop();
      hudMap.delete();
   }
   new ActionMap( hudMap );
   hudMap.blockBind( moveMap, toggleArmoryDlg );
   hudMap.blockBind( moveMap, bringUpOptions );
   hudMap.blockBind( moveMap, showPlayerList );
   // If escape is mapped to moveMap this will work fine else it will crash :)
   //hudMap.bindCmd( keyboard, "escape", "", "Canvas.popDialog(%this);" );
   hudMap.push();
}

function vehicleHud::onSleep( %this )
{
   // Make sure the proper key maps are pushed
   tge.updateKeyMaps(); 
}

function VehiclesMenu::onSelect( %this, %id, %text )
{
   if(%text $= "") // Only select if it was the mouse that called this
      return;

   commandToServer('SelectVehicle', addTaggedString(%text));
}

function clientCmdSetVehicleCount( %v1, %v2, %v3 )
{
   echo("Empty");
}

function clientCmdSelectedVehicle(%vehicle, %bitmap)
{
   $pref::Player::SelectedVehicle = deTag(%vehicle);
   ChatHud.AddLine( "Vehicle \c4\"" @ $pref::Player::SelectedVehicle @ "\" \c0selected." );
}

function clientCmdGetVehicleList(%list)
{
   $Client::VehicleList = deTag(%list);
   //error("clientCmdGetVehicleList(" SPC $Client::VehicleList SPC ")");
}

function clientCmdGetVBitmapList(%taggedlist)
{
   %list = deTag(%taggedlist);

   // First clear out the old
   for ( %i = 0; $VehicleBitmap[%i] !$=""; %i++ )
      $VehicleBitmap[%i] = "";

   // Now rebuild the list
   for ( %i = 0; %i < getFieldCount(%list); %i++ )
      $VehicleBitmap[%i] = getField(%list, %i);
}
