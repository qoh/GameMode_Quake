datablock AudioProfile(QuakeWeaponPickup)
{
	fileName = "Add-Ons/GameMode_Quake/sound/misc/w_pkup.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock FxLightData(QuakeItemWhiteLight : GreenLight)
{
	color = "1 1 1";
	flareOn = 0;

	radius = 4;
	brightness = 1;
};

datablock FxLightData(QuakeItemGoldLight : QuakeItemWhiteLight)
{
	color = "1 0.8 0";
};

datablock FxLightData(QuakeItemGreenLight : QuakeItemWhiteLight)
{
	color = "0 1 0";
};

datablock FxLightData(QuakeItemBlueLight : QuakeItemWhiteLight)
{
	color = "0 0 1";
};

datablock FxLightData(QuakeItemYellowLight : QuakeItemWhiteLight)
{
	color = "1 1 0";
};

datablock FxLightData(QuakeItemRedLight : QuakeItemWhiteLight)
{
	color = "1 0 0";
};

datablock FxLightData(QuakeItemGreenGrayLight : QuakeItemWhiteLight)
{
	color = "0.5 1 0.75";
};