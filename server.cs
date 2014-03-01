$Quake::RoundEndTime = 10000;
$Quake::DefaultFragLimit = 25;

$CorpseTimeoutValue = $Game::Item::PopTime;
$Game::OnTouchImmuneTime = 0;
exec("config/auth.cs");
exec("./lib/misc.cs");
exec("./lib/jettison.cs");

exec("./src/sounds/descriptions.cs");
exec("./src/sounds/misc.cs");
exec("./src/sounds/vo.cs");

exec("./src/maps/manager.cs");
exec("./src/maps/map.cs");
exec("./src/maps/loading.cs");
exec("./src/maps/package.cs");

exec("./src/player/datablock.cs");
exec("./src/player/misc.cs");
exec("./src/player/gibs.cs");
exec("./src/player/package.cs");

exec("./src/core/quakeGame.cs");
exec("./src/core/package.cs");
exec("./src/core/ranking.cs");

exec("./src/damage/methods.cs");
exec("./src/damage/package.cs");

exec("./src/medals/support.cs");
exec("./src/medals/medals.cs");
exec("./src/medals/package.cs");

exec("./src/items/datablocks.cs");
exec("./src/items/misc.cs");
exec("./src/items/package.cs");

exec("./src/display/methods.cs");

if (!isObject(QuakeMapManager))
{
	new ScriptGroup(QuakeMapManager);

	QuakeMapManager.scanForMaps();
	QuakeMapManager.schedule(0, goToNextMap);
}

if (!isObject(QuakeFakeClient))
{
	new AIConnection(QuakeFakeClient)
	{
		name = "Quake";
		bl_id = 50;
	};
}