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
