
singleton Material(JungleHut02__TD_WoodPlank02a)
{
   mapTo = "_TD_WoodPlank02a";
   diffuseMap[0] = "3td_WoodPlank_02";
   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = "10";
   translucentBlendOp = "None";
};

singleton Material(JungleHut02__td_WoodPlank_Trans)
{
   mapTo = "_td_WoodPlank_Trans";
   diffuseMap[0] = "levels/Ambience/JungleHut02/3TD_WoodPlank_T_01";
   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = "10";
   translucentBlendOp = "None";
   useAnisotropic[0] = "1";
   alphaTest = "1";
};

singleton Material(JungleHut02__td_Canvas)
{
   mapTo = "_td_Canvas";
   diffuseMap[0] = "levels/Ambience/JungleHut02/3td_ThatchRoof_01";
   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = "10";
   translucentBlendOp = "None";
   useAnisotropic[0] = "1";
   doubleSided = "1";
   alphaTest = "1";
   alphaRef = "100";
};

singleton Material(JungleHut02_ThatchRoof01)
{
   mapTo = "ThatchRoof01";
   diffuseMap[0] = "levels/Ambience/JungleHut02/3td_ThatchRoof_01";
   specular[0] = "0.9 0.9 0.9 1";
   specularPower[0] = "10";
   translucentBlendOp = "LerpAlpha";
   useAnisotropic[0] = "1";
   subSurface[0] = "0";
   subSurfaceColor[0] = "1 0.882353 0 1";
   doubleSided = "1";
   alphaTest = "1";
   alphaRef = "100";
};
