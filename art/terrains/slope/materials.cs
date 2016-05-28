singleton Material(TerrainFX_ter_slope_rubble_01)    
{    
   mapTo = "ter_slope_rubble_01_B";    
   footstepSoundId = "1";    
   terrainMaterials = "1";    
   ShowDust = "0";    
   showFootprints = "1";   
   materialTag0 = "Terrain_Slope";    
   specularPower[0] = "1";  
   impactSoundId = "0";
};

new TerrainMaterial()
{
   diffuseMap = "art/terrains/slope/ter_slope_rubble_01_B";
   diffuseSize = "200";
   detailMap = "art/terrains/slope/ter_slope_rubble_01_D";
   detailSize = "5";
   detailDistance = "100";
   macroSize = "40";
   internalName = "ter_slope_rubble_01";
   macroMap = "art/terrains/slope/ter_slope_rubble_01_M";
   macroStrength = "0.6";
   parallaxScale = "0.03";
   normalMap = "art/terrains/slope/ter_slope_rubble_01_N";
};

singleton Material(TerrainFX_ter_slope_rubble_01_side)    
{    
   mapTo = "ter_slope_rubble_01_B_side";    
   footstepSoundId = "1";    
   terrainMaterials = "1";    
   ShowDust = "0";    
   showFootprints = "0";   
   materialTag0 = "Terrain_Slope";    
   specularPower[0] = "1";  
   impactSoundId = "0";
};

new TerrainMaterial()
{
   diffuseMap = "art/terrains/slope/ter_slope_rubble_01_B";
   diffuseSize = "200";
   detailMap = "art/terrains/slope/ter_slope_rubble_01_D";
   detailSize = "5";
   detailDistance = "100";
   macroSize = "40";
   internalName = "ter_slope_rubble_01_side";
   macroMap = "art/terrains/slope/ter_slope_rubble_01_M";
   macroStrength = "0.6";
   parallaxScale = "0";
   normalMap = "art/terrains/slope/ter_slope_rubble_01_N";
   useSideProjection = "1";
};

