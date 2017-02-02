
singleton TSShapeConstructor(Canopy_01_bDts)
{
   baseShape = "./canopy_01_b.dts";
};

function Canopy_01_bDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
