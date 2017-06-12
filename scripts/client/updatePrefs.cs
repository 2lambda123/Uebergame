/// This function force update all pref variables for clients who had the game installed already
/// The function needs to be manually edited whenever some pref variables have changed
/// and the game would no longer work properly for people who stil have settings from older versions in their config files
/// for example the $pref::Video::missingTexturePath is necessary to work,
/// otherwise the editor will crash when a texture is not found
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
    $pref::Version = 1050;
}