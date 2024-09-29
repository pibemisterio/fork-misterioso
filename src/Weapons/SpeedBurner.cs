﻿using System;
using System.Collections.Generic;

namespace MMXOnline;

public class SpeedBurner : Weapon {
	public SpeedBurner(Player? player) : base() {
		if (player != null) {
			damager = new Damager(player, 4, Global.defFlinch, 0.5f);
		}
		shootSounds = new string[] { "speedBurner", "speedBurner", "speedBurner", "speedBurnerCharged" };
		rateOfFire = 1f;
		index = (int)WeaponIds.SpeedBurner;
		weaponBarBaseIndex = 16;
		weaponBarIndex = weaponBarBaseIndex;
		weaponSlotIndex = 16;
		killFeedIndex = 27;
		weaknessIndex = 10;
		damage = "2/4";
		effect = "Fire DOT: 1. Charged Grants Super Armor.";
		hitcooldown = "0-0.25/0";
		Flinch = "0/26";
		FlinchCD = "0/0.5";
	}

	public override void getProjectile(Point pos, int xDir, Player player, float chargeLevel, ushort netProjId) {
		if (chargeLevel < 3) {
			if (!player.character.isUnderwater()) {
				new SpeedBurnerProj(this, pos, xDir, player, netProjId);
			} else {
				player.setNextActorNetId(netProjId);
				new SpeedBurnerProjWater(this, pos, xDir, 0, player, player.getNextActorNetId(true));
				new SpeedBurnerProjWater(this, pos, xDir, 1, player, player.getNextActorNetId(true));
			}
		} else {
			if (player.character.ownedByLocalPlayer) {
				player.character.changeState(new SpeedBurnerCharState(), true);
			}
		}
	}
}

public class SpeedBurnerProj : Projectile {
	float groundSpawnTime;
	float airSpawnTime;
	int groundSpawns;
	public SpeedBurnerProj(Weapon weapon, Point pos, int xDir, Player player, ushort netProjId, bool rpc = false) :
		base(weapon, pos, xDir, 275, 2, player, "speedburner_start", 0, 0, netProjId, player.ownedByLocalPlayer) {
		maxTime = 0.6f;
		projId = (int)ProjIds.SpeedBurner;
		if (rpc) {
			rpcCreate(pos, player, netProjId, xDir);
		}
	}

	public override void update() {
		base.update();
		if (sprite.name == "speedburner_start") {
			if (isAnimOver()) {
				changeSprite("speedburner_proj", true);
			}
		}
		Helpers.decrementTime(ref groundSpawnTime);
		Helpers.decrementTime(ref airSpawnTime);

		if (airSpawnTime == 0) {
			var anim = new Anim(
				pos.addxy(Helpers.randomRange(-10, 10),
				Helpers.randomRange(-10, 10)), "speedburner_dust", xDir, null, true
			);
			anim.vel.x = 50 * xDir;
			anim.vel.y = 10;
			airSpawnTime = 0.05f;
		}
		if (!ownedByLocalPlayer) {
			return;
		}
		CollideData? hit = Global.level.raycast(pos, pos.addxy(0, 18), [typeof(Wall)]);

		if (hit != null && groundSpawnTime == 0) {
			Point spawnPos = pos.addxy((groundSpawns * -15 + 10) * xDir, 0);
			spawnPos.y = hit.hitData.hitPoint?.y - 1 ?? pos.y;
			new SpeedBurnerProjGround(
				weapon, spawnPos, xDir, damager.owner, damager.owner.getNextActorNetId(), rpc: true
			);
			groundSpawns++;

			groundSpawnTime = 0.075f;
		}
	}
}

public class SpeedBurnerProjWater : Projectile {
	float initY;
	float offsetTime;
	float smokeTime;
	public SpeedBurnerProjWater(Weapon weapon, Point pos, int xDir, int type, Player player, ushort netProjId, bool rpc = false) :
		base(weapon, pos, xDir, 275, 1, player, "speedburner_underwater", 0, 0, netProjId, player.ownedByLocalPlayer) {
		maxTime = 0.6f;
		projId = (int)ProjIds.SpeedBurnerWater;
		initY = pos.y;
		if (type == 1) {
			offsetTime = MathF.PI / 2;
		}
		if (rpc) {
			rpcCreate(pos, player, netProjId, xDir);
		}
	}

	public override void update() {
		base.update();

		smokeTime += Global.spf;
		if (smokeTime > 0.1f) {
			smokeTime = 0;
			new Anim(pos, "torpedo_smoke", xDir, null, true);
		}

		var y = initY + MathF.Sin((Global.level.time + offsetTime) * 10) * 15;
		changePos(new Point(pos.x, y));
	}
}

public class SpeedBurnerProjGround : Projectile {
	public SpeedBurnerProjGround(Weapon weapon, Point pos, int xDir, Player player, ushort netProjId, bool rpc = false) :
		base(weapon, pos, xDir, 275, 1, player, "speedburner_ground", 0, 0.25f, netProjId, player.ownedByLocalPlayer) {
		maxTime = 0.4f;
		destroyOnHit = true;
		frameIndex = Helpers.randomRange(0, 2);
		projId = (int)ProjIds.SpeedBurnerTrail;
		if (rpc) {
			rpcCreate(pos, player, netProjId, xDir);
		}
	}

	public override void update() {
		base.update();
	}
}

public class SpeedBurnerCharState : CharState {
	Anim? proj;

	public SpeedBurnerCharState() : base("speedburner", "", "", "") {
		superArmor = true;
		immuneToWind = true;
	}

	public override void update() {
		base.update();

		if (character.isUnderwater() && proj != null) {
			proj.destroySelf();
			proj = null;
		}

		character.move(new Point(character.xDir * 350, 0));

		CollideData collideData = Global.level.checkTerrainCollisionOnce(character, character.xDir, 0);
		if (collideData != null && collideData.isSideWallHit() && character.ownedByLocalPlayer) {
			character.applyDamage(2, player, character, (int)WeaponIds.SpeedBurner, (int)ProjIds.SpeedBurnerRecoil);
			//character.changeState(new Hurt(-character.xDir, Global.defFlinch, 0), true);
			character.changeToIdleOrFall();
			character.playSound("hurt", sendRpc: true);
			return;
		} else if (stateTime > 0.6f) {
			character.changeToIdleOrFall();
			return;
		}

		if (proj != null) {
			proj.changePos(character.pos.addxy(0, -15));
			proj.xDir = character.xDir;
		}
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		character.useGravity = false;
		character.vel.y = 0;
		if (!character.isUnderwater()) {
			proj = new Anim(character.pos, "speedburner_charged", character.xDir, player.getNextActorNetId(), false, sendRpc: true);
		}
	}

	public override void onExit(CharState newState) {
		base.onExit(newState);
		character.useGravity = true;
		if (proj != null && !proj.destroyed) proj.destroySelf();
	}
}
