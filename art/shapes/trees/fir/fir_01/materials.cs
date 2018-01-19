
singleton Material(Mat_fir_01_branch)
{
   mapTo = "fir_01_branch";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "0.25 0.25 0.25 1";
   specularPower[0] = "1";
   translucent = "0";
   diffuseMap[0] = "art/shapes/trees/fir/fir_01/fir_01_branch_D.dds";
   doubleSided = "1";
   alphaTest = "1";
   alphaRef = "80";
   subSurface[0] = "0";
   subSurfaceColor[0] = "0.286275 0.345098 0.109804 1";
   subSurfaceRolloff[0] = "0.2";
   normalMap[0] = "art/shapes/trees/fir/fir_01/fir_01_branch_N.dds";
   useAnisotropic[0] = "1";
   showFootprints = "0";
   specular[1] = "1 1 1 1";
   accuEnabled[0] = "1";
   accuDirection[0] = "-1";
};

singleton Material(Mat_fir_01_bark)
{
   mapTo = "fir_01_bark";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "0.25 0.25 0.25 1";
   specularPower[0] = "1";
   translucentBlendOp = "None";
   diffuseMap[0] = "art/shapes/trees/fir/fir_01/fir_01_bark_D.dds";
   normalMap[0] = "art/shapes/trees/fir/fir_01/fir_01_bark_N.dds";
};
