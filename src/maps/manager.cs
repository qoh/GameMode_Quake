// This is a comment.

function QuakeMapManager::scanForMaps(%this)
{
    %currMapName = %this.currMap.name;

    %this.currMap = "";
    %this.deleteAll();

    %pattern = "Add-Ons/Quake_*/map.json";
    %fp = new FileObject();

    for (%file = findFirstFile(%pattern); %file !$= ""; %file = findNextFile(%pattern))
    {
        %path = filePath(%file) @ "/";

        if (!isFile(%path @ "save.bls"))
        {
            warn("Warning: Map '" @ %file @ "' does not have a save file.");
            continue;
        }

        %json = loadJSON(%file, "", %fp);

        if (getJSONType(%json) !$= "hash")
        {
            if (isJSONObject(%json))
            {
                %json.delete();
            }

            warn("Warning: Invalid JSON (must be hash) in map '" @ %file @ "'.");
            continue;
        }

        %name = trim(%json.name);

        if (%name $= "")
        {
            warn("Warning: Map file '" @ %file @ "' has an invalid name, skipping.");
            %json.delete();
            continue;
        }

        %obj = %this.findMapByName(%name);

        if (isObject(%obj))
        {
            warn("Warning: Map '" @ %name @ "' (from '" @ %file @ "'') is already defined in '" @ %obj.mapFile @ "', skipping");
            %json.delete();
            continue;
        }

        %obj = new ScriptObject()
        {
            class = QuakeMap;
            data = %json;

            name = %name;
            description = %json.description;

            authors = %json.authors;
            cameras = %json.cameras;

            mapFile = %file;
            saveFile = %path @ "save.bls";
            envFile = %path @ "env.txt";
        };

        %this.add(%obj);

        %json.remove("authors", 1);
        %json.remove("cameras", 1);

        if (%currMapName $= %name)
        {
            %this.currMap = %obj;
            %currMapName = "";
        }
    }

    %fp.delete();

    if (!%this.warnedNoMaps && !%this.getCount())
    {
        %this.warnedNoMaps = 1;

        messageAll('', "\c0ERROR\c6: No Quake maps found!");
        warn("ERROR: No Quake maps found!");
    }
}

function QuakeMapManager::findMapByName(%this, %name)
{
    %count = %this.getCount();

    for (%i = 0; %i < %count; %i++)
    {
        %obj = %this.getObject(%i);

        if (%obj.name $= %name)
        {
            return %obj;
        }
    }

    return 0;
}

function QuakeMapManager::getNextMap(%this)
{
    %count = %this.getCount();

    if (!%count)
    {
        return 0;
    }

    for (%i = 0; %i < %count; %i++)
    {
        if (%this.getObject(%i) == %this.currMap)
        {
            break;
        }
    }

    return %this.getObject((%i + 1) % %count);
}

function QuakeMapManager::goToNextMap(%this)
{
    %obj = %this.getNextMap();

    if (isObject(%obj))
    {
        %this.setMap(%obj);
    }
}

function QuakeMapManager::setMap(%this, %obj)
{
    if (!isObject(%obj) || !%this.isMember(%obj))
    {
        error("ERROR: Invalid map.");
        return;
    }

    if ($DefaultMiniGame.switchingQuakeMap)
    {
        return;
    }

    %this.currMap = %obj;
    _loadQuakeMapPhase1(%obj);
}