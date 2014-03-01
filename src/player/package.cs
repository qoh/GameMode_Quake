package QuakePlayerPackage
{
	function Player::playDeathCry(%this)
	{
		if (!%this.gibbed)
		{
			Parent::playDeathCry(%this);
		}
	}

	function Player::removeBody(%this)
	{
		if (%this.gibbed)
		{
			%this.delete();
		}
		else
		{
			Parent::removeBody(%this);
		}
	}
};

activatePackage("QuakePlayerPackage");