//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Loading info is text displayed on the client side while the mission
// is being loaded.  This information is extracted from the mission file
// and sent to each the client as it joins.
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// clearLoadInfo
//
// Clears the mission info stored
//------------------------------------------------------------------------------
function clearLoadInfo()
{
   for( %line = 0; %line < $HostGameRulesCount; %line++ )
      $HostGameRules[%line] = "";

   $HostGameRulesCount = 0;
}

//------------------------------------------------------------------------------
// buildLoadInfo
//
// Extract the map description from the .mis file
//------------------------------------------------------------------------------

function buildLoadInfo(%mission, %missionType)
{
   clearLoadInfo();
   $MissionDisplayName = fileBase(%mission);
   $MissionTypeDisplayName = %missionType;

   for( %i = 0; %i < $HostTypeCount; %i++ )
   {
      if( $HostTypeName[%i] $= %missionType )
         break;
   }

   $MissionDisplayName = $MissionDisplayName[%mission];
   $MissionTypeDisplayName = $HostTypeDisplayName[%i];
   for(%line = 0; $HostGameRules[%missionType, %line] !$= ""; %line++)
   {
      $HostGameRules[$HostGameRulesCount] = $HostGameRules[%missionType, %line];
      $HostGameRulesCount++;
   }
}

//------------------------------------------------------------------------------
// sendLoadInfoToClient
//
// Sends mission description to the client
//------------------------------------------------------------------------------
function sendLoadInfoToClient(%client)
{
   //echo("sendLoadInfoToClient(" SPC %client.nameBase SPC ")");
   messageClient( %client, 'MsgLoadInfo', "", $Server::MissionFile, $MissionDisplayName, 
                                             $MissionTypeDisplayName, $Server::MissionType );

   for( %line = 0; $HostMissionDesc[$Server::MissionFile, %line] !$= ""; %line++ )
   {
      messageClient(%client, 'MsgLoadDescLine', "", $HostMissionDesc[$Server::MissionFile, %line]);
   }

   for(%line = 0; %line < $HostGameRulesCount; %line++ )
   {
      if( $HostGameRules[%line] !$= "" )
         messageClient( %client, 'MsgLoadRulesLine', "", $HostGameRules[%line] );
   }

   for(%line = 0; $pref::Server::Message[%line] !$= ""; %line++ )
   {
      messageClient( %client, 'MsgLoadServerInfoLine', "", $pref::Server::Message[%line] );
   }

   messageClient( %client, 'MsgLoadInfoDone' );
}
