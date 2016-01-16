singleton Material(Mat_paintball_player)
{
   mapTo = "base_paintball_player";
   specularPower[0] = "1";
   diffuseMap[0] = "art/shapes/actors/paintball_player/paintball_player_base_D.dds";
   normalMap[0] = "art/shapes/actors/paintball_player/paintball_player_N.dds";
   specularMap[0] = "art/shapes/actors/paintball_player/paintball_player_S.dds";
   specularStrength[0] = "0.2";
   materialTag0 = "Player";
};

singleton Material(Mat_blue_paintball_player : Mat_paintball_player)
{
   mapTo = "blue_paintball_player";
   diffuseMap[0] = "art/shapes/actors/paintball_player/paintball_player_blue_D.dds";
};

singleton Material(Mat_red_paintball_player : Mat_paintball_player)
{
   mapTo = "red_paintball_player";
   diffuseMap[0] = "art/shapes/actors/paintball_player/paintball_player_red_D.dds";
};

singleton Material(Mat_green_paintball_player : Mat_paintball_player)
{
   mapTo = "green_paintball_player";
   diffuseMap[0] = "art/shapes/actors/paintball_player/paintball_player_green_D.dds";
};

singleton Material(Mat_yellow_paintball_player : Mat_paintball_player)
{
   mapTo = "yellow_paintball_player";
   diffuseMap[0] = "art/shapes/actors/paintball_player/paintball_player_yellow_D.dds";
};

singleton Material(paintball_player_paintball_marker_01_001)
{
   mapTo = "paintball_marker_01_001";
   diffuseColor[0] = "0.8 0.8 0.8 1";
   translucentBlendOp = "None";
};
