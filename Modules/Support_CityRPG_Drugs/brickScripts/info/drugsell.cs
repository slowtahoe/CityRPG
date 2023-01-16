// ============================================================
// Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGDrugsellBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "CityRPG Infoblock";

	uiName = "Drug Sell Brick";

	CityRPGBrickType = $CityBrick_Info;
	CityRPGBrickAdmin = true;

	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};

// ============================================================
// Trigger Data
// ============================================================
function CityRPGDrugsellBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	for(%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%clientCheck = ClientGroup.getObject(%i);
		if((%clientCheck.getJobSO().law) == true)
		{
			%disableDrugs = false;
		}
		else
		{
			%disableDrugs = true;
		}
	}

	if((%disableDrugs == true) && (%triggerStatus == true && %client.stage $= ""))
	{
		commandToClient(%client,'centerPrint',"\c6No law enforcement online. Drug selling is disabled.",3);
	}
	else
	{
		if(%triggerStatus !$= "")
		{
			if(%triggerStatus == true && %client.stage $= "")
			{
				messageClient(%client, '', "\c3Drug Market");
				messageClient(%client, '', "\c6What do you want to sell? \c6(New? /drughelp)");
				messageClient(%client, '', "\c6Type a number in chat:");

				messageClient(%client, '', "\c31 \c6- Sell Marijuana.");
				messageClient(%client, '', "\c32 \c6- Sell Opium.");
				messageClient(%client, '', "\c33 \c6- Sell Speed.");
				messageClient(%client, '', "\c34 \c6- Sell Steroid.");

				%client.drugname = "";
				%client.selling = false;

				%client.stage = 0;
			}

			if(%triggerStatus == false && %client.stage !$= "")
			{
				%client.selling = false;
				messageClient(%client, '', "\c6You are no longer selling.");
				%client.drugname = "";
				%client.stage = "";
			}

			return;
		}

		%input = strLwr(%text);

		if((mFloor(%client.stage) == 0) && (%disableDrugs == false))
		{
			if(strReplace(%input, "1", "") !$= %input || strReplace(%input, "one", "") !$= %input)
			{
				%client.stage = 1.1;

				if(CityRPGData.getData(%client.bl_id).valueMarijuana >= 1)
				{
					messageClient(%client,'',"\c6You have started selling.");
					%client.drugname = "marijuana";
					%client.selling = true;
				}
				else
				{
					messageClient(%client,'',"\c3You don't have any marijuana to sell.");
					%client.drugname = "";
					%client.selling = false;
				}
			}
		}

		if((mFloor(%client.stage) == 0) && (%disableDrugs == false))
		{
			if(strReplace(%input, "2", "") !$= %input || strReplace(%input, "two", "") !$= %input)
			{
				%client.stage = 1.2;

				if(CityRPGData.getData(%client.bl_id).valueopium >= 1)
				{
					messageClient(%client,'',"\c6You have started selling.");
					%client.drugname = "opium";
					%client.selling = true;
				}
				else
				{
					messageClient(%client,'',"\c3You don't have any opium to sell.");
					%client.drugname = "";
					%client.selling = false;
				}
			}
		}
		if((mFloor(%client.stage) == 0) && (%disableDrugs == false))
		{
			if(strReplace(%input, "3", "") !$= %input || strReplace(%input, "three", "") !$= %input)
			{
				%client.stage = 1.3;

				if(CityRPGData.getData(%client.bl_id).valuespeed >= 1)
				{
					messageClient(%client,'',"\c6You have started selling.");
					%client.drugname = "speed";
					%client.selling = true;
				}
				else
				{
					messageClient(%client,'',"\c3You don't have any speed to sell.");
					%client.drugname = "";
					%client.selling = false;
				}
			}
		}
		if((mFloor(%client.stage) == 0) && (%disableDrugs == false))
		{
			if(strReplace(%input, "4", "") !$= %input || strReplace(%input, "four", "") !$= %input)
			{
				%client.stage = 1.4;

				if(CityRPGData.getData(%client.bl_id).valuesteroid >= 1)
				{
					messageClient(%client,'',"\c6You have started selling.");
					%client.drugname = "steroid";
					%client.selling = true;
				}
				else
				{
					messageClient(%client,'',"\c3You don't have any steroids to sell.");
					%client.drugname = "";
					%client.selling = false;
				}
			}
		}
	}
}
