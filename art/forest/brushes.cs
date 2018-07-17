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

// This is the default save location for any ForestBrush(s) created in 
// the Forest Editor.
// This script is executed from ForestEditorPlugin::onWorldEditorStartup().

//--- OBJECT WRITE BEGIN ---
new SimGroup(ForestBrushGroup) {
   canSave = "1";
   canSaveDynamicFields = "1";

   new ForestBrush() {
      internalName = "fir_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "fir_01_5m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fir_01_5m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.5";
         scaleMax = "1.5";
         scaleExponent = "1";
         sinkMin = "0.1";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "60";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "fir_01_5m_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fir_01_5m_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.5";
         scaleMax = "1.5";
         scaleExponent = "1";
         sinkMin = "0.1";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "60";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "fir_01_10m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fir_01_10m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0.1";
         sinkMax = "1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "60";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "fir_01_10m_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fir_01_10m_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0.1";
         sinkMax = "1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "60";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "fir_01_15m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fir_01_15m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0.1";
         sinkMax = "1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "60";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "fir_01_15m_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fir_01_15m_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0.1";
         sinkMax = "1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "60";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "fir_01_15m_c";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fir_01_15m_c";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0.1";
         sinkMax = "1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "60";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "fern_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "fern_01_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fern_01_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.5";
         scaleMax = "1.5";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "60";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "fern_01_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fern_01_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.5";
         scaleMax = "1.5";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "60";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "fern_01_c";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fern_01_c";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.5";
         scaleMax = "1.5";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "60";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "palm_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "palm_01_bush";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "palm_01_bush";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "30";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "palm_01_short";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "palm_01_short";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.2";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "40";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "palm_01_tall";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "palm_01_tall";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "palm_01_tall_leaning";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "palm_01_tall_leaning";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "canopy_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "canopy_01_shrub";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "canopy_01_shrub";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "40";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "canopy_01_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "canopy_01_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.2";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "40";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "canopy_01_a_vines";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "canopy_01_a_vines";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "40";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "canopy_01_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "canopy_01_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "40";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "canopy_01_c";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "canopy_01_c";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "40";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "banana_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "banana_01_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "banana_01_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "2";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "40";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "thintree_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "thintree_01_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "thintree_01_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "1";
         scaleMax = "1.5";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.3";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "50";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "thintree_01_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "thintree_01_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "1";
         scaleMax = "1.5";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.3";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "50";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "thintree_01_c";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "thintree_01_c";
         probability = "1";
         rotationRange = "360";
         scaleMin = "1";
         scaleMax = "1.5";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.3";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "50";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "deadbush_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "deadbush_01_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "deadbush_01_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.6";
         scaleMax = "1.3";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "deadbush_01_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "deadbush_01_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.6";
         scaleMax = "1.3";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "deadbush_01_c";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "deadbush_01_c";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.6";
         scaleMax = "1.3";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "birch_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "birch_01_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "birch_01_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.6";
         scaleMax = "1.1";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "35";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "birch_01_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "birch_01_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.6";
         scaleMax = "1.1";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "35";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "birch_01_c";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "birch_01_c";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.6";
         scaleMax = "1.1";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "35";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "birch_01_d";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "birch_01_d";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.6";
         scaleMax = "1.1";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "35";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "fern_tropical_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "fern_tropical_01_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "fern_tropical_01_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.75";
         scaleMax = "1.25";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "leaf_plants";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "bigleaf_01_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "bigleaf_01_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.3";
         scaleMax = "1";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "longleaf_01_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "longleaf_01_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.6";
         scaleMax = "1.4";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "cedar_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "cedar_01_2m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_2m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.7";
         scaleMax = "1.3";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.2";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "cedar_01_5m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_5m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.8";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.2";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "cedar_01_8m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_8m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.8";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.3";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "cedar_01_8m_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_8m_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.8";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.3";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "cedar_01_12m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_12m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.8";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "cedar_01_12m_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_12m_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.8";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "cedar_01_15m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_15m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.8";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "cedar_01_15m_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_15m_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.8";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "cedar_01_12m_a_dead";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_12m_a_dead";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.6";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.6";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "cedar_01_15m_a_dead";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_15m_a_dead";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.6";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.7";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "cedar_01_stump_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "cedar_01_stump_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.5";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "-0.5";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "45";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
   new ForestBrush() {
      internalName = "poplar_01";
      canSave = "1";
      canSaveDynamicFields = "1";

      new ForestBrushElement() {
         internalName = "poplar_01_15m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "poplar_01_15m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.8";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "37";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "poplar_01_15m_b";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "poplar_01_15m_b";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.8";
         scaleMax = "1.2";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "1";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "37";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "poplar_01_12m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "poplar_01_12m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.7";
         scaleMax = "1.3";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.7";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "37";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "poplar_01_6m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "poplar_01_6m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.7";
         scaleMax = "1.3";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "37";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
      new ForestBrushElement() {
         internalName = "poplar_01_4m_a";
         canSave = "1";
         canSaveDynamicFields = "1";
         ForestItemData = "poplar_01_4m_a";
         probability = "1";
         rotationRange = "360";
         scaleMin = "0.7";
         scaleMax = "1.3";
         scaleExponent = "1";
         sinkMin = "0";
         sinkMax = "0.5";
         sinkRadius = "1";
         slopeMin = "0";
         slopeMax = "37";
         elevationMin = "-10000";
         elevationMax = "10000";
      };
   };
};
//--- OBJECT WRITE END ---
