function QuakeGame(%miniGame)
{
	if (!isObject(%miniGame) || %miniGame.class !$= MiniGameSO)
	{
		error("ERROR: Invalid mini-game.");
		return 0;
	}

	return new ScriptObject()
	{
		class = QuakeGame;
		miniGame = %miniGame;
	};
}

function QuakeGame::onAdd(%this)
{
	if (%this.fragLimit $= "")
	{
		%this.fragLimit = $Quake::DefaultFragLimit;
	}

	%this.maxFrags = 0;
	%this.scores = %this.miniGame.getSortedScores();
}

function QuakeGame::onRemove(%this)
{
	cancel(%this.resetGameSchedule);
}

function QuakeGame::hasEnded(%this)
{
	return isEventPending(%this.resetGameSchedule);
}

function QuakeGame::end(%this, %time)
{
	if (%this.hasEnded())
	{
		return;
	}

	%this.maxFrags = 0;

	%scoreboard = "<font:impact:50>\c6ROUND ENDED\n<font:impact:30>";
	%scoreboard = %scoreboard NL "<just:left>\c3RANK<just:center>\c3PLAYERS<just:right>\c3FRAGS";

	%recordCount = getRecordCount(%this.scores);

	for (%i = 0; %i < 3 && %i < %recordCount; %i++)
	{
		%record = getRecord(%this.scores, %i);
		%names = "";

		%score = getField(%record, 0);
		%holders = getField(%record, 1);

		%count = getWordCount(%holders);
		
		for (%j = 0; %j < %count; %j++)
		{
			%names = %names @ (%j ? "\t" : "") @ getWord(%holders, %j).name;
		}

		%scoreboard = %scoreboard NL "<just:left>\c3" @ formatRank(%i);
		%scoreboard = %scoreboard @ "<just:center>\c6" @ naturalGrammarList(%names);
		%scoreboard = %scoreboard @ "<just:right>\c6" @ %score;
	}

	for (%i = 0; %i < %this.miniGame.numMembers; %i++)
	{
		%client = %this.miniGame.member[%i];

		commandToClient(%client, 'ClearBottomPrint');
		schedule(500, 0, commandToClient, %client, 'CenterPrint', %scoreboard);
	}

	if (%time $= "")
	{
		%time = $Quake::RoundEndTime;
	}

	//%this.resetGameSchedule = %this.miniGame.schedule(%time, reset, 0);
	%this.resetGameSchedule = QuakeMapManager.schedule(%time, goToNextMap, 0);
}

function QuakeGame::onFragsChanged(%this, %client, %frags)
{
	if (%this.hasEnded())
	{
		return;
	}

	if (%frags >= %this.fragLimit)
	{
		%this.miniGame.chatMessageAll("\x11\c3~win win win~");
		%this.scores = %this.miniGame.getSortedScores();

		%this.end();
		return;
	}

	%frags = mClamp(%frags, 0, %this.fragLimit);

	if (%frags > %this.maxFrags)
	{
		%this.maxFrags = %frags;
		%delta = %this.fragLimit - %frags;

		if (%delta >= 1 && %delta <= 3)
		{
			%this.miniGame.play2D(nameToID("QuakeVO_FragsLeft_" @ %delta));
		}
	}

	%this.updateLeaders();
}

function QuakeGame::updateLeaders(%this)
{
	%a = %this.scores;
	%b = %this.scores = %this.miniGame.getSortedScores();

	%a = getField(getRecord(%a, 0), 1);
	%b = getField(getRecord(%b, 0), 1);

	%countA = getWordCount(%a);
	%countB = getWordCount(%b);

	%tieA = %countA > 1;
	%tieB = %countB > 1;

	for (%i = 0; %i < %countA; %i++)
	{
		%client = getWord(%a, %i);

		if (!spaceListContains(%b, %client))
		{
			%client.play2D(QuakeVO_PlayerLostLead);
		}
	}

	for (%i = 0; %i < %countB; %i++)
	{
		%client = getWord(%b, %i);
		
		if (!spaceListContains(%a, %client))
		{
			%client.play2D(%tieB ? QuakeVO_PlayerTiedLead : QuakeVO_PlayerTookLead);
			continue;
		}

		if (%tieA != %tieB)
		{
			if (%tieB)
			{
				%client.play2D(QuakeVO_PlayerTiedLead);
			}
			else
			{
				%client.play2D(QuakeVO_PlayerTookLead);
			}
		}
	}

	%recordCount = getRecordCount(%this.scores);
	%score1 = getField(getRecord(%this.scores, 0), 0);

	if (%recordCount)
	{
		%score2 = getField(getRecord(%this.scores, 1), 0);
	}

	for (%i = 0; %i < %this.miniGame.numMembers; %i++)
	{
		%this.miniGame.member[%i].updateQuakeDisplay();
	}
}