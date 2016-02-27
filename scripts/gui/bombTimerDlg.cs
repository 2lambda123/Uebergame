function clientCmdOpenBombTimer()
{
   BombTimerDlg.toggle();
}

function BombTimerDlg::toggle(%this, %val)
{
   if(%this.isAwake())
      Canvas.popDialog(%this);
   else
      Canvas.pushDialog(%this);
}

function BombTimerDlg::onWake(%this)
{
   BombTimerMenu.clear();
   BombTimerMenu.selectedTime = 0;
   BombTimerMenu.add("5 Seconds", 0);
   BombTimerMenu.add("10 Seconds", 1);
   BombTimerMenu.add("15 Seconds", 2);
   BombTimerMenu.add("20 Seconds", 3);
   BombTimerMenu.add("25 Seconds", 4);
   BombTimerMenu.add("30 Seconds", 5);
   BombTimerMenu.setSelected(0);
   BombTimerMenu.onSelect(0, "5 Seconds");

   if ( isObject( hudMap ) )
   {
      hudMap.pop();
      hudMap.delete();
   }
   new ActionMap( hudMap );
   hudMap.blockBind( moveMap, bringUpOptions );
   hudMap.blockBind( moveMap, toggleArmoryDlg );
   hudMap.blockBind( moveMap, showScoreBoard );
   hudMap.blockBind( moveMap, toggleVehicleHud );
   hudMap.blockBind( moveMap, showPlayerList );
   hudMap.blockBind( moveMap, toggleNetGraph );

   hudMap.bindCmd( keyboard, escape, "", "Canvas.popDialog(%this);" );
   hudMap.push();
}

function BombTimerDlg::onSleep(%this)
{
   // Make sure the proper key maps are pushed
   tge.updateKeyMaps();
}

function BombTimerMenu::onSelect(%this, %id, %text)
{
   switch$(%id)
   {
      case 0:
         %this.selectedTime = 5;
      case 1:
         %this.selectedTime = 10;
      case 2:
         %this.selectedTime = 15;
      case 3:
         %this.selectedTime = 20;
      case 4:
         %this.selectedTime = 25;
      case 5:
         %this.selectedTime = 30;
      default:
         %this.selectedTime = 15;
   }
}

function BombTimerDlg::sendTime(%this)
{
   commandToServer('setBombTimer', BombTimerMenu.selectedTime);
   Canvas.popDialog( %this );
}

