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
   baseTex[0] = "./scorch_decal.png";

   translucent = true;
   translucentBlendOp = None;
   translucentZWrite = true;
   alphaTest = true;
   alphaRef = 84;
};

singleton Material(DECAL_RocketEXP)
{
   translucent = true;
   translucentBlendOp = "LerpAlpha";
   translucentZWrite = true;
   alphaTest = "0";
   alphaRef = "1";
   mapTo = "rBlast";
   diffuseMap[0] = "art/decals/rBlast";
   materialTag0 = "decal";
   specular[0] = "0.501961 0.501961 0.501961 1";
   specularPower[0] = "64";
   specularStrength[0] = "0.35";
   pixelSpecular[0] = "1";
};

singleton Material(DECAL_bulletHole)
{
   baseTex[0] = "art/decals/Bullet Holes/BulletHole_Walls.dds";

   vertColor[0] = true;
   translucent = true;
   translucentBlendOp = LerpAlpha;
   translucentZWrite = true;
};

singleton Material(DECAL_defaultblobshadow)
{
   baseTex[0] = "art/decals/defaultblobshadow";

   translucent = true;
   translucentBlendOp = LerpAlpha;
   translucentZWrite = true;
   alphaTest = false;
   //alphaRef = 64;
   //emissive[0] = "1";
};

singleton Material(DECAL_blood_splatter_01)
{

   vertColor[0] = true;
   translucent = true;
   translucentBlendOp = LerpAlpha;
   translucentZWrite = true;
   mapTo = "DECAL_blood_splatter_01";
   diffuseMap[0] = "art/decals/blood_splatter_01_a.png";
   alphaTest = "0";
   alphaRef = "172";
   materialTag0 = "Decal";
   specular[0] = "0.494118 0.054902 0.117647 1";
   pixelSpecular[0] = "1";
   specularPower[0] = "32";
   specular[1] = "1 1 1 1";
   diffuseColor[0] = "White";
   castShadows = "0";
};

