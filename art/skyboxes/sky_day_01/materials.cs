singleton CubemapData( sky_day_01_Cubemap )
{
   cubeFace[0] = "./sky_day_01_east"; //right
   cubeFace[1] = "./sky_day_01_west"; //left
   cubeFace[2] = "./sky_day_01_south"; //back
   cubeFace[3] = "./sky_day_01_north"; //front
   cubeFace[4] = "./sky_day_01_up"; //up
   cubeFace[5] = "./sky_day_01_down"; //down
};

singleton Material( sky_day_01_SkyMat )
{
   cubemap = sky_day_01_Cubemap;
   materialTag0 = "Skyboxes";
   isSky = true;
};
