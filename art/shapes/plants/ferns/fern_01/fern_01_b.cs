
singleton TSShapeConstructor(Fern_01_bDts)
{
   baseShape = "./fern_01_b.dts";
};

function Fern_01_bDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "64", "1", "0");
   %this.setDetailLevelSize("4", "64");
}
