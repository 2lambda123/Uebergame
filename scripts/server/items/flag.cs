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

datablock ItemData(Flag)
{
   category = "Objectives";
   shapeFile = "art/shapes/objectives/flags/flag_01.dts";
   computeCRC = false;
   emap = false;

   mass = 25;
   drag = 0.5;
   density = 2;
   elasticity = 0.2;
   friction = 0.6;
   sticky = false;
   gravityMod = 1;
   maxVelocity = 16;
   dynamicType = $TypeMasks::ItemObjectType;

   image = FlagImage;
   canImpulse = true;

   lightType = "NoLight";
   lightRadius = 3;
   lightColor = "0.5 0.5 0.5";
   lightOnlyStatic = false;

   pickUpName = 'Flag';
   pickupSound = WeaponPickupSound;
   throwSound = ThrowSnd;
};

// we need extra blue/red flag items, since items are not skinnable at the moment
datablock ItemData(BlueFlag: Flag)
{
   shapeFile = "art/shapes/objectives/flags/flag_01_blue.dts";
};

datablock ItemData(RedFlag: Flag)
{
   shapeFile = "art/shapes/objectives/flags/flag_01_red.dts";
};

datablock ShapeBaseImageData(FlagImage)
{
   shapeFile = "art/shapes/objectives/flags/flag_01.dts";
   //shapeFileFP = "art/shapes/objectives/flags/flag_01.dts";

   emap = false;
   cloakable = false;

   mountPoint = 1;
   offset     = "0.1 -0.1 -0.4"; // L/R - F/B - T/B
   rotation = "0 0 1 180";
   //rotation   = "-0.616949 0.628951 0.473069 80.6613";
   //mountPoint = 2;
   //offset = "-0.15 -0.22 0.7"; // L/R - F/B - T/B
   //rotation = "-0.0353276 0.202168 0.978714 199.41";
   firstPerson = true;
   useEyeNode = true;
   animateOnServer = true;
   mass = 4;

   correctMuzzleVector = false;

   throwTimeout = 800;
   item = Flag;
   usesEnergy = 0;
   minEnergy = 0;

   lightType = "NoLight";
   lightColor = "0.5 0.5 0.5";
   lightTime = 1000;
   lightRadius = 3;

   stateName[0] = "Preactivate";
   stateTransitionOnLoaded[0]      = "Ready";

   stateName[1]                    = "Ready";
   stateTransitionOnTriggerDown[1] = "Charge";

   stateName[2]                    = "Charge";
   stateScript[2]                  = "chargeStart";
   stateTransitionOnTriggerUp[2]   = "Activate";
   stateWaitForTimeout[2]          = false;

   stateName[3]                    = "Activate";
   stateScript[3]                  = "onActivate";
   stateSequence[3]                = "fire";
};

// we need extra blue/red flag weaponImages, since weaponImages are not skinnable at the moment
datablock ShapeBaseImageData(BlueFlagImage : FlagImage)
{
   shapeFile = "art/shapes/objectives/flags/flag_01_blue.dts";
};

datablock ShapeBaseImageData(RedFlagImage : FlagImage)
{
   shapeFile = "art/shapes/objectives/flags/flag_01_red.dts";
};

function Flag::onAdd(%this, %obj)
{
   Parent::onAdd(%this, %obj);
   %obj.static = false;
   %obj.rotate = false;
   %obj.playThread( 0, "ambient" );
}

function Flag::onCollision(%data, %flag, %col, %vec, %speed)
{
   if ( %col.getType() & ( $TypeMasks::PlayerObjectType ) )
   {
      if ( %col.getState() $= "Dead" || %col.isMounted() || %col.flagTossWait )
         return;

      Game.onTouchFlag( %col, %flag );
   }
}

function Flag::onEnterLiquid(%data, %flag, %coverage, %type)
{
   if ( %type > 3 )
      Game.resetFlag(%flag);
}

//-----------------------------------------------------------------------------

function FlagImage::chargeStart(%data, %obj, %slot)
{
   //LogEcho("FlagImage::chargeStart(" SPC %data.getName() @", "@ %obj.getClassname() @", "@ %slot SPC ")");
   %obj.startTime = getSimTime();
}

function FlagImage::onActivate(%data, %obj, %slot)
{
   //LogEcho("FlagImage::onActivate(" SPC %data.getName() @", "@ %obj.getClassname() @", "@ %slot SPC ")");

   if ( %obj.holdingFlag $= "" )
      return;

   // Work out the force of the throw
   %throwStrength = (getSimTime() - %obj.startTime) / 1000;
   if( %throwStrength < 1 ) // Realy really short hold?
      %throwStrength = 1;

   if( %throwStrength > 2 )
      %throwStrength = 2;

   %obj.throwStrength = %throwStrength;

   // Should handle un-hide of item
   %obj.throwObject( %obj.holdingFlag );

   // We have to schedule image dismount to avoid engine crash.. this is very annoying
   // Function reside in shapebase.cs
   %obj.schedule( 50, "dismountImage", %slot );

   // Tell the game
   Game.onFlagDropped( %obj, %obj.holdingFlag );

   // Keep his object from colliding with the flag unil some things are straight
   %obj.flagTossWait = true;
   Game.schedule( 1000, resetFlagTossWait, %obj );
}

function FlagImage::onMount(%data, %obj, %slot)
{
   echo("FlagImage::onMount(" SPC %data.getName() @", "@ %obj.getClassname() @", "@ %slot SPC ")");
}

function FlagImage::onUnmount(%data, %obj, %slot)
{
   echo("FlagImage::onUnmount(" SPC %data.getName() @", "@ %obj.getClassname() @", "@ %slot SPC ")");
}
