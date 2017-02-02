
singleton TSShapeConstructor(Canopy_01_aDts)
{
   baseShape = "./canopy_01_a.dts";
};

function Canopy_01_aDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
