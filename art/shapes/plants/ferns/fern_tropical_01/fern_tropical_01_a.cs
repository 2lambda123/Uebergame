
singleton TSShapeConstructor(Fern_tropical_01_aDts)
{
   baseShape = "./fern_tropical_01_a.dts";
};

function Fern_tropical_01_aDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "64", "1", "0");
}
