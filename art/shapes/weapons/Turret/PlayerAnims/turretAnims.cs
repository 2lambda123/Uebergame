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

   // BEGIN: Turret Sequences
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Back.dts Back", "Turret_Back", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Crouch_Root.dts Crouch_Root", "Turret_Crouch_Root", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Crouch_Backward.dts Crouch_Backward", "Turret_Crouch_Backward", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Crouch_Forward.dts Crouch_Forward", "Turret_Crouch_Forward", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Crouch_Side.dts Crouch_Side", "Turret_Crouch_Side", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Death1.dts Death1", "Turret_Death1", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Death2.dts Death2", "Turret_Death2", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Fall.dts Fall", "Turret_Fall", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Run.dts Run", "Turret_Run", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Jump.dts Jump", "Turret_Jump", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Land.dts Land", "Turret_Land", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Look.dts Look", "Turret_Look", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Head.dts Head", "Turret_Head", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Recoil.dts Recoil", "Turret_Recoil", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Fire_Release.dts Fire_Release", "Turret_Fire_Release", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Root.dts Root", "Turret_Root", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Side.dts Side", "Turret_Side", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Sitting.dts Sitting", "Turret_Sitting", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Swim_Backward.dts Swim_Backward", "Turret_Swim_Backward", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Swim_Forward.dts Swim_Forward", "Turret_Swim_Forward", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Swim_Root.dts Swim_Root", "Turret_Swim_Root", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Swim_Left.dts Swim_Left", "Turret_Swim_Left", 0, -1);
   %this.addSequence( "art/shapes/weapons/Turret/PlayerAnims/PlayerAnim_Turret_Swim_Right.dts Swim_Right", "Turret_Swim_Right", 0, -1);
   
   %this.setSequenceCyclic( "Turret_Back", true);
   %this.setSequenceCyclic( "Turret_Crouch_Backward", true);
   %this.setSequenceCyclic( "Turret_Crouch_Forward", true);
   %this.setSequenceCyclic( "Turret_Crouch_Side", true);
   %this.setSequenceCyclic( "Turret_Death1", false);
   %this.setSequenceCyclic( "Turret_Death2", false);
   %this.setSequenceCyclic( "Turret_Fall", true);
   %this.setSequenceCyclic( "Turret_Run", true);
   %this.setSequenceCyclic( "Turret_Jump", false);
   %this.setSequenceCyclic( "Turret_Land", false);
   %this.setSequenceCyclic( "Turret_Look", false);
   %this.setSequenceCyclic( "Turret_Head", false);
   %this.setSequenceCyclic( "Turret_Recoil", false);
   %this.setSequenceCyclic( "Turret_Fire_Release", false);
   %this.setSequenceCyclic( "Turret_Root", true);
   %this.setSequenceCyclic( "Turret_Side", true);
   %this.setSequenceCyclic( "Turret_Sitting", true);
   %this.setSequenceCyclic( "Turret_Swim_Backward", true);
   %this.setSequenceCyclic( "Turret_Swim_Forward", true);
   %this.setSequenceCyclic( "Turret_Swim_Root", true);
   %this.setSequenceCyclic( "Turret_Swim_Left", true);
   %this.setSequenceCyclic( "Turret_Swim_Right", true);
   
   %this.setSequenceBlend( "Turret_Head", "1", "Turret_Root", "0");
   %this.setSequenceBlend( "Turret_Look", "1", "Turret_Root", "0");
   %this.setSequenceBlend( "Turret_Recoil", "1", "Turret_Root", "0");
   %this.setSequenceBlend( "Turret_Fire_Release", "1", "Turret_Root", "0");
   
   %this.setSequenceGroundSpeed( "Turret_Back", "0 -3.6 0");
   %this.setSequenceGroundSpeed( "Turret_Run", "0 5.0 0");
   %this.setSequenceGroundSpeed( "Turret_Side", "3.6 0 0");
   %this.setSequenceGroundSpeed( "Turret_Swim_Backward", "0 -1 0");
   %this.setSequenceGroundSpeed( "Turret_Swim_Forward", "0 1 0");
   %this.setSequenceGroundSpeed( "Turret_Swim_Left", "1 0 0");
   %this.setSequenceGroundSpeed( "Turret_Swim_Right", "-1 0 0");
   %this.setSequenceGroundSpeed( "Turret_Crouch_Backward", "0 -2 0");
   %this.setSequenceGroundSpeed( "Turret_Crouch_Forward", "0 2 0");
   %this.setSequenceGroundSpeed( "Turret_Crouch_Side", "1 0 0");
   // END: Turret Sequences
