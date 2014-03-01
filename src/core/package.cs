package QuakeCorePackage
{
	function MiniGameSO::onRemove(%this)
	{
		if (isObject(%this.quakeGame))
		{
			%this.quakeGame.delete();
		}

		Parent::onRemove(%this);
	}

	function MiniGameSO::addMember(%this, %client)
	{
		Parent::addMember(%this, %client);

		if (%this.owner == 0 && %this.numMembers && !isObject(%this.quakeGame))
		{
			%this.reset(0);
		}
	}

	function MiniGameSO::removeMember(%this, %client)
	{
		Parent::removeMember(%this, %client);

		if (!%this.numMembers && isObject(%this.quakeGame))
		{
			%this.quakeGame.delete();
		}
	}

	function MiniGameSO::reset(%this, %client)
	{
		Parent::reset(%this, %client);

		if (isObject(%this.quakeGame))
		{
			%this.quakeGame.delete();
			%existed = 1;
		}

		if (%this.numMembers && (%existed || %this.owner == 0)) {
			%this.quakeGame = QuakeGame(%this);
		}
	}

	function GameConnection::setScore(%this, %score)
	{
		Parent::setScore(%this, %score);
		%miniGame = getMiniGameFromObject(%this);

		if (isObject(%miniGame.quakeGame))
		{
			%miniGame.quakeGame.onFragsChanged(%this, %score);
		}
	}

	function GameConnection::onDeath(%this, %player, %client, %type, %area)
	{
		Parent::onDeath(%this, %player, %client, %type, %area);

		if (!isObject(%client) || %client == %this)
		{
			return;
		}

		if (%type == $DamageType::Gauntlet)
		{
			%this.play2D(QuakeVO_Humiliation);
			%client.play2D(QuakeVO_Humiliation);
		}

		%miniGame = getMiniGameFromObject(%client);

		if (!isObject(%miniGame.quakeGame))
		{
			return;
		}

		if (%miniGame.quakeGame.hasEnded())
		{
			return;
		}

		%data = findClientInSortedScores(%miniGame.quakeGame.scores, %client);

		if (%data == -1)
		{
			echo(%miniGame.quakeGame.scores);
			return;
		}

		%rank = getWord(%data, 0);
		%tied = getWord(%data, 1);
		%score = getWord(%data, 2);

		%style = "<font:impact:45><shadow:2:2>";

		%fragLine = "\c6You fragged" SPC %this.name;
		%rankLine = "\c6";

		if (%tied)
		{
			%rankLine = %rankLine @ "Tied for ";
		}

		%rankLine = %rankLine @ "\c1" @ formatRank(%rank) SPC "\c6with" SPC %score;
		%client.displayPoints(%rankLine, "<font:palatino linotype:30>" @ %fragLine @ "\n");
		//%client.centerPrint(%style @ %fragLine NL %rankLine, 4);
	}

	function ProjectileData::damage(%this, %obj, %col, %fade, %pos, %normal)
	{
		%obj.directHit[%obj] = 1;
		Parent::damage(%this, %obj, %col, %fade, %pos, %normal);
	}

	function ProjectileData::radiusDamage(%this, %obj, %col, %factor, %pos, %damage)
	{
		if (!%obj.directHit[%obj])
		{
			Parent::radiusDamage(%this, %obj, %col, %factor, %pos, %damage);
		}
	}
};

activatePackage("QuakeCorePackage");