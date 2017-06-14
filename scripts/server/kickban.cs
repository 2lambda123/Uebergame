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

function kick( %client, %admin )
{
   messageAll( 'MsgAdminForce', '\c2The Admin has kicked %1.', %client.playerName);

   if (!%client.isAIControlled())
      BanList::add(%client.guid, %client.getAddress(), $pref::Server::KickBanTime);

   %client.delete( $AdminCl.nameBase @ " has kicked you from this server." );
}

function ban( %client, %admin )
{
   if ( %admin )
      messageAll('MsgAdminForce', '\c2%1 has banned %2.', $AdminCl.playerName, %client.playerName);
   else
      messageAll( 'MsgVotePassed', '\c2%1 was banned by vote.', %client.playerName );

   if (!%client.isAIControlled())
      BanList::add(%client.guid, %client.getAddress(), $pref::Server::BanTime);

   %client.addToServerBanList();
   %client.delete( $AdminCl.nameBase @ " has banned you from this server." );
}

function GameConnection::addToServerBanList( %client, %guid )
{
   if ( $Banned::GuidCount $= "" )
      $Banned::GuidCount = 0;

   // Lets make sure this id is not allready in the array
   %found = false;
   for ( %i = 0; %i < $Banned::GuidCount; %i++ )
   {
      if ( $Banned::Guid[%i] $= %guid )
      {
         %found = true;
         break;
      }
   }

   // Didn't find it, add to the next free slot.
   if ( !%found )
      $Banned::Guid[$Banned::GuidCount++] = %client.guid;
}

function GameConnection::removeFromServerBanList( %client, %guid )
{
   if ( $Banned::GuidCount $= "" )
      $Banned::GuidCount = 0;

   %count = 0;
   for ( %i = 0; %i < $Banned::GuidCount; %i++ )
   {
      if ( $Banned::Guid[%i] !$= %guid )
         %Temp[%count++] = %guid;
   }

   for( %j = 0; %j < %count; %j++ )
      $Banned::Guid[%j] = %Temp[%j];

   $Banned::GuidCount = %count;
}

function GameConnection::isOnServerBanList( %client, %guid )
{
   if ( $Banned::GuidCount $= "" )
      $Banned::GuidCount = 0;

   %found = false;
   for ( %i = 0; %i < $Banned::GuidCount; %i++ )
   {
      if ( $Banned::Guid[%i] $= %guid )
      {
         %found = true;
         break;
      }
   }

   return( %found );
}

function exportBanList()
{
   echo("Exporting server ip banlist");
   export("$Banned::*", $HomePath @ "/banlist.cs", false);
}
