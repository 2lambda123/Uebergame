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

datablock SFXProfile(BasePowerOn)
{
   filename    = "art/sound/turret/wpn_turret_deploy.wav";
   description = Audio2D;
   preload = true;
};

datablock SFXProfile(BasePowerOff)
{
   filename    = "art/sound/items/health_mono_01.ogg";
   description = Audio2D;
   preload = true;
};

datablock SFXProfile(BasePowerHum)
{
   filename    = "art/sound/turret/wpn_turret_scan.wav";
   description = AudioLooping2D;
   preload = true;
};

//-----------------------------------------------------------------------------
// Power  -  Functions

function GameBase::clearPower(%this)
{
   // Thou shalt not spam
}

function SimGroup::clearPower(%this)
{
   %this.powerCount = 0;
   for ( %i = 0; %i < %this.getCount(); %i++ )
   {
      %obj = %this.getObject(%i);
      if ( %obj.getType() & $TypeMasks::GameBaseObjectType )
         %obj.clearPower();
   }
}

function SimObject::powerInit(%this, %powerCount)
{
   // Thou shalt not spam
}

function SimGroup::powerInit(%this, %powerCount)
{
   if ( %this.providesPower )
      %powerCount++;

   %count = %this.getCount();
   for ( %i = 0; %i < %count; %i++ )
   {
      %obj = %this.getObject(%i);
      if ( %obj.getType() & $TypeMasks::GameBaseObjectType )
      {
         if ( %obj.getDatablock().isPowering( %obj ) )
            %powerCount++;
      }
   }
   %this.powerCount = %powerCount;
   for ( %i = 0; %i < %this.getCount(); %i++ )
   {
      %obj = %this.getObject(%i);
      %obj.powerInit( %powerCount );
   }
}

function GameBase::powerInit(%this, %powerCount)
{
   if(%powerCount)
      %this.getDatablock().gainPower(%this);
   else
      %this.getDataBlock().losePower(%this);
}

function SimObject::isPowering(%data, %obj)
{
   return false;
}

function SimObject::updatePowerCount()
{
   // Thou shalt not spam
}

function SimObject::powerCheck()
{
   // Thou shalt not spam
}

function SimGroup::updatePowerCount(%this, %value)
{
   if ( %this.powerCount > 0 || %value > 0 )
      %this.powerCount += %value;

   for ( %i = 0; %i < %this.getCount(); %i++ )
   {
      %this.getObject(%i).updatePowerCount( %value );
   }

   for ( %i = 0; %i < %this.getCount(); %i++ )
      %this.getObject(%i).powerCheck( %this.powerCount );
}

function GameBaseData::gainPower(%data, %obj)
{
   // Thou shalt not spam
}

function GameBaseData::losePower(%data, %obj)
{
   // Thou shalt not spam
}

function InteriorInstance::powerCheck(%this, %powerCount)
{
   if(%powerCount > 0)
      %mode = "Off";
   else
      %mode = "On";

   %this.setAlarmMode(%mode);
}

function GameBase::powerCheck(%this, %powerCount)
{
   if ( %powerCount || %this.selfPower )
      %this.getDatablock().gainPower(%this);
   else
      %this.getDatablock().losePower(%this);
}

function GameBase::incPowerCount(%this)
{
   %this.getGroup().updatePowerCount(1);
}

function GameBase::decPowerCount(%this)
{
   %this.getGroup().updatePowerCount(-1);
}

function GameBase::setSelfPowered(%this)
{
   if ( !%this.isPowered() )
   {
      %this.selfPower = true;
      if(%this.getDatablock().deployedObject)
         %this.initDeploy = true;

      %this.getDataBlock().gainPower(%this);
   }
   else
      %this.selfPower = true;
}

function GameBase::clearSelfPowered(%this)
{
   %this.selfPower = "";
   if ( !%this.isPowered() )
      %this.getDataBlock().losePower(%this);
}

function GameBase::isPowered(%this)
{
   return %this.selfPower || %this.getGroup().powerCount > 0;
}
