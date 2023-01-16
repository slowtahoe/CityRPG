// Init
CityRPGData.scheduleDrug = schedule((($Pref::Server::City::tick::speed * 60000) / $CityRPG::drug::sellTimes), false, "Drug_Tick");

// ============================================================
// Preferences
// ============================================================

// Drugs
$Pref::Server::City::Speed::Cost										= 2000;
$Pref::Server::City::Steroid::Cost									= 2000;

//if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
//{
//	if(!$RTB::RTBR_ServerControl_Hook)
//	{
//		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
//	}

	RTB_registerPref("Speed Cost", "CityRPG Drugs|Drugs", "$Pref::Server::City::Speed::Cost", "int 0 10000", "Support_CityRPG_Drugs", 2000, 0, 0);
	RTB_registerPref("Steroid Cost", "CityRPG Drugs|Drugs", "$Pref::Server::City::Steroid::Cost", "int 0 10000", "Support_CityRPG_Drugs", 2000, 0, 0);
//}

// A bit of a hack. Registering as a glass section after registering all of the RTB prefs.
registerPreferenceAddon("Support_CityRPG_Drugs", "CityRPG Drugs", "grass"); //grass

// Misc Drug Prefs
$CityRPG::drug::minSellSpeed = 1; // In seconds
$CityRPG::drug::maxSellSpeed = 10; // In seconds
$CityRPG::drug::minBuyAmt = 1; // Minimum grams of weed player is capable of selling // Grams 1 - 5 have special names
$CityRPG::drug::maxBuyAmt = 5; // Maximum ^
$CityRPG::drug::sellPrice = 10; // About the real value of a gram of weed in the US // The actual price randomly changes by a couple digits
$CityRPG::drug::maxdrugplants = 99;
$CityRPG::drug::sellTimes = 50;
$CityRPG::drug::demWorth = 3; // The amount of dems each gram is worth. If their grams are worth the wanted limit or higher, they can be jailed.

// Drug Color Prefs
$CityRPG::drug::startcolor = 45;
$CityRPG::drug::emittertype = GrassEmitter;

// Drug Evidence Prefs
$CityRPG::drug::evidenceWorth = 1000; // How much someone can turn in drug evidence for
$CityRPG::evidenceWorth = $CityRPG::drug::evidenceWorth;

// Drug Types
// -Marijuana
$CityRPG::drugs::marijuana::placePrice = 1800; // How much it costs to plant the brick
$CityRPG::drugs::marijuana::harvestMin = 9; //9 Amount of grams you get from harvest
$CityRPG::drugs::marijuana::harvestMax = 14; //14
$CityRPG::drugs::marijuana::growthTime = 8; //8 In minutes
$CityRPG::drugs::marijuana::basePrice = getRandom(9,11); // Price per gram
// -Speed
$CityRPG::drugs::Speed::placePrice = $Pref::Server::City::Speed::Cost; // How much it costs to plant the brick
$CityRPG::drugs::Speed::harvestMin = 9; // Amount of grams you get from harvest
$CityRPG::drugs::Speed::harvestMax = 14;
$CityRPG::drugs::Speed::growthTime = 8; // In minutes
$CityRPG::drugs::Speed::basePrice = getRandom(3,5); // Price per gram
// -Steroid
$CityRPG::drugs::Steroid::placePrice = $Pref::Server::City::Steroid::Cost; // How much it costs to plant the brick
$CityRPG::drugs::Steroid::harvestMin = 9; // Amount of grams you get from harvest
$CityRPG::drugs::Steroid::harvestMax = 14;
$CityRPG::drugs::Steroid::growthTime = 8; // In minutes
$CityRPG::drugs::Steroid::basePrice = getRandom(3,5); // Price per gram
// -Opium
$CityRPG::drugs::opium::placePrice = 3000;
$CityRPG::drugs::opium::harvestMin = 11;
$CityRPG::drugs::opium::harvestMax = 18;
$CityRPG::drugs::opium::growthTime = 8;
$CityRPG::drugs::opium::basePrice = getRandom(12,16);

// ============================================================
// Bricks
// ============================================================

exec("./brickScripts/drugs/marijuana.cs");
exec("./brickScripts/drugs/opium.cs");
exec("./brickScripts/drugs/speed.cs");
exec("./brickScripts/drugs/steroid.cs");

exec("./brickScripts/info/drugsell.cs");

// ============================================================
// Datablocks
// ============================================================
datablock ItemData(mariItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./shapes/weedbag.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	doColorShift = true;
	image = mariImage;
	candrop = true;
	canPickup = false;
};

datablock ShapeBaseImageData(mariImage)
{
	shapeFile = "./shapes/weedbag.dts";
	emap = true;

	doColorShift = true;
	canPickup = false;
};

datablock ParticleData(DrugsmokeParticle)
{
	dragCoefficient      = 1.0;
	gravityCoefficient   = -0.2;
	inheritedVelFactor   = 0.0;
	constantAcceleration = 1.0;
	lifetimeMS           = 8000;
	lifetimeVarianceMS   = 300;
	useInvAlpha          = true;
	textureName          = "./shapes/cloud";
	colors[0]     = "1.0 1.0 1.0 1.0";
	colors[1]     = "1.0 1.0 1.0 1.0";
	colors[2]     = "1.0 1.0 1.0 0.0";
	sizes[0]      = 1.5;
	sizes[1]      = 1.5;
	sizes[2]      = 1.5;
	times[0]      = 0.0;
	times[1]      = 0.2;
	times[2]      = 1.0;
};

datablock ParticleEmitterData(DrugsmokeEmitter)
{
	ejectionPeriodMS = 20;
	periodVarianceMS = 0;
	ejectionVelocity = 0.2;
	ejectionOffset   = 1.5;
	velocityVariance = 0.2;
	thetaMin         = 0;
	thetaMax         = 30;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "DrugsmokeParticle";

	uiName = "Drugsmoke";
};

datablock ShapeBaseImageData(DrugsmokeImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $HeadSlot;

	stateName[0] = "Ready";
	stateTransitionOnTimeout[0] = "FireA";
	stateTimeoutValue[0] = 0.06;

	stateName[1] = "FireA";
	stateTransitionOnTimeout[1] = "Done";
	stateWaitForTimeout[1] = True;
	stateTimeoutValue[1] = 0.9;
	stateEmitter[1] = DrugsmokeEmitter;
	stateEmitterTime[1] = 0.9;

	stateName[2] = "Done";
	stateScript[2] = "onDone";
};

datablock AudioProfile(Smokingsound)
{
	filename    = "./sounds/smokedaweed.wav";
	description = AudioClose3d;
	preload = true;
};

// ============================================================
// Drug Tick
// ============================================================
function Drug_Tick(%client)
{
	CityRPGData.lastDrugTickOn = $Sim::DTime;

	CityRPG_DoDrugLoop(0);

	if(CityRPGData.scheduleDrug)
		cancel(CityRPGData.scheduleDrug);

	CityRPGData.scheduleDrug = schedule(((getRandom($CityRPG::drug::minSellSpeed,$CityRPG::drug::maxSellSpeed)) * 1000), false, "Drug_Tick");

	startSelling(%client);
}

function CityRPG_DoDrugLoop(%loop2,%client)
{
	%drugtime = (((getRandom($CityRPG::drug::minSellSpeed,$CityRPG::drug::maxSellSpeed)) * 1000) / CityRPGData.dataCount);

	if(isObject(%client = findClientByBL_ID(CityRPGData.data[%loop2].ID)))
		%client.drugtick();

	if(%loop2 < CityRPGData.dataCount)
		schedule(%drugtime, false, "CityRPG_DoDrugLoop", (%loop2 + 1));
}

function gameConnection::drugtick(%client)
{
	if(%client.selling)
	{
		startSelling(%client);
	}
}

function gameConnection::getDrugs(%client)
{
	return CityRPGData.getData(%client.bl_id).valuetotaldrugs;
}

// ============================================================
// Commands
// ============================================================
function servercmddrugamount(%client)
{
	%client.cityLog("/drugamount");

	if(!isObject(%client.player))
		return;
	messageClient(%client,'',CityRPGData.getData(%client.bl_id).valuedrugamount);
}

function servercmduseSpeed(%client)
{
	%client.cityLog("/useSpeed");

	if(!isObject(%client.player))
		return;

		%time = getSimTime();
		if(%client.lastTimeuseSpeed $= "" || %time - %client.lastTimeuseSpeed > 15000)
		{
			if(isObject(%client.player))
			{
				if(CityRPGData.getData(%client.bl_id).valueSpeed > 0)
				{
					serverplay3d(Smokingsound,%client.player.getHackPosition() SPC "0 0 1 0");
					%client.player.setWhiteout(1);
					%client.player.emote(DrugsmokeImage);
					messageClient(%client,'',"\c6Used \c31 gram \c6of \c3Speed\c6.");
					CityRPGData.getData(%client.bl_id).valueSpeed--;
					%client.lastTimeuseSpeed = %time;
					%client.player.setDatablock(FastPlayerArmor);
					messageClient(%client,'',"\c6You can now run fast!");
					schedule(14000,0,"resetDatablock",%client);
				}
				else
					messageClient(%client,'',"\c6You don't have any.");
			}
			else
				messageClient(%client,'',"\c6You must spawn first.");
	}
}

function serverCmdgiveSteroid(%client, %money, %name)
{
	%client.cityLog("/giveSteroid" SPC %money SPC %name);

	if(!isObject(%client.player))
		return;

	%money = mFloor(%money);
	if(%client.getJobSO().law)
	{
		return;
	}

	if(%money > 0)
	{
		if((CityRPGData.getData(%client.bl_id).valueSteroid - %money) >= 0)
		{
			if(isObject(%client.player))
			{
				if(%name !$= "")
				{
					%target = findclientbyname(%name);
				}
				else
				{
					%target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType,%client.player).client;
				}

				if(isObject(%target))
				{
					messageClient(%client, '', "\c6You give \c3" @ %money SPC "\c6steroid to \c3" @ %target.name @ "\c6.");
					messageClient(%target, '', "\c3" @ %client.name SPC "\c6has given you \c3$" @ %money @ "\c6.");
					CityRPGData.getData(%client.bl_id).valueSteroid -= %money;
					CityRPGData.getData(%target.bl_id).valueSteroid += %money;
					%client.SetInfo();
					%target.SetInfo();
				}
				else
					messageClient(%client, '', "\c6You must be looking at and be in a reasonable distance of the player. \nYou can also type in the person's name after the amount.");
			}
			else
				messageClient(%client, '', "\c6Spawn first before you use this command.");
		}
		else
			messageClient(%client, '', "\c6You don't have that much.");
	}
	else
		messageClient(%client, '', "\c6You must enter a valid amount.");
}

function serverCmdgiveSpeed(%client, %money, %name)
{
	%client.cityLog("/giveSpeed" SPC %money SPC %name);

	if(!isObject(%client.player))
		return;

	%money = mFloor(%money);
	if(%client.getJobSO().law)
	{
		return;
	}

		 if(%money > 0)
		 {
			 if((CityRPGData.getData(%client.bl_id).valueSpeed - %money) >= 0)
			 {
				 if(isObject(%client.player))
				 {
					 if(%name !$= "")
					 {
						 %target = findclientbyname(%name);
					 }
					 else
					 {
						 %target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType,%client.player).client;
					 }

					 if(isObject(%target))
					 {
							 messageClient(%client, '', "\c6You give \c3" @ %money SPC "\c6speed to \c3" @ %target.name @ "\c6.");
							 messageClient(%target, '', "\c3" @ %client.name SPC "\c6has given you \c3" @ %money @ " speed\c6.");
							 CityRPGData.getData(%client.bl_id).valueSpeed -= %money;
							 CityRPGData.getData(%target.bl_id).valueSpeed += %money;
							 %client.SetInfo();
						%target.SetInfo();
					}
					else
						messageClient(%client, '', "\c6You must be looking at and be in a reasonable distance of the player. \nYou can also type in the person's name after the amount.");
				}
				else
					messageClient(%client, '', "\c6Spawn first before you use this command.");
			}
			else
				messageClient(%client, '', "\c6You don't have that much.");
		}
		else
			messageClient(%client, '', "\c6You must enter a valid amount.");
}

function resetScale(%client)
{
	%client.drugged = 0;
	messageClient(%client,'',"\c6Drug has wore off!");
	%client.player.setScale("1 1 1");
}

function resetDatablock(%client)
{
	messageClient(%client,'',"\c6The drug has wore off!");
	%client.player.setDatablock(%client.getJobSO().db);
}

function servercmduseSteroid(%client)
{
	%client.cityLog("/useSteroid");

	if(!isObject(%client.player))
		return;

	messageClient(%client,'',"\c6!");
	%time = getSimTime();

	if(%client.lastTimeuseSteroid $= "" || %time - %client.lastTimeuseSteroid > 14000)
	{
		if(isObject(%client.player))
		{
			if(CityRPGData.getData(%client.bl_id).valueSteroid > 0)
			{
				serverplay3d(Smokingsound,%client.player.getHackPosition() SPC "0 0 1 0");
				%client.player.setWhiteout(1);
				%client.player.emote(DrugsmokeImage);
				messageClient(%client,'',"\c6Used \c31 gram \c6of \c3Steroid\c6.");
				CityRPGData.getData(%client.bl_id).valueSteroid--;
				%client.lastTimeuseSteroid = %time;
				%target = findClientByName(%client).player;
				%client.drugged = 1;
				%client.player.setScale("2 2 2");
				messageClient(%client,'',"\c6You are now bigger!");
				schedule(14000,0,"resetScale",%client);
			}
			else
				messageClient(%client,'',"\c6You don't have any.");
		}
		else
			messageClient(%client,'',"\c6You must spawn first.");
	}
}

function servercmdusemarijuana(%client)
{
	%client.cityLog("/usemarijuana");

	if(!isObject(%client.player))
		return;

	%time = getSimTime();
	if(%client.lastTimeusemarijuana $= "" || %time - %client.lastTimeusemarijuana > 5000)
	{
		if(isObject(%client.player))
		{
			if(CityRPGData.getData(%client.bl_id).valueMarijuana > 0)
			{
				serverplay3d(Smokingsound,%client.player.getHackPosition() SPC "0 0 1 0");
				%client.player.setWhiteout(1);
				%client.player.emote(DrugsmokeImage);
				messageClient(%client,'',"\c6Used \c31 gram \c6of \c3marijuana\c6.");
				CityRPGData.getData(%client.bl_id).valueMarijuana--;
				%client.lastTimeusemarijuana = %time;
			}
			else
				messageClient(%client,'',"\c6You don't have any.");
		}
		else
			messageClient(%client,'',"\c6You must spawn first.");
	}
}

function DrugsmokeImage::onDone(%this,%obj,%slot)
{
	%obj.unMountImage(%slot);
}

function fxDTSBrick::bagPlant(%col)
{
	%col.schedule(0, "delete");
	CityRPGData.getData(%col.owner).valuedrugamount--;
	if(isObject(getBrickGroupFromObject(%col).client))
	{
		getBrickGroupFromObject(%col).client.SetInfo();
	}
}

function fxDTSBrick::startGrowing(%drug,%brick)
{
	//echo("drug level <" @ %drug.growtime @ ">");
	//echo("drug time <" @ %drugtime @ ">");
	//echo("mari grow time <" @ $CityRPG::drugs::marijuana::growthTime @ ">");
	//echo("drug name <" @ %drug.uiName @ ">");

	if(%drug.uiName $= "Marijuana")
	{
		%drugtype = $CityRPG::drugs::marijuana::growthTime;
		%drugtime = ((($CityRPG::drugs::marijuana::growthTime) * 60000) / 8);
	}
	else if(%drug.uiName $= "Opium")
	{
		%drugtype = $CityRPG::drugs::opium::growthTime;
		%drugtime = ((($CityRPG::drugs::opium::growthTime) * 60000) / 8);
	}
	else if(%drug.uiName $= "Speed")
	{
		%drugtype = $CityRPG::drugs::speed::growthTime;
		%drugtime = ((($CityRPG::drugs::speed::growthTime) * 60000) / 8);
	}
	else if(%drug.uiName $= "Steroid")
	{
		%drugtype = $CityRPG::drugs::steroid::growthTime;
		%drugtime = ((($CityRPG::drugs::steroid::growthTime) * 60000) / 8);
	}

	%drug.isGrowing = true;
	%drug.canchange = false;
	%drug.currentColor = 45;

	if(%drug.growtime == 1)
	{
		%drug.canchange = true;
		%drug.currentColor = 54;
		%drug.setColor(54);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 2)
	{
		%drug.canchange = true;
		%drug.currentColor = 55;
		%drug.setColor(55);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 3)
	{
		%drug.canchange = true;
		%drug.currentColor = 56;
		%drug.setColor(56);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 4)
	{
		%drug.canchange = true;
		%drug.currentColor = 57;
		%drug.setColor(57);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 5)
	{
		%drug.canchange = true;
		%drug.currentColor = 58;
		%drug.setColor(58);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 6)
	{
		%drug.canchange = true;
		%drug.currentColor = 59;
		%drug.setColor(59);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 7)
	{
		%drug.canchange = true;
		%drug.currentColor = 60;
		%drug.setColor(60);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	else if(%drug.growtime == 8)
	{
		%drug.canchange = true;
		%drug.currentColor = 61;
		%drug.setColor(61);
		%drug.setEmitter(None);
		%drug.canchange = false;
	}
	%drug.canbecolored = false;

	if(%drug.growtime < 8)
	{
		%drug.hasemitter = false;
		%drug.canchange = false;
		%drug.growtime++;
		%drug.schedule(%drugtime, "startGrowing", %drug,%brick);
	}
	else if(%drug.growtime == 8)
	{
		%drug.canchange = true;
		%drug.grow();
		%drug.canchange = false;
	}
}

function fxDTSBrick::grow(%drug,%brick)
{
	%drug.health = 0;
	%drug.hasDrug = true;
	%drug.grew = true;
	%drug.setColor(61);
	%drug.canChange = true;
	%drug.cansetemitter = true;
	%drug.emitter = "GrassEmitter";
	//%drug.setEmitter(GrassEmitter);
	%drug.cansetemitter = false;
	%drug.hasemitter = true;
	%drug.canchange = false;
}

function fxDTSBrick::harvest(%this, %client)
{
	%drug = %this.getID();
	%brickData = %this.getDatablock();
	if(%this.hasDrug)
	{
		if(%drug.health < %drug.random)
		{
			%drug.health++;
			%percentage = mFloor((%drug.health / %drug.random) * 100);

			if(%percentage >= 0 && %percentage < 10)
				%color = "<color:ff0000>";
			else if(%percentage >= 10 && %percentage < 20)
				%color = "<color:ff2200>";
			else if(%percentage >= 10 && %percentage < 30)
				%color = "<color:ff4400>";
			else if(%percentage >= 10 && %percentage < 40)
				%color = "<color:ff6600>";
			else if(%percentage >= 10 && %percentage < 50)
				%color = "<color:ff8800>";
			else if(%percentage >= 10 && %percentage < 60)
				%color = "<color:ffff00>";
			else if(%percentage >= 10 && %percentage < 70)
				%color = "<color:88ff00>";
			else if(%percentage >= 10 && %percentage < 80)
				%color = "<color:66ff00>";
			else if(%percentage >= 10 && %percentage < 90)
				%color = "<color:44ff00>";
			else if(%percentage >= 10 && %percentage < 100)
				%color = "<color:22ff00>";
			else if(%percentage == 100)
				%color = "<color:00ff00>";

			commandToClient(%client,'centerPrint',"\c3" @ %brickData.uiName @ " \c6harvested: %" @ %color @ "" @ %percentage,3);
		}
		else
		{
			if(%brickData.drugType $= "Opium" && %brickData.drugType !$= "Marijuana" && %brickData.drugType !$= "Speed")
			{
				%harvestamt = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
				%client.cityLog("Harvest " @ %harvestamt @ "g of Opium");

				CityRPGData.getData(%client.bl_id).valueopium += %harvestamt;
				CityRPGData.getData(%client.bl_id).valuetotaldrugs += %harvestamt;

				if(%client.bl_id == %drug.owner)
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of \c3Opium\c6.",3);
				}
				else
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of someone elses \c3Opium\c6.",3);
				}

				%drug.canchange = true;
				%drug.currentColor = 45;
				%drug.setColor(45);
				%drug.grew = false;
				%drug.health = 0;
				%drug.growtime = 0;
				%drug.canbecolored = false;
				%drug.watered = false;
				%drug.random = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
				%drug.hasDrug = false;
				%drug.isGrowing = false;
				%drug.cansetemitter = true;
				%drug.setEmitter(None);
				%drug.cansetemitter = false;
				%drug.canchange = false;

				%client.SetInfo();
			}
			else if(%brickData.drugType $= "Marijuana" && %brickData.drugType !$= "Opium" && %brickData.drugType !$= "Speed")
			{
				%harvestamt = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);

				%client.cityLog("Harvest " @ %harvestamt @ "g of Marijuana");

				CityRPGData.getData(%client.bl_id).valuemarijuana += %harvestamt;
				CityRPGData.getData(%client.bl_id).valuetotaldrugs += %harvestamt;
				if(%client.bl_id == %drug.owner)
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of \c3Marijuana\c6.",3);
				}
				else
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of someone elses \c3Marijuana\c6.",3);
				}

				%drug.canchange = true;
				%drug.currentColor = 45;
				%drug.setColor(45);
				%drug.grew = false;
				%drug.health = 0;
				%drug.growtime = 0;
				%drug.canbecolored = false;
				%drug.watered = false;
				%drug.random = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);
				%drug.hasDrug = false;
				%drug.isGrowing = false;
				%drug.cansetemitter = true;
				%drug.setEmitter(None);
				%drug.cansetemitter = false;
				%drug.canchange = false;
				%client.SetInfo();
			}
			else if(%brickData.drugType $= "Speed" && %brickData.drugType !$= "Opium" && %brickData.drugType !$= "Marijuana")
			{
				%harvestamt = getRandom($CityRPG::drugs::Speed::harvestMin,$CityRPG::drugs::Speed::harvestMax);

				%client.cityLog("Harvest " @ %harvestamt @ "g of Speed");

				CityRPGData.getData(%client.bl_id).valueSpeed += %harvestamt;
				CityRPGData.getData(%client.bl_id).valuetotaldrugs += %harvestamt;

				if(%client.bl_id == %drug.owner)
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of \c3Speed\c6.",3);
				}
				else
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of someone elses \c3Speed\c6.",3);
				}

				%drug.canchange = true;
				%drug.currentColor = 45;
				%drug.setColor(45);
				%drug.grew = false;
				%drug.health = 0;
				%drug.growtime = 0;
				%drug.canbecolored = false;
				%drug.watered = false;
				%drug.random = getRandom($CityRPG::drugs::Speed::harvestMin,$CityRPG::drugs::Speed::harvestMax);
				%drug.hasDrug = false;
				%drug.isGrowing = false;
				%drug.cansetemitter = true;
				%drug.setEmitter(None);
				%drug.cansetemitter = false;
				%drug.canchange = false;
				%client.SetInfo();
			}
			else if(%brickData.drugType $= "Steroid" && %brickData.drugType !$= "Opium" && %brickData.drugType !$= "Marijuana")
			{
				%harvestamt = getRandom($CityRPG::drugs::Steroid::harvestMin,$CityRPG::drugs::Steroid::harvestMax);

				%client.cityLog("Harvest " @ %harvestamt @ "g of Steroid");

				CityRPGData.getData(%client.bl_id).valueSteroid += %harvestamt;
				CityRPGData.getData(%client.bl_id).valuetotaldrugs += %harvestamt;
				if(%client.bl_id == %drug.owner)
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of \c3Steroid\c6.",3);
				}
				else
				{
					commandToClient(%client,'centerPrint',"\c6You harvested \c3" @ %harvestamt @ "\c6 grams of someone elses \c3Steroid\c6.",3);
				}

				%drug.canchange = true;
				%drug.currentColor = 45;
				%drug.setColor(45);
				%drug.grew = false;
				%drug.health = 0;
				%drug.growtime = 0;
				%drug.canbecolored = false;
				%drug.watered = false;
				%drug.random = getRandom($CityRPG::drugs::Steroid::harvestMin,$CityRPG::drugs::Steroid::harvestMax);
				%drug.hasDrug = false;
				%drug.isGrowing = false;
				%drug.cansetemitter = true;
				%drug.setEmitter(None);
				%drug.cansetemitter = false;
				%drug.canchange = false;
				%client.SetInfo();
			}
		}
	}
}

function startSelling(%client)
{
	%drugname = %client.drugname;
	if(%drugname $= "Marijuana")
	{
		%amount = CityRPGData.getData(%client.bl_id).valueMarijuana;
	}
	else if(%drugname $= "Opium")
	{
		%amount = CityRPGData.getData(%client.bl_id).valueOpium;
	}
	else if(%drugname $= "Speed")
	{
		%amount = CityRPGData.getData(%client.bl_id).valueSpeed;
	}
	else if(%drugname $= "Steroid")
	{
		%amount = CityRPGData.getData(%client.bl_id).valueSteroid;
	}

if(%amount > 0)
{
	%buymin = $CityRPG::drug::minBuyAmt;
	%buymax = $CityRPG::drug::maxBuyAmt;
	%grams = getRandom(%buymin,%buymax);

	if(%grams > %amount)
	{
		%grams = %amount;
	}
	else if(%grams == 0)
	{
		messageClient(%client,'',"\c6You're all out!");
		return;
	}

	%grams = mFloor(%grams);

	if(%drugname $= "marijuana")
	{
		%profit = $CityRPG::drugs::marijuana::basePrice;
	}
	else if(%drugname $= "opium")
	{
		%profit = $CityRPG::drugs::opium::basePrice;
	}
	else if(%drugname $= "speed")
	{
		%profit = $CityRPG::drugs::speed::basePrice;
	}
	else if(%drugname $= "steroid")
	{
		%profit = $CityRPG::drugs::steroid::basePrice;
	}

	%totalcash = %grams * %profit;
	%randomize = getRandom(1,2);

	if(%randomize == 1)
	{
		%totalcash -= getRandom(0.75,1);
	}

	else if(%randomize == 2)
	{
		%totalcash += getRandom(1,1.25);
	}

	%totalcash = mFloor(%totalcash);
	%client.cityLog("Drug sell for " @ %totalcash);
	CityRPGData.getData(%client.bl_id).valueMoney += %totalcash;
	%client.setGameBottomPrint();
	%slang = %grams;
	switch(%slang)
	{
		case 1: %slang = "a \c3gram\c6 of";
		case 2: %slang = "a \c3dimebag\c6 of";
		case 3: %slang = "\c3three grams\c6 of";
		case 4: %slang = "a \c3dub\c6 of";
		case 5: %slang = "\c3five grams\c6 of";
		default: %slang = "some";
		}

		%client.setInfo();

		messageClient(%client,'',"\c6You sold " @ %slang @ " " @ %drugname @ " to a stranger for \c3$" @ %totalcash @"\c6.");

		if(%drugname $= "marijuana")
		{
			CityRPGData.getData(%client.bl_id).valuemarijuana -= %grams;
			CityRPGData.getData(%client.bl_id).valuetotaldrugs -= %grams;
		}
		else if(%drugname $= "opium")
		{
			CityRPGData.getData(%client.bl_id).valueopium -= %grams;
			CityRPGData.getData(%client.bl_id).valuetotaldrugs -= %grams;
		}
		else if(%drugname $= "speed")
		{
			CityRPGData.getData(%client.bl_id).valuespeed -= %grams;
			CityRPGData.getData(%client.bl_id).valuetotaldrugs -= %grams;
		}
		else if(%drugname $= "steroid")
		{
			CityRPGData.getData(%client.bl_id).valuesteroid -= %grams;
			CityRPGData.getData(%client.bl_id).valuetotaldrugs -= %grams;
		}
	}
	else
		messageClient(%client,'',"\c6You're all out!");
	return;
}

function addEvid(%col,%client)
{
	if(isObject(%col) && %col.isPlanted())
	{
		%col.bagPlant(%col,%client);
		commandToClient(%client,'centerPrint',"\c6You can turn this in as \c3Evidence \c6at the Police Department.",3);
		CityRPGData.getData(%client.bl_id).valueevidence++;

		%client.cityLog("Collect evidence +1");
	}
}

// Commands
function servercmdmydrugs(%client)
{
	%client.cityLog("/mydrugs");

	if(!isObject(%client.player))
		return;

	messageClient(%client,'',"\c6Your marijuana in grams :" @ CityRPGData.getData(%client.bl_id).valueMarijuana);
	messageClient(%client,'',"\c6Your opium in grams: " @ CityRPGData.getData(%client.bl_id).valueopium);
	messageClient(%client,'',"\c6Your speed in grams: " @ CityRPGData.getData(%client.bl_id).valuespeed);
	messageClient(%client,'',"\c6Your steroid in grams: " @ CityRPGData.getData(%client.bl_id).valuesteroid);
	messageClient(%client,'',"\c6Your total drugs in grams: " @ CityRPGData.getData(%client.bl_id).valuetotaldrugs);
}

function servercmddrughelp(%client)
{
	%client.cityLog("/drughelp");

	if(!isObject(%client.player))
		return;
	messageClient(%client,'',"\c6- \c3How to grow drugs for dummies\c6 -");
	messageClient(%client,'',"\c3Step 1\c6: Navigate to the City RPG tab in the brick menu");
	messageClient(%client,'',"\c3Step 2\c6: Scroll down until you find the drug bricks");
	messageClient(%client,'',"\c3Step 3\c6: Select a drug and place it in your City RPG Lot");
	messageClient(%client,'',"\c3Step 4\c6: Click your drug brick to water it");
	messageClient(%client,'',"\c3Step 5\c6: Wait a few in-game days");
	messageClient(%client,'',"\c3Step 6\c6: Find/buy a knife");
	messageClient(%client,'',"\c3Step 7\c6: Harvest your drug brick with your knife");
	messageClient(%client,'',"\c3Step 8\c6: Find a drug sell brick placed around the city");
	messageClient(%client,'',"\c3Step 9\c6: Don't get caught!");
	messageClient(%client,'',"\c6---");
	messageClient(%client,'',"\c3Tip\c6: Having a lot of drugs on you and getting batoned will get you jail time!");
	messageClient(%client,'',"\c3Tip\c6: Cops can baton your crops and turn them in as evidence, so hide them well!");
}


function serverCmddropmarijuana(%client,%amt)
{
	%client.cityLog("/dropmarijuana" SPC %amt);

	%amt = mFloor(%amt);
	if(%amt >= 1)
	{
		if(CityRPGData.getData(%client.bl_id).valueMarijuana >= %amt)
		{
			%mari = new Item()
			{
				datablock = mariItem;
				canPickup = false;
				value = %amt;
			};

			%mari.setTransform(setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 4));
			%mari.setVelocity(VectorScale(%client.player.getEyeVector(), 10));
			MissionCleanup.add(%mari);
			%mari.setShapeName(%mari.value @ " grams");
			CityRPGData.getData(%client.bl_id).valueMarijuana = CityRPGData.getData(%client.bl_id).valueMarijuana - %amt;
			CityRPGData.getData(%client.bl_id).valuetotaldrugs = CityRPGData.getData(%client.bl_id).valuetotaldrugs - %amt;
			%client.cityLog("Drop '" @ %amt @ "' marijuana");
			messageClient(%client,'',"\c6You drop \c3" @ %amt @ " grams\c6 of marijuana..");
		}
		else
			messageClient(%client,'',"\c6You don't have that much marijuana to drop!");
	}
	else
		messageClient(%client,'',"\c6The least you can drop is \c3 5 grams\c6.");
}
