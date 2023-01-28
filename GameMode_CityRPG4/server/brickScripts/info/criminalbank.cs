// ============================================================
// Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGCriminalBankBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "City Info Bricks";

	uiName = "Criminal Bank Brick";

	CityRPGBrickType = $CityBrick_Info;
	CityRPGBrickAdmin = true;

	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};

// ============================================================
// Menu
// ============================================================
// Initial menu
$City::Menu::CrimBankBaseTxt =	
		"Withdraw money."
	TAB "Deposit money."
	TAB "Deposit all money."
	TAB "Donate to the economy.";

$City::Menu::CrimBankBaseFunc =
	"CityMenu_CrimBankWithdrawPrompt"
	TAB "CityMenu_CrimBankDepositPrompt"
	TAB "CityMenu_CrimBankDepositAll"
	TAB "CityMenu_CrimBankDonatePrompt";

function CityMenu_CrimBank(%client, %brick)
{
	%menu =	$City::Menu::CrimBankBaseTxt;

	%functions = $City::Menu::CrimBankBaseFunc;

	%client.cityLog("Enter bank");

	%client.cityMenuOpen(%menu, %functions, %brick, "\c6Thanks, come again.", 0, 0, "Undergound Bank");
}

// Withdraw money.
function CityMenu_CrimBankWithdrawPrompt(%client)
{
	%client.cityLog("Bank withdraw prompt");

	%client.cityMenuMessage("\c6Please enter the amount of money you wish to withdraw.");
	%client.cityMenuFunction = CityMenu_CrimBankWithdraw;
}

function CityMenu_CrimBankWithdraw(%client, %input)
{
	if(mFloor(%input) < 1)
	{
		%client.cityMenuMessage("\c6Please enter a valid amount of money to withdraw.");

		return;
	}

	%crimFee = mFloor((%input) * ($Pref::Server:City::CrimBankFee / 100));

	if((City.get(%client.bl_id, "bank") - (mFloor(%input + %crimFee))) < 0)
	{
		if((City.get(%client.bl_id, "bank") - (mFloor(%input + %crimFee))) < 1)
		{
			%client.cityMenuMessage("\c6You don't have $" @ $c_p @ %econColor @ %input @ "\c6 plus a fee of $" @ $c_p @ %econColor @ %crimFee @ "\c6 in the bank to withdraw.");

			%client.cityMenuClose();

			return;
		}

		%input = (City.get(%client.bl_id, "bank"));
	}

	%inputAddFee = (%input + %crimFee);

	%client.cityLog("Bank withdraw $" @ mFloor(%inputAddFee));
	%client.cityMenuMessage("\c6A service fee of $" @ $c_p @ %econColor @ %crimFee @ "\c6 has been applied. You have withdrawn " @ $c_p @ "$" @ mFloor(%inputAddFee) @ "\c6.");

	%client.cityMenuClose();

	City.subtract(%client.bl_id, "bank", mFloor(%inputAddFee));
	City.add(%client.bl_id, "money", mFloor(%input));
	if (((%crimFee) * 0.002) > 2)
	{
		if($City::Economics::Condition < $Pref::Server::City::Economics::Cap)
		{
			$City::Economics::Condition = ($City::Economics::Condition + 20);
		}
	}
	else
	{
		if($City::Economics::Condition < $Pref::Server::City::Economics::Cap)
		{
			$City::Economics::Condition = (($City::Economics::Condition + %crimFee) * 0.002);
		}
	}

	%client.SetInfo();
}

// Deposit money.
function CityMenu_CrimBankDepositPrompt(%client)
{
	%client.cityLog("Bank deposit prompt");

	%client.cityMenuMessage("\c6Please enter the amount of money you wish to deposit.");
	%client.cityMenuFunction = CityMenu_CrimBankDeposit;
}

function CityMenu_CrimBankDeposit(%client, %input)
{
	if(mFloor(%input) < 1)
	{
		%client.cityMenuMessage("\c6Please enter a valid amount of money to deposit.");

		return;
	}

	if(City.get(%client.bl_id, "money") - mFloor(%input) < 0)
	{
		if(City.get(%client.bl_id, "money") < 1)
		{
			%client.cityMenuMessage("\c6You don't have that much money to deposit.");

			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, (isObject(%client.player) ? %client.player : 0));

			return;
		}

		%input = City.get(%client.bl_id, "money");
	}

	%crimFee = mFloor((%input) * ($Pref::Server:City::CrimBankFee / 100));
	%inputSubFee = (%input - %crimFee);

	%client.cityLog("Bank deposit $" @ mFloor(%input));

	%client.cityMenuMessage("\c6A service fee of $" @ $c_p @ %econColor @ %crimFee @ "\c6 has been applied. You have deposited $" @ $c_p @ mFloor(%inputSubFee) @ "\c6!");

	City.add(%client.bl_id, "bank", mFloor(%inputSubFee));
	City.subtract(%client.bl_id, "money", mFloor(%input));
	if (((%crimFee) * 0.002) > 2)
	{
		if($City::Economics::Condition < $Pref::Server::City::Economics::Cap)
		{
			$City::Economics::Condition = ($City::Economics::Condition + 20);
		}
	}
	else
	{
		if($City::Economics::Condition < $Pref::Server::City::Economics::Cap)
		{
			$City::Economics::Condition = (($City::Economics::Condition + %crimFee) * 0.002);
		}
	}

	%client.cityMenuClose();
	%client.SetInfo();
}

// Deposit all money.
function CityMenu_CrimBankDepositAll(%client)
{
	%client.cityLog("Bank deposit all");
	CityMenu_CrimBankDeposit(%client, City.get(%client.bl_id, "money"));
}

// Donate to the economy.
function CityMenu_CrimBankDonatePrompt(%client)
{
	%client.cityLog("Bank donate prompt");
	%client.cityMenuMessage("\c6Enter the amount you would like to donate:");
	%client.cityMenuFunction = CityMenu_CrimBankDonate;
}

function CityMenu_CrimBankDonate(%client, %input)
{
	serverCmddonate(%client, %input);
	return;
}

// ============================================================
// Trigger Data
// ============================================================
function CityRPGCriminalBankBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus == true && !%client.cityMenuOpen)
	{
		%client.cityMenuMessage("\c6Welcome to the " @ $Pref::Server::City::name @ " Underground Bank. Your account balance is $" @ $c_p @ City.get(%client.bl_id, "bank") @ "\c6. Current economy value: " @ $c_p @ %econColor @ $City::Economics::Condition @ "\c6%");
		%client.cityMenuMessage("\c6Current Underground Service Fees are " @ $c_p @ %econColor @ $Pref::Server:City::CrimBankFee @ "\c6%");
		CityMenu_CrimBank(%client, %brick);
	}
}
