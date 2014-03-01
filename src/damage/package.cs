package QuakeDamagePackage
{
	function Player::damage(%this, %a, %b, %c, %d, %e)
	{
		if (isObject(%this.client) && !isObject(%this.client.miniGame))
		{
			return;
		}

		Parent::damage(%this, %a, %b, %c, %d, %e);
	}

	function Player::setHealth(%this, %health, %activator)
	{
		if (%this.getDataBlock() == nameToID("QuakePlayer"))
		{
			%this.health = mClampF(%health, -20, 200);

			if (%this.health > 100 && !isEventPending(%this.decayQuakeHealthTick))
			{
				%this.decayQuakeHealthTick = %this.schedule(1000, decayQuakeHealthTick);
			}

			if (%this.health <= 0)
			{
				%this.kill();
			}

			if (isObject(%this.client))
			{
				%this.client.updateQuakeDisplay();
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

	function GameConnection::spawnPlayer(%this)
	{
		Parent::spawnPlayer(%this);
	}

	function ProjectileData::radiusDamage(%this, %obj, %col, %factor, %pos, %damage)
	{
		%factor *= calcExplosionCoverage(%pos, %obj, $TypeMasks::FxBrickObjectType);
		Parent::radiusDamage(%this, %obj, %col, %factor, %pos, %damage);
	}

	function ProjectileData::radiusImpulse(%this, %obj, %col, %factor, %pos, %force)
	{
		if (obstructRadiusDamageCheck(%pos, %col))
		{
			Parent::radiusImpulse(%this, %obj, %col, %factor, %pos, %force);
		}
	}
};

activatePackage("QuakeDamagePackage");