//------------------------------------------------------------------------------------------
// SMS: Standard Modularzation System
//------------------------------------------------------------------------------------------
//
// Originally Started by Myself, Yardik, Zod and BH (Brian Helmkamp).
// At the begining Yardik did most of this work, with just a smidge of help from BH.
// It never worked well, and T2's constant early patches made it worse. Finally it was
// abandoned By Yardik, Zod and BH. (BH seemed to quit modding) However, I
// never gave up on it, and continued to work the code until it operated flawlessly.
// By GOD this is Mod support!!!
//		-Founder
//
//
// Worked so well after Founder got it up. Of course that got me interested again :x
// So I went ahead and exploded it into an script object tabbed list using beast.
//          - ZOD
//------------------------------------------------------------------------------------------

// This is our inventory object, we use this so mod packages can be
// deactivated without loss of parent functions.
if( !isObject( SmsInv ) )
{
   new ScriptObject(SmsInv)
   {
      class = SMS;
      Version = '7.10.02';
   };
   ScriptClassGroup.add(SmsInv);
}
//-----------------------------------------------------------------------------
//Initialisers.. keep em at 0
//-----------------------------
$SMS::ArmorCounter = 0;
$SMS::MaxArmors = 0;
$SMS::MaxWeapons = 0;
$SMS::MaxAmmos = 0;
$SMS::MaxClips = 0;
$SMS::MaxItems = 0;
$SMS::MaxGrenades = 0;
$SMS::MaxMines = 0;
$SMS::MaxVehicles = 0;
$SMS::MaxClips = 0;

//-----------------------------------------------------------------------------
// SMS allow functions set up arrays for armor max's
//-----------------------------------------------------

function SMS::AllowWeapon( %this, %list )
{
   for(%i = 0; %i < getFieldCount(%list); %i++)
   {
      %armor = getField(%list, %i);
      $SMS::ArmorAllowedWeapon[%armor, $SMS::MaxWeapons] = 1;
      //echo("SMS::AllowWeapon(" SPC %armor SPC $SMS::MaxWeapons SPC ")");
   }
}

function SMS::AllowAmmo( %this, %list )
{
   for(%i = 1; %i < getFieldCount(%list); %i++)
   {
      if(getField( %list, %i - 1 ) $= armor)
      {
         %armor = getField(%list, %i);
         %amount = getField(%list, %i+1);

         $SMS::ArmorAllowedAmmo[%armor, $SMS::MaxAmmos] = %amount;
         //echo("SMS::AllowAmmo(" SPC %armor @", "@ %amount @", "@ $SMS::MaxAmmos SPC ")");
      }
   }
}

function SMS::AllowClip( %this, %list )
{
   for(%i = 1; %i < getFieldCount(%list); %i++)
   {
      if(getField( %list, %i - 1 ) $= armor)
      {
         %armor = getField(%list, %i);
         %amount = getField(%list, %i+1);

         $SMS::ArmorAllowedClip[%armor, $SMS::MaxClips] = %amount;
         //echo("SMS::AllowClip(" SPC %armor @", "@ %amount @", "@ $SMS::MaxClips SPC ")");
      }
   }
}

function SMS::AllowItem( %this, %list )
{
   for(%i = 1; %i < getFieldCount(%list); %i++)
   {
      if(getField( %list, %i - 1 ) $= armor)
      {
         %armor = getField(%list, %i);
         %amount = getField(%list, %i+1);

         $SMS::ArmorAllowedItem[%armor, $SMS::MaxItems] = %amount;
         //echo("SMS::AllowItem(" SPC %armor @", "@ %amount @", "@ $SMS::MaxItems SPC ")");
      }
   }
}

// Grenades are treated as a weapon having ammo or using energy which is why we do this instead of below
function SMS::AllowGrenade( %this, %list )
{
   for(%i = 0; %i < getFieldCount(%list); %i++)
   {
      %armor = getField(%list, %i);
      $SMS::ArmorAllowedGrenade[%armor, $SMS::MaxGrenades] = 1;
      //echo("SMS::AllowGrenade(" SPC %armor SPC $SMS::MaxGrenades SPC ")");
   }
}

//function SMS::AllowGrenade( %this, %list )
//{
//   for(%i = 1; %i < getFieldCount(%list); %i++)
//   {
//      if(getField( %list, %i - 1 ) $= armor)
//      {
//         %armor = getField(%list, %i);
//         %amount = getField(%list, %i+1);

//         $SMS::ArmorAllowedGrenade[%armor, $SMS::MaxGrenades] = %amount;
         //echo("SMS::AllowGrenade(" SPC %armor @", "@ %amount @", "@ $SMS::MaxGrenades SPC ")");
//      }
//   }
//}

function SMS::AllowMine( %this, %list )
{
   for(%i = 1; %i < getFieldCount(%list); %i++)
   {
      if(getField( %list, %i - 1 ) $= armor)
      {
         %armor = getField(%list, %i);
         %amount = getField(%list, %i+1);

         $SMS::ArmorAllowedMine[%armor, $SMS::MaxMines] = %amount;
         //echo("SMS::AllowMine(" SPC %armor @", "@ %amount @", "@ $SMS::MaxMines SPC ")");
      }
   }
}

//-----------------------------------------------------------------------------
// SMS add functions for setting items in Global arrays
//-----------------------------------------------------

function SMS::AddArmor(%this, %armor, %invName, %index) 
{
   $SMS::Armor[$SMS::MaxArmors] = %armor;
   $SMS::ArmorName[$SMS::MaxArmors] = %invName;

   $NameToData[%invName] = %armor;
   $DataToName[%armor] = %invName;
   $SMS::MaxArmors++;
   //echo("SMS::AddArmor(" SPC %armor @", "@ %invName @", "@ %index @", "@ $SMS::MaxArmors SPC ")");
}

function SMS::AddWeapon( %this, %weapon, %invName, %invShow )
{
   $SMS::ShowInInv[%weapon] = %invShow;
   $SMS::Weapon[$SMS::MaxWeapons] = %weapon;
   $SMS::WeaponName[$SMS::MaxWeapons] = %invName;

   $NameToData[%invName] = %weapon;
   $DataToName[%weapon] = %invName;

   $SMS::MaxWeapons++;
   //echo("SMS::AddWeapon(" SPC %weapon @", "@ %invName @", "@ %hudName @", "@ $SMS::MaxWeapons SPC ")");
}

function SMS::AddAmmo( %this, %ammoName, %increment )
{
   $AmmoIncrement[%ammoName] = %increment;
   $SMS::AmmoName[$SMS::MaxAmmos] = %ammoName;
   $SMS::MaxAmmos++;
   //echo("SMS::AddAmmo(" SPC %ammoName @", "@ %increment @", "@ $SMS::MaxAmmos SPC ")");
}

function SMS::AddClip( %this, %clip, %invName, %increment )
{
   $AmmoIncrement[%clipName] = %increment;
   $SMS::Clip[$SMS::MaxClips] = %clip;
   $SMS::ClipName[$SMS::MaxClips] = %invName;

   $NameToData[%invName] = %clip;
   $DataToName[%clip] = %invName;

   $SMS::MaxClips++;
   //echo("SMS::AddClip(" SPC %clip @", "@ %invName @", "@ %increment @", "@ $SMS::MaxClips SPC ")");
}

function SMS::AddItem( %this, %item, %invName, %increment )
{
   $AmmoIncrement[%item] = %increment;
   $SMS::Item[$SMS::MaxItems] = %item;
   $SMS::ItemName[$SMS::MaxItems] = %invName;

   $NameToData[%invName] = %item;
   $DataToName[%item] = %invName;

   $SMS::MaxItems++;
   //echo("SMS::AddItem(" SPC %item @", "@ %invName @", "@ $SMS::MaxItems SPC ")");
}

function SMS::AddGrenade( %this, %grenade, %invName )
{
   $SMS::Grenade[$SMS::MaxGrenades] = %grenade;
   $SMS::GrenadeName[$SMS::MaxGrenades] = %invName;

   $NameToData[%invName] = %grenade;
   $DataToName[%grenade] = %invName;

   $SMS::MaxGrenades++;
   //echo("SMS::AddGrenade(" SPC %grenade @", "@ %invName @", "@ $SMS::MaxGrenades SPC ")");
}

function SMS::AddMine( %this, %mine, %invName, %increment )
{
   $AmmoIncrement[%mine] = %increment;
   $SMS::Mine[$SMS::MaxMines] = %mine;
   $SMS::MineName[$SMS::MaxMines] = %invName;

   $NameToData[%invName] = %mine;
   $DataToName[%mine] = %invName;

   $SMS::MaxMines++;
   //echo("SMS::AddMine(" SPC %mine @", "@ %invName @", "@ $SMS::MaxMines SPC ")");
}

function SMS::AddVehicle( %this, %vehicle, %invName, %limit )
{
   $InvVehicle[$SMS::MaxVehicles] = %vehicle;
   $VehicleToName[%vehicle] = %invName;
   $NameToVehicle[%invName] = %vehicle;
   //$VehicleMax[%vehicle] = %limit;
   $SMS::MaxVehicles++;
   //echo("SMS::AddVehicle(" SPC %vehicle @", "@ %invName @", "@ $SMS::MaxVehicles SPC ")");
}

//-----------------------------------------------------------------------------
// SMS setup functions for setting armor maxs for items
//-----------------------------------------------------

function SMS::SetupMaxWeapons( %this )
{
   for ( %i = 0; %i < $SMS::MaxWeapons; %i++ ) 
   {
      for ( %j = 0; %j < $SMS::MaxArmors; %j++ )
      {
         %thisArmor = $SMS::Armor[%j];
         %thisArmorName = $SMS::ArmorName[%j];
         %thisArmor.maxInv[$SMS::Weapon[%i]] = $SMS::ArmorAllowedWeapon[%thisArmorName, %i];
         //echo("SMS::SetupMaxWeapons(" SPC %thisArmor @", "@ %thisArmorName @", "@ %thisArmor.maxInv[$SMS::WeaponName[%i]] SPC ")");
      }
   }
}

function SMS::SetupMaxAmmos( %this )
{
   for ( %i = 0; %i < $SMS::MaxAmmos; %i++ )
   {
      for ( %j = 0; %j < $SMS::MaxArmors; %j++ )
      {
         %thisArmor = $SMS::Armor[%j];
         %thisArmorName = $SMS::ArmorName[%j];
         %thisArmor.maxInv[$SMS::AmmoName[%i]] = $SMS::ArmorAllowedAmmo[%thisArmorName, %i];
         //echo("SMS::SetupMaxAmmos(" SPC %thisArmor @", "@ %thisArmorName @", "@ %thisArmor.maxInv[$SMS::AmmoName[%i]] SPC ")");
      }
   }
}

function SMS::SetupMaxClips( %this )
{
   for ( %i = 0; %i < $SMS::MaxClips; %i++ )
   {
      for ( %j = 0; %j < $SMS::MaxArmors; %j++ )
      {
         %thisArmor = $SMS::Armor[%j];
         %thisArmorName = $SMS::ArmorName[%j];
         %thisArmor.maxInv[$SMS::Clip[%i]] = $SMS::ArmorAllowedClip[%thisArmorName, %i];
         //echo("SMS::SetupMaxClips(" SPC %thisArmor @", "@ %thisArmorName @", "@ %thisArmor.maxInv[$SMS::Clip[%i]] SPC ")");
      }
   }
}

function SMS::SetupMaxItems( %this )
{
   for ( %i = 0; %i < $SMS::MaxItems; %i++ )
   {
      for ( %j = 0; %j < $SMS::MaxArmors; %j++ )
      {
         %thisArmor = $SMS::Armor[%j];
         %thisArmorName = $SMS::ArmorName[%j];
         %thisArmor.maxInv[$SMS::Item[%i]] = $SMS::ArmorAllowedItem[%thisArmorName, %i];
         //echo("SMS::SetupMaxItems(" SPC %thisArmor @", "@ %thisArmorName @", "@ %thisArmor.maxInv[$SMS::ItemName[%i]] SPC ")");
      }
   }
}

function SMS::SetupMaxGrenades( %this )
{
   for ( %i = 0; %i < $SMS::MaxGrenades; %i++ )
   {
      for ( %j = 0; %j < $SMS::MaxArmors; %j++ )
      {
         %thisArmor = $SMS::Armor[%j];
         %thisArmorName = $SMS::ArmorName[%j];
         %thisArmor.maxInv[$SMS::Grenade[%i]] = $SMS::ArmorAllowedGrenade[%thisArmorName, %i];
         //echo("SMS::SetupMaxGrenades(" SPC %thisArmor @", "@ %thisArmorName @", "@ %thisArmor.maxInv[$SMS::GrenadeName[%i]] SPC ")");
      }
   }
}

function SMS::SetupMaxMines( %this )
{
   for ( %i = 0; %i < $SMS::MaxMines; %i++ )
   {
      for ( %j = 0; %j < $SMS::MaxArmors; %j++ )
      {
         %thisArmor = $SMS::Armor[%j];
         %thisArmorName = $SMS::ArmorName[%j];
         %thisArmor.maxInv[$SMS::Mine[%i]] = $SMS::ArmorAllowedMine[%thisArmorName, %i];
         //echo("SMS::SetupMaxMines(" SPC %thisArmor @", "@ %thisArmorName @", "@ %thisArmor.maxInv[$SMS::MineName[%i]] SPC ")");
      }
   }
}
