function MiniGameSO::getSortedScores(%this)
{
	%count = 0;

	for (%i = 0; %i < %this.numMembers; %i++)
	{
		%client = %this.member[%i];
		%score = mFloor(%client.score);

		if (%holders[%score] $= "")
		{
			%rank[%count] = %score;
			%count++;
		}

		%delim = %holders[%score] $= "" ? "" : " ";
		%holders[%score] = %holders[%score] @ %delim @ %client;
	}

	for (%i = 1; %i <= %count && !%stop; %i++)
	{
		%stop = 1;

		for (%j = 0; %j < %count - 1; %j++)
		{
			if (%rank[%j + 1] > %rank[%j])
			{
				%temp = %rank[%j];
				%stop = 0;

				%rank[%j] = %rank[%j + 1];
				%rank[%j + 1] = %temp;
			}
		}
	}

	for (%i = 0; %i < %count; %i++)
	{
		if (%i)
		{
			%string = %string @ "\n";
		}

		%string = %string @ %rank[%i] TAB %holders[%rank[%i]];
	}

	return %string;
}

function findClientInSortedScores(%scores, %client)
{
	%recordCount = getRecordCount(%scores);

	for (%i = 0; %i < %recordCount; %i++)
	{
		%record = getRecord(%scores, %i);
		%holders = getField(%record, 1);

		%holderCount = getWordCount(%holders);

		for (%j = 0; %j < %holderCount; %j++)
		{
			if (getWord(%holders, %j) == %client)
			{
				return %i SPC (%holderCount > 1) SPC getField(%record, 0);
			}
		}
	}

	return -1;
}

function formatRank(%index)
{
	%suffix[1] = "st";
	%suffix[2] = "nd";
	%suffix[3] = "rd";

	%last = getSubStr(%index++, strLen(%index) - 1, 1);
	return %index @ (%suffix[%last] $= "" ? "th" : %suffix[%last]);
}

function GameConnection::getRank(%this)
{
	if (!isObject(%client) || %client == %this)
	{
		return 0;
	}

	%miniGame = getMiniGameFromObject(%client);

	if (!isObject(%miniGame.quakeGame))
	{
		return 0;
	}

	return getWord(findClientInSortedScores(%miniGame.quakeGame.scores, %this), 0);
}