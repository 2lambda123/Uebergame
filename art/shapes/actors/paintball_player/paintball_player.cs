
singleton TSShapeConstructor(Paintball_playerDae)
{
   baseShape = "./paintball_player.dts";
};

function Paintball_playerDae::onLoad(%this)
{
   %this.addSequence("ambient", "root", "13", "61", "1", "0");
   %this.addSequence("ambient", "head", "109", "117", "1", "0");
   %this.setSequenceBlend("head", "1", "root", "0");
   %this.setSequenceCyclic("head", "0");
   %this.addSequence("ambient", "look", "119", "127", "1", "0");
   %this.setSequenceBlend("look", "1", "root", "0");
   %this.setSequenceCyclic("look", "0");
   %this.setSequencePriority("Look", "1");
   %this.addSequence("ambient", "run", "62", "83", "1", "0");
   %this.setSequenceGroundSpeed("run", "0 0.75 0", "0 0 0");
   %this.addTrigger("run", "4", "1");
   %this.addTrigger("run", "14", "2");
   %this.addSequence("ambient", "back", "62", "83", "1", "0");
   %this.setSequenceGroundSpeed("back", "0 0.75 0", "0 0 0");
   %this.addTrigger("back", "4", "1");
   %this.addTrigger("back", "14", "2");
   %this.addSequence("ambient", "side", "62", "83", "1", "0");
   %this.setSequenceGroundSpeed("side", "0 0 0", "0 0.75 0");
   %this.addTrigger("side", "4", "1");
   %this.addTrigger("side", "14", "2");
   %this.addSequence("ambient", "side_right", "62", "83", "1", "0");
   %this.setSequenceGroundSpeed("side_right", "0 0 0", "0 0.75 0");
   %this.addTrigger("side_right", "4", "1");
   %this.addTrigger("side_right", "14", "2");
   %this.addSequence("ambient", "sprint_forward", "85", "106", "1", "0");
   %this.setSequenceGroundSpeed("sprint_forward", "0 0.75 0", "0 0 0");
   %this.addTrigger("sprint_forward", "4", "1");
   %this.addTrigger("sprint_forward", "14", "2");
   %this.addSequence("ambient", "sprint_backward", "85", "106", "1", "0");
   %this.setSequenceGroundSpeed("sprint_backward", "0 0.75 0", "0 0 0");
   %this.addTrigger("sprint_backward", "4", "1");
   %this.addTrigger("sprint_backward", "14", "2");
   %this.addSequence("ambient", "sprint_side", "85", "106", "1", "0");
   %this.setSequenceGroundSpeed("sprint_side", "0 0 0", "0 0.75 0");
   %this.addTrigger("sprint_side", "4", "1");
   %this.addTrigger("sprint_side", "14", "2");
   %this.addSequence("ambient", "sprint_side_right", "85", "106", "1", "0");
   %this.setSequenceGroundSpeed("sprint_side_right", "0 0 0", "0 0.75 0");
   %this.addTrigger("sprint_side_right", "4", "1");
   %this.addTrigger("sprint_side_right", "14", "2");
   %this.addSequence("ambient", "crouch_root", "129", "177", "1", "0");
   %this.addSequence("ambient", "crouch_forward", "178", "199", "1", "0");
   %this.setSequenceGroundSpeed("crouch_forward", "0 0 0", "0 0.75 0");
   %this.addSequence("ambient", "crouch_backward", "178", "199", "1", "0");
   %this.setSequenceGroundSpeed("crouch_backward", "0 0 0", "0 0.75 0");
   %this.addSequence("ambient", "crouch_side", "178", "199", "1", "0");
   %this.setSequenceGroundSpeed("crouch_side", "0 0 0", "0 0.75 0");
   %this.addSequence("ambient", "death1", "201", "223", "1", "0");
   %this.setSequenceCyclic("death1", "0");
   %this.addSequence("ambient", "reload", "280", "328", "1", "0");
   %this.setSequenceCyclic("reload", "0");
   %this.setSequencePriority("reload", "1");
   %this.setSequenceBlend("reload", "1", "root", "0");
}
