
singleton TSShapeConstructor(Birch_01_aDts)
{
   baseShape = "./birch_01_a.dts";
};

function Birch_01_aDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
