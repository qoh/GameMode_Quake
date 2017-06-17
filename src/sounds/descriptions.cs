function quakeSound(%name, %fileName, %description)
{
	%fileName = "Add-Ons/GameMode_Quake/sound/" @ %fileName @ ".wav";
	if (%description $= "") %description = AudioDefault3D;

	eval("datablock AudioProfile(" @ %name @ "){preload=1;description=%description;fileName=%fileName;};");
}

datablock AudioDescription(AudioQuiet3D : AudioDefault3D)
{
	volume = 0.65;
};
