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

//-----------------------------------------------------------------------------
// GameBase

function GameBaseData::onAdd(%data, %obj)
{
   //function was added to reduce console err msg spam
}

//-----------------------------------------------------------------------------
// ShapeBase

function ShapeBaseData::damage(%this, %obj, %position, %source, %amount, %damageType)
{
   // Ignore damage by default. This empty method is here to avoid console warnings.
}

function ShapeBaseData::onInventory(%this,%user,%value)
{
   // Called from ShapeBase::setInventory
   // Invoked whenever an user's inventory total changes for
   // this datablock.
}

function ShapeBase::onInventory(%this, %data, %value)
{
   // Called from ShapeBase::setInventory
   // Invoked on ShapeBase objects whenever their inventory changes
   // for the given datablock.
}

//function RigidShape::getState(%this)
//{
 // Thou shalt not spam
//}

//function StaticShape::getState(%this)
//{
 // Thou shalt not spam
//}

//function Vehicle::getState(%this)
//{
 // Thou shalt not spam
//}

//-----------------------------------------------------------------------------
// SimObject

function SimObject::onAdd(%data, %obj)
{
   //function was added to reduce console err msg spam
}

function SimObject::damage(%this, %obj, %position, %source, %amount, %damageType)
{
   //function was added to reduce console err msg spam
}

function SimObject::decCatagory(%this)
{
   //function was added to reduce console err msg spam
}

function SimObject::incCatagory(%this)
{
   //function was added to reduce console err msg spam
}

function SimObject::onInventory(%this, %obj)
{
   // Called from ShapeBase::setInventory
   //function was added to reduce console error msg spam
}

function SimObject::initializeObjective(%this)
{
   //function was added to reduce console err msg spam
}

function SimObject::getType(%this)
{
   // Thou shalt not spam
}

function SimObject::setupPositionMarkers(%group, %create)
{
   //function was added to reduce console err msg spam
}

function MissionMarker::onTrigger(%obj, %triggerId, %val)
{
   // Thou shalt not spam
}

function WayPoint::onTrigger(%obj, %triggerId, %val)
{
   // Thou shalt not spam
}

function SpawnSphere::onTrigger(%obj, %triggerId, %val)
{
   // Thou shalt not spam
}

//-----------------------------------------------------------------------------
// Interior

function VehicleBlocker::getDataBlock(%this)
{
   return %this;
}

function VehicleBlocker::getName(%this)
{
   return "Blocker";
}

function InteriorInstance::getDataBlock(%this)
{
   return %this;
}

function InteriorInstance::getName(%this)
{
   return "Interior";
}

function PhysicalZone::getDataBlock(%this)
{
   return %this;
}

function PhysicalZone::getName(%this)
{
   return "pz";
}

//-----------------------------------------------------------------------------
// Water

function WaterBlock::damage(%this)
{
   // Do nothing
}

function TerrainBlock::damage(%this)
{
   // Do nothing
}

//-----------------------------------------------------------------------------
// Terrain

function TerrainBlock::getDataBlock(%this)
{
   return %this;
}

function TerrainBlock::getName(%this)
{
   return "Terrain";
}

function Projectile::isMounted(%this)
{
   return false;
}