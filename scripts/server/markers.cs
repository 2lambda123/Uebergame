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

datablock MissionMarkerData(WayPointMarker)
{
   category = "Misc";
   shapeFile = "art/editor/octahedron.dts";
};

datablock MissionMarkerData(SpawnSphereMarker)
{
   category = "Spawns";
   shapeFile = "art/editor/octahedron.dts";
};

datablock MissionMarkerData(CameraBookmarkMarker)
{
   category = "Misc";
   shapeFile = "art/editor/camera.dts";
};

datablock MissionMarkerData(FlagMarker)
{
   category = "Objectives";
   shapeFile = "art/editor/octahedron.dts";
};

datablock MissionMarkerData(VehicleMarker)
{
   category = "Misc";
   shapeFile = "art/editor/octahedron.dts";
};

//------------------------------------------------------------------------------
// - serveral marker types may share MissionMarker datablock type
function MissionMarkerData::create(%block)
{
   switch$(%block)
   {
      case "WayPointMarker":
         %obj = new WayPoint() {
            dataBlock = %block;
            name = "Waypoint";
            team = 0;
         };
         return(%obj);

      case "SpawnSphereMarker":
         %obj = new SpawnSphere() {
            datablock = %block;
         };
         return(%obj);

      case "FlagMarker":
         %obj = new MissionMarker() {
            datablock = %block;
         };
         return(%obj);

      case "VehicleMarker":
         %obj = new MissionMarker() {
            datablock = %block;
         };
         return(%obj);
   }
   return(-1);
}
