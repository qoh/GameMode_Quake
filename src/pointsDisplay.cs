function GameConnection::displayPoints(%this, %points, %style, %step)
{
	cancel(%this.displayPoints);
	%step = mFloor(%step);

	if (%step > 10)
	{
		return;
	}

	if (%points + 0 $= %points)
	{
		%text = mCeil(%points * (%step / 10));

		if (%text >= 0)
		{
			%text = "+" @ %text;
		}
	}
	else
	{
		%text = getSubStr(%points, 0, mCeil(strLen(%points) * (%step / 10)));
	}

	%text = "<color:00" @ repeatString(getHexChar(%step + 5), 2) @ "00>" @ %text;
	%bars = %step >= 3;

	if (%step == 4)
	{
		%bars++;
	}

	if (%step >= 6)
	{
		%bars++;
	}

	if (%step >= 8)
	{
		%bars++;
	}

	if (%bars)
	{
		%barColor = "<color:" @ repeatString((%step - 1) @ "0", 3) @ ">";
		%barChars = %barColor @ repeatString("-", %bars);

		%text = %barChars SPC %text SPC %barChars;
	}

	%this.centerPrint(%style @ %text, 2);
	%this.displayPoints = %this.schedule(50, displayPoints, %points, %style, %step++);
}

function repeatString(%text, %n)
{
	for (%i = 0; %i < %n; %i++)
	{
		%result = %result @ %text;
	}

	return %result;
}

function getHexChar(%num)
{
	return getSubStr("0123456789ABCDEF", %num, 1);
}
