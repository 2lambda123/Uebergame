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
function StaticShapeData::create(%data)
{
   // The mission editor invokes this method when it wants to create
   // an object of the given datablock type.

   %obj = new StaticShape() {
      dataBlock = %data;
      scale = %data.scale !$= "" ? %data.scale : "1 1 1";
   };
   return %obj;
}

function StaticShapeData::onAdd(%data, %obj)
{
   %obj.setEnergyLevel(%data.MaxEnergy);
   %obj.setRechargeRate(%data.rechargeRate);
   %obj.setRepairRate(0);
   %obj.playThread(0, "Ambient");
}

function StaticShapeData::damage(%data, %obj, %sourceObject, %position, %amount, %damageType)
{
   if ( %obj.isDestroyed() || %data.isInvincible || !$BaseSacking )
      return;

   //echo("StaticShapeData::damage( "@%data.getName()@", "@%obj@", "@sourceObject@", "@%position@", "@%amount@", "@%damageType@" )");

   if ( isObject( %sourceObject ) )
      %obj.lastDamagedBy = %sourceObject;
   else
      %obj.lastDamagedBy = 0;

   %obj.damageTimeMS = GetSimTime();

   if ( %data.isShielded )
      %amount = %obj.imposeShield(%position, %amount, %damageType); // Resides in server/shapeBase.cs

   // Cap the amount of damage applied if same team
   //if ( !$FriendlyFire && !%data.deployedObject )
   if ( !%data.deployedObject )
   {
      if ( isObject( %sourceObject ) )
      {
         if ( %sourceObject.team == %obj.team )
         {
            %curDamage = %obj.getDamageLevel();
            %availableDamage = %data.disabledLevel - %curDamage - 0.05;
            if ( %amount > %availableDamage )
               %amount = %availableDamage;
         }
      }
   }

   %damageScale = %data.damageScale[%damageType];
   if( %damageScale !$= "" )
      %amount *= %damageScale;

   // apply damage
   if ( %amount > 0 )
      %obj.applyDamage(%amount);
}

function StaticShapeData::onDamage(%data, %obj)
{
   //echo("StaticShapeData::onDamage( "@%data@", "@%obj@" )");
   // Set damage state based on current damage level
   %damage = %obj.getDamageLevel();
   if ( %damage >= %data.destroyedLevel )
   {
      if ( %obj.getDamageState() !$= "Destroyed" )
      {
         %obj.setDamageState(Destroyed);

         // Let the game object have a shot at doing something with this information..
         // We put this here so we have some time before the vehicle is deleted.
         if ( isObject( %obj.lastDamagedBy ) && isObject( Game ) )
            Game.staticShapeDestroyed( %obj, %obj.lastDamagedBy );

         // if object has an explosion damage radius associated with it, apply explosion damage
         if ( %data.damageRadius )
            radiusDamage(%obj, 0, %obj.getWorldBoxCenter(), %data.damageRadius, %data.radiusDamage, %data.radiusDamageType, %data.areaImpulse);

         %obj.setDamageLevel(%data.maxDamage);
      }
   }
   else if ( %damage >= %data.disabledLevel )
   {
      if ( %obj.getDamageState() !$= "Disabled" )
         %obj.setDamageState(Disabled);
   }
   else
   {
      // Lets add some sound to this, grab the sound profile from the datablock
      if ( %data.damageSound !$= "" )
         ServerPlay3D(%data.damageSound, %obj.getTransform());

      if ( %obj.getDamageState() !$= "Enabled" )
         %obj.setDamageState(Enabled);
   }
}

function StaticShapeData::onEnabled(%data, %obj, %state)
{
   //echo("StaticShapeData::onEnabled( "@%data@", "@%obj@", "@%state@" )");

   // Lets add some sound to this, grab the sound profile from the datablock
   if ( %data.ambientSound !$= "" )
      ServerPlay3D(%data.ambientSound, %obj.getTransform());

   if ( %obj.isPowered() )
      %data.onGainPowerEnabled(%obj);

   %obj.playThread(0, "Ambient");
}

function StaticShapeData::onDisabled(%data, %obj, %state)
{
   //echo("StaticShapeData::onDisabled( "@%data@", "@%obj@", "@%state@" )");

   if ( %obj.isPowered() || ( %data.className $= "Generator" ) )
      %data.onLosePowerDisabled(%obj);

   %obj.wasDisabled = true;
   %obj.playThread(0, "Damage");
}

function StaticShapeData::onDestroyed(%data, %obj, %prevState)
{
   //echo("StaticShapeData::onDestroyed( "@%data@", "@%obj@", "@%prevState@" )");

   // delete object
   if ( !%data.renderWhenDestroyed )
      %obj.schedule(200, "delete");
   else
      %obj.playThread(0, "Visibility");
}


//-----------------------------------------------------------------------------
// Power - Functions

function StaticShapeData::onGainPowerEnabled(%data, %obj)
{
   if(%data.ambientThreadPowered)
      %obj.playThread($AmbientThread, "ambient");
   // if it's a deployed object, schedule the power thread; else play it immediately
   if(%data.deployAmbientThread)
      %obj.schedule(750, "playThread", $PowerThread, "Power");
   else
      %obj.playThread($PowerThread,"Power");

   // deployable objects get their recharge rate set right away -- don't set it again unless
   // the object has just been re-enabled
   if ( %obj.initDeploy )
      %obj.initDeploy = false;
   else
   {
      if ( %obj.getRechargeRate() <= 0 )
      {
         %oldERate = %obj.getRechargeRate();
         %obj.setRechargeRate(%oldERate + %data.rechargeRate);
      }
   }
   if(%data.humSound !$= "")
      %obj.playAudio($HumSound, %data.humSound);

   %obj.setPoweredState(true);
}

function StaticShapeData::onLosePowerDisabled(%data, %obj)
{
   if(%data.ambientThreadPowered)
      %obj.pauseThread($AmbientThread);

   if(!%data.alwaysAmbient)
   {
      %obj.stopThread($PowerThread);
      %obj.setRechargeRate(0.0);
      %obj.setEnergyLevel(0.0);
   }

   if(%data.humSound !$= "")
      %obj.stopAudio($HumSound);

   %obj.setPoweredState(false);
}

function StaticShapeData::gainPower(%data, %obj)
{
   if ( %obj.isEnabled() )
      %data.onGainPowerEnabled(%obj);

   Parent::gainPower(%data, %obj);
}

function StaticShapeData::losePower(%data, %obj)
{
   if ( %obj.isEnabled() )
      %data.onLosePowerDisabled(%obj);

   Parent::losePower(%data, %obj);
}

//-----------------------------------------------------------------------------
// TSStatics

function TSStatic::create(%shapeName)
{
   %obj = new TSStatic()
   {
      shapeName = %shapeName;
   };
   return(%obj);
}

function TSStatic::damage(%this)
{
   // prevent console error spam
}
