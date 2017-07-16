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

singleton Material(DefaultDecalRoadMaterial)
{
   diffuseMap[0] = "art/roads/defaultRoadTextureTop.png";
   mapTo = "unmapped_mat";
   materialTag0 = "RoadAndPath";
};

singleton Material(DefaultRoadMaterialTop)
{
   mapTo = "unmapped_mat";
   diffuseMap[0] = "art/roads/defaultRoadTextureTop.png";
   materialTag0 = "RoadAndPath";
};

singleton Material(DefaultRoadMaterialOther)
{
   mapTo = "unmapped_mat";
   diffuseMap[0] = "art/roads/defaultRoadTextureOther.png";
   materialTag0 = "RoadAndPath";
};

singleton Material(Mat_road_asphalt_small_01)
{
   mapTo = "road_asphalt_small_01";
   diffuseMap[0] = "art/roads/road_asphalt_small_01_D.dds";
   materialTag0 = "RoadAndPath";
   normalMap[0] = "art/roads/road_asphalt_small_01_N.dds";
   specularPower[0] = "1";
   specularStrength[0] = "0.5";
   specularMap[0] = "art/roads/road_asphalt_small_01_S.dds";
   useAnisotropic[0] = "1";
   alphaTest = "0";
   alphaRef = "128";
   showFootprints = "0";
   footstepSoundId = "0";
   impactSoundId = "0";
   translucent = "1";
   translucentZWrite = "1";
   castShadows = "0";
};

singleton Material(Mat_road_asphalt_stripes_01)
{
   mapTo = "road_asphalt_stripes_01";
   diffuseMap[0] = "art/roads/road_asphalt_stripes_01_D.dds";
   materialTag0 = "RoadAndPath";
   normalMap[0] = "art/roads/road_asphalt_stripes_01_N.dds";
   specularMap[0] = "art/roads/road_asphalt_stripes_01_S.dds";
   useAnisotropic[0] = "1";
   castShadows = "0";
   translucentBlendOp = "LerpAlpha";
   alphaTest = "0";
   alphaRef = "128";
   showFootprints = "0";
   footstepSoundId = "0";
   impactSoundId = "0";
   specularPower[0] = "1";
   specularStrength[0] = "0.5";
   effectColor[0] = "InvisibleBlack";
   translucent = "1";
   translucentZWrite = "1";
};

singleton Material(Mat_road_sand_01)
{
   mapTo = "road_sand_01";
   diffuseMap[0] = "art/roads/road_sand_01_D.dds";
   specularPower[0] = "8";
   specularStrength[0] = "1";
   useAnisotropic[0] = "1";
   alphaTest = "0";
   alphaRef = "0";
   showFootprints = "1";
   footstepSoundId = "1";
   impactSoundId = "1";
   materialTag0 = "RoadAndPath";
   translucent = "1";
   castShadows = "0";
   normalMap[0] = "art/roads/road_sand_01_N.dds";
   specularMap[0] = "art/roads/road_sand_01_S.dds";
   specular[2] = "1 1 1 1";
};

singleton Material(Mat_road_sand_01_wet)
{
   mapTo = "road_sand_01_wet";
   diffuseMap[0] = "art/roads/road_sand_01_wet_D.dds";
   normalMap[0] = "art/roads/road_sand_01_wet_N.dds";
   specularPower[0] = "1";
   specularStrength[0] = "0.5";
   useAnisotropic[0] = "1";
   alphaTest = "0";
   alphaRef = "128";
   showFootprints = "1";
   footstepSoundId = "0";
   impactSoundId = "0";
   materialTag0 = "RoadAndPath";
   translucent = "1";
   castShadows = "0";
   specularMap[0] = "art/roads/road_sand_01_wet_S.dds";
};

singleton Material(Mat_shore_debris_01)
{
   mapTo = "shore_debris_01";
   diffuseMap[0] = "art/roads/shore_debris_01_D.dds";
   normalMap[0] = "art/roads/shore_debris_01_N.dds";
   specularPower[0] = "1";
   specularStrength[0] = "2";
   useAnisotropic[0] = "1";
   alphaTest = "1";
   alphaRef = "100";
   showFootprints = "1";
   footstepSoundId = "0";
   impactSoundId = "0";
   materialTag0 = "RoadAndPath";
   translucent = "1";
   castShadows = "0";
   specularMap[0] = "art/roads/shore_debris_01_S.dds";
   translucentZWrite = "0";
};

singleton Material(Mat_trail_dirt_01)
{
   mapTo = "trail_dirt_01";
   diffuseMap[0] = "art/roads/trail_dirt_01_D.dds";
   normalMap[0] = "art/roads/trail_dirt_01_N.dds";
   specularMap[0] = "art/roads/trail_dirt_01_S.dds";
   specularPower[0] = "1";
   specularStrength[0] = "0.5";
   useAnisotropic[0] = "1";
   alphaTest = "0";
   alphaRef = "128";
   showFootprints = "1";
   footstepSoundId = "0";
   impactSoundId = "0";
   materialTag0 = "RoadAndPath";
   translucent = "1";
   castShadows = "0";
};
