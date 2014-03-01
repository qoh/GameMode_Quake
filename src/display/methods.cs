function GameConnection::updateQuakeDisplay(%this)
{
	if (!isObject(%this.miniGame.quakeGame))
	{
		%this.clearBottomPrint();
		return;
	}

	%text = "<font:palatino linotype:30>";

	if (isObject(%this.player) && %this.player.getState() !$= "Dead")
	{
		%obj = %this.player;
		%text = %text @ "\c6HEALTH ";

		if (%obj.health > 100)
		{
			%text = %text @ "<color:AABBAA>";
		}
		else if (%obj.health > 40)
		{
			%text = %text @ "<color:AAAA55>";
		}
		else
		{
			%text = %text @ "<color:AA5555>";
		}

		%text = %text @ mCeil(%obj.health);

		if (%obj.armor)
		{
			%text = %text @ "      \c6ARMOR <color:AABBAA>" @ mCeil(%obj.armor);
		}
	}

	if (!%this.miniGame.quakeGame.hasEnded())
	{
		%scores = %this.miniGame.quakeGame.scores;

		%recordCount = getRecordCount(%scores);
		%score1 = getField(getRecord(%scores, 0), 0);

		if (%recordCount)
		{
			%score2 = getField(getRecord(%scores, 1), 0);
		}

		%text = %text @ "<just:right>\c6FRAGS \c2" @ %this.miniGame.quakeGame.fragLimit;

		if (%this.score < %score1)
		{
			%text = %text SPC "\c6" @ %score1;
			%text = %text SPC "\c0" @ %client.score;
		}
		else
		{
			%text = %text SPC "\c0" @ %this.score;

			if (%recordCount)
			{
				%text = %text SPC "\c6" @ %score2;
			}
		}

		%text = %text @ " ";
	}

	%this.bottomPrint(%text, 0, 1);
}

function GameConnection::clearBottomPrint(%this)
{
	commandToClient(%this, 'ClearBottomPrint');
}