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

$DeployError::None                = 0;
$DeployError::MaxDeployed         = 1;
$DeployError::NoSurfaceFound      = 2;
$DeployError::SlopeTooGreat       = 3;
$DeployError::SelfTooClose        = 4;
$DeployError::ObjectTooClose      = 5;
$DeployError::VerticleObstruction = 6;
$DeployError::ObjectInterference  = 7;
$DeployError::NoTerrainFound      = 8;
$DeployError::NoInteriorFound     = 9;
$DeployError::MissionArea         = 10;

$MinDeployableDistance            =  1.5;
$MaxDeployableDistance            =  15.0; //meters from body
             
//-----------------------------------------------------------------------------

function addToDeploySet(%object)
{
   %depSet = nameToID("MissionCleanup/Deployed");
   if ( %depSet <= 0 )
   {
      %depSet = new SimSet( "Deployed" );
      MissionCleanup.add( %depSet );
   }
   %depSet.add(%object);
}

//-----------------------------------------------------------------------------
// Deployable Procedures

function ShapeBaseImageData::testMaxDeployed(%data, %player)
{
   return $TeamDeployedCount[%player.team, %data.item] >= $TeamDeployableMax[%data.item];
}

function ShapeBaseImageData::testNoSurfaceInRange(%data, %player)
{
   return !%player.doRaycast( $MaxDeployableDistance, ( $TypeMasks::TerrainObjectType | $TypeMasks::StaticObjectType ) );
}

function ShapeBaseImageData::testSlopeTooGreat(%data, %surface, %surfaceNrm)
{
   if ( %surface )
      return getTerrainAngle( %surfaceNrm ) > %data.maxDepSlope;
}

function ShapeBaseImageData::testSelfTooClose(%data, %surfacePt, %player)
{
   InitContainerRadiusSearch( %surfacePt, $MinDeployableDistance, $TypeMasks::PlayerObjectType );

   return containerSearchNext() == %player;
}

function ShapeBaseImageData::testObjectTooClose(%data, %surfacePt, %player)
{
   %mask = ( $TypeMasks::VehicleObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::MoveableObjectType );
 
   InitContainerRadiusSearch( %surfacePt, $MinDeployableDistance, %mask );

   %test = containerSearchNext();
   return %test;
}

function ShapeBaseImageData::testVerticleObstruction(%data, %player)
{
   return 0;

   %height = VectorAdd(%data.surfacePt, "0 0 30");
   %mask = $TypeMasks::ShapeBaseObjectType | $TypeMasks::StaticObjectType;
   %obstruction = ContainerRayCast(%data.surfacePt, %height, %mask);
   return %obstruction;
}

function ShapeBaseImageData::testObjectInterference(%data, %surfacePt)
{
   return 0;
}

function ShapeBaseImageData::testNoTerrainFound(%data, %surface)
{
   return 0;
   return %surface.getClassName() !$= TerrainBlock;
}

function ShapeBaseImageData::testNoInteriorFound(%data, %surface)
{
   return 0;
   return %surface.getClassName() !$= InteriorInstance;
}

function ShapeBaseImageData::testMissionArea(%data, %player)
{
   if ( %player.outOfBounds == true )
      return 1;

   return 0;
}

function ShapeBaseImageData::checkPositions(%data, %pos1, %pos2)
{
   %passed = 1;
   if((mFloor(getWord(%pos1, 0)) - mFloor(getWord(%pos2,0))))
      %passed = 0;   
   if((mFloor(getWord(%pos1, 1)) - mFloor(getWord(%pos2,1))))
      %passed = 0;   
   if((mFloor(getWord(%pos1, 2)) - mFloor(getWord(%pos2,2))))
      %passed = 0;
   return %passed;      
}

function ShapeBaseImageData::testDeployConditions(%data, %player, %slot)
{
   //error("ShapeBaseImageData::testDeployConditions(" SPC %data.getName() @", "@ %player.client.nameBase @", "@ %slot SPC ")");

   cancel( %player.deployCheckThread );
   %disqualified = $DeployError::None;  //default everything is a-ok
   $MaxDeployDistance = %data.maxDeployDis; 
   $MinDeployDistance = %data.minDeployDis; 
   %surface = %player.doRaycast( $MaxDeployDistance, ( $TypeMasks::TerrainObjectType | $TypeMasks::StaticObjectType ) );

   if ( %surface )  
   {  
      %surfacePt  = posFromRaycast(%surface);
      %surfaceNrm = normalFromRaycast(%surface);

      // Check the eye point to see if anything is objstructing its view...
      %eyeTrans = %player.getEyeTransform();
      %eyePos   = posFromTransform(%eyeTrans);

      %searchResult = containerRayCast( %eyePos, %surfacePt, -1, %player );
      if ( !%searchResult )
      {
         %data.surface = %surface;
         %data.surfacePt = %surfacePt;
         %data.surfaceNrm = %surfaceNrm;
      }
      else
      {
         %data.surface = %surface;
         %data.surfacePt = %surfacePt;
         %data.surfaceNrm = %surfaceNrm;
      }

      if ( !getTerrainAngle( %surfaceNrm ) && %data.flatMaxDeployDis !$= "" )
      {
         $MaxDeployDistance = %data.flatMaxDeployDis; 
         $MinDeployDistance = %data.flatMinDeployDis; 
      }
   }

   if ( %data.testMaxDeployed( %player ) )
   {
      %disqualified = $DeployError::MaxDeployed;
   }
   else if ( %data.testNoSurfaceInRange( %player ) )
   {
      %disqualified = $DeployError::NoSurfaceFound;
   }
   else if ( %data.testSlopeTooGreat( %surface, %surfaceNrm ) )
   {
      %disqualified = $DeployError::SlopeTooGreat;
   }
   else if ( %data.testSelfTooClose( %player, %surfacePt ) )
   {
      %disqualified = $DeployError::SelfTooClose;
   }
   else if ( %data.testObjectTooClose( %surfacePt, %player ) )
   {
      %disqualified = $DeployError::ObjectTooClose;   
   }
   else if ( %data.testVerticleObstruction( %surfacePt ) )
   {
      %disqualified = $DeployError::VerticleObstruction;   
   }
   else if ( %data.testObjectInterference( %surfacePt ) )
   {
      %disqualified = $DeployError::ObjectInterference;   
   }
   else if ( %data.testNoTerrainFound( %surface ) )
   {
      %disqualified = $DeployError::NoTerrainFound;
   }
   else if ( %data.testMissionArea( %player ) )
   {
      %disqualified = $DeployError::MissionArea;
   }

   if ( %player.getMountedImage( %slot ) == %data )  //player still have the item?
   {
      if ( %disqualified )
         activateDeploySensorRed( %player );
      else
         activateDeploySensorGrn( %player );      

      if ( %player.client.deploySpecial == true )
         %data.attemptDeploy(%player, %slot, %disqualified);       
      else
      {
         %player.deployCheckThread = %data.schedule(25, "testDeployConditions", %player, %slot); //update checks every 25 milliseconds
      }
   }
   else
       deactivateDeploySensor( %player );
}

function ShapeBaseImageData::attemptDeploy(%data, %player, %slot, %disqualified)
{
   deactivateDeploySensor(%player);
   %data.displayErrorMsg(%player, %slot, %disqualified);
}  

function activateDeploySensorRed(%pl)
{
   if(%pl.deploySensor !$= "red")
   {
      messageClient(%pl.client, 'msgDeploySensorRed', "");
      %pl.deploySensor = "red";
   }
}

function activateDeploySensorGrn(%pl)
{
   if(%pl.deploySensor !$= "green")
   {
      messageClient(%pl.client, 'msgDeploySensorGrn', "");
      %pl.deploySensor = "green";
   }
}  

function deactivateDeploySensor(%pl)
{
   if (%pl.deploySensor !$= "")
   {
      messageClient(%pl.client, 'msgDeploySensorOff', "");
      %pl.deploySensor = "";
   }
}

function ShapeBaseImageData::displayErrorMsg(%data, %plyr, %slot, %error)
{
   deactivateDeploySensor(%plyr);
   
   %errorSnd = '~wdeploy_error.wav';
   switch (%error)
   {
      case $NotDeployableReason::None:
         %data.onDeploy(%plyr, %slot);
         messageClient(%plyr.client, 'MsgTeamDeploySuccess', "");
         return;

      case $DeployError::MaxDeployed:
         %msg = '\c2Your team has reached it\'s capacity for this item.%1';

      case $DeployError::NoSurfaceFound:
         %msg = '\c2Item must be placed within reach.%1';

      case $DeployError::SlopeTooGreat:
         %msg = '\c2Surface is too steep to place this item on.%1';

      case $DeployError::SelfTooClose:
         %msg = '\c2You are too close to the surface you are trying to place the item on.%1';

      case $DeployError::ObjectTooClose:
         %msg = '\c2You cannot place this item so close to another object.%1';

      case $DeployError::VerticleObstruction:
         %msg = '\c2Position above deploy point obstructed.%1';

      case $DeployError::ObjectInterference:
         %msg = '\c2Interference from a nearby object prevents placement here.%1';

      case $DeployError::NoTerrainFound:
         %msg = '\c2You must place this on outdoor terrain.%1';

      case $DeployError::MissionArea:
         %msg = '\c2You must place this item within the mission area.%1';

      default:
         %msg = '\c2Deploy failed.';
   }
   messageClient(%plyr.client, 'MsgDeployFailed', %msg, %errorSnd);
}

function ShapeBaseImageData::onDeploy(%data, %player, %slot)
{
   if ( isEventPending( %player.deployCheckThread ) )
      cancel( %player.deployCheckThread );

   // take the deployable off the player's back and out of inventory
   %player.decInventory(%data.item, 1); // This will unmount the image as well
   
   // create the actual deployable
   %class = %data.deployed.getClassName();
   %deplObj = %data.deployed.create();
   %deplObj.team = %player.team;
   %deplObj.setTeamId( %player.team );
   %deplObj.owner = %player.client;

   %rot = %data.getInitialRotation(%player);
   %deplObj.setTransform(%data.surfacePt SPC %rot);

   // set the recharge rate right away
   if(%deplObj.getDatablock().rechargeRate)
      %deplObj.setRechargeRate(%deplObj.getDatablock().rechargeRate);

   // Power
   %deplObj.setSelfPowered();

   // play the deploy sound
   //serverPlay3D(%data.deploySound, %deplObj.getTransform());

   // increment the team count for this deployed object
   $TeamDeployedCount[%player.team, %data.item]++;

   %deplObj.justdeployed = 1;
   schedule(2500, %deplObj, "resetjustdeployed", %deplObj);

   addToDeploySet(%deplObj);
   return %deplObj;  
}

function resetjustdeployed(%obj)
{
   %obj.justdeployed = 0;
}

function ShapeBaseImageData::getInitialRotation(%item, %plyr)
{
   //return rotFromTransform(%plyr.getTransform());

   // For angles
   %rotAxis = vectorNormalize(vectorCross(%item.surfaceNrm, "0 0 1"));
   if (getWord(%item.surfaceNrm, 2) == 1 || getWord(%item.surfaceNrm, 2) == -1)
      %rotAxis = vectorNormalize(vectorCross(%item.surfaceNrm, "0 1 0"));
    
   return %rotAxis SPC mACos(vectorDot(%item.surfaceNrm, "0 0 1"));
}
