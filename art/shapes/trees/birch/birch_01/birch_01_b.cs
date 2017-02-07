
singleton TSShapeConstructor(Birch_01_bDts)
{
   baseShape = "./birch_01_b.dts";
};

function Birch_01_bDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
