new SFXDescription(GuiAudioDescription)
{
   volume      = 1.0;
   isLooping   = false;
   is3D        = false;
   channel     = $GuiAudioType;
};

singleton SFXProfile(AudioButtonOver)
{
   filename = "art/sound/gui/button_over_01";
   description = GuiAudioDescription;
   preload = true;
};

singleton SFXPlayList(AudioButtonOverSoundList)
{
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "GuiAudioDescription";
   track[0] = "AudioButtonOver";
   pitchScaleVariance[0] = "-0.2 0.2";
   volumeScaleVariance[0] = "-0.2 0";
};

singleton SFXProfile(AudioButtonAccept)
{
   filename = "art/sound/gui/button_accept_01";
   description = GuiAudioDescription;
   preload = true;
};

singleton SFXPlayList(AudioButtonAcceptSoundList)
{
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "GuiAudioDescription";
   track[0] = "AudioButtonAccept";
   pitchScaleVariance[0] = "-0.2 0.2";
   volumeScaleVariance[0] = "-0.2 0";
};

singleton SFXProfile(AudioButtonCancel)
{
   filename = "art/sound/gui/button_cancel_01";
   description = GuiAudioDescription;
   preload = true;
};

singleton SFXPlayList(AudioButtonCancelSoundList)
{
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "GuiAudioDescription";
   track[0] = "AudioButtonCancel";
   pitchScaleVariance[0] = "-0.2 0.2";
   volumeScaleVariance[0] = "-0.2 0";
};