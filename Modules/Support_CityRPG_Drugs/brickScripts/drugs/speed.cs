datablock fxDTSBrickData(CityRPGSpeedData)
{
	brickFile = "Add-Ons/Support_CityRPG_Drugs/brick_6x6.blb";
	iconName = "Add-Ons/Support_CityRPG_Drugs/runningguy";

	category = "CityRPG";
	subCategory = "CityRPG Drugs";

	uiName = "Speed";
	drugType = "Speed";

	owner = 0;
	canchange = false;
	cansetemitter = false;
	emitter = "ArrowVanishEmitter";
	isDrug = true;
	hasDrug = false;
	isSpeed = true;
	isGrowing = false;
	growtime = 0;
	canbecolored = false;
	health = 100;
	orighealth = 100;

	watered = 0;
	grew = 0;

	price = 4500;

	harvestAmt = $CityRPG::drugs::Speed::harvestAmt;
	growthTime = $CityRPG::drugs::Speed::growthTime;

	CityRPGBrickType = 420;
	CityRPGBrickAdmin = false;
};
