﻿namespace MMXOnline;

public enum RenderEffectType {
	Hit,
	Flash,
	//StockedCharge,
	Invisible,
	InvisibleFlash,
	BlueShadow,
	RedShadow,
	Trail,
	GreenShadow,
	PurpleShadow,
	YellowShadow,
	OrangeShadow,
	//StockedSaber,
	BoomerangKTrail,
	SpeedDevilTrail,
	TrailX,
	TrailZero,
	TrailAxl,
	TrailSigma,
	StealthModeBlue,
	StealthModeRed,
	Shake,
	ChargeGreen,
	ChargeOrange,
	ChargePink,
	ChargeYellow,
	ChargeBlue,
}

public class RenderEffect {
	public RenderEffectType type;
	public float time;
	public float flashTime;
	public RenderEffect(RenderEffectType type, float flashTime = 0, float time = float.MaxValue) {
		this.type = type;
		this.flashTime = flashTime;
		this.time = time;
	}

	public bool isFlashing() {
		return time < flashTime;
	}
}
