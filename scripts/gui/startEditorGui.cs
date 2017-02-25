
function loadTemplate( %mission )
{
   // Show the loading screen immediately.
   if ( isObject( LoadingGui ) )
   {
      Canvas.setContent("LoadingGui");
      LoadingProgress.setValue(1);
      LoadingProgressTxt.setValue("LOADING MISSION FILE");
      Canvas.repaint();
   }
   
   activatePackage( "BootEditor" ); // launch templates with editor open

   // Prepare and launch the server.
   return createAndConnectToLocalServer( "SinglePlayer", %mission, DM);
}