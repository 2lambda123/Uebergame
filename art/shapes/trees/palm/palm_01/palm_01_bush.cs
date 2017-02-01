
singleton TSShapeConstructor(Palm_01_bushDae)
{
   baseShape = "./palm_01_bush.dae";
   loadLights = "0";
};

function Palm_01_bushDae::onLoad(%this)
{
   %this.addImposter("32", "6", "0", "0", "64", "1", "0");
}
