//--- OBJECT WRITE BEGIN ---
new Root(WanderPointTree) {
   canSave = "1";
   canSaveDynamicFields = "1";
   
   new Sequence() {
      canSave = "1";
      canSaveDynamicFields = "1";

      new ScriptedBehavior() {
         preconditionMode = "ONCE";
         internalName = "find wander point";
         class = "findWanderPointTask";
         canSave = "1";
         canSaveDynamicFields = "1";
      };
      new ScriptedBehavior() {
         preconditionMode = "ONCE";
         internalName = "go to wander point";
         class = "goToWanderPointTask";
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
         new Sequence() {
            canSave = "1";
            canSaveDynamicFields = "1";
      
            new RandomWait() {
               waitMinMs = "1000";
               waitMaxMs = "15000";
               internalName = "WaitForSignal timeout";
               canSave = "1";
               canSaveDynamicFields = "1";
            };
            new SubTree() {
               subTreeName = "wanderTree";
               internalName = "wander";
               canSave = "1";
               canSaveDynamicFields = "1";
            };
         };
      };
   };
};
//--- OBJECT WRITE END ---
