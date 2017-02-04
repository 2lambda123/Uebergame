
singleton TSShapeConstructor(Banana_01_aDts)
{
   baseShape = "./banana_01_a.dts";
};

function Banana_01_aDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
