package CityRPG_Commands
{
	// ============================================================
	// Common
	// ============================================================
	function GameConnection::cityRateLimitCheck(%client)
	{
		%simTime = getSimTime()+0; // Hack to compare the time.
		if(%client.cityCommandTime+$City::CommandRateLimitMS > %simTime)
		{
			%client.cityCommandTime = %simTime;
			return 1;
		}
		else
		{
			%client.cityCommandTime = %simTime;
			return 0;
		}

		%client.cityCommandTime = %simTime;
	}

	// ============================================================
	// Player Commands
	// ============================================================
	function serverCmdhelp(%client, %strA, %strB, %strC)
	{
		%client.cityLog("/help" SPC %section SPC %term);

		switch$(%strA)
		{
			case "":
				messageClient(%client, '', "\c6Type " @ $c_p @ "/help starters\c6 for information to get started in CityRPG");
				messageClient(%client, '', "\c6Type " @ $c_p @ "/help commands\c6 to list the commands in the game");
				messageClient(%client, '', "\c6More: " @ $c_p @ "/help events\c6, " @ $c_p @ "/help admin\c6, " @ $c_p @ "/stats");


				if($GameModeArg $= "Add-Ons/GameMode_CityRPG4/gamemode.txt")
				{
					%sentenceStr = "\c6This server is running vanilla CityRPG 4";
				}
				else
				{
					%sentenceStr = "\c6This server is running a " @ $c_p @ "custom configuration\c6 of CityRPG 4";
				}

				if($City::isGitBuild)
				{
					%suffix = " (Git build)";
				}

				messageClient(%client, '', %sentenceStr @ " (" @ $c_p @ $City::Version @ "\c6)" @ %suffix);
			case "starters":
				messageClient(%client, '', "\c6Welcome! To get started, you'll want to explore and familiarize yourself with the map.");
				messageClient(%client, '', "\c6Some of the places most important to you will include the jobs office, the education office, and the bank.");
				messageClient(%client, '', "\c6Once you've taken some time to explore, go to the jobs office to apply for your first job.");
				messageClient(%client, '', "\c6From there, you can invest in education and assets to advance yourself in the city and work your way up the ladder.");
				messageClient(%client, '', "\c6Good luck!");
			case "commands":
				messageClient(%client, '', $c_p @ "/stats\c6 - View your stats");
				messageClient(%client, '', $c_p @ "/pardon\c6 [player] - Issue a pardon to a player in jail. Can only be used by officials.");
				messageClient(%client, '', $c_p @ "/eraseRecord\c6 [player] - Erases the record of a player, for a price. Can only be used by officials.");
				messageClient(%client, '', $c_p @ "/reset\c6 - Reset your in-game account. WARNING: This will clear your save data if typed!");
				messageClient(%client, '', $c_p @ "/dropmoney\c6 [amount] - Make it rain!");
				messageClient(%client, '', $c_p @ "/giveMoney\c6 [amount] [player] - Give money to another player");
				messageClient(%client, '', $c_p @ "/lot\c6 - View information about the lot you are standing on");

			case "events":
				messageClient(%client, '', "\c6 - brick -> " @ $c_p @ "sellFood\c6 [Food] [Markup] - Feeds a player using the automated sales system.");
				messageClient(%client, '', "\c6 - brick -> " @ $c_p @ "sellItem\c6 [Item] [Markup] - Sells an item using the automated system.");
				messageClient(%client, '', "\c6 - brick -> " @ $c_p @ "requestFunds\c6 [Service] [Price] - Requests $" @ $c_p @ "[Price]\c6 for " @ $c_p @ "[Service]\c6. Charge money to call events.");
				messageClient(%client, '', "\c6 - brick -> " @ $c_p @ "doJobTest\c6 [Job] [NoConvicts] - Tests if user's job is [Job]. NoConvicts will fail inmates. Calls onJobTestFail and onJobTestPass");
			case "admin":
				messageClient(%client, '', "\c6Type " @ $c_p @ "/admin\c6 to access the main admin panel.");
				messageClient(%client, '', "\c6Admin commands: /" @ $c_p @ "updateScore\c6, /" @ $c_p @ "setMinerals\c6 [" @ $c_p @ "value\c6], /" @ $c_p @ "setLumber\c6 [" @ $c_p @ "value\c6], /" @ $c_p @ "editEducation\c6 [" @ $c_p @ "level\c6] [" @ $c_p @ "player\c6]");
				messageClient(%client, '', "\c6/" @ $c_p @ "clearMoney\c6, \c6/" @ $c_p @ "gMoney\c6 [" @ $c_p @ "amount\c6] [" @ $c_p @ "player\c6], /" @ $c_p @ "dMoney\c6 [" @ $c_p @ "amount\c6] [" @ $c_p @ "player\c6], /" @ $c_p @ "cleanse\c6, /" @ $c_p @ "editHunger\c6 [" @ $c_p @ "level\c6], /" @ $c_p @ "manageLot");
				messageClient(%client, '', "\c6/" @ $c_p @ "resetAllJobs\c6");

			case "jobs":
				messageClient(%client, '', "\c6Visit the jobs office to view the available jobs.");

			default:
				messageClient(%client, '', "\c6Unknown help section. Please try again.");
		}
	}

	function serverCmdYes(%client)
	{
		if(%client.cityRateLimitCheck())
		{
			return;
		}

		%client.cityLog("/yes");

		if(!isObject(%client.player))
			return;

		if(isObject(%client.player) && isObject(%client.player.serviceOrigin))
		{
			if(mFloor(VectorDist(%client.player.serviceOrigin.getPosition(), %client.player.getPosition())) < 16)
			{
				if(City.get(%client.bl_id, "money") >= %client.player.serviceFee)
				{
					%ownerBL_ID = %client.player.serviceOrigin.getGroup().bl_id;
					switch$(%client.player.serviceType)
					{
						case "service":
							%client.cityLog("Evnt buy service for " @ %client.player.serviceFee @ " from " @ %sellerID);
							City.subtract(%client.bl_id, "money", %client.player.serviceFee);

							City.add(%client.player.serviceOrigin.getGroup().bl_id, "bank", %client.player.serviceFee);

							messageClient(%client, '', "\c6You have accepted the service fee of " @ $c_p @ "$" @ %client.player.serviceFee @ "\c6!");
							%client.setInfo();

						if(%client.player.serviceOrigin.getGroup().client)
							messageClient(%client.player.serviceOrigin.getGroup().client, '', $c_p @ %client.name @ "\c6 has wired you " @ $c_p @ "$" @ %client.player.serviceFee @ "\c6 for a service.");

						%client.player.serviceOrigin.onTransferSuccess(%client);

						case "food":
							%client.sellFood(%ownerBL_ID, %client.player.serviceSize, %client.player.serviceItem, %client.player.serviceFee, %client.player.serviceMarkup);

						case "item":
							%client.sellItem(%ownerBL_ID, %client.player.serviceItem, %client.player.serviceFee, %client.player.serviceMarkup);

						case "zone":
							%client.sellZone(%ownerBL_ID, %client.player.serviceOrigin, %client.player.serviceFee);

						case "clothes":
							%client.sellClothes(%ownerBL_ID, %client.player.serviceOrigin, %client.player.serviceItem, %client.player.serviceFee);
					}
				}
				else
				{
					messageClient(%client, '', "\c6You cannot afford this service.");
				}
			}
			else
			{
				messageClient(%client, '', "\c6You are too far away from the service to purchase it!");
			}
		}

		%client.player.serviceType = "";
		%client.player.serviceFee = "";
		%client.player.serviceMarkup = "";
		%client.player.serviceItem = "";
		%client.player.serviceSize = "";
		%client.player.serviceOrigin = "";
	}

	function serverCmdNo(%client)
	{
		%client.cityLog("/no");
		%serviceOrigin = %client.player.serviceOrigin;

		if(!isObject(%client.player))
			return;

		if(isObject(%serviceOrigin) || (!isObject(%serviceOrigin) && %serviceOrigin !$= ""))
		{
			messageClient(%client, '', "\c6You have rejected the service fee!");

			if(isObject(%serviceOrigin))
			{
				%serviceOrigin.onTransferDecline(%client);
			}

			%client.player.serviceType = "";
			%client.player.serviceFee = "";
			%client.player.serviceMarkup = "";
			%client.player.serviceItem = "";
			%client.player.serviceSize = "";
			%client.player.serviceOrigin = "";
		}
	}

	function serverCmddonate(%client, %arg1)
	{
		%client.cityLog("/donate" SPC %arg1);

		if(!isObject(%client.player))
			return;

		%arg1 = mFloor(%arg1);

		if(%arg1*0.15+$City::Economics::Condition > $Pref::Server::City::Economics::Cap) {
			%arg1 = mFloor(($Pref::Server::City::Economics::Cap-$City::Economics::Condition)/0.15);
		}

		if(%arg1 <= 0)
		{
			return;
		}

		if($City::Economics::Condition > $Pref::Server::City::Economics::Cap)
		{
			messageClient(%client, '', "\c6The economy is currently at the maxiumum rate. Please try again later.");
			return;
		}

		if((City.get(%client.bl_id, "money") - %arg1) < 0)
		{
			messageClient(%client, '', "\c6You don't have that much money to donate to the economy.");
			return;
		}

		%amoutPer = %arg1 * 0.15;
		City.subtract(%client.bl_id, "money", %arg1);
		messageClient(%client, '', "\c6You've donated " @ $c_p @ "$" @ %arg1 SPC "\c6to the economy! (" @ %amoutPer @ "%)");
		messageAll('',$c_p @ %client.name SPC "\c6has donated " @ $c_p @ "$" @ %arg1 SPC "\c6to the economy! (" @ %amoutPer @ "%)");
		$City::Economics::Condition = $City::Economics::Condition + %amoutPer;
		%client.setGameBottomPrint();
	}

	function serverCmdbuyErase(%client)
	{
		if(%client.cityRateLimitCheck())
		{
			return;
		}

		%client.cityLog("/buyErase");

		if(!isObject(%client.player))
			return;

		%cost = %client.getCityRecordClearCost();
		if((City.get(%client.bl_id, "money") - %cost) < 0)
		{
			messageClient(%client, '', "\c6You don't have $" @ %cost @ ".");
			return;
		}

		if(!getWord(City.get(%client.bl_id, "jaildata"), 0))
		{
			messageClient(%client, '', %target @ "\c6You do not have a criminal record.");
			return;
		}

		if(City.get(%client.bl_id, "money") < %cost && !%client.isAdmin)
		{
			messageClient(%client, '', "\c6You need at least " @ $c_p @ "$" @ %cost SPC "\c6to erase someone's record.");
			return;
		}

		City.set(%client.bl_id, "jaildata", "0" SPC getWord(City.get(%client.bl_id, "jaildata"), 1));
		messageClient(%client, '', "\c6You have erased your criminal record.");
		%client.spawnPlayer();
		%client.setInfo();
		City.subtract(%client.bl_id, "money", %cost);
	}

	function serverCmdgiveMoney(%client, %money, %name)
	{
		if(%client.cityRateLimitCheck())
		{
			return;
		}

		%client.cityLog("/giveMoney" SPC %money SPC %name);

		if(!isObject(%client.player))
			return;

		%money = mFloor(%money);

		if(%money <= 0)
		{
			messageClient(%client, '', "\c6You must enter a valid amount of money to give.");
			return;
		}

		if((City.get(%client.bl_id, "money") - %money) < 0)
		{
			messageClient(%client, '', "\c6You don't have that much money to give.");
			return;
		}

		if(!isObject(%client.player))
		{
			messageClient(%client, '', "\c6Spawn first before you use this command.");
			return;
		}

		if(%name !$= "")
		{
			%target = findclientbyname(%name);
		}
		else
		{
			%target = containerRayCast(%client.player.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%client.player.getEyeVector()), 5), %client.player.getEyePoint()), $typeMasks::playerObjectType,%client.player).client;
		}

		if(!isObject(%target))
		{
			messageClient(%client, '', "\c6You must be looking at and be in a reasonable distance of the player in order to give them money. \nYou can also type in the person's name after the amount.");
			return;
		}

		%client.cityLog("Give money to " @ %target.bl_id);
		messageClient(%client, '', "\c6You give " @ $c_p @ "$" @ %money SPC "\c6to " @ $c_p @ %target.name @ "\c6.");
		messageClient(%target, '', $c_p @ %client.name SPC "\c6has given you " @ $c_p @ "$" @ %money @ "\c6.");

		City.subtract(%client.bl_id, "money", %money);
		City.add(%target.bl_id, "money", %money);

		%client.SetInfo();
		%target.SetInfo();
	}

	function serverCmdjobs(%client, %str1, %str2, %str3, %str4)
	{
		if(%client.cityRateLimitCheck() || !isObject(%client.player))
		{
			return;
		}

		%client.cityLog("/jobs" SPC %job SPC %job2 SPC %job3 SPC %job4 SPC %job5);

		// Combine the job input.
		// Trim spaces for args that are not used.
		%jobInput = rtrim(%str1 SPC %str2 SPC %str3 SPC %str4);
		%jobObject = findJobByName(%jobInput);

		if(!isObject(%jobObject))
		{
			messageClient(%client, '', "\c6No such job. Please try again.");
			return;
		}

		%client.setCityJob(%jobObject.id);
	}

	function serverCmdreset(%client)
	{
		messageClient(%client, '', "\c6Please ask an admin for help if you want to reset your account.");
	}

	function serverCmdeducation(%client, %do) {
		messageClient(%client, '', "\c6Find the education office to enroll for an education.");
	}

	function serverCmdpardon(%client, %name)
	{
		if(%client.cityRateLimitCheck())
		{
			return;
		}

		%client.cityLog("/pardon" SPC %name);

		if(!isObject(%client.player))
			return;

		if(!%client.getJobSO().canPardon && !%client.isCityAdmin())
		{
			messageClient(%client, '', "\c6You can't pardon people.");
			return;
		}

		if(%name $= "")
		{
			messageClient(%client, '' , "\c6Please enter a name.");
			return;
		}

		%target = findClientByName(%name);
		if(!isObject(%target))
		{
			messageClient(%client, '', "\c6That person does not exist.");
			return;
		}

		if(!getWord(City.get(%target.bl_id, "jaildata"), 1))
		{
			messageClient(%client, '', "\c6That person is not a convict.");
			return;
		}

		if(!%client.isCityAdmin() && %target == %client)
		{
			messageClient(%client, '', "\c6The extent of your legal corruption only goes so far. You cannot pardon yourself.");
			return;
		}

		%client.pardonTarget = %target;
		%jailTime = getWord(City.get(%target.bl_id, "jailData"), 1);

		%client.cityLog("Pardon prompt for " @ %target.bl_id);

		if(%client.isCityAdmin() && %target != %client)
		{
			%client.cityMenuMessage("\c6You are about to pardon " @ $c_p @ %target.name @ "\c6 from their " @ $c_p @ %jailTime @ "\c6 remaining days in prison using your magic admin powers.");
			%client.cityMenuMessage("\c6Type " @ $c_p @ "1\c6 in chat to confirm, or " @ $c_p @ "2\c6 to cancel.");
		}
		else if(%client.isCityAdmin())
		{
			%client.cityMenuMessage("\c6You are about to pardon yourself using your magic admin powers.");
			%client.cityMenuMessage("\c6Type " @ $c_p @ "1\c6 in chat to confirm, or " @ $c_p @ "2\c6 to cancel.");
		}
		else
		{
			%client.cityMenuMessage("\c6You are about to pardon " @ $c_p @ %target.name @ "\c6 from their " @ $c_p @ %jailTime @ "\c6 remaining days in prison.");
			%client.cityMenuMessage("\c6Proceeding will negatively impact the economy by up to \c0" @ %jailTime * $Pref::Server::City::demerits::pardonCostMultiplier @ "%\c6. Type " @ $c_p @ "1\c6 in chat to confirm, or " @ $c_p @ "2\c6 to cancel.");
		}

		%functions = "CityMenu_Pardon";
		%client.cityMenuOpen("", %functions, %client, "Pardon cancelled.", 0, 1);
	}

	function CityMenu_Pardon(%client, %input)
	{
		if(%input !$= "1")
		{
			%client.cityMenuClose();
			return;
		}

		%client.cityMenuClose(1);

		// Security check
		if(!%client.getJobSO().canPardon && !%client.isCityAdmin())
		{
			messageClient(%client, '', "You are no-longer able to pardon people.");
			return;
		}

		// Extract the cost
		if(!%client.isCityAdmin())
		{
			%jailTime = getWord(City.get(%target.bl_id, "jailData"), 1);
			$City::Economics::Condition -= %jailTime * $Pref::Server::City::demerits::pardonCostMultiplier;
		}

		%target = %client.pardonTarget;

		if(%target != %client)
		{
			messageClient(%client, '', "\c6You have let" @ $c_p SPC %target.name SPC "\c6out of prison.");
			messageClient(%target, '', $c_p @ %client.name SPC "\c6has issued you a pardon.");
		}
		else
		{
			messageClient(%client, '', "\c6You have pardoned yourself.");
		}

		City.set(%target.bl_id, "jailData", getWord(City.get(%target.bl_id, "jailData"), 0) SPC 0);

		%target.buyResources();
		%target.spawnPlayer();
		%client.SetInfo();
	}

	function serverCmderaseRecord(%client, %name)
	{
		if(%client.cityRateLimitCheck())
		{
			return;
		}

		%client.cityLog("/eraseRecord" SPC %name);

		if(!%client.getJobSO().canPardon && %client.BL_ID != getNumKeyID())
		{
			messageClient(%client, '', "\c6You can't erase people's record!");
			return;
		}

		if(%name $= "")
		{
			messageClient(%client, '' , "\c6Please enter a name.");
			return;
		}

		%target = findClientByName(%name);
		if(!isObject(%target))
		{
			messageClient(%client, '', "\c6That person does not exist.");
			return;
		}

		if(!getWord(City.get(%target.bl_id, "jaildata"), 0))
		{
			messageClient(%client, '', "\c6That person does not have a criminal record.");
			return;
		}

		%cost = $Pref::Server::City::demerits::recordShredCost;
		if(City.get(%client.bl_id, "money") < %cost && !%client.isAdmin)
		{
			messageClient(%client, '', "\c6You need at least " @ $c_p @ "$" @ %cost SPC "\c6to erase someone's record.");
			return;
		}

		City.set(%target.bl_id, "jaildata", "0" SPC getWord(City.get(%target.bl_id, "jaildata"), 1));
		if(%target != %client)
		{
			messageClient(%client, '', "\c6You have ran" @ $c_p SPC %target.name @ "\c6's criminal record through a paper shredder.");
			messageClient(%target, '', $c_p @ "It seems your criminal record has simply vanished...");

			if(!%client.BL_ID == getNumKeyID())
				City.subtract(%client.bl_id, "money", %cost);
		}
		else
			messageClient(%client, '', "\c6You have erased your criminal record.");

		%target.spawnPlayer();
		%client.setInfo();
		
		return true;
	}

	function serverCmdReincarnate(%client, %do)
	{
		if(%client.cityRateLimitCheck())
		{
			return;
		}

		%client.cityLog("/reincarnate" SPC %do);

		if(City.get(%client.bl_id, "reincarnated"))
		{
			messageClient(%client, '', "\c6You have already reincarnated.");
			return;
		}

		if(%do $= "accept")
		{
			if((City.get(%client.bl_id, "money") + City.get(%client.bl_id, "bank")) >= 100000)
			{
				%client.doReincarnate();
			}
		}
		else
		{
			messageClient(%client, '', "\c6Reincarnation is a method for those who are on top to once again replay the game.");
			messageClient(%client, '', "\c6It costs $100,000 to Reincarnate yourself. Your account will almost completely reset.");
			messageClient(%client, '', "\c6The perks of doing this are...");
			messageClient(%client, '', "\c6 - You will start with a level " @ $City::EducationReincarnateLevel @ " education (+" @ $City::EducationReincarnateLevel-$City::EducationCap @ " maximum)");
			messageClient(%client, '', "\c6 - Your name will be yellow by default and white if you are wanted.");
			messageClient(%client, '', "\c6Type " @ $c_p @ "/reincarnate accept\c6 to start anew!");
		}
	}

	function serverCmddropmoney(%client,%amt)
	{
		if(%client.cityRateLimitCheck())
		{
			return;
		}

		%client.cityLog("/dropmoney" SPC %amt);

		%amt = mFloor(%amt);

		if(%amt <= 0)
		{
			messageClient(%client,'',"\c6You must enter a valid amount of money to drop.");
			return;
		}

		if($City::Cache::DroppedCash[%client.bl_id] > 30)
		{
			messageClient(%client,'',"\c6You're dropping too much cash! Wait a while, or pick up some of your dropped cash before dropping more.");
			return;
		}

		if(City.get(%client.bl_id, "money") < %amt)
		{
			messageClient(%client,'',"\c6You don't have that much money to drop!");
			return;
		}

		%cash = new Item()
		{
			datablock = cashItem;
			canPickup = false;
			value = %amt;
			dropper = %client;
		};

		%cash.setTransform(setWord(%client.player.getTransform(), 2, getWord(%client.player.getTransform(), 2) + 4));
		%cash.setVelocity(VectorScale(%client.player.getEyeVector(), 10));
		MissionCleanup.add(%cash);
		%cash.setShapeName("$" @ %cash.value);
		City.set(%client.bl_id, "money", City.get(%client.bl_id, "money") - %amt);
		%client.setInfo();

		$City::Cache::DroppedCash[%client.bl_id]++;

		messageClient(%client,'',"\c6You drop " @ $c_p @ "$" @ %amt @ ".");
		%client.cityLog("Drop '$" @ %amt @ "'");
	}

	function serverCmdstats(%client, %name)
	{
		if(%client.cityRateLimitCheck())
		{
			return;
		}

		%client.cityLog("/stats" SPC %name);

		if(!isObject(%client.player))
			return;

		if(%client.isAdmin && %name !$= "")
			%target = findClientByName(%name);
		else
			%target = %client;

		if(isObject(%target))
		{
			%job = %target.getJobSo();

			// Career
			%string = "Career:" SPC $c_p @ %target.getJobSO().track;

			// Title
			if(%job.title !$= "")
			{
				%string = %string @ "\n" @ "Title:" SPC %job.title SPC %target.name;
			}

			// Job
			%string = %string @ "\n" @ "Job:" SPC %job.name;

			// Net worth
			%string = %string @ "\n" @ "Net worth:" SPC $c_p @ "$" @ (City.get(%target.bl_id, "money") + City.get(%target.bl_id, "bank"));

			// Crim record
			%string = %string @ "\n" @ "Criminal record:" SPC $c_p @ (getWord(City.get(%target.bl_id, "jaildata"), 0) ? "Yes" : "No");

			// Education
			%level = City.get(%target.bl_id, "education");
			if($CityRPG::EducationStr[%level] !$= "")
			{
				%eduString = $CityRPG::EducationStr[%level];
			}
			else
			{
				%eduString = "Level " @ %level;
			}
			%string = %string @ "\n" @ "Education:" SPC $c_p @ %eduString;
			
			// Lots visited
			%lotsVisited = getWordCount(City.get(%target.bl_id, "lotsvisited"));
			%string = %string @ "\nLots visited: " @ (City.get(%target.bl_id, "lotsvisited") == -1? 0 : %lotsVisited);


			commandToClient(%client, 'MessageBoxOK', "Stats for " @ %target.name, %string);
		}
		else
			messageClient(%client, '', "\c6Either you did not enter or the person specified does not exist.");
	}

	function serverCmdjob(%client, %job, %job2, %job3, %job4, %job5)
	{
		%client.cityLog("/job [...]");

		serverCmdjobs(%client, %job, %job2, %job3, %job4, %job5);
	}

	function serverCmdLot(%client)
	{
		if(%client.cityMenuOpen)
		{
			if(isObject(%client.cityMenuID.dataBlock) && %client.cityMenuID.dataBlock.CityRPGBrickType == $CityBrick_Lot)
			{
				// If the open menu is a lot menu, close it.
				%client.cityMenuClose();
			}

			return;
		}

		CityMenu_Lot(%client);
	}
	function serverCmdgetposition(%client)
	{
		messageClient(%client,'',"<color:FFFFFF>You are at "@%client.player.position);
	}
};

deactivatePackage("CityRPG_Commands");
activatePackage("CityRPG_Commands");
