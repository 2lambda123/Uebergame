singleton CubemapData( sky_space_03_Cubemap )
{
   cubeFace[0] = "./sky_space_03_3"; // right
   cubeFace[1] = "./sky_space_03_1"; // left
   cubeFace[2] = "./sky_space_03_6"; // back
   cubeFace[3] = "./sky_space_03_5"; // front
   cubeFace[4] = "./sky_space_03_2"; // up
   cubeFace[5] = "./sky_space_03_4"; // down
};

singleton Material( sky_space_03_Mat )
{
   cubemap = sky_space_03_Cubemap;
   materialTag0 = "Skyboxes";
   isSky = 1;
};
