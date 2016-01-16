 datablock SFXProfile(BulletImpactSound)
{
   filename = "art/sound/weapons/ricochet/bullet_ricochet_01";
   description = AudioClosest3D;
   preload = true;
};

datablock SFXPlayList(BulletImpactSoundList)
{
   random = "StrictRandom";
   loopMode = "Single";
   numSlotsToPlay = "1";
   description = "AudioClosest3D";
   track[0] = "BulletImpactSound";
   pitchScaleVariance[0] = "-0.2 0.2";
   track[1] = "BulletImpactSound";
   pitchScaleVariance[1] = "-0.2 0.2";
   track[2] = "BulletImpactSound";
   pitchScaleVariance[2] = "-0.2 0.2";
   track[3] = "BulletImpactSound";
   pitchScaleVariance[3] = "-0.2 0.2";
};