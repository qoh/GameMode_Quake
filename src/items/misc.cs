function Item::createFxLight(%this)
{
	%light = %this.getDataBlock().getQuakeLight();

	if (!isObject(%light) || !isObject(%this.spawnBrick))
	{
		return;
	}

	%this.light = new FxLight()
	{
		datablock = %light;

		enable = 1;
		iconSize = 1;
	};

	MissionCleanup.add(%this.light);
	%this.light.setTransform(%this.getTransform());
}

function ItemData::getQuakeLight(%this)
{
	if (%this.noQuakeLight)
	{
		return 0;
	}

	%light["white"] = QuakeItemWhiteLight;
	%light["gold"] = QuakeItemGoldLight;
	%light["green"] = QuakeItemGreenLight;
	%light["blue"] = QuakeItemBlueLight;
	%light["yellow"] = QuakeItemYellowLight;
	%light["red"] = QuakeItemRedLight;
	%light["greenGray"] = QuakeItemGreenGrayLight;

	%light = %light[%this.quakeLightColor];

	if (isObject(%light))
	{
		return %light;
	}

	return %light["red"];
}