forceRequiredAddOn("GameMode_CityRPG4");

if($City::Loaded)
{
  exec("./Player_Fast.cs");
  exec("./drugs.cs");
  exec("./drugPackage.cs");
}
else
{
  error("Support_CityRPG_Drugs - Unable to load CityRPG.");
}
