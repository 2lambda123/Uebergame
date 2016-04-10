singleton TSShapeConstructor(TP_Lurker_darkDAE)
{
   baseShape = "./TP_Lurker_dark.dts";
   lodType = "DetectDTS";
   loadLights = "0";
};

function TP_Lurker_darkDAE::onLoad(%this)
{
   %this.renameSequence("ambient", "timeline");
   %this.setSequenceCyclic("timeline", "0");
   %this.addSequence("timeline", "root", "0", "0", "1", "0");
   %this.addSequence("timeline", "fire", "0", "1", "1", "0");
   %this.addSequence("timeline", "fire_vis", "9", "18", "1", "0");
}
