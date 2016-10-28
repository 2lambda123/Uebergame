new SFXDescription(GuiAudioDescription)
{
   volume      = 1.0;
   isLooping   = false;
   is3D        = false;
   channel     = $GuiAudioType;
};

//hover effect for main menu buttons
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

//click effect for main menu buttons
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

//click effect for sub menu buttons, just a more quiet version
singleton SFXProfile(AudioButtonAcceptSub)
{
   filename = "art/sound/gui/button_accept_01";
   description = GuiAudioDescription;
   preload = true;
};

singleton SFXPlayList(AudioButtonAcceptSubSoundList)
{
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "GuiAudioDescription";
   track[0] = "AudioButtonAccept";
   pitchScaleVariance[0] = "-0.1 0.3";
   volumeScaleVariance[0] = "-0.3 -0.1";
};

//sound effect for main menu button cancel effects like exit, disconnect etc
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

//general hover sound for normal menu items
singleton SFXProfile(AudioMenuItemHover)
{
   filename = "art/sound/gui/menu_item_hover_01";
   description = GuiAudioDescription;
   preload = true;
};

singleton SFXPlayList(AudioMenuItemHoverSoundList)
{
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "GuiAudioDescription";
   track[0] = "AudioMenuItemHover";
   pitchScaleVariance[0] = "-0.2 0.2";
   volumeScaleVariance[0] = "-0.2 0";
};

// check box sound effects
singleton SFXProfile(AudioCheckboxCheck)
{
   filename = "art/sound/gui/check_box_check_01";
   description = GuiAudioDescription;
   preload = true;
};

singleton SFXPlayList(AudioCheckboxCheckSoundList)
{
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "GuiAudioDescription";
   track[0] = "AudioCheckboxCheck";
   pitchScaleVariance[0] = "-0.2 0.2";
   volumeScaleVariance[0] = "-0.2 0";
};