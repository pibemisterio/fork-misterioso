﻿using System;
using System.Collections.Generic;

namespace MMXOnline;

public class BubbleSplash : Weapon {
	public static BubbleSplash netWeapon = new();

	public List<BubbleSplashProj> bubblesOnField = new List<BubbleSplashProj>();
	public List<float> bubbleAfterlifeTimes = new List<float>();
	public float hyperChargeDelay;

	public BubbleSplash() : base() {
		shootSounds = new string[] { "bubbleSplash", "bubbleSplash", "bubbleSplash", "bubbleSplashCharged" };
		rateOfFire = 0.1f;
		isStream = true;
		index = (int)WeaponIds.BubbleSplash;
		weaponBarBaseIndex = 10;
		weaponBarIndex = weaponBarBaseIndex;
		weaponSlotIndex = 10;
		killFeedIndex = 21;
		weaknessIndex = 12;
		maxStreams = 7;
		streamCooldown = 1;
		switchCooldown = 0.25f;
		damage = "1/1";
		ammousage = 0.5;
		//effect = "Shoot a Stream up to 7 bubbles. C:Jump Boost.";
		effect = "Charged: Grants Jump Boost.";
	}

	public override float getAmmoUsage(int chargeLevel) {
		if (chargeLevel >= 3) {
			return base.getAmmoUsage(chargeLevel);
		} else {
			return 0.5f;// return Global.spf * 10;
		}
	}

	public override bool canShoot(int chargeLevel, Player player) {
		if (!base.canShoot(chargeLevel, player)) return false;
		if (hyperChargeDelay > 0) return false;

		return bubblesOnField.Count + bubbleAfterlifeTimes.Count < 7;
	}

	public override void update() {
		base.update();
		Helpers.decrementTime(ref hyperChargeDelay);

		for (int i = bubblesOnField.Count - 1; i >= 0; i--) {
			if (bubblesOnField[i].destroyed) {
				float timeCutShort = bubblesOnField[i].maxTime - bubblesOnField[i].time;
				bubbleAfterlifeTimes.Add(0.4f);
				bubblesOnField.RemoveAt(i);
				continue;
			}
		}

		for (int i = bubbleAfterlifeTimes.Count - 1; i >= 0; i--) {
			bubbleAfterlifeTimes[i] -= Global.spf;
			if (bubbleAfterlifeTimes[i] <= 0) {
				bubbleAfterlifeTimes.RemoveAt(i);
			}
		}
	}

	// Friendly reminder that this method MUST be deterministic across all clients, i.e. don't vary it on a field that could vary locally.
	public override void getProjectile(Point pos, int xDir, Player player, float chargeLevel, ushort netProjId) {
		if (chargeLevel < 3) {
			if (player.ownedByLocalPlayer) {
				int type = 0;
				if (player.input.isHeld(Control.Up, player)) {
					type = 1;
				}
				var proj = new BubbleSplashProj(type, pos, xDir, player, netProjId, rpc: true);
				bubblesOnField.Add(proj);
			}
		} else if (chargeLevel >= 3 && player.character is MegamanX mmx) {
			player.setNextActorNetId(netProjId);
			mmx.popAllBubbles();
			float time = 0;
			for (int i = 0; i < 6; i++) {
				mmx?.chargedBubbles?.Add(new BubbleSplashProjCharged(this, pos, xDir, player, time, player.getNextActorNetId(true)));
				time += 0.2f;
			}
		}
	}
}

public class BubbleSplashProj : Projectile {
	int size;

	public BubbleSplashProj(
		int type, Point pos, int xDir, Player player, ushort netProjId,
		int? size = null, int? randX = null, int? randY = null,
		bool rpc = false
	) : base(
		BubbleSplash.netWeapon, pos, xDir,
		75, 1, player, "bubblesplash_proj_start", 0, 0f,
		netProjId, player.ownedByLocalPlayer
	) {
		// RNG shenanigans.
		if (randX == null) {
			randX = Helpers.randomRange(75, 125);
		}
		if (randY == null) {
			randY = Helpers.randomRange(75, 125);
		}
		if (size == null) {
			size = Helpers.randomRange(0, spriteVariants.Length - 1);
		}
		// Create variables.
		this.size = size.Value;
		maxTime = 0.75f;
		useGravity = false;

		vel.x *= randX.Value / 100f;
		vel.y = -20 * (randY.Value / 100f);

		if (type == 0) {
			vel.y *= 0.5f;
			vel.x *= 1.75f;
		} else {
			vel.y *= 3;
		}

		fadeSprite = "bubblesplash_pop";
		fadeSound = "bubbleSplashPop";
		fadeOnAutoDestroy = true;
		projId = (int)ProjIds.BubbleSplash;

		if (rpc) {
			rpcCreate(
				pos, player, netProjId, xDir,
				(byte)type, (byte)size, (byte)randX, (byte)randY
			);
		}
	}

	public override void update() {
		base.update();
		vel.y -= 1.65f;

		if (sprite.name == "bubblesplash_proj_start" && isAnimOver()) {
			changeSprite(spriteVariants[size], true);
		}
	}

	public static string[] spriteVariants = {
		"bubblesplash_proj",
		"bubblesplash_proj2",
		"bubblesplash_proj3",
	};

	public static Projectile rpcInvoke(ProjParameters arg) {
		return new BubbleSplashProj(
			arg.extraData[0], arg.pos, arg.xDir, arg.player, arg.netId,
			arg.extraData[1], arg.extraData[2], arg.extraData[3]
		);
	}
}

public class BubbleSplashProjCharged : Projectile {
	public MegamanX? character;
	public float yPos;
	public float initTime;
	public BubbleSplashProjCharged(Weapon weapon, Point pos, int xDir, Player player, float time, ushort netProjId, bool rpc = false) :
		base(weapon, pos, xDir, 75, 1, player, "bubblesplash_proj1", 0, 0, netProjId, player.ownedByLocalPlayer) {
		useGravity = false;
		fadeSprite = "bubblesplash_pop";

		int randColor = Helpers.randomRange(0, 2);
		if (randColor == 0) changeSprite("bubblesplash_proj2", true);
		if (randColor == 1) changeSprite("bubblesplash_proj3", true);
		character = (player.character as MegamanX);
		initTime = time;
		this.time = time;
		sprite.doesLoop = true;
		projId = (int)ProjIds.BubbleSplashCharged;

		if (rpc) {
			rpcCreate(pos, player, netProjId, xDir);
		}

		isOwnerLinked = true;
		if (player.character != null) {
			owningActor = player.character;
		}
	}

	public override void update() {
		base.update();
		if (character == null || !Global.level.gameObjects.Contains(character) || (character.player.weapon is not BubbleSplash)) {
			destroySelf();
			return;
		}
		time += Global.spf;
		if (time > 2) time = 0;

		float x = 20 * MathF.Sin(time * 5);
		yPos = -15 * time;
		Point newPos = character.pos.addxy(x, yPos);
		changePos(newPos);
	}

	public override void onDestroy() {
		if (character != null) {
			character?.chargedBubbles?.Remove(this);
		}
	}
}
