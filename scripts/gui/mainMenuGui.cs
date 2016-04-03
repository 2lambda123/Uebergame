function MainMenuGui::onWake(%this)
{
   if (isFunction("getWebDeployment") &&
       getWebDeployment() &&
       isObject(%this-->ExitButton))
      %this-->ExitButton.setVisible(false);
	  
	%header = "Version" SPC getVersionString();
	Version_Number.setText(%header);
}