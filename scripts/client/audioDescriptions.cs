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

// Always declare SFXDescription's (the type of sound) before SFXProfile's (the
// sound itself) when creating new ones

//-----------------------------------------------------------------------------
// Single shot sounds
//-----------------------------------------------------------------------------

singleton SFXDescription( AudioDefault3D : AudioEffect )
{
   is3D              = true;
   ReferenceDistance = 5.0;
   MaxDistance       = 100.0;
};

singleton SFXDescription( AudioClose3D : AudioEffect )
{
   is3D              = true;
   ReferenceDistance = 2.0;
   MaxDistance       = 50.0;
   priority = "1";
};

singleton SFXDescription( AudioClosest3D : AudioEffect )
{
   is3D              = true;
   ReferenceDistance = 2.0;
   MaxDistance       = 20.0;
   priority = "1";
};

singleton SFXDescription( AudioBulletFire : AudioEffect )
{
   is3D              = true;
   ReferenceDistance = 2.0;
   MaxDistance       = 70.0;
   priority = "1";
   volume = 0.8;
};

singleton SFXDescription(AudioBulletImpact : AudioEffect )
{
   is3D              = true;
   ReferenceDistance = 1.0;
   MaxDistance       = 25.0;
   priority = "2";
};

singleton SFXDescription(AudioGrenadeImpact : AudioEffect )
{
   is3D              = true;
   ReferenceDistance = 10.0;
   MaxDistance       = 70.0;
   volume = 0.8;
};

//-----------------------------------------------------------------------------
// Looping sounds
//-----------------------------------------------------------------------------

singleton SFXDescription( AudioDefaultLoop3D : AudioEffect )
{
   isLooping         = true;
   is3D              = true;
   ReferenceDistance = 20.0;
   MaxDistance       = 100.0;
};

singleton SFXDescription( AudioCloseLoop3D : AudioEffect )
{
   isLooping         = true;
   is3D              = true;
   ReferenceDistance = 5.0;
   MaxDistance       = 50.0;
};

singleton SFXDescription( AudioClosestLoop3D : AudioEffect )
{
   isLooping         = true;
   is3D              = true;
   ReferenceDistance = 5.0;
   MaxDistance       = 20.0;
   priority = "1";
};

//-----------------------------------------------------------------------------
// 2d sounds
//-----------------------------------------------------------------------------

// Used for non-looping environmental sounds (like power on, power off)
singleton SFXDescription( Audio2D : AudioEffect )
{
   isLooping         = false;
};

// Used for Looping Environmental Sounds
singleton SFXDescription( AudioLoop2D : AudioEffect )
{
   isLooping         = true;
};

singleton SFXDescription( AudioStream2D : AudioEffect )
{
   isStreaming       = true;
};

singleton SFXDescription( AudioStreamLoop2D : AudioEffect )
{
   isLooping         = true;
   isStreaming       = true;
};

//-----------------------------------------------------------------------------
// Gui sounds
//-----------------------------------------------------------------------------

// Used for non-looping Gui sounds
singleton SFXDescription( AudioGui2D : AudioGui )
{
   isLooping         = false;
};

// Used for Looping Gui sounds
singleton SFXDescription( AudioLoopGui2D : AudioGui )
{
   isLooping         = true;
};

singleton SFXDescription( AudioStreamGui2D : AudioGui )
{
   isStreaming       = true;
};

singleton SFXDescription( AudioStreamLoopGui2D : AudioGui )
{
   isLooping         = true;
   isStreaming       = true;
};

//-----------------------------------------------------------------------------
// Music
//-----------------------------------------------------------------------------

singleton SFXDescription( AudioMusic2D : AudioMusic )
{
   isStreaming       = true;
};

singleton SFXDescription( AudioMusicLoop2D : AudioMusic )
{
   isLooping         = true;
   isStreaming       = true;
};

singleton SFXDescription( AudioMusic3D : AudioMusic )
{
   isStreaming       = true;
   is3D              = true;
};

singleton SFXDescription( AudioMusicLoop3D : AudioMusic )
{
   isStreaming       = true;
   is3D              = true;
   isLooping         = true;
};
