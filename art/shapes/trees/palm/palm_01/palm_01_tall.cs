
singleton TSShapeConstructor(Palm_01_tallDae)
{
   baseShape = "./palm_01_tall.dae";
   loadLights = "0";
};

function Palm_01_tallDae::onLoad(%this)
{
   %this.addImposter("32", "4", "0", "0", "128", "1", "0");
}
