function GameMenuGui::onWake(%this)
{
   sfxPlayOnce( AudioButtonAcceptSubSoundList );
}

function GameMenuGui::onSleep(%this)
{
   sfxPlayOnce( AudioButtonCancelSoundList );
}