
singleton TSShapeConstructor(Canopy_01_cDts)
{
   baseShape = "./canopy_01_c.dts";
};

function Canopy_01_cDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
