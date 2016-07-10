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
      
    #include "shadergen:/autogenConditioners.h"  
    #include "../gl/hlslCompat.glsl"
    #include "./gl/postFX.glsl"
    #include "./../gl/torque.glsl"
      
    uniform sampler2D backBuffer;      // Screen buffer  
    uniform sampler2D bokehTex;        // Screen bokeh texture  
    uniform sampler2D dirtTex;         // Screen dirt texture  
    uniform float sizeX;                                // %this.setShaderConst("$sizeX",getWord($pref::Video::mode, 0));  
    uniform float sizeY;                                // %this.setShaderConst("$sizeY",getWord($pref::Video::mode, 1));  
      
    uniform vec3 camForward;  
    uniform vec3 lightDirection;  
    uniform vec2 screenSunPos;  
    uniform float sunVisibility;  
      
    static const vec2 fViewportDimensions = vec2(sizeX,sizeY); // vec2(screenWidth, screenHeight)  
    static const vec2 fInverseViewportDimensions = vec2(1/sizeX,1/sizeY); // reciprocal of fViewportDimensions  
      
    uniform float sunAmount;    // Amount of bokeh around the sun  
    uniform float edgeAmount;   // Amount of bokeh around the screen edges  
    uniform float haloAmount;   // Amount of dirt/halo  
    uniform float sunSize;      // Sun circle size  
    uniform float bokehSize;    // Bokeh texture tiling value  
    uniform bool debug;  
      
    out vec4 OUT_col;

    void main()  
    {  
        // UV coords  
        vec2 texCoord = IN_uv0;  //IN.uv0
        // Screen color  
        vec4 color = tex2D( backBuffer, texCoord );  
          
        // Screen aspect ratio  
        float aspect = sizeY / sizeX;  
        // Screen bokeh texture (tiling)  
        vec3 screenBokeh = ( tex2D(bokehTex, texCoord * fViewportDimensions / bokehSize).xyz + 1.e-6f ) * 0.5;  
        // Screen dirt/halo texture  
        vec3 screenDirt = tex2D(dirtTex, texCoord);  
        // Calculate current sun strength on screen  
        float sunStrength = saturate( dot( -lightDirection, camForward ) );  
          
        float circle = 0.0f;  
        circle = 1 - distance(texCoord * vec2(1.0f, aspect), vec2(screenSunPos.x, screenSunPos.y * aspect) );  
        circle = smoothstep(saturate(1-sunSize),1,circle) * sunAmount;  
        vec3 circle2 = 0.0f;  
        circle2 = distance(texCoord , 0.5 );  
        circle2 = smoothstep(0.2,1,circle2);  
          
        float edgeMask;  
        edgeMask = lerp(0,1-screenSunPos.y,1-texCoord.y) + lerp(0,screenSunPos.y,texCoord.y) + lerp(0,1-screenSunPos.x,1-texCoord.x) + lerp(0,screenSunPos.x,texCoord.x);  
        edgeMask = (1-edgeMask) * circle2;  
          
        edgeMask = saturate( edgeMask * 10);  
        screenDirt *= edgeMask * sunStrength * haloAmount;  
        edgeMask *= edgeAmount;  
        circle = (circle + edgeMask) * sunStrength;  
        screenBokeh *= circle;  
      
        if( !debug )  
            OUT_col = color + vec4(screenBokeh + screenDirt,1) * 1 * sunVisibility;  
        else  
            OUT_col = vec4(screenBokeh + screenDirt,1) * 1.5;  
    }  