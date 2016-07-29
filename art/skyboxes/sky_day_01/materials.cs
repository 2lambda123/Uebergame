singleton CubemapData( sky_day_01_Cubemap )
{
   cubeFace[0] = "./sky_day_01_east";
   cubeFace[1] = "./sky_day_01_west";
   cubeFace[2] = "./sky_day_01_south";
   cubeFace[3] = "./sky_day_01_north";
   cubeFace[4] = "./sky_day_01_up";
   cubeFace[5] = "./sky_day_01_down";
};

singleton Material( sky_day_01_SkyMat )
{
   cubemap = sky_day_01_Cubemap;
   materialTag0 = "Skies";
   effectColor[1] = "InvisibleBlack";
   isSky = true;
};
