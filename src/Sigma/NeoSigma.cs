using System;
using System.Collections.Generic;

namespace MMXOnline;

public class NeoSigma : BaseSigma {
	public float sigmaUpSlashCooldown;
	public float sigmaDownSlashCooldown;

	public NeoSigma(
		Player player, float x, float y, int xDir,
		bool isVisible, ushort? netId,
		bool ownedByLocalPlayer, bool isWarpIn = true
	) : base(
		player, x, y, xDir, isVisible,
		netId, ownedByLocalPlayer, isWarpIn
	) {
		sigmaSaberMaxCooldown = 0.5f;
	}

	public override void update() {
		// Call base update.
		base.update();
		if (!ownedByLocalPlayer) {
			return;
		}
		// Cooldowns.
		Helpers.decrementTime(ref sigmaUpSlashCooldown);
		Helpers.decrementTime(ref sigmaDownSlashCooldown);
		// For ladder and slide attacks.
		if (isAttacking() && charState is WallSlide or LadderClimb) {
			if (isAnimOver() && charState != null && charState is not SigmaClawState) {
				changeSprite(getSprite(charState.defaultSprite), true);
				if (charState is WallSlide && sprite != null) {
					frameIndex = sprite.totalFrameNum - 1;
				}
			} else if (grounded && sprite?.name != "sigma2_attack" && sprite?.name != "sigma2_attack2") {
				changeSprite("sigma2_attack", false);
			}
		}
	}

	public override bool attackCtrl() {
		if (isInvulnerableAttack() || player.weapon is MaverickWeapon) {
			return false;
		}
		if (player.weapon is MaverickWeapon) {
			return false;
		}
		bool attackPressed = false;
		if (player.weapon is not AssassinBullet) {
			if (player.input.isPressed(Control.Shoot, player)) {
				attackPressed = true;
				lastAttackFrame = Global.level.frameCount;
			}
		}
		framesSinceLastAttack = Global.level.frameCount - lastAttackFrame;
		bool lenientAttackPressed = (attackPressed || framesSinceLastAttack < 5);

		// Shoot button attacks.
		if (lenientAttackPressed && saberCooldown == 0) {
			if (player.input.isHeld(Control.Up, player) && flag == null && grounded) {
				if (sigmaUpSlashCooldown == 0) {
					sigmaUpSlashCooldown = 0.75f;
					changeState(new SigmaUpDownSlashState(true), true);
				}
				return true;
			} else if (player.input.isHeld(Control.Down, player) && !grounded && getDistFromGround() > 25) {
				if (sigmaDownSlashCooldown == 0) {
					sigmaUpSlashCooldown += 0.5f;
					sigmaDownSlashCooldown = 1f;
					changeState(new SigmaUpDownSlashState(false), true);
				}
				return true;
			}
			saberCooldown = sigmaSaberMaxCooldown;

			if (charState is WallSlide || charState is LadderClimb) {
				if (charState is LadderClimb) {
					int inputXDir = player.input.getXDir(player);
					if (inputXDir != 0) {
						xDir = inputXDir;
					}
				}
				changeSprite(getSprite(charState.attackSprite), true);
				playSound("sigma2slash", sendRpc: true);
				return true;
			}
			changeState(new SigmaClawState(charState, !grounded), true);
			return true;
		}
		if (grounded && player.input.isPressed(Control.Special1, player) &&
			flag == null && player.sigmaAmmo >= 14
		) {
			if (player.sigmaAmmo < 28) {
				player.sigmaAmmo -= 14;
				changeState(new SigmaElectricBallState(), true);
				return true;
			} else {
				player.sigmaAmmo = 0;
				changeState(new SigmaElectricBall2State(), true);
				return true;
			}
		}
		return base.attackCtrl();
	}

	public override Collider getBlockCollider() {
		Rect rect = Rect.createFromWH(0, 0, 18, 50);
		return new Collider(rect.getPoints(), false, this, false, false, HitboxFlag.Hurtbox, new Point(0, 0));
	}

	public override string getSprite(string spriteName) {
		return "sigma2_" + spriteName;
	}

	// This can run on both owners and non-owners. So data used must be in sync.
	public override Projectile? getProjFromHitbox(Collider collider, Point centerPoint) {
		Projectile? proj = sprite.name switch {
			"sigma2_attack" => new GenericMeleeProj(
				SigmaClawWeapon.netWeapon, centerPoint, ProjIds.Sigma2Claw, player,
				2, 0, 0.2f
			),
			"sigma2_attack2" => new GenericMeleeProj(
				SigmaClawWeapon.netWeapon, centerPoint, ProjIds.Sigma2Claw2, player,
				2, Global.halfFlinch, 0.5f
			),
			"sigma2_attack_air" => new GenericMeleeProj(
				SigmaClawWeapon.netWeapon, centerPoint, ProjIds.Sigma2Claw, player,
				3, 0, 0.375f
			),
			"sigma2_attack_dash" => new GenericMeleeProj(
				SigmaClawWeapon.netWeapon, centerPoint, ProjIds.Sigma2Claw, player,
				3, 0, 0.375f
			),
			"sigma2_upslash" or "sigma2_downslash" => new GenericMeleeProj(
				SigmaClawWeapon.netWeapon, centerPoint, ProjIds.Sigma2UpDownClaw, player,
				3, Global.defFlinch, 0.5f
			),
			"sigma2_ladder_attack" => new GenericMeleeProj(
				SigmaClawWeapon.netWeapon, centerPoint, ProjIds.Sigma2Claw, player,
				3, 0, 0.25f
			),
			"sigma2_wall_slide_attack" => new GenericMeleeProj(
				SigmaClawWeapon.netWeapon, centerPoint, ProjIds.Sigma2Claw, player,
				3, 0, 0.25f
			),
			"sigma2_shoot2" => new GenericMeleeProj(
				new SigmaElectricBall2Weapon(), centerPoint, ProjIds.Sigma2Ball2, player,
				6, Global.defFlinch, 1f
			),
			_ => null
		};
		if (proj != null) {
			return proj;
		}
		return base.getProjFromHitbox(collider, centerPoint);
	}

	public override void addAmmo(float amount) {
		weaponHealAmount += amount;
	}

	public override void addPercentAmmo(float amount) {
		weaponHealAmount += amount * 0.32f;
	}

	public override bool canAddAmmo() {
		return (player.sigmaAmmo < 28);
	}

	public override List<byte> getCustomActorNetData() {
		List<byte> customData = base.getCustomActorNetData();
		customData.Add((byte)MathF.Floor(player.sigmaAmmo));

		return customData;
	}

	public override void updateCustomActorNetData(byte[] data) {
		// Update base arguments.
		base.updateCustomActorNetData(data);
		data = data[data[0]..];

		// Per-player data.
		player.sigmaAmmo = data[0];
	}
}
