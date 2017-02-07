
singleton TSShapeConstructor(Birch_01_cDts)
{
   baseShape = "./birch_01_c.dts";
};

function Birch_01_cDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
