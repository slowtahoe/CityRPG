datablock fxDTSBrickData(CityRPGSteroidData)
{
	brickFile = "Add-Ons/Support_CityRPG_Drugs/brick_6x6.blb";
	iconName = "Add-Ons/Support_CityRPG_Drugs/mushroom";

	category = "CityRPG";
	subCategory = "CityRPG Drugs";

	uiName = "Steroid";
	drugType = "Steroid";

	owner = 0;
	canchange = false;
	cansetemitter = false;
	emitter = "ArrowVanishEmitter";
	isDrug = true;
	hasDrug = false;
	isSteroid = true;
	isGrowing = false;
	growtime = 0;
	canbecolored = false;
	health = 100;
	orighealth = 100;

	watered = 0;
	grew = 0;

	price = 4500;

	harvestAmt = $CityRPG::drugs::Steroid::harvestAmt;
	growthTime = $CityRPG::drugs::Steroid::growthTime;

	CityRPGBrickType = 420;
	CityRPGBrickAdmin = false;
};
