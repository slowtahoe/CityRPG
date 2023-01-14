datablock fxDTSBrickData(CityRPGSmallOreData)
{
	brickFile = $City::DataPath @ "bricks/SmallOre.blb";
	iconName = $City::DataPath @ "base/client/ui/brickIcons/SmallOre";

	category = "CityRPG";
	subCategory = "Resources";

	uiName = "Small Ore";

	CityRPGBrickType = $CityBrick_ResourceOre;
	CityRPGBrickAdmin = true;
};
