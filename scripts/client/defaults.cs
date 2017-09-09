//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

// Paths for core editor objects
// eventually #cleanup or update them to somewhere else since they make the 
// art/editor folder show in the shape editor, but there is nothing useful in there.
$pref::TSShapeConstructor::CapsuleShapePath = "art/editor/unit_capsule.dts";
$pref::TSShapeConstructor::CubeShapePath = "art/editor/unit_cube.dts";
$pref::TSShapeConstructor::SphereShapePath = "art/editor/unit_sphere.dts";
$pref::Video::missingTexturePath = "art/editor/missingTexture";
$pref::Video::unavailableTexturePath = "art/editor/unavailable";
$pref::Video::warningTexturePath = "art/editor/warnmat";

// Constants for referencing video resolution preferences
$WORD::RES_X = 0;
$WORD::RES_Y = 1;
$WORD::FULLSCREEN = 2;
$WORD::BITDEPTH = 3;
$WORD::REFRESH = 4;
$WORD::AA = 5;

// Debugging
$GameBase::boundingBox = false;

// Now add your own game specific client preferences as well as any overloaded core defaults here.
$pref::LogEchoEnabled = 0;

$pref::Menu::Level = 1;
$pref::Menu::Ads = 1;
$pref::Menu::RandomBackgrounds = 1;

$pref::debug::FPV = "1";
$pref::debug::showFramerate = "1";
$pref::decalMgr::enabled = "1";
$pref::enablePostEffects = "1";
$pref::Server::Multiplayer = "1";

$pref::imposter::canShadow = "1";
$pref::enableBadWordFilter = "1";

$pref::SFX::distanceModel = "logarithmic";
$pref::SFX::dopplerFactor = "0.75";
$pref::SFX::rolloffFactor = "0.25";

$pref::Player = "Player\tbase";
$pref::Player::defaultFov = 90;
$pref::Player::Fov = 90;
//$pref::Player::FriendlyColor = "0.000000 1.000000 0.000000 1.000000"; //not in use
//$pref::Player::EnemyColor = "1.000000 0.000000 0.000000 1.000000"; //not in use
//$pref::Player::SelectedVehicle = "Cheetah"; //vehicles not ready yet

$pref::Audio::BackgroundMusic = 1;

$pref::Net::LagThreshold = 250; // 400 is insane
$pref::Net::Port = 28000;
$pref::Net::PacketRateToClient = "32";
$pref::Net::PacketRateToServer = "32";
$pref::Net::PacketSize = "450";

$pref::HudMessageLogSize = 40;
$pref::ChatHudLength = 1;

$pref::Input::LinkMouseSensitivity = 0.4;
// DInput keyboard, mouse, and joystick prefs
$pref::Input::KeyboardEnabled = 1;
$pref::Input::MouseEnabled = 1;
$pref::Input::JoystickEnabled = 0;
$pref::Input::KeyboardTurnSpeed = 0.1;

$sceneLighting::cacheSize = 20000;
$sceneLighting::purgeMethod = "lastCreated";
$sceneLighting::cacheLighting = 1;

$pref::Video::displayDevice = "D3D9";
$pref::Video::disableVerticalSync = 0;
$pref::Video::mode = "1024 768 false 32 60 4";
$pref::Video::defaultFenceCount = 0;
$pref::Video::screenShotSession = 0;
$pref::Video::screenShotFormat = 0; // 0=png, 1=jpg

// This disables the hardware FSAA/MSAA so that we depend completely on the FXAA post effect
// which works on all cards and in deferred mode. Note the new Intel Hybrid graphics on laptops
// will fail to initialize when hardware AA is enabled... so you've been warned.
$pref::Video::disableHardwareAA = true;
$pref::Video::disableNormalmapping = false;
$pref::Video::disablePixSpecular = false;
$pref::Video::disableCubemapping = false;
$pref::Video::disableParallaxMapping = true;
$pref::Video::Gamma = 2.25;
$pref::Video::Contrast = 1.0;
$pref::Video::Brightness = 0.0;

// This is the path used by ShaderGen to cache proceduralhaders.
// If left blank ShaderGen will only cache shaders to memory and not to disk.
$shaderGen::cachePath = "shaders/procedural";

// The prefered light manager to use at startup. If blank
// or if the selected one doesn't work on this platfom it will try the defaults below.
$pref::lightManager = "Advanced Lighting";
$pref::LightManager::Enable = "1";
$pref::LightManager::Enable::AA = "1";
$pref::LightManager::Enable::DOF = "1";
$pref::LightManager::Enable::HDR = "0";
$pref::LightManager::Enable::LightRay = "1";
$pref::LightManager::Enable::SSAO = "0";
$pref::LightManager::Enable::Vignette = "1";
$pref::LightManager::Enable::SunBokeh = "0";
$pref::LightManager::sgAtlasMaxDynamicLights = "64";
$pref::LightManager::sgDynamicShadowDetailSize = "0";
$pref::LightManager::sgDynamicShadowQuality = "0";
$pref::LightManager::sgLightingProfileAllowShadows = "1";
$pref::LightManager::sgLightingProfileQuality = "0";
$pref::LightManager::sgMaxBestLights = "50";
$pref::LightManager::sgMultipleDynamicShadows = "0";
$pref::LightManager::sgShowCacheStats = "0";
$pref::LightManager::sgUseBloom = "";
$pref::LightManager::sgUseDRLHighDynamicRange = "0";
$pref::LightManager::sgUseDynamicRangeLighting = "0";
$pref::LightManager::sgUseDynamicShadows = "1";
$pref::LightManager::sgUseToneMapping = "";

// This is the default list of light managers ordered from most to least desirable for initialization.
$lightManager::defaults = "Advanced Lighting" NL "Basic Lighting";

// A scale to apply to the camera view distance typically used for tuning performance.
$pref::camera::distanceScale = 1.0;

// Causes the system to do a one time autodetect of an SFX provider and device at startup if the provider is unset.
$pref::SFX::autoDetect = true;

// The sound provider to select at startup. Typically this is DirectSound, OpenAL, or XACT.  There is also 
// a special Null provider which acts normally, but plays no sound.
$pref::SFX::provider = "";

// The sound device to select from the provider. Each provider may have several different devices.
$pref::SFX::device = "OpenAL";

// If true the device will try to use hardware buffers and sound mixing. If not it will use software.
$pref::SFX::useHardware = false;

// If you have a software device you have a choice of how many software buffers to
// allow at any one time.  More buffers cost more CPU time to process and mix.
$pref::SFX::maxSoftwareBuffers = 16;

// This is the playback frequency for the primary sound buffer used for mixing.  Although most providers
// will reformat on the fly, for best quality and performance match your sound files to this setting.
$pref::SFX::frequency = 44100;

// This is the playback bitrate for the primary sound buffer used for mixing. Although most providers
// will reformat on the fly, for best quality and performance match your sound files to this setting.
$pref::SFX::bitrate = 32;

// The overall system volume at startup. Note that you can only scale volume down, volume does not get louder than 1.
$pref::SFX::masterVolume = 0.5;

// The startup sound channel volumes.  These are used to control the overall volume of different classes of sounds.
$pref::SFX::channelVolume1 = 1;
$pref::SFX::channelVolume2 = 1;
$pref::SFX::channelVolume3 = 1;
$pref::SFX::channelVolume4 = 1;
$pref::SFX::channelVolume5 = 1;
$pref::SFX::channelVolume6 = 1;
$pref::SFX::channelVolume7 = 1;
$pref::SFX::channelVolume8 = 1;

$pref::PostEffect::PreferedHDRFormat = "GFXFormatR8G8B8A8";

// This is an scalar which can be used to reduce the reflection textures on all objects to save fillrate.
$pref::Reflect::refractTexScale = 1.0;

// This is the total frame in milliseconds to budget for reflection rendering.
// If your CPU bound and have alot of smaller reflection surfaces try reducing this time.
$pref::Reflect::frameLimitMS = 10;

// Set true to force all water objects to use static cubemap reflections.
$pref::Water::disableTrueReflections = true;

// A global LOD scalar which can reduce the overall density of placed GroundCover.
$pref::GroundCover::densityScale = 1.0;

// An overall scaler on the lod switching between DTS models. Smaller numbers makes the lod switch sooner.
$pref::TS::detailAdjust = 1.0;

// Draw decals yes or no, this probably does not deactivate the decal instances being created.
$pref::Decals::enabled = true;

// How long decals should stay, by default all decals are setup to stay for around 1 minute
$pref::Decals::lifeTimeScale = 2;

// How long debris will stay
$pref::PhysicsDebris::lifetimeScale = "1";

// The number of mipmap levels to drop on loaded textures to reduce video memory usage.  
// It will skip any textures that have been defined as not allowing down scaling.
$pref::Video::textureReductionLevel = 0;

// Increaing this will make sharper shadows
$pref::Shadows::textureScalar = 1.0;

// Disable shadows alltogether yes or no
$pref::Shadows::disable = false;

// Sets the shadow filtering mode:
//
// None - Disables filtering.
// SoftShadow - Does a simple soft shadow
// SoftShadowHighQuality 
$pref::Shadows::filterMode = "SoftShadowHighQuality";

// No anisotropic filtering by default
$pref::Video::defaultAnisotropy = 0;

// Radius in meters around the camera that ForestItems are affected by wind.
// Note that a very large number with a large number of items is not cheap.
$pref::windEffectRadius = 25;

// AutoDetect graphics quality levels the next startup.
$pref::Video::autoDetect = 1;

// smallestVisiblePixelSize stops rendering shapes smaller than this amount
$pref::TS::smallestVisiblePixelSize = 4;

// terrain LOD scale
$pref::Terrain::lodScale = 2.0;

//-----------------------------------------------------------------------------
// Graphics Quality Groups
//-----------------------------------------------------------------------------

// The graphics quality groups are used by the options dialog to control the state of the $prefs.
// You should overload these in your game specific defaults.cs file if they need to be changed.

if ( isObject( MeshQualityGroup ) )
   MeshQualityGroup.delete();
if ( isObject( TextureQualityGroup ) )
   TextureQualityGroup.delete();
if ( isObject( LightingQualityGroup ) )
   LightingQualityGroup.delete();
if ( isObject( ShaderQualityGroup ) )
   ShaderQualityGroup.delete();
if ( isObject( DecalQualityGroup ) )
   DecalQualityGroup.delete();
 
new SimGroup( MeshQualityGroup )
{ 
   new ArrayObject( [Potato] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::TS::detailAdjust"] = 0.7;
      key["$pref::TS::skipRenderDLs"] = 0;
      key["$pref::TS::smallestVisiblePixelSize"] = 6;    
      key["$pref::Terrain::lodScale"] = 8.0;
      key["$pref::decalMgr::enabled"] = false;
      key["$pref::GroundCover::densityScale"] = 0;
   };
   
   new ArrayObject( [Lowest] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::TS::detailAdjust"] = 0.8;
      key["$pref::TS::skipRenderDLs"] = 0;
      key["$pref::TS::smallestVisiblePixelSize"] = 6;	
      key["$pref::Terrain::lodScale"] = 4.0;
      key["$pref::decalMgr::enabled"] = true;
      key["$pref::GroundCover::densityScale"] = 0.3;
   };
   
   new ArrayObject( [Low] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::TS::detailAdjust"] = 0.9;
      key["$pref::TS::skipRenderDLs"] = 0;
      key["$pref::TS::smallestVisiblePixelSize"] = 6;  
      key["$pref::Terrain::lodScale"] = 3.0;
      key["$pref::decalMgr::enabled"] = true;
      key["$pref::GroundCover::densityScale"] = 0.6;
   };
   
   new ArrayObject( [Normal] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::TS::detailAdjust"] = 1.0;
      key["$pref::TS::skipRenderDLs"] = 0;  
      key["$pref::TS::smallestVisiblePixelSize"] = 4;
      key["$pref::Terrain::lodScale"] = 2.0;
      key["$pref::decalMgr::enabled"] = true;
      key["$pref::GroundCover::densityScale"] = 1.0;
   };

   new ArrayObject( [High] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::TS::detailAdjust"] = 1.5;
      key["$pref::TS::skipRenderDLs"] = 0; 
      key["$pref::TS::smallestVisiblePixelSize"] = 2; 
      key["$pref::Terrain::lodScale"] = 1.5;
      key["$pref::decalMgr::enabled"] = true;
      key["$pref::GroundCover::densityScale"] = 1.5;
   };   
   
      new ArrayObject( [Ueber] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::TS::detailAdjust"] = 2.0;
      key["$pref::TS::skipRenderDLs"] = 0; 
      key["$pref::TS::smallestVisiblePixelSize"] = 1;
      key["$pref::Terrain::lodScale"] = 1.0;
      key["$pref::decalMgr::enabled"] = true;
      key["$pref::GroundCover::densityScale"] = 2.0;
   };   
};

new SimGroup( TextureQualityGroup )
{
   new ArrayObject( [Potato] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::textureReductionLevel"] = 2;
      key["$pref::Reflect::refractTexScale"] = 0.25;
      key["$pref::Terrain::detailScale"] = 0.25;      
   };
   
   new ArrayObject( [Lowest] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::textureReductionLevel"] = 2;
      key["$pref::Reflect::refractTexScale"] = 0.5;
      key["$pref::Terrain::detailScale"] = 0.5;      
   };
   
   new ArrayObject( [Low] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::textureReductionLevel"] = 1;
      key["$pref::Reflect::refractTexScale"] = 0.75;
      key["$pref::Terrain::detailScale"] = 0.75;      
   };
   
   new ArrayObject( [Normal] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::textureReductionLevel"] = 0;
      key["$pref::Reflect::refractTexScale"] = 1;
      key["$pref::Terrain::detailScale"] = 1;      
   };

   new ArrayObject( [High] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::textureReductionLevel"] = 0;
      key["$pref::Reflect::refractTexScale"] = 1.25;
      key["$pref::Terrain::detailScale"] = 1.5;      
   };   
   
      new ArrayObject( [Ueber] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::textureReductionLevel"] = 0;
      key["$pref::Reflect::refractTexScale"] = 1.5;
      key["$pref::Terrain::detailScale"] = 2.0;      
   }; 
};

function TextureQualityGroup::onApply( %this, %level )
{
   // Note that this can be a slow operation.  
   reloadTextures();
}

new SimGroup( LightingQualityGroup )
{ 
   new ArrayObject( [Potato] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::lightManager"] = "Basic Lighting";
      key["$pref::Shadows::disable"] = true;
      key["$pref::Shadows::textureScalar"] = 0.5;
      key["$pref::Shadows::filterMode"] = "None";
      key["$pref::PSSM::smallestVisiblePixelSize"] = 32;      
   };

   new ArrayObject( [Lowest] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::lightManager"] = "Advanced Lighting";
      key["$pref::Shadows::disable"] = true;
      key["$pref::Shadows::textureScalar"] = 0.5;
      key["$pref::Shadows::filterMode"] = "None";
      key["$pref::PSSM::smallestVisiblePixelSize"] = 32;      
   };
   
   new ArrayObject( [Low] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;          
      key["$pref::lightManager"] = "Advanced Lighting";
      key["$pref::Shadows::disable"] = false;
      key["$pref::Shadows::textureScalar"] = 0.5;
      key["$pref::Shadows::filterMode"] = "SoftShadow";
      key["$pref::PSSM::smallestVisiblePixelSize"] = 16;
   };
   
   new ArrayObject( [Normal] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::lightManager"] = "Advanced Lighting";
      key["$pref::Shadows::disable"] = false;
      key["$pref::Shadows::textureScalar"] = 1.0;
      key["$pref::Shadows::filterMode"] = "SoftShadowHighQuality";
      key["$pref::PSSM::smallestVisiblePixelSize"] = 8;      
   };

   new ArrayObject( [High] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::lightManager"] = "Advanced Lighting";
      key["$pref::Shadows::disable"] = false;
      key["$pref::Shadows::textureScalar"] = 2.0;
      key["$pref::Shadows::filterMode"] = "SoftShadowHighQuality";
      key["$pref::PSSM::smallestVisiblePixelSize"] = 4;      
   };  

   new ArrayObject( [Ueber] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::lightManager"] = "Advanced Lighting";
      key["$pref::Shadows::disable"] = false;
      key["$pref::Shadows::textureScalar"] = 3.0;
      key["$pref::Shadows::filterMode"] = "SoftShadowHighQuality";
      key["$pref::PSSM::smallestVisiblePixelSize"] = 2;      
   };   
};

function LightingQualityGroup::onApply( %this, %level )
{
   // Set the light manager. This should do nothing if its already set or if its not compatible.   
   setLightManager( $pref::lightManager );
}

// Reduce shader complexity of specularity, normal mapping, parallax, reflections and wind effect radius
new SimGroup( ShaderQualityGroup )
{
   new ArrayObject( [Potato] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::disablePixSpecular"] = true;
      key["$pref::Video::disableNormalmapping"] = true;
      key["$pref::Video::disableParallaxMapping"] = true;
      key["$pref::Water::disableTrueReflections"] = true;
      key["$pref::windEffectRadius"] = 0;
   };
   
   new ArrayObject( [Lowest] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::disablePixSpecular"] = true;
      key["$pref::Video::disableNormalmapping"] = true;
      key["$pref::Video::disableParallaxMapping"] = true;
      key["$pref::Water::disableTrueReflections"] = true;
      key["$pref::windEffectRadius"] = 8;
   };
   
   new ArrayObject( [Low] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::disablePixSpecular"] = true;
      key["$pref::Video::disableNormalmapping"] = false;
      key["$pref::Video::disableParallaxMapping"] = true;
      key["$pref::Water::disableTrueReflections"] = true;
      key["$pref::windEffectRadius"] = 16;
   };
   
   new ArrayObject( [Normal] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::disablePixSpecular"] = false;
      key["$pref::Video::disableNormalmapping"] = false;
      key["$pref::Video::disableParallaxMapping"] = true;   
      key["$pref::Water::disableTrueReflections"] = true;   
      key["$pref::windEffectRadius"] = 25;
   };
   
   new ArrayObject( [High] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::disablePixSpecular"] = false;
      key["$pref::Video::disableNormalmapping"] = false;
      key["$pref::Video::disableParallaxMapping"] = true;     
      key["$pref::Water::disableTrueReflections"] = false;   
      key["$pref::windEffectRadius"] = 50;	  
   };   
   
   new ArrayObject( [Ueber] )
   {
      class = "GraphicsQualityLevel";
      caseSensitive = true;
      key["$pref::Video::disablePixSpecular"] = false;
      key["$pref::Video::disableNormalmapping"] = false;
      key["$pref::Video::disableParallaxMapping"] = false;     
      key["$pref::Water::disableTrueReflections"] = false; 
      key["$pref::windEffectRadius"] = 100;	  
   };  
};

new SimGroup( DecalQualityGroup ) //extra compact code design
{
   new ArrayObject([0.1]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=0.1; };
   new ArrayObject([0.25]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=0.25; };
   new ArrayObject([0.5]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=0.5; };
   new ArrayObject([1]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=1; };
   new ArrayObject([2]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=2; };
   new ArrayObject([3]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=3; };
   new ArrayObject([4]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=4; };
   new ArrayObject([5]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=5; };
   new ArrayObject([10]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=10; };
   new ArrayObject([15]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=15; };
   new ArrayObject([30]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=30; };
   new ArrayObject([60]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=60; };
   new ArrayObject([1440]) {class="GraphicsQualityLevel"; key["$pref::Decals::lifeTimeScale"]=1440; };
};

function GraphicsQualityAutodetect()
{
   $pref::Video::autoDetect = false;
   
   %shaderVer = getPixelShaderVersion();
   %intel = ( strstr( strupr( getDisplayDeviceInformation() ), "INTEL" ) != -1 ) ? true : false;
   %videoMem = GFXCardProfilerAPI::getVideoMemoryMB();
   
   return GraphicsQualityAutodetect_Apply( %shaderVer, %intel, %videoMem );
}

function GraphicsQualityAutodetect_Apply( %shaderVer, %intel, %videoMem )
{
   if ( %shaderVer < 2.0 )
   {      
      return "Your video card does not meet the minimum requirment of shader model 2.0.";
   }
   
   if ( %shaderVer < 3.0 || %intel )
   {
      // Allow specular and normals for 2.0a and 2.0b
      if ( %shaderVer > 2.0 )
      {
         MeshQualityGroup-->Lowest.apply();
         TextureQualityGroup-->Lowest.apply();
         LightingQualityGroup-->Lowest.apply();
         ShaderQualityGroup-->Low.apply();
         DecalQualityGroup-->(0.1).apply();
         $pref::LightManager::Enable::sunBokeh = "0";
         $pref::LightManager::Enable::Vignette = "1";
         $pref::LightManager::Enable::HDR = "0";
         $pref::LightManager::Enable::SSAO = "0";
         $pref::LightManager::Enable::LightRay = "0";
         $pref::LightManager::Enable::DOF = "0"; 
         $pref::Video::defaultAnisotropy = "0";
      }
      else
      {
         MeshQualityGroup-->Lowest.apply();
         TextureQualityGroup-->Lowest.apply();
         LightingQualityGroup-->Lowest.apply();
         ShaderQualityGroup-->Lowest.apply();
         DecalQualityGroup-->(0.1).apply();	
         $pref::LightManager::Enable::sunBokeh = "0";
         $pref::LightManager::Enable::Vignette = "1";
         $pref::LightManager::Enable::HDR = "0";
         $pref::LightManager::Enable::SSAO = "0";
         $pref::LightManager::Enable::LightRay = "0";
         $pref::LightManager::Enable::DOF = "0"; 
         $pref::Video::defaultAnisotropy = "0";	 
      }
   }   
   else
   {
	  if ( %videoMem > 2000 )
      {
         MeshQualityGroup-->Ueber.apply();
         TextureQualityGroup-->Ueber.apply();
         LightingQualityGroup-->Ueber.apply();
         ShaderQualityGroup-->Ueber.apply();
         DecalQualityGroup-->(15).apply();	
         $pref::LightManager::Enable::sunBokeh = "0";
         $pref::LightManager::Enable::Vignette = "1";
         $pref::LightManager::Enable::HDR = "1";
         $pref::LightManager::Enable::SSAO = "1";
         $pref::LightManager::Enable::LightRay = "1";
         $pref::LightManager::Enable::DOF = "1"; 
         $pref::Video::defaultAnisotropy = "16";
      }
      else if ( %videoMem > 1000 )
      {
         MeshQualityGroup-->High.apply();
         TextureQualityGroup-->High.apply();
         LightingQualityGroup-->High.apply();
         ShaderQualityGroup-->High.apply();
         DecalQualityGroup-->(5).apply();	
         $pref::LightManager::Enable::sunBokeh = "0";
         $pref::LightManager::Enable::Vignette = "1";
         $pref::LightManager::Enable::HDR = "0";
         $pref::LightManager::Enable::SSAO = "1";
         $pref::LightManager::Enable::LightRay = "1";
         $pref::LightManager::Enable::DOF = "1"; 
         $pref::Video::defaultAnisotropy = "8";
      }
      else if ( %videoMem > 400 || %videoMem == 0 )
      {
         MeshQualityGroup-->Normal.apply();
         TextureQualityGroup-->Normal.apply();
         LightingQualityGroup-->Normal.apply();
         ShaderQualityGroup-->Normal.apply();
         DecalQualityGroup-->(2).apply();
         $pref::LightManager::Enable::sunBokeh = "0";
         $pref::LightManager::Enable::Vignette = "1";
         $pref::LightManager::Enable::HDR = "0";
         $pref::LightManager::Enable::SSAO = "0";
         $pref::LightManager::Enable::LightRay = "1";
         $pref::LightManager::Enable::DOF = "1"; 	
         $pref::Video::defaultAnisotropy = "4";	 
         
         if ( %videoMem == 0 )
            return "Uebergame was unable to detect available video memory. Applying 'Normal' quality.";
      }
      else
      {
         MeshQualityGroup-->Low.apply();
         TextureQualityGroup-->Low.apply();
         LightingQualityGroup-->Low.apply();
         ShaderQualityGroup-->Low.apply();
         DecalQualityGroup-->(0.5).apply();	
         $pref::LightManager::Enable::sunBokeh = "0";
         $pref::LightManager::Enable::Vignette = "1";
         $pref::LightManager::Enable::HDR = "0";
         $pref::LightManager::Enable::SSAO = "0";
         $pref::LightManager::Enable::LightRay = "1";
         $pref::LightManager::Enable::DOF = "0"; 
         $pref::Video::defaultAnisotropy = "0";
      }
   }
   
   return "Graphics quality settings have been auto detected.";
}