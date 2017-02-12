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

datablock ItemData(VehiclePack)
{
   // Mission editor category, this datablock will show up in the
   // specified category under the "shapes" root category.
   category = "Specials";
   className = "Special";

   // Basic Item properties
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

   image = VehiclePackImage;

   pickUpName = 'Vehicle Deployer';
   pickupSound = WeaponPickupSound;
   throwSound = WeaponThrowSound;

   lightType = "NoLight";
};

datablock ShapeBaseImageData(VehiclePackImage)
{
   shapeFile = "art/editor/invisible.dts";
   computeCRC = false;
   cloakable = true;
   item = VehiclePack;
   //mountPoint = 2;
   mass = 2;
   //offset     = "0 0 -0.1"; // L/R - F/B - T/B
   //offset = "0 +0.01 0";
   //rotation = "1 0 0 180";

   deployed = Talon;
   //deploySound = TowerDeploySound;
   maxDepSlope = 45;
   flatMinDeployDis   = 10;
   flatMaxDeployDis   = 15;
   minDeployDis       = 15;
   maxDeployDis       = 20;

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

function VehiclePackImage::testMaxDeployed(%data, %player)
{
   %vehData = $NameToVehicle[%player.client.selectedVehicle];
   if ( !isObject( %vehData ) )
      return 0;

   if ( ( $Game::VehicleMax[%vehData.getName()] - $VehicleTotalCount[%player.team, %vehData.getName()] ) > 0 )
      return 0;
   else
      return 1;
}

function ShapeBaseImageData::testObjectTooClose(%data, %surfacePt, %player)
{
   return 0;
}

function VehiclePackImage::testVerticleObstruction(%data, %player)
{
   %height = VectorAdd(%data.surfacePt, "0 0 50");
   %mask = $TypeMasks::ShapeBaseObjectType;
   %obstruction = ContainerRayCast(%data.surfacePt, %height, %mask);
   return %obstruction;
}

function VehiclePackImage::testObjectInterference(%data, %pos)
{
   return 0;
}

function VehiclePackImage::onActivate(%data, %obj, %slot)
{
   %obj.client.deploySpecial = false;
   %data.testDeployConditions( %obj, %slot );
}

function VehiclePackImage::onDeactivate(%data, %obj, %slot)
{
   %obj.client.deploySpecial = true;
}

function VehiclePackImage::displayErrorMsg(%data, %player, %slot, %error)
{
   deactivateDeploySensor(%player);
   
   %errorSnd = 'art/sound/gui/deploy_error.wav';
   switch (%error)
   {
      case $NotDeployableReason::None:
         %data.onDeploy(%player, %slot);
         messageClient(%player.client, 'MsgTeamDeploySuccess', "");
         return;

      case $DeployError::MaxDeployed:
         %msg = '\c2Your team has reached it\'s capacity of this vehicle.%1';

      case $DeployError::NoSurfaceFound:
         %msg = '\c2Vehicle must be created closer to your position.%1';

      case $DeployError::SlopeTooGreat:
         %msg = '\c2Surface is too steep to create vehicle.%1';

      case $DeployError::SelfTooClose:
         %msg = '\c2You are too close to the surface you are trying to create a vehicle on.%1';

      case $DeployError::ObjectTooClose:
         %msg = '\c2You cannot place this item so close to another object.%1';

      case $DeployError::VerticleObstruction:
         %msg = '\c2Position above creation point is obstructed.%1';

      case $DeployError::ObjectInterference:
         %msg = '\c2You must be within 150 meters of a Fusion Tower to create vehicle.%1';

      case $DeployError::NoTerrainFound:
         %msg = '\c2You cannot create vehicle here.%1';

      case $DeployError::MissionArea:
         %msg = '\c2You must create vehicle within the mission area.%1';

      default:
         %msg = '\c2Creation of vehicle failed.';
   }
   messageClient(%player.client, 'MsgDeployFailed', %msg, %errorSnd);
}

function VehiclePackImage::onDeploy(%data, %obj, %slot)
{
   if ( isEventPending( %obj.deployCheckThread ) )
      cancel( %obj.deployCheckThread );

   %vehData = $NameToVehicle[%obj.client.selectedVehicle];
   %veh = %vehData.create(%obj.team);
   if ( isObject( %veh ) )
   {
      %obj.unmountImage( %slot ); // Unmount the pack
      %obj.decInventory( %data.item, 1 ); // Remove from inventory
      $VehicleTotalCount[%obj.team, %vehData.getName()]++;
      %vehData.isMountable( %veh, false );
      %vehData.schedule( 6500, "isMountable", %veh, true );
      MissionCleanup.add( %veh );

      %rot = %obj.getEulerRotation();
      %x = getword( %data.surfacePt, 0 );
      %y = getword( %data.surfacePt, 1 );
      %z = ( getword( %data.surfacePt, 2 ) + %vehData.createHoverHeight );

      %veh.setTransform( %x SPC %y SPC %z SPC "0 0 1 " @ getWord(%rot, 2) );
      //%veh.setTransform( bumpZ( %data.surfacePt, %vehData.createHoverHeight ) SPC "0 0 1 " @ getWord(%rot, 2) );
      %veh.setCloaked( 1 );
      %veh.schedule(4800, "setCloaked", false);
   }
   else
   {
      messageClient( %obj.client, 'MsgDeployFailed', '\c2Failed to create vehicle!' );
   }
}

//-----------------------------------------------------------------------------
// SMS Inventory

SmsInv.AllowItem("armor\tSoldier\t1");
SmsInv.AddItem(VehiclePack, "Vehicle Deployer", 1);

