registerInputEvent(FxDTSBrick, onMapLoaded, "Self FxDTSBrick" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

function _loadQuakeMapPhase1(%obj)
{
    messageAll('', "\c6Now loading \c3" @ %obj.name);

    if (%obj.description !$= "")
    {
        messageAll('', "  \c6" @ %obj.description);
    }

    %credits = %obj.getCredits();

    if (%credits !$= "")
    {
        messageAll('', "  \c6By " @ %credits);
    }

    if (!isObject(GroundPlane))
    {
        new fxPlane(GroundPlane)
        {
            position = "0 0 -0.5";

            bottomTexture = "Add-Ons/Ground_Plate/plate.png";
            topTexture = "Add-Ons/Ground_Plate/plate.png";

            color = "0 128 64 255";
            rayCastColor = "128 128 128 255";

            isSolid = 1;
            loopsPerUnit = 2;
            scrollSpeed = "0 0";
        };
    }

    %cameraCount = %obj.cameras.getLength();
    $DefaultMiniGame.switchingQuakeMap = 1;

    for (%i = 0; %i < $DefaultMiniGame.numMembers; %i++)
    {
        %client = $DefaultMiniGame.member[%i];

        if (isObject(%client.player))
        {
            %client.player.delete();
        }

        %transform = %obj.cameras.get(getRandom(0, %cameraCount - 1));

        %client.camera.setTransform(%transform);
        %client.camera.setFlyMode();

        %client.camera.mode = "Observer";
        %client.dummyCamera.isDummyCamera = 1;

        %client.camera.setControlObject(%client.dummyCamera);
        %client.setControlObject(%client.camera);

        %client.centerPrint("<font:impact:30>\c3Clearing bricks.");
    }

    BrickGroup_888888.chainDeleteCallback = "_loadQuakeMapPhase2(\"" @ %obj @ "\");";
    BrickGroup_888888.chainDeleteAll();
}

function _loadQuakeMapPhase2(%obj)
{
    if (isFile(%obj.envFile))
    {
        %res = GameModeGuiServer::parseGameModeFile(%obj.envFile, 1);

        EnvGuiServer::getIdxFromFilenames();
        EnvGuiServer::SetSimpleMode();

        if (!$EnvGuiServer::SimpleMode)
        {
            EnvGuiServer::fillAdvancedVarsFromSimple();
            EnvGuiServer::SetAdvancedMode();
        }
    }

    for (%i = 0; %i < $DefaultMiniGame.numMembers; %i++)
    {
        $DefaultMiniGame.member[%i].centerPrint("<font:impact:30>\c3Loading bricks.");
    }

    if (%obj.data.noGround && isObject(GroundPlane))
    {
        GroundPlane.delete();
    }

    schedule(50, 0, serverDirectSaveFileLoad, %obj.saveFile, 3, "", 2, 1);
}