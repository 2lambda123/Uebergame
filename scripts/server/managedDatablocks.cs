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

// This is the default save location for any Datablocks created in the
// Datablock Editor (this script is executed from onServerCreated())

datablock PlayerData(TorqueSoldierPlayerData : DefaultSoldier)
{
   className = TorqueSoldier;
   availableSkins =  "base	olive	urban	desert	swamp	water	blue	red	green	yellow";
   mainWeapon = "Lurker";
   maxInvDeployableTurret = "1";
   maxInvProxMine = "4";
   maxInvLurkerGrenadeLauncher = "1";
   maxInvRyder = "1";
   maxInvLurker = "1";
   maxInvProxMineAmmo = "4";
   maxInvRyderClip = "6";
   maxInvLurkerClip = "6";
   maxInvLurkerGrenadeAmmo = "12";
};
   //SMS          ( Datablock, $SMS::ArmorName, Index)
   SmsInv.AddArmor( TorqueSoldierPlayerData, "Soldier", 1 );

datablock PlayerData(PaintballPlayerData : DefaultSoldier)
{  
   className = PaintballPlayer;
   shapeFile = "art/shapes/actors/paintball_player/paintball_player.dts";
   shapeNameFP[0] = "";
   availableSkins =  "base blue red green yellow";
   boundingBox = "0.75 0.75 1.8";
   crouchBoundingBox = "0.75 0.75 1.25";
   mainWeapon = "PaintballMarkerBlue";  
   renderFirstPerson = "1";
   maxInvPaintballClip = "10";
   maxInvPaintballMarkerRed = "1";
   maxInvPaintballMarkerYellow = "1";
   maxInvPaintballMarkerBlue = "1";
   maxInvPaintballMarkerGreen = "1";
};
   //SMS          ( Datablock, $SMS::ArmorName, Index)
   SmsInv.AddArmor( PaintballPlayerData, "Paintballer", 2 );
