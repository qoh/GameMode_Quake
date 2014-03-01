package QuakeMedalPackage
{
	function GameConnection::onDeath(%this, %player, %client, %type, %area)
	{
		Parent::onDeath(%this, %player, %client, %type, %area);

		if (!isObject(%client) || %client == %this)
		{
			return;
		}

		if ($Sim::Time - %player.lastKillTime <= 2)
		{
			%client.assignMedal("Excellent");
		}

		%player.lastKillTime = $Sim::Time;

		if (%type == $DamageType::Gauntlet)
		{
			%client.assignMedal("Gauntlet");
			%this.play2D(QuakeMedal_Gauntlet);
		}
	}

	function GameConnection::setScore(%this, %score)
	{
		if (isObject(%this.miniGame.quakeGame) && %score > 0 && %score % 100 == 0)
		{
			%this.assignMedal("Frags");
		}

		Parent::setScore(%this, %score);
	}
};

activatePackage("QuakeMedalPackage");