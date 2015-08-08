
singleton TSShapeConstructor(Fir_01_15m_bDae)
{
   baseShape = "./fir_01_15m_b.dae";
};

function Fir_01_15m_bDae::onLoad(%this)
{
   %this.addImposter("50", "6", "0", "0", "128", "1", "0");
}
