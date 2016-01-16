  singleton Material(Mat_pallet_01)
{
   mapTo = "base_pallet_01";
   diffuseMap[0] = "art/shapes/storage/pallets/pallet_01_bright_D.dds";
   normalMap[0] = "art/shapes/storage/pallets/pallet_01_N.dds";
   specularMap[0] = "art/shapes/storage/pallets/pallet_01_worn_D.dds";
   materialTag0 = "wood";
   specularPower[0] = "32";
   specularStrength[0] = "0.3";
   showFootprints = "0";
 };
 
   singleton Material(Mat_contrast_pallet_01 : Mat_pallet_01)
{
   mapTo = "contrast_pallet_01";
   diffuseMap[0] = "pallet_01_contrast_D.dds";
   materialTag0 = "wood";
 };
 
    singleton Material(Mat_sated_pallet_01 : Mat_pallet_01)
{
   mapTo = "sated_pallet_01";
   diffuseMap[0] = "pallet_01_sated_D.dds";
   materialTag0 = "wood";
 };
 
    singleton Material(Mat_worn_pallet_01 : Mat_pallet_01)
{
   mapTo = "worn_pallet_01";
   diffuseMap[0] = "pallet_01_worn_D.dds";
   materialTag0 = "wood";
 };
 
   singleton Material(Mat_pallet_02)
{
   mapTo = "pallet_02";
   diffuseMap[0] = "pallet_02_D.dds";
   normalMap[0] = "pallet_02_N.dds";
   specularMap[0] = "pallet_02_D.dds";
   materialTag0 = "wood";
   showFootprints = "0";
   specularStrength[0] = "0.5";
 };
 
    singleton Material(Mat_pallet_03)
{
   mapTo = "pallet_03";
   diffuseMap[0] = "pallet_03_D.dds";
   normalMap[0] = "pallet_03_N.dds";
   specularMap[0] = "pallet_03_D.dds";
   materialTag0 = "wood";
   showFootprints = "0";
 };
