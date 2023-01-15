datablock fxDTSBrickData(CityRPGOpiumData)
{
	brickFile = "Add-Ons/Support_CityRPG_Drugs/brick_6x6.blb";
	iconName = "Add-Ons/Support_CityRPG_Drugs/opium";

	category = "CityRPG";
	subCategory = "CityRPG Drugs";

	uiName = "Opium";
	drugType = "Opium";

	owner = 0;
	canchange = false;
	cansetemitter = false;
	emitter = "GrassEmitter";
	isDrug = true;
	hasDrug = false;
	isOpium = true;
	isGrowing = false;
	growtime = 0;
	canbecolored = false;
	health = 100;
	orighealth = 100;

	watered = 0;
	grew = 0;

	price = $CityRPG::drugs::Opium::placePrice;

	harvestAmt = $CityRPG::drugs::Opium::harvestAmt;
	growthTime = $CityRPG::drugs::Opium::growthTime;

	CityRPGBrickType = 420;
	CityRPGBrickAdmin = false;
};
