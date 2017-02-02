
singleton TSShapeConstructor(Palm_01_bushDts)
{
   baseShape = "./palm_01_bush.dts";
};

function Palm_01_bushDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "64", "1", "0");
}
