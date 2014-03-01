package QuakeInterfacePackage
{
	function Player::setHealth(%this, %health, %activator)
	{
		if (%this.getDataBlock() == nameToID("QuakePlayer"))
		{
			%this.health = mClampF(%health, -20, 200);

			if (isObject(%this.client))
			{
				commandToClient(%this.client, 'Quake_Health', %this.health);
			}

			if (%this.health > 100 && !isEventPending(%this.decayQuakeHealthTick))
			{
				%this.decayQuakeHealthTick = %this.schedule(1000, decayQuakeHealthTick);
			}

			if (%this.health <= 0)
			{
				%this.kill();
			}
		}
		else
		{
			Parent::setHealth(%this, %health, %activator);
		}
	}

	function Player::kill(%this, %activator)
	{
		if (%this.getDataBlock() == nameToID("QuakePlayer"))
		{
			%this.health = 0;
			%this.armor = 0;

			%this.damage(%this, %this.getHackPosition(), 1, $DamageType::Suicide);
		}
		else
		{
			Parent::kill(%this, %activator);
		}
	}

	function Player::setDamageFlash(%this, %intensity)
	{
		if (%this.getDataBlock() != nameToID("QuakePlayer"))
		{
			Parent::setDamageFlash(%this, %intensity);
		}
	}

	function GameConnection::setControlObject(%this, %control)
	{
		Parent::setControlObject(%this, %control);

		if (!isObject(%control) || %control.getClassName() !$= "Player" ||
			%control.getDataBlock() != nameToID("QuakePlayer")
		)
		{
			commandToClient(%this, 'Quake_GUI', 0);
			return;
		}

		commandToClient(%this, 'Quake_GUI', 1);
		commandToClient(%this, 'Quake_Health', %control.health);
		commandToClient(%this, 'Quake_Armor', %control.armor);
	}

	function GameConnection::spawnPlayer(%this)
	{
		Parent::spawnPlayer(%this);
		%obj = %this.player;

		if (!isObject(%obj))
		{
			return;
		}

		if (%obj.getDataBlock() != nameToID("QuakePlayer"))
		{
			return;
		}

		%obj.health = 125;
		%obj.armor = 0;

		commandToClient(%this, 'Quake_Health', %obj.health);
		commandToClient(%this, 'Quake_Armor', %obj.armor);
		commandToClient(%this, 'Quake_Blood', 0);

		if (isEventPending(%this.setQuakeBlood))
		{
			cancel(%this.setQuakeBlood);
		}

		%obj.decayQuakeHealthTick = %obj.schedule(1000, "decayQuakeHealthTick");
		%obj.decayQuakeArmorTick = %obj.schedule(1000, "decayQuakeArmorTick");

		serverCmdUseTool(%this, 0);
	}
};

activatePackage("QuakeInterfacePackage");

function QuakePlayer::damage(%this, %obj, %src, %pos, %dmg, %type)
{
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

			schedule(0, 0, eval, %sourceClient @ ".disallowHitSound = 0;");
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

		if (isObject(%client))
		{
			commandToClient(%client, 'Quake_Armor', %obj.armor);
		}
	}

	%obj.health -= %dmg;

	if (isObject(%client))
	{
		commandToClient(%client, 'Quake_Health', %obj.health);
		%client.setQuakeBlood(%client.lastBlood + %dmg);
	}

	if (%obj.health <= 0)
	{
		%obj.spawnTime = 0;

		// if ( %obj.health <= -20 )
		// {
		//  kill with gib;
		// }
		// else
		// {
				Parent::damage(%this, %obj, %src, %pos, %this.maxDamage, %type);
		// }
	}
	else
	{
		%obj.playPain();
	}
}

function gameConnection::setQuakeBlood(%this, %amt)
{
	cancel(%this.setQuakeBlood);
	%amt = mClampF(%amt, 0, 150);

	if (%amt <= 0)
	{
		commandToClient(%this, 'Quake_Blood', 0);
		%this.lastBlood = 0;

		return;
	}

	commandToClient(%this, 'Quake_Blood', %amt / 150);

	%this.lastBlood = %amt;
	%this.setQuakeBlood = %this.schedule(50, "setQuakeBlood", %amt - 3.75);
}

function Player::addArmor( %this, %armor)
{
	%this.setArmor(%this.armor + %armor);
}

function Player::setArmor(%this, %armor)
{
	%this.armor = mClamp(%armor, 0, 200);

	if (%this.client)
	{
		commandToClient(%this.client, 'Quake_Armor', %this.armor);
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
	}
	else
	{
		return;
	}

	if (%this.client)
	{
		commandToClient(%this.client, 'Quake_Health', %this.health);
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
	}
	else
	{
		return;
	}

	if (isObject(%this.client))
	{
		commandToClient(%this.client, 'Quake_Armor', %this.armor);
	}

	%this.decayQuakeArmorTick = %this.schedule(1000, decayQuakeArmorTick);
}