using System;
using System.Collections.Generic;
using System.Linq;

namespace MMXOnline;

public class BusterZero : Character {
	public float zSaberCooldown;
	public float lemonCooldown;
	public bool isBlackZero;
	public int stockedBusterLv;
	public bool stockedSaber;
	public List<BusterProj> zeroLemonsOnField = new();
	public ZeroBuster busterWeapon = new();
	public ZSaberProjSwing meleeWeapon;

	public BusterZero(
		Player player, float x, float y, int xDir,
		bool isVisible, ushort? netId, bool ownedByLocalPlayer, bool isWarpIn = true
	) : base(
		player, x, y, xDir, isVisible, netId, ownedByLocalPlayer, isWarpIn
	) {
		meleeWeapon = new ZSaberProjSwing(player);
	}

	public override void update() {
		base.update();
		if (stockedBusterLv > 0 || stockedSaber) {
			var renderGfx = stockedBusterLv switch {
				_ when stockedSaber => RenderEffectType.ChargeGreen,
				1  => RenderEffectType.ChargePink,
				2 => RenderEffectType.ChargeOrange,
				_ => RenderEffectType.ChargeBlue
			};
			addRenderEffect(renderGfx, 0.033333f, 0.1f);
		}
		if (!ownedByLocalPlayer) {
			return;
		}
		// Cooldowns.
		Helpers.decrementTime(ref zSaberCooldown);
		Helpers.decrementTime(ref lemonCooldown);

		// For the shooting animation.
		if (shootAnimTime > 0) {
			shootAnimTime -= Global.spf;
			if (shootAnimTime <= 0) {
				shootAnimTime = 0;
				changeSpriteFromName(charState.defaultSprite, false);
				if (charState is WallSlide) {
					frameIndex = sprite.frames.Count - 1;
				}
			}
		}
		// Charge and release shoot logic.
		if (chargeButtonHeld() && flag == null && rideChaser == null && rideArmor == null) {
			if (stockedBusterLv == 0 && !stockedSaber && !isInvulnerableAttack()) {
				increaseCharge();
			}
		}
		// Release charge.
		else if (charState.attackCtrl) {
			int chargeLevel = getChargeLevel();
			if (isCharging()) {
				if (chargeLevel >= 1) {
					shoot(chargeLevel);
				}
			}
			stopCharge();
		}
		chargeLogic();
	}

	public override bool normalCtrl() {
		// Handles Standard Hypermode Activations.
		if (player.currency >= Player.zeroHyperCost &&
			!isBlackZero &&
			player.input.isHeld(Control.Special2, player) &&
			charState is not HyperZeroStart and not WarpIn
		) {
			hyperProgress += Global.spf;
		} else {
			hyperProgress = 0;
		}
		if (hyperProgress >= 1 && player.currency >= Player.zeroHyperCost) {
			hyperProgress = 0;
			changeState(new HyperZeroStart(0), true);
			return true;
		}
		return base.normalCtrl();
	}

	public override bool attackCtrl() {
		bool shootPressed = player.input.isPressed(Control.Shoot, player);
		bool specialPressed = player.input.isPressed(Control.Special1, player);
		if (specialPressed) {
			if (zSaberCooldown == 0) {
				if (stockedSaber) {
					changeState(new BusterZeroHadangeki(), true);
					return true;
				}
				changeState(new BusterZeroMelee(), true);
				return true;
			}
		}
		if (!isCharging()) {
			if (shootPressed) {
				lastShootPressed = Global.frameCount;
			}
			int framesSinceLastShootPressed = Global.frameCount - lastShootPressed;
			if (shootPressed || framesSinceLastShootPressed < 6) {
				if (stockedBusterLv == 1) {
					changeState(new BusterZeroDoubleBuster(true, true), true);
					return true;
				}
				if (stockedBusterLv == 2) {
					changeState(new BusterZeroDoubleBuster(true, false), true);
					return true;
				}
				if (stockedSaber) {
					changeState(new BusterZeroHadangeki(), true);
					return true;
				}
				if (lemonCooldown <= 0) {
					shoot(0);
					return true;
				}
			}
		}
		return base.attackCtrl();
	}

	// Shoots stuff.
	public void shoot(int chargeLevel) {
		if (chargeLevel == 0) {
			for (int i = zeroLemonsOnField.Count - 1; i >= 0; i--) {
				if (zeroLemonsOnField[i].destroyed) {
					zeroLemonsOnField.RemoveAt(i);
				}
			}
			if (zeroLemonsOnField.Count >= 3) { return; }
		}
		string shootSprite = getSprite(charState.shootSprite);
		if (!Global.sprites.ContainsKey(shootSprite)) {
			if (grounded) { shootSprite = "zero_shoot"; } else { shootSprite = "zero_fall_shoot"; }
		}
		if (shootAnimTime == 0) {
			changeSprite(shootSprite, false);
		} else if (charState is Idle) {
			frameIndex = 0;
			frameTime = 0;
		}
		if (charState is LadderClimb) {
			if (player.input.isHeld(Control.Left, player)) {
				this.xDir = -1;
			} else if (player.input.isHeld(Control.Right, player)) {
				this.xDir = 1;
			}
		}
		shootAnimTime = 0.3f;
		Point shootPos = getShootPos();
		int xDir = getShootXDir();

		if (chargeLevel == 0) {
			playSound("busterX3", sendRpc: true);
			var lemon = new BusterProj(
				busterWeapon, shootPos, xDir, 1, player, player.getNextActorNetId(), rpc: true
			);
			zeroLemonsOnField.Add(lemon);
			lemonCooldown = 0.15f;
		} else if (chargeLevel == 1) {
			playSound("buster2X3", sendRpc: true);
			new ZBuster2Proj(
				busterWeapon, shootPos, xDir, 1, player, player.getNextActorNetId(), rpc: true
			);
			lemonCooldown = 22f / 60f;
		} else if (chargeLevel == 2) {
			playSound("buster3X3", sendRpc: true);
			new ZBuster4Proj(
				busterWeapon, shootPos, xDir, 1, player, player.getNextActorNetId(), rpc: true
			);
			lemonCooldown = 22f / 60f;
		} else if (chargeLevel >= 3) {
			shootAnimTime = 0;
			changeState(new BusterZeroDoubleBuster(false, true), true);
		}
		if (chargeLevel >= 4) {
			shootAnimTime = 0;
			changeState(new BusterZeroDoubleBuster(false, false), true);
		}
		if (chargeLevel >= 1) {
			stopCharge();
		}
	}

	// This can run on both owners and non-owners. So data used must be in sync
	public override Projectile? getProjFromHitbox(Collider collider, Point centerPoint) {
		if (sprite.name == "zero_projswing") {
			return new GenericMeleeProj(
				meleeWeapon, centerPoint, ProjIds.ZSaberProjSwing, player,
				isBlackZero ? 4 : 3, Global.defFlinch, 0.5f, isReflectShield: true
			);
		}
		return null;
	}

	public override string getSprite(string spriteName) {
		return "zero_" + spriteName;
	}

	public override bool chargeButtonHeld() {
		return player.input.isHeld(Control.Shoot, player);
	}

	public override float getRunSpeed() {
		float runSpeed = 90;
		if (isBlackZero) {
			runSpeed *= 1.15f;
		}
		return runSpeed * getRunDebuffs();
	}

	public override float getDashSpeed() {
		if (flag != null || !isDashing) {
			return getRunSpeed();
		}
		float dashSpeed = 210;
		if (isBlackZero) {
			dashSpeed *= 1.15f;
		}
		return dashSpeed * getRunDebuffs();
	}
}
