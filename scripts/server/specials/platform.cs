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

datablock ItemData(Platform)
{
   category = "Specials"; // Not only used in mission editor but also for cleaning up deploy counts
   className = "Special";
   shapeFile = "art/shapes/items/ammo/futuristic_ammo_box_01.dts";
   computeCRC = false;
   mass = 2;
   drag = 0.5;
   density = 2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   maxVelocity = 20;
   dynamicType = $TypeMasks::ItemObjectType;

   image = PlatformImage;
   pickUpName = 'platform';
   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;

   lightType = "NoLight";
};

datablock ShapeBaseImageData(PlatformImage)
{
   shapeFile = "art/editor/invisible.dts";
   emap = false;
   computeCRC = false;
   cloakable  = true;

   mountPoint = 2;
   mass       = 2;
   //offset     = "0 0 -0.1"; // L/R - F/B - T/B
   offset = "0 +0.01 0";
   rotation = "1 0 0 180";

   item = Platform;
   deployed = DeployedPlatform;
   //deploySound = TowerDeploySound;

   maxDepSlope = 30;
   flatMinDeployDis   = 1.0;
   flatMaxDeployDis   = 8.0;
   minDeployDis       = 2.5;
   maxDeployDis       = 10.0;

   stateName[0]                    = "Preactivate";
   stateSequence[0]                = "activation";
   stateTransitionOnTriggerDown[0] = "Activate";

   stateName[1]                    = "Activate";
   stateScript[1]                  = "onActivate";
   stateSequence[1]                = "fire";
   stateTransitionOnTriggerUp[1]   = "Deactivate";

   stateName[2]                    = "Deactivate";
   stateScript[2]                  = "onDeactivate";
   stateTimeoutValue[2]            = 0.2;
   stateTransitionOnTimeout[2]     = "Preactivate";
};

datablock StaticShapeData(DeployedPlatform : StaticShapeDamageScale)
{
   category = "Deployable";

   shapeFile = "art/shapes/storage/pallets/pallet_02.dts";
   emap = true;
   dynamicType = $TypeMasks::StaticShapeObjectType;
   computeCRC = false;
   scale = "10 10 10";

   cameraMaxDist = 2.20449;
   cameraMinDist = 0.2;
   cameraDefaultFov = 90;
   cameraMinFov = 5;
   cameraMaxFov = 120;
   firstPersonOnly = false;
   useEyePoint = false;
   observeThroughObject = false;

   mass = 10;
   drag = 1;
   density = 20;

   isShielded = false;
   repairRate = 0;
   energyPerDamagePoint = 75;
   maxEnergy = 50;
   rechargeRate = 0;
   inheritEnergyFromMount = false;

   isInvincible = false;
   maxDamage = 15.1; // Must be higher then destroyed level
   destroyedLevel = 15.0;
   disabledLevel = 14.0;

   debrisShapeName =  "art/shapes/weapons/grenade/grenadeDebris.dts";
   debris = SmallExplosionDebris;
   renderWhenDestroyed = true;

   explosion = SmallExplosion;
   damageRadius = 2.0;
   radiusDamage = 5.0;
   damageType = $DamageType::Explosion;
   areaImpulse = 500;

   nameTag = 'Platform';
   damageSound = "";
   ambientSound = "";

   // Radius damage

   canImpulse = false;
   item = Platform; // UnDeploy flag
   deployedObject = true;
};

//-----------------------------------------------------------------------------

function PlatformImage::onActivate(%data, %obj, %slot)
{
   //error("PlatformImage::onActivate(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");
   %obj.client.deploySpecial = false;
   %data.testDeployConditions( %obj, %slot );
}

function PlatformImage::onDeactivate(%data, %obj, %slot)
{
   %obj.client.deploySpecial = true;
}

function PlatformImage::onUnmount(%data, %obj, %slot)
{
   //error("FusionTowerImage::onUnmount(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");
   %obj.setImageTrigger(%slot, false);
}

function PlatformImage::testVerticleObstruction(%data, %player)
{
   %height = VectorAdd(%data.surfacePt, "0 0 500");
   %mask = $TypeMasks::ShapeBaseObjectType;
   %obstruction = ContainerRayCast(%data.surfacePt, %height, %mask);
   return %obstruction;
}

function PlatformImage::testObjectInterference(%data, %pos)
{
   return( false );
}

function PlatformImage::testNoTerrainFound(%data, %surface)
{
   return( false );
}

function PlatformImage::testMaxDeployed(%data, %player)
{
   return( false );
}

function PlatformImage::displayErrorMsg(%data, %player, %slot, %error)
{
   deactivateDeploySensor(%player);
   
   %errorSnd = '~wdeploy_error.wav';
   switch (%error)
   {
      case $NotDeployableReason::None:
         %data.onDeploy(%player, %slot);
         messageClient(%player.client, 'MsgTeamDeploySuccess', "");
         return;

      case $DeployError::MaxDeployed:
         %msg = '\c2Your team has reached it\'s capacity of Platforms.%1';

      case $DeployError::NoSurfaceFound:
         %msg = '\c2Tower must be air dropped within reach.%1';

      case $DeployError::SlopeTooGreat:
         %msg = '\c2Surface is too steep to air drop a tower on.%1';

      case $DeployError::SelfTooClose:
         %msg = '\c2You are too close to the surface you are trying to air drop a tower on.%1';

      case $DeployError::ObjectTooClose:
         %msg = '\c2You cannot air drop a tower so close to another object.%1';

      case $DeployError::VerticleObstruction:
         %msg = '\c2Position above drop point obstructed.%1';

      case $DeployError::ObjectInterference:
         %msg = '\c2Interference from a nearby Fusion Tower prevents an air drop here.%1';

      case $DeployError::NoTerrainFound:
         %msg = '\c2You must air drop a tower on outdoor terrain.%1';

      case $DeployError::NoInteriorFound:
         %msg = '\c2You must air drop a tower on a man made surface.%1';

      case $DeployError::MissionArea:
         %msg = '\c2You cannot air drop a tower outside the mission area.%1';

      default:
         %msg = '\c2Deploy failed.';
   }
   messageClient(%player.client, 'MsgDeployFailed', %msg, %errorSnd);
}

function PlatformImage::getNewScale(%item, %plyr)
{
   %maxwidth = 20; // Max Width of platform
   %mask = $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType;
   %vec = VectorNormalize(%plyr.getEyeVector());
   %x1 = getWord(%vec, 0);
   %y1 = getWord(%vec, 1);
   %vec = %x1 SPC %y1 @ " 0"; //Forward
   %vec2 = %y1 * -1 SPC %x1 @ " 0"; //Left
   %vec3 = %y1 SPC %x1 * -1 @ " 0"; //Right
   %vec4 = %x1 * -1 SPC %y1 * -1 @ " 0"; //Back
   if (getWord(%item.surfaceNrm, 2) > 0.45)
   {
      %pos = VectorAdd(%item.surfacePt, VectorScale(%vec, 2));
      %pos = VectorAdd(%pos, "0 0 -0.5");
   }
   else if (getWord(%item.surfaceNrm, 2) < -0.45)
   {
      %pos = VectorAdd(%item.surfacePt, VectorScale(%vec, 2));
      %pos = VectorAdd(%pos, "0 0 +0.5");
   }
   else
      %pos = VectorAdd(%item.surfacePt, VectorScale(%item.surfaceNrm, 2));

   %endPos = vectorAdd(%pos, VectorScale(%vec, %maxwidth / 2));
   %res = containerRayCast(%pos, %endpos, %mask, 0);
   if (%res)
      %FDis = VectorDist(getword(%res,1) SPC getword(%res,2) @ " 0", getword(%pos, 0) SPC getword(%pos, 1) @ " 0");
   else
      %FDis = %maxwidth / 2.5;

   %endPos = vectorAdd(%pos, VectorScale(%vec4, %maxwidth / 2));
   %res = containerRayCast(%pos, %endpos, %mask, 0);
   if (%res)
      %BDis = VectorDist(getword(%res,1) SPC getword(%res,2) @ " 0", getword(%pos, 0) SPC getword(%pos, 1) @ " 0");
   else
      %BDis = %maxwidth / 2.5;

   %endPos = vectorAdd(%pos, VectorScale(%vec2, %maxwidth / 2));
   %res = containerRayCast(%pos, %endpos, %mask, 0);
   if (%res)
      %RDis = VectorDist(getword(%res,1) SPC getword(%res,2) @ " 0", getword(%pos, 0) SPC getword(%pos, 1) @ " 0");
   else
      %RDis = %maxwidth / 2;

   %endPos = vectorAdd(%pos, VectorScale(%vec3, %maxwidth / 2));
   %res = containerRayCast(%pos, %endpos, %mask, 0);
   if (%res)
      %LDis = VectorDist(getword(%res,1) SPC getword(%res,2) @ " 0", getword(%pos, 0) SPC getword(%pos, 1) @ " 0");
   else
      %LDis = %maxwidth / 2;

   if ((%FDis > 0.01) && (%BDis > 0.01) && (%RDis > 0.01) && (%LDis > 0.01))
   {
      %pos = VectorAdd(%pos, VectorScale(%vec, (%FDis - %BDis) * 1.2 / 2));
      %pos = VectorAdd(%pos, VectorScale(%vec2, (%RDis - %LDis) / 2));
      %item.surfacePt = %pos;
      %wid = %FDis + %BDis;
      %wid2 = %RDis + %LDis;
   }
   else
   {
      // Set as our normal size
      %pos = VectorAdd(%item.surfacePt, VectorScale(%item.surfaceNrm, 0.5));
      %wid = 5.8;
      %wid2 = 5.1;
   }
   return( %wid / 3.85 @ " " @ %wid2 / 3 @ " 1" );
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowItem("armor\tSoldier\t1");
SmsInv.AddItem(Platform, "Deployable Platform", 1);
