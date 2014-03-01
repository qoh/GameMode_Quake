package QuakeMapsPackage
{
	function serverLoadSaveFile_Tick()
	{
		Parent::serverLoadSaveFile_Tick();

		%count = $LoadingBricks_BrickGroup.getCount();
		%index = %count - $FxBrick::ChainBatchSize < 0 ? 0 : %count - $FxBrick::ChainBatchSize;

		//echo("tick from" SPC %index SPC "to" SPC %count);

		for (%i = %index; %i < %count; %i++)
		{
			%brick = $LoadingBricks_BrickGroup.getObject(%i);

			if (%brick.hasInputEvent("onMapLoaded") && !spaceListContains($LoadingBricks_EventTargets, %brick))
			{
				$LoadingBricks_EventTargets = setWord(
					$LoadingBricks_EventTargets,
					getWordCount($LoadingBricks_EventTargets),
					%brick
				);
			}
		}
	}

	function ServerLoadSaveFile_End()
	{
		Parent::ServerLoadSaveFile_End();
		%count = getWordCount($LoadingBricks_EventTargets);

		for (%i = 0; %i < %count; %i++)
		{
			%brick = getWord($LoadingBricks_EventTargets, %i);
			%client = %brick.client;

			if (!isObject(%client))
			{
				%client = QuakeFakeClient;

				if (!isObject(%client))
				{
					continue;
				}
			}

			$InputTarget_["Self"] = %brick;
			$InputTarget_["Player"] = %client.player;
			$InputTarget_["Client"] = %client;
			$InputTarget_["MiniGame"] = $DefaultMiniGame;

			%brick.processInputEvent("onMapLoaded", %client);
		}

		$LoadingBricks_EventTargets = "";

		if ($DefaultMiniGame.switchingQuakeMap && $DefaultMiniGame.numMembers)
		{
			$DefaultMiniGame.scheduleReset();
		}

		$DefaultMiniGame.switchingQuakeMap = 0;
	}

	function Observer::onTrigger(%this, %obj, %slot, %state)
	{
		if (!%obj.isDummyCamera && !%obj.getControlObject().isDummyCamera)
		{
			Parent::onTrigger(%this, %obj, %slot, %state);
		}
	}

	function GameConnection::spawnPlayer(%this)
	{
		Parent::spawnPlayer(%this);

		if (isObject(%this.player) && QuakeMapManager.currMap.data.noGround)
		{
			%this.player.belowGroundCheck();
		}
	}
};

activatePackage("QuakeMapsPackage");

function Player::belowGroundCheck(%this)
{
	cancel(%this.belowGroundCheck);

	if (%this.getState() $= "Dead" || !isObject(%this.client))
	{
		return;
	}

	if (getWord(%this.getPosition(), 2) < -20)
	{
		%this.schedule(10000, delete);

		%this.client.player = new AIPlayer()
		{
			datablock = PlayerStandardArmor;
			client = %this.client;
		};

		%this.client.player.schedule(0, delete);
		%this.client.player.kill();

		%rotation = "1 0 0" SPC $pi / 2;

		%this.client.camera.setFlyMode();
		%this.client.camera.setTransform(%this.getPosition() SPC %rotation);
		%this.client.camera.setControlObject(%this);

		%this.client.setControlObject(%this.client.camera);
		return;
	}

	%this.belowGroundCheck = %this.schedule(1000, belowGroundCheck);
}