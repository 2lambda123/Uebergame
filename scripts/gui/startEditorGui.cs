
function loadTemplate( %mission )
{
   disconnect(); // quit in case a mission is running like main menu level
   
   // Show the loading screen immediately.
   if ( !$UsingMainMenuLevel &&  isObject( LoadingGui ) )
   {
      Canvas.setContent("LoadingGui");
      LoadingProgress.setValue(1);
      LoadingProgressTxt.setValue("LOADING MISSION FILE");
      Canvas.repaint();
   }
   
   $UsingMainMenuLevel = false; // loading mission so we are no longer in main menu
   
   activatePackage( "BootEditor" ); // launch templates with editor open

   // Prepare and launch the server.
   return createAndConnectToLocalServer( "SinglePlayer", %mission, DM);
}
