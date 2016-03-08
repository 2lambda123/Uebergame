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

//--------------------------------------------------------------------------
// TYPES OF ALLOWED DAMAGE
//--------------------------------------------------------------------------

$DamageCount = 0;

//-------------------------------------------------------------------------- Game
$DamageType::Default = $DamageCount;
$DamageText[$DamageCount] = "Default";

$DamageType::Impact = $DamageCount++;
$DamageText[$DamageCount] = "Impact";

$DamageType::Ground = $DamageCount++;
$DamageText[$DamageCount] = "Ground";

$DamageType::Crash = $DamageCount++;
$DamageText[$DamageCount] = "Crash";

$DamageType::OutOfBounds = $DamageCount++;
$DamageText[$DamageCount] = "Out of Bounds";

$DamageType::Lava = $DamageCount++;
$DamageText[$DamageCount] = "Lava";

$DamageType::Water = $DamageCount++;
$DamageText[$DamageCount] = "Water";

$DamageType::Lightning = $DamageCount++;
$DamageText[$DamageCount] = "Lightning";

$DamageType::VehicleSpawn = $DamageCount++;
$DamageText[$DamageCount] = "Spawning Vehicle";

$DamageType::Explosion = $DamageCount++;
$DamageText[$DamageCount] = "Explosion";

$DamageType::Suicide = $DamageCount++;
$DamageText[$DamageCount] = "Suicide";

$DamageType::ScriptDamage = $DamageCount++;
$DamageText[$DamageCount] = "ScriptDamage";

$DamageType::QuickSand = $DamageCount++;
$DamageText[$DamageCount] = "QuickSand";

//-------------------------------------------------------------------------- Weapons
$DamageType::Melee = $DamageCount++;
$DamageText[$DamageCount] = "Melee";

$DamageType::Pistol = $DamageCount++;
$DamageText[$DamageCount] = "Pistol";

$DamageType::Shotgun = $DamageCount++;
$DamageText[$DamageCount] = "Shotgun";

$DamageType::Rifle = $DamageCount++;
$DamageText[$DamageCount] = "Assault Rifle";

$DamageType::Sniper = $DamageCount++;
$DamageText[$DamageCount] = "Sniper Rifle";

$DamageType::GrenadeLauncher = $DamageCount++;
$DamageText[$DamageCount] = "Grenade Launcher";

$DamageType::Rocket = $DamageCount++;
$DamageText[$DamageCount] = "Rocket";

//-------------------------------------------------------------------------- Grenades - Mines etc.
$DamageType::Grenade = $DamageCount++;
$DamageText[$DamageCount] = "Grenade";

$DamageType::Mine = $DamageCount++;
$DamageText[$DamageCount] = "Mine";

$DamageType::ShapeCharge = $DamageCount++;
$DamageText[$DamageCount] = "Shape Charge";

//-------------------------------------------------------------------------- Turrets
$DamageType::Turret = $DamageCount++;
$DamageText[$DamageCount] = "Turret";

//----------------------------------------------------------------------------
// TURRET DAMAGE PROFILES
//----------------------------------------------------------------------------

datablock SimDataBlock(TurretDamageScale)
{
   shieldDamageScale[$DamageType::Default] = 1.0;
   shieldDamageScale[$DamageType::Impact] = 1.25;
   shieldDamageScale[$DamageType::Ground] = 1.0;
   shieldDamageScale[$DamageType::Crash] = 1.0;
   shieldDamageScale[$DamageType::OutOfBounds] = 1.0;
   shieldDamageScale[$DamageType::Lava] = 1.0;
   shieldDamageScale[$DamageType::Water] = 1.0;
   shieldDamageScale[$DamageType::Lightning] = 5.0;
   shieldDamageScale[$DamageType::VehicleSpawn] = 1.0;
   shieldDamageScale[$DamageType::Explosion] = 2.0;
   shieldDamageScale[$DamageType::Suicide] = 2.0;
   shieldDamageScale[$DamageType::ScriptDamage] = 2.0;
   
   shieldDamageScale[$DamageType::Melee] = 1.0;
   shieldDamageScale[$DamageType::Pistol] = 0.8;
   shieldDamageScale[$DamageType::Sniper] = 1.0;
   shieldDamageScale[$DamageType::Rifle] = 1.0;
   shieldDamageScale[$DamageType::Shotgun] = 0.8;
   shieldDamageScale[$DamageType::GrenadeLauncher] = 1.5;
   shieldDamageScale[$DamageType::Rocket] = 2.0;

   shieldDamageScale[$DamageType::Grenade] = 1.5;
   shieldDamageScale[$DamageType::Mine] = 3.0;
   shieldDamageScale[$DamageType::ShapeCharge] = 4.5;

   damageScale[$DamageType::Default] = 1.0;
   damageScale[$DamageType::Impact] = 1.0;
   damageScale[$DamageType::Ground] = 1.0;
   damageScale[$DamageType::Crash] = 1.0;
   damageScale[$DamageType::OutOfBounds] = 1.0;
   damageScale[$DamageType::Lava] = 1.0;
   damageScale[$DamageType::Water] = 1.0;
   damageScale[$DamageType::Lightning] = 5.0;
   damageScale[$DamageType::VehicleSpawn] = 1.0;
   damageScale[$DamageType::Explosion] = 1.0;
   damageScale[$DamageType::Suicide] = 2.0;
   damageScale[$DamageType::ScriptDamage] = 2.0;

   damageScale[$DamageType::Melee] = 0.5;
   damageScale[$DamageType::Pistol] = 0.8;
   damageScale[$DamageType::Sniper] = 1.0;
   damageScale[$DamageType::Rifle] = 0.75;
   damageScale[$DamageType::Shotgun] = 0.9;
   damageScale[$DamageType::GrenadeLauncher] = 1.0;
   damageScale[$DamageType::Rocket] = 1.0;

   damageScale[$DamageType::Grenade] = 1.0;
   damageScale[$DamageType::Mine] = 1.5;
   damageScale[$DamageType::ShapeCharge] = 1.5;
};

//----------------------------------------------------------------------------
// STATIC SHAPE DAMAGE PROFILES
//----------------------------------------------------------------------------

datablock SimDataBlock(StaticShapeDamageScale)
{
   shieldDamageScale[$DamageType::Default] = 1.0;
   shieldDamageScale[$DamageType::Impact] = 1.25;
   shieldDamageScale[$DamageType::Ground] = 1.0;
   shieldDamageScale[$DamageType::Crash] = 1.0;
   shieldDamageScale[$DamageType::OutOfBounds] = 1.0;
   shieldDamageScale[$DamageType::Lava] = 1.0;
   shieldDamageScale[$DamageType::Water] = 1.0;
   shieldDamageScale[$DamageType::Lightning] = 5.0;
   shieldDamageScale[$DamageType::VehicleSpawn] = 1.0;
   shieldDamageScale[$DamageType::Explosion] = 2.0;
   shieldDamageScale[$DamageType::Suicide] = 2.0;
   shieldDamageScale[$DamageType::ScriptDamage] = 2.0;

   shieldDamageScale[$DamageType::Melee] = 1.0;
   shieldDamageScale[$DamageType::Pistol] = 0.8;
   shieldDamageScale[$DamageType::Sniper] = 1.0;
   shieldDamageScale[$DamageType::Rifle] = 1.0;
   shieldDamageScale[$DamageType::Shotgun] = 1.0;
   shieldDamageScale[$DamageType::GrenadeLauncher] = 1.2;
   shieldDamageScale[$DamageType::Rocket] = 2.0;

   shieldDamageScale[$DamageType::Grenade] = 1.2;
   shieldDamageScale[$DamageType::Mine] = 2.0;
   shieldDamageScale[$DamageType::ShapeCharge] = 6.0;

   damageScale[$DamageType::Default] = 1.0;
   damageScale[$DamageType::Impact] = 1.25;
   damageScale[$DamageType::Ground] = 1.0;
   damageScale[$DamageType::Crash] = 1.0;
   damageScale[$DamageType::OutOfBounds] = 1.0;
   damageScale[$DamageType::Lava] = 1.0;
   damageScale[$DamageType::Water] = 1.0;
   damageScale[$DamageType::Lightning] = 5.0;
   damageScale[$DamageType::VehicleSpawn] = 1.0;
   damageScale[$DamageType::Explosion] = 1.0;
   damageScale[$DamageType::Suicide] = 2.0;
   damageScale[$DamageType::ScriptDamage] = 2.0;

   damageScale[$DamageType::Melee] = 0.5;
   damageScale[$DamageType::Pistol] = 1.0;
   damageScale[$DamageType::Sniper] = 1.0;
   damageScale[$DamageType::Rifle] = 1.0;
   damageScale[$DamageType::Shotgun] = 1.0;
   damageScale[$DamageType::GrenadeLauncher] = 1.2;
   damageScale[$DamageType::Rocket] = 1.5;

   damageScale[$DamageType::Grenade] = 1.2;
   damageScale[$DamageType::Mine] = 2.0;
   damageScale[$DamageType::ShapeCharge] = 4.0;
};

//----------------------------------------------------------------------------
// PLAYER DAMAGE PROFILES
//----------------------------------------------------------------------------

datablock SimDataBlock(ArmorDamageScale)
{
   damageScale[$DamageType::Default] = 1.0;
   damageScale[$DamageType::Impact] = 1.0;
   damageScale[$DamageType::Ground] = 1.0;
   damageScale[$DamageType::Crash] = 1.0;
   damageScale[$DamageType::OutOfBounds] = 1.0;
   damageScale[$DamageType::Lava] = 1.0;
   damageScale[$DamageType::Water] = 1.0;
   damageScale[$DamageType::Lightning] = 1.0;
   damageScale[$DamageType::VehicleSpawn] = 1.0;
   damageScale[$DamageType::Explosion] = 1.0;
   damageScale[$DamageType::Suicide] = 2.0;
   damageScale[$DamageType::ScriptDamage] = 2.0;

   damageScale[$DamageType::Melee] = 1.0;
   damageScale[$DamageType::Pistol] = 1.0;
   damageScale[$DamageType::Rifle] = 1.0;
   damageScale[$DamageType::Sniper] = 1.0;
   damageScale[$DamageType::Shotgun] = 1.0;
   damageScale[$DamageType::GrenadeLauncher] = 1.0;
   damageScale[$DamageType::Rocket] = 1.0;

   damageScale[$DamageType::Grenade] = 1.0;
   damageScale[$DamageType::Mine] = 1.0;
   damageScale[$DamageType::ShapeCharge] = 1.0;
};
