package QuakeItemsPackage
{
	function GameConnection::onDeath(%this, %player, %client, %type, %area)
	{
		if (%this.player.currTool != -1)
		{
			serverCmdDropTool(%this, %this.player.currTool);
		}

		Parent::onDeath(%this, %player, %client, %type, %area);
	}

	function GameConnection::spawnPlayer(%this)
	{
		Parent::spawnPlayer(%this);

		if (!isObject(%this.miniGame.quakeGame))
		{
			return;
		}

		serverCmdUseTool(%this, 0);
		commandToClient(%this, 'SetActiveTool', 0);

		%obj = %this.player;

		if (!isObject(%obj))
		{
			return;
		}

		%slots = %obj.getDataBlock().maxTools;

		for (%i = 5; %i < %slots; %i++)
		{
			messageClient(%this, 'MsgItemPickup', "", %i, 0);
		}
	}

	function ItemData::onAdd(%this, %obj)
	{
		Parent::onAdd(%this, %obj);

		if (!%this.noRotate)
		{
			%obj.rotate = 1;
		}

		%obj.schedule(0, createFxLight);
	}

	function ItemData::onRemove(%this, %obj)
	{
		if (isObject(%obj.light))
		{
			%obj.light.delete();
		}
	}

	function Item::fadeOut(%this)
	{
		Parent::fadeOut(%this);

		if (isObject(%this.light))
		{
			%this.light.setEnable(0);
		}
	}

	function Item::fadeIn(%this)
	{
		Parent::fadeIn(%this);

		if (isObject(%this.light))
		{
			%this.light.setEnable(1);
		}
	}

	function Weapon::onPickup(%this, %item, %obj)
	{
		if (!%this.isQuakeItem)
		{
			return Parent::onPickup(%this, %item, %obj);
		}

		return 0;
	}

	function ItemData::onCollision(%this, %obj, %col, %velocity, %speed)
	{
		if (!(%col.getType() & $TypeMasks::PlayerObjectType) || %col.getState() $= "Dead")
		{
			return Parent::onCollision(%this, %obj, %col, %velocity, %speed);
		}

		%dataBlock = %obj.getDataBlock();

		if (!%dataBlock.isQuakeItem)
		{
			return Parent::onCollision(%this, %obj, %col, %velocity, %speed);
		}

		if (isEventPending(%obj.respawn))
		{
			return;
		}

		%slots = %col.getDataBlock().maxTools;

		for (%i = 0; %i < %slots; %i++)
		{
			if (%col.tool[%i] == %dataBlock)
			{
				return;
			}
		}

		for (%i = 0; %i < %slots; %i++)
		{
			if (!isObject(%col.tool[%i]))
			{
				break;
			}
		}

		if (%i == %slots)
		{
			return;
		}

		%transform = %obj.getTransform();

		%obj.setTransform("0 0 -1000");
		%obj.respawn = %obj.schedule(15000, setTransform, %transform);

		%obj.fadeOut();
		%obj.schedule(15000, fadeIn);

		%col.tool[%i] = %dataBlock;

		if (isObject(%col.client))
		{
			messageClient(%col.client, 'MsgItemPickup', "", %i, %dataBlock);

			commandToClient(%col.client, 'SetActiveTool', %i);
			serverCmdUseTool(%col.client, %i);
		}

		serverPlay3D(QuakeWeaponPickup, %col.getHackPosition());
	}

	function serverCmdUnUseTool(%client)
	{
		if (!isObject(%client.miniGame.quakeGame))
		{
			Parent::serverCmdUnUseTool(%client);
		}
		else if (%client.player.currTool !$= "" && %client.player.currTool !$= -1)
		{
			schedule(0, 0, commandToClient, %client, 'SetActiveTool', %client.player.currTool);
		}
	}
};

activatePackage("QuakeItemsPackage");