// Package
package CityDrugs {
  function City_OnPlant(%brick, %lotTrigger) {
    %result = Parent::City_OnPlant(%brick, %lotTrigger);
    %client = %brick.client;

    if(%result == -1 || %brick.getDataBlock().CityRPGBrickType != 420 || !isObject(%client)) {
      return %result;
    }

    if(CityRPGData.getData(%client.bl_id).valueMoney >= mFloor(%brick.getDatablock().price)) {
      if(CityRPGData.getData(%client.bl_id).valuedrugamount <= $CityRPG::drug::maxdrugplants) {
        schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
        CityRPGData.getData(%client.bl_id).valueMoney -= %brick.getDatablock().price;
        CityRPGData.getData(%client.bl_id).valuedrugamount++;
        %drug = %brick.getID();
        %drug.canchange = true;
        %drug.isGrowing = false;
        %drug.grew = false;
        %drug.watered = false;
        %drug.isDrug = true;
        %drug.currentColor = 45;
        %drug.setColor(45);
        %drug.owner = %client.bl_id;
        %drug.hasemitter = true;
        %drug.growtime = 0;
        %drug.health = 0;
        %drug.orighealth = %drug.health;

        if(%brick.getDataBlock().drugType $= "marijuana") {
          %drug.random = getRandom($CityRPG::drugs::marijuana::harvestMin,$CityRPG::drugs::marijuana::harvestMax);
          %drug.uiName = "Marijuana";
          messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
        }
        else if(%brick.getDataBlock().drugType $= "opium") {
          %drug.random = getRandom($CityRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
          %drug.uiName = "opium";
          messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
        }
        else if(%brick.getDataBlock().drugType $= "speed") {
          %drug.random = getRandom($CityRPG::drugs::speed::harvestMin,$CityRPG::drugs::speed::harvestMax);
          %drug.uiName = "speed";
          messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
        }
        else if(%brick.getDataBlock().drugType $= "steroid") {
          %drug.random = getRandom($CityRPG::drugs::steroid::harvestMin,$CityRPG::drugs::steroid::harvestMax);
          %drug.uiName = "steroid";
          messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
        }

        %drug.canbecolored = false;
        %drug.setEmitter("None");
        %drug.cansetemitter = false;
        %drug.canchange = false;
      }
      else {
        commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
        %brick.schedule(0, "delete");
        return -1;
      }
    }
    else {
      commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
      %brick.schedule(0, "delete");
      return -1;
    }
  }

  function fxDTSBrick::onRemove(%brick,%client)
  {
    if(%brick.getDatablock().CityRPGBrickType == 420) {
      %brick.handleCityRPGBrickDelete();
    }

    parent::onRemove(%brick);
  }

  function fxDTSBrick::onActivate(%brick, %obj, %client, %pos, %dir)
  {
    parent::onActivate(%brick, %obj, %client, %pos, %dir);

    if(%brick.getDataBlock().hasDrug == false)
    {
      if(%brick.getDataBlock().isDrug == true)
      {
        %drug = %brick.getID();
        if(%drug.watered == 0)
        {
          %drug.watered = 1;

          if(%client.bl_id == %brick.client.bl_id || %client.name == %brick.client.getPlayerName())
          {
            messageClient(%client,'',"\c6You watered your \c3" @ %brick.getDataBlock().uiName @ " \c6plant.");
          }
          else
          {
            messageClient(%client,'',"\c6You watered someones \c3" @ %brick.getDataBlock().uiName @ " \c6plant.");
          }
          %drug.uiName = %brick.getDataBlock().uiName;
          %drug.startGrowing(%drug,%brick);
        }
        else if(%drug.watered == 1)
        {
          if(%drug.hasDrug)
            commandToClient(%client,'centerPrint',"\c6This \c3" @ %brick.getDataBlock().uiName @ " \c6plant is ready to be harvested!",1);
          else
            commandToClient(%client,'centerPrint',"\c6This plant is already watered.",1);
        }
      }
    }
  }

	function Armor::onCollision(%this, %obj, %col, %thing, %other)
	{
		if(%col.getDatablock().getName() $= "mariItem")
		{
			if(isObject(%obj.client))
			{
				if(isObject(%col))
				{
          %obj.client.cityLog("Pick up " @ %col.value @ " grams of marijuana");

					if(%obj.client.minigame)
						%col.minigame = %obj.client.minigame;

					CityRPGData.getData(%obj.client.bl_id).valueMarijuana += %col.value;
					CityRPGData.getData(%obj.client.bl_id).valuetotaldrugs += %col.value;
					messageClient(%obj.client,'',"\c6You have picked up \c3" @ %col.value @ " \c3grams\c6 of marijuana.");

					%obj.client.SetInfo();
					%col.canPickup = false;
					%col.delete();
				}
				else
				{
					%col.delete();
					MissionCleanup.remove(%col);
				}
			}
		}

		Parent::onCollision(%this, %obj, %col, %thing, %other);
	}

	function mariItem::onAdd(%this, %item, %b, %c, %d, %e, %f, %g)
	{
		parent::onAdd(%this, %item, %b, %c, %d, %e, %f, %g);
		schedule($Pref::Server::City::moneyDieTime, 0, "eval", "if(isObject(" @ %item.getID() @ ")) { " @ %item.getID() @ ".delete(); }");
	}

  function CityRPGLBImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
  {
    if(%col.getClassName() $= "fxDTSBrick" && %col.getDatablock().isDrug)
    {
      if(%col.isPlanted())
      {
        schedule(3000, 0, addEvid, %col, %obj.client);
      }
    }

    Parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
  }

  function CityRPGBatonImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
  {
    if(%col.getClassName() $= "fxDTSBrick" && %col.getDatablock().isDrug)
    {
      if(%col.isPlanted())
      {
        schedule(3000, 0, addEvid, %col, %obj.client);
      }
    }

    Parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
  }

  function CityRPGBatonImage::onCityPlayerHit(%this, %obj, %slot, %col, %pos, %normal)
  {
    if(((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth) >= $Pref::Server::City::demerits::wantedLevel)
    {
      %col.client.getWantedLevel();
      if(%col.getDatablock().maxDamage - (%col.getDamageLevel() + 25) < %this.raycastDirectDamage)
      {
        CityRPGData.getData(%col.client.bl_id).valueDemerits += ((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth);
        %col.setDamageLevel(%this.raycastDirectDamage + 1);
        %col.client.arrest(%client);
        CityRPGData.getData(%col.client.bl_id).valuemarijuana -= CityRPGData.getData(%col.client.bl_id).valuemarijuana;
        CityRPGData.getData(%col.client.bl_id).valuetotaldrugs -= CityRPGData.getData(%col.client.bl_id).valuetotaldrugs;
      }
      else
      {
        commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6is carrying drugs!", 3);
        if(CityRPGData.getData(%col.client.bl_id).valueDemerits < $Pref::Server::City::demerits::wantedLevel)
        {
          CityRPGData.getData(%col.client.bl_id).valueDemerits += ((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth);
        }
      }

      // Return true to indicate that the baton check is being used.
      // Skip the parent call to override any other potential checks.
      return true;
    }

    Parent::onCityPlayerHit(%this, %obj, %slot, %col, %pos, %normal);
  }

  function CityRPGLBImage::onCityPlayerHit(%this, %obj, %slot, %col, %pos, %normal)
  {
    if(((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth) >= $Pref::Server::City::demerits::wantedLevel)
    {
      %col.client.getWantedLevel();
      if(%col.getDatablock().maxDamage - (%col.getDamageLevel() + 25) < %this.raycastDirectDamage)
      {
        CityRPGData.getData(%col.client.bl_id).valueDemerits += ((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth);
        %col.setDamageLevel(%this.raycastDirectDamage + 1);
        %col.client.arrest(%client);
        CityRPGData.getData(%col.client.bl_id).valuemarijuana -= CityRPGData.getData(%col.client.bl_id).valuemarijuana;
        CityRPGData.getData(%col.client.bl_id).valuetotaldrugs -= CityRPGData.getData(%col.client.bl_id).valuetotaldrugs;
      }
      else
      {
        commandToClient(%client, 'CenterPrint', "\c3" @ %col.client.name SPC "\c6is carrying drugs!", 3);
        if(CityRPGData.getData(%col.client.bl_id).valueDemerits < $Pref::Server::City::demerits::wantedLevel)
        {
          CityRPGData.getData(%col.client.bl_id).valueDemerits += ((CityRPGData.getData(%col.client.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth);
        }
      }

      // Return true to indicate that the baton check is being used.
      // Skip the parent call to override any other potential checks.
      return true;
    }
  }

  function knifeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal,%brick)
  {
    if(%col.getClassName() $= "fxDTSBrick")
    {
      %brickData = %col.getDatablock();
      %drug = %col.getID();
      if(%brickData.isDrug)
      {
        %col.harvest(%obj.client);
      }
    }
    parent::onCollision(%this,%obj,%col,%fade,%pos,%normal,%brick);
  }

  function disconnect()
  {
    if(!$Server::Dedicated && CityRPGData.scheduleDrug)
      cancel(CityRPGData.scheduleDrug);

    parent::disconnect();
  }

  function gameConnection::arrest(%client, %cop)
  {
    %robSO.valuemarijuana = 0;
    %robSO.valueopium = 0;
    %robSO.valuetotaldrugs = 0;

    parent::arrest(%client, %cop);
  }

  function fxDTSBrick::onDeath(%brick)
  {
    if(%brick.getDataBlock().isDrug)
    {
      CityRPGData.getData(%brick.owner).valuedrugamount--;

      if(isObject(getBrickGroupFromObject(%brick).client))
      {
        getBrickGroupFromObject(%brick).client.SetInfo();
      }
    }

    parent::onDeath(%brick);
  }

  function City_illegalAttackTest(%atkr, %vctm)
  {
    if(isObject(%atkr) && isObject(%vctm) && %atkr.getClassName() $= "GameConnection" && %vctm.getClassName() $= "GameConnection")
    {
      if(%atkr != %vctm)
      {
        if(((CityRPGData.getData(%vctm.bl_id).valuetotaldrugs) * $CityRPG::drug::demWorth) >= $Pref::Server::City::demerits::wantedLevel)
          return false;
      }
    }

    // Resort to the rest of the checks if this one does not pass
    parent::City_illegalAttackTest(%atkr, %vctm);
  }

  function fxDTSBrick::handleCityRPGBrickDelete(%brick, %data)
  {
    if(isObject(%brick.trigger))
    {
      if(%brick.getDatablock().CityRPGBrickType == 420)
      {
        getBrickGroupFromObject(%brick).valuedrugamount--;
        if(isObject(getBrickGroupFromObject(%brick).client))
          getBrickGroupFromObject(%brick).client.SetInfo();

        CityRPGData.getData(%client.bl_id).valuedrugamount--;
      }
    }
  }
};
deactivatePackage(CityDrugs);
activatepackage(CityDrugs);
