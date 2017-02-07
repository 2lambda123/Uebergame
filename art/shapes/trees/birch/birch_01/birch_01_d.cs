
singleton TSShapeConstructor(Birch_01_dDts)
{
   baseShape = "./birch_01_d.dts";
};

function Birch_01_dDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
