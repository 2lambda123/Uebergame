
singleton Material(Mat_flag_stand_01)
{
   mapTo = "base_flag_stand_01";
   diffuseColor[0] = "0.666667 0.666667 0.666667 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "1";
   translucentBlendOp = "None";
   normalMap[0] = "art/shapes/objectives/flag_stands/flag_stand_01_N.dds";
   specularStrength[0] = "1";
   specularMap[0] = "art/shapes/objectives/flag_stands/flag_stand_01_S.dds";
   showFootprints = "0";
};

singleton Material(Mat_blue_flag_stand_01 : Mat_flag_stand_01)
{
   mapTo = "blue_flag_stand_01";
   diffuseMap[0] = "art/textures/colors/team_blue_01.dds";
};

singleton Material(Mat_red_flag_stand_01 : Mat_flag_stand_01)
{
   mapTo = "red_flag_stand_01";
   diffuseMap[0] = "art/textures/colors/team_red_01.dds";
};

singleton Material(Mat_green_flag_stand_01 : Mat_flag_stand_01)
{
   mapTo = "green_flag_stand_01";
   diffuseMap[0] = "art/textures/colors/team_green_01.dds";
};

singleton Material(Mat_yellow_flag_stand_01 : Mat_flag_stand_01)
{
   mapTo = "yellow_flag_stand_01";
   diffuseMap[0] = "art/textures/colors/team_yellow_01.dds";
};

singleton Material(Mat_black_flag_stand_01 : Mat_flag_stand_01)
{
   mapTo = "black_flag_stand_01";
   diffuseMap[0] = "art/textures/colors/team_black_01.dds";
};
