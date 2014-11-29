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

singleton Material(Mat_FP_Soldier_Arms_Main)
{
   mapTo = "base_FP_Soldier_Arms_Main";
   diffuseMap[0] = "art/shapes/actors/Soldier/FP/FP_SoldierArms_D";
   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = "10";
   translucentBlendOp = "None";
   normalMap[0] = "art/shapes/actors/Soldier/FP/FP_SoldierArms_N.dds";
   specularMap[0] = "art/shapes/actors/Soldier/FP/FP_SoldierArms_S.dds";
};

//-----------------------------------------------------------------------------
// Soldier Skins
// Add names to PlayerData.availableSkins list in art/datablock/player.cs
singleton Material(Mat_olive_FP_Soldier_Arms_Main : Mat_FP_Soldier_Arms_Main)
{
   mapTo = "olive_FP_Soldier_Arms_Main";
   diffuseMap[0] = "FP_SoldierArms_olive_D.dds";
};

singleton Material(Mat_urban_FP_Soldier_Arms_Main : Mat_FP_Soldier_Arms_Main)
{
   mapTo = "urban_FP_Soldier_Arms_Main";
   diffuseMap[0] = "FP_SoldierArms_urban_D.dds";
};

singleton Material(Mat_desert_FP_Soldier_Arms_Main : Mat_FP_Soldier_Arms_Main)
{
   mapTo = "desert_FP_Soldier_Arms_Main";
   diffuseMap[0] = "FP_SoldierArms_desert_D.dds";
};

singleton Material(Mat_swamp_FP_Soldier_Arms_Main : Mat_FP_Soldier_Arms_Main)
{
   mapTo = "swamp_FP_Soldier_Arms_Main";
   diffuseMap[0] = "FP_SoldierArms_swamp_D.dds";
};

singleton Material(Mat_water_FP_Soldier_Arms_Main : Mat_FP_Soldier_Arms_Main)
{
   mapTo = "water_FP_Soldier_Arms_Main";
   diffuseMap[0] = "FP_SoldierArms_water_D.dds";
};