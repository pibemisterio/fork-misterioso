﻿using System;
using System.Diagnostics.CodeAnalysis;
using SFML.Graphics;

namespace MMXOnline;

public abstract class PZeroGenericMeleeState : CharState {
	public PunchyZero zero = null!;

	public float projMaxTime;
	public int projId;
	public int comboFrame = Int32.MaxValue;

	public string sound = "";
	public bool soundPlayed;
	public int soundFrame = Int32.MaxValue;

	public PZeroGenericMeleeState(string spr) : base(spr) {
	}

	public override void update() {
		base.update();
		if (character.sprite.frameIndex >= soundFrame && !soundPlayed) {
			character.playSound(sound, forcePlay: false, sendRpc: true);
			soundPlayed = true;
		}
		if (character.sprite.frameIndex >= comboFrame) {
			altCtrls[0] = true;
		}
		if (character.isAnimOver()) {
			character.changeToIdleOrFall();
		}
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		character.turnToInput(player.input, player);
		zero = character as PunchyZero ?? throw new NullReferenceException();
	}

	public virtual bool altCtrlUpdate(bool[] ctrls) {
		return false;
	}
}

public class PZeroPunch1 : PZeroGenericMeleeState {
	public PZeroPunch1() : base("punch") {
		sound = "punch1";
		projId = (int)ProjIds.PZeroPunch;
		soundFrame = 1;
		comboFrame = 3;
	}

	public override bool altCtrlUpdate(bool[] ctrls) {
		if (zero.shootPressTime > 0 || player.isAI) {
			zero.changeState(new PZeroPunch2(), true);
			return true;
		}
		return false;
	}
}

public class PZeroPunch2 : PZeroGenericMeleeState {
	public PZeroPunch2() : base("punch2") {
		sound = "punch2";
		projId = (int)ProjIds.PZeroPunch2;
		soundFrame = 1;
		comboFrame = 4;
	}

	public override bool altCtrlUpdate(bool[] ctrls) {
		if (zero.specialPressTime > 0) {
			return zero.groundSpcAttacks();
		}
		if (zero.shootPressTime > 0 && player.input.getYDir(player) == -1) {
			zero.changeState(new PZeroShoryuken(), true);
			return true;
		}
		return false;
	}
}

public class PZeroKick : PZeroGenericMeleeState {
	public PZeroKick() : base("kick_air") {
		sound = "punch1";
		projId = (int)ProjIds.PZeroAirKick;
		soundFrame = 1;

		airMove = true;
		exitOnLanding = true;
		useDashJumpSpeed = true;
	}
}

public class PZeroYoudantotsu : PZeroGenericMeleeState {
	public PZeroYoudantotsu() : base("megapunch") {
		sound = "megapunch";
		projId = (int)ProjIds.PZeroYoudantotsu;
		soundFrame = 7;
	}
}

public class PZeroSpinKick : CharState {
	public float dashTime = 0;
	public float soundTime = 0;

	public PZeroSpinKick() : base("spinkick") {
		exitOnAirborne = true;
		airMove = true;
		attackCtrl = false;
		normalCtrl = true;
	}

	public override void update() {
		base.update();
		soundTime -= Global.speedMul;
		if (soundTime <= 0) {
			soundTime = 9;
			character.playSound("spinkick", sendRpc: true);
		}
		float iXDir = player.input.getXDir(player);
		if (iXDir != 0 && iXDir != character.xDir || player.input.isPressed(Control.Dash, player)) {
			character.changeToIdleOrFall();
			return;
		}
		dashTime += Global.speedMul;
		if (dashTime >= 28) {
			attackCtrl = true;
		}
		if (dashTime >= 32) {
			character.changeToIdleOrFall();
			return;
		}
		character.move(new Point(character.getDashSpeed() * character.xDir, 0));
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		character.isDashing = true;
		character.sprite.tempOffY = 2;
	}

	public override void onExit(CharState newState) {
		character.slideVel = character.xDir * character.getRunSpeed();
		character.isDashing = false;
		base.onExit(newState);
		if (character is PunchyZero zero) {
			zero.dashAttackCooldown = 30;
		}
	}
}

public class PZeroDiveKickState : CharState {
	float stuckTime;
	float diveTime;
	PunchyZero zero = null!;

	public PZeroDiveKickState() : base("dropkick") {
	}

	public override void update() {
		if (character.frameIndex >= 3 && !once) {
			character.vel.x = character.xDir * 300;
			character.vel.y = 450;
			character.playSound("punch2", sendRpc: true);
			once = true;
		}
		base.update();
		if (!once) {
			return;
		}
		if (character.vel.y < 100) {
			character.changeToLandingOrFall();
			return;
		}
		CollideData hit = Global.level.checkTerrainCollisionOnce(
			character, character.vel.x * Global.spf, character.vel.y * Global.spf
		);
		if (hit?.isSideWallHit() == true) {
			character.changeState(new Fall(), true);
			return;
		} else if (hit != null) {
			stuckTime += Global.speedMul;
			if (stuckTime >= 6) {
				character.changeToLandingOrFall();
				return;
			}
		}
		if (character.grounded || diveTime >= 6 && character.deltaPos.y == 0) {
			character.changeToLandingOrFall();
			return;
		}
		diveTime += Global.spf;
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		zero = character as PunchyZero ?? throw new NullReferenceException();
		character.stopMovingWeak();
		character.useGravity = false;
	}

	public override void onExit(CharState newState) {
		base.onExit(newState);
		character.useGravity = true;
		character.stopMovingWeak();
		zero.diveKickCooldown = 60;
	}
}


public class ZeroDropkickLand : CharState {
	public ZeroDropkickLand() : base("land") {
		exitOnAirborne = true;
	}

	public override void update() {
		base.update();
		if (character.isAnimOver()) {
			character.changeToIdleOrFall();
		}
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		character.playSound("land", sendRpc: true);
	}
}

public class PZeroParry : CharState {
	PunchyZero zero = null!;

	public PZeroParry() : base("parry_start") {
		superArmor = true;
	}

	public override void update() {
		base.update();
		if (character.frameIndex != 0) {
			character.specialState = (int)SpecialStateIds.None;
		}
		if (character.isAnimOver()) {
			character.changeToIdleOrFall();
		}
	}

	public void counterAttack(Player damagingPlayer, Actor? damagingActor) {
		zero.parryCooldown = 0;
		Actor? counterAttackTarget = null;
		bool isMelee = false;
		if (damagingActor is Projectile proj) {
			if (proj.damager != null) {
				zero.gigaAttack.addAmmo(proj.damager.damage, player);
			}
			if (proj.isMelee && proj.owningActor != null) {
				counterAttackTarget = proj.owningActor;
				isMelee = true;
			}
		}
		if (counterAttackTarget == null) {
			counterAttackTarget = damagingPlayer?.character ?? damagingActor;
		}
		if (counterAttackTarget is Character chara && isMelee) {
			if (!chara.ownedByLocalPlayer) {
				RPC.actorToggle.sendRpc(chara.netId, RPCActorToggleType.ChangeToParriedState);
			} else {
				chara.changeState(new ParriedState(), true);
			}
		}
		if (Helpers.randomRange(0, 10) != 10) {
			character.playSound("zeroParry", forcePlay: false, sendRpc: true);
		} else {
			character.playSound("zeroParry2", forcePlay: false, sendRpc: true);
		}
		character.changeState(new PZeroParryCounter(counterAttackTarget, isMelee), true);
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		character.specialState = (int)SpecialStateIds.PZeroParry;
		zero = character as PunchyZero ?? throw new NullReferenceException();
	}

	public override void onExit(CharState newState) {
		character.specialState = (int)SpecialStateIds.None;
		base.onExit(newState);
		zero.parryCooldown = 30;
	}

	public bool canParry(Actor? actor, int projId) {
		return (!Damager.isDot(projId));
	}
}

public class PZeroParryCounter : CharState {
	private Actor? counterAttackTarget;
	bool isMelee;

	private Point counterAttackPos;
	private Point enemyAttackPos;
	private int initialFacingPos;

	private Point currentPos;
	private bool calcOnce = false;
	private bool canCounterDash = false;
	private float counterSpeed = 0.1f;
	private float acumulatedPos = 0;

	public PZeroParryCounter(Actor? counterAttackTarget, bool isMelee) : base("parry", "", "", "") {
		invincible = true;
		this.counterAttackTarget = counterAttackTarget;
		this.isMelee = isMelee;
	}

	public override void update() {
		base.update();
		if (character.frameIndex < 5) {
			character.addRenderEffect(RenderEffectType.ChargeOrange, 0.033333f, 0.1f);
		}
		if (counterAttackTarget == null || calcOnce && !canCounterDash) {
			if (character.frameIndex >= 2) {
				character.move(new Point(initialFacingPos * 100, 0));
			}
		} else {
			dashToTarget();
		}
		if (!canCounterDash && character.isAnimOver()) {
			character.changePos(character.pos);
			character.changeToIdleOrFall();
		}
	}

	public void dashToTarget() {
		if (counterAttackTarget == null) {
			return;
		}
		if (!calcOnce && character.frameIndex >= 5) {
			float dist = currentPos.distanceTo(counterAttackPos);
			enemyAttackPos = counterAttackTarget.pos;
			character.turnToPos(counterAttackTarget.pos);
			initialFacingPos = character.xDir;
			counterAttackPos = counterAttackTarget.pos.addxy(character.xDir * -20, 0f);
			float dist2 = currentPos.distanceTo(enemyAttackPos);

			if (isMelee || dist <= 150 || dist2 <= 150) {
				canCounterDash = true;
			}
			calcOnce = true;
		}
		if (canCounterDash) {
			character.turnToPos(counterAttackTarget.pos);
			currentPos += (counterAttackPos - currentPos) * (Global.speedMul * counterSpeed);
			acumulatedPos += counterSpeed;
			character.pos = currentPos;
			counterSpeed += 0.01f;
			if (counterSpeed >= 0.125) {
				counterSpeed += 0.05f;
			}
			if (acumulatedPos >= 1.1) {
				canCounterDash = false;
				character.pos = counterAttackPos;
				character.sprite.frameIndex = 8;
				character.sprite.frameTime = 0;
			}
		}
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		character.useGravity = false;
		if (counterAttackTarget != null) {
			currentPos = character.pos;
			counterAttackPos = counterAttackTarget.pos;
		}
	}

	public override void onExit(CharState newState) {
		character.useGravity = true;
		base.onExit(newState);
	}
}


public class PZeroShoryuken : CharState {
	bool jumpedYet;
	float timeInWall;
	public PunchyZero zero = null!;

	public PZeroShoryuken() : base("shoryuken") {
	}

	public override void update() {
		base.update();
		if (character.sprite.frameIndex >= 3 && !jumpedYet) {
			jumpedYet = true;
			character.dashedInAir++;
			character.vel.y = -character.getJumpPower() * 1.2f;
			character.playSound("punch2", sendRpc: true);
		}

		if (character.sprite.frameIndex >= 3 && character.sprite.frameIndex < 6) {
			float speed = 100;
			character.move(new Point(character.xDir * speed, 0));
			if (jumpedYet) {
				character.vel.y += Global.speedMul * character.getGravity() * 0.5f;
			}
		}

		var wallAbove = Global.level.checkTerrainCollisionOnce(character, 0, -10);
		if (wallAbove != null && wallAbove.gameObject is Wall) {
			timeInWall += Global.speedMul;
			if (timeInWall >= 6) {
				character.changeState(new Fall());
				return;
			}
		}

		if (canDownSpecial() &&
			(zero.shootPressTime > 0 || zero.specialPressTime > 0) &&
			player.input.getYDir(player) == 1
		) {
			character.changeState(new PZeroDiveKickState(), true);
			return;
		}

		if (character.isAnimOver()) {
			character.changeState(new Fall());
		}
	}

	public bool canDownSpecial() {
		return (
			zero.diveKickCooldown == 0 &&
			character.sprite.frameIndex >= character.sprite.totalFrameNum- 3
		);
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		if (!character.grounded) {
			character.sprite.frameIndex = 3;
		}
		zero = character as PunchyZero ?? throw new NullReferenceException();
	}
}

public class HyperPunchyZeroStart : CharState {
	public float radius = 200;
	public float time;
	PunchyZero zero = null!;
	Anim? virusEffectParts;
	Anim[] virusAnim = new Anim[3];
	float[] delayedVirusTimer = {0, 7, 14};
	string virusAnimName = "";

	public HyperPunchyZeroStart() : base("hyper_start") {
		invincible = true;
	}

	public override void update() {
		base.update();
		if (virusAnimName != "") {
			int animCount = 0;
			for (int i = 0; i < virusAnim.Length; i++) {
				if (virusAnim[i] != null) {
					if (virusAnim[i].pos == character.getCenterPos()) {
						virusAnim[i].destroySelf();
					}
					if (virusAnim[i].destroyed) {
						character.playSound("shingetsurinx5", true);
						if (stateFrames > 55) {
							virusAnim[i] = null!;
							continue;
						}
						virusAnim[i] = virusAnim[i] = createVirusAnim();
					} else {
						animCount++;
					}
					virusAnim[i].moveToPos(character.getCenterPos(), 300);
					if (virusAnim[i].pos.distanceTo(character.getCenterPos()) < 10) {
						virusAnim[i].destroySelf();
					}
				} else if (delayedVirusTimer[i] > 0) {
					delayedVirusTimer[i] -= Global.speedMul;
					if (delayedVirusTimer[i] <= 0) {
						delayedVirusTimer[i] = 0;
						virusAnim[i] = createVirusAnim();
					}
				}
				if (animCount == 0 && stateFrames > 55 && virusEffectParts != null) {
					virusEffectParts.destroySelf();
				}
			}
		}
		if (time == 0) {
			if (radius >= 0) {
				radius -= Global.spf * 200;
			} else {
				time = Global.spf;
				radius = 0;
				activateHypermode();
				character.playSound("ching");
				character.fillHealthToMax();
			}
		} else {
			time += Global.spf;
			if (time >= 1) {
				character.changeToIdleOrFall();
			}
		}
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		zero = character as PunchyZero ?? throw new NullReferenceException();
		character.useGravity = false;
		character.vel = new Point();
		if (zero == null) {
			throw new NullReferenceException();
		}
		character.player.currency -= 10;
		if (zero.hyperMode == 2) {
			zero.changeSpriteFromName("hyper_viral", true);
			virusAnimName = "sigmavirushead";
			virusAnim[0] = createVirusAnim();
		}
		if (zero.hyperMode == 1) {
			zero.changeSpriteFromName("hyper_awakened", true);
			virusAnimName = "zerovirus";
			virusAnim[0] = createVirusAnim();
			virusEffectParts = new Anim(character.pos.addxy(0, 4), "viruseffect", -character.xDir, null, false);
			virusEffectParts.blink = true;
		}
		if (zero.hyperMode == 0) {
			character.playSound("blackzeroentry", forcePlay: false, sendRpc: true);
		}
	}

	public override void onExit(CharState newState) {
		base.onExit(newState);
		character.useGravity = true;
		if (character != null) {
			character.invulnTime = 0.5f;
		}
		if (zero.isAwakened || zero.isBlack) {
			zero.hyperModeTimer = PunchyZero.maxBlackZeroTime + 30;
		}
		virusEffectParts?.destroySelf();
		bool playedHitSound = false;
		for (int i = 0; i < virusAnim.Length; i++) {
			if (virusAnim[i]?.destroyed == false) {
				if (!playedHitSound) {
					character?.playSound("shingetsurinx5", true);
					playedHitSound = true;
				}
				virusAnim[i]?.destroySelf();
			}
		}
	}

	public Anim createVirusAnim() {
		float newAngle = Helpers.randomRange(0, 359);
		Point newPos = new(Helpers.cosd(newAngle) * 100, Helpers.sind(newAngle) * 100);
		float diplayAngle = newAngle;
		int xDir = -1;
		if (virusAnimName == "sigmavirushead") {
			xDir = 1;
		}
		if (diplayAngle > 90 && diplayAngle < 270) {
			diplayAngle = (diplayAngle + 180f) % 360f;
			xDir *= -1;
		}
		return new Anim(
			character.getCenterPos() + newPos,
			virusAnimName, xDir,
			player.getNextActorNetId(), false, sendRpc: true
		) {
			angle = diplayAngle
		};
	}

	public void activateHypermode() {
		if (zero.hyperMode == 1) {
			zero.awakenedPhase = 1;
			float storedAmmo = zero.gigaAttack.ammo;
			zero.gigaAttack = new ShinMessenkou();
			zero.gigaAttack.ammo = storedAmmo;
		} else if (zero.hyperMode == 2) {
			zero.isViral = true;
			float storedAmmo = zero.gigaAttack.ammo;
			zero.gigaAttack = new DarkHoldWeapon();
			zero.gigaAttack.ammo = storedAmmo;
			zero.freeBusterShots = 10;
		} else {
			zero.isBlack = true;
		}
	}

	public override void render(float x, float y) {
		base.render(x, y);
		Point pos = character.getCenterPos();
		if (zero.hyperMode == 0) {
			DrawWrappers.DrawCircle(
				pos.x + x, pos.y + y, radius, false, Color.White, 5, character.zIndex + 1, true, Color.White
			);
		}
	}
}

public class PunchyZeroHadangeki : CharState {
	bool fired;

	public PunchyZeroHadangeki() : base("projswing") {
		landSprite = "projswing";
		airSprite = "projswing_air";
		useDashJumpSpeed = true;
		airMove = true;
		superArmor = true;
	}

	public override void update() {
		base.update();
		if (character.grounded) {
			character.isDashing = false;
		}
		if (character.frameIndex >= 7 && !fired) {
			character.playSound("zerosaberx3", sendRpc: true);
			fired = true;
			new PZeroHadangeki(
				character.pos.addxy(30 * character.xDir, -20), character.xDir,
				player, player.getNextActorNetId(), rpc: true
			);
		}
		if (character.isAnimOver()) {
			character.changeToIdleOrFall();
		} else {
			if ((character.grounded || character.canAirJump() && character.flag == null) &&
				player.input.isPressed(Control.Jump, player)
			) {
				if (!character.grounded) {
					character.dashedInAir++;
				}
				character.vel.y = -character.getJumpPower();
				sprite = "projswing_air";
				character.changeSpriteFromName(sprite, false);
			}
		}
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		if (!character.grounded || character.vel.y < 0) {
			sprite = "projswing_air";
			defaultSprite = sprite;
			character.changeSpriteFromName(sprite, true);
		}
	}

	public override void onExit(CharState oldState) {
		base.onExit(oldState);
	}
}

public class PunchyZeroHadangekiWall : CharState {
	bool fired;
	public int wallDir;
	public Collider wallCollider;

	public PunchyZeroHadangekiWall(int wallDir, Collider wallCollider) : base("wall_slide_attack") {
		this.wallDir = wallDir;
		this.wallCollider = wallCollider;
		superArmor = true;
		useGravity = false;
	}

	public override void update() {
		base.update();
		if (character.frameIndex >= 4 && !fired) {
			character.playSound("zerosaberx3", sendRpc: true);
			fired = true;
			new PZeroHadangeki(
				character.pos.addxy(30 * -wallDir, -20), -wallDir,
				player, player.getNextActorNetId(), rpc: true
			);
		}
		if (character.isAnimOver()) {
			character.changeState(new WallSlide(wallDir, wallCollider));
			character.sprite.frameIndex = character.sprite.totalFrameNum - 1;
		}
	}

	public override void onExit(CharState oldState) {
		base.onExit(oldState);
		useGravity = true;
	}
}
