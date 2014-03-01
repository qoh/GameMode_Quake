function MiniGameSO::play2D(%this, %profile)
{
	if (!isObject(%profile))
	{
		return;
	}

	for (%i = 0; %i < %this.numMembers; %i++)
	{
		%this.member[%i].play2D(%profile);
	}
}

function fxDtsBrick::hasInputEvent(%this,%input)
{
	for(%i = 0; %i < %this.numEvents; %i ++)
	{
		if(%this.eventInput[%i] $= %input)
			return 1;
	}

	return 0;
}

function spaceListContains(%list, %item)
{
	%count = getWordCount(%list);

	for (%i = 0; %i < %count; %i++)
	{
		if (getWord(%list, %i) $= %item)
		{
			return 1;
		}
	}

	return 0;
}

function naturalGrammarList(%list) {
	%fields = getFieldCount(%list);

	if (%fields < 2) {
			return %list;
	}

	for (%i = 0; %i < %fields - 1; %i++) {
			%partial = %partial @ (%i ? ", " : "") @ getField(%list, %i);
	}

	return %partial SPC "and" SPC getField(%list, %fields - 1);
}