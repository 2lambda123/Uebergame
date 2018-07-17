
singleton Material(Mat_poplar_01_branch)
{
   mapTo = "poplar_01_branch";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "0.25 0.25 0.25 1";
   specularPower[0] = "1";
   translucent = "0";
   doubleSided = "1";
   alphaTest = "1";
   alphaRef = "40";
   subSurface[0] = "0";
   subSurfaceColor[0] = "0.286275 0.345098 0.109804 1";
   subSurfaceRolloff[0] = "0.2";
   useAnisotropic[0] = "1";
   showFootprints = "0";
   specular[1] = "1 1 1 1";
   accuEnabled[0] = "0";
   accuDirection[0] = "-1";
   diffuseMap[0] = "art/shapes/trees/poplar/poplar_01/poplar_01_branch_D.dds";
   normalMap[0] = "art/shapes/trees/poplar/poplar_01/poplar_01_branch_N.dds";
};

singleton Material(Mat_poplar_01_bark)
{
   mapTo = "poplar_01_bark";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "0.25 0.25 0.25 1";
   specularPower[0] = "1";
   translucentBlendOp = "None";
   diffuseMap[0] = "art/shapes/trees/poplar/poplar_01/poplar_01_bark_D.dds";
   normalMap[0] = "art/shapes/trees/poplar/poplar_01/poplar_01_bark_N.dds";
};
