    
package SELLVEHICLE
{
    function fxDTSBrick::setVehicle(%brick, %vehicle, %client)
    {
        %client.setVehicleBrick = %brick;
        %client.setVehicleVehicle = %vehicle;
        echo("ui name <" @ %vehicle.uiname @ ">");

        if(%client.skipMessageBoxOK)
        {
            %client.skipMessageBoxOK = false;
            return parent::setVehicle(%brick, %vehicle, %client);
        }
        if(%vehicle.uiname !$= "")
        {
            commandToClient(%client, 'MessageBoxYesNo', "Purchase", "\c6Would you like to buy a\c3" SPC %vehicle.uiname SPC "\c6for\c3 $" @ $VehiclePrice[%vehicle.uiname], 'ConfirmVehiclePurchase');
        }
        else
        {
            return parent::setVehicle(%brick, %vehicle, %client);
        }
	}

    function serverCmdConfirmVehiclePurchase(%client)
    {

        %brick = %client.setVehicleBrick;
        %vehicle = %client.setVehicleVehicle;
        
        if(%brick.getDatablock().getName() !$= "CityRPGPoliceVehicleData")
        {
            if(isObject(%client.player)) 
            {
                if(%vehicle == %brick.vehicleDataBlock) return messageClient(%client,'',"\c3You already own this vehicle!"); //Check if vehicle spawned already has this vehicle set, exits if it is
                if($VehiclePrice[%vehicle.uiname] $= "") return messageClient(%client,'',"\c3This vehicle is not for sale!"); //Check if there is a vehicle price, exits if not
                if(CityRPGData.getData(%client.bl_id).valueMoney >= $VehiclePrice[%vehicle.uiname]) 
                { //Checks if client has enough money, More or equal to price
                    messageClient(%client,'',"\c6You have purchased a\c3" SPC %vehicle.uiname SPC "\c6for\c3 $" @ $VehiclePrice[%vehicle.uiname]); //Messages client  
                    messageClient(%client,'',"\c6You can now spawn a \c3" @ %vehicle.uiname);  
                    CityRPGData.getData(%client.bl_id).valueMoney -= $VehiclePrice[%vehicle.uiname]; //Takes money from client    
                    
                    %client.SetBottomPrint(); //Updates bottom print  
                    %client.skipMessageBoxOK = true;
                } 
                else 
                {
                    messageClient(%client,'',"\c3You do not have enough money!"); //Gives this message if client does not have enough money
            }
        }           
        else parent::setVehicle(%brick, %vehicle, %client); //Spawns vehicle if client is not on server (To prevent loss when uploading a save)
        
    if(!isObject(%brick.getGroup().client) || !%brick.getGroup().client.isAdmin)
	    {
		    if(isObject(%vehicle))
		    {
			    for(%a = 0; $CityRPG::vehicles::banned[%a] !$= "" && !%hasBeenBanned; %a++)
			    {
				    if(%vehicle.getName() $= $CityRPG::vehicles::banned[%a])
				    {
					    if(isObject(%brick.getGroup().client))
					    {
						    messageClient(%brick.getGroup().client, '', "\c6Standard users may not spawn a\c3" SPC %vehicle.uiName @ "\c6.");
					    }
					        %vehicle = 0;
					        %hasBeenBanned = true;
                        }    
                    }        
				}
			}
		}    
    }
};
deactivatePackage(SELLVEHICLE);
activatePackage(SELLVEHICLE);

