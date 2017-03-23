
singleton Material(birch_01_a_birch_01_bark)
{
   mapTo = "birch_01_bark";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "0 0 0 1";
   specularPower[0] = "50";
   doubleSided = "0";
   translucentBlendOp = "None";
   diffuseMap[0] = "art/shapes/trees/birch/birch_01/birch_01_bark_D.dds";
   showFootprints = "0";
   detailMap[0] = "art/shapes/trees/birch/birch_01/birch_01_bark_detail_D.dds";
   detailScale[0] = "4 4";
   normalMap[0] = "art/shapes/trees/birch/birch_01/birch_01_bark_N.dds";
   detailNormalMapStrength[0] = "4";
   specularMap[0] = "art/shapes/trees/birch/birch_01/birch_01_bark_D.dds";
};

singleton Material(birch_01_a_birch_01_leaves)
{
   mapTo = "birch_01_leaves";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "0 0 0 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucent = "0";
   diffuseMap[0] = "art/shapes/trees/birch/birch_01/birch_01_leaves_D.dds";
   accuSpecular[0] = "0.9";
   alphaTest = "1";
   alphaRef = "100";
   useAnisotropic[0] = "1";
   subSurface[0] = "0";
   showFootprints = "0";
   normalMap[0] = "art/shapes/trees/birch/birch_01/birch_01_leaves_N.dds";
   accuCoverage[0] = "1";
};
