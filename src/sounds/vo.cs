datablock AudioProfile(QuakeVO_PlayerTookLead)
{
	fileName = "Add-Ons/GameMode_Quake/sound/feedback/takenlead.wav";
	description = AudioQuiet3D;
	preload = 1;
};

datablock AudioProfile(QuakeVO_PlayerTiedLead)
{
	fileName = "Add-Ons/GameMode_Quake/sound/feedback/tiedlead.wav";
	description = AudioQuiet3D;
	preload = 1;
};

datablock AudioProfile(QuakeVO_PlayerLostLead)
{
	fileName = "Add-Ons/GameMode_Quake/sound/feedback/lostlead.wav";
	description = AudioQuiet3D;
	preload = 1;
};

datablock AudioProfile(QuakeVO_FragsLeft_1)
{
	fileName = "Add-Ons/GameMode_Quake/sound/feedback/1_frag.wav";
	description = AudioQuiet3D;
	preload = 1;
};

datablock AudioProfile(QuakeVO_FragsLeft_2)
{
	fileName = "Add-Ons/GameMode_Quake/sound/feedback/2_frags.wav";
	description = AudioQuiet3D;
	preload = 1;
};

datablock AudioProfile(QuakeVO_FragsLeft_3)
{
	fileName = "Add-Ons/GameMode_Quake/sound/feedback/3_frags.wav";
	description = AudioQuiet3D;
	preload = 1;
};

quakeSound(QuakeVO_prepare, "feedback/prepare", AudioQuiet3D);
quakeSound(QuakeVO_three, "feedback/three", AudioQuiet3D);
quakeSound(QuakeVO_two, "feedback/two", AudioQuiet3D);
quakeSound(QuakeVO_one, "feedback/one", AudioQuiet3D);
quakeSound(QuakeVO_fight, "feedback/fight", AudioQuiet3D);

datablock AudioProfile(QuakeVO_Humiliation)
{
	fileName = "Add-Ons/GameMode_Quake/sound/feedback/humiliation.wav";
	description = AudioQuiet3D;
	preload = 1;
};
