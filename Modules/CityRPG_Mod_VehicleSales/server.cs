%error = ForceRequiredAddOn("GameMode_CityRPG4");
if(%error == $Error::AddOn_NotFound)
{
  error("ERROR: CityRPG_Mod_VehicleSales - required add-on GameMode_CityRPG4 not found");
  return;
}

exec("./CityRPG4_vehicleSales.cs");
exec("./vehiclePackage.cs");

