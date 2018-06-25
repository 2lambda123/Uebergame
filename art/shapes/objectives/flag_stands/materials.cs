
singleton Material(Mat_flag_stand_01)
{
   mapTo = "base_flag_stand_01";
   diffuseMap[0] = "art/shapes/objectives/flag_stands/flag_stand_01_D.dds";
   normalMap[0] = "art/shapes/objectives/flag_stands/flag_stand_01_N.dds";
   specularMap[0] = "art/shapes/objectives/flag_stands/flag_stand_01_S.dds";
   showFootprints = "0";
};

singleton Material(Mat_blue_flag_stand_01 : Mat_flag_stand_01)
{
   mapTo = "blue_flag_stand_01";
   diffuseColor[0] = "0.188235 0.439216 0.8 1";
};

singleton Material(Mat_red_flag_stand_01 : Mat_flag_stand_01)
{
   mapTo = "red_flag_stand_01";
   diffuseColor[0] = "0.608 0.176 0.188 1";
};

singleton Material(Mat_green_flag_stand_01 : Mat_flag_stand_01)
{
   mapTo = "green_flag_stand_01";
   diffuseColor[0] = "0.247 0.439 0.11 1";
};

singleton Material(Mat_yellow_flag_stand_01 : Mat_flag_stand_01)
{
   mapTo = "yellow_flag_stand_01";
   diffuseColor[0] = "0.918 0.678 0.078 1";
};

singleton Material(Mat_black_flag_stand_01 : Mat_flag_stand_01)
{
   mapTo = "black_flag_stand_01";
   diffuseColor[0] = "0.128 0.128 0.128 1";
};
