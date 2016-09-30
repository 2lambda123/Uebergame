//--- OBJECT WRITE BEGIN ---
new Root(reloadTree) {
   canSave = "1";
   canSaveDynamicFields = "1";

   new Sequence() {
      canSave = "1";
      canSaveDynamicFields = "1";

      new ScriptEval() {
         behaviorScript = "if (%obj.getInventory(%image.clip) > 0) return SUCCESS;";
         defaultReturnStatus = "FAILURE";
         internalName = "need to reload?";
         canSave = "1";
         canSaveDynamicFields = "1";
      };
      new ScriptedBehavior() {
         preconditionMode = "ONCE";
         internalName = "reload weapon";
         class = "reloadWeaponTask";
         canSave = "1";
         canSaveDynamicFields = "1";
      };
   };
};
//--- OBJECT WRITE END ---