﻿using System;

namespace MMXOnline;

public enum ZeroDownthrustType {
	Hyouretsuzan,
	Rakukojin,
	QuakeBlazer,
	DropKick,
}

public class HyouretsuzanWeapon : Weapon {
	public static HyouretsuzanWeapon staticWeapon = new();

	public HyouretsuzanWeapon() : base() {
		//damager = new Damager(player, 4, 12, 0.5f);
		index = (int)WeaponIds.Hyouretsuzan;
		weaponBarBaseIndex = 24;
		weaponBarIndex = weaponBarBaseIndex;
		killFeedIndex = 12;
		type = (int)ZeroDownthrustType.Hyouretsuzan;
		displayName = "Hyouretsuzan";
		description = new string[] { "A dive attack that can freeze enemies." };
	}

	public static Weapon getWeaponFromIndex(int index) {
		return index switch {
			(int)ZeroDownthrustType.Hyouretsuzan => new HyouretsuzanWeapon(),
			(int)ZeroDownthrustType.Rakukojin => new RakukojinWeapon(),
			(int)ZeroDownthrustType.QuakeBlazer => new DanchienWeapon(),
			_ => throw new Exception("Invalid Zero Downthrust weapon index!")
		};
	}
}

public class RakukojinWeapon : Weapon {
	public static RakukojinWeapon staticWeapon = new();

	public RakukojinWeapon() : base() {
		//damager = new Damager(player, 4, 12, 0.5f);
		index = (int)WeaponIds.Rakukojin;
		weaponBarBaseIndex = 38;
		killFeedIndex = 37;
		type = (int)ZeroDownthrustType.Rakukojin;
		displayName = "Rakukojin";
		description = new string[] { "Drop with a metal blade that deals more damage", "the faster Zero is falling." };
	}
}

public class DanchienWeapon : Weapon {
	public static DanchienWeapon staticWeapon = new();

	public DanchienWeapon() : base() {
		//damager = new Damager(player, 2, 0, 0.5f);
		index = (int)WeaponIds.QuakeBlazer;
		rateOfFire = 0.3f;
		weaponBarBaseIndex = 38;
		killFeedIndex = 82;
		type = (int)ZeroDownthrustType.QuakeBlazer;
		displayName = "Danchien";
		description = new string[] { "A dive attack that can burn enemies", "and knock them downwards." };
	}
}

public class ZeroDownthrust : CharState {
	public ZeroDownthrustType type;
	public int quakeBlazerBounces;

	public ZeroDownthrust(
		ZeroDownthrustType type
	) : base(
		getSpriteName(type) + "_fall",
		transitionSprite: getSpriteName(type) + "_start"
	) {
		this.type = type;
	}

	public ZeroDownthrust(
		int type
	) : base(
		getSpriteName((ZeroDownthrustType)type) + "_fall",
		transitionSprite: getSpriteName((ZeroDownthrustType)type) + "_start"
	) {
		this.type = (ZeroDownthrustType)type;
	}

	public static string getSpriteName(ZeroDownthrustType type) {
		return type switch {
			ZeroDownthrustType.Hyouretsuzan => "hyouretsuzan",
			ZeroDownthrustType.Rakukojin => "rakukojin",
			_ => "quakeblazer"
		};
	}

	public override void update() {
		base.update();
		if (isUnderwaterQuakeBlazer()) {
			if (!sprite.EndsWith("_water")) {
				transitionSprite += "";
				sprite += "_water";
				defaultSprite += "_water";
				character.changeSpriteFromName(sprite, false);
			}
		}
		if (type == ZeroDownthrustType.QuakeBlazer) {
			int xDir = player.input.getXDir(player);
			if (xDir != 0) {
				character.xDir = xDir;
				character.move(new Point(100 * xDir, 0));
			}
		}
		if (character.grounded) {
			character.changeState(new ZeroDownthrustLand(type), true);
			if (type == ZeroDownthrustType.QuakeBlazer) {
				quakeBlazerExplode(true);
			}
		}
	}

	public bool isUnderwaterQuakeBlazer() {
		return character.isUnderwater() && type == ZeroDownthrustType.QuakeBlazer;
	}

	public void quakeBlazerExplode(bool hitGround) {
		if (!character.ownedByLocalPlayer) return;
		if (isUnderwaterQuakeBlazer()) return;

		character.playSound("circleBlazeExplosion", sendRpc: true);
		new DanchienExplosionProj(
			character.pos.addxy(10 * character.xDir, -10),
			character.xDir, player, player.getNextActorNetId(), sendRpc: true
		);

		if (!hitGround) {
			if (player.input.isHeld(Control.Jump, player) && quakeBlazerBounces < 1) {
				character.vel.y = Physics.JumpSpeed;
				quakeBlazerBounces++;
			}
			character.changeState(new Fall(), true);
		}
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		if (character.vel.y < 0) {
			character.vel.y = 0;
		}
	}
}

public class ZeroDownthrustLand : CharState {
	ZeroDownthrustType type;

	public ZeroDownthrustLand(ZeroDownthrustType type) : base(getSpriteName(type)) {
		exitOnAirborne = true;
		this.type = type;
	}

	public static string getSpriteName(ZeroDownthrustType type) {
		return type switch {
			ZeroDownthrustType.Hyouretsuzan => "hyouretsuzan_land",
			ZeroDownthrustType.Rakukojin => "rakukojin_land",
			_ => "quakeblazer_land"
		};
	}

	public override void update() {
		base.update();
		if (character.isAnimOver()) {
			character.changeToIdleOrFall();
		}
	}

	public override void onEnter(CharState oldState) {
		base.onEnter(oldState);
		altCtrls[1] = true;
		character.playSound("land", sendRpc: true);
		switch (type) {
			case ZeroDownthrustType.Hyouretsuzan:
				character.breakFreeze(player, character.pos.addxy(character.xDir * 5, 0), sendRpc: true);
				break;
			case ZeroDownthrustType.Rakukojin:
				character.playSound("swordthud", sendRpc: true);
				break;
		}
	}
}

public class DanchienExplosionProj : Projectile {
	public DanchienExplosionProj(
		Point pos, int xDir,
		Player player, ushort netProjId, bool sendRpc = false
	) : base(
		DanchienWeapon.staticWeapon, pos, xDir, 0, 2, player, "quakeblazer_explosion",
		0, 0.5f, netProjId, player.ownedByLocalPlayer
	) {
		destroyOnHit = false;
		projId = (int)ProjIds.QuakeBlazer;
		shouldShieldBlock = false;
		if (sendRpc) {
			rpcCreate(pos, owner, netProjId, xDir);
		}
	}

	public override void onStart() {
		base.onStart();
		if (!ownedByLocalPlayer) return;

		new QuakeBlazerFlamePart(
			pos.addxy(0, -10).addRand(5, 5), xDir, -1,
			owner, owner.getNextActorNetId(), rpc: true
		);
		new QuakeBlazerFlamePart(
			pos.addxy(0, -10).addRand(5, 5), xDir,
			1, owner, owner.getNextActorNetId(), rpc: true
		);
		new QuakeBlazerFlamePart(
			pos.addxy(0, 0).addRand(5, 5), xDir,
			0, owner, owner.getNextActorNetId(), rpc: true
		);
		new QuakeBlazerFlamePart(
			pos.addxy(0, 10).addRand(5, 5), xDir,
			-1, owner, owner.getNextActorNetId(), rpc: true
		);
		new QuakeBlazerFlamePart(
			pos.addxy(0, 10).addRand(5, 5), xDir,
			1, owner, owner.getNextActorNetId(), rpc: true
		);

	}

	public override void update() {
		base.update();
		if (isAnimOver()) {
			destroySelf(disableRpc: true);
		}
	}
}

public class QuakeBlazerFlamePart : Projectile {
	public QuakeBlazerFlamePart(
		Point pos, int xDir,
		int speedDir, Player player, ushort netProjId, bool rpc = false
	) : base(
		DanchienWeapon.staticWeapon, pos, xDir, 0, 0, player, "quakeblazer_part",
		0, 1f, netProjId, player.ownedByLocalPlayer
	) {
		projId = (int)ProjIds.QuakeBlazerFlame;
		useGravity = true;
		collider.wallOnly = true;
		destroyOnHit = false;
		shouldShieldBlock = false;
		vel.x = speedDir * 75;
		if (rpc) {
			rpcCreate(pos, player, netProjId, xDir, (byte)(speedDir + 1));
		}
	}

	public override void update() {
		base.update();
		if (grounded) {
			vel = new Point();
		}
		if (isUnderwater()) {
			destroySelf(disableRpc: true);
			return;
		}
		if (isAnimOver()) {
			destroySelf(disableRpc: true);
			return;
		}
	}
}
