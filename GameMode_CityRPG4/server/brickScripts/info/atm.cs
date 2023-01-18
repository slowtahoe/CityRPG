// ============================================================
// Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGATMBrickData)
{
	brickFile = $City::DataPath @ "bricks/atm.blb";
	iconName = $City::DataPath @ "ui/BrickIcons/ATM";

	category = "CityRPG";
	subCategory = "Info Bricks";

	uiName = "ATM Brick";

	CityRPGBrickType = $CityBrick_Info;
	CityRPGBrickAdmin = false;

	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "4 4 3";
	trigger = 0;
};

// ============================================================
// Menu
// ============================================================
$City::Menu::ATMBaseTxt =	
		"Withdraw money."
	TAB "Hack ATM.";

$City::Menu::ATMBaseFunc =	
		"CityMenu_BankWithdrawPrompt"
	TAB "CityMenu_HackATMPrompt";

function CityMenu_ATM(%client, %brick)
{
	%client.cityMenuMessage("\c6You have \c3$" @ City.get(%client.bl_id, "bank") SPC "\c6in your account.");

	%client.cityLog("Enter ATM");

	%menu = $City::Menu::ATMBaseTxt;
	// We can call directly on the same prompt that the bank uses.
	%functions = $City::Menu::ATMBaseFunc;

	%client.cityMenuOpen(%menu, %functions, %brick, "\c6Thanks, come again.", 0, 0, "\c3ATM");
}

function CityMenu_HackATMPrompt(%client)
{
	for(%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%clientCheck = ClientGroup.getObject(%i);
		if((%clientCheck.getJobSO().law) == true)
		{
			%disableHacking = false;
		}
		else
		{
			%disableHacking = true;
		}
	}

	if(%disableHacking == true)
	{
		messageClient(%client,'',"\c6No law enforcement online. ATM hacking is disabled.");
		return;
	}

	if(%client.AccessableATM == 1) 
	{
		messageClient(%client,'',"\c6You need to wait a while before hacking again. 7sec from the last.");
		return;
	}
	else
	{
		if(CityRPGData.getData(%client.bl_id).valueEducation >= 3)
		{
			%stealchance = getRandom(1,2);
			%caughtchance = getRandom(1,4);
			%lockoutchance = getRandom(1,3);
			%beencaught = 1;
			if(%stealchance == 1)
			{
				if(%lockoutchance != 1)
				{
					%stolen = getRandom($ATM::Min,$ATM::Max);
					messageClient(%client,'',"\c6You managed to steal \c3$" @ %stolen @ "\c6 from the ATM.");
					CityRPGData.getData(%client.bl_id).valueMoney += %stolen;
					%client.AccessableATM = 1;
					CityRPGData.getData(%client.bl_id).valueDemerits += $ATM::Demerits;
					schedule(7000,0,"resetAccessableATM",%client);
					if(%caughtchance != 1)
					{
						messageAll('',"\c3" @ %client.name @ "\c6 has been caught hacking an ATM!");
						CityRPGData.getData(%client.bl_id).valueDemerits += $ATM::Demerits;
					}
				}
				else
				{
					%this.hackable = 0;
					messageClient(%client,'',"\c6Your attempt failed, and you have been locked out of the machine.");
					
				}
			}
			else
			{
				messageClient(%client,'',"\c6Failed to hack.");
			}
		}
		else
		{
			messageClient(%client,'',"\c6Your education level must be \c3" @ $Pref::Server::City::hack::education @ "\c6.");
		}
		return;
	}
}

// ============================================================
// Trigger Data
// ============================================================
function CityRPGATMBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus == true && !%client.cityMenuOpen)
	{
		%client.cityMenuMessage("\c6Welcome to the " @ $Pref::Server::City::name @ " Bank. Your account balance is \c3$" @ City.get(%client.bl_id, "bank") @ "\c6. Current economy value: \c3" @ %econColor @ $City::Economics::Condition @ "\c6%");

		CityMenu_ATM(%client, %brick);
	}
}

function resetAccessableATM(%client)
{
	%client.AccessableATM = 0;
}
