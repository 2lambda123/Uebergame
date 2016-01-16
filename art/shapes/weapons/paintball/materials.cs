
singleton Material(Mat_paintball_ball)
{
   mapTo = "paintball_ball";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   translucentBlendOp = "None";
   vertColor[0] = "1";
   castShadows = "0";
   showFootprints = "0";
   materialTag0 = "Weapons";
};

singleton Material(Mat_paintball_marker_01)
{
   mapTo = "paintball_marker_01";
   diffuseColor[0] = "1 1 1 1";
   translucentBlendOp = "None";
   diffuseMap[0] = "art/shapes/weapons/paintball/paintball_marker_01_D.dds";
   specularMap[0] = "art/shapes/weapons/paintball/paintball_marker_01_S.dds";
   vertColor[0] = "1";
   normalMap[0] = "art/shapes/weapons/paintball/paintball_marker_01_N.dds";
   specularPower[0] = "16";
   specularStrength[0] = "0.7";
};
