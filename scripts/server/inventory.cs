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

function serverCmdShowArmoryHud(%client)
{
   //LogEcho("serverCmdShowArmoryHud(" SPC %client.nameBase @", "@ %hud SPC ")");

   commandToClient( %client, 'OpenArmoryHud' );
   %client.numFavsCount = 0;
   SmsInv.updateArmoryHud( %client );
}

function serverCmdHideArmoryHud(%client)
{
   commandToClient( %client, 'CloseArmoryHud' );
}

function SMS::updateArmoryHud(%this, %client)
{
   %armorList = %client.loadout[0];

   %armor = $NameToData[%armorList];

   if ( %client.lastArmor !$= %armor )
   {
      %client.lastArmor = %armor;
      for ( %z = 0; %z < %client.lastNumFavs; %z++ )
         commandToClient( %client, 'RemoveArmoryLine', %z );

      %setLastNum = true;
   }

   // Get the current mission type so we can check for banned inventory.
   %cmt = $Server::MissionType;

   for ( %i = 0; %i < $SMS::MaxArmors; %i++ )
   {
      if ( $SMS::ArmorName[%i] !$= %client.loadout[0] )  
      {
         %armorList = %armorList TAB $SMS::ArmorName[%i];
      }
   }

   //-----------------------------------------------------------------------------
   // Create Weapon List
   for ( %y = 0; %y < $SMS::MaxWeapons; %y++ )
   {
      %notFound = true;
      for ( %i = 0; %i < getFieldCount( %client.weaponIndex ); %i++ )
      {
         %WInv = $NameToData[$SMS::WeaponName[%y]];
         if ( ( $SMS::WeaponName[%y] $= %client.loadout[getField( %client.weaponIndex, %i )] ) || !%armor.maxInv[%WInv] )  
         {
            %notFound = false;
            break;
         }
      }

      // Dont show it in the list if it is banned by gametype, flagged or class cannot carry.
      if ( !$GameBanList[%cmt, %WInv] && $SMS::ShowInInv[%WInv] && %armor.maxInv[%WInv] != 0 )
      {
         if ( %notFound && %weaponList $= "" )
            %weaponList = $SMS::WeaponName[%y];
         else if ( %notFound )
            %weaponList = %weaponList TAB $SMS::WeaponName[%y];
      }
   }

   //-----------------------------------------------------------------------------
   // Create Special List
   for ( %y = 0; %y < $SMS::MaxItems; %y++ )
   {
      %notFound = true;
      for(%i = 0; %i < getFieldCount( %client.specialIndex ); %i++)
      {
         %SInv = $NameToData[$SMS::ItemName[%y]];
         if ( ( $SMS::ItemName[%y] $= %client.loadout[getField( %client.specialIndex, %i )] ) || !%armor.maxInv[%SInv] )  
         {
            %notFound = false;
            break;
         }
      }

      if ( !$GameBanList[%cmt, %SInv] && %armor.maxInv[%SInv] != 0)
      { 
         if ( %notFound && %specialList $= "" )
            %specialList = $SMS::ItemName[%y];
         else if ( %notFound )
            %specialList = %specialList TAB $SMS::ItemName[%y];
      }
   }

   //-----------------------------------------------------------------------------
   // Create Grenade List
   for ( %i = 0; %i < $SMS::MaxGrenades; %i++ )
   {
      %notFound = true;
      for(%j = 0; %j < getFieldCount( %client.grenadeIndex ); %j++)
      {
         %GInv = $NameToData[$SMS::GrenadeName[%i]];
         if ( ( $SMS::GrenadeName[%i] $= %client.loadout[getField( %client.grenadeIndex, %j )] ) || !%armor.maxInv[%GInv] )  
         {
            %notFound = false;
            break;
         }
      }

      if ( !$GameBanList[%cmt, %GInv] && %armor.maxInv[%GInv] != 0)
      { 
         if ( %notFound && %grenadeList $= "" )
            %grenadeList = $SMS::GrenadeName[%i];
         else if ( %notFound )
            %grenadeList = %grenadeList TAB $SMS::GrenadeName[%i];
      }
   }

   // Don't add selectable armors
   %client.numFavsCount++;
   commandToClient( %client, 'SetArmoryLine', 0, "Class:", %armorList, armor, %client.numFavsCount );

   %lineCount = 1;
   //%lineCount = 0;
   //-----------------------------------------------------------------------------
   // Weapons
   for ( %x = 0; %x < %armor.maxWeapons; %x++ )
   {
      %client.numFavsCount++;
      if ( %x < getFieldCount( %client.weaponIndex ) )
      {
         %list = %client.loadout[getField( %client.weaponIndex, %x )];
         if ( %list $= "Invalid" && %list !$= "Empty")
         {
            %client.loadout[%client.numFavs] = "Invalid";
            %client.weaponIndex = %client.weaponIndex TAB %client.numFavs;
         }   
      }
      else
      {
         %list = "Empty";
         %client.loadout[%client.numFavs] = "Empty";
         %client.weaponIndex = %client.weaponIndex TAB %client.numFavs;
         %client.numFavs++;
      }

      if ( %list $= "Empty")
         %list = %list TAB %weaponList;
      else if ( %weaponList !$= "" )
         %list = %list TAB %weaponList TAB "Empty";
      else
         %list = %list TAB "Empty";

      commandToClient( %client, 'SetArmoryLine', %x + %lineCount, "Weapon Slot " @ %x + 1 @ ": ", %list, weapon, %client.numFavsCount );
   }
   %lineCount = %lineCount + %armor.maxWeapons;
   %client.numFavsCount++;

   //-----------------------------------------------------------------------------
   // Specials
   for( %x = 0; %x < %armor.maxSpecials; %x++ )
   {
      %client.numFavsCount++;
      if ( %x < getFieldCount( %client.specialIndex ) )
      {
         %list = %client.loadout[getField( %client.specialIndex, %x )];
         if (%list $= "Invalid")
         {
            %client.loadout[%client.numFavs] = "Invalid";
            %client.specialIndex = %client.specialIndex TAB %client.numFavs;
         }
      }
      else
      {
         %list = "Empty";
         %client.loadout[%client.numFavs] = "Empty";
         %client.specialIndex = %client.specialIndex TAB %client.numFavs;
         %client.numFavs++;
      }

      //if ( %list !$= "Invalid" )
      //{
         if ( %list $= "Empty" )
            %list = %list TAB %specialList;
         else if ( %specialList !$= "" )
            %list = %list TAB %specialList TAB "Empty";
         else 
            %list = %list TAB "Empty";
      //}
      commandToClient( %client, 'SetArmoryLine', %x + %lineCount, "Special " @ %x + 1 @ ": ", %list, special, %client.numFavsCount );
   }
   %lineCount = %lineCount + %armor.maxSpecials;

   //-----------------------------------------------------------------------------
   // Grenades
   for( %x = 0; %x < %armor.maxGrenades; %x++ )
   {
      %client.numFavsCount++;
      if ( %x < getFieldCount( %client.grenadeIndex ) )
      {
         %list = %client.loadout[getField( %client.grenadeIndex, %x )];
         if (%list $= "Invalid")
         {
            %client.loadout[%client.numFavs] = "Invalid";
            %client.grenadeIndex = %client.grenadeIndex TAB %client.numFavs;
         }
      }
      else
      {
         %list = "Empty";
         %client.loadout[%client.numFavs] = "Empty";
         %client.grenadeIndex = %client.grenadeIndex TAB %client.numFavs;
         %client.numFavs++;
      }

      //if ( %list !$= "Invalid" )
      //{
         if ( %list $= "Empty" )
            %list = %list TAB %grenadeList;
         else if ( %grenadeList !$= "" )
            %list = %list TAB %grenadeList TAB "Empty";
         else 
            %list = %list TAB "Empty";
      //}
      commandToClient( %client, 'SetArmoryLine', %x + %lineCount, "Grenade " @ %x + 1 @ ": ", %list, grenade, %client.numFavsCount );
   }

   if ( %setLastNum )
      %client.lastNumFavs = %client.numFavs;

   return( %client.numFavs );
}

//------------------------------------------------------------------------------
function serverCmdSetClientLoadout(%client, %list)
{
   %list = deTag( %list );

   //LogEcho("serverCmdSetClientLoadout(" SPC %client.nameBase @", "@ %list SPC ")");
   // Make sure there is a favortite else use a default
   if ( %list $= "" )
      %list = "armor\tSoldier";

   //%armorList = %client.gender SPC %client.race;
   //%armor = $NameToData[%armorList];

   %validList = SmsInv.validateInventory( %client, %list );
   %client.loadout[0] = getField( %validList, 1 );
   %armor = $NameToData[%client.loadout[0]];
   %weaponCount = 0;
   %specialCount = 0;
   %grenadeCount = 0;
   %count = 1;
   %client.weaponIndex = "";
   %client.specialIndex = "";
   %client.grenadeIndex = "";

   for ( %i = 1; %i < getFieldCount( %validList ); %i++ )
   {
      %setItem = false;
      switch$ ( getField(%validList, %i-1 ) )
      {
         case Weapon:
            if ( %weaponCount < %armor.maxWeapons )
            {
               if ( !%weaponCount )
                  %client.weaponIndex = %count;
               else
                  %client.weaponIndex = %client.weaponIndex TAB %count;

               %weaponCount++;
               %setItem = true;   
            } 

         case Special:
            if ( %specialCount < %armor.maxSpecials )
            {
               if ( !%specialCount )
                  %client.specialIndex = %count;
               else
                  %client.specialIndex = %client.specialIndex TAB %count;

               %specialCount++;
               %setItem = true;
            }

         case Grenade:
            if ( %grenadeCount < %armor.maxGrenades )
            {
               if ( !%grenadeCount )
                  %client.grenadeIndex = %count;
               else
                  %client.grenadeIndex = %client.grenadeIndex TAB %count;

               %grenadeCount++;
               %setItem = true;
            }
      }
      if ( %setItem )
      {
         %client.loadout[%count] = getField( %validList, %i );
         %count++;
      }
   }
   %client.numFavs = %count;
   %client.numFavsCount = 0;
   SmsInv.updateArmoryHud( %client );
}

function SMS::validateInventory(%this, %client, %text)
{
   //LogEcho("SMS::validateInventory(" SPC %this @", "@ %client.nameBase @", "@ %text SPC ")");
   %cmt = $Server::MissionType;

   //%armorList = %client.gender SPC %client.race;
   //%armor = $NameToData[%armorList];

   %list = getField( %text, 0 );
   %armorName = getField( %text, 1 );
   %list = %list TAB %armorName;
   %armor = $NameToData[%armorName];

   %weaponIndex = 0;
   //for ( %i = 1; %i < getFieldCount( %text ); %i++ )
   for ( %i = 3; %i < getFieldCount( %text ); %i = %i + 2 )
   {
      %inv = $NameToData[getField(%text, %i)];

      if((( %armor.maxInv[%inv] && !($GameBanList[%cmt, %inv])) || getField( %text, %i ) $= "Empty" || getField( %text, %i ) $= "Invalid") 
         && (($InvTotalCount[getField( %text, %i - 1 )] - $BanCount[getField( %text, %i - 1 )]) > 0))
      {
         %list = %list TAB getField( %text, %i - 1 ) TAB getField( %text, %i );
      }
      else if( $GameBanList[%cmt, %inv] || %inv $= "Empty" || %inv $= "")
      {
         %list = %list TAB getField( %text, %i - 1 ) TAB "Invalid";
      } 
   }
   //echo("validlist:" SPC %list);
   return( %list );
}

function SMS::SetDefaultInventory(%this, %client)
{
   // Initial loading of last selected loadout
   if ( !%client.isAiControlled() )
      commandToClient( %client, 'InitLoadFavorite' );
}

function SMS::CreateInvBanCount(%this)
{
   //LogEcho("SMS::CreateInvBanCount(" SPC $Server::MissionType SPC ")");
   $BanCount["Weapon"] = 0;
   $BanCount["Special"] = 0;
   $BanCount["Grenade"] = 0;
   $BanCount["Mine"] = 0;

   for(%i = 0; %i < $SMS::MaxWeapons; %i++)
   {
      if($GameBanList[$Server::MissionType, $NameToData[$SMS::WeaponName[%i]]])
         $BanCount["Weapon"]++;
   }
   $InvTotalCount["Weapon"] = %i;

   for(%i = 0; %i < $SMS::MaxItems; %i++)
   {
      if($GameBanList[$Server::MissionType, $NameToData[$SMS::ItemName[%i]]])
         $BanCount["Special"]++;
   }
   $InvTotalCount["Special"] = %i;

   for(%i = 0; %i < $SMS::MaxGrenades; %i++)
   {
      if($GameBanList[$Server::MissionType, $NameToData[$SMS::GrenadeName[%i]]])
         $BanCount["Grenade"]++;
   }
   $InvTotalCount["Grenade"] = %i;

   for(%i = 0; %i < $SMS::MaxMines; %i++)
   {
      if($GameBanList[$Server::MissionType, $NameToData[$SMS::MineName[%i]]])
         $BanCount["Mine"]++;
   }
   $InvTotalCount["Mine"] = %i;
}

function SMS::ProcessLoadout(%this, %client)
{
   //LogEcho("SMS::ProcessLoadout(" SPC %this @", "@ %client.nameBase SPC ")");
   %player = %client.player;
   if ( !isObject( %player ) )
      return;

   %player.clearInventory();

   %weapCount = 0;
   // Now give them the stuff thats not listed in the armory (flagged).
   // These are default weapons. Inventory restrictions will keep it clean
   for ( %i = 0; %i < $SMS::MaxWeapons; %i++ )
   {
      %default = $SMS::Weapon[%i];

      // Filter out what isnt flagged..
      if ( $SMS::ShowInInv[%default] == 0 )
      {
         %player.incInventory( %default, 1 );

         if ( %default.image.clip !$= "" )
            %player.incInventory( %default.image.clip, %player.maxInventory( %default.image.clip ) );

         if ( %default.image.ammo !$= "" )
            %player.incInventory( %default.image.ammo, %player.maxInventory( %default.image.ammo ) );
      }
      // We dont want these counting against maxWeapons or we will hit the ceiling instantly.
   }

   for ( %i = 0; %i < getFieldCount( %client.weaponIndex ); %i++ )
   {
      %WInv = $NameToData[%client.loadout[getField( %client.weaponIndex, %i )]];
      if(%WInv !$= "")
      {
         // increment weapon count if current armor can hold this weapon
         if ( %player.incInventory( %WInv, 1 ) > 0 )
         {
            %weapCount++;

            if ( %WInv.image.clip !$= "" )
               %player.incInventory( %WInv.image.clip, %player.maxInventory( %WInv.image.clip ) );

            if ( %WInv.image.ammo !$= "" )
               %player.incInventory( %WInv.image.ammo, %player.maxInventory( %WInv.image.ammo ) );
         }
      }

      if( %weapCount >= %player.getDatablock().maxWeapons )
         break;
   }

   //echo("Weapon Count:" SPC %weapCount);
   %player.weaponCount = %weapCount;

   //-----------------------------------------------------------------------------
   // Specials
   %specCount = 0;
   for ( %i = 0; %i < getFieldCount( %client.specialIndex ); %i++ )
   {
      %SInv = $NameToData[%client.loadout[getField( %client.specialIndex, %i )]];
      if(%SInv !$= "")
      {
         //warn("Trying to give player" SPC %SInv SPC "Special");
         // increment special count if current armor can hold this special
         if ( %player.incInventory( %SInv, %player.maxInventory(%SInv) ) > 0 )
            %specCount++;
         else
            warn("Player cannot have" SPC %SInv SPC "Special");
      }

      if( %specCount >= %player.getDatablock().maxSpecials )
         break;
   }

   // Make sure the hud is empty
   if ( %specCount <= 0 )
      messageClient(%player.client, 'MsgSpecialCnt', "", 'Special', '0' );

   %player.specialCount = %specCount;

   //-----------------------------------------------------------------------------
   // Grenades
   %grenCount = 0;
   for ( %i = 0; %i < getFieldCount( %client.grenadeIndex ); %i++ )
   {
      %GInv = $NameToData[%client.loadout[getField( %client.grenadeIndex, %i )]];
      if(%GInv !$= "")
      {
         // increment grenade count if current armor can hold this grenade
         if ( %player.incInventory( %GInv, %player.maxInventory(%GInv) ) > 0 )
         {
            //%player.lastGrenade = %GInv;
            %player.incInventory( %GInv.image.ammo, %player.maxInventory( %GInv.image.ammo ) );
            %grenCount++;
         }
      }

      if( %grenCount >= %player.getDatablock().maxGrenades )
         break;
   }

   // Make sure the hud is empty
   if ( %grenCount <= 0 )
      messageClient(%player.client, 'MsgGrenadeCnt', "", 'Grenade', 0 );

   %player.grenadeCount = %grenCount;
}

function SMS::ReplenishLoadoutAmmo(%this, %player)
{
   // Get the weapons the player has
   for(%i = 0; %i < %player.weaponSlotCount; %i++)
   {
      %weapon = %player.weaponSlot[%i];
      // See if the weapon needs mags
      if ( %weapon.image.clip !$= "" )
         %player.incInventory( %weapon.image.clip, %player.maxInventory( %weapon.image.clip ) );

      // See if the weapon needs ammo
      if ( %weapon.image.ammo !$= "" )
         %player.incInventory( %weapon.image.ammo, %player.maxInventory( %weapon.image.ammo ) );
   }

   // Grenades
   %grenade = (%player.getMountedImage($GrenadeSlot) == 0 ) ? "" : %player.getMountedImage($GrenadeSlot);
   if ( %grenade.ammo !$= "" )
      %player.incInventory( %grenade.ammo, %player.maxInventory( %grenade.ammo ) );

   //if ( %grenade !$= "" )
   //   %player.incInventory( %grenade, %player.getDataBlock().maxInv[%grenade] );
}

function listInventoryArrays()
{
   //Weapons
   for( %i = 0; %i < $SMS::MaxWeapons; %i++ )
      echo($SMS::Weapon[%i]);

   // Ammo
   for( %i = 0; %i < $SMS::MaxAmmos; %i++ )
      echo($SMS::AmmoName[%i]);

   // Ammo Clips
   for( %i = 0; %i < $SMS::MaxClips; %i++ )
      echo($SMS::Clip[%i]);

   // Specials
   for( %i = 0; %i < $SMS::MaxItems; %i++ )
      echo($SMS::ItemName[%i]);

   // Grenades
   for( %i = 0; %i < $SMS::MaxGrenades; %i++ )
      echo($SMS::GrenadeName[%i]);

   // Mines
   for( %i = 0; %i < $SMS::MaxMines; %i++ )
      echo($SMS::MineName[%i]);
}

