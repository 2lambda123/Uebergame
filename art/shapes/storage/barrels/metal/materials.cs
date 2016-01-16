  singleton Material(Mat_barrel_oil_01)
{
   mapTo = "base_barrel_oil_01";
   diffuseMap[0] = "barrel_oil_01_bare_D";
   normalMap[0] = "barrel_oil_01_N";
   specularMap[0] = "barrel_oil_01_S";
   materialTag0 = "metal";
   specularPower[0] = "1";
   specularStrength[0] = "0.8";
   showFootprints = "0";
 };
 
 singleton Material(Mat_black_barrel_oil_01 : Mat_barrel_oil_01)  
 {
   mapTo = "black_barrel_oil_01";
   diffuseMap[0] = "barrel_oil_01_black_D";
   materialTag0 = "metal";
 };
 
  singleton Material(Mat_blue_barrel_oil_01 : Mat_barrel_oil_01) 
 {
   mapTo = "blue_barrel_oil_01";
   diffuseMap[0] = "barrel_oil_01_blue_D";
   materialTag0 = "metal";
 };
 
   singleton Material(Mat_green_barrel_oil_01 : Mat_barrel_oil_01) 
 {
   mapTo = "green_barrel_oil_01";
   diffuseMap[0] = "barrel_oil_01_green_D";
   materialTag0 = "metal";
 };
 
   singleton Material(Mat_red_barrel_oil_01 : Mat_barrel_oil_01) 
 {
   mapTo = "red_barrel_oil_01";
   diffuseMap[0] = "barrel_oil_01_red_D";
   materialTag0 = "metal";
 };
 
   singleton Material(Mat_rust_barrel_oil_01 : Mat_barrel_oil_01) 
 {
   mapTo = "rust_barrel_oil_01";
   diffuseMap[0] = "barrel_oil_01_rust_D";
   materialTag0 = "metal";
 };
 
   singleton Material(Mat_yellow_barrel_oil_01 : Mat_barrel_oil_01) 
 {
   mapTo = "yellow_barrel_oil_01";
   diffuseMap[0] = "barrel_oil_01_yellow_D";
   materialTag0 = "metal";
 };

   singleton Material(Mat_barrel_oil_02)
{
   mapTo = "base_barrel_oil_02";
   diffuseMap[0] = "barrel_oil_02_blue_D";
   normalMap[0] = "barrel_oil_02_N";
   specularMap[0] = "barrel_oil_02_S";
   materialTag0 = "metal";
   specularPower[0] = "1";
   showFootprints = "0";
   specularStrength[0] = "1.5";
 };
 
    singleton Material(Mat_barrel_oil_03)
{
   mapTo = "base_barrel_oil_03";
   diffuseMap[0] = "art/shapes/storage/barrels/metal/barrel_oil_03_D.dds";
   normalMap[0] = "art/shapes/storage/barrels/metal/barrel_oil_03_N";
   specularMap[0] = "art/shapes/storage/barrels/metal/barrel_oil_03_S";
   materialTag0 = "metal";
   specularPower[0] = "50";
   showFootprints = "0";
 };

    singleton Material(Mat_blue_barrel_oil_03 : Mat_barrel_oil_03) 
 {
   mapTo = "blue_barrel_oil_03";
   diffuseColor[0] = "0.443137 0.698039 0.996078 1";
 };
 
     singleton Material(Mat_green_barrel_oil_03 : Mat_barrel_oil_03) 
 {
   mapTo = "green_barrel_oil_03";
   diffuseColor[0] = "0.588235 0.996078 0.415686 1";
 };
 
      singleton Material(Mat_yellow_barrel_oil_03 : Mat_barrel_oil_03) 
 {
   mapTo = "yellow_barrel_oil_03";
   diffuseColor[0] = "0.980392 0.996078 0.407843 1";
 };
 
      singleton Material(Mat_black_barrel_oil_03 : Mat_barrel_oil_03) 
 {
   mapTo = "black_barrel_oil_03";
   diffuseColor[0] = "0.352941 0.352941 0.352941 1";
 };

singleton Material(Mat_red_barrel_oil_03 : Mat_barrel_oil_03)
{
   mapTo = "red_barrel_oil_03";
   diffuseColor[0] = "0.996078 0.498039 0.364706 1";
};
