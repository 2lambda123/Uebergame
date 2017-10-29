
singleton TSShapeConstructor(Longleaf_01_aDae)
{
   baseShape = "./longleaf_01_a.dae";
};

function Longleaf_01_aDae::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "64", "1", "0");
}
