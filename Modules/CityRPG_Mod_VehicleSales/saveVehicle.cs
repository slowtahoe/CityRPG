function loadGarage()
{
	exec($City::SavePath @ "Garage.cs");
	$City::Garage::Loaded = 1;
}

function saveGarage()
{
	export("$City::Garage::*",$City::SavePath @ "Garage.cs");
}

//////////////////////////////////////////////////
