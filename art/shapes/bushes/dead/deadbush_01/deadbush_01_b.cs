
singleton TSShapeConstructor(Deadbush_01_bDts)
{
   baseShape = "./deadbush_01_b.dts";
};

function Deadbush_01_bDts::onLoad(%this)
{
   %this.addImposter("1", "4", "0", "0", "128", "0", "0");
   %this.setDetailLevelSize("96", "64");
}
