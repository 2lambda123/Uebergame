function MainMenuGui::onWake(%this)
{
   if (isFunction("getWebDeployment") &&
       getWebDeployment() &&
       isObject(%this-->ExitButton))
      %this-->ExitButton.setVisible(false);
	  
	%header = "Version" SPC getVersionString();
	Version_Number.setText(%header);
}

function MainMenuGui::DuioncomLink() {gotoWebPage("http://www.duion.com");}
function MainMenuGui::Torque3DorgLink() {gotoWebPage("http://www.torque3d.org");}