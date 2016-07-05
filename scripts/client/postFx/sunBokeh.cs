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

$SunBokehPostFX::sunAmount = 0.35;  //Controls the amount of bokeh around the sun.
$SunBokehPostFX::edgeAmount = 0.4;  //Controls the amount of bokeh around the edge of the screen.
$SunBokehPostFX::haloAmount = 0.7;  //Controls the amount of the glare/halo.
$SunBokehPostFX::sunSize = 0.125;  //Controls the size of the sun mask.
$SunBokehPostFX::bokehSize = 550; //Controls how much the bokeh texture tiles (eg. bokeh size).
$SunBokehPostFX::debug = 0; //Set this to 1 if you'd like to see only the effect mask. Useful for tweaking.

singleton GFXStateBlockData( PFX_DefaultSunBokehStateBlock )  
{  
   zDefined = true;  
   zEnable = false;  
   zWriteEnable = false;     
   samplersDefined = true;  
   samplerStates[0] = SamplerClampPoint;  
};  
      
singleton ShaderData( PFX_SunBokehShader )  
{     
   DXVertexShaderFile  = "shaders/common/postFx/postFxV.hlsl";  
   DXPixelShaderFile   = "shaders/common/postFx/sunBokeh.hlsl";  

   OGLVertexShaderFile  = "shaders/common/postFx/gl/postFxV.glsl";  
   OGLPixelShaderFile   = "shaders/common/postFx/sunBokeh.glsl";  
          
   samplerNames[1] = "$bokehTex";  
   samplerNames[2] = "$dirtTex";  
          
   pixVersion = 3.0;  
};  
      
singleton PostEffect( SunBokehPostFx )  
{  
   renderTime = "PFXAfterDiffuse";  
   renderPriority = 0.3;  
   isEnabled = true;  
   allowReflectPass = false;  
      
   shader = PFX_SunBokehShader;  
   stateBlock = PFX_DefaultSunBokehStateBlock;  
   texture[0] = "$backBuffer";  
   texture[1] = "textures/screenBokeh";  
   texture[2] = "textures/screenDirt";  
   target = "$backBuffer";  
};  
      
function SunBokehPostFx::setShaderConsts( %this )  
{     
   %this.setShaderConst("$sunAmount", $SunBokehPostFX::sunAmount);  
   %this.setShaderConst("$edgeAmount", $SunBokehPostFX::edgeAmount);  
   %this.setShaderConst("$haloAmount", $SunBokehPostFX::haloAmount);  
   %this.setShaderConst("$sunSize", $SunBokehPostFX::sunSize);  
   %this.setShaderConst("$bokehSize", $SunBokehPostFX::bokehSize);  
   %this.setShaderConst("$debug", $SunBokehPostFX::debug);  
      
   %this.setShaderConst( "$sizeX",getWord($pref::Video::mode, 0) );  
   %this.setShaderConst( "$sizeY",getWord($pref::Video::mode, 1) );  
}  