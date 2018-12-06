
singleton TSShapeConstructor(Fern_01_cDts)
{
   baseShape = "./fern_01_c.dts";
};

function Fern_01_cDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "64", "1", "0");
   %this.setDetailLevelSize("4", "64");
}
