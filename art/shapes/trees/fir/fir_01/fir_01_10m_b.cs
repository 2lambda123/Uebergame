
singleton TSShapeConstructor(Fir_01_10m_bDae)
{
   baseShape = "./fir_01_10m_b.dts";
};

function Fir_01_10m_bDae::onLoad(%this)
{
   %this.addImposter("1", "6", "0", "0", "128", "1", "0");
}
