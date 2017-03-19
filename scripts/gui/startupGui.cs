
function startupGui::onWake(%this)
{  
	%text = getRandomStartupText();
	startupGuiText.setText(%text);
}

function getRandomStartupText(%text)
{
   %list = new ArrayObject();
   %list.add("0", "Loading");
   %list.add("1", "More Loading");
   %list.add("2", "Ueber-Loading-Screen");
   %list.add("3", "Still Loading");
   %list.add("4", "Loading fancy graphics");
   %list.add("5", "zzzZZzzzZZZzzzZZzzzzzZzz");
   %list.add("6", "Kork-Chan approved");
   %list.add("7", "The most Ueber game you will ever play");
   %list.add("8", "Still loading");
   %list.add("9", "Slow potato is slow");
   %list.add("10", "Ueberengine initializing");
   %list.add("11", "...");
   %list.add("12", "Loading fancy loading screen");
   %list.add("13", "Loading the Ueber-Screensaver");
   %list.add("14", "Loading super 3D menu background");
   %list.add("15", "Loading resource wasting features");
   %list.add("16", "Loading cool background");
   %list.add("17", "Prepare for meditative background");
   %list.add("18", "You can hide main Menu Gui with ESC");
   %list.add("19", "3D menu can be turned off in options/misc");
   %list.add("20", "Uebercharging");
   %list.add("21", "Just another loading screen");
   %list.add("22", "Nothing to see here");
   %list.add("23", "Prepare for the ultimate experience");
   %list.add("24", "You will be in the Main Menu soon");
   %list.add("25", "Energize");
   %list.add("26", "Loading harder");
   %list.add("27", "Loading intensifies");
   %list.add("28", "Loadingception");
   %list.add("29", "Hello");
   %list.add("30", "I see you");
   %list.add("31", "Boring startup screen");
   %list.add("32", "Replace me");
   
   %random = getRandom(0, 32);
   %text = %list.getValue(%random);
   
   return (%text);
}