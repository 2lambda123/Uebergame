
singleton TSShapeConstructor(Palm_01_tallDts)
{
   baseShape = "./palm_01_tall.dts";
};

function Palm_01_tallDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
