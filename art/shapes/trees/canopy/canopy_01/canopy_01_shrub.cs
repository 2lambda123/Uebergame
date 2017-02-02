
singleton TSShapeConstructor(Canopy_01_shrubDts)
{
   baseShape = "./canopy_01_shrub.dts";
};

function Canopy_01_shrubDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
