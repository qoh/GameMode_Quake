function registerMedal(%name, %sound, %bitmap)
{
	if (!$Quake::IsMedal[%name])
	{
		$Quake::IsMedal[%name] = 1;

		$Quake::Medal[$Quake::MedalCount] = %name;
		$Quake::MedalCount++;
	}

	$Quake::MedalSound[%name] = %sound;
	$Quake::MedalBitmap[%name] = %bitmap;
}

function GameConnection::assignMedal(%this, %name)
{
	if (!$Quake::IsMedal[%name])
	{
		return;
	}

	if (isObject($Quake::MedalSound[%name]))
	{
		%this.play2D($Quake::MedalSound[%name]);
	}

	%message = "<font:impact:64>\n\n\n\n\c3" @ strUpr(%name);

	if ($Quake::MedalBitmap[%name] !$= "")
	{
		%message = %message NL "<bitmap:" @ $Quake::MedalBitmap[%name] @ ">";
	}

	%this.centerPrint(%message, 3);
}