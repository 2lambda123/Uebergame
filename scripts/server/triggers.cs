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
// DefaultTrigger is used by the mission editor.  This is also an example
// of trigger methods and callbacks.

datablock TriggerData(DefaultTrigger)
{
   // The period is value is used to control how often the console
   // onTriggerTick callback is called while there are any objects
   // in the trigger.  The default value is 100 MS.
   tickPeriodMS = 100;
};

datablock TriggerData(ClientTrigger : DefaultTrigger)
{
   clientSide = true;
};

datablock TriggerData(GameTrigger)
{
   tickPeriodMS = 500;
};

datablock TriggerData(hairTrigger)
{
   tickPeriodMS = 30;
};

datablock TriggerData(slowTrigger)
{
   tickPeriodMS = 1000;
};

datablock TriggerData(DamageTrigger)
{
   tickPeriodMS = 1000;
};

function Trigger::initializeObjective(%this)
{
   %this.getDataBlock().initializeObjective(%this);
}

//-----------------------------------------------------------------------------

function SimGroup::onTrigger(%this, %triggerId, %on)
{
   //echo("SimGroup::onTrigger(" SPC %this @", "@ %triggerId @", "@ %on SPC ")");
   // Just relay the trigger event to all sub objects...
   //
   for (%i = 0; %i < %this.getCount(); %i++)
      %this.getObject(%i).onTrigger(%triggerId, %on);
}

function SimGroup::onTriggerTick(%this, %triggerId)
{
   //echo("SimGroup::onTriggerTick(" SPC %this @", "@ %triggerId SPC ")");
   // Just relay the trigger event to all sub objects...
   //
   for (%i = 0; %i < %this.getCount(); %i++)
      %this.getObject(%i).onTriggerTick(%triggerId);
}

function DefaultTrigger::onEnterTrigger(%this,%trigger,%obj)
{
   // This method is called whenever an object enters the %trigger
   // area, the object is passed as %obj.  The default onEnterTrigger
   // method (in the C++ code) invokes the ::onTrigger(%trigger,1) method on
   // every object (whatever it's type) in the same group as the trigger.
   Parent::onEnterTrigger(%this, %trigger, %obj);
}

function DefaultTrigger::onLeaveTrigger(%this,%trigger,%obj)
{
   // This method is called whenever an object leaves the %trigger
   // area, the object is passed as %obj.  The default onLeaveTrigger
   // method (in the C++ code) invokes the ::onTrigger(%trigger,0) method on
   // every object (whatever it's type) in the same group as the trigger.
   Parent::onLeaveTrigger(%this, %trigger, %obj);
}

function DefaultTrigger::onTickTrigger(%this,%trigger)
{
   // This method is called every tickPerioMS, as long as any
   // objects intersect the trigger. The default onTriggerTick
   // method (in the C++ code) invokes the ::onTriggerTick(%trigger) method on
   // every object (whatever it's type) in the same group as the trigger.

   // You can iterate through the objects in the list by using these
   // methods:
   //    %this.getNumObjects();
   //    %this.getObject(n);
   Parent::onTickTrigger(%this, %trigger);
}

//-----------------------------------------------------------------------------

function GameTrigger::onEnterTrigger(%this, %trigger, %colObj)
{
   //error("GameTrigger::onEnterTrigger(" SPC %this.getName() SPC %trigger.getName() SPC %colObj.client.nameBase SPC ")");
   //Parent::onEnterTrigger(%this, %trigger, %obj);

   if(isObject(Game))
      Game.onEnterTrigger(%this, %trigger, %colObj);
}

function GameTrigger::onLeaveTrigger(%this, %trigger, %colObj)
{
   //error("GameTrigger::onLeaveTrigger(" SPC %this.getName() SPC %trigger.getName() SPC %colObj.client.nameBase SPC ")");
   //Parent::onLeaveTrigger(%this, %trigger, %colObj);

   if(isObject(Game))
      Game.onLeaveTrigger(%this, %trigger, %colObj);
}

function GameTrigger::onTickTrigger(%this, %trigger)
{
   //Parent::onTickTrigger(%this, %trigger);

   if(isObject(Game))
      Game.onTickTrigger(%this, %trigger);
}

//-----------------------------------------------------------------------------

function TriggerData::addPlayerToTrigger(%data, %trigger, %player)
{
   %found = 0;
   for ( %a = 0; %trigger.occupied[%a] !$= ""; %a++ )
   {
      if ( %trigger.occupied[%a] == %player )
      {
         %found = 1; // allready in there
         break;
      }
   }

   // Didn't find him, add to the next free slot.
   if ( !%found )
      %trigger.occupied[%a++] = %player;
}

function TriggerData::remPlayerFromTrigger(%data, %trigger, %player)
{
   %count = 0;
   for(%a = 0; %trigger.occupied[%a] !$= ""; %a++)
   {
      if(%trigger.occupied[%a] != %player)
      {
         %Temp[%count] = %trigger.occupied[%a];
         %count++;
      }
   }
   for( %b = 0; %b < %count; %b++ )
      %trigger.occupied[%b] = %Temp[%b];
}

function TriggerData::isPlayerInTrigger(%data, %trigger, %player)
{
   %found = 0;
   for ( %a = 0; %trigger.occupied[%a] !$= ""; %a++ )
   {
      if ( %trigger.occupied[%a] == %player )
         %found = 1;
   }
   return( %found );
}

//-----------------------------------------------------------------------------

function DamageTrigger::onEnterTrigger(%this, %trigger, %colObj)
{
   // Filter out objects that aren't player objects
   if ( %colObj.getClassName() !$= "Player" && %colObj.getClassName() !$= "AIPlayer" )
      return;

   // Possibly a player object created this trigger
   %source = isObject(%trigger.sourceObject) ? %trigger.sourceObject : %trigger;
   %colObj.damage(%source, %colObj.posFromTransform(), %trigger.damageAmount, %trigger.damageType);
}

function DamageTrigger::onTickTrigger(%data, %trigger)
{
   // Possibly a player object created this trigger
   %source = isObject(%trigger.sourceObject) ? %trigger.sourceObject : %trigger;

   // Get a count of all the objects inside the trigger area
   %count = %trigger.getNumObjects();

   // Loop through all the objects in the trigger area
   for ( %i = 0; %i < %count; %i++ )
   {
      // Get the object at the index
      %colObj = %trigger.getObject( %i );

      // Filter out objects that aren't player objects
      if ( %colObj.getClassName() !$= "Player" && %colObj.getClassName() !$= "AIPlayer" )
         continue;

      if ( %colObj.getState() !$= "Dead" )
      {
         // Found a live player in the trigger area.
         // Owwwwwwwwww it HURTS!
         %colObj.damage(%source, %colObj.posFromTransform(), %trigger.damageAmount, %trigger.damageType);
      }
   }
}

