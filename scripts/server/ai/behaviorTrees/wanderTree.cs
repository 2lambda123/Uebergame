//--- OBJECT WRITE BEGIN ---
new Root(wanderTree) {
   canSave = "1";
   canSaveDynamicFields = "1";

   new Sequence() {
      canSave = "1";
      canSaveDynamicFields = "1";

      new ScriptedBehavior() {
         preconditionMode = "ONCE";
         internalName = "move somewhere";
         class = "wanderTask";
         canSave = "1";
         canSaveDynamicFields = "1";
      };
      new Parallel() {
         returnPolicy = "REQUIRE_ONE";
         canSave = "1";
         canSaveDynamicFields = "1";
      
         new WaitForSignal() {
            signalName = "onReachDestination";
            timeoutMs = "0";
            internalName = "wait until there";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
         new RandomWait() {
            waitMinMs = "1000";
            waitMaxMs = "10000";
            internalName = "WaitForSignal timeout";
            canSave = "1";
            canSaveDynamicFields = "1";
         };
      };
      new RandomWait() {
         waitMinMs = "0";
         waitMaxMs = "1000";
         internalName = "pause";
         canSave = "1";
         canSaveDynamicFields = "1";
      };
   };
};
//--- OBJECT WRITE END ---
