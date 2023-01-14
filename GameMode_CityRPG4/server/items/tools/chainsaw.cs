if(!isObject(CityRPGChainsawItem))
{

	datablock AudioProfile(CityRPGChainsawDrawSound)
	{
	filename    = $City::DataPath @ "sounds/draw_chainsaw.wav";
	description = AudioClose3d;
	preload = true;
	};

	datablock AudioProfile(CityRPGChainsawIdleSound)
	{
	filename    = $City::DataPath @ "sounds/idle_chainsaw.wav";
	description = AudioCloseLooping3d;
	preload = true;
	};

	datablock AudioProfile(CityRPGChainsawHitSound)
	{
	filename    = $City::DataPath @ "sounds/hit_chainsaw.wav";
	description = AudioClose3d;
	preload = true;
	};

	datablock AudioProfile(CityRPGChainsawActiveSound)
	{
	filename    = $City::DataPath @ "sounds/active_chainsaw.wav";
	description = AudioCloseLooping3d;
	preload = true;
	};


	datablock ParticleData(sparkParticle)
	{
	textureName          = $City::DataPath @ "shapes/spark";
	dragCoefficient      = 1;
	gravityCoefficient   = 0.0;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 100;
	lifetimeVarianceMS   = 50;

	colors[0]     = "0.60 0.40 0.30 1.0";
	colors[1]     = "0.60 0.40 0.30 1.0";
	colors[2]     = "1.0 0.40 0.30 0.0";

	sizes[0]      = 0.25;
	sizes[1]      = 0.15;
	sizes[2]      = 0.15;

	times[0]      = 0.0;
	times[1]      = 0.5;
	times[2]      = 1.0;
	};

	datablock ParticleEmitterData(spark)
	{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 1;
	ejectionOffset   = 0.2;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvances = false;
	orientParticles  = true;
	lifetimeMS       = 50;
	particles = "sparkParticle";
	};

	datablock ExplosionData(CityRPGChainsawExplosion)
	{
	lifeTimeMS = 300;

	soundProfile = CityRPGChainsawHitSound;

	particleEmitter = spark;
	particleDensity = 8;
	particleRadius = 0.2;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "12.0 14.0 12.0";
	camShakeAmp = "0.7 0.7 0.7";
	camShakeDuration = 0.35;
	camShakeRadius = 7.0;

	lightStartRadius = 1.5;
	lightEndRadius = 0;
	lightStartColor = "1.0 0.0 0.0";
	lightEndColor = "0 0 0";
	};

	datablock ProjectileData(CityRPGChainsawProjectile)
	{

	directDamage      = 20;
	directDamageType  = $DamageType::Chainsaw;
	radiusDamageType  = $DamageType::Chainsaw;
	explosion         = CityRPGChainsawExplosion;

	muzzleVelocity      = 65;
	velInheritFactor    = 1;

	armingDelay         = 0;
	lifetime            = 100;
	fadeDelay           = 70;
	bounceElasticity    = 0;
	bounceFriction      = 0;
	isBallistic         = false;
	gravityMod = 0.0;

	hasLight    = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";

	uiName = "Chainsaw Hit";
	};

	datablock ParticleData(CityRPGChainsawSmokeEmitterAParticle)
	{
		dragCoefficient      = 1.0;
		gravityCoefficient   = -1;
		inheritedVelFactor   = 0.2;
		constantAcceleration = 0.5;
		lifetimeMS           = 300;
		lifetimeVarianceMS   = 200;
		textureName          = "base/data/particles/cloud";
		spinSpeed		= 10.0;
		spinRandomMin		= -50.0;
		spinRandomMax		= 50.0;
		colors[0]     = "1.000000 1.000000 1.000000 0.010000";
		colors[1]     = "1.000000 1.000000 1.000000 0.020000";
		colors[2]     = "1.000000 1.000000 1.000000 0.020000";
		colors[3]     = "1.000000 1.000000 1.000000 0.030000";
		colors[4]     = "1.000000 1.000000 1.000000 0.040000";
		colors[5]     = "1.000000 1.000000 1.000000 0.006000";
		colors[6]     = "1.000000 1.000000 1.000000 0.007000";

		sizes[0]      = 0.50;
		sizes[1]      = 1.1;
		sizes[2]      = 1.2;
		sizes[3]      = 1.3;
		sizes[4]      = 0.50;
		sizes[5]      = 0.2;
		sizes[6]      = 0.1;

		times[0] = 0;
		times[1] = 1;
		times[2] = 4;
		times[3] = 5;

		useInvAlpha = false;
	};

	datablock ParticleEmitterData(CityRPGChainsawSmokeEmitterAEmitter)
	{
		ejectionPeriodMS = 13;
		periodVarianceMS = 4;
		ejectionVelocity = 0.3;
		velocityVariance = 0.1;
		ejectionOffset   = 0.0;
		thetaMin         = 0;
		thetaMax         = 0;
		phiReferenceVel  = 0;
		phiVariance      = 360;
		overrideAdvance = false;
		orientOnVelocity = true;
		particles = "CityRPGChainsawSmokeEmitterAParticle";
	};

	datablock ParticleData(CityRPGChainsawSmokeEmitterBParticle)
	{
		dragCoefficient      = 1.0;
		gravityCoefficient   = -1;
		inheritedVelFactor   = 0.2;
		constantAcceleration = 0.5;
		lifetimeMS           = 300;
		lifetimeVarianceMS   = 200;
		textureName          = "base/data/particles/cloud";
		spinSpeed		= 10.0;
		spinRandomMin		= -50.0;
		spinRandomMax		= 50.0;
		colors[0]     = "1.000000 1.000000 1.000000 0.030000";
		colors[1]     = "1.000000 1.000000 1.000000 0.030000";
		colors[2]     = "1.000000 1.000000 1.000000 0.040000";
		colors[3]     = "1.000000 1.000000 1.000000 0.040000";
		colors[4]     = "1.000000 1.000000 1.000000 0.050000";
		colors[5]     = "1.000000 1.000000 1.000000 0.007000";
		colors[6]     = "1.000000 1.000000 1.000000 0.008000";

		sizes[0]      = 0.50;
		sizes[1]      = 1.1;
		sizes[2]      = 1.2;
		sizes[3]      = 1.3;
		sizes[4]      = 0.50;
		sizes[5]      = 0.2;
		sizes[6]      = 0.1;

		times[0] = 0;
		times[1] = 1;
		times[2] = 4;
		times[3] = 5;

		useInvAlpha = false;
	};

	datablock ParticleEmitterData(CityRPGChainsawSmokeEmitterBEmitter)
	{
		ejectionPeriodMS = 13;
		periodVarianceMS = 4;
		ejectionVelocity = 0.3;
		velocityVariance = 0.1;
		ejectionOffset   = 0.0;
		thetaMin         = 0;
		thetaMax         = 0;
		phiReferenceVel  = 0;
		phiVariance      = 360;
		overrideAdvance = false;
		orientOnVelocity = true;
		particles = "CityRPGChainsawSmokeEmitterBParticle";
	};

	//////////
	// item //
	//////////
	datablock ItemData(CityRPGChainsawItem : swordItem)
	{
		shapeFile = $City::DataPath @ "shapes/chainsawitem.1.dts";
		uiName = "Chainsaw";
		doColorShift = false;
		colorShiftColor = "0.471 0.471 0.471 1.000";

		image = CityRPGChainsawImage;
		canDrop = true;
		iconName = "ui/CI/chainsaw";
	};

	datablock shapeBaseImageData(CityRPGChainsawImage)
	{
		shapeFile = $City::DataPath @ "shapes/chainsawweapon.1.dts";
		emap = true;
		
		mountPoint = 0;
		offset = "0 0 0";
		eyeOffset = 0;
		
		correctMuzzleVector = false;
		
		className = "WeaponImage";
		
		projectile = CityRPGChainsawProjectile;
		projectileType = Projectile;
		
		stateName[0]                     = "Activate";
		stateTimeoutValue[0]             = 0.5;
		stateTransitionOnTimeout[0]      = "Ready";
		stateSound[0]                    = CityRPGChainsawDrawSound;

		stateName[1]			= "Ready";
		stateTransitionOnTriggerDown[1]	= "Fire";
		stateAllowImageChange[1]	= true;
		stateTimeoutValue[1]		= 0.1;
		stateTransitionOnTimeout[1]	= "Ready";
		stateSequence[1]		= "Active";
		stateSound[1]			= CityRPGChainsawIdleSound;
		stateEmitter[1]                 = CityRPGChainsawSmokeEmitterA;
		stateEmitterNode[1]		= everything;
		stateEmitterTime[1]		= 0.2;

		stateName[2]			= "Fire";
		stateScript[2]			= "onFire";
		stateFire[2]			= true;
		stateAllowImageChange[2]	= true;
		stateTimeoutValue[2]		= 0.10;
		stateTransitionOnTimeout[2]	= "Fire";
		stateTransitionOnTriggerUp[2]	= "Ready";
		stateSound[2]			= CityRPGChainsawActiveSound;
		stateSequence[2]		= "TrigDown";
		stateEmitter[2]			= CityRPGChainsawSmokeEmitterB;
		stateEmitterNode[2]		= everything;
		stateEmitterTime[2]		= 0.2;
	};

}

function CityRPGChainsawProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	if(%col.getClassName() $= "fxDTSBrick" && %col.getDatablock().CityRPGBrickType == $CityBrick_ResourceLumber)
		%col.onChop(%obj.client);

	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
}
