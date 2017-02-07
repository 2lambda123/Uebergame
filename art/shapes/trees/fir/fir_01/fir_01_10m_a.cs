
singleton TSShapeConstructor(Fir_01_10m_aDae)
{
   baseShape = "./fir_01_10m_a.dts";
};

function Fir_01_10m_aDae::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
