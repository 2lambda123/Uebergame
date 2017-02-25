singleton CubemapData( sky_space_02_Cubemap )
{
   cubeFace[0] = "./sky_space_02_3"; // right
   cubeFace[1] = "./sky_space_02_1"; // left
   cubeFace[2] = "./sky_space_02_6"; // back
   cubeFace[3] = "./sky_space_02_5"; // front
   cubeFace[4] = "./sky_space_02_2"; // up
   cubeFace[5] = "./sky_space_02_4"; // down
};

singleton Material( sky_space_02_Mat )
{
   cubemap = sky_space_02_Cubemap;
   materialTag0 = "Skyboxes";
   isSky = 1;
};
