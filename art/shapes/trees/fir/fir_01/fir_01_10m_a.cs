
singleton TSShapeConstructor(Fir_01_10m_aDae)
{
   baseShape = "./fir_01_10m_a.dae";
};

function Fir_01_10m_aDae::onLoad(%this)
{
   %this.addImposter("50", "6", "0", "0", "128", "1", "0");
}
