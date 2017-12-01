
singleton Material(Mat_canopy_01_bark)
{
   mapTo = "canopy_01_bark";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "9 9 9 1";
   specularPower[0] = "50";
   translucentBlendOp = "None";
   diffuseMap[0] = "art/shapes/trees/canopy/canopy_01/canopy_01_bark_D.dds";
   normalMap[0] = "art/shapes/trees/canopy/canopy_01/canopy_01_bark_N.dds";
   specularMap[0] = "art/shapes/trees/canopy/canopy_01/canopy_01_bark_S.dds";
   detailScale[0] = "6 6";
   detailNormalMapStrength[0] = "3";
};

singleton Material(Mat_canopy_01_branch)
{
   mapTo = "canopy_01_branch";
   diffuseColor[0] = "1 1 1 1";
   diffuseColor[1] = "White";
   specular[0] = "9 9 9 1";
   specularPower[0] = "50";
   translucentBlendOp = "None";
   diffuseMap[0] = "art/shapes/trees/canopy/canopy_01/canopy_01_branch_D.dds";
   normalMap[0] = "art/shapes/trees/canopy/canopy_01/canopy_01_branch_N.dds";
   specularMap[0] = "art/shapes/trees/canopy/canopy_01/canopy_01_branch_S.dds";
   alphaTest = "1";
   alphaRef = "110";
   showFootprints = "0";
   useAnisotropic[0] = "1";
};

singleton Material(Mat_canopy_01_extras)
{
   mapTo = "canopy_01_extras";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "9 9 9 1";
   specularPower[0] = "50";
   translucentBlendOp = "None";
   diffuseMap[0] = "art/shapes/trees/canopy/canopy_01/canopy_01_extras_D.dds";
   normalMap[0] = "art/shapes/trees/canopy/canopy_01/canopy_01_extras_N.dds";
   specularMap[0] = "art/shapes/trees/canopy/canopy_01/canopy_01_extras_S.dds";
   alphaTest = "1";
   alphaRef = "10";
   useAnisotropic[0] = "1";
};

singleton Material(Mat_canopy_01_LOD)
{
   mapTo = "canopy_01_LOD";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "9 9 9 1";
   specularPower[0] = "50";
   translucentBlendOp = "None";
   diffuseMap[0] = "art/shapes/trees/canopy/canopy_01/canopy_01_LOD.dds";
   alphaTest = "1";
   alphaRef = "110";
};
