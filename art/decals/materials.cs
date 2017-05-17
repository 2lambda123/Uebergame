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

singleton Material(DECAL_scorch)
{
   baseTex[0] = "art/decals/scorch_decal.png";
   materialTag0 = "decal";
   translucent = true;
   translucentBlendOp = None;
   translucentZWrite = true;
   alphaTest = true;
   alphaRef = 84;
   castShadows = "0";
   showFootprints = "0";
   vertColor[0] = true;
};

singleton Material(DECAL_RocketEXP)
{
   translucent = true;
   translucentBlendOp = "LerpAlpha";
   translucentZWrite = true;
   alphaTest = "0";
   alphaRef = "1";
   mapTo = "rBlast";
   diffuseMap[0] = "art/decals/rBlast.png";
   materialTag0 = "decal";
   specular[0] = "0.501961 0.501961 0.501961 1";
   specularPower[0] = "64";
   specularStrength[0] = "0.35";
   pixelSpecular[0] = "1";
   castShadows = "0";
   showFootprints = "0";
   vertColor[0] = true;
};

singleton Material(DECAL_bulletHole)
{
   translucent = true;
   translucentBlendOp = LerpAlpha;
   translucentZWrite = true;
   mapTo = "bullet_hole_01.dds";
   diffuseMap[0] = "art/decals/bulletHoles/bullet_hole_01.dds";
   materialTag0 = "decal";
   castShadows = "0";
   showFootprints = "0";
   vertColor[0] = true;
};

singleton Material(DECAL_defaultblobshadow)
{
   baseTex[0] = "art/decals/defaultblobshadow";
   materialTag0 = "decal";
   translucent = true;
   translucentBlendOp = LerpAlpha;
   translucentZWrite = true;
   alphaTest = false;
   //alphaRef = 64;
   //emissive[0] = "1";
   castShadows = "0";
   showFootprints = "0";
   vertColor[0] = true;
};

singleton Material(DECAL_blood_splatter_01)
{
   translucent = true;
   translucentBlendOp = LerpAlpha;
   translucentZWrite = true;
   mapTo = "DECAL_blood_splatter_01";
   diffuseMap[0] = "art/decals/blood_splatter_01.png";
   alphaTest = "0";
   alphaRef = "172";
   materialTag0 = "decal";
   specular[0] = "0.494118 0.054902 0.117647 1";
   pixelSpecular[0] = "1";
   specularPower[0] = "32";
   specular[1] = "1 1 1 1";
   diffuseColor[0] = "White";
   castShadows = "0";
   showFootprints = "0";
   vertColor[0] = true;
};

singleton Material(DECAL_paint_splatter_01_red)
{
   translucent = "1";
   translucentBlendOp = LerpAlpha;
   translucentZWrite = "1";
   mapTo = "DECAL_paint_splatter_01_red";
   diffuseMap[0] = "art/decals/paintball/pb_splatter_01_D.dds";
   alphaTest = "0";
   alphaRef = "1";
   materialTag0 = "decal";
   specular[0] = "0.996078 0.333333 0.156863 1";
   pixelSpecular[0] = "0";
   specularPower[0] = "32";
   specular[1] = "1 1 1 1";
   diffuseColor[0] = "1 0.3 0 1";
   subSurface[0] = "0";
   subSurfaceColor[0] = "1 0.2 0.2 1";
   subSurfaceRolloff[0] = "0.2";
   specularStrength[0] = "0.5";
   specularMap[0] = "art/decals/paintball/pb_splatter_01_D.dds";
   castShadows = "0";
   showFootprints = "0";
   vertColor[0] = true;
};

singleton Material(DECAL_paint_splatter_01_blue)
{
   translucent = "1";
   translucentBlendOp = LerpAlpha;
   translucentZWrite = "1";
   mapTo = "DECAL_paint_splatter_01_blue";
   diffuseMap[0] = "art/decals/paintball/pb_splatter_01_D.dds";
   alphaTest = "0";
   alphaRef = "1";
   materialTag0 = "decal";
   specular[0] = "0.243137 0.454902 0.996078 1";
   pixelSpecular[0] = "0";
   specularPower[0] = "32";
   specular[1] = "1 1 1 1";
   diffuseColor[0] = "0.223529 0.345098 0.952941 1";
   subSurface[0] = "0";
   subSurfaceColor[0] = "1 0.2 0.2 1";
   subSurfaceRolloff[0] = "0.2";
   specularStrength[0] = "0.5";
   specularMap[0] = "art/decals/paintball/pb_splatter_01_D.dds";
   castShadows = "0";
   showFootprints = "0";
   vertColor[0] = true;
};

singleton Material(DECAL_paint_splatter_01_green)
{
   translucent = "1";
   translucentBlendOp = LerpAlpha;
   translucentZWrite = "1";
   mapTo = "DECAL_paint_splatter_01_green";
   diffuseMap[0] = "art/decals/paintball/pb_splatter_01_D.dds";
   alphaTest = "0";
   alphaRef = "1";
   materialTag0 = "decal";
   specular[0] = "0.337255 0.996078 0.431373 1";
   pixelSpecular[0] = "0";
   specularPower[0] = "32";
   specular[1] = "1 1 1 1";
   diffuseColor[0] = "0.298039 0.929412 0.25098 1";
   subSurface[0] = "0";
   subSurfaceColor[0] = "1 0.2 0.2 1";
   subSurfaceRolloff[0] = "0.2";
   specularStrength[0] = "0.5";
   specularMap[0] = "art/decals/paintball/pb_splatter_01_D.dds";
   castShadows = "0";
   showFootprints = "0";
   vertColor[0] = true;
};

singleton Material(DECAL_paint_splatter_01_yellow)
{
   translucent = "1";
   translucentBlendOp = LerpAlpha;
   translucentZWrite = "1";
   mapTo = "DECAL_paint_splatter_01_yellow";
   diffuseMap[0] = "art/decals/paintball/pb_splatter_01_D.dds";
   alphaTest = "0";
   alphaRef = "1";
   materialTag0 = "decal";
   specular[0] = "0.996078 0.901961 0.00784314 1";
   pixelSpecular[0] = "0";
   specularPower[0] = "1";
   specular[1] = "1 1 1 1";
   diffuseColor[0] = "0.941177 0.85098 0.00784314 1";
   subSurface[0] = "0";
   subSurfaceColor[0] = "1 0.2 0.2 1";
   subSurfaceRolloff[0] = "0.2";
   specularStrength[0] = "0.5";
   specularMap[0] = "art/decals/paintball/pb_splatter_01_D.dds";
   castShadows = "0";
   showFootprints = "0";
   vertColor[0] = true;
};

singleton Material(decal_text_rote_basis_01)
{
   mapTo = "decal_text_rote_basis_01";
   diffuseMap[0] = "art/decals/teams/decal_text_rote_basis_01";
   diffuseColor[0] = "0.996 0.2 0.02 1";
   useAnisotropic[0] = "1";
   specular[0] = "0.996 0.671 0.514 1";
   specularPower[0] = "1";
   specularStrength[0] = "2";
   pixelSpecular[0] = "1";
   translucent = "1";
   translucentBlendOp = LerpAlpha;
   translucentZWrite = "1";
   materialTag0 = "decal";
};

singleton Material(decal_text_blaue_basis_01)
{
   mapTo = "decal_text_blaue_basis_01";
   diffuseMap[0] = "art/decals/teams/decal_text_blaue_basis_01";
   diffuseColor[0] = "0.216 0.482 0.996 1";
   useAnisotropic[0] = "1";
   specular[0] = "0.576 0.753 0.996 1";
   specularPower[0] = "1";
   specularStrength[0] = "2";
   pixelSpecular[0] = "1";
   translucent = "1";
   translucentBlendOp = LerpAlpha;
   translucentZWrite = "1";
   materialTag0 = "decal";
};
