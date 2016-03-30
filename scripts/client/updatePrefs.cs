function updatePrefs()
{
   if($Pref::Version !$= 1050)
   {
      //<change some prefs>
      $Pref::Version = 1050;
   }
   
   if($Pref::Version == 1050)
   {
      //<change some prefs again>
   }
}