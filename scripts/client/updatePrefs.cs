function updatePrefs()
{
	//update changed prefs for older versions
	$pref::TSShapeConstructor::CapsuleShapePath = "art/editor/unit_capsule.dts";
	$pref::TSShapeConstructor::CubeShapePath = "art/editor/unit_cube.dts";
	$pref::TSShapeConstructor::SphereShapePath = "art/editor/unit_sphere.dts";
	$pref::Video::missingTexturePath = "art/editor/missingTexture";
	$pref::Video::unavailableTexturePath = "art/editor/unavailable";
	$pref::Video::warningTexturePath = "art/editor/warnmat";
	
	//new server settings
	$pref::Server::TimeLimit = 15;
	
    //update the keybindings to use the new features
	restoreDefaultMappings();
	//set the pref version so we know the user's config files have been updated
    $Pref::Version = 1050;
}