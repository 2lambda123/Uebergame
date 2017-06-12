//-----------------------------------------------------------------------------
// Bot Inventory loadout sets, will chose one out of each group at random
//-----------------------------------------------------------------------------

// Loadouts for the default soldier class
//-----------------------------------------------------------------------------
function AIClient::getRandomLoadout(%this, %client, %gameType)
{
   %index = getRandom( 1, $BotInventoryIndex );
   return( $BotInventorySet[%index] );
}

$BotInventoryIndex = 0;

//riflers
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tLurker rifle\tSpecial\tMunitions\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tLurker rifle\tSpecial\tMedical\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tLurker rifle\tSpecial\tMunitions\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tLurker rifle\tSpecial\tMedical\tGrenade\tGrenade";
//shotgunners
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tShotgun\tSpecial\tMunitions\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tShotgun\tSpecial\tMedical\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tShotgun\tSpecial\tMunitions\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tShotgun\tSpecial\tMedical\tGrenade\tGrenade";
//grenadiers
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tGrenade Launcher\tSpecial\tMunitions\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tGrenade Launcher\tSpecial\tMedical\tGrenade\tGrenade";
//snipers
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tSniper Rifle\tSpecial\tMunitions\tGrenade\tGrenade";
$BotInventorySet[$BotInventoryIndex++] = "armor\tSoldier\tWeapon\tSniper Rifle\tSpecial\tMedical\tGrenade\tGrenade";

// Loadouts for the paintballer class
//-----------------------------------------------------------------------------
function AIClient::getRandomLoadout2(%this, %client, %gameType)
{
   %index = getRandom( 1, $BotInventoryIndex2 );
   return( $BotInventorySet2[%index] );
}

$BotInventoryIndex2 = 0;

//paintball deathmatch loadouts
$BotInventorySet2[$BotInventoryIndex2++] = "armor\tPaintballer\tWeapon\tBlue Marker\tSpecial\tEmpty\tGrenade\tEmpty";
$BotInventorySet2[$BotInventoryIndex2++] = "armor\tPaintballer\tWeapon\tRed Marker\tSpecial\tEmpty\tGrenade\tEmpty";
$BotInventorySet2[$BotInventoryIndex2++] = "armor\tPaintballer\tWeapon\tGreen Marker\tSpecial\tEmpty\tGrenade\tEmpty";
$BotInventorySet2[$BotInventoryIndex2++] = "armor\tPaintballer\tWeapon\tYellow Marker\tSpecial\tEmpty\tGrenade\tEmpty";
//team blue
$BotInventorySet3[$BotInventoryIndex3++] = "armor\tPaintballer\tWeapon\tBlue Marker\tSpecial\tEmpty\tGrenade\tEmpty";
//team red
$BotInventorySet4[$BotInventoryIndex4++] = "armor\tPaintballer\tWeapon\tRed Marker\tSpecial\tEmpty\tGrenade\tEmpty";
