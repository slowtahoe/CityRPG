// City_OnInitData(obj) - Called when CityRPGData is initialized.
// obj - The CityRPGData object.
// Package this to add new values to the saver.
function City_OnDataInit(%obj) { }

// City_Init()
// Initializes the game-mode.
function City_Init()
{
	if(!isObject(City))
	{
		// New object reference
		new scriptObject(City) {};
	}

	if(!isObject(JobSO))
	{
		new scriptObject(JobSO) { };
		JobSO.populateJobs();
	}

	if(!isObject(CityRPGData))
	{
		// Deprecated object reference
		new scriptObject(CityRPGData)
		{
			class = Sassy;
			dataFile = $City::SavePath @ "Profiles.dat";
		};

		if(!isObject($DamageType::Starvation))
			AddDamageType("Starvation", '%1 starved', '%1 starved', 0.5, 0);

		// Since the active values change so often, we'll re-attempt to add them each time.
		CityRPGData.addValue("bank", 0);
		CityRPGData.addValue("bounty", "0");
		CityRPGData.addValue("demerits", "0");
		CityRPGData.addValue("education", "0");
		CityRPGData.addValue("gender", "Male");
		CityRPGData.addValue("hunger", "7");
		CityRPGData.addValue("jailData", "0 0");
		CityRPGData.addValue("jobID", "StarterCivilian");
		CityRPGData.addValue("jobRevert", "0");
		CityRPGData.addValue("lotData", "0");
		CityRPGData.addValue("money", "0");
		CityRPGData.addValue("name", "noName");
		CityRPGData.addValue("outfit", "none none none none whitet whitet skin bluejeans blackshoes");
		CityRPGData.addValue("reincarnated", "0");
		CityRPGData.addValue("resources", "0 0");
		CityRPGData.addValue("student", "0");
		CityRPGData.addValue("ElectionID", "0");
		CityRPGData.addValue("lotsVisited", "-1");
		CityRPGData.addValue("spawnPoint", "");

		City_OnDataInit(CityRPGData);
		
		if(CityRPGData.loadedSaveFile)
		{
			for(%a = 1; %a <= CityRPGData.dataCount; %a++)
			{
				if(JobSO.job[CityRPGData.data[%a].valueJobID] $= "")
				{
					CityRPGData.data[%a].valueJobID = $City::CivilianJobID;
				}
			}
		}

		// These two need to be done in this order to work properly.
		City_Init_Items();
		City_Init_AssembleEvents();

		CalendarSO.date = 0;
		CityRPGData.lastTickOn = $Sim::Time;
		CityRPGData.scheduleTick = schedule($Pref::Server::City::tick::speed * 60000, false, "City_Tick");
		$City::ClockStart = getSimTime();

		City_BottomPrintLoop();
	}

	// Generic client to handle checks for external utilities as the host.
	if(!isObject(CityRPGHostClient))
	{
		new ScriptObject(CityRPGHostClient)
		{
			isSuperAdmin = 1;
		};
	}

	// Generic client to run events such as spawnProjectile. See: minigameCanDamage
	if(!isObject(CityRPGEventClient))
	{
		new ScriptObject(CityRPGEventClient)
		{
		};
	}

	if(!isObject(CityRPGMini))
	{
		City_Init_Minigame();
	}

	CityMayor_refresh();

	activatePackage("CityRPG_Overrides");

	echo("CityRPG initialization complete.");
}

function CityRPGHostClient::onBottomPrint(%this, %message)
{
	return;
}

function City::get(%this, %profileID, %key)
{
	return CityRPGData.getData(%profileID).value[%key];
}

function City::set(%this, %profileID, %key, %value)
{
	CityRPGData.getData(%profileID).value[%key] = %value;
}

function City::add(%this, %profileID, %key, %value)
{
	CityRPGData.getData(%profileID).value[%key] = CityRPGData.getData(%profileID).value[%key] + %value;
}

function City::subtract(%this, %profileID, %key, %value)
{
	CityRPGData.getData(%profileID).value[%key] = CityRPGData.getData(%profileID).value[%key] - %value;
}

function City::keyExists(%this, %profileID)
{
	return CityRPGData.getData(%profileID) != 0;
}

// City_Init_Minigame()
// Creates the minigame for the game-mode.
function City_Init_Minigame()
{
	loadMayor();

	if(isObject(CityRPGMini))
	{
		for(%i = 0; %i < ClientGroup.getCount(); %i++)
		{
			%subClient = ClientGroup.getObject(%i);
			CityRPGMini.removeMember(%subClient);
		}
		CityRPGMini.delete();
	}
	else
	{
		for(%i = 0; %i < ClientGroup.getCount(); %i++)
		{
			%subClient = ClientGroup.getObject(%i);
			%subClient.minigame = NULL;
		}
	}

	new scriptObject(CityRPGMini)
	{
		class = miniGameSO;

		brickDamage = true;
		brickRespawnTime = 10000;
		colorIdx = -1;

		enableBuilding = true;
		enablePainting = true;
		enableWand = true;
		fallingDamage = true;
		inviteOnly = false;

		points_plantBrick = 0;
		points_breakBrick = 0;
		points_die = 0;
		points_killPlayer = 0;
		points_killSelf = 0;
		playerDatablock = Player9SlotPlayer;
		respawnTime = 5;
		selfDamage = true;

		playersUseOwnBricks = false;
		useAllPlayersBricks = true;
		useSpawnBricks = false;
		VehicleDamage = true;
		vehicleRespawnTime = 10000;
		weaponDamage = true;

		numMembers = 0;
		vehicleRunOverDamage = false;
	};
	for(%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%subClient = ClientGroup.getObject(%i);
		CityRPGMini.addMember(%subClient);
	}
}

// City_Init_Spawns()
// Records the spawn points for player spawning. Called on init and every tick.
// This will be optimized in the future.
function City_Init_Spawns()
{
	if($CityRPG::BuildingSpawns)
	{
		warn("City: Already building spawns, skipping init...");
		return;
	}

	if(mainBrickGroup.getCount()<1)
		return;

	$CityRPG::BuildingSpawns = 1;
	$CityRPG::temp::spawnPointsTemp = "";

	$CityRPG::BuildSpawnsSched = schedule(206,mainBrickGroup,"City_Init_Spawns_Tick",0,0);
}

function City_Init_Spawns_Tick(%bgi, %bi)
{
	cancel($CityRPG::BuildSpawnsSched);
	%mbgc = mainBrickGroup.getCount();
	for(%bgi;%bgi<%mbgc;%bgi++)
	{
		%bg = mainBrickGroup.getObject(%bgi);
		%bgc = %bg.getCount();
		for(%bi;%bi<%bgc;%bi++)
		{
			%b = %bg.getObject(%bi);
			if(%b.getDatablock().CityRPGBrickType == $CityBrick_Spawn)
			{
				$CityRPG::temp::spawnPointsTemp = (!$CityRPG::temp::spawnPointsTemp ? %b : $CityRPG::temp::spawnPointsTemp SPC %b);
			}
			%sc++;
			if(%sc>=1000)
			{
				$CityRPG::BuildSpawnsSched = schedule(206,mainBrickGroup,"City_Init_Spawns_Tick",%bgi,%bi);
				return;
			}
		}
		%bi=0;
	}
	echo("City: Built CityRPG Spawns");
	$CityRPG::BuildingSpawns = 0;
	$CityRPG::temp::spawnPoints = $CityRPG::temp::spawnPointsTemp;
}

function City_RegisterItem(%datablock, %cost, %mineral, %itemRestrictionLevel) {
	if(!isObject(%datablock)) {
		warn("GameMode_CityRPG4 - Attempting to register nonexistent item '" @ %datablock @ "'. This might indicate one of your add-ons is a version not supported by CityRPG 4. This item will not be purchase-able.");
		return;
	}

	$City::Item::name[$City::ItemCount] = %datablock;
	$City::Item::price[$City::ItemCount] = %cost;
	$City::Item::mineral[$City::ItemCount] = %mineral;
	$City::Item::isRestrictedItem[$City::ItemCount] = %itemRestrictionLevel > 0;
	$City::Item::restrictionLevel[$City::ItemCount] = %itemRestrictionLevel;
	$City::ItemCount++;
}

function City_Init_Items()
{
	// Weapon Prices
	$City::ItemCount = 0;

	// CityRPG Stuff
	City_RegisterItem(CityRPGLBItem, 100, 1, 1);
	City_RegisterItem(CityRPGPickaxeItem, 25, 1, 0);
	City_RegisterItem(CityRPGJackhammerItem, 25, 1, 0);
	City_RegisterItem(CityRPGLumberjackItem, 25, 1, 0);
	City_RegisterItem(CityRPGChainsawItem, 25, 1, 0);
	City_RegisterItem(taserItem, 40, 1, 1);

	// Default weapons
	if(!$Pref::Server::City::disabledefaultweps)
	{
		City_RegisterItem(gunItem, 80, 1, 1);
		City_RegisterItem(akimboGunItem, 150, 1, 2);
	}

	if(isObject(shotgunItem))
		City_RegisterItem(shotgunItem, 260, 1, 2);

	if(isObject(sniperRifleItem))
		City_RegisterItem(sniperRifleItem, 450, 1, 2);

	// Weapon support: Any T+T ammo system weapons
	if($Pref::Server::TT::Ammo !$= "")
	{
		// We don't use ammo currently, so we need to handle that.
		// Configure this if we don't have a server control mod
		if(!$RTB::Hooks::ServerControl)
			$Pref::Server::TT::Ammo = 1;
	}

	// Weapon support: Weapon_Package_Tier1
	if(!$Pref::Server::TT::DisableTier1 && isObject(TTLittleRecoilExplosion) && isObject(SubmachineGunItem))
	{
		City_RegisterItem(SubmachineGunItem, 150, 1, 2);
		City_RegisterItem(PumpShotgunItem, 200, 1, 2);
		City_RegisterItem(PistolItem, 80, 1, 1);
		City_RegisterItem(AkimboPistolItem, 160, 1, 2);
	}

	// Weapon support: Weapon_Package_Tier1A
	if(isObject(SingleShotgunItem))
	{
		City_RegisterItem(SingleShotgunItem, 80, 1, 1);
		City_RegisterItem(SnubnoseItem, 150, 1, 2);
		City_RegisterItem(PepperPistolItem, 80, 1, 2);

		if($Pref::Server::TT::EasterEgg)
			City_RegisterItem(nailgunItem, 150, 1, 2);
	}

	// Weapon support: Weapon_Package_Tier2
	if(isObject(TAssaultRifleItem))
	{
		City_RegisterItem(TAssaultRifleItem, 200, 1, 2);
		City_RegisterItem(BattleRifleItem, 180, 1, 2);
		City_RegisterItem(MagnumItem, 150, 1, 2);
		City_RegisterItem(MilitarySniperItem, 200, 1, 2);
		City_RegisterItem(CombatShotgunItem, 300, 1, 2);

		if($Pref::Server::TT::EasterEgg)
			City_RegisterItem(ScopedMagnumItem, 500, 1, 2);
	}

	// Weapon support: Weapon_Package_Tier2A
	if(isObject(BullpupItem))
	{
		City_RegisterItem(BullpupItem, 200, 1, 1);
		City_RegisterItem(TCrossbowItem, 100, 1, 2);
		City_RegisterItem(MachstilItem, 150, 1, 2);
		City_RegisterItem(DualSMGsItem, 300, 1, 2);

		if($Pref::Server::TT::EasterEgg)
			City_RegisterItem(MatchPistolItem, 200, 1, 2);
	}

	// Ghost's Mail Mod Support: System_Mail
	if(isObject(MlbxCardItem))
	{
		City_RegisterItem(MlbxLetterItem, 5, 1, 0);
		City_RegisterItem(MlbxCardItem, 10, 1, 0);
		City_RegisterItem(MlbxNoteItem, 1, 1, 0);
	}

	// Tool_ToolGun
	if(isObject(toolgunItem))
	{
		$Pref::Server::City::defaultTools = "toolgunItem";
	}
}

// City_Init_AssembleEvents()
// Initializes events for the game.
function City_Init_AssembleEvents()
{
	// Basic Input
	registerInputEvent("fxDTSBrick", "onLotEntered", "Self fxDTSBrick" TAB "Player player" TAB "Client gameConnection");
	registerInputEvent("fxDTSBrick", "onLotLeft", "Self fxDTSBrick" TAB "Player player" TAB "Client gameConnection");
	registerInputEvent("fxDTSBrick", "onLotFirstEntered", "Self fxDTSBrick" TAB "Player player" TAB "Client gameConnection");
	registerInputEvent("fxDTSBrick", "onTransferSuccess", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection");
	registerInputEvent("fxDTSBrick", "onTransferDecline", "Self fxDTSBrick" TAB "Client GameConnection");
	registerInputEvent("fxDTSBrick", "onJobTestPass", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection");
	registerInputEvent("fxDTSBrick", "onMenuOpen", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection");
	registerInputEvent("fxDTSBrick", "onMenuClose", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection");
	registerInputEvent("fxDTSBrick", "onMenuInput", "Self fxDTSBrick" TAB "Player Player" TAB "Client GameConnection");

	// Basic Output
	registerOutputEvent("fxDTSBrick", "requestFunds", "string 80 200" TAB "int 1 9000 1");

	for(%a = 1; $CityRPG::portion[%a] !$= ""; %a++)
	{
		%sellFood_Portions = %sellFood_Portions SPC $CityRPG::portion[%a] SPC %a;
	}

	registerOutputEvent("fxDTSBrick", "sellFood", "list" @ %sellFood_Portions TAB "string 45 100" TAB "int 1 50 1");
	for(%b = 1; isObject(JobSO.job[%b]); %b++)
	{
		if(strlen(JobSO.job[%b].name) > 10)
			%jobName = getSubStr(JobSO.job[%b].name, 0, 9) @ ".";
		else
			%jobName = JobSO.job[%b].name;

		%doJobTest_List = %doJobTest_List SPC strreplace(%jobName, " ", "") SPC %b;
	}

	registerOutputEvent("fxDTSBrick", "doJobTest", "list NONE 0" @ %doJobTest_List TAB "list NONE 0" @ %doJobTest_List TAB "bool");
	for(%c = 0; %c <= $City::ItemCount-1; %c++)
	{
		%sellItem_List = %sellItem_List SPC strreplace($City::Item::name[%c].uiName, " ", "") SPC %c;
	}
	registerOutputEvent("fxDTSBrick", "sellItem", "list" @ %sellItem_List TAB "int 0 500 1");
	for(%d = 0; %d < ClientGroup.getCount(); %d++)
	{
		%subClient = ClientGroup.getObject(%d);
		serverCmdRequestEventTables(%subClient);
	}
}
