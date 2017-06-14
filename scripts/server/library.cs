//----------------------------------------------------------------------------------------------------------------
// Library Functions v2.1
//----------------------------------------------------------------------------------------------------------------
// These are nifty lil functions that are useful for other things then solely supporting the functions they
// were written to help.

//*************************************************************************************
//torque engine tidbits
//
// The rotation value returned for the z axis of an object in torque, is specified by
// the last 2 words.(it can be made a single value by: getWord(%rot, 2) * getWord(%rot, 3))
// This value will remain positive until the angle is larger than 4.18879 radians OR 240
// degrees, any angle larger than this will revert to a negative value.
// GOing clockwise around the torque compass there are the following degree values...
//
// North -  0 deg, 0 Rad
// East  - 90 deg, 1.5707963 rad
// South- 180 deg, 3.1415926 rad
//        240 deg, 4.18879 rad (changeover point, 30 degrees past south)
// West - 270 deg, -1.5707963 rad (the negative value for due east)
//
//  pi = 3.1415926 (3.14159265358979323846)
// 2pi = 6.2831853 (6.28318530717958647692)
//
// Table X degrees = X radians
//  1d = 0.0174532
//  5d = 0.0872664
// 10d = 0.1745329
// 45d = 0.785398
// 60d = 1.0471974
// 90d = 1.5707963
//180d = 3.1415926
//
// when getting a rotation via an objects ID.rotation, the final word of the rotation
// is in degrees, NOT radians!
// ID.rotation = x y z amount(in degrees)


// getWorldBox returns box.min.x, box.min.y, box.min.z, box.max.x, box.max.y, box.max.z
// which are two diagonaly opposiing corners of the box

// Just thought someone might find this useful :D  -Founder
//*************************************************************************************

//-------------------------------------------------------------------------------------
// Rotations, positions, vectors, and Matricies OH MY!
//-------------------------------------------------------------------------------------
//set some useful globals
$Pi = 3.14159;
$TwoPi = 6.28319;

$North   = 0;
$NE      = 0.785398; //45
$East    = 1.5708;   //90
$SE      = 2.35619;  //135
$South   = $Pi;      //180
$SW      = 3.92699;  //225
$West    = 4.71239;  //270
$NW      = 5.49779;  //315

//----Vectors----

//this function serves a similar purpose to vectorScale, except instead of scaling it
//makes the specified vector.length equal to the number provided
function setVectorLength(%vec, %desiredLength)
{
   error("setVectorLength vec:" SPC %vec SPC "desiredLength" SPC %desiredLength);
   //this only works if we start with a unit vector so we have the relative valuse for x y and z
   %vecNorm = vectorLen(%vec) > 1 ? vectorNormalize(%vec) : %vec;

   // x + y + z = the number to devide the length by in order to get the proper scale by which to
   //multiply the vector elements
   %nl = mAbs(getWord(%vecNorm, 0)) + mAbs(getWord(%vecNorm, 1)) + mAbs(getWord(%vecNorm, 2));
   %scale = %desiredLength / %nl;
   //now vector scale it and send it back
   %newVec = vectorScale(%vecNorm, %scale);
   error("finalVec:" SPC %newVec);
   return %newVec;
}

//----common transform utilities----
function posFromRaycast(%transform)
{
   // the 2nd, 3rd, and 4th words returned from a successful raycast call are the position of the point
   %position = getWord(%transform, 1) SPC getWord(%transform, 2) SPC getWord(%transform, 3);
   return %position;
}

function normalFromRaycast(%transform)
{
   // the	5th,	6th and 7th words returned from a successful	raycast call are the normal of the	surface
   %norm = getWord(%transform, 4) SPC getWord(%transform, 5) SPC getWord(%transform, 6);
   return %norm;
}

// these stay, since having to type %rot = %obj.rotFromTransform(%transform); after already having the transform is
// just way too much.
function posFromTransform(%transform)
{
   // the first three words of an object's transform are the object's position
   %position = getWord(%transform, 0) SPC getWord(%transform, 1) SPC getWord(%transform, 2);
   return %position;
}

function rotFromTransform(%transform)
{
   // the last four words of an object's transform are the object's rotation
   %rotation = getWord(%transform, 3) SPC getWord(%transform, 4) SPC getWord(%transform, 5) SPC getWord(%transform, 6);
   return %rotation;
}

//if we are going to make these inherited functions, might as welllet them get the transform as well
function SceneObject::posFromTransform(%obj, %transform)
{
   if(%transform $= "")
      %transform = %obj.getTransForm();
   // the first three words of an object's transform are the object's position
   %position = getWord(%transform, 0) SPC getWord(%transform, 1) SPC getWord(%transform, 2);
   return %position;
}

function SceneObject::rotFromTransform(%obj, %transform)
{
   if(%transform $= "")
      %transform = %obj.getTransForm();
   // the last four words of an object's transform are the object's rotation
   %rotation = getWord(%transform, 3) SPC getWord(%transform, 4) SPC getWord(%transform, 5) SPC getWord(%transform, 6);
   return %rotation;
}

//this finds the terrain height for the position and sets it x meters above the terrain
function bumpZ(%pos, %heightMod)
{
   %x = getword(%pos, 0);
   %y = getword(%pos, 1);
   %height = getTerrainHeight(%x SPC %y);

   return %x SPC %y SPC %height + %heightMod;
}

//this function merely zeroes the z of a pos, not used anymore
function gimpPosition(%pos)
{
   return getword(%pos, 0) SPC getword(%pos, 1) SPC "0";
}

function getAngleDifferance(%angleOne, %angleTwo)
{
   //we need the smallest angle from angle one to angle two

   //First ensure both angles are positive
   %angleOne = %angleOne < 0 ? $TwoPi + %angleOne : %angleOne;
   %angleTwo = %angleTwo < 0 ? $TwoPi + %angleTwo : %angleTwo;

   %diff = %angleTwo - %angleOne;

   %diff = %diff > $Pi ? $TwoPi - %diff : %diff;
   %diff = %diff < -$Pi ? -$TwoPi - %diff : %diff;

   return %diff;
}

//---Directional (compass) functions----

function getClosestCompassTick(%angle)
{
   %value = "";

   %angle = %angle < 0 ? $TwoPi + %angle : %angle;

   if(%angle >= mDegToRad(337.5) || %angle < mDegToRad(22.5))
      %value = "North" SPC $North;
   else if(%angle >= mDegToRad(22.5) && %angle < mDegToRad(67.5))
      %value = "NorthEast" SPC $NE;
   else if(%angle >= mDegToRad(67.5) && %angle < mDegToRad(112.5))
      %value = "East" SPC $East;
   else if(%angle >= mDegToRad(112.5) && %angle < mDegToRad(157.5))
      %value = "SouthEast" SPC $SE;
   else if(%angle >= mDegToRad(157.5) && %angle < mDegToRad(202.5))
      %value = "South" SPC $South;
   else if(%angle >= mDegToRad(202.5) && %angle < mDegToRad(247.5))
      %value = "SouthWest" SPC $SW;
   else if(%angle >= mDegToRad(247.5) && %angle < mDegToRad(292.5))
      %value = "West" SPC $West;
   else if(%angle >= mDegToRad(292.5) && %angle < mDegToRad(337.5))
      %value = "NorthWest" SPC $NW;

   return %value;
}

function getClosestCardinal(%angle)
{
   %value = "";

   %angle = %angle < 0 ? $TwoPi + %angle : %angle;
   %angle = %angle > $TwoPi ? %angle - $TwoPi : %angle;

   if(%angle >= mDegToRad(315) || %angle < mDegToRad(45))
      %value = "North" SPC $North;
   else if(%angle >= mDegToRad(45) && %angle < mDegToRad(135))
      %value = "East" SPC $East;
   else if(%angle >= mDegToRad(135) && %angle < mDegToRad(225))
      %value = "South" SPC $South;
   else if(%angle >= mDegToRad(225) && %angle < mDegToRad(315))
      %value = "West" SPC $West;


   //error("getClosestCardinal Value:" SPC %value);
   return %value;
}

//----angles and rotations----

function invertDegAngle(%angle)
{
   if(%angle > 0)
      %angle = -(180 - %angle);
   else
      %angle = mAbs(%angle + 180);

   return %angle;
}

function invertAngle(%angle)
{
   //add pi, then cap
   %angle += $PI;
   %angle = %angle > $TwoPi ? %angle - $TwoPi : %angle;

   return %angle;
}

function getAnglefromRot(%rot)
{
   //return a positive z angle in radians
   %angle = getWord(%rot, 2) * getWord(%rot, 3);
   %angle = %angle < 0 ? $TwoPi + %angle : %angle;
   return %angle;
}

function getDegAnglefromRot(%rot)
{
   //return the rot's z angle in positive degrees
   %angleOne = mRadtoDeg(getWord(%rot, 2) * getWord(%rot, 3));
   %angle = %angleOne < 0 ? 360 + %angleOne : %angleOne;
   return %angle;
}

function getZangle(%posOne, %posTwo)
{
   %vec = VectorSub(%posTwo, %posOne);
   //get the angle
   %rotAngleZ = mATan( firstWord(%vec), getWord(%vec, 1) );
   //add pi to the angle
   //%rotAngleZ += $Pi;

   //make this rotation a proper torque game value, anything more than 240 degrees is negative
   //if(%rotAngleZ > 4.18879)
      //subtract 2pi from the value, then make sure its positive
     // %rotAngleZ = -(mAbs(%rotAngleZ - $TwoPi));

   return %rotAngleZ;
}

function absRot(%rot)
{
   %rot = %rot < 0 ? %rot + $TwoPi : %rot;

   return %rot;
}

function adjustRot(%rot, %rotAdjust)
{
   %angle = getWord(%rot, 2) * getWord(%rot, 3);
   //%rotAdjust = ((%rotAdjust >= 4.18879) ? (($TwoPi - %rotAdjust) * -1) : %rotAdjust);

   %rotAdjust = %rotAdjust < 0 ? $TwoPi + %rotAdjust : %rotAdjust;

   //now add the angle adjustment to the refrence rot
   %adjust = %angle + %rotAdjust;
   //bring in back in line with the torque world
   %adjust = ((%adjust >= 4.18879) ? (%adjust - $TwoPi) : %adjust);
   //if it's negative we'll stuff it into the vector and leave the angle value unchanged
   %rotZ = ((%adjust >= 0) ? 1 : (-1));
   %adjust = mAbs(%adjust);
   %rot = getWord(%rot, 0) SPC getWord(%rot, 1)  SPC %rotZ SPC %adjust;
   return %rot;
}

// This lil function generates the rotation required for an object at PosOne to point at PosTwo (z rot only)
function pointToXYPos(%posOne, %posTwo)
{
   %vec = VectorSub(%posOne, %posTwo);
   //get the angle
   %rotAngleZ = mATan( firstWord(%vec), getWord(%vec, 1) );
   //add pi to the angle
   %rotAngleZ += $Pi;

   //make this rotation a proper torque game value, anything more than 240 degrees is negative
   if(%rotAngleZ > 4.18879)
   {
      //the rotation scale is seldom negative, instead make the axis value negative
      %modifier = -1;
      //subtract 2pi from the value, then make sure its positive
      %rotAngleZ = mAbs(%rotAngleZ - $TwoPi);
      //sigh, if only this were all true
   }
   else
      %modifier = 1;

   //assemble the rotation and send it back
   return "0 0" SPC %modifier SPC %rotAngleZ;
}

// And this lil function generates the rotation required for an object at posOne to
// point at PosTwo for X, Y And Z axis.
function pointToPos(%posOne, %posTwo)
{
   //sub the two positions so we get a vector pointing from the origin in the direction we want our object to face
   %vec = VectorSub(%posTwo, %posOne);

   // pull the values out of the vector
   %x = firstWord(%vec);
   %y = getWord(%vec, 1);
   %z = getWord(%vec, 2);

   //this finds the distance from origin to our point
   %len = vectorLen(%vec);

   //---------X-----------------
   //given the rise and length of our vector this will give us the angle in radians
   %rotAngleX = mATan( %z, %len );

   //---------Z-----------------
   //get the angle for the z axis
   %rotAngleZ = mATan( %x, %y );

   //create 2 matrices, one for the z rotation, the other for the x rotation
   %matrix = MatrixCreateFromEuler("0 0" SPC %rotAngleZ * -1);
   %matrix2 = MatrixCreateFromEuler(%rotAngleX SPC "0 0");

   //now multiply them together so we end up with the rotation we want
   %finalMat = MatrixMultiply(%matrix, %matrix2);

   //we're done, send the proper numbers back
   return getWords(%finalMat, 3, 6);
}

//new function by Founder (Martin Hoover)
//PosOne needs to be the location of the object, PosTwo is the position you want it to face.
//sub the two positions so we get a vector pointing from the origin in the direction we want our object to face
function setPointAtPos(%obj, %posTwo)
{      
   %px = getWord(%obj.getWorldBoxCenter(), 0);
   %py = getWord(%obj.getWorldBoxCenter(), 1);
   %pz = getWord(%obj.getWorldBoxCenter(), 2);
   %ps = %px SPC %py SPC %pz;

   %pz += 1;// up a little here - Lagg...

   %vec = VectorSub(%posTwo, %ps);

   // pull the values out of the vector
   %x = firstWord(%vec);
   %y = getWord(%vec, 1);
   %z = getWord(%vec, 2);

   //this finds the distance from origin to our point
   %len = vectorLen(%vec);

   //---------XY----------------- 
   //given the rise and length of our vector this will give us the angle in radians
   %rotAngleXY = mATan( %z, %len );

   //---------Z-----------------
   //get the angle for the z axis
   %rotAngleZ = mATan( %x, %y );

   //create 2 matrices, one for the z rotation, the other for the x rotation
   %matrix = MatrixCreateFromEuler("0 0" SPC %rotAngleZ * -1);
   %matrix2 = MatrixCreateFromEuler(%rotAngleXY SPC "0 0");

   //now multiply them together so we end up with the rotation we want
   %finalMat = MatrixMultiply(%matrix, %matrix2);

   //we&#180;re done, send the proper numbers back
   //return getWords(%finalMat, 3, 6);
   %rt = getWords(%finalMat, 3, 6);

   %obj.setTransform( %ps SPC %rt );//------------------------ set the Transform Here
}

function getXYAngle(%posOne, %posTwo)
{
   //sub the two positions so we get a vector pointing from the origin in the direction we want our object to face
   %vec = VectorSub(%posTwo, %posOne);

   // pull the values out of the vector
   %x = firstWord(%vec);
   %y = getWord(%vec, 1);
   %z = getWord(%vec, 2);

   //this finds the distance from origin to our point
   %len = vectorLen(%vec);

   //---------XY-----------------
   //given the rise and length of our vector this will give us the angle in radians
   %rotAngleXY = mATan( %z, %len );
   //%rotAngleXY+=$Pi;
   return %rotAngleXY;
}

function getXYangleFromRot(%rot)
{
   %a = getWord(%rot, 3);

   %x = firstWord(%rot) * %a;
   %y = getWord(%rot, 1) * %a;
   %z = getWord(%rot, 2) * %a;


   %xy = mSqrt((%x * %x) + (%y * %y));
   %rotAngleXY = mATan( %xy, mAbs(%z) );
   return %rotAngleXY;
}

function getZrise(%rot, %posOne, %posTwo)
{
   %XYAngle = getXYangleFromRot(%rot);

   %vec = VectorSub(%posTwo, %posOne);

    //this finds the distance from origin to our point
   %len = vectorLen(%vec);

   //pythagorian theroem says right triangle sides: a squared + b squared = c squared
   //we need to find the rise amount based on the angle and length
   // c^2 - a^2 = b^2
   //the height of the right triangle equals the tangent of the angle times the length of the base
   //%zmod = mTan(%XYAngle + $Pi) * (%len + $Pi);
   %zmod = mTan(%XYAngle) * (%len - $Pi);
   return %zmod;
}

//--------------------------------------------------------------------------------------------
// Spline functions
//--------------------------------------------------------------------------------------------

// These functions draw a curve from the first point to the last point, going near any middle points at time t.
// t represents the percentage of distance between the first and last point that you want the curve point for
// They can be used to draw a 3-d spline when all three axis are run through the function.

// %t = (position Of Desired Spline Point) / (distance Between First And Last Points)
// %curvePoint1.x = get3PointCurveValue(%point1.x, %point2.x, %point3.x, %t)
// %curvePoint1.y = get3PointCurveValue(%point1.y, %point2.y, %point3.y, %t)
// %curvePoint1.z = get3PointCurveValue(%point1.z, %point2.z, %point3.z, %t)

// this lil guy doesn't actually draw a curve, but rather a line, however it is very useful for mixing
// two values together based on a percentage of time or distance
function get2PointLineValue(%point1, %point2, %t)
{
   %pd = ( %point1 * (1-%t)) + (%point2 * %t);
   return %pd;
}

function get3PointCurveValue(%point1, %point2, %point3, %t)
{
   %step1 = %point1 * ((1-%t) * (1-%t));
   %step2 = %point2 * 2 * %t * (1 - %t);
   %step3 = %point3 * (%t * %t);
   %pd = %step1 + %step2 + %step3;
   return %pd;
}

function get4PointCurveValue(%point1, %point2, %point3, %point4, %t)
{
   %step1 = %point1 * ( (1 - %t)*(1 - %t)*(1 - %t) );
   %step2 = %point2 * 3 * %t * ( (1 - %t)*(1 - %t) );
   %step3 = %point3 * 3 * (%t * %t) * (1 - %t);
   %step4 = %point4 * (%t * %t * %t);
   %pd = %step1 + %step2 + %step3 + %step4;
   return %pd;
}

//---------------------------------------------------------------------------------------------------------------
// functions dealing with terrain
//---------------------------------------------------------------------------------------------------------------

//this takes the provided position, and sets it to the position of the earest terrain vertex
function findNearestTerrainPoint(%pos)
{
   %x = findGridPoint(firstWord(%pos));
   %y = findGridPoint(getWord(%pos, 1));
   %z = getWord(%pos, 2);

   return(%x SPC %y SPC %z);
}

function findGridPoint(%axis)
{
   if(%axis == 0)
      return 0;
   %axis = mFloor(%axis);
   %remainder = %axis % Terrain.squareSize;

   if(%remainder >= (Terrain.squareSize / 2))
   {
      while(%axis % Terrain.squareSize != 0)
         %axis++;
   }
   else
   {
      while(%axis % Terrain.squareSize != 0)
         %axis--;
   }
   return %axis;
}

//Takes the pos provided, and sets the z value to the surface height
function getTerrainSurfacePoint(%pos)
{
   %x = getword(%pos, 0);
   %y = getword(%pos, 1);
   %height = getTerrainHeight(%x SPC %y);
   return %x SPC %y SPC %height;
}

//in a box with the given max and min x and y extents, this will find the average height value
function getAverageTerrainHeight(%minX, %maxX, %minY, %maxY, %squareSize)
{
   %total = 0;
   %count = 0;
   for(%x = %minX; %x <= %maxX; %x += %squareSize)
   {
      //we are already looping through x, so loop through Y while we're at it
      for(%y = %minY; %y <= %maxY; %y += %squareSize)
      {
         %total += getTerrainHeight(%x SPC %y);
         %count++;
      }
   }
   return %total / %count;
}

function getTerrainAngle(%point)
{
   %angleRad = mACos(vectorDot(%point, "0 0 1"));
   %angleDeg = mRadtoDeg(%angleRad);
   return %angleDeg;
}

function getTerrainLevel(%pos, %rad)
{
   while(%retries < 500)
   {
      %x = getWord(%pos, 0) + mFloor(getRandom(%rad * 2) - %rad);
      %y = getWord(%pos, 1) + mFloor(getRandom(%rad * 2) - %rad);
      %z = getWord(%pos, 2) + mFloor(getRandom(%rad * 2) - %rad);

      %start = %x @ " " @ %y @ " 5000";
      %end = %x @ " " @ %y @ " -1";
      %ground = containerRayCast(%start, %end, $TypeMasks::TerrainObjectType, 0);
      %z = getWord(%ground, 3);

      %z += 3.5;
      %position = %x @ " " @ %y @ " " @ %z;

      %mask = ( $TypeMasks::ShapeBaseObjectType | $TypeMasks::MoveableObjectType |
                $TypeMasks::StaticShapeObjectType | $TypeMasks::WaterObjectType | 
                $TypeMasks::InteriorObjectType );

      InitContainerRadiusSearch( %position, 3.5, %mask );
      if(containerSearchNext() == 0)
      {
         //error ("Position Is Good");
         return %position;
      }
      else
         %retries++;
   }
   return( %pos );
}

function SceneObject::setOnTerrain(%obj, %pos, %calcOnly)
{
   if(%pos $= "")
      %pos = %obj.position;
   %terHeight = getTerrainHeight(%pos);
   %offset = %obj.GetHeightOffset();
   %newPos = firstWord(%pos) SPC getWord(%pos, 1) SPC %terHeight + %offset;
   if(%calcOnly)
      return %newPos;
   else
      %obj.setTransform(%newPos SPC %obj.rotFromTransform());
}

//--------------------------------------------------------------------------------------------
//
//	SceneObject::alignTransforms() -by Founder
//
//--------------------------------------------------------------------------------------------
// Here it is!! THE mother of all functions!! AMAZING!!
//
// usage: %objectID.alignTransforms(%pos, %rot, %rotAdjust, %xAngle, %yRadius, %zMod, %invert, %calc);
//
//	%pos is the position of the object(or point) around which to move the (%objectID)
//	%rot is the final rotation you want %objectID to face
//	%rotAdjust is for making a rotational offset of %objectID relative to the provided %rot
//		valid values go from 0 to 360; going clockwise from north(0), east(90)
//		south(180), west(270) etc. 360 will work, but I don't know what kind of
//		dumbass would rotate an object by 360 degrees in the z.
//	%xAngle and %yRadius are vars for adjusting %objectID's position relative to %pos
//		%yRadius describes a circle of (%yRadius) in size, along which you wish %objectID to be
//		placed. %xAngle is the relative angle in degrees (0 - 360) from %pos. Where this
//		angle intersects the circle is the position the object will be placed.
//
//	%zMod is how far up you want to move the object relative to %pos
//	%invert is weather or not you want to turn %objectID upside down.
//		true and 1 will invert it, 0, false, or giveing it no value will not.
//	%calc will have the function return the calculated transform instead of actually moving
//		the object.
// if for any option you do not want to make an adjustment fill the spot with a 0(zero)
// negative values will subtract from x y and z axis, but negative values are not valid for
// the %rotAdjust value
// examples:
//              %deplObj.alignTransforms(%pos, %rot, 270, 0, 0, 8, true);
//              %deplObj.alignTransforms(%dPos, %rot, 0, 0, 0.7, -0.05);
//
//
//
//                               --
//                          -          X
//                         -            -
//                               P
//                         -            -
//                          -          -
//                               --
//
//
//  In the diagram above, P equals the position of the player or the position generated by the
//  deploy code or the postion of the reference object. The circle made up of (-)'s represents
//  a %yRadius of 5 meters. Or if you prefer, a circle which represents the distance
//  away from P that you want this object to be. X represents the object for whom we are
//  generating the new position, and where we want it. X is at an approximately 45 degree angle
//  from P, which is the northEast tick on the Torque compass. In order to keep X's position relative
//  to P, no matter what direction P faces you would use the following line of code:
//         %ObjectID.alignTransforms(%pos(of P), %rot(of P), 0, 45, 5, 0);
//--------------------------------------------------------------------------------------------

function SceneObject::alignTransforms(%obj, %pos, %rot, %rotAdjust, %xAngle, %yRadius, %zMod, %invert, %calc)
{
   //rotation hack - these rots just aren't natural!
   if(%rot $= "0 0 1 0" || %rot $= "0 1 0 0"  || %rot $= "1 0 0 0")
      %rot = "0.0001 0.00001 0.99 0.001";

   //get the angle of the refrence object(rot) so we know what direction to go to find the new position
   %angle = getWord(%rot, 2) * getWord(%rot, 3);

   //this is the angle to find the position at, relative to the refrence point
   %xAngle = mDegToRad(%xAngle);
   //now adjust it to be compatible with the refrence rotation
   %xAngleMod = ((%angle < 0) ? (-1 * %xAngle) : %xAngle);

   //generate a vector to the adjusted position
   %matrix = VectorOrthoBasis(getXYangleFromRot(%rot) SPC getWords(%rot,1,2) SPC getWord(%rot, 3) + %xAngleMod);
   //%matrix = VectorOrthoBasis(getWords(%rot,0,2) SPC getWord(%rot, 3) + %xAngleMod);
   %yRot = getWords(%matrix, 3, 5);

   //now scale from our current position the yRadius amount to get our new position
   %zPos = vectorAdd(%pos, vectorScale(%yRot, %yRadius));
   %bPos = firstWord(%zPos) SPC getWord(%zPos, 1) SPC (getWord(%zPos, 2) + %zMod);

   //right now our new position is on the horizontal plane of world space, if the refrence rot is tilted
   //we need to move the pos up or down to account for it.
   //do wee need to adjust the Z hieght position to account for objects rotated on an x or y axis?
   %XYAngle = getXYangleFromRot(%rot);
   //only add to the Z if the angle is more then 1 deg (Otherwise the rotation hack above will try to bump the target up)
   if(mAbs(%XYAngle) > 0.0174533)
   {
      %zRise = getZrise(%rot, %bpos, %pos);// + $Pi;
      %bPos = getWords(%bPos, 0, 1) SPC getword(%bPos, 2) + %zRise;
   }

   //lets try using the trusty ole matrix to take care of any tilt
   //so first thing is to subtract the target position from the ref position.


   //it would probably more accurate to calculate this on the surface of a shpere instead of a circle then adjusting
   //the hieght, but since I don't know how to do that just now we'll go this route, which is plenty damned acurate

   //if the generated position needs to point in a certain direction relative to the refrence, then generate it's rot
   if(%rotAdjust !$= "" && %rotAdjust != 0)
   {
      //convert the adjustment to radians, then make it a valid torque world angle
      %rotAdjust = mDegToRad(%rotAdjust);
      %rotAdjust = ((%rotAdjust >= 4.18879) ? (($TwoPi - %rotAdjust) * -1) : %rotAdjust);

      //now add the angle adjustment to the refrence rot
      %adjust = %angle + %rotAdjust;
      //bring in back in line with the torque world
      %adjust = ((%adjust >= 4.18879) ? (%adjust - $TwoPi) : %adjust);
      //if it's negative we'll stuff it into the vector and leave the angle value unchanged
      %rotZ = ((%adjust >= 0) ? 1 : (-1));
      %adjust = mAbs(%adjust);
      %rot = getWord(%rot, 0) SPC getWord(%rot, 1)  SPC %rotZ SPC %adjust;
      %angle = getWord(%rot, 2) * getWord(%rot, 3);
   }
   if(%invert)
   {
      //this is for turning the generated position upside down, wonder if this and the above are broken
      //for tilted refrence rotations?
      %angleR = -1 * %angle;
      %Mrot = MatrixCreateFromEuler("0 3.14" SPC %angleR);
      %rot = getWord(%Mrot, 3) SPC getWord(%Mrot, 4) SPC getWord(%Mrot, 5) SPC getWord(%Mrot, 6);
   }

   //we're done, get the hell outta here!
   %transForm = %bPos SPC %rot;
   if(%calc)
      return %transForm;
   else
   {
      %obj.setTransform(%transForm);
      return;
   }
}

function SceneObject::alignGroupRot(%obj, %pos, %rot, %rotAdjust, %xAngle, %yRadius, %zMod, %invert, %calc)
{
   %vAngle = getWord(%rot, 3);
   %angle = getWord(%rot, 2) * getWord(%rot, 3);
   %xAngle = mDegToRad(%xAngle);
   %xAngleMod = ((%angle < 0) ? (-1 * %xAngle) : %xAngle);
   %vRot =  getWords(%rot,0,2);
   %matrix = VectorOrthoBasis(%vRot SPC %vAngle + %xAngleMod);
   %yRot = getWords(%matrix, 3, 5);
   %zPos = vectorAdd(%pos, vectorScale(%yRot, %yRadius));
   %bPos = firstWord(%zPos) SPC getWord(%zPos, 1) SPC (getWord(%zPos, 2) + %zMod);
   if(%rotAdjust !$= "" && %rotAdjust != 0)
   {
      %deg = mRadtoDeg(%angle);
      %rotAdjust = ((%rotAdjust >= 240) ? ((360 - %rotAdjust) * -1) : %rotAdjust);
      %adjust = %deg + %rotAdjust;
      %adjust = ((%adjust >= 240) ? (%adjust - 360) : %adjust);
      %rotZ = ((%adjust >= 0) ? 1 : (-1));
      %adjust = mAbs(%adjust);
      %finAngle = mDegToRad(%adjust);
      %rot = getWord(%rot, 0) SPC getWord(%rot, 1)  SPC %rotZ SPC %finAngle;
      %angle = getWord(%rot, 2) * getWord(%rot, 3);
   }
   if(%invert)
   {
      %angleR = -1 * %angle;
      %Mrot = MatrixCreateFromEuler("0 3.14" SPC %angleR);
      %rot = getWord(%Mrot, 3) SPC getWord(%Mrot, 4) SPC getWord(%Mrot, 5) SPC getWord(%Mrot, 6);
   }
   %transForm = %bPos SPC %rot;
   if(%calc)
      return %transForm;
   else
   {
      %obj.setTransform(%transForm);
      return;
   }
}

//---------------------------------------------------------------------------------------------------------------
// Functions for moving objects
//---------------------------------------------------------------------------------------------------------------

function SceneObject::StartMoveObject(%obj, %endpos, %time, %smoothness, %delay)
{
   if ( isObject( %obj ) )
   {
      %startpos = %obj.getTransform();
      %diff = VectorSub( %endpos, %startpos );
      %numsteps = (%time/1000) * %smoothness;
      %interval = 1000 / %smoothness;
      %stepvec = VectorScale(%diff, (1/%numsteps));
      %numstepsleft = %numsteps;
      %currpos = %startpos;

      if ( %delay > 0 )
         %obj.schedule( %delay, "MoveObject", %startpos, %endpos, %numsteps, %numstepsleft, %stepvec, %currpos, %interval );
      else
         %obj.MoveObject( %startpos, %endpos, %numsteps, %numstepsleft, %stepvec, %currpos, %interval );
   }
}

function SceneObject::MoveObject(%obj, %startpos, %endpos, %numsteps, %numstepsleft, %stepvec, %currpos, %interval)
{
   %rot = rotFromTransform(%obj.getTransform());
   %currpos = VectorAdd(%currpos, %stepvec);

   %obj.setTransForm( %currpos SPC %rot );
   %numstepsleft--;
   if( %numstepsleft < 1 )
      return;
   else
      %obj.schedule(%interval, "MoveObject", %startpos, %endpos, %numsteps, %numstepsleft, %stepvec, %currpos, %interval, %delay);
}

function getObjectTypeMask(%obj)
{
   if(%obj.getType() & $TypeMasks::DefaultObjectType)
    error("\c4Type is: $TypeMasks::DefaultObjectType");
   else if(%obj.getType() & $TypeMasks::StaticObjectType)
    error("\c4Type is: $TypeMasks::StaticObjectType");
   else if(%obj.getType() & $TypeMasks::EnvironmentObjectType)
    error("\c4Type is: $TypeMasks::EnvironmentObjectType");
   else if(%obj.getType() & $TypeMasks::TerrainObjectType)
    error("\c4Type is: $TypeMasks::TerrainObjectType");
   else if(%obj.getType() & $TypeMasks::WaterObjectType)
    error("\c4Type is: $TypeMasks::WaterObjectType");
   else if(%obj.getType() & $TypeMasks::TriggerObjectType)
    error("\c4Type is: $TypeMasks::TriggerObjectType");
   else if(%obj.getType() & $TypeMasks::MarkerObjectType)
    error("\c4Type is: $TypeMasks::MarkerObjectType");
   else if(%obj.getType() & $TypeMasks::GameBaseObjectType)
    error("\c4Type is: $TypeMasks::GameBaseObjectType");
   else if(%obj.getType() & $TypeMasks::ShapeBaseObjectType)
    error("\c4Type is: $TypeMasks::ShapeBaseObjectType");
   else if(%obj.getType() & $TypeMasks::CameraObjectType)
    error("\c4Type is: $TypeMasks::CameraObjectType");
   else if(%obj.getType() & $TypeMasks::StaticShapeObjectType)
    error("\c4Type is: $TypeMasks::StaticShapeObjectType");
   else if(%obj.getType() & $TypeMasks::DynamicShapeObjectType)
    error("\c4Type is: $TypeMasks::DynamicShapeObjectType");
   else if(%obj.getType() & $TypeMasks::PlayerObjectType)
    error("\c4Type is: $TypeMasks::PlayerObjectType");
   else if(%obj.getType() & $TypeMasks::ItemObjectType)
    error("\c4Type is: $TypeMasks::ItemObjectType");
   else if(%obj.getType() & $TypeMasks::VehicleObjectType)
    error("\c4Type is: $TypeMasks::VehicleObjectType");
   else if(%obj.getType() & $TypeMasks::VehicleBlockerObjectType)
    error("\c4Type is: $TypeMasks::VehicleBlockerObjectType");
   else if(%obj.getType() & $TypeMasks::ProjectileObjectType)
    error("\c4Type is: $TypeMasks::ProjectileObjectType");
   else if(%obj.getType() & $TypeMasks::ExplosionObjectType)
    error("\c4Type is: $TypeMasks::ExplosionObjectType");
   else if(%obj.getType() & $TypeMasks::CorpseObjectType)
    error("\c4Type is: $TypeMasks::CorpseObjectType");
   else if(%obj.getType() & $TypeMasks::DebrisObjectType)
    error("\c4Type is: $TypeMasks::DebrisObjectType");
   else if(%obj.getType() & $TypeMasks::PhysicalZoneObjectType)
    error("\c4Type is: $TypeMasks::PhysicalZoneObjectType");
   else if(%obj.getType() & $TypeMasks::LightObjectType)
    error("\c4Type is: $TypeMasks::LightObjectType");
   else if(%obj.getType() & $TypeMasks::ZoneObjectType)
    error("\c4Type is: $TypeMasks::ZoneObjectType");
   else if(%obj.getType() & $TypeMasks::GameBaseHiFiObjectType)
    error("\c4Type is: $TypeMasks::GameBaseHiFiObjectType");
   else
      error("\c4Object type is: UNKNOWN");
}

// Founder: Apply kickbak to anything
function SceneObject::applyKickback(%obj, %kick)
{
   %dir = %obj.getVectorFromTransform( %obj.getTransform(), "reverse" );
   %force = vectorScale( %dir, %kick );
   %obj.applyImpulse( %obj.posFromTransform(), %force );
}

// ZOD: Modified version of above to allow different directions
function SceneObject::applyKick(%obj, %amt, %velCap, %dir)
{
   %vec = %obj.getVectorFromTransform( %obj.getTransform(), %dir );
   %data = %obj.getDataBlock();
   %pos = %obj.posFromTransform();
   %velocity = %obj.getVelocity();
   %normal = VectorDot( %velocity, VectorNormalize( %velocity ) );

   if( %velCap != 0 && %normal > %velCap) // Whatever cap we wish..
   {
      %kick = %amt / %Normal; // Reduce impulse.
      //retrun; // Or apply no impulse at all and exit out.
   }
   else
      %kick = %amt;

   %impulse = VectorScale( %vec, %data.mass * %kick );
   %obj.applyImpulse(%pos, %impulse);
}

// Player to player or player to item
function SceneObject::repulse(%obj, %col)
{
   if ( !isObject( %obj ) || !isObject( %col ) )
      return( 0 );

   %velocity = %obj.getVelocity();
   %normal = vectorDot( %velocity, VectorNormalize( %velocity ) );
   if( %normal < 15 )
      %multi = 8;
   else
      %multi = 8 / %normal;

   %objPos = %obj.posFromTransform();
   %colPos = %col.posFromTransform();
   %vec = VectorNormalize( VectorSub( %objPos, %colPos ) );
   %force = VectorScale( %vec, %obj.getDatablock().mass * %multi );
   %obj.applyImpulse( %objPos, %force );

   return( %force );
}

//--------------------------------------------------------------------------------------------
// Taken from Harold Paul /*Wedge*/ DElia's post in the GG mod forums
// http://www.garagegames.com/mg/forums/result.thread.php?qt=50109
function SceneObject::getVectorFromTransform(%obj, %transform, %vector)
{
   %aa = getWords(%transform, 3, 6);   
   %tmat = VectorOrthoBasis(%aa);

   %rv = getWords(%tMat, 0, 2);
   %fv = getWords(%tMat, 3, 5);
   %uv = getWords(%tMat, 6, 8);

   if(%vector $= "up")
      return %uv;

   if(%vector $= "down")
      return VectorScale(%uv, -1);

   if(%vector $= "left")
      return VectorScale(%rv, -1);

   if(%vector $= "right")
      return %rv;

   if(%vector $= "forward")
      return %fv;

   if(%vector $= "reverse")
     return VectorScale(%fv, -1);
}

//--------------------------------------------------------------------------------------------

function stripTaggedVar(%var)
{
   return stripChars( detag( getTaggedString( %var ) ), "\cp\co\c6\c7\c8\c9" );
}

//------------------------------------------------------------------------------------
// Object Box functions
//------------------------------------------------------------------------------------
//box = min.x min.y min.z max.x max.y max.z

//parse out a particular element
function getBoxMinX(%box){ return getWord(%box, 0); }
function getBoxMinY(%box){ return getWord(%box, 1); }
function getBoxMinZ(%box){ return getWord(%box, 2); }
function getBoxMaxX(%box){ return getWord(%box, 3); }
function getBoxMaxY(%box){ return getWord(%box, 4); }
function getBoxMaxZ(%box){ return getWord(%box, 5); }

function SceneObject::getWorldBoxMin(%obj)
{
   %box = %obj.getObjectBox();
   %matrix = %obj.getTransForm();
   return MatrixMulPoint(%matrix, getWords(%box, 0, 2));
}

function SceneObject::getWorldBoxMax(%obj)
{
   %box = %obj.getObjectBox();
   %matrix = %obj.getTransForm();
   return MatrixMulPoint(%matrix, getWords(%box, 3, 5));
}

function SceneObject::getWorldBoxMinX(%obj) { return getWord(%obj.getWorldBoxMin(), 0); }
function SceneObject::getWorldBoxMinY(%obj) { return getWord(%obj.getWorldBoxMin(), 1); }
function SceneObject::getWorldBoxMinZ(%obj) { return getWord(%obj.getWorldBoxMin(), 2); }
function SceneObject::getWorldBoxMaxX(%obj) { return getWord(%obj.getWorldBoxMax(), 0); }
function SceneObject::getWorldBoxMaxY(%obj) { return getWord(%obj.getWorldBoxMax(), 1); }
function SceneObject::getWorldBoxMaxZ(%obj) { return getWord(%obj.getWorldBoxMax(), 2); }


//return a box dimension
function SceneObject::GetBoxWidth(%obj)
{
   %box = %obj.getObjectBox();
   return mAbs(getBoxMaxX(%box)) + mAbs(getBoxMinX(%box));
}

function SceneObject::GetBoxLength(%obj)
{
   %box = %obj.getObjectBox();
   return mAbs(getBoxMaxY(%box)) + mAbs(getBoxMinY(%box));
}

function SceneObject::GetBoxHeight(%obj)
{
   %box = %obj.getObjectBox();
   return mAbs(getBoxMaxZ(%box)) + mAbs(getBoxMinZ(%box));
}

function SceneObject::getBoxCenter(%obj)
{
   //box center by default will be 0 0 0, so use a lil matrix math, get it's current world position
   %matrix = %obj.getTransForm();
   %box = %obj.getObjectBox();
   %newBox = MatrixMulPoint(%matrix, getWords(%box, 0, 2));
   %newBox = %newBox SPC MatrixMulPoint(%matrix, getWords(%box, 3, 5));
   %centerX = (getBoxMinX(%newBox) + getBoxMaxX(%newBox)) * 0.5;
   %centerY = (getBoxMinY(%newBox) + getBoxMaxY(%newBox)) * 0.5;
   %centerZ = (getBoxMinZ(%newBox) + getBoxMaxZ(%newBox)) * 0.5;
   return %centerX SPC %centerY SPC %centerZ;
}

function SceneObject::GetHeightOffset(%obj)
{
   return mAbs(getWord(%obj.position, 2) - %obj.getWorldBoxMinZ());
}

//------------------------------------------------------------------------------------
// Other functionality
//------------------------------------------------------------------------------------

function getValidSpawnSurface(%pos, %radius)
{
   //echo( "getValidSpawnSurface(" SPC %pos @", "@ %radius SPC ")");
   while(%retries < 500)
   {
      %x = getWord(%pos, 0) + mFloor(getRandom(%radius * 2) - %radius);
      %y = getWord(%pos, 1) + mFloor(getRandom(%radius * 2) - %radius);

      %start = %x SPC %y SPC VectorAdd(getWord(%pos, 2), 8);
      %end = %x SPC %y SPC "-1";
      %surface = containerRayCast(%start, %end, $TypeMasks::TerrainObjectType | $TypeMasks::StaticObjectType, 0);
      if ( !%surface )
      {
         //error( "No valid spawn surface could be found" );
         return( %pos );
      }

      %z = getWord(%surface, 3);
      %position = %x SPC %y SPC %z;

      %mask = ( $TypeMasks::VehicleObjectType | $TypeMasks::PlayerObjectType );

      InitContainerRadiusSearch( %position, 3.5, %mask );
      if ( containerSearchNext() == 0 )
      {
         //error( "Random spawn position is good:" SPC %position );
         return %position;
      }
      else
         %retries++;
   }

   error( "Random spawn position cannot be found" );
   return( %pos );
}

//--------------------------------------------------------------------------------
//a lil function to mark a position with an object and a default object to use as marker

datablock StaticShapeData(markerObject)
{
   category = "Misc";
   shapeFile = "art/editor/octahedron.dts";
   dynamicType = $TypeMasks::StaticShapeObjectType;
};

function markPoint(%pos, %rot, %block, %type, %name)
{
   if(%block $= "" && %type $= "")
   {
      %type = StaticShape;
      %block = markerObject;
   }

   %obj = new (%type)(%name) {
      dataBlock = %block;
   };
   MissionCleanup.add(%obj);

   %obj.setTransform(%pos SPC %rot);
}

function markPointArray(%array)
{
   %count = %array.count();
   for(%i = 0; %i < %count; %i++)
   {
      markPoint(%array.getValue(%i));
   }
}
//--------------------------------------------------------------------------------------------
// String functions
//--------------------------------------------------------------------------------------------

// Returns true if a string passed to it consists of nothing but digits and/or decimals.
// Passes false for strings with more than one decimal, or with a +
// or - as anything but the first character (+ or - are only allowed as the first character in the string)
function isCleanNumber(%string)
{
   %dot = 0;
   for(%i = 0; (%char = getSubStr(%string, %i, 1)) !$= ""; %i++)
   {
      switch$(%char)
      {
         case "0" or "1" or "2" or "3" or "4" or "5" or "6" or "7" or "8" or "9":
            continue;

         case ".":
            if(%dot > 1)
               return false;

            %dot++;
            continue;

         case "-":
            if(%i) // only valid as first character
               return false;

            continue;

         case "+":
            if(%i) // only valid as first character
               return false;

            continue;

         default:
            return false;
        }
    }
    // %text passed the test
    return true;
}

//this function takes a pathed filename (common/missions/blah.mis) and takes out everything excpet the name (blah)
function cropToName(%filePAth)
{
   %name = "";
  %length = strlen(%filePAth);
   %dot = 0;

   for(%i = 0; %i < %length; %i++)
   {
      if(getSubStr(%filePAth, %i, 1) $= "/")
         %lastDir = %i;
      if(getSubStr(%filePAth, %i, 1) $= ".")
         %dot = %i;
   }

   if(%dot > 0)
   {
      if(%lastDir $= "")
         %name = getSubStr(%filePAth, 0, %dot - 1);
      else
         %name = getSubStr(%filePAth, %lastDir + 1, %dot - 1);
   }

   else
      %name = getSubStr(%filePAth, %lastDir + 1, %length);

  return %name;

}

function cropDecimal(%num)
{
  %length = strlen(%num);
  %dot = 0;

   for(%i = 0; %i < %length; %i++)
   {
      if(getSubStr(%num, %i, 1) $= ".")
         %dot = %i;
   }

   if(%dot > 0)
     %name = getSubStr(%num, 0, %dot);
   else
      %name = %num;

  return %name;
}

function messageTest()
{
   messageAll('MsgTestColors', '\c0Color 0 \c1Color 1 \c2Color 2 \c3 Color 3 \c4 Color 4 \c5 Color 5 \c6 Color 6 \c7Color 7 \c8 Color 8 \c9 Color 9');
   echo("\c0test \c1test \c2test \c3test \c4test \c5test \c6test \c7test \c8test \c9test");
}

//--------------------------------------------------------------------------------------------
// Taken from Harold "LabRat" Brown's .plan
// http://www.garagegames.com/index.php?sec=mg&mod=resource&page=view&qid=10202
function dec2hex(%val)
{
   // Converts a decimal number into a 2 digit HEX number
   %digits ="0123456789ABCDEF"; //HEX digit table
	
   // To get the first number we divide by 16 and then round down, using 
   // that number as a lookup into our HEX table.
   %firstDigit = getSubStr(%digits,mFloor(%val/16),1);
	
   // To get the second number we do a MOD 16 and using that number as a
   // lookup into our HEX table.
   %secDigit = getSubStr(%digits,%val % 16,1);
	
   // return our two digit HEX number
   return %firstDigit @ %secDigit;
}

function chrValue(%chr)
{
   // So we don't have to do any C++ changes we approximate the function
   // to return ASCII Values for a character.  This ignores the first 31
   // characters and the last 128.
	
   // Setup our Character Table.  Starting with ASCII character 32 (SPACE)
   %charTable = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_'abcdefghijklmnopqrstuvwxyz{|}~\t\n\r";
	
   //Find the position in the string for the Character we are looking for the value of
   %value = strpos(%charTable,%chr);

   // Add 32 to the value to get the true ASCII value
   %value = %value + 32;
	
   //HACK:  Encode TAB, New Line and Carriage Return
   if (%value >= 127)
   {
      if(%value == 127)
         %value = 9;
      if(%value == 128)
         %value = 10;
      if(%value == 129)
         %value = 13;
   }
   //return the value of the character
   return %value;
}

function URLEncode(%rawString)
{
   // Encode strings to be HTTP safe for URL use

   // Table of characters that are valid in an HTTP URL
   %validChars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz:/.?=_-$(){}~&";

   // If the string we are encoding has text... start encoding
   if (strlen(%rawString) > 0)
   {
      // Loop through each character in the string
      for(%i=0;%i<strlen(%rawString);%i++)
      {
         // Grab the character at our current index location
         %chrTemp = getSubStr(%rawString,%i,1);

         //  If the character is not valid for an HTTP URL... Encode it
         if (strstr(%validChars,%chrTemp) == -1)
         {
            //Get the HEX value for the character
            %chrTemp = dec2hex(chrValue(%chrTemp));

            // Is it a space?  Change it to a "+" symbol
            if (%chrTemp $= "20")
            {
               %chrTemp = "+";
            }
            else
            {
               // It's not a space, prepend the HEX value with a %
               %chrTemp = "%" @ %chrTemp;
            }
         }
         // Build our encoded string
         %encodeString = %encodeString @ %chrTemp;
      }
   }
   // Return the encoded string value
   return %encodeString;
}

//--------------------------------------------------------------------------------------------
// Zod: A default raycast function
function ShapeBase::doRaycast(%this, %range, %mask)
{
   // get the eye vector and eye transform of the player
   %eyeVec   = %this.getEyeVector();
   %eyeTrans = %this.getEyeTransform();

   // extract the position of the player's camera from the eye transform (first 3 words)
   %eyePos = posFromTransform(%eyeTrans);

   // normalize the eye vector
   %nEyeVec = VectorNormalize(%eyeVec);

   // scale (lengthen) the normalized eye vector according to the search range
   %scEyeVec = VectorScale(%nEyeVec, %range);

   // add the scaled & normalized eye vector to the position of the camera
   %eyeEnd = VectorAdd(%eyePos, %scEyeVec);

   // see if anything gets hit
   %searchResult = containerRayCast(%eyePos, %eyeEnd, %mask, %this);

   return %searchResult;
}

function loadDatablockFiles( %datablockFiles, %recurse )
{
   if ( %recurse )
   {
      recursiveLoadDatablockFiles( %datablockFiles, 9999 );
      return;
   }
   
   %count = %datablockFiles.count();
   for ( %i=0; %i < %count; %i++ )
   {
      %file = %datablockFiles.getKey( %i );
      if ( !isScriptFile( %file ) )
         continue;
                  
      exec( %file );
   }
      
   // Destroy the incoming list.
   %datablockFiles.delete();
}

function recursiveLoadDatablockFiles( %datablockFiles, %previousErrors )
{
   %reloadDatablockFiles = new ArrayObject();

   // Keep track of the number of datablocks that 
   // failed during this pass.
   %failedDatablocks = 0;
   
   // Try re-executing the list of datablock files.
   %count = %datablockFiles.count();
   for ( %i=0; %i < %count; %i++ )
   {      
      %file = %datablockFiles.getKey( %i );
      if ( !isScriptFile( %file ) )
         continue;
         
      // Start counting copy constructor creation errors.
      $Con::objectCopyFailures = 0;
                                       
      exec( %file );
                                    
      // If errors occured then store this file for re-exec later.
      if ( $Con::objectCopyFailures > 0 )
      {
         %reloadDatablockFiles.add( %file );
         %failedDatablocks = %failedDatablocks + $Con::objectCopyFailures;
      }
   }
            
   // Clear the object copy failure counter so that
   // we get console error messages again.
   $Con::objectCopyFailures = -1;
                  
   // Delete the old incoming list... we're done with it.
   %datablockFiles.delete();
               
   // If we still have datablocks to retry.
   %newCount = %reloadDatablockFiles.count();
   if ( %newCount > 0 )
   {
      // If the datablock failures have not been reduced
      // from the last pass then we must have a real syntax
      // error and not just a bad dependancy.         
      if ( %lastFailures > %failedDatablocks )
         recursiveLoadDatablockFiles( %reloadDatablockFiles, %failedDatablocks );
                  
      else
      {      
         // Since we must have real syntax errors do one 
         // last normal exec to output error messages.
         loadDatablockFiles( %reloadDatablockFiles, false );
      }
      
      return;
   }
                  
   // Cleanup the empty reload list.
   %reloadDatablockFiles.delete();         
}

function ListDatablocks()
{
   %count = DataBlockGroup.getCount();
   for ( %i - 0; %i < %count; %i++ )
   {
      %datablock = DataBlockGroup.getObject(%i);
      echo("Datablock: " @ %datablock.getClassName());
	  
	  %fileName = "datablocks/" @ %datablock.getClassName() @ "/" @ %datablock.getName() @ ".cs";
			
      %datablock.save(%filename);
   }

   echo("Datablock total: " @ %count);
}

function stripFields(%this)
{
   if(%this $= "")
      %this = MissionGroup;

   for (%i = 0; %i < %this.getCount(); %i++)
   {
      %obj = %this.getObject(%i);
      if (%obj.getClassName() $= SimGroup)
      {
         %obj.powerCount = "";
         %obj.team = "";
         stripFields(%obj);
      }
      else 
      {
         %obj.isAIControlled = "";
         %obj.team = "";
         %obj.powerCount = "";
         %obj.hidden = "";
         %obj.locked = "true";
      }
   }
}

function findNextGridPoint(%axis)
{
   // Code: Martin "Founder" Hoover
   // www.garagegames.com/my/home/view.profile.php?qid=5055

   if(%axis == 0)
      return 0;

   %square = NameToID("MissionGroup/Terrain").squaresize * 8;

   %axis = mFloor(%axis);
   %remainder = %axis % %square;

   if(%remainder >= (%square / 2))
   {
      while(%axis % %square != 0)
         %axis++;
   }
   else
   {
      while(%axis % %square != 0)
         %axis--;
   }
   return %axis;
}

$GridPoint::PosCount = 0;
function generateGridPointArray(%multi)
{
   // Code: Martin "Founder" Hoover
   // www.garagegames.com/my/home/view.profile.php?qid=5055

   // "x x x x"   (startPointX StartPointY WidthTotal heightTotal)
   %area = NameToID("MissionGroup/PlayArea").area;

   error( "Play Area:" SPC %area );

   %square = NameToID("MissionGroup/Terrain").squareSize * %multi;

   error( "Square Size:" SPC %square);

   %minX = getWord( %area, 0 );
   %minY = getWord( %area, 1 );
   %maxX = getWord( %area, 2 );
   %maxY = getWord( %area, 3 );

   %xStart = findNextGridPoint( %minX );
   %yStart = findNextGridPoint( %minY );
   %xEnd = findNextGridPoint( %xStart + %maxX );
   %yEnd = findNextGridPoint( %yStart + %maxY );

   for ( %x = %xStart; %x < %xEnd; %x+=%square )
   {
      for ( %y = %yStart; %y < %yEnd; %y+=%square )
      {
         %z = getTerrainHeight( %x SPC %y ) + 1.0; // Set above terrain 1 unit
         %pos = %x SPC %y SPC %z;

         $GridPoint::Pos[$GridPoint::PosCount++] = %pos;
      }
   }
   export("$GridPoint::*", "logs/gridPoints.cs", false);
}
