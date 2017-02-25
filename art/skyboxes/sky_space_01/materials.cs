singleton CubemapData( sky_space_01_Cubemap )
{
   cubeFace[0] = "./sky_space_01_3"; // right
   cubeFace[1] = "./sky_space_01_1"; // left
   cubeFace[2] = "./sky_space_01_6"; // back
   cubeFace[3] = "./sky_space_01_5"; // front
   cubeFace[4] = "./sky_space_01_2"; // up
   cubeFace[5] = "./sky_space_01_4"; // down
};

singleton Material( sky_space_01_Mat )
{
   cubemap = sky_space_01_Cubemap;
   materialTag0 = "Skyboxes";
   isSky = 1;
};
