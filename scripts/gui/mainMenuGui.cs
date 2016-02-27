function MainMenuGui::onWake(%this)
{
   if (isFunction("getWebDeployment") &&
       getWebDeployment() &&
       isObject(%this-->ExitButton))
      %this-->ExitButton.setVisible(false);
}

function MainMenuGui::onWake(%this)
{
   %header = "Version" SPC getVersionString();
   Version_Number.setText(%header);
}