
singleton TSShapeConstructor(Fern_01_aDts)
{
   baseShape = "./fern_01_a.dts";
};

function Fern_01_aDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "64", "1", "0");
   %this.setDetailLevelSize("4", "64");
}
