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

datablock StaticShapeData(FlagStand)
{
   category = "Objectives";
   //className = "StaticShapeData";
   shapeFile = "art/shapes/objectives/flag_stands/flag_stand_01.dts";
   computeCRC = false;
   emap = true;
   dynamicType = $TypeMasks::StaticShapeObjectType;
   scale = "4 4 0.2"; // 3 3 0.1
   isInvincible = true;

   // Script access
   nameTag = 'Flag Stand';

   damageSound = "";
   ambientSound = "";
   deployedObject = false;
};

function FlagStand::initializeObjective(%data, %obj)
{
   //%obj.emitter = new ParticleEmitterNode() {
   //   scale = "1 1 1";
   //   dataBlock = "DefaultEmitterNode";
   //   emitter = "Team" @ %obj.team @ "Emitter";
   //   velocity = "1";
   //};
   //MissionCleanup.add( %obj.emitter );

   //%pos = VectorAdd( %obj.GetBoxCenter(), "0 0 0.1" );
   //%obj.emitter.setTransform( %pos SPC "1 0 0 0" );

   // Trigger does the work
   %obj.trigger = new Trigger()
   {
      dataBlock = GameTrigger;
      polyhedron = "0 0 0 1 0 0 0 -1 0 0 0 1";
      scale = "2 2 3";
      parent = %obj;
      team = %obj.team;
   };
   MissionCleanup.add( %obj.trigger );
   //%pos = MatrixMulPoint( %obj.getTransform(), "-1 1.3 0" ); //"-0.5 0.8 0" if scale 1 1 1
   %pos = MatrixMulPoint( %obj.getTransform(), "-1 1.2 0" );
   %obj.trigger.setTransform( %pos SPC %obj.rotFromTransform() );

   %blocker = new VehicleBlocker()
   {
      position = %obj.getPosition();
      rotation = %obj.rotFromTransform();
      dimensions = "4 4 4";
   };
   MissionCleanup.add(%blocker);
   %blocker.setTransform( %obj.getTransform() );

   %waypoint = new WayPoint() {
      position = %obj.getPosition();
      rotation = %obj.rotFromTransform();
      scale = "1 1 1";
      dataBlock = "WayPointMarker";
      name = ( %obj.team > 0 ) ? $pref::Server::teamName[%obj.team] @ " Flag Stand" : "Flag Stand";
      team = %obj.team;
      locked = "true";
   };
   missionCleanup.add(%waypoint);

   if ( %obj.team > 0 && %obj.team <= Game.numTeams )
      %obj.setSkinName("base" @ %obj.team);
}

function FlagStand::onCollision(%this, %obj, %col)
{
   // If the dts collision is setup correctly we could do the following
   // For now we use a trigger to do the work

   //if ( ( %col.getClassName() !$= "Player" && %col.getClassName() !$= "AiIPlayer" ) || 
   //       %col.getState() $= "Dead" || %col.isMounted() )
   //   return;

   //Game.onFlagStandCollision( %obj, %col );
}

