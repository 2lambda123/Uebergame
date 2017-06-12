
function ConsoleDlg::onWake()
{
   %position = $pref::Console::position;
   if(%position $= "")
      %position = ConsoleDlgWindow.position;
   
   %extent = $pref::Console::extent;
   if(getWord(%extent, 0) < getWord(ConsoleDlgWindow.minExtent, 0) ||
      getWord(%extent, 1) < getWord(ConsoleDlgWindow.minExtent, 1))
      %extent = ConsoleDlgWindow.extent;
   
   ConsoleDlgWindow.resize(getWord(%position, 0), getWord(%position, 1),
                           getWord(%extent, 0),   getWord(%extent, 1));
}

function ConsoleDlg::onSleep()
{
   $pref::Console::position = ConsoleDlgWindow.position;
   $pref::Console::extent = ConsoleDlgWindow.extent;
}

function ConsoleEntry::eval()
{
   %text = trim( ConsoleEntry.getValue() );
   if( %text $= "" )
      return;
   
   // If it's missing a trailing () and it's not a variable, append the parentheses.
   if( strpos(%text, "(") == -1 && !isDefined( %text ) )
   {
      if(strpos(%text, "=") == -1 && strpos(%text, " ") == -1)
      {
         if(strpos(%text, "{") == -1 && strpos(%text, "}") == -1)
         {
            %text = %text @ "()";
         }
      }
   }
   
   // Append a semicolon if need be.
   %pos = strlen(%text) - 1;
   if(strpos(%text, ";", %pos) == -1 && strpos(%text, "}") == -1)
   {
      %text = %text @ ";";
   }
   
   // Turn off warnings for assigning from void and evaluate the snippet.
   if( !isDefined( "$Con::warnVoidAssignment" ) )
      %oldWarnVoidAssignment = true;
   else
      %oldWarnVoidAssignment = $Con::warnVoidAssignment;
   $Con::warnVoidAssignment = false;
   
   echo("==>" @ %text);
   if(    !startsWith( %text, "function " )
       && !startsWith( %text, "datablock " )
       && !startsWith( %text, "foreach(" )
       && !startsWith( %text, "foreach$(" )
       && !startsWith( %text, "if(" )
       && !startsWith( %text, "while(" )
       && !startsWith( %text, "for(" )
       && !startsWith( %text, "switch(" )
       && !startsWith( %text, "switch$(" ) )
      eval( "%result = " @ %text );
   else
      eval( %text );
   $Con::warnVoidAssignment = %oldWarnVoidAssignment;
   
   ConsoleEntry.setValue("");
   
   // Echo result.
   if( %result !$= "" )
      echo( %result );
}

function ToggleConsole(%make)
{
   if (%make)
   {
      if (ConsoleDlg.isAwake())
      {
         // Deactivate the console.
         Canvas.popDialog(ConsoleDlg);
      }
      else
      {
         Canvas.pushDialog(ConsoleDlg, 99);

         // Check for any pending errors to display
         updateConsoleErrorWindow();
         
	      // Collapse the errors if this is the first time...
         if(ConsoleErrorPane._initialized == false)
            ConsoleErrorPane.setCollapsed(true);
      }
   }
}

function ConsoleDlg::hideWindow( %this )
{
   %this-->Scroll.setVisible( false );
}

function ConsoleDlg::showWindow( %this )
{
   %this-->Scroll.setVisible( true );
}

function ConsoleDlg::setAlpha(  %this, %alpha )
{
   if ( %alpha $= "" )
      ConsoleScrollProfile.fillColor = $ConsoleDefaultFillColor;
   else
      ConsoleScrollProfile.fillColor = getWords( $ConsoleDefaultFillColor, 0, 2 ) SPC %alpha * 255.0;
}

// If a message is selected that has a source location preceding it, allow jumping to the
// source location in Torsion by clicking on the message in the log view.
function ConsoleMessageLogView::onMessageSelected( %this, %level, %message )
{
   if( !isFunction( "EditorOpenFileInTorsion" ) )
      return;
      
   %fileText = getWord( %message, 0 );
   %lineText = getWord( %message, 1 );
   
   if( %fileText $= "" || %lineText $= "" )
      return;
   
   %fileName = makeFullPath( %fileText );
   if( !isFile( %fileName ) )
      return;
      
   %lineTextLen = strlen( %lineText );
   if( !startsWith( %lineText, "(" ) ||
       !endsWith( %lineText, "):" ) )
      return;
      
   %lineNumber = getSubStr( %lineText, 1, %lineTextLen - 2 );
   
   EditorOpenFileInTorsion( %fileName, %lineNumber );
}

// The first hash is 1000...
$ScriptErrorHashDisplayed = 999;

function updateConsoleErrorWindow()
{
   if($ScriptErrorHash != $ScriptErrorHashDisplayed && $ScriptErrorHash != 0)
   {
      // Hash was different so there must be a new error. Update the display!
      %oldText = ConsoleErrorDisplay.getText();
      
      if(%oldText !$= "")
      	ConsoleErrorDisplay.setText(%oldText @ "\n" @ $ScriptError);
      else
      	ConsoleErrorDisplay.setText($ScriptError);
      
      ConsoleErrorDisplay.setCursorPosition(100000); // Hacka hacka hacka
      ConsoleErrorDisplay.scrollToBottom();
      
      // Update the pane caption.
      $ConsoleErrorCount += $ScriptErrorHash - $ScriptErrorHashDisplayed;
      ConsoleErrorPane.caption = $ConsoleErrorCount @ " script compilation error(s) have occured!";
      
      // Indicate we dealt with this...
      $ScriptErrorHashDisplayed = $ScriptErrorHash;
      $ScriptError = "";
   }
}
