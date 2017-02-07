
singleton TSShapeConstructor(Fir_01_5m_aDae)
{
   baseShape = "./fir_01_5m_a.dts";
};

function Fir_01_5m_aDae::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
