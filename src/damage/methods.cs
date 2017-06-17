function QuakePlayer::onNewDataBlock(%this, %obj)
{
	Parent::onNewDataBlock(%this, %obj);

	%obj.health = 125;
	%obj.armor = 0;

	%obj.decayQuakeHealthTick = %obj.schedule(1000, "decayQuakeHealthTick");
	%obj.decayQuakeArmorTick = %obj.schedule(1000, "decayQuakeArmorTick");

	%obj.schedule(0, scheduledQuakeDisplay);
}

function QuakePlayer::damage(%this, %obj, %src, %pos, %dmg, %type)
{
	if (%obj.getState() $= "Dead")
	{
		if (!%obj.gibbed)
		{
			%obj.spawnGibs();
		}

		return;
	}

	if (%dmg <= 0)
	{
		return;
	}

	%client = %obj.client;

	if (isObject(%src))
	{
		%sourceClient = getBrickGroupFromObject(%src).client;
	}

	if (isObject(%sourceClient) && %sourceClient != %client)
	{
		if (!%sourceClient.disallowHitSound)
		{
			%sourceClient.disallowHitSound = 1;
			%sourceClient.play2D(QuakeHitSound);

			schedule(1, %sourceClient, reEnableHitSound, %sourceClient);
			// schedule(0, 0, eval, %sourceClient @ ".disallowHitSound = 0;");
		}
	}

	if (%obj.armor)
	{
		%part = mCeil(%dmg * (2 / 3));

		if (%obj.armor >= %part)
		{
			%dmg -= %part;
			%obj.armor -= %part;
		}
		else
		{
			%dmg -= %obj.armor;
			%obj.armor = 0;
		}
	}

	%obj.health -= %dmg;

	if (%obj.health <= 0)
	{
		%obj.spawnTime = 0;

		if (%obj.health <= -20)
		{
			%obj.spawnGibs();
		}

		Parent::damage(%this, %obj, %src, %pos, %this.maxDamage * 2, %type);
	}
	else
	{
		%obj.setDamageFlash(0.3);
		%obj.playPain();
	}

	if (isObject(%obj.client))
	{
		%obj.client.updateQuakeDisplay();
	}
}

function reEnableHitSound(%client)
{
	%client.disallowHitSound = 0;
}

function Player::addArmor( %this, %armor)
{
	%this.setArmor(%this.armor + %armor);
}

function Player::setArmor(%this, %armor)
{
	%this.armor = mClamp(%armor, 0, 200);

	if (isObject(%this.client))
	{
		%this.client.updateQuakeDisplay();
	}

	if (%this.armor > 100 && !isEventPending(%this.decayQuakeArmorTick))
	{
		%this.decayQuakeArmorTick = %this.schedule(1000, decayQuakeArmorTick);
	}
}

function Player::decayQuakeHealthTick(%this)
{
	cancel(%this.decayQuakeHealthTick);

	if (%this.health > 100)
	{
		%this.health -= 1;

		if (%this.health < 100)
		{
			%this.health = 100;
		}

		if (isObject(%this.client))
		{
			%this.client.updateQuakeDisplay();
		}
	}
	else
	{
		return;
	}

	%this.decayQuakeHealthTick = %this.schedule(1000, decayQuakeHealthTick);
}

function Player::decayQuakeArmorTick(%this)
{
	cancel(%this.decayQuakeArmorTick);

	if (%this.armor > 100)
	{
		%this.armor -= 1;

		if (%this.armor < 100)
		{
			%this.armor = 100;
		}

		if (isObject(%this.client))
		{
			%this.client.updateQuakeDisplay();
		}
	}
	else
	{
		return;
	}

	%this.decayQuakeArmorTick = %this.schedule(1000, decayQuakeArmorTick);
}

function Player::scheduledQuakeDisplay(%this)
{
	if (isObject(%this.client))
	{
		%this.client.updateQuakeDisplay();
	}
}

function obstructRadiusDamageCheck(%pos, %col)
{
	%b = %col.getHackPosition();
	%half = vectorSub(%b, %col.position);

	%a = vectorAdd(%col.position, vectorScale(%half, 0.1));
	%c = vectorAdd(%col.position, vectorScale(%half, 1.9));

	%mask = $TypeMasks::FxBrickObjectType;

	if (containerRayCast(%pos, %a, %mask) !$= 0)
	{
		if (containerRayCast(%pos, %b, %mask) !$= 0)
		{
			if (containerRayCast(%pos, %c, %mask) !$= 0)
			{
				return 0;
			}
		}
	}

	return 1;
}
