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

// This is the default save location for any Decal datablocks created in the
// Decal Editor (this script is executed from onServerCreated())

datablock DecalData(ScorchBigDecal)
{
   Material = "DECAL_scorch";
   size = "4.0";
   lifeSpan = "60000";
};

datablock DecalData(ScorchRXDecal)
{
   Material = "DECAL_RocketEXP";
   size = "3.0";
   lifeSpan = "10000";
   fadeTime = "50000";
   randomize = "1";
   texRows = "2";
   texCols = "2";
   clippingAngle = "66";
   screenStartRadius = "200";
   screenEndRadius = "100";
};

datablock DecalData(BulletHoleDecal)
{
   Material = "DECAL_bulletHole";
   size = "0.2";
   lifeSpan = "10000";
   fadeTime = "50000";
   randomize = "1";
   texRows = "2";
   texCols = "2";
   screenStartRadius = "20";
   screenEndRadius = "5";
   clippingAngle = "66";
};

datablock DecalData(bloodDecalData)  
{  
   Material = "DECAL_blood_splatter_01";  
   size = "1";
   lifeSpan = "10000";
   fadeTime = "50000";
   randomize = "1";
   texRows = "2";
   texCols = "2";
   clippingAngle = "66";
   screenStartRadius = "200";
   screenEndRadius = "100";  
}; 

datablock DecalData(redPaintSplatterDecal)  
{  
   Material = "DECAL_paint_splatter_01_red";  
   size = "1";
   lifeSpan = "10000";
   fadeTime = "50000";
   randomize = "1";
   texRows = "4";
   texCols = "4";
   clippingAngle = "66";
   screenStartRadius = "200";
   screenEndRadius = "100";  
};

datablock DecalData(bluePaintSplatterDecal)  
{  
   Material = "DECAL_paint_splatter_01_blue";  
   size = "1";
   lifeSpan = "10000";
   fadeTime = "50000";
   randomize = "1";
   texRows = "4";
   texCols = "4";
   clippingAngle = "66";
   screenStartRadius = "200";
   screenEndRadius = "100";  
}; 

datablock DecalData(greenPaintSplatterDecal)  
{  
   Material = "DECAL_paint_splatter_01_green";  
   size = "1";
   lifeSpan = "10000";
   fadeTime = "50000";
   randomize = "1";
   texRows = "4";
   texCols = "4";
   clippingAngle = "66";
   screenStartRadius = "200";
   screenEndRadius = "100";  
}; 

datablock DecalData(yellowPaintSplatterDecal)  
{  
   Material = "DECAL_paint_splatter_01_yellow";  
   size = "1";
   lifeSpan = "10000";
   fadeTime = "50000";
   randomize = "1";
   texRows = "4";
   texCols = "4";
   clippingAngle = "66";
   screenStartRadius = "200";
   screenEndRadius = "100";  
}; 

datablock DecalData(text_blaue_basis_01)  
{  
   Material = "decal_text_blaue_basis_01";  
   size = "3";
   lifeSpan = "10000";
   fadeTime = "50000";
   clippingAngle = "66";
   screenStartRadius = "200";
   screenEndRadius = "100";  
   textureCoordCount = "0";
};

datablock DecalData(text_rote_basis_01)  
{  
   Material = "decal_text_rote_basis_01";  
   size = "3";
   lifeSpan = "10000";
   fadeTime = "50000";
   clippingAngle = "66";
   screenStartRadius = "200";
   screenEndRadius = "100";  
   textureCoordCount = "0";
}; 
