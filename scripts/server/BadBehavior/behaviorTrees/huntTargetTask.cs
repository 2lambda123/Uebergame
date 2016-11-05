//--- OBJECT WRITE BEGIN ---
new Root(huntTargetTree) {
   canSave = "1";
   canSaveDynamicFields = "1";

   new Sequence() {
      canSave = "1";
      canSaveDynamicFields = "1";

      new ScriptEval() {
         behaviorScript = "if (!%obj.checkInLos(%obj.targetObject)) return SUCCESS;";
         defaultReturnStatus = "FAILURE";
         internalName = "need to hunt?";
         canSave = "1";
         canSaveDynamicFields = "1";
      };
      new ScriptedBehavior() {
         preconditionMode = "ONCE";
         internalName = "hunting target";
         class = "huntTargetTask";
         canSave = "1";
         canSaveDynamicFields = "1";
      };
   };
};
//--- OBJECT WRITE END ---