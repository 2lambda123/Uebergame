function InspectVars( %filter )
{
   if ( %filter $= "" )
      %filter = "*";
   
   //if ( !ConsoleVarDlg.isAwake() )   
      Canvas.pushDialog( ConsoleVarDlg, 100 );
      
   ConsoleVarInspector.loadVars( %filter );
}

function InspectVarsToggleCursor()
{
   ConsoleVarDlg.noCursor = !(ConsoleVarDlg.noCursor);
}