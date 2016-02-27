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

datablock ItemData(FusionTower)
{
   category = "Deployable";
   className = "Special";
   shapeFile = "art/shapes/items/pack.dts";
   computeCRC = false;
   mass = 2;
   drag = 0.5;
   density = 2;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   maxVelocity = 20;
   dynamicType = $TypeMasks::ItemObjectType;

   image = FusionTowerImage;
   pickUpName = 'fusion tower';
   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;

   lightType = "NoLight";
};

datablock ShapeBaseImageData(FusionTowerImage)
{
   shapeFile = "art/editor/invisible.dts";
   computeCRC = false;
   cloakable  = true;

   item = FusionTower;
   mountPoint = 2;
   mass       = 2;
   //offset     = "0 0 -0.1"; // L/R - F/B - T/B
   offset = "0 +0.01 0";
   rotation = "1 0 0 180";
   deployed = Deployed1FusionTower;
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

//-----------------------------------------------------------------------------

function FusionTowerImage::onActivate(%data, %obj, %slot)
{
   //error("FusionTowerImage::onActivate(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");
   %obj.client.deploySpecial = false;
   %data.testDeployConditions( %obj, %slot );
}

function FusionTowerImage::onDeactivate(%data, %obj, %slot)
{
   %obj.client.deploySpecial = true;
}

function FusionTowerImage::onUnmount(%data, %obj, %slot)
{
   //error("FusionTowerImage::onUnmount(" SPC %data.getName() @", "@ %obj.client.nameBase @", "@ %slot SPC ")");
   %obj.setImageTrigger(%slot, false);
}

function FusionTowerImage::testVerticleObstruction(%data, %player)
{
   %height = VectorAdd(%data.surfacePt, "0 0 500");
   %mask = $TypeMasks::ShapeBaseObjectType | $TypeMasks::InteriorObjectType;
   %obstruction = ContainerRayCast(%data.surfacePt, %height, %mask);
   return %obstruction;
}

function FusionTowerImage::testObjectInterference(%data, %pos)
{
   InitContainerRadiusSearch(%pos, 150, $TypeMasks::StaticShapeObjectType );

   %found = false;
   while( ( %obj = containerSearchNext() ) != 0 )
   {
      %name = %obj.getDataBlock().getName();
      if ( %name $= "Deployed1FusionTower" || %name $= "Deployed2FusionTower" )
      {
         %found = true;
         break;
      }
   }
   return %found;
}

function FusionTowerImage::testNoTerrainFound(%data, %surface)
{
   return %data.surface.getClassName() !$= TerrainBlock;
}

function FusionTowerImage::displayErrorMsg(%data, %player, %slot, %error)
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
         %msg = '\c2Your team has reached it\'s capacity of Fusion Towers.%1';

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

function FusionTowerImage::onDeploy(%data, %player, %slot)
{
   //error("FusionTowerImage::onDeploy(" SPC %data.getName() @", "@ %player.client.nameBase @", "@ %slot SPC ")");

   if ( isEventPending( %player.deployCheckThread ) )
      cancel( %player.deployCheckThread );

   if ( %player.dropOrder > 0 )
   {
      messageClient( %player.client, 'MsgError', '\c2A supply drop for you is allready active. Please stand by.' );
      return;
   }

   // take the deployable off the player's back and out of inventory
   %player.unmountImage(%slot);
   %player.decInventory(%data.item, 1);

   // play the deploy sound
   //serverPlay3D(%data.deploySound, %deplObj.getTransform());

   // increment the team count for this deployed object
   $TeamDeployedCount[%player.team, %data.item]++;
   messageClient(%player.client, 'MsgDeploySuccess', '\c2Fusion Tower will be air dropped shortly.');
   %rot = %data.getInitialRotation(%player);

   // Tell the inventory class to handle it so we can overload this functionality.
   SmsInv.startDropSupplies(%player.client, ( %data.surfacePt SPC %rot ), "tower"); // inventory.cs

   //return schedule( getRandom( 2000, 5000 ), %player, "TowerDrop", %player, ( %data.surfacePt SPC %rot ) );
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowItem("armor\tSoldier\t1");
SmsInv.AddItem(FusionTower, "Fusion Tower", 1);
