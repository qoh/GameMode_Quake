datablock PlayerData(QuakePlayer : PlayerStandardArmor)
{
	uiName = "Quake Player";
	canJet = 0;
	maxTools = 8;

	maxForwardSpeed = 15;
	maxBackwardSpeed = 15;
	maxSideSpeed = 16;

	maxUnderwaterForwardSpeed = 12;
	maxUnderwaterBackwardSpeed = 12;
	maxUnderwaterSideSpeed = 13;

	maxForwardCrouchSpeed = 7;
	maxBackwardCrouchSpeed = 7;
	maxSideCrouchSpeed = 8;

	runForce = 100 * 90;
	jumpForce = 9 * 90;

	jumpDelay = 0;
	minImpactSpeed = 20;
	firstPersonOnly = 1;

	runSurfaceAngle = 55;
	jumpSurfaceAngle = 55;

	exitingWater = QuakePlayerWaterExitSound;
	crouchBoundingBox = "5 5 8.6";

	impactWaterEasy = QuakePlayerWaterEnterSound;
	impactWaterMedium = QuakePlayerWaterEnterSound;
	impactWaterHard = QuakePlayerWaterEnterSound;
};

datablock ItemData(QuakePlayerGibs)
{
	shapeFile = "Add-Ons/GameMode_Quake/shapes/gibs.dts";
	rotate = 0;

	emap = 1;
	mass = 1;
	density = 0.2;
	elasticity = 0.4;
	friction = 0.2;

	doColorShift = 1;
	colorShiftColor = "0.5 0.1 0.1 1";
	
	noRotate = 1;
	noQuakeLight = 1;
};

function QuakePlayerGibs::onPickup(%this, %item, %obj)
{
}