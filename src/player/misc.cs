AddDamageType("QuakeFall", '%1 cratered', '%2 cratered %1', 1, 1);

function QuakePlayer::onImpact(%this, %obj, %col, %velocity, %speed)
{
	%angle = vectorDot("0 0 1", vectorNormalize(%velocity));

	if (%angle >= 0.75)
	{
		%obj.damage(0, %obj.getHackPosition(), (%speed - 20) * 1.75, $DamageType::QuakeFall);
	}
}