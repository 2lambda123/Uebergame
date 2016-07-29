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

singleton Material(Mat_Soldier_Main)
{
   mapTo = "base_Soldier_Main";

   diffuseMap[0] = "art/shapes/actors/Soldier/Soldier_D.dds";
   normalMap[0] = "art/shapes/actors/Soldier/Soldier_N.dds";
   specularMap[0] = "art/shapes/actors/Soldier/Soldier_S.dds";

   diffuseColor[0] = "1 1 1 1";
   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = "1";

   doubleSided = false;
   translucent = false;
   showFootprints = "0";
   materialTag0 = "Player";
   useAnisotropic[0] = "0";
   specularStrength[0] = "0.784314";
   castDynamicShadows = "0";
};

singleton Material(Mat_Soldier_Dazzle)
{
   mapTo = "base_Soldier_Dazzle";

   diffuseMap[0] = "Soldier_Dazzle.dds";

   diffuseColor[0] = "0.196078 0.196078 0.196078 1";
   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = "1";

   doubleSided = false;
   translucent = "1";
   translucentBlendOp = "Add";
   glow[0] = "0";
   emissive[0] = "0";
   castShadows = "0";
   showFootprints = "0";
   materialTag0 = "Player";
};

singleton Material(soldier_rigged_BoundsMaterial)
{
   mapTo = "BoundsMaterial";
   diffuseColor[0] = "0.0705882 1 0 0.27";
   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = "10";
   translucent = "1";
};

singleton Material(soldier_rigged_ShapeBounds)
{
   mapTo = "ShapeBounds";
   diffuseColor[0] = "0.164706 1 0 1";
   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = "10";
   translucentBlendOp = "None";
   materialTag0 = "Player";
};

//-----------------------------------------------------------------------------
// Soldier Skins
// Add names to PlayerData.availableSkins list in art/datablock/player.cs
singleton Material(Mat_olive_Soldier_Main : Mat_Soldier_Main)
{
   mapTo = "olive_Soldier_Main";
   diffuseMap[0] = "Soldier_olive_D.dds";
};

singleton Material(Mat_urban_Soldier_Main : Mat_Soldier_Main)
{
   mapTo = "urban_Soldier_Main";
   diffuseMap[0] = "Soldier_urban_D.dds";
};

singleton Material(Mat_desert_Soldier_Main : Mat_Soldier_Main)
{
   mapTo = "desert_Soldier_Main";
   diffuseMap[0] = "Soldier_desert_D.dds";
};

singleton Material(Mat_swamp_Soldier_Main : Mat_Soldier_Main)
{
   mapTo = "swamp_Soldier_Main";
   diffuseMap[0] = "Soldier_swamp_D.dds";
};

singleton Material(Mat_water_Soldier_Main : Mat_Soldier_Main)
{
   mapTo = "water_Soldier_Main";
   diffuseMap[0] = "Soldier_water_D.dds";
};

singleton Material(Mat_blue_Soldier_Main : Mat_Soldier_Main)
{
   mapTo = "blue_Soldier_Main";
   diffuseMap[0] = "Soldier_blue_D.dds";
};

singleton Material(Mat_red_Soldier_Main : Mat_Soldier_Main)
{
   mapTo = "red_Soldier_Main";
   diffuseMap[0] = "Soldier_red_D.dds";
};

singleton Material(Mat_green_Soldier_Main : Mat_Soldier_Main)
{
   mapTo = "green_Soldier_Main";
   diffuseMap[0] = "Soldier_green_D.dds";
};

singleton Material(Mat_yellow_Soldier_Main : Mat_Soldier_Main)
{
   mapTo = "yellow_Soldier_Main";
   diffuseMap[0] = "Soldier_yellow_D.dds";
};

