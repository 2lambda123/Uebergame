
singleton TSShapeConstructor(Fir_01_5m_bDae)
{
   baseShape = "./fir_01_5m_b.dae";
};

function Fir_01_5m_bDae::onLoad(%this)
{
   %this.addImposter("32", "6", "0", "0", "128", "1", "0");
}
