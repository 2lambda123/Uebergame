
singleton TSShapeConstructor(Palm_01_shortDae)
{
   baseShape = "./palm_01_short.dae";
   loadLights = "0";
};

function Palm_01_shortDae::onLoad(%this)
{
   %this.addImposter("32", "6", "0", "0", "128", "1", "0");
}
