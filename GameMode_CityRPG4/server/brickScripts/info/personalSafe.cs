// ============================================================
// Project			: MetroRP
// Author			: Diggy, Phydeoux (safe model)
// Description			: Personal safe Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Trigger Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================

%error = ForceRequiredAddOn("Support_LegacyDoors");

//if(%error == $Error::AddOn_NotFound)
//{
//	error("MetroRP(JVS_personalSafe): Support_LegacyDoors is missing and is required by this Add-On.");
//}
//else
//{
	ContentTypesSO.addContentType("Add-Ons/Gamemode_MetropolisRP/Bricks/JVS/types/PersonalSafe.cs");
//}

//addContentType("Add-Ons/Gamemode_MetropolisRP/Bricks/JVS/types/PersonalSafe.cs");

datablock fxDTSBrickData(contentBrickPersonalSafe2 : contentBrickPersonalSafe)
{
	category = "MetroRP";
	subCategory = "Player Bricks";
	
	uiName = "MetroRP Personal Safe";
	initialPrice = $MetroRP::prices::personalVault;
	
	MetroRPBrickType = 2;
	MetroRPBrickAdmin = false;
	
	triggerDatablock = MetroRPInputTriggerData;
	triggerSize = "9 9 3";
	trigger = 0;

	brickSizeX = 4;
	brickSizeY = 4;
	brickSizeZ = 10;
};

// ============================================================
// Section 2 : Trigger Data
// ============================================================
// old function contentBrick::parseData(%datablock, %brick, %client, %triggerStatus, %text)

function contentBrickPersonalSafe2::parseData(%datablock, %brick, %client, %triggerStatus, %text)
{
	echo("contentBrickPersonalSafe2::parseData(%datablock):" @ %datablock);
	contentBrickPersonalSafe.parseData(%brick, %client, %triggerStatus, %text);
}

function contentBrickPersonalSafe::parseData(%datablock, %brick, %client, %triggerStatus, %text)
{
	echo("contentBrickPersonalSafe::parseData(%datablock):" @ %datablock);
	if(%client.MetroRPInputTrigger != %brick.trigger)
	{
		return;
	}

	%bGroup = getBrickGroupFromObject(%brick);

	if(!isObject(%bGroup.client))
	{
		%offline = 1;
	}
	else
	{
		%bClient = %bGroup.client;
	}

	if(!%triggerStatus && %client.stage > 0)
	{
		messageClient(%client, '', "\c6Thanks, come again.");
		%client.stage = 0;
		return;
	}
	if(!%client.stage)
	{
		messageClient(%client, '', "\c6You have accessed a personal safe. It contains \c3" @ (getWord(MetroRPData.getData(%bGroup.bl_id).valueBank, 1) > 0 ? "$" @ getWord(MetroRPData.getData(%bGroup.bl_id).valueBank, 1) : "no money") @ "\c6.");

		messageClient(%client, '', "\c31 \c6- Withdraw money.");
		messageClient(%client, '', "\c32 \c6- Deposit money.");
		messageClient(%client, '', "\c33 \c6- Deposit all money.");

		%client.stage = 1;
		return;
	}
	
	%input = strLwr(%text);
	
	if(%client.stage == 1)
	{
		if(strReplace(%input, "1", "") !$= %input || strReplace(%input, "one", "") !$= %input)
		{
			%client.stage = 2.1;
			
			messageClient(%client, '', "\c6Please enter the amount of money you wish to take out.");
			
			return;
		}
		
		if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
		{
			%client.stage = 2.2;
			
			messageClient(%client, '', "\c6Please enter the amount of money you wish to put in.");
			
			return;
		}
		
		if(strReplace(%input, "3", "") !$= %input || strReplace(%input, "three", "") !$= %input)
		{
			%client.stage = 2.2;
			
			serverCmdMessageSent(%client, %client.Money);
			
			return;
		}

		
		messageClient(%client, '', "\c3" @ %text SPC "\c6is not a valid option!");
		
		return;
	}
	
	if(mFloor(%client.stage) == 2)
	{
		if(%client.stage == 2.1)
		{

			%input = mFloor(%input);
			if(%input < 1)
			{
				messageClient(%client, '', "\c6Please enter a valid amount of money to withdraw.");
				
				return;
			}
			
			if(%offline && getWord(MetroRPData.getData(%bGroup.bl_id).valueBank, 1) < %input || isObject(%bClient) && %bClient.Bank < %input)
			{
				if(%offline && getWord(MetroRPData.getData(%bGroup.bl_id).valueBank, 1) < 1 || isObject(%bClient) && %bClient.Bank < 1)
				{
					messageClient(%client, '', "\c6There's no money in this safe to withdraw.");
					
					%client.stage = 0;
					
					return;
				}
				
				%input = (%offline ? getWord(MetroRPData.getData(%bGroup.bl_id).valueBank, 1) : %bClient.Bank);
			}

			messageClient(%client, '', "\c6You have taken out \c3$" @ %input @ "\c6.");


			%client.Money += %input;

			if(%offline)
			{
				MetroRPData.getData(%bGroup.bl_id).valueBank = setWord(MetroRPData.getData(%bGroup.bl_id).valueBank, 1, getWord(MetroRPData.getData(%bGroup.bl_id).valueBank, 1) - %input);
			}
			else
			{
				%bClient.Vault -= %input;
			}

			%client.stage = 0;
			%client.SetInfo();
		}
		
		if(%client.stage == 2.2)
		{
			%input = mFloor(%input);
			if(%input < 1)
			{
				messageClient(%client, '', "\c6Please enter a valid amount of money to put in the safe.");
				
				return;
			}
			
			if(%client.Money < %input)
			{
				if(%client.Money < 1)
				{
					messageClient(%client, '', "\c6You don't have any money to put in the safe.");
					
					%client.stage = 0;
					
					return;
				}
				
				%input = %client.Money;
			}

			%client.Money -= %input;

			if(%offline)
			{
				MetroRPData.getData(%bGroup.bl_id).valueBank = setWord(MetroRPData.getData(%bGroup.bl_id).valueBank, 1, getWord(MetroRPData.getData(%bGroup.bl_id).valueBank, 1) + %input);
			}
			else
			{
				%bClient.Vault += %input;
			}

			messageClient(%client, '', "\c6You have put \c3$" @ %input @ "\c6 in the safe!");

			%client.SetInfo();
			%client.stage = 0;
		}
	}
}

