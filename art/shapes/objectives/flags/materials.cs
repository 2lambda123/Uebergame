
singleton Material(Mat_flag_01)
{
   mapTo = "base_flag_01";
   diffuseColor[0] = "1 1 1 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "1";
   translucentBlendOp = "None";
   diffuseMap[0] = "art/shapes/objectives/flags/flag_01_D.dds";
   normalMap[0] = "art/shapes/objectives/flags/flag_01_N.dds";
   specularMap[0] = "art/shapes/objectives/flags/flag_01_S.dds";
   specularStrength[0] = "0.8";
   showFootprints = "0";
   castDynamicShadows = "1";
   useAnisotropic[0] = "1";
};

singleton Material(Mat_blue_flag_01 : Mat_flag_01)
{
   mapTo = "blue_flag_01";
   diffuseMap[0] = "flag_01_D_blue.dds";
};

singleton Material(Mat_red_flag_01 : Mat_flag_01)
{
   mapTo = "red_flag_01";
   diffuseMap[0] = "flag_01_D_red.dds";
};

singleton Material(Mat_green_flag_01 : Mat_flag_01)
{
   mapTo = "green_flag_01";
   diffuseMap[0] = "flag_01_D_green.dds";
};

singleton Material(Mat_yellow_flag_01 : Mat_flag_01)
{
   mapTo = "yellow_flag_01";
   diffuseMap[0] = "flag_01_D_yellow.dds";
};

singleton Material(Mat_black_flag_01 : Mat_flag_01)
{
   mapTo = "black_flag_01";
   diffuseMap[0] = "flag_01_D_black.dds";
};

singleton Material(Mat_white_flag_01 : Mat_flag_01)
{
   mapTo = "white_flag_01";
   diffuseMap[0] = "flag_01_D_white.dds";
};
