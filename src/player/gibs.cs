function Player::spawnGib(%this, %node, %velocity)
{
	if (%velocity $= "")
	{
		%x = getRandom(-5, 5);
		%y = getRandom(-5, 5);
		%z = getRandom(3, 7);

		%velocity = vectorAdd(%this.getVelocity(), %x SPC %y SPC %z);
	}

	%item = new Item()
	{
		datablock = QuakePlayerGibs;
	};

	MissionCleanup.add(%item);

	%item.hideNode("ALL");
	%item.unHideNode(%node);

	%item.setCollisionTimeout(%this);
	%item.schedulePop();

	%x = getRandom() * $pi * 2;
	%y = getRandom() * $pi * 2;
	%z = getRandom() * $pi * 2;

	%transform = vectorAdd(%this.getTransform(), "0 0 1");
	%transform = %transform SPC getWords(matrixCreateFromEuler(%x SPC %y SPC %z), 3, 6);

	%item.setTransform(%transform);
	%item.setVelocity(%velocity);

	%item.monitorGibVelocity(vectorLen(%velocity));
}

function Player::spawnGibs(%this)
{
	%this.gibbed = 1;
	serverPlay3D(QuakePlayerGibSplat, %this.getHackPosition());

	%nodes = "headSkin chest rshoe lshoe rhand rhand rarm larm";
	%count = getRandom(4, 7);

	for (%i = 0; %i < %count && %nodes !$= ""; %i++)
	{
		%nodeIndex = getRandom(0, getWordCount(%nodes) - 1);
		%nodeName = getWord(%nodes, %nodeIndex);

		%nodes = setWord(%nodes, %nodeIndex, "");
		%nodes = trim(strReplace(%nodes, "  ", " "));

		%this.schedule(0, spawnGib, %nodeName);
	}

	%this.hideNode("ALL");
	%this.unMountImage(0);
	%this.setShapeNameDistance(0);
}

function Item::monitorGibVelocity(%this, %before)
{
	cancel(%this.monitorGibVelocity);

	%now = vectorLen(%this.getVelocity());
	%delta = %before - %now;

	if (%delta >= 2)
	{
		serverPlay3D(QuakePlayerGibImpact @ getRandom(1, 3), %this.getPosition());
	}

	%this.monitorGibVelocity = %this.schedule(50, monitorGibVelocity, %now);
}