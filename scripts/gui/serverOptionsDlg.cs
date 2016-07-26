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

function ServerOptionsDlg::onWake(%this)
{
   SOP_ServerName.setValue( $pref::Server::Name );
   SOP_ServerInfo.setValue( $pref::Server::Info );
   SOP_MaxPlayers.setValue( $pref::Server::MaxPlayers );
   SOP_ServerPass.setValue( $pref::Server::Password );
   SOP_SuperPass.setValue( $pref::Server::SuperAdminPassword );
   SOP_AdminPass.setValue( $pref::Server::AdminPassword );
   SOP_KickTime.setValue( $pref::Server::KickBanTime );
   SOP_BanTime.setValue( $pref::Server::BanTime );
   SOP_TimeLimit.setValue( $pref::Server::TimeLimit );
   SOP_WarmupTime.setValue( $pref::Server::warmupTime );
   SOP_MaxWeapons.setValue( $pref::Server::MaxWeapons );

   // Setup the AI control
   if($pref::Server::AiCount > $pref::Server::MaxPlayers - 1)
      $pref::Server::AiCount = $pref::Server::MaxPlayers - 1;

   if ( $pref::Server::AiCount <= 0 )
      %sliderValue = 0;
   else
      %sliderValue = $pref::Server::AiCount;

   SOP_AiCountSlider.setValue(%sliderValue);
   SOP_AiCountText.setValue( "AI Count ("@%sliderValue@")" );
}

function ServerOptionsDlg::onSleep(%this)
{
   $pref::Server::Name = SOP_ServerName.getValue();
   $pref::Server::Info = SOP_ServerInfo.getValue();

   if ( checkIsNumber( SOP_MaxPlayers.getValue() ) )
      $pref::Server::MaxPlayers = SOP_MaxPlayers.getValue();

   $pref::Server::Password = SOP_ServerPass.getValue();
   $pref::Server::SuperAdminPassword = SOP_SuperPass.getValue();
   $pref::Server::AdminPassword = SOP_AdminPass.getValue();

   if ( checkIsNumber( SOP_KickTime.getValue() ) )
      $pref::Server::KickBanTime = SOP_KickTime.getValue();

   if ( checkIsNumber( SOP_BanTime.getValue() ) )
      $pref::Server::BanTime = SOP_BanTime.getValue();

   if ( checkIsNumber( SOP_TimeLimit.getValue() ) )
      $pref::Server::TimeLimit = SOP_TimeLimit.getValue();

   if ( checkIsNumber( SOP_WarmupTime.getValue() ) )
      $pref::Server::warmupTime = SOP_WarmupTime.getValue();
  
   if ( checkIsNumber( SOP_MaxWeapons.getValue() ) )
      $pref::Server::MaxWeapons = SOP_MaxWeapons.getValue();
}

function checkIsNumber(%string)
{
   for( %i = 0; ( %char = getSubStr( %string, %i, 1 ) ) !$= ""; %i++ )
   {
      switch$( %char )
      {
         case "0" or "1" or "2" or "3" or "4" or "5" or "6" or "7" or "8" or "9":
            continue;

         default:
            return false;
        }
    }
    return true;
}

function setBotCount()
{
   %value = mFloor( SOP_AiCountSlider.getValue() );
   if ( %value > $pref::Server::MaxPlayers - 1 )
      %value = $pref::Server::MaxPlayers - 1;

   $pref::Server::AICount = %value;
   SOP_AiCountText.setValue( "AI Count ("@%value@")" );
   SOP_AiCountSlider.setValue(%value);
}