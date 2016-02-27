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

// TIME OF DAY CALLBACKS

function TimeOfDay::onTimeEvent( %this )
{
// Nothing to see here
}

function TimeOfDay::onAnimateStart( %this )
{
// Nothing to see here
}

// ENVIROMENTAL EFFECTS GO HERE (PRECIPITATION - LIGHTNING)

// ----------------------------------------------------------------------------
// Rain
// ----------------------------------------------------------------------------

datablock SFXProfile(HeavyRainSound)
{
   filename = "art/sound/environment/amb";
   description = AudioLoop2d;
};

datablock PrecipitationData(HeavyRain2)
{
   dropTexture = "~/data/textures/environment/mist";
   splashTexture = "~/data/textures/environment/mist2";
   dropSize = 10;
   splashSize = 0.1;
   useTrueBillboards = false;
   splashMS = 250;
};

datablock PrecipitationData(HeavyRain3)
{
   dropTexture = "~/data/textures/environment/shine";
   splashTexture = "~/data/textures/environment/mist2";
   dropSize = 20;
   splashSize = 0.1;
   useTrueBillboards = false;
   splashMS = 250;
};

datablock PrecipitationData(HeavyRain)
{
   soundProfile = "HeavyRainSound";

   dropTexture = "art/environment/precipitation/rain";
   splashTexture = "art/environment/precipitation/water_splash";
   dropSize = 0.2;
   splashSize = 0.1;
   useTrueBillboards = false;
   splashMS = 500;
};

datablock PrecipitationData(Rain)
{
   soundProfile = "HeavyRainSound";

   dropTexture = "art/environment/precipitation/bigRain";
   splashTexture = "art/environment/precipitation/water_splash";
   dropSize = 0.15;
   splashSize = 0.1;
   useTrueBillboards = false;
   splashMS = 250;
};

datablock PrecipitationData(Snow)
{
   //soundProfile = "WindSound";

   dropTexture = "art/environment/precipitation/snow";
   splashTexture = "art/environment/precipitation/snow_Impact";
   dropSize = 0.25;
   splashSize = 0.35;
   useTrueBillboards = false;
   splashMS = 250;
};

datablock SFXProfile(SandstormSound)
{
   filename = "art/sound/wind/wind_01.ogg";
   description = AudioLoop2d;
};

datablock PrecipitationData(Sandstorm)
{
   soundProfile = "SandstormSound";

   dropTexture = "art/environment/precipitation/stormcloud_01";
   splashTexture = "art/environment/precipitation/sandstorm2";
   dropSize = 1;
   splashSize = 0;
   useTrueBillboards = false;
   splashMS = 250;
};

datablock PrecipitationData(dustspecks)
{
   //soundProfile = "dustsound";

   dropTexture = "art/environment/precipitation/dust";
   splashTexture = "art/environment/precipitation/dust2";
   dropSize = 0.25;
   splashSize = 0.25;
   useTrueBillboards = false;
   splashMS = 250;
};

datablock PrecipitationData(Leaves)
{
   //soundProfile = "forest";

   dropTexture = "art/environment/precipitation/leaves";
   //splashTexture = "art/environment/precipitation/deathrain_splash";
   dropSize = 0.35;
   splashSize = 0.1;
   useTrueBillboards = false;
   splashMS = 500;
};

// ----------------------------------------------------------------------------
// GroundFog
// ----------------------------------------------------------------------------

datablock PrecipitationData(groundFog)
{
   //soundProfile = "ADD SOUND HERE";

   dropTexture = "art/environment/precipitation/G_Fog.png";
   dropSize = 35;
   useTrueBillboards = false;
};

// ----------------------------------------------------------------------------
// Heavy Clouds
// ----------------------------------------------------------------------------

datablock PrecipitationData(HeavyCloud)
{
   //soundProfile = "ADD SOUND FILE HERE";

   dropTexture = "art/environment/precipitation/heavyCloud.png";
   dropSize = 1;
   useTrueBillboards = false;
};

// ----------------------------------------------------------------------------
// Lightning
// ----------------------------------------------------------------------------

// When setting up thunder sounds for lightning it should be known that:
// - strikeSound is a 3d sound
// - thunderSounds[n] are 2d sounds

datablock SFXProfile(ThunderCrash1Sound)
{
   filename = "art/sound/environment/thunder1";
   description = Audio2d;
};

datablock SFXProfile(ThunderCrash2Sound)
{
   filename = "art/sound/environment/thunder2";
   description = Audio2d;
};

datablock SFXProfile(ThunderCrash3Sound)
{
   filename = "art/sound/environment/thunder3";
   description = Audio2d;
};

datablock SFXProfile(ThunderCrash4Sound)
{
   filename = "art/sound/environment/thunder4";
   description = Audio2d;
};

datablock LightningData(LightningStorm)
{
   //strikeSound = "";
   strikeTextures = "art/environment/precipitation/lightning";
   thunderSounds[0] = ThunderCrash1Sound;
   thunderSounds[1] = ThunderCrash2Sound;
   thunderSounds[2] = ThunderCrash3Sound;
   thunderSounds[3] = ThunderCrash4Sound;

   damageType = $DamageType::Lightning;
   directDamage = 50;
};

function Lightning::applyDamage(%this, %position, %normal, %targetObject)
{
   echo( "Lightning::applyDamage(" SPC %lightningObj.getName() @", "@ %position @", "@ %normal @", "@ %targetObject.getClassName() SPC ")" );
   if ( isObject( %targetObject ) )
      %targetObject.damage( %this, %position, %this.getDataBlock().directDamage, %this.getDataBlock().damageType );
}

function Lightning::onNewDataBlock(%data, %obj)
{
   if ( %obj != 0 )
      %obj.warningFlashes();
}

//-----------------------------------------------------------------------------

function addPrecip(%type)
{
   echo("addPrecip(" SPC %type SPC ")");
   if( isObject( Game ) )
   {
      switch$( strlwr( %type ) )
      {
         case "0":
	    for(%i = 0; %i < MissionGroup.getCount(); %i++)
            {
               if( MissionGroup.getObject(%i).getName() $= "precip" )
               {
                  MissionGroup.getObject(%i).delete();
                  break;
               }
            }

         case "rain":
            %rain = new Precipitation(precip) {
               numDrops = "1024";
               boxWidth = "200";
               boxHeight = "100";
               dropSize = "0.15";
               splashSize = "0.15";
               splashMS = "250";
               animateSplashes = "1";
               dropAnimateMS = "0";
               fadeDist = "0";
               fadeDistEnd = "0";
               useTrueBillboards = "0";
               useLighting = "1";
               glowIntensity = "0 0 0 0";
               reflect = "0";
               rotateWithCamVel = "1";
               doCollision = "1";
               hitPlayers = "0";
               hitVehicles = "0";
               followCam = "1";
               useWind = "1";
               minSpeed = "1";
               maxSpeed = "1.5";
               minMass = "0.75";
               maxMass = "0.85";
               useTurbulence = "0";
               numDrops = "1000";
               maxTurbulence = "0.1";
               turbulenceSpeed = "0.2";
               dataBlock = "Rain";
               position = "0 0 400";
               rotation = "1 0 0 0";
               scale = "1 1 1";
               canSave = "0";
               canSaveDynamicFields = "0";
            };
            MissionGroup.add(%rain);

         case "snow":
            %snow = new Precipitation(precip) {
               numDrops = "1024";
               boxWidth = "200";
               boxHeight = "100";
               dropSize = "0.25";
               splashSize = "0.25";
               splashMS = "250";
               animateSplashes = "1";
               dropAnimateMS = "0";
               fadeDist = "0";
               fadeDistEnd = "0";
               useTrueBillboards = "0";
               useLighting = "1";
               glowIntensity = "0 0 0 0";
               reflect = "0";
               rotateWithCamVel = "1";
               doCollision = "1";
               hitPlayers = "0";
               hitVehicles = "0";
               followCam = "1";
               useWind = "1";
               minSpeed = "1";
               maxSpeed = "1.5";
               minMass = "0.75";
               maxMass = "0.85";
               useTurbulence = "0";
               numDrops = "1000";
               maxTurbulence = "0.1";
               turbulenceSpeed = "0.2";
               dataBlock = "Snow";
               position = "0 0 400";
               rotation = "1 0 0 0";
               scale = "1 1 1";
               canSave = "0";
               canSaveDynamicFields = "0";
            };
            MissionGroup.add(%snow);

         case "sand":
            %sand = new Precipitation(precip) {
               numDrops = "1024";
               boxWidth = "200";
               boxHeight = "100";
               dropSize = "0.25";
               splashSize = "0.25";
               splashMS = "250";
               animateSplashes = "1";
               dropAnimateMS = "0";
               fadeDist = "0";
               fadeDistEnd = "0";
               useTrueBillboards = "0";
               useLighting = "1";
               glowIntensity = "0 0 0 0";
               reflect = "0";
               rotateWithCamVel = "1";
               doCollision = "1";
               hitPlayers = "0";
               hitVehicles = "0";
               followCam = "1";
               useWind = "1";
               minSpeed = "1";
               maxSpeed = "1.5";
               minMass = "0.75";
               maxMass = "0.85";
               useTurbulence = "0";
               numDrops = "1000";
               maxTurbulence = "0.1";
               turbulenceSpeed = "0.2";
               dataBlock = "Sandstorm";
               position = "0 0 400";
               rotation = "1 0 0 0";
               scale = "1 1 1";
               canSave = "0";
               canSaveDynamicFields = "0";
            };
            MissionGroup.add(%sand);
      }
   }
}

//--------------------------------------------------------------------------
// Fireball data
//--------------------------------------------------------------------------

datablock ParticleData(FireballAtmosphereCrescentParticle)
{
   dragCoefficient      = 2;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.2;
   constantAcceleration = -0.0;
   lifetimeMS           = 600;
   lifetimeVarianceMS   = 000;
   textureName          = "art/particles/crescent1";
   colors[0]     = "1.0 0.75 0.2 1.0";
   colors[1]     = "1.0 0.75 0.2 0.5";
   colors[2]     = "1.0 0.75 0.2 0.0";
   sizes[0]      = 2.0;
   sizes[1]      = 4.0;
   sizes[2]      = 5.0;
   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(FireballAtmosphereCrescentEmitter)
{
   ejectionPeriodMS = 25;
   periodVarianceMS = 0;
   ejectionVelocity = 20;
   velocityVariance = 5.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 80;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   orientParticles  = true;
   lifetimeMS       = 200;
   particles = "FireballAtmosphereCrescentParticle";
};

datablock ParticleData(FireballAtmosphereExplosionParticle)
{
   dragCoefficient      = 2;
   gravityCoefficient   = 0.2;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   lifetimeMS           = 750;
   lifetimeVarianceMS   = 150;
   textureName          = "art/particles/smoke";
   colors[0]     = "0.56 0.36 0.26 1.0";
   colors[1]     = "0.56 0.36 0.26 0.0";
   sizes[0]      = 1;
   sizes[1]      = 2;
};

datablock ParticleEmitterData(FireballAtmosphereExplosionEmitter)
{
   ejectionPeriodMS = 7;
   periodVarianceMS = 0;
   ejectionVelocity = 12;
   velocityVariance = 1.75;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 60;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvances = false;
   particles = "FireballAtmosphereExplosionParticle";
};

datablock ExplosionData(FireballExplosion)
{
   //soundProfile   = "";
   particleEmitter = FireballAtmosphereExplosionEmitter;
   particleDensity = 250;
   particleRadius = 1.25;
   faceViewer = true;

   emitter[0] = FireballAtmosphereCrescentEmitter;

   shakeCamera = true;
   camShakeFreq = "10.0 9.0 9.0";
   camShakeAmp = "70.0 70.0 70.0";
   camShakeDuration = 1.3;
   camShakeRadius = 15.0;
};

datablock ParticleData(FireballAtmosphereParticle)
{
   dragCoeffiecient     = 0.0;
   gravityCoefficient   = -0.0;
   inheritedVelFactor   = 0.85;
   lifetimeMS           = 1600;
   lifetimeVarianceMS   = 0;
   textureName          = "art/particles/flameExplosion";
   useInvAlpha = false;
   spinRandomMin = -100.0;
   spinRandomMax = 100.0;

   colors[0] = "1.0 0.7 0.5 1.0";
   colors[1] = "1.0 0.5 0.2 1.0";
   colors[2] = "1.0 0.25 0.1 0.0";

   sizes[0]  = 10.0;
   sizes[1]  = 4.0;
   sizes[2]  = 2.0;

   times[0]  = 0.0;
   times[1]  = 0.2;
   times[2]  = 1.0;
};

datablock ParticleEmitterData(FireballAtmosphereEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 0.25;
   velocityVariance = 0.0;
   thetaMin         = 0.0;
   thetaMax         = 30.0;

   particles = "FireballAtmosphereParticle";
};

datablock DebrisData(FireballAtmosphereDebris)
{
   emitters[0] = FireballAtmosphereEmitter;
   explosion = FireballExplosion;
   explodeOnMaxBounce = true;
   elasticity = 0.0;
   friction = 1.0;
   lifetime = 100.0;
   lifetimeVariance = 0.0;
   numBounces = 0;
   bounceVariance = 0;
   ignoreWater = false;
};             
/*
datablock FireballAtmosphereData(Fireball)
{
   fireball = FireballAtmosphereDebris;
};
*/
//function ExplosionData::onNewDataBlock(%this, %obj)
function FireballExplosion::onNewDataBlock(%this, %obj)
{
   if ( %obj != 0 )
   {
      // This needs its own radius damage function because radiusDamage() requires GameBase source object.
      //radiusDamage( %this, 0, %obj.getPosition(), ( %this.camShakeRadius * 0.7 ), 10, "Meteor", 2500 );
      //warn("FireballExplosion::onNewDataBlock(" SPC %this.getName() @", "@ %obj SPC ")");
      //error("Position:" SPC %obj.getPosition());
   }
}
