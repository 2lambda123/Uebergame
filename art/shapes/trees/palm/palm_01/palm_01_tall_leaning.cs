
singleton TSShapeConstructor(Palm_01_tall_leaningDae)
{
   baseShape = "./palm_01_tall_leaning.dae";
   loadLights = "0";
};

function Palm_01_tall_leaningDae::onLoad(%this)
{
   %this.addImposter("32", "14", "0", "0", "128", "1", "0");
}
