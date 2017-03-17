
singleton TSShapeConstructor(Deadbush_01_cDts)
{
   baseShape = "./deadbush_01_c.dts";
};

function Deadbush_01_cDts::onLoad(%this)
{
   %this.addImposter("1", "4", "0", "0", "128", "0", "0");
   %this.setDetailLevelSize("96", "64");
}
