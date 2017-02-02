
singleton TSShapeConstructor(Palm_01_tall_leaningDts)
{
   baseShape = "./palm_01_tall_leaning.dts";
};

function Palm_01_tall_leaningDts::onLoad(%this)
{
   %this.addImposter("1", "14", "0", "0", "128", "1", "0");
}
