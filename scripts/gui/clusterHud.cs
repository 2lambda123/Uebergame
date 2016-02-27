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

function ClusterHud::onWake(%this)
{
   //%this.updateValues();
}

function ClusterHud::onSleep(%this)
{
   //if( isEventPending( %this.valueCheck ) )
   //{
   //   cancel( %this.valueCheck );
   //   %this.valueCheck = "";
   //}
}

function ClusterHud::updateValues(%this)
{
   if( isEventPending( %this.valueCheck ) )
      cancel( %this.valueCheck );

   %this.updateSpeed();
   //%this.updateAlt();
   %this.valueCheck = %this.schedule( 250, "updateValues" );
}

function ClusterHud::updateSpeed(%this)
{
   %vel = getControlObjectSpeed();

   //The claim is that the torque unit = 1 meter
   //1 kilometer = 3280 feet

   //but 1 torque unit does not equal 1 meter, it is approx 0.691 meters!

   //so for KPH conversion it should be:
   // TUperS * 0.691896 = meters per second (mps)
   // mps * 3.6 = KPH

   //1 torque unit = 2.27 feet
   //TUperS * 2.27 = feet per second (fps)
   // (fps * 3600) / 5280 = MPH

   //%cVel = mFloor(%vel * 2.491); // m/s * 0.691896 * 3600 / 1000 = 2.4908256 km/h
   //%cVel = mFloor(%vel * 1.5476); //2.27 * 3600) / 5280 = 1.5476 MPH
   //%cVel = mFloor( VectorLen( $MyClient.player.getVelocity() ) ); // Velocity
   //%cVel = mFloor(%vel * 3.6); // m/s * (3600/1000) = km/h - This is wrong but people are used to it.

   %cVel = mFloor(%vel * 2.491); // m/s * 0.691896 * 3600 / 1000 = 2.4908256 km/h
   vSpeedText.setValue( %cVel );
}

function ClusterHud::updateAlt(%this)
{
   %alt = getControlObjectAltitude();
   vAltText.setValue( %alt );
}
