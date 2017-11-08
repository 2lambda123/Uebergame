
singleton Material(Mat_weaponcrate_old_01)
{
   mapTo = "weaponcrate_old_01";
   diffuseColor[0] = "0.8 0.8 0.8 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   translucentBlendOp = "None";
   diffuseMap[0] = "weaponcrate_old_01_D.dds";
   normalMap[0] = "weaponcrate_old_01_N.dds";
   specularMap[0] = "weaponcrate_old_01_S.dds";
   specular[2] = "1 1 1 1";
   showFootprints = "0";
};

singleton Material(Mat_weapon_box_01)
{
   mapTo = "base_weapon_box_01";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   diffuseMap[0] = "art/shapes/storage/crates/weapons/weapon_box_01_D_white.dds";
   specular[2] = "1 1 1 1";
   showFootprints = "0";
   normalMap[0] = "art/shapes/storage/crates/weapons/weapon_box_01_N.dds";
   specularMap[0] = "art/shapes/storage/crates/weapons/weapon_box_01_S.dds";
};

   singleton Material(Mat_green_weapon_box_01 : Mat_weapon_box_01)
{
   mapTo = "green_weapon_box_01";
   diffuseMap[0] = "weapon_box_01_D_green.dds";
 };
 
    singleton Material(Mat_blue_weapon_box_01 : Mat_weapon_box_01)
{
   mapTo = "blue_weapon_box_01";
   diffuseMap[0] = "weapon_box_01_D_blue.dds";
 };
 
    singleton Material(Mat_brown_weapon_box_01 : Mat_weapon_box_01)
{
   mapTo = "brown_weapon_box_01";
   diffuseMap[0] = "weapon_box_01_D_brown.dds";
 };
 
     singleton Material(Mat_jungle_weapon_box_01 : Mat_weapon_box_01)
{
   mapTo = "jungle_weapon_box_01";
   diffuseMap[0] = "weapon_box_01_D_jungle.dds";
 };
 
     singleton Material(Mat_sand_weapon_box_01 : Mat_weapon_box_01)
{
   mapTo = "sand_weapon_box_01";
   diffuseMap[0] = "weapon_box_01_D_sand.dds";
 };
