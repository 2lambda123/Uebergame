
singleton TSShapeConstructor(Fir_01_5m_bDae)
{
   baseShape = "./fir_01_5m_b.dts";
};

function Fir_01_5m_bDae::onLoad(%this)
{
   %this.addImposter("2", "6", "0", "0", "128", "1", "0");
}
