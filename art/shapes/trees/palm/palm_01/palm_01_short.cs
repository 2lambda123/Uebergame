
singleton TSShapeConstructor(Palm_01_shortDts)
{
   baseShape = "./palm_01_short.dts";
};

function Palm_01_shortDts::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
