﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lidgren.Network;
using Newtonsoft.Json;

namespace MMXOnline;

public class RPC {
	public NetDeliveryMethod netDeliveryMethod;
	public bool isString;
	public bool toHostOnly;
	public bool isServerMessage;
	public int index;

	// Need templates? Use these:
	// -Sending a value to an actor: RPCChangeDamage
	public static RPCSendString sendString = new();
	public static RPCStartLevel startLevel = new();
	public static RPCSpawnCharacter spawnCharacter = new();
	public static RPCUpdateActor updateActor = new();
	public static RPCApplyDamage applyDamage = new();
	public static RPCDecShieldAmmo decShieldAmmo = new();
	public static RPCShoot shoot =  new RPCShoot(NetDeliveryMethod.ReliableOrdered);
	public static RPCShoot shootFast = new RPCShoot(NetDeliveryMethod.Unreliable);
	public static RPCDestroyActor destroyActor = new();
	public static RPCPlayerToggle playerToggle = new();
	public static RPCDestroyPlayer destroyCharacter = new();
	public static RPCKillPlayer killPlayer = new();
	public static RPCCreateAnim createAnim = new();
	public static RPCCreateProj createProj = new();
	public static RPCCreateActor createActor = new();
	public static RPCSwitchCharacter switchCharacter = new();
	public static RPCReflectProj reflectProj = new();
	public static RPCJoinLateRequest joinLateRequest = new();
	public static RPCJoinLateResponse joinLateResponse = new();
	public static RPCUpdateStarted updateStarted = new();
	public static RPCHostPromotion hostPromotion = new();
	public static RPCMatchOver matchOver = new();
	public static RPCSwitchTeam switchTeam = new();
	public static RPCSyncTeamScores syncTeamScores = new();
	public static RPCSyncGameTime syncGameTime = new();
	public static RPCSyncSetupTime syncSetupTime = new();
	public static RPCSendKillFeedEntry sendKillFeedEntry = new();
	public static RPCSendChatMessage sendChatMessage = new();
	public static RPCSyncControlPoints syncControlPoints = new();
	public static RPCSetHyperAxlTime setHyperAxlTime = new();
	public static RPCAxlShoot axlShoot = new();
	public static RPCAxlDisguise axlDisguise = new();
	public static RPCReportPlayerRequest reportPlayerRequest = new();
	public static RPCReportPlayerResponse reportPlayerResponse = new();
	public static RPCKickPlayerRequest kickPlayerRequest = new();
	public static RPCKickPlayerResponse kickPlayerResponse = new();
	public static RPCVoteKickStart voteKickStart = new();
	public static RPCVoteKickEnd voteKickEnd = new();
	public static RPCVoteKick voteKick = new();
	public static RPCEndMatchRequest endMatchRequest = new();
	public static RPCPeriodicServerSync periodicServerSync = new();
	public static RPCPeriodicServerPing periodicServerPing = new();
	public static RPCPeriodicHostSync periodicHostSync = new();
	public static RPCUpdatePlayer updatePlayer = new();
	public static RPCAddBot addBot = new();
	public static RPCRemoveBot removeBot = new();
	public static RPCMakeSpectator makeSpectator = new();
	public static RPCSyncValue syncValue = new();
	public static RPCHeal heal = new();
	public static RPCCommandGrabPlayer commandGrabPlayer = new();
	public static RPCClearOwnership clearOwnership = new();
	public static RPCActorToggle actorToggle = new();
	public static RPCPlaySound playSound = new();
	public static RPCStopSound stopSound = new();
	public static RPCAddDamageText addDamageText = new();
	public static RPCSyncAxlBulletPos syncAxlBulletPos = new();
	public static RPCSyncAxlScopePos syncAxlScopePos = new();
	public static RPCBoundBlasterStick boundBlasterStick = new();
	public static RPCBroadcastLoadout broadcastLoadout = new();
	public static RPCCreditPlayerKillMaverick creditPlayerKillMaverick = new();
	public static RPCCreditPlayerKillVehicle creditPlayerKillVehicle = new();
	public static RPCChangeDamage changeDamage = new();
	public static RPCLogWeaponKills logWeaponKills = new();
	public static RPCCheckRAEnter checkRAEnter = new();
	public static RPCRAEnter raEnter = new();
	public static RPCCheckRCEnter checkRCEnter = new();
	public static RPCRCEnter rcEnter = new();
	public static RPCUseSubTank useSubtank = new();
	public static RPCPossess possess = new();
	public static RPCSyncPossessInput syncPossessInput = new();
	public static RPCFeedWheelGator feedWheelGator = new();
	public static RPCHealDoppler healDoppler = new();
	public static RPCResetFlag resetFlags = new();
	// For mods and stuff.
	// It allow to not override stuff when developing mods.
	public static RPCCustom custom = new();
	public static RpcChangeOwnership changeOwnership = new();
	public static RpcReflect reflect = new();
	public static RpcDeflect deflect = new();
	public static RpcUpdateMaxTime updateMaxTime = new();
	public static RpcReviveSigma reviveSigma = new();

	public static RPC[] templates = {
		// Strings.
		sendString,
		// Server messages.
		updateStarted,
		reportPlayerRequest,
		reportPlayerResponse,
		kickPlayerRequest,
		kickPlayerResponse,
		updatePlayer, 
		addBot,
		removeBot,
		makeSpectator,
		logWeaponKills,
		periodicServerSync,
		periodicServerPing,
		periodicHostSync,
		voteKickStart,
		voteKickEnd,
		voteKick,
		// Match stuff.
		startLevel,
		hostPromotion,
		joinLateRequest,
		joinLateResponse,
		matchOver,
		endMatchRequest,
		// General stuff.
		applyDamage,
		heal,
		updateActor,
		destroyActor,
		actorToggle,
		spawnCharacter,
		destroyCharacter,
		playerToggle,
		killPlayer,
		createAnim,
		createProj,
		createActor,
		clearOwnership,
		sendChatMessage,
		playSound,
		stopSound,
		syncValue,
		// Gameplay stuff.
		broadcastLoadout,
		switchCharacter,
		reflectProj,
		commandGrabPlayer,
		addDamageText,
		changeDamage,
		// Gamemode specific.
		switchTeam,
		syncTeamScores,
		syncGameTime,
		syncSetupTime,
		resetFlags,
		sendKillFeedEntry,
		syncControlPoints,
		// XOD Only stuff.
		decShieldAmmo,
		shoot,
		shootFast,
		axlShoot,
		axlDisguise,
		setHyperAxlTime,
		syncAxlBulletPos,
		syncAxlScopePos,
		boundBlasterStick,
		creditPlayerKillMaverick,
		creditPlayerKillVehicle,
		checkRAEnter,
		raEnter,
		checkRCEnter,
		rcEnter,
		useSubtank,
		possess,
		syncPossessInput,
		feedWheelGator,
		healDoppler,
		// Custom generic RCP.
		custom,
	};

	public virtual void invoke(params byte[] arguments) {
	}

	public virtual void invoke(string message) {
	}

	public void sendFromServer(NetServer s_server, byte[] bytes) {
		if (s_server.Connections.Count == 0) return;

		var om = s_server.CreateMessage();
		om.Write((byte)templates.IndexOf(this));
		if (bytes.Length > ushort.MaxValue) {
			throw new Exception("SendFromServer RPC bytes too big, max ushort.MaxValue");
		}
		ushort argCount = (ushort)bytes.Length;
		var argCountBytes = BitConverter.GetBytes(argCount);
		om.Write(argCountBytes[0]);
		om.Write(argCountBytes[1]);
		if (bytes.Length > 0) {
			om.Write(bytes);
		}
		s_server.SendToAll(om, netDeliveryMethod, 0);
	}
}

public class RPCSendString : RPC {
	public RPCSendString() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
	}
}

public class RPCStartLevel : RPC {
	public RPCStartLevel() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
	}

	public override void invoke(string message) {
		var rpcStartLevelJson = JsonConvert.DeserializeObject<RPCStartLevelJson>(message);

		// Sometimes server won't have player in it preventing mainPlayer from being set, in this case need to be a late joiner
		if (rpcStartLevelJson?.server.players.Any(p => p.id == Global.serverClient?.serverPlayer.id) != true) {
			Global.serverClient?.disconnect("Host recreated before client could reconnect");
			Global.serverClient = null;
			Menu.change(new ErrorMenu(new string[] { "Could not reconnect in time.", "Please rejoin the server manually." }, new JoinMenu(false)));
			return;
		}

		Global.level?.startLevel(rpcStartLevelJson.server, false);
	}
}

public class RPCStartLevelJson {
	public Server server;
	public RPCStartLevelJson(Server server) {
		this.server = server;
	}
}

public class BackloggedSpawns {
	public int playerId;
	public Point spawnPoint;
	public int xDir;
	public ushort charNetId;
	public int charNum;
	public float time;
	public byte[] extraData;

	public BackloggedSpawns(
		int charNum, byte[] extraData, int playerId, 
		Point spawnPoint, int xDir, ushort charNetId
	) {
		this.charNum = charNum;
		this.extraData = extraData;
		this.playerId = playerId;
		this.spawnPoint = spawnPoint;
		this.xDir = xDir;
		this.charNetId = charNetId;
		time = 0;
	}
	public bool trySpawnPlayer() {
		var player = Global.level.getPlayerById(playerId);
		// Player could not exist yet if late joiner.
		if (player != null) {
			player.spawnCharAtPoint(
				charNum, extraData,
				spawnPoint, xDir, charNetId, false
			);
			return true;
		}
		return false;
	}
}

public class RPCSpawnCharacter : RPC {
	public RPCSpawnCharacter() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		float x = BitConverter.ToSingle(new byte[] { arguments[0], arguments[1], arguments[2], arguments[3] }, 0);
		float y = BitConverter.ToSingle(new byte[] { arguments[4], arguments[5], arguments[6], arguments[7] }, 0);
		int xDir = arguments[8] - 128;
		int playerId = arguments[9];
		ushort charNetId = BitConverter.ToUInt16(new byte[] { arguments[10], arguments[11] }, 0);
		int charNum = arguments[12];
		byte[] extraData;
		if (arguments.Length > 13) {
			extraData = arguments[13..];
		} else {
			extraData = [];
		}
		var player = Global.level.getPlayerById(playerId);
		// Player could not exist yet if late joiner.
		if (player != null) {
			player.spawnCharAtPoint(
				charNum, extraData,
				new Point(x, y), xDir, charNetId, false
			);
		} else {
			Global.level.backloggedSpawns.Add(
				new BackloggedSpawns(
					charNum, extraData,
					playerId, new Point(x, y), xDir, charNetId
				)
			);
		}
	}

	public void sendRpc(int charNum, byte[] extraData, Point spawnPos, int xDir, int playerId, ushort charNetId) {
		if (Global.serverClient == null) return;
		List<byte> sendBytes = new();

		sendBytes.AddRange(BitConverter.GetBytes(spawnPos.x));
		sendBytes.AddRange(BitConverter.GetBytes(spawnPos.y));
		sendBytes.Add((byte)(xDir + 128));
		sendBytes.Add((byte)playerId);
		sendBytes.AddRange(BitConverter.GetBytes(charNetId));
		sendBytes.Add((byte)charNum);
		sendBytes.AddRange(extraData);

		Global.serverClient.rpc(this, sendBytes.ToArray());
	}
}

public class FailedSpawn {
	public Point spawnPos;
	public int xDir;
	public ushort netId;
	public float time;
	public FailedSpawn(Point spawnPos, int xDir, ushort netId) {
		this.spawnPos = spawnPos;
		this.xDir = xDir;
		this.netId = netId;
	}
}

public class RPCApplyDamage : RPC {
	public RPCApplyDamage() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int ownerId = arguments[0];
		float damage = BitConverter.ToSingle(
			new byte[] { arguments[1], arguments[2], arguments[3], arguments[4] }, 0
		);
		float hitCooldown = BitConverter.ToSingle(
			new byte[] { arguments[5], arguments[6], arguments[7], arguments[8] }, 0
		);
		int flinch = arguments[9];
		ushort victimId = BitConverter.ToUInt16(
			new byte[] { arguments[10], arguments[11] }, 0
		);
		bool weakness = arguments[12] == 1;
		int weaponIndex = arguments[13];
		int weaponKillFeedIndex = arguments[14];
		ushort actorId = BitConverter.ToUInt16(
			new byte[] { arguments[15], arguments[16] }, 0
		);
		ushort projId = BitConverter.ToUInt16(
			new byte[] { arguments[17], arguments[18] }, 0
		);
		byte linkedMeleeId = arguments[19];
		bool isLinkedMelee = (linkedMeleeId != byte.MaxValue);

		var player = Global.level.getPlayerById(ownerId);
		var victim = Global.level.getActorByNetId(victimId, true);
		Actor? actor = null;
		// For when the projectile was a melee without a NetID.
		if (isLinkedMelee) {
			Actor? mainActor = Global.level.getActorByNetId(actorId, true);
			List<Projectile> projs = new();
			if (mainActor != null) {
				// We try to search anything with a matching MeleeID.
				actor = mainActor.getMeleeProjById(linkedMeleeId, mainActor.pos, false);

				// If that fails... screw it we create one.
				if (actor == null) {
					actor = new GenericMeleeProj(
						new Weapon(), mainActor.pos, (ProjIds)projId,
						player, damage, flinch, hitCooldown, mainActor,
						addToLevel: false
					);
				}
			}
		}
		// For normal projectiles.
		else {
			actor = (actorId == 0 ? null : Global.level.getActorByNetId(actorId, true));
		}

		if (player != null && victim != null) {
			Damager.applyDamage(
				player,
				damage,
				hitCooldown,
				flinch,
				victim,
				weakness,
				weaponIndex,
				weaponKillFeedIndex,
				actor,
				projId,
				sendRpc: false);
		}
	}

	public void sendRpc(byte[] byteArray) {
		Global.serverClient?.rpc(applyDamage, byteArray);
	}
}

public class BackloggedDamage {
	public ushort actorId;
	public Action<Actor> damageAction;
	public float time;
	public BackloggedDamage(ushort actorId, Action<Actor> damageAction) {
		this.actorId = actorId;
		this.damageAction = damageAction;
	}
}

public class RPCDecShieldAmmo : RPC {
	public RPCDecShieldAmmo() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];
		float decAmmoAmount = BitConverter.ToSingle(new byte[] { arguments[1], arguments[2], arguments[3], arguments[4] }, 0);

		var player = Global.level.getPlayerById(playerId);

		if ((player?.character as MegamanX)?.chargedRollingShieldProj != null) {
			(player.character as MegamanX).chargedRollingShieldProj.decAmmo(decAmmoAmount);
		}
	}
}

public class RPCShoot : RPC {
	public RPCShoot(NetDeliveryMethod netDeliveryMethod) {
		this.netDeliveryMethod = netDeliveryMethod;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];
		int xPos = BitConverter.ToInt16(new byte[] { arguments[1], arguments[2] }, 0);
		int yPos = BitConverter.ToInt16(new byte[] { arguments[3], arguments[4] }, 0);
		int xDir = arguments[5] - 128;
		int chargeLevel = arguments[6];
		ushort projNetId = BitConverter.ToUInt16(new byte[] { arguments[7], arguments[8] }, 0);
		int weaponIndex = arguments[9];

		var player = Global.level.getPlayerById(playerId);
		(player?.character as MegamanX)?.shootRpc(
			new Point(xPos, yPos), weaponIndex, xDir, chargeLevel, projNetId, false
		);
	}
}

public class BufferedDestroyActor {
	public ushort netId;
	public string destroySprite;
	public string destroySound;
	public float time;
	public BufferedDestroyActor(ushort netId, string destroySprite, string destroySound) {
		this.netId = netId;
		this.destroySprite = destroySprite;
		this.destroySound = destroySound;
	}
}

public class RPCDestroyActor : RPC {
	public RPCDestroyActor() {
		netDeliveryMethod = NetDeliveryMethod.ReliableUnordered;
	}

	public override void invoke(params byte[] arguments) {
		ushort netId = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		int spriteIndex = BitConverter.ToUInt16(new byte[] { arguments[2], arguments[3] }, 0);
		int soundIndex = BitConverter.ToUInt16(new byte[] { arguments[4], arguments[5] }, 0);

		string destroySprite = null;
		string destroySound = null;
		if (spriteIndex < Global.spriteCount) {
			destroySprite = Global.spriteNameByIndex[spriteIndex];
		}
		if (soundIndex < Global.soundCount) {
			destroySound = Global.soundNameByIndex[soundIndex];
		}
		float x = BitConverter.ToSingle(new byte[] { arguments[6], arguments[7], arguments[8], arguments[9] }, 0);
		float y = BitConverter.ToSingle(new byte[] { arguments[10], arguments[11], arguments[12], arguments[13] }, 0);
		Point destroyPos = new Point(x, y);

		bool favorDefenderProjDestroy = arguments[14] == 1;
		float speed = BitConverter.ToSingle(new byte[] { arguments[15], arguments[16], arguments[17], arguments[18] }, 0);

		var actor = Global.level.getActorByNetId(netId);
		if (actor != null) {
			// Special case for favor the defender projectiles: give it time to move to its position of destruction, before destroying it
			if (actor is Projectile proj && speed > 0 && favorDefenderProjDestroy) {
				proj.moveToPosThenDestroy(destroyPos, speed);
				return;
			}

			if (actor is FrostShieldProj fsp) {
				fsp.noSpawn = true;
			}

			actor.changePos(destroyPos);
			// Any actors with custom destroySelf methods that are invoked by RPC need to be specified here
			if (actor is Character) {
				(actor as Character).destroySelf(destroySprite, destroySound, disableRpc: true);
			} else if (actor is RollingShieldProjCharged) {
				(actor as RollingShieldProjCharged).destroySelf(destroySprite, destroySound, disableRpc: true);
			} else {
				actor.destroySelf(destroySprite, destroySound, disableRpc: true);
			}
		} else {
			Global.level.bufferedDestroyActors.Add(new BufferedDestroyActor(netId, destroySprite, destroySound));
		}
	}

	public void sendRpc(ushort netId, ushort spriteIndex, ushort soundIndex, Point pos, bool favorDefenderProjDestroy, float speed) {
		var netIdBytes = BitConverter.GetBytes(netId);
		var spriteIndexBytes = BitConverter.GetBytes(spriteIndex);
		var soundIndexBytes = BitConverter.GetBytes(soundIndex);
		var xBytes = BitConverter.GetBytes(pos.x);
		var yBytes = BitConverter.GetBytes(pos.y);
		var speedBytes = BitConverter.GetBytes(speed);

		Global.serverClient?.rpc(RPC.destroyActor, netIdBytes[0], netIdBytes[1], spriteIndexBytes[0], spriteIndexBytes[1], soundIndexBytes[0], soundIndexBytes[1],
			xBytes[0], xBytes[1], xBytes[2], xBytes[3], yBytes[0], yBytes[1], yBytes[2], yBytes[3],
			(byte)(favorDefenderProjDestroy ? 1 : 0),
			speedBytes[0], speedBytes[1], speedBytes[2], speedBytes[3]);
	}
}

public class RPCDestroyPlayer : RPC {
	public RPCDestroyPlayer() {
		netDeliveryMethod = NetDeliveryMethod.ReliableUnordered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];

		var player = Global.level.getPlayerById(playerId);
		player?.destroyCharacter();
	}
}

public enum RPCToggleType {
	AddTransformEffect,
	PlayDingSound,
	StartCrystalize,
	StopCrystalize,
	StrikeChainReversed,
	StockCharge,
	UnstockCharge,
	StartRaySplasher,
	StopRaySplasher,
	StartBarrier,
	StopBarrier,
	StockSaber,
	UnstockSaber,
	SetWhiteAxl,
	ReviveVileTo2,
	ReviveVileTo5,
	ReviveX,
	StartRev,
	StopRev
}

public class RPCPlayerToggle : RPC {
	public RPCPlayerToggle() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];
		RPCToggleType toggleId = (RPCToggleType)(int)arguments[1];

		var player = Global.level.getPlayerById(playerId);
		if (player?.character == null) {
			return;
		} else if (toggleId == RPCToggleType.AddTransformEffect) {
			player.character?.addTransformAnim();
		} else if (toggleId == RPCToggleType.PlayDingSound) {
			player.character?.playSound("m10ding");
		} else if (toggleId == RPCToggleType.StartCrystalize) {
			player.character?.crystalizeStart();
		} else if (toggleId == RPCToggleType.StopCrystalize) {
			player.character?.crystalizeEnd();
		} else if (toggleId == RPCToggleType.StrikeChainReversed) {
			(player?.character as MegamanX)?.strikeChainProj?.reverseDir();
		} else if (toggleId == RPCToggleType.StockCharge) {
			if (player?.character is MegamanX mmx) {
				mmx.stockedCharge = true;
			}
		} else if (toggleId == RPCToggleType.UnstockCharge) {
			if (player?.character is MegamanX mmx) {
				mmx.stockedCharge = false;
			}
		} else if (toggleId == RPCToggleType.StartRaySplasher) {
			if (player.character is MegamanX mmx) {
				mmx.isShootingRaySplasher = true;
			}
		} else if (toggleId == RPCToggleType.StopRaySplasher) {
			if (player.character is MegamanX mmx) {
				mmx.isShootingRaySplasher = false;
			}
		} else if (toggleId == RPCToggleType.StartBarrier) {
			if (player.character is MegamanX mmx) {
				mmx.barrierTime = mmx.barrierDuration;
			}
		} else if (toggleId == RPCToggleType.StopBarrier) {
			if (player.character is MegamanX mmx) {
				mmx.barrierTime = 0;
			}
		} else if (toggleId == RPCToggleType.StockSaber) {
			if (player.character is MegamanX mmx) {
				mmx.stockedXSaber = true;
			}
		} else if (toggleId == RPCToggleType.UnstockSaber) {
			if (player.character is MegamanX mmx) {
				mmx.stockedXSaber = false;
			} 
		} else if (toggleId == RPCToggleType.SetWhiteAxl) {
			if (player.character is Axl axl) {
				axl.whiteAxlTime = axl.maxHyperAxlTime;
			}
		} else if (toggleId == RPCToggleType.ReviveVileTo2) {
			player.reviveVileNonOwner(false);
		} else if (toggleId == RPCToggleType.ReviveVileTo5) {
			player.reviveVileNonOwner(true);
		} else if (toggleId == RPCToggleType.ReviveX) {
			player.reviveXNonOwner();
		} else if (toggleId == RPCToggleType.StartRev) {
			if (player.character is Axl axl) {
				axl.isNonOwnerRev = true;
			}
		} else if (toggleId == RPCToggleType.StopRev) {
			if (player.character is Axl axl) {
				axl.isNonOwnerRev = false;
			}
		}
	}

	public void sendRpc(int playerId, RPCToggleType toggleType) {
		Global.serverClient?.rpc(this, (byte)playerId, (byte)toggleType);
	}
}

public enum RPCActorToggleType {
	SonicSlicerBounce,
	StartGravityWell,
	CrackedWallDamage,
	CrackedWallDestroy,
	AddWolfSigmaMusicSource,
	AddDrLightMusicSource,
	AddDrDopplerMusicSource,
	AddGoliathMusicSource,
	StartMechSelfDestruct,
	ShakeCamera,
	ReverseRocketPunch,
	DropFlagManual,
	AwardCurrency,
	AddViralSigmaMusicSource,
	MorphMothCocoonSelfDestruct,
	AddKaiserSigmaMusicSource,
	AddKaiserViralSigmaMusicSource,
	ChangeToParriedState,
	KaiserShellFadeOut,
	AddVaccineTime,
	AddWolfSigmaIntroMusicSource,
}

public class RPCActorToggle : RPC {
	public RPCActorToggle() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		RPCActorToggleType toggleId = (RPCActorToggleType)arguments[2];

		// A hack to avoid having to create new RPC and redeploy server code
		if (toggleId == RPCActorToggleType.CrackedWallDamage) {
			if (Global.isHost) {
				byte crackedWallId = arguments[0];
				byte damage = arguments[1];
				CrackedWall crackedWall = Global.level.getCrackedWallById(crackedWallId);
				if (crackedWall != null) {
					crackedWall.applyDamage(damage, null, null, null, null);
				}
			}
			return;
		} else if (toggleId == RPCActorToggleType.CrackedWallDestroy) {
			byte crackedWallId = arguments[0];
			CrackedWall crackedWall = Global.level.getCrackedWallById(crackedWallId);
			if (crackedWall != null) {
				crackedWall.destroySelf();
			}
			return;
		}

		ushort netId = BitConverter.ToUInt16(arguments, 0);
		var actor = Global.level.getActorByNetId(netId);
		if (actor == null) {
			return;
		}
		if (toggleId == RPCActorToggleType.SonicSlicerBounce) {
			actor.playSound("dingX2");
			new Anim(actor.pos, "sonicslicer_sparks", actor.xDir, null, true);
		} else if (toggleId == RPCActorToggleType.StartGravityWell && actor is GravityWellProjCharged gw) {
			gw.started = true;
		}
		else if (toggleId == RPCActorToggleType.AddWolfSigmaMusicSource) {
			actor.addMusicSource("wolfSigma", actor.pos.addxy(0, -75), false);
		} else if (toggleId == RPCActorToggleType.AddWolfSigmaIntroMusicSource) {
			actor.addMusicSource("wolfSigmaIntro", actor.pos.addxy(0, -75), false, loop: false);
		} else if (toggleId == RPCActorToggleType.AddDrLightMusicSource) {
			actor.addMusicSource("drLigth_X1", actor.getCenterPos(), false, loop: false);
		} else if (toggleId == RPCActorToggleType.AddDrDopplerMusicSource) {
			actor.addMusicSource("demo_X3", actor.getCenterPos(), false, loop: false);
		} else if (toggleId == RPCActorToggleType.AddGoliathMusicSource) {
			actor.addMusicSource("vile_X3", actor.getCenterPos(), true);
		} else if (toggleId == RPCActorToggleType.AddViralSigmaMusicSource) {
			actor.addMusicSource("virusSigma", actor.getCenterPos(), true);
		} else if (toggleId == RPCActorToggleType.AddKaiserSigmaMusicSource) {
			actor.destroyMusicSource();
			actor.addMusicSource("kaiserSigma", actor.getCenterPos(), true);
		} else if (toggleId == RPCActorToggleType.AddKaiserViralSigmaMusicSource) {
			actor.destroyMusicSource();
			actor.addMusicSource("demo_X3", actor.getCenterPos(), true);
		}
		else if (toggleId == RPCActorToggleType.StartMechSelfDestruct && actor is RideArmor ra) {
			ra.selfDestructTime = Global.spf;
		} else if (toggleId == RPCActorToggleType.ShakeCamera) {
			actor.shakeCamera();
		} else if (toggleId == RPCActorToggleType.ReverseRocketPunch) {
			if (actor is RocketPunchProj rpp) {
				rpp.reversed = true;
			}
		} else if (toggleId == RPCActorToggleType.DropFlagManual) {
			if (Global.isHost && actor is Character chr) {
				chr.dropFlag();
				chr.dropFlagCooldown = 1;
			}
		} else if (toggleId == RPCActorToggleType.AwardCurrency) {
			if (actor is Character chr) {
				chr.player.currency += 5;
			}
		} else if (toggleId == RPCActorToggleType.MorphMothCocoonSelfDestruct) {
			if (actor is MorphMothCocoon mmc) {
				mmc.selfDestructTime = 0.1f;
			}
		} else if (toggleId == RPCActorToggleType.ChangeToParriedState) {
			(actor as Character)?.changeState(new ParriedState(), true);
		} else if (toggleId == RPCActorToggleType.KaiserShellFadeOut) {
			(actor as Anim)?.setFadeOut(0.25f);
		} else if (toggleId == RPCActorToggleType.AddVaccineTime) {
			(actor as Character)?.addVaccineTime(2);
		}
	}

	public void sendRpc(ushort? netId, RPCActorToggleType toggleType) {
		if (netId == null) return;
		byte[] netIdBytes = BitConverter.GetBytes((ushort)netId);
		Global.serverClient?.rpc(this, netIdBytes[0], netIdBytes[1], (byte)toggleType);
	}

	// A hack to avoid having to create new RPC and redeploy server code
	public void sendRpcDamageCw(byte crackedWallId, byte damage) {
		Global.serverClient?.rpc(this, crackedWallId, damage, (byte)RPCActorToggleType.CrackedWallDamage);
	}

	// A hack to avoid having to create new RPC and redeploy server code
	public void sendRpcDestroyCw(byte crackedWallId) {
		Global.serverClient?.rpc(this, crackedWallId, 0, (byte)RPCActorToggleType.CrackedWallDestroy);
	}
}

public class RPCKillPlayer : RPC {
	public RPCKillPlayer() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}
	public override void invoke(params byte[] arguments) {
		int hasOwnerId = arguments[0];
		int killerId = arguments[1];
		int assisterId = arguments[2];
		ushort victimId = BitConverter.ToUInt16(new byte[] { arguments[3], arguments[4] }, 0);
		int? weaponIndex = null;
		ushort? projId = null;
		if (arguments.Length >= 6) {
			weaponIndex = arguments[5];
		}
		if (arguments.Length >= 7) {
			projId = BitConverter.ToUInt16(new byte[] { arguments[6], arguments[7] }, 0);
		}

		var victim = Global.level.getPlayerById(victimId);
		var killer = (hasOwnerId == 0) ? null : Global.level.getPlayerById(killerId);
		var assister = (hasOwnerId == 0) ? null : Global.level.getPlayerById(assisterId);

		// If assister is passed in as the same as the killer it is a sentinel value for no killer
		if (assister == killer) assister = null;

		victim?.lastCharacter?.killPlayer(killer, assister, weaponIndex, projId);
	}
}

public class RPCCreateAnim : RPC {
	public RPCCreateAnim() {
		netDeliveryMethod = NetDeliveryMethod.Unreliable;
	}

	public override void invoke(params byte[] arguments) {
		var netProjByte = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		int spriteIndex = BitConverter.ToUInt16(new byte[] { arguments[2], arguments[3] }, 0);
		float xPos = BitConverter.ToSingle(new byte[] { arguments[4], arguments[5], arguments[6], arguments[7] }, 0);
		float yPos = BitConverter.ToSingle(new byte[] { arguments[8], arguments[9], arguments[10], arguments[11] }, 0);
		int xDir = arguments[12] - 128;

		if (spriteIndex >= Global.spriteCount) {
			return;
		}
		string netSprName = Global.spriteNameByIndex[spriteIndex];

		if (netSprName == "parasitebomb_latch_start") {
			new ParasiteAnim(
				new Point(xPos, yPos), netSprName,
				netProjByte, sendRpc: false, ownedByLocalPlayer: false
			);
			return;
		}

		// The rest of the bytes are for optional, expensive-to-sync data that should be used sparingly.
		RPCAnimModel extendedAnimModel = null;
		Actor? zIndexRelActor = null;
		if (arguments.Length > 13) {
			var argumentsList = arguments.ToList();
			var restofArgs = argumentsList.GetRange(13, argumentsList.Count - 13);
			extendedAnimModel = Helpers.deserialize<RPCAnimModel>(restofArgs.ToArray());
			if (extendedAnimModel.zIndexRelActorNetId != null) {
				zIndexRelActor = Global.level.getActorByNetId(
					extendedAnimModel.zIndexRelActorNetId.Value, true
				);
			}
		}

		new Anim(
			new Point(xPos, yPos), netSprName, xDir, netProjByte, true, ownedByLocalPlayer: false,
			zIndex: extendedAnimModel?.zIndex, zIndexRelActor: zIndexRelActor,
			fadeIn: extendedAnimModel?.fadeIn ?? false,
			hasRaColorShader: extendedAnimModel?.hasRaColorShader ?? false
		);
	}
}

public class RPCSwitchCharacter : RPC {
	public RPCSwitchCharacter() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];
		int charNum = arguments[1];
		var player = Global.level.getPlayerById(playerId);
		if (player == null) return;
		player.newCharNum = charNum;
	}
}

public class RPCSwitchTeam : RPC {
	public const string prefix = "changeteam:";

	public RPCSwitchTeam() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
	}

	public override void invoke(string message) {
		if (Global.level == null) return;
		getMessageParts(message, out int playerId, out int alliance);
		var player = Global.level.getPlayerById(playerId);
		if (player == null) return;
		player.newAlliance = alliance;
	}

	public static string getSendMessage(int playerId, int alliance) {
		return prefix + playerId + ":" + alliance;
	}

	public static void getMessageParts(string message, out int playerId, out int alliance) {
		var pieces = message.RemovePrefix(prefix).Split(':');
		playerId = int.Parse(pieces[0]);
		alliance = int.Parse(pieces[1]);
	}
}

// Only covers airblast type reflect
public class RPCReflectProj : RPC {
	public RPCReflectProj() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		ushort projNetId = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		int playerId = arguments[2];
		int angle = arguments[3];

		Player reflecter = Global.level.getPlayerById(playerId);
		if (reflecter == null) return;

		var proj = Global.level.getActorByNetId(projNetId) as Projectile;
		if (proj != null) {
			float floatAngle = angle * 2;
			proj.reflect2(reflecter, floatAngle);
		}
	}

	public void sendRpc(ushort? netId, int reflecterPlayerId, float angle) {
		if (netId == null) return;
		var netIdBytes = BitConverter.GetBytes((ushort)netId);
		angle = Helpers.to360(angle);
		angle *= 0.5f;
		Global.serverClient?.rpc(reflectProj, netIdBytes[0], netIdBytes[1], (byte)reflecterPlayerId, (byte)(int)(angle));
	}
}

public class RPCJoinLateRequest : RPC {
	public RPCJoinLateRequest() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		toHostOnly = true;
	}

	public override void invoke(params byte[] arguments) {
		var serverPlayer = Helpers.deserialize<ServerPlayer>(arguments);

		Global.level.addPlayer(serverPlayer, true);

		foreach (var player in Global.level.players) {
			player.charNetId = null;
			if (player.character != null) {
				player.charNetId = player.character.netId;
				player.charXPos = player.character.pos.x;
				player.charYPos = player.character.pos.y;
				player.charXDir = player.character.xDir;
				player.charRollingShieldNetId = (player?.character as MegamanX)?.chargedRollingShieldProj?.netId;
			}
		}

		var controlPoints = new List<ControlPointResponseModel>();
		foreach (var cp in Global.level.controlPoints) {
			controlPoints.Add(new ControlPointResponseModel() {
				alliance = cp.alliance,
				num = cp.num,
				locked = cp.locked,
				captured = cp.captured,
				captureTime = cp.captureTime
			});
		}

		var magnetMines = new List<MagnetMineResponseModel>();
		foreach (var go in Global.level.gameObjects) {
			var magnetMine = go as MagnetMineProj;
			if (magnetMine != null && magnetMine.netId != null && magnetMine.player != null) {
				magnetMines.Add(new MagnetMineResponseModel() {
					x = magnetMine.pos.x,
					y = magnetMine.pos.y,
					netId = magnetMine.netId.Value,
					playerId = magnetMine.player.id
				});
			}
		}

		var turrets = new List<TurretResponseModel>();
		foreach (var go in Global.level.gameObjects) {
			var turret = go as RaySplasherTurret;
			if (turret != null && turret.netId != null && turret.netOwner != null) {
				turrets.Add(new TurretResponseModel() {
					x = turret.pos.x,
					y = turret.pos.y,
					netId = turret.netId.Value,
					playerId = turret.netOwner.id
				});
			}
		}

		var joinLateResponseModel = new JoinLateResponseModel() {
			players = Global.level.players.Select(p => new PlayerPB(p)).ToList(),
			newPlayer = serverPlayer,
			controlPoints = controlPoints,
			magnetMines = magnetMines,
			turrets = turrets
		};

		Global.serverClient?.rpc(RPC.joinLateResponse, Helpers.serialize(joinLateResponseModel));
	}
}

public class RPCJoinLateResponse : RPC {
	public RPCJoinLateResponse() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		JoinLateResponseModel joinLateResponseModel = null;
		try {
			joinLateResponseModel = Helpers.deserialize<JoinLateResponseModel>(arguments);
		} catch {
			try {
				Logger.logEvent(
					"error",
					"Bad joinLateResponseModel bytes. name: " +
					Options.main.playerName + ", match: " + Global.level?.server?.name +
					", bytes: " + arguments.ToString()
				);
				//Console.Write(message); 
			} catch { }
			throw;
		}

		// Original requester
		if (Global.serverClient.serverPlayer.id == joinLateResponseModel.newPlayer.id) {
			Global.level.joinedLateSyncPlayers(joinLateResponseModel.players);
			Global.level.joinedLateSyncControlPoints(joinLateResponseModel.controlPoints);
			Global.level.joinedLateSyncMagnetMines(joinLateResponseModel.magnetMines);
			Global.level.joinedLateSyncTurrets(joinLateResponseModel.turrets);
		} else {
			Global.level.addPlayer(joinLateResponseModel.newPlayer, true);
		}
	}
}

public class RPCUpdateStarted : RPC {
	public RPCUpdateStarted() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isServerMessage = true;
	}
}

public class RPCHostPromotion : RPC {
	public RPCHostPromotion() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];

		var player = Global.level.getPlayerById(playerId);
		if (player == null) return;

		if (!player.serverPlayer.isHost) {
			player.serverPlayer.isHost = true;
			player.promoteToHost();
		}
	}
}

public class RPCMatchOver : RPC {
	public RPCMatchOver() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
	}

	public override void invoke(string message) {
		var rpcMatchOverResponse = JsonConvert.DeserializeObject<RPCMatchOverResponse>(message);
		Global.level?.gameMode?.matchOverRpc(rpcMatchOverResponse);
	}
}

public class RPCSyncTeamScores : RPC {
	public RPCSyncTeamScores() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		if (Global.level?.gameMode != null) {
			Global.level.gameMode.teamPoints = arguments;
		}
	}
}

public class RPCSyncGameTime : RPC {
	public RPCSyncGameTime() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		if (Global.level?.gameMode == null) return;

		int time = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		Global.level.gameMode.remainingTime = time;
		if (Global.level.gameMode.remainingTime.Value <= 10 && Global.level.gameMode.remainingTime.Value > 0) Global.playSound("text");
		if (arguments.Length >= 4) {
			int elimTime = BitConverter.ToUInt16(new byte[] { arguments[2], arguments[3] }, 0);
			Global.level.gameMode.eliminationTime = elimTime;
			Global.level.gameMode.localElimTimeInc = 0;
		}
	}
}

public class RPCSyncSetupTime : RPC {
	public RPCSyncSetupTime() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		if (Global.level?.gameMode == null) return;

		int time = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		Global.level.gameMode.setupTime = time;
		if (Global.level.gameMode.setupTime <= 0) {
			Global.level.gameMode.setupTime = 0;
			Global.level.gameMode.removeAllGates();
		}
	}
}

public class RPCKillFeedEntryResponse {
	public string message;
	public int alliance;
	public int? playerId;

	public RPCKillFeedEntryResponse(string message, int alliance, int? id) {
		this.message = message;
		this.alliance = alliance;
		playerId = id;
	}
}

public class RPCSendKillFeedEntry : RPC {
	public RPCSendKillFeedEntry() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
	}

	public override void invoke(string message) {
		var response = JsonConvert.DeserializeObject<RPCKillFeedEntryResponse>(message);
		Player player = null;
		if (response.playerId != null) {
			player = Global.level.getPlayerById(response.playerId.Value);
		}
		Global.level.gameMode.addKillFeedEntry(new KillFeedEntry(response.message, response.alliance, player));
	}
}

public class RPCSendChatMessage : RPC {
	public RPCSendChatMessage() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
	}

	public override void invoke(string message) {
		var response = JsonConvert.DeserializeObject<ChatEntry>(message);
		Global.level?.gameMode?.chatMenu.addChatEntry(response);
	}
}

public class RPCSyncControlPoints : RPC {
	public RPCSyncControlPoints() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int cpIndex = arguments[0];
		int alliance = arguments[1];
		int captureProgress = arguments[2];
		int attackerCount = arguments[3];
		int defenderCount = arguments[4];
		bool locked = arguments[5] == 0 ? false : true;
		bool captured = arguments[6] == 0 ? false : true;
		int redCaptureProgress = arguments[7];
		int blueCaptureProgress = arguments[8];
		int redRemainingTime = arguments[9];
		int blueRemainingTime = arguments[10];
		byte hillAttackerCountSync = arguments[11];

		ControlPoint cp;
		if (Global.level?.gameMode is ControlPoints) {
			if (cpIndex >= Global.level.controlPoints.Count) return;
			cp = Global.level.controlPoints[cpIndex];
		} else {
			cp = Global.level.hill;
		}

		cp.alliance = alliance;
		cp.captureTime = captureProgress;
		cp.attackerCount = attackerCount;
		cp.defenderCount = defenderCount;
		cp.locked = locked;
		cp.captured = captured;
		cp.redCaptureTime = redCaptureProgress;
		cp.blueCaptureTime = blueCaptureProgress;
		cp.redRemainingTime = redRemainingTime;
		cp.blueRemainingTime = blueRemainingTime;
		cp.hillAttackerCountSync = hillAttackerCountSync;
	}
}

public class RPCSetHyperAxlTime : RPC {
	public RPCSetHyperAxlTime() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];
		int time = arguments[1];
		int type = arguments[2];
		var player = Global.level.getPlayerById(playerId);
		if (player?.character is Axl axl) {
			if (type == 1) axl.whiteAxlTime = time;
		}

	}

	public void sendRpc(int playerId, float time, int type) {
		Global.serverClient?.rpc(this, (byte)playerId, (byte)(int)time, (byte)type);
	}
}

public class RPCAxlShoot : RPC {
	public RPCAxlShoot() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];
		int projId = BitConverter.ToUInt16(new byte[] { arguments[1], arguments[2] }, 0);
		ushort netId = BitConverter.ToUInt16(new byte[] { arguments[3], arguments[4] }, 0);
		float x = BitConverter.ToSingle(new byte[] { arguments[5], arguments[6], arguments[7], arguments[8] }, 0);
		float y = BitConverter.ToSingle(new byte[] { arguments[9], arguments[10], arguments[11], arguments[12] }, 0);
		int xDir = Helpers.byteToDir(arguments[13]);
		float angle = Helpers.byteToDegree(arguments[14]);
		int axlBulletWeaponType = arguments.InRange(15) ? arguments[16] : 0;

		var player = Global.level.getPlayerById(playerId);
		if (player?.character == null) return;

		if (projId == (int)ProjIds.AxlBullet || projId == (int)ProjIds.MetteurCrash || projId == (int)ProjIds.BeastKiller || projId == (int)ProjIds.MachineBullets || projId == (int)ProjIds.RevolverBarrel || projId == (int)ProjIds.AncientGun) {
			var pos = new Point(x, y);
			var flash = new Anim(pos, "axl_pistol_flash", 1, null, true);
			flash.angle = angle;
			flash.frameSpeed = 1;
			if (projId == (int)ProjIds.AxlBullet) {
				var bullet = new AxlBulletProj(new AxlBullet((AxlBulletWeaponType)axlBulletWeaponType), pos, player, Point.createFromAngle(angle), netId);
				player.character.playSound("axlBullet");
			} else if (projId == (int)ProjIds.MetteurCrash) {
				//var bullet = new MettaurCrashProj(new AxlBullet((AxlBulletWeaponType)axlBulletWeaponType), pos, player, Point.createFromAngle(angle), netId);
				player.character.playSound("mettaurCrash");
			} else if (projId == (int)ProjIds.BeastKiller) {
				//var bullet = new BeastKillerProj(new AxlBullet((AxlBulletWeaponType)axlBulletWeaponType), pos, player, Point.createFromAngle(angle), netId);
				player.character.playSound("beastKiller");
			} else if (projId == (int)ProjIds.MachineBullets) {
				//var bullet = new MachineBulletProj(new AxlBullet((AxlBulletWeaponType)axlBulletWeaponType), pos, player, Point.createFromAngle(angle), netId);
				player.character.playSound("machineBullets");
			} else if (projId == (int)ProjIds.RevolverBarrel) {
				//var bullet = new RevolverBarrelProj(new AxlBullet((AxlBulletWeaponType)axlBulletWeaponType), pos, player, Point.createFromAngle(angle), netId);
				player.character.playSound("revolverBarrel");
			} else if (projId == (int)ProjIds.AncientGun) {
				//var bullet = new AncientGunProj(new AxlBullet((AxlBulletWeaponType)axlBulletWeaponType), pos, player, Point.createFromAngle(angle), netId);
				player.character.playSound("ancientGun3");
			}
		} else if (projId == (int)ProjIds.CopyShot) {
			var pos = new Point(x, y);
			var bullet = new CopyShotProj(new AxlBullet(), pos, 0, player, Point.createFromAngle(angle), netId);
			var flash = new Anim(pos, "axl_pistol_flash_charged", 1, null, true);
			flash.angle = angle;
			flash.frameSpeed = 3;
			player.character.playSound("axlBulletCharged");
		} else if (projId == (int)ProjIds.BlastLauncher) {
			var pos = new Point(x, y);
			var bullet = new GrenadeProj(
				new BlastLauncher(0), pos, xDir, player, Point.createFromAngle(angle), null, new Point(), 0, netId
			);
			var flash = new Anim(pos, "axl_pistol_flash", 1, null, true);
			flash.angle = angle;
			flash.frameSpeed = 1;
			player.character.playSound("grenadeShoot");
		} else if (projId == (int)ProjIds.GreenSpinner) {
			var pos = new Point(x, y);
			var bullet = new GreenSpinnerProj(
				new BlastLauncher(0), pos, xDir, player, Point.createFromAngle(angle), null, netId
			);
			var flash = new Anim(pos, "axl_pistol_flash_charged", 1, null, true);
			flash.angle = angle;
			flash.frameSpeed = 3;
			player.character.playSound("rocketShoot");
		} else if (projId == (int)ProjIds.RayGun || projId == (int)ProjIds.RayGun2) {
			var pos = new Point(x, y);
			Point velDir = Point.createFromAngle(angle);
			if (projId == (int)ProjIds.RayGun) {
				var bullet = new RayGunProj(new RayGun(0), pos, xDir, player, velDir, netId);
				player.character.playSound("raygun");
			} else if (projId == (int)ProjIds.RayGun2) {
				var bullet = Global.level.getActorByNetId(netId) as RayGunAltProj;
				if (bullet == null) {
					new RayGunAltProj(new RayGun(0), pos, pos, xDir, player, netId);
				}
			}

			string fs = "axl_raygun_flash";
			if (Global.level.gameMode.isTeamMode && player.alliance == GameMode.redAlliance) fs = "axl_raygun_flash2";
			var flash = new Anim(pos, fs, 1, null, true);
			flash.setzIndex(player.character.zIndex - 100);
			flash.angle = angle;
			flash.frameSpeed = 1;
		} else if (projId == (int)ProjIds.SpiralMagnum || projId == (int)ProjIds.SpiralMagnumScoped) {
			var pos = new Point(x, y);
			var bullet = new SpiralMagnumProj(
				new SpiralMagnum(0), pos, 0, 0, player, Point.createFromAngle(angle), null, null, netId
			);
			if (projId == (int)ProjIds.SpiralMagnumScoped) {
				AssassinBulletTrailAnim trail = new AssassinBulletTrailAnim(pos, bullet);
			}
			var flash = new Anim(pos, "axl_pistol_flash", 1, null, true);
			flash.angle = angle;
			flash.frameSpeed = 1;
			player.character.playSound("spiralMagnum");
		} else if (projId == (int)ProjIds.IceGattling || projId == (int)ProjIds.IceGattlingHyper) {
			var pos = new Point(x, y);
			var bullet = new IceGattlingProj(new IceGattling(0), pos, xDir, player, Point.createFromAngle(angle), netId);
			var flash = new Anim(pos, "axl_pistol_flash", 1, null, true);
			flash.angle = angle;
			flash.frameSpeed = 1;
			player.character.playSound("iceGattling");
		} else if (projId == (int)ProjIds.AssassinBullet || projId == (int)ProjIds.AssassinBulletQuick) {
			var pos = new Point(x, y);
			var bullet = new AssassinBulletProj(new AssassinBullet(), pos, new Point(), xDir, player, null, null, netId);
			AssassinBulletTrailAnim trail = new AssassinBulletTrailAnim(pos, bullet);
			var flash = new Anim(pos, "axl_pistol_flash_charged", 1, null, true);
			flash.angle = angle;
			flash.frameSpeed = 3;
			player.character.playSound("assassinate");
		}
	}

	public void sendRpc(int playerId, int projId, ushort netId, Point pos, int xDir, float angle) {
		var xBytes = BitConverter.GetBytes(pos.x);
		var yBytes = BitConverter.GetBytes(pos.y);
		var netIdBytes = BitConverter.GetBytes(netId);
		var projIdBytes = BitConverter.GetBytes((ushort)projId);
		Global.serverClient?.rpc(
			this, (byte)playerId, projIdBytes[0], projIdBytes[1], netIdBytes[0], netIdBytes[1],
			xBytes[0], xBytes[1], xBytes[2], xBytes[3],
			yBytes[0], yBytes[1], yBytes[2], yBytes[3],
			Helpers.dirToByte(xDir), Helpers.degreeToByte(angle)
		);
	}
}

public class RPCAxlDisguiseJson {
	public int playerId;
	public ushort dnaNetId;
	public string targetName;
	public int charNum;
	public byte[] extraData;
	public LoadoutData? loadout;

	public RPCAxlDisguiseJson() { }

	public RPCAxlDisguiseJson(
		int playerId, string targetName, int charNum,
		LoadoutData? loadout = null,
		ushort dnaNetId = 0, byte[]? extraData = null
	) {
		this.playerId = playerId;
		this.targetName = targetName;
		this.charNum = charNum;
		this.dnaNetId = dnaNetId;
		this.loadout = loadout;
		this.extraData = extraData ?? new byte[1];
	}
}

public class RPCAxlDisguise : RPC {
	public RPCAxlDisguise() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
	}

	public override void invoke(string json) {
		var axlDisguiseData = (
			JsonConvert.DeserializeObject<RPCAxlDisguiseJson>(json) ?? throw new NullReferenceException()
		);
		var player = Global.level.getPlayerById(axlDisguiseData.playerId);
		if (player == null) {
			return;
		}
		if (axlDisguiseData.charNum == -1) {
			player.revertToAxl();
		} else if (axlDisguiseData.charNum  == -2) {
			player.revertToAxlDeath();
		} else {
			player.transformAxlNet(axlDisguiseData);
		}
	}
}

public class RPCReportPlayerRequest : RPC {
	public RPCReportPlayerRequest() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
		isServerMessage = true;
	}

	public override void invoke(string playerName) {
	}
}

public class RPCReportPlayerResponse : RPC {
	public RPCReportPlayerResponse() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
	}

	public override void invoke(string json) {
		var reportedPlayer = JsonConvert.DeserializeObject<ReportedPlayer>(json);
		reportedPlayer.chatHistory = Global.level.gameMode.chatMenu.chatHistory.Select(c => c.getDisplayMessage()).ToList();
		reportedPlayer.timestamp = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
		reportedPlayer.description = "";
		Helpers.WriteToFile(reportedPlayer.getFileName(), JsonConvert.SerializeObject(reportedPlayer));
	}
}

public class RPCKickPlayerJson {
	public string playerName;
	public string deviceId;
	public int banTimeMinutes;
	public string banReason;
	public VoteType type;
	public RPCKickPlayerJson(VoteType type, string playerName, string deviceId, int banTimeMinutes, string banReason) {
		this.type = type;
		this.playerName = playerName;
		this.deviceId = deviceId;
		this.banTimeMinutes = banTimeMinutes;
		this.banReason = banReason;
	}
}

public class RPCKickPlayerRequest : RPC {
	public RPCKickPlayerRequest() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
		isServerMessage = true;
	}

	public override void invoke(string playerName) {
	}
}

public class RPCKickPlayerResponse : RPC {
	public RPCKickPlayerResponse() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
	}

	public override void invoke(string kickPlayerJson) {
		var kickPlayerObj = JsonConvert.DeserializeObject<RPCKickPlayerJson>(kickPlayerJson);
		if (kickPlayerObj.playerName == Global.level.mainPlayer.name) {
			Global.leaveMatchSignal = new LeaveMatchSignal(LeaveMatchScenario.Kicked, null, kickPlayerObj.banReason);
		} else {
			string kickMsg = string.Format("{0} was kicked for reason: {1}.", kickPlayerObj.playerName, kickPlayerObj.banReason);
			Global.level.gameMode.chatMenu.addChatEntry(new ChatEntry(kickMsg, null, null, true));
		}
	}
}

public class RPCVoteKickStart : RPC {
	public RPCVoteKickStart() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isString = true;
	}

	public override void invoke(string kickPlayerJson) {
		var rpcKickPlayerObj = JsonConvert.DeserializeObject<RPCKickPlayerJson>(kickPlayerJson);
		if (rpcKickPlayerObj == null) { return; }
		var player = Global.level.getPlayerByName(rpcKickPlayerObj.playerName);
		if (player == null) return;
		VoteKick.sync(player, rpcKickPlayerObj.type, rpcKickPlayerObj.banTimeMinutes, rpcKickPlayerObj.banReason);
	}
}

public class RPCVoteKickEnd : RPC {
	public RPCVoteKickEnd() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		Global.level.gameMode.currentVoteKick = null;
	}
}

public class RPCVoteKick : RPC {
	public RPCVoteKick() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		if (Global.level.gameMode.currentVoteKick == null) return;

		if (arguments[0] == 0) {
			Global.level.gameMode.currentVoteKick.yesVotes++;
		} else {
			Global.level.gameMode.currentVoteKick.noVotes++;
		}
	}
}

public class RPCEndMatchRequest : RPC {
	public RPCEndMatchRequest() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		if (Global.isHost && Global.level?.gameMode != null) {
			Global.level.gameMode.noContest = true;
		}
	}

	public void sendRpc() {
		Global.serverClient?.rpc(this);
	}
}

public class RPCPeriodicServerSync : RPC {
	public RPCPeriodicServerSync() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		var syncModel = Helpers.deserialize<PeriodicServerSyncModel>(arguments);
		if (syncModel.players == null) return;
		if (!Global.level.started) {
			var waitMenu = Menu.mainMenu as WaitMenu;
			if (waitMenu != null) {
				foreach (var player in waitMenu.server.players) {
					var updatePlayer = syncModel.players.Find(p => p.id == player.id);
					if (updatePlayer == null) continue;
					player.isSpectator = updatePlayer.isSpectator;
				}
			}
			return;
		}

		foreach (ServerPlayer serverPlayer in syncModel.players) {
			Player player = Global.level.getPlayerById(serverPlayer.id);
			if (player != null) {
				player.syncFromServerPlayer(serverPlayer);
			} else {
				Global.level.addPlayer(serverPlayer, serverPlayer.joinedLate);
			}
		}
		foreach (var player in Global.level.players.ToList()) {
			if (!syncModel.players.Any(sp => sp.id == player.id)) {
				Global.level.removePlayer(player);
			}
		}
	}
}

public class RPCPeriodicServerPing : RPC {
	public RPCPeriodicServerPing() {
		netDeliveryMethod = NetDeliveryMethod.Unreliable;
	}
}

public class RPCPeriodicHostSync : RPC {
	public RPCPeriodicHostSync() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		if (Global.level?.gameMode == null) return;

		var syncModel = Helpers.deserialize<PeriodicHostSyncModel>(arguments);

		if (syncModel.matchOverResponse != null) {
			Global.level.gameMode.matchOverRpc(syncModel.matchOverResponse);
		}
		Global.level.gameMode.teamPoints = syncModel.teamPoints;
		Global.level.syncCrackedWalls(syncModel.crackedWallBytes);
		Global.level.gameMode.virusStarted = syncModel.virusStarted;
		Global.level.gameMode.safeZoneSpawnIndex = syncModel.safeZoneSpawnIndex;
	}

	public void sendRpc() {
		var syncModel = new PeriodicHostSyncModel() {
			matchOverResponse = Global.level.gameMode.matchOverResponse,
			crackedWallBytes = Global.level.getCrackedWallBytes(),
			virusStarted = Global.level.gameMode.virusStarted,
			safeZoneSpawnIndex = Global.level.gameMode.safeZoneSpawnIndex,
			teamPoints = Global.level.gameMode.teamPoints,
		};

		var bytes = Helpers.serialize(syncModel);

		Global.serverClient?.rpc(this, bytes);
	}
}

public class RPCUpdatePlayer : RPC {
	public RPCUpdatePlayer() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isServerMessage = true;
	}

	public void sendRpc(int playerId, int kills, int deaths) {
		byte[] killsBytes = BitConverter.GetBytes((ushort)kills);
		byte[] deathBytes = BitConverter.GetBytes((ushort)deaths);
		Global.serverClient?.rpc(this, (byte)playerId, killsBytes[0], killsBytes[1], deathBytes[0], deathBytes[1]);
	}
}

public class RPCAddBot : RPC {
	public RPCAddBot() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isServerMessage = true;
	}

	public void sendRpc(int charNum, int team) {
		if (charNum == -1) charNum = 255;
		if (team == -1) team = 255;
		Global.serverClient?.rpc(this, (byte)charNum, (byte)team);
	}
}

public class RPCRemoveBot : RPC {
	public RPCRemoveBot() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isServerMessage = true;
	}

	public void sendRpc(int playerId) {
		Global.serverClient?.rpc(this, (byte)playerId);
	}
}

public class RPCMakeSpectator : RPC {
	public RPCMakeSpectator() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isServerMessage = true;
	}

	public void sendRpc(int playerId, bool makeSpectator) {
		Global.serverClient?.rpc(this, (byte)playerId, makeSpectator ? (byte)0 : (byte)1);
	}
}

public class RPCSyncValue : RPC {
	public RPCSyncValue() {
		netDeliveryMethod = NetDeliveryMethod.Unreliable;
	}

	public override void invoke(params byte[] arguments) {
		float syncValue = BitConverter.ToSingle(arguments, 0);
		Global.level.hostSyncValue = syncValue;
	}

	public void sendRpc(float syncValue) {
		var bytes = BitConverter.GetBytes(syncValue);
		Global.serverClient?.rpc(this, bytes[0], bytes[1], bytes[2], bytes[3]);
	}
}

public class RPCHeal : RPC {
	public RPCHeal() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];
		ushort healNetId = BitConverter.ToUInt16(new byte[] { arguments[1], arguments[2] }, 0);
		int healAmount = arguments[3];

		Actor? actor = Global.level.getActorByNetId(healNetId, true);
		if (actor == null) {
			return;
		}
		Player player = Global.level.getPlayerById(playerId);

		IDamagable? damagable = actor as IDamagable;
		if (damagable != null) {
			if (actor.ownedByLocalPlayer) {
				damagable.heal(player, healAmount, allowStacking: true, drawHealText: true);
			}
		}
	}

	public void sendRpc(Player player, ushort healNetId, int healAmount) {
		var healNetIdBytes = BitConverter.GetBytes(healNetId);
		Global.serverClient?.rpc(this, (byte)player.id, healNetIdBytes[0], healNetIdBytes[1], (byte)healAmount);
	}
}

public enum CommandGrabScenario {
	StrikeChain,
	MK2Grab,
	UPGrab,
	WhirlpoolGrab,
	DeadLiftGrab,
	WSpongeChain,
	WheelGGrab,
	FStagGrab,
	MagnaCGrab,
	BeetleLiftGrab,
	CrushCGrab,
	BBuffaloGrab,
	Release,
}

public class RPCCommandGrabPlayer : RPC {
	public RPCCommandGrabPlayer() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public void maverickGrabCode(
		Maverick grabber, Character victimChar,
		CharState grabbedState, bool isDefenderFavored, MaverickState? optionalGrabberState = null
	) {
		if (grabber == null || victimChar == null) return;
		if (!victimChar.canBeGrabbed()) return;

		if (!isDefenderFavored) {
			if (victimChar.ownedByLocalPlayer &&
				!Helpers.isOfClass(victimChar.charState, grabbedState.GetType())
			) {
				victimChar.changeState(grabbedState, true);
			}
		} else {
			if (grabber.ownedByLocalPlayer) {
				if (optionalGrabberState != null) {
					grabber.changeState(optionalGrabberState, true);
				}
				grabber.state.trySetGrabVictim(victimChar);
			}
		}
	}

	public override void invoke(params byte[] arguments) {
		ushort grabberNetId = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		ushort victimNetId = BitConverter.ToUInt16(new byte[] { arguments[2], arguments[3] }, 0);
		CommandGrabScenario hookScenario = (CommandGrabScenario)arguments[4];
		bool isDefenderFavored = Helpers.byteToBool(arguments[5]);

		var grabber = Global.level.getActorByNetId(grabberNetId);
		if (grabber == null) return;

		var victim = Global.level.getActorByNetId(victimNetId);
		if (victim == null) return;

		Character? grabberChar = grabber as Character;
		Maverick? grabberMaverick = grabber as Maverick;
		Character? victimChar = victim as Character;

		if (hookScenario == CommandGrabScenario.StrikeChain) {
			if (victimChar == null) return;

			if (!isDefenderFavored) {
				var scp = Global.level.getActorByNetId(grabberNetId) as Projectile;
				if (scp is not StrikeChainProj && scp is not WSpongeSideChainProj) return;
				victimChar.hook(scp);
			} else if (grabber is StrikeChainProj scp) {
				scp.hookActor(victimChar);
			}
		} else if (hookScenario == CommandGrabScenario.MK2Grab) {
			if (grabberChar == null || victimChar == null) return;
			if (!victimChar.canBeGrabbed()) return;

			if (!isDefenderFavored) {
				if (victim.ownedByLocalPlayer && victimChar.charState is not VileMK2Grabbed) {
					victimChar.changeState(new VileMK2Grabbed(grabberChar), true);
				}
			} else {
				if (grabberChar.ownedByLocalPlayer) {
					grabberChar.changeState(new VileMK2GrabState(victimChar));
				}
			}
		} else if (hookScenario == CommandGrabScenario.UPGrab) {
			if (grabberChar == null || victimChar == null) return;
			if (!victimChar.canBeGrabbed()) return;

			if (!isDefenderFavored) {
				if (victimChar.ownedByLocalPlayer && victimChar.charState is not UPGrabbed) {
					victimChar.changeState(new UPGrabbed(grabberChar), true);
				}
			} else {
				if (grabberChar.ownedByLocalPlayer) {
					grabberChar.changeState(new XUPGrabState(victimChar));
				}
			}
		} else if (hookScenario == CommandGrabScenario.Release) {
			if (victimChar != null) {
				victimChar.charState?.releaseGrab();
			}
		} else if (grabberMaverick != null && victimChar != null) {
			if (hookScenario == CommandGrabScenario.WhirlpoolGrab && grabber is LaunchOctopus launchOctopus) {
				maverickGrabCode(grabberMaverick, victimChar, new WhirlpoolGrabbed(launchOctopus), isDefenderFavored);
			} else if (hookScenario == CommandGrabScenario.DeadLiftGrab && grabber is BoomerangKuwanger boomerangKuwanger) {
				maverickGrabCode(
					grabberMaverick, victimChar,
					new DeadLiftGrabbed(boomerangKuwanger),
					isDefenderFavored
				);
			} else if (hookScenario == CommandGrabScenario.WheelGGrab && grabber is WheelGator wheelGator) {
				maverickGrabCode(
					grabberMaverick, victimChar,
					new WheelGGrabbed(wheelGator), isDefenderFavored
				);
			} else if (hookScenario == CommandGrabScenario.FStagGrab && grabber is FlameStag flameStag) {
				maverickGrabCode(
					grabberMaverick, victimChar,
					new FStagGrabbed(flameStag), isDefenderFavored,
					optionalGrabberState: new FStagUppercutState(victimChar)
				);
			} else if (hookScenario == CommandGrabScenario.MagnaCGrab && grabber is MagnaCentipede magnaCentipede) {
				maverickGrabCode(
					grabberMaverick, victimChar,
					new MagnaCDrainGrabbed(magnaCentipede), isDefenderFavored
				);
			} else if (hookScenario == CommandGrabScenario.BeetleLiftGrab && grabber is GravityBeetle gravityBeetle) {
				maverickGrabCode(
					grabberMaverick, victimChar,
					new BeetleGrabbedState(gravityBeetle), isDefenderFavored
				);
			} else if (hookScenario == CommandGrabScenario.CrushCGrab && grabber is CrushCrawfish crushCrawfish) {
				maverickGrabCode(
					grabberMaverick, victimChar,
					new CrushCGrabbed(crushCrawfish), isDefenderFavored,
					optionalGrabberState: new CrushCGrabState(victimChar)
				);
			} else if (hookScenario == CommandGrabScenario.BBuffaloGrab && grabber is BlizzardBuffalo blizzardBuffalo) {
				maverickGrabCode(
					grabberMaverick, victimChar,
					new BBuffaloDragged(blizzardBuffalo), isDefenderFavored
				);
			}
		}
	}

	public void sendRpc(ushort? grabberNetId, ushort? victimCharNetId, CommandGrabScenario hookScenario, bool isDefenderFavored) {
		if (victimCharNetId == null) return;
		if (grabberNetId == null) return;

		var grabberNetIdBytes = BitConverter.GetBytes(grabberNetId.Value);
		var victimNetIdBytes = BitConverter.GetBytes(victimCharNetId.Value);
		Global.serverClient?.rpc(
			this, grabberNetIdBytes[0], grabberNetIdBytes[1], victimNetIdBytes[0], victimNetIdBytes[1],
			(byte)hookScenario, Helpers.boolToByte(isDefenderFavored)
		);
	}
}

public class RPCClearOwnership : RPC {
	public RPCClearOwnership() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		ushort netId = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		var actor = Global.level.getActorByNetId(netId, true);
		if (actor == null) return;
		actor.ownedByLocalPlayer = false;
	}

	public void sendRpc(ushort? netId) {
		if (netId == null) return;
		var netIdBytes = BitConverter.GetBytes(netId.Value);
		Global.serverClient?.rpc(this, netIdBytes[0], netIdBytes[1]);
	}
}

public class RPCPlaySound : RPC {
	public RPCPlaySound() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		ushort netId = BitConverter.ToUInt16(arguments, 0);
		ushort soundIndex = BitConverter.ToUInt16(new byte[] { arguments[2], arguments[3] }, 0);

		Actor? actor = Global.level.getActorByNetId(netId);
		if (actor == null) { return; }

		if (soundIndex < Global.soundCount) {
			string sound = Global.soundNameByIndex[soundIndex];
			SoundWrapper? soundWrapper = actor.playSound(sound);
			if (soundWrapper != null) {
				actor.netSounds[soundIndex] = soundWrapper;
			}
		}
	}

	public void sendRpc(string sound, ushort? netId) {
		if (netId == null) return;
		if (Global.serverClient == null) return;

		if (!Global.soundIndexByName.ContainsKey(sound)) {
			return;
		}
		ushort soundIndex = Global.soundIndexByName[sound];
		byte[] netIdBytes = BitConverter.GetBytes((ushort)netId);
		byte[] soundIndexBytes = BitConverter.GetBytes((ushort)soundIndex);
		Global.serverClient?.rpc(this, netIdBytes[0], netIdBytes[1], soundIndexBytes[0], soundIndexBytes[1]);
	}
}

public class RPCStopSound : RPC {
	public RPCStopSound() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		ushort netId = BitConverter.ToUInt16(arguments, 0);
		ushort soundIndex = BitConverter.ToUInt16(new byte[] { arguments[2], arguments[3] }, 0);

		var actor = Global.level.getActorByNetId(netId, true);
		if (actor == null) { return; }

		if (actor.netSounds.ContainsKey(soundIndex)) {
			SoundWrapper soundWrapper = actor.netSounds[soundIndex];
			if (soundWrapper == null) {
				actor.netSounds.Remove(soundIndex);
			} else if (soundWrapper.deleted == false) {
				soundWrapper.sound?.Stop();
			}
		}
	}

	public void sendRpc(string sound, ushort? netId) {
		if (netId == null || Global.serverClient == null) {
			return;
		}
		if (!Global.soundIndexByName.ContainsKey(sound)) {
			return;
		}
		int soundIndex = Global.soundIndexByName[sound];
		byte[] netIdBytes = BitConverter.GetBytes((ushort)netId);
		byte[] soundIndexBytes = BitConverter.GetBytes((ushort)soundIndex);
		Global.serverClient?.rpc(this, netIdBytes[0], netIdBytes[1], soundIndexBytes[0], soundIndexBytes[1]);
	}
}

public class RPCAddDamageText : RPC {
	public RPCAddDamageText() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		ushort netId = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		short damage = BitConverter.ToInt16(new byte[] { arguments[2], arguments[3] }, 0);
		int attackerId = arguments[4];

		if (Global.level?.mainPlayer == null) return;
		if (Global.level.mainPlayer.id != attackerId) return;
		Actor? actor = Global.level.getActorByNetId(netId, true);
		if (actor == null) return;

		float floatDamage = damage / 10f;

		actor.addDamageText(floatDamage);

		if (floatDamage < 0) {
			Global.level.mainPlayer.creditHealing(-floatDamage);
		}
	}

	public void sendRpc(int attackerId, ushort? netId, float damage) {
		if (netId == null) return;
		if (Global.serverClient == null) return;

		byte[] netIdBytes = BitConverter.GetBytes((ushort)netId);

		short damageShort = (short)MathF.Round(damage * 10);

		byte[] damageBytes = BitConverter.GetBytes(damageShort);
		Global.serverClient?.rpc(this, netIdBytes[0], netIdBytes[1], damageBytes[0], damageBytes[1], (byte)attackerId);
	}
}

public class RPCSyncAxlBulletPos : RPC {
	public RPCSyncAxlBulletPos() {
		netDeliveryMethod = NetDeliveryMethod.Unreliable;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];

		short xPos = BitConverter.ToInt16(new byte[] { arguments[1], arguments[2] }, 0);
		short yPos = BitConverter.ToInt16(new byte[] { arguments[3], arguments[4] }, 0);

		var player = Global.level.getPlayerById(playerId);
		Axl? axl = player?.character as Axl;
		if (axl == null) { return; }

		axl.nonOwnerAxlBulletPos = new Point(xPos, yPos);
	}

	public void sendRpc(int playerId, Point bulletPos) {
		byte[] xBytes = BitConverter.GetBytes((short)MathF.Round(bulletPos.x));
		byte[] yBytes = BitConverter.GetBytes((short)MathF.Round(bulletPos.y));
		Global.serverClient?.rpc(this, (byte)playerId, xBytes[0], xBytes[1], yBytes[0], yBytes[1]);
	}
}

public class RPCSyncAxlScopePos : RPC {
	public RPCSyncAxlScopePos() {
		netDeliveryMethod = NetDeliveryMethod.Unreliable;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];

		var player = Global.level.getPlayerById(playerId);
		Axl? axl = player?.character as Axl;
		if (axl == null) {
			return;
		}
		bool isZooming = arguments[1] == 1 ? true : false;

		axl.isNonOwnerZoom = isZooming;

		short sxPos = BitConverter.ToInt16(new byte[] { arguments[2], arguments[3] }, 0);
		short syPos = BitConverter.ToInt16(new byte[] { arguments[4], arguments[5] }, 0);

		short exPos = BitConverter.ToInt16(new byte[] { arguments[6], arguments[7] }, 0);
		short eyPos = BitConverter.ToInt16(new byte[] { arguments[8], arguments[9] }, 0);

		axl.nonOwnerScopeStartPos = new Point(sxPos, syPos);
		axl.netNonOwnerScopeEndPos = new Point(exPos, eyPos);
	}

	public void sendRpc(int playerId, bool isZooming, Point startScopePos, Point endScopePos) {
		byte[] sxBytes = BitConverter.GetBytes((short)MathF.Round(startScopePos.x));
		byte[] syBytes = BitConverter.GetBytes((short)MathF.Round(startScopePos.y));

		byte[] exBytes = BitConverter.GetBytes((short)MathF.Round(endScopePos.x));
		byte[] eyBytes = BitConverter.GetBytes((short)MathF.Round(endScopePos.y));

		Global.serverClient?.rpc(this, (byte)playerId, isZooming ? (byte)1 : (byte)0, sxBytes[0], sxBytes[1], syBytes[0], syBytes[1], exBytes[0], exBytes[1], eyBytes[0], eyBytes[1]);
	}
}

public class RPCBoundBlasterStick : RPC {
	public RPCBoundBlasterStick() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		ushort beaconNetId = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		ushort stuckActorNetId = BitConverter.ToUInt16(new byte[] { arguments[2], arguments[3] }, 0);

		short xPos = BitConverter.ToInt16(new byte[] { arguments[4], arguments[5] }, 0);
		short yPos = BitConverter.ToInt16(new byte[] { arguments[6], arguments[7] }, 0);

		BoundBlasterAltProj? beaconActor = Global.level.getActorByNetId(beaconNetId) as BoundBlasterAltProj;
		Actor? stuckActor = Global.level.getActorByNetId(stuckActorNetId);

		if (beaconActor == null || stuckActor == null) {
			return;
		}
		beaconActor.isActorStuck = true;
		beaconActor.stuckActor = stuckActor;
		beaconActor.stopSyncingNetPos = true;
		beaconActor.changePos(new Point(xPos, yPos));
	}

	public void sendRpc(ushort? beaconNetId, ushort? stuckActorNetId, Point hitPos) {
		if (beaconNetId == null) return;
		if (stuckActorNetId == null) return;

		byte[] beaconNetIdBytes = BitConverter.GetBytes(beaconNetId.Value);
		byte[] stuckActorNetIdBytes = BitConverter.GetBytes(stuckActorNetId.Value);

		byte[] sxBytes = BitConverter.GetBytes((short)MathF.Round(hitPos.x));
		byte[] syBytes = BitConverter.GetBytes((short)MathF.Round(hitPos.y));

		Global.serverClient?.rpc(this, beaconNetIdBytes[0], beaconNetIdBytes[1], stuckActorNetIdBytes[0], stuckActorNetIdBytes[1],
			sxBytes[0], sxBytes[1], syBytes[0], syBytes[1]);
	}
}

public class RPCBroadcastLoadout : RPC {
	public RPCBroadcastLoadout() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		LoadoutData loadout = Helpers.deserialize<LoadoutData>(arguments);
		var player = Global.level?.getPlayerById(loadout.playerId);
		if (player == null) return;

		player.loadout = loadout;
		player.loadoutSet = true;
	}

	public void sendRpc(Player player) {
		byte[] loadoutBytes = Helpers.serialize(player.loadout);
		Global.serverClient?.rpc(this, loadoutBytes);
	}
}

public class RPCCreditPlayerKillMaverick : RPC {
	public RPCCreditPlayerKillMaverick() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int killerId = arguments[0];
		int assisterId = arguments[1];
		ushort victimNetId = BitConverter.ToUInt16(new byte[] { arguments[2], arguments[3] });
		int? weaponIndex = null;
		if (arguments.Length >= 5) weaponIndex = arguments[4];

		Player killer = Global.level.getPlayerById(killerId);
		Player assister = Global.level.getPlayerById(assisterId);
		Maverick? victim = Global.level.getActorByNetId(victimNetId, true) as Maverick;

		victim?.creditMaverickKill(killer, assister, weaponIndex);
	}

	public void sendRpc(Player killer, Player assister, Maverick victim, int? weaponIndex) {
		if (killer == null) return;
		if (victim?.netId == null) return;

		byte assisterId = assister == null ? byte.MaxValue : (byte)assister.id;
		var victimBytes = BitConverter.GetBytes(victim.netId.Value);

		var bytesToAdd = new List<byte>()
		{
				(byte)killer.id, assisterId, victimBytes[0], victimBytes[1]
			};

		if (weaponIndex != null) {
			bytesToAdd.Add((byte)weaponIndex.Value);
		}

		if (Global.serverClient != null) {
			Global.serverClient.rpc(RPC.creditPlayerKillMaverick, bytesToAdd.ToArray());
		}
	}
}

public class RPCCreditPlayerKillVehicle : RPC {
	public RPCCreditPlayerKillVehicle() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int killerId = arguments[0];
		int assisterId = arguments[1];
		ushort victimNetId = BitConverter.ToUInt16(new byte[] { arguments[2], arguments[3] });
		int? weaponIndex = null;
		if (arguments.Length >= 5) weaponIndex = arguments[4];

		Player killer = Global.level.getPlayerById(killerId);
		Player assister = Global.level.getPlayerById(assisterId);
		Actor? victim = Global.level.getActorByNetId(victimNetId, true);

		if (victim is RideArmor ra) {
			ra.creditKill(killer, assister, weaponIndex);
		} else if (victim is RideChaser rc) {
			rc.creditKill(killer, assister, weaponIndex);
		}
	}

	public void sendRpc(Player? killer, Player? assister, Actor victim, int? weaponIndex) {
		if (killer == null) return;
		if (victim?.netId == null) return;

		byte assisterId = assister == null ? byte.MaxValue : (byte)assister.id;
		var victimBytes = BitConverter.GetBytes(victim.netId.Value);

		var bytesToAdd = new List<byte>()
		{
				(byte)killer.id, assisterId, victimBytes[0], victimBytes[1]
			};

		if (weaponIndex != null) {
			bytesToAdd.Add((byte)weaponIndex.Value);
		}

		if (Global.serverClient != null) {
			Global.serverClient.rpc(creditPlayerKillVehicle, bytesToAdd.ToArray());
		}
	}
}

public class RPCChangeDamage : RPC {
	public RPCChangeDamage() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		ushort netId = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		float damage = BitConverter.ToSingle(new byte[] { arguments[2], arguments[3], arguments[4], arguments[5] }, 0);
		int flinch = arguments[6];

		var proj = Global.level.getActorByNetId(netId, true) as Projectile;
		if (proj?.damager != null) {
			proj.damager.damage = damage;
			proj.damager.flinch = flinch;
		}
	}

	public void sendRpc(ushort netId, float damage, int flinch) {
		if (Global.serverClient == null) return;

		byte[] netIdBytes = BitConverter.GetBytes(netId);
		byte[] damageBytes = BitConverter.GetBytes(damage);

		Global.serverClient.rpc(RPC.changeDamage, netIdBytes[0], netIdBytes[1],
			damageBytes[0], damageBytes[1], damageBytes[2], damageBytes[3],
			(byte)flinch);
	}
}

public class RPCLogWeaponKills : RPC {
	public RPCLogWeaponKills() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
		isServerMessage = true;
	}

	public void sendRpc() {
		Global.serverClient?.rpc(logWeaponKills);
	}
}

public class RPCCheckRAEnter : RPC {
	public RPCCheckRAEnter() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];
		ushort raNetId = BitConverter.ToUInt16(new byte[] { arguments[1], arguments[2] }, 0);
		int neutralId = arguments[3];
		int raNum = arguments[4];

		Player player = Global.level.getPlayerById(playerId);
		if (player == null) {
			return;
		}
		RideArmor? ra = Global.level.getActorByNetId(raNetId) as RideArmor;
		if (ra == null) {
			return;
		}
		if (ra.isNeutral && ra.ownedByLocalPlayer && !ra.claimed && ra.character == null) {
			ra.claimed = true;
			RPC.raEnter.sendRpc(player.id, ra.netId, neutralId, raNum);
		}
	}

	public void sendRpc(int playerId, ushort? raNetId, int? neutralId, int raNum) {
		if (raNetId == null) return;
		if (neutralId == null) return;
		byte[] netIdBytes = BitConverter.GetBytes(raNetId.Value);
		Global.serverClient?.rpc(this, (byte)playerId, netIdBytes[0], netIdBytes[1], (byte)neutralId, (byte)raNum);
	}
}

public class RPCRAEnter : RPC {
	public RPCRAEnter() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];

		Player player = Global.level.getPlayerById(playerId);
		if (player == null) return;

		ushort oldRaNetId = BitConverter.ToUInt16(new byte[] { arguments[1], arguments[2] }, 0);
		int neutralId = arguments[3];
		int raNum = arguments[4];

		if (player.ownedByLocalPlayer && player.character != null) {
			RideArmor? oldRa = Global.level.getActorByNetId(oldRaNetId) as RideArmor;
			Point pos = player.character.pos;
			RideArmor ra = new RideArmor(player, pos, raNum, neutralId, player.getNextActorNetId(), true, sendRpc: true);

			float oldRaHealth = ra.health;
			if (oldRa != null) {
				pos = oldRa.pos;
				oldRaHealth = oldRa.health;
				oldRa.destroySelf(doRpcEvenIfNotOwned: true);
			}
			ra.health = oldRaHealth;
			ra.putCharInRideArmor(player.character);
		}
	}

	public void sendRpc(int playerId, ushort? oldRaNetId, int neutralId, int raNum) {
		if (oldRaNetId == null) return;
		byte[] netIdBytes = BitConverter.GetBytes(oldRaNetId.Value);
		Global.serverClient?.rpc(this, (byte)playerId, netIdBytes[0], netIdBytes[1], (byte)neutralId, (byte)raNum);
	}
}

public class RPCCheckRCEnter : RPC {
	public RPCCheckRCEnter() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];
		ushort rcNetId = BitConverter.ToUInt16(new byte[] { arguments[1], arguments[2] }, 0);
		int neutralId = arguments[3];

		Player player = Global.level.getPlayerById(playerId);
		if (player == null) return;
		RideChaser? rc = Global.level.getActorByNetId(rcNetId) as RideChaser;
		if (rc == null) {
			return;
		}
		if (rc.ownedByLocalPlayer && !rc.claimed && rc.character == null) {
			rc.claimed = true;
			RPC.rcEnter.sendRpc(player.id, rc.netId, neutralId);
		}
	}

	public void sendRpc(int playerId, ushort? rcNetId, int? neutralId) {
		if (rcNetId == null) return;
		if (neutralId == null) return;
		byte[] netIdBytes = BitConverter.GetBytes(rcNetId.Value);
		Global.serverClient?.rpc(this, (byte)playerId, netIdBytes[0], netIdBytes[1], (byte)neutralId);
	}
}

public class RPCRCEnter : RPC {
	public RPCRCEnter() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];

		Player player = Global.level.getPlayerById(playerId);
		if (player == null) return;

		ushort oldRcNetId = BitConverter.ToUInt16(new byte[] { arguments[1], arguments[2] }, 0);
		int neutralId = arguments[3];

		if (player.ownedByLocalPlayer && player.character != null) {
			RideChaser? oldRc = Global.level.getActorByNetId(oldRcNetId) as RideChaser;
			Point pos = player.character.pos;
			RideChaser ra = new RideChaser(player, pos, neutralId, player.getNextActorNetId(), true, sendRpc: true);

			float oldRcHealth = ra.health;
			if (oldRc != null) {
				pos = oldRc.pos;
				oldRcHealth = oldRc.health;
				oldRc.destroySelf(doRpcEvenIfNotOwned: true);
			}
			ra.health = oldRcHealth;
			ra.putCharInRideChaser(player.character);
		}
	}

	public void sendRpc(int playerId, ushort? oldRaNetId, int neutralId) {
		if (oldRaNetId == null) return;
		byte[] netIdBytes = BitConverter.GetBytes(oldRaNetId.Value);
		Global.serverClient?.rpc(this, (byte)playerId, netIdBytes[0], netIdBytes[1], (byte)neutralId);
	}
}

public class RPCUseSubTank : RPC {
	public RPCUseSubTank() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		ushort netId = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		int subtankHealAmount = arguments[2];

		var actor = Global.level.getActorByNetId(netId);
		if (actor == null) return;
		if (actor is Character chr) chr.netSubtankHealAmount = subtankHealAmount;
		if (actor is Maverick mvk) mvk.netSubtankHealAmount = subtankHealAmount;
	}

	public void sendRpc(ushort? netId, int subtankHealAmount) {
		if (netId == null) return;
		byte[] netIdBytes = BitConverter.GetBytes(netId.Value);
		Global.serverClient?.rpc(this, netIdBytes[0], netIdBytes[1], (byte)subtankHealAmount);
	}
}

public class RPCPossess : RPC {
	public RPCPossess() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int possesserPlayerId = arguments[0];
		int victimPlayerId = arguments[1];
		bool isUnpossess = arguments[2] == 1;

		var possesser = Global.level.getPlayerById(possesserPlayerId);
		var victim = Global.level.getPlayerById(victimPlayerId);

		if (isUnpossess) {
			victim?.unpossess();
		} else if (possesser != null) {
			victim?.startPossess(possesser);
		}
	}

	public void sendRpc(int possesserPlayerId, int victimPlayerId, bool isUnpossess) {
		Global.serverClient?.rpc(this, (byte)possesserPlayerId, (byte)victimPlayerId, isUnpossess ? (byte)1 : (byte)0);
	}
}

public class RPCSyncPossessInput : RPC {
	public RPCSyncPossessInput() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		int playerId = arguments[0];
		var player = Global.level.getPlayerById(playerId);
		if (player == null) return;
		if (!player.isPossessed()) {
			return;
		}
		bool[] inputHeldArray = Helpers.byteToBoolArray(arguments[1]);
		bool[] inputPressedArray = Helpers.byteToBoolArray(arguments[2]);

		player.input.possessedControlHeld[Control.Left] = inputHeldArray[0];
		player.input.possessedControlHeld[Control.Right] = inputHeldArray[1];
		player.input.possessedControlHeld[Control.Up] = inputHeldArray[2];
		player.input.possessedControlHeld[Control.Down] = inputHeldArray[3];
		player.input.possessedControlHeld[Control.Jump] = inputHeldArray[4];
		player.input.possessedControlHeld[Control.Dash] = inputHeldArray[5];
		player.input.possessedControlHeld[Control.Taunt] = inputHeldArray[6];

		player.input.possessedControlPressed[Control.Left] = inputPressedArray[0];
		player.input.possessedControlPressed[Control.Right] = inputPressedArray[1];
		player.input.possessedControlPressed[Control.Up] = inputPressedArray[2];
		player.input.possessedControlPressed[Control.Down] = inputPressedArray[3];
		player.input.possessedControlPressed[Control.Jump] = inputPressedArray[4];
		player.input.possessedControlPressed[Control.Dash] = inputPressedArray[5];
		player.input.possessedControlPressed[Control.Taunt] = inputPressedArray[6];
	}

	public void sendRpc(int playerId, byte inputHeldByte, byte inputPressedByte) {
		Global.serverClient?.rpc(this, (byte)playerId, inputHeldByte, inputPressedByte);
	}
}

public class RPCFeedWheelGator : RPC {
	public RPCFeedWheelGator() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		ushort netId = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		float damage = BitConverter.ToSingle(new byte[] { arguments[2], arguments[3], arguments[4], arguments[5] }, 0);

		var wheelGator = Global.level.getActorByNetId(netId) as WheelGator;
		if (wheelGator != null) {
			wheelGator.feedWheelGator(damage);
		}
	}

	public void sendRpc(WheelGator wheelGator, float damage) {
		if (wheelGator?.netId == null || Global.serverClient == null) return;

		byte[] netIdBytes = BitConverter.GetBytes(wheelGator.netId.Value);
		byte[] damageBytes = BitConverter.GetBytes(damage);

		Global.serverClient.rpc(RPC.feedWheelGator, netIdBytes[0], netIdBytes[1],
			damageBytes[0], damageBytes[1], damageBytes[2], damageBytes[3]);
	}
}

public class RPCHealDoppler : RPC {
	public RPCHealDoppler() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		ushort netId = BitConverter.ToUInt16(new byte[] { arguments[0], arguments[1] }, 0);
		float damage = BitConverter.ToSingle(new byte[] { arguments[2], arguments[3], arguments[4], arguments[5] }, 0);
		int attackerPlayerId = arguments[6];

		var player = Global.level.getPlayerById(attackerPlayerId);
		var drDoppler = Global.level.getActorByNetId(netId) as DrDoppler;
		if (drDoppler != null) {
			drDoppler.healDrDoppler(player, damage);
		}
	}

	public void sendRpc(DrDoppler drDoppler, float damage, Player attacker) {
		if (drDoppler.netId == null || Global.serverClient == null) return;

		byte[] netIdBytes = BitConverter.GetBytes(drDoppler.netId.Value);
		byte[] damageBytes = BitConverter.GetBytes(damage);

		Global.serverClient.rpc(RPC.healDoppler, netIdBytes[0], netIdBytes[1],
			damageBytes[0], damageBytes[1], damageBytes[2], damageBytes[3], (byte)attacker.id);
	}
}

public class RPCResetFlag : RPC {
	public RPCResetFlag() {
		netDeliveryMethod = NetDeliveryMethod.ReliableOrdered;
	}

	public override void invoke(params byte[] arguments) {
		if (!Global.isHost) {
			if (Global.level?.redFlag?.ownedByLocalPlayer == true || Global.level?.blueFlag?.ownedByLocalPlayer == true) {
				Logger.logException(new Exception("A non-host owned the flags. Removing ownership"), false);
				Global.level.redFlag.ownedByLocalPlayer = false;
				Global.level.redFlag.frameSpeed = 0;
				Global.level.blueFlag.ownedByLocalPlayer = false;
				Global.level.blueFlag.frameSpeed = 0;
				return;
			}
		} else {
			Global.level?.resetFlags();
		}
	}

	public void sendRpc() {
		Global.serverClient?.rpc(RPC.resetFlags);
	}
}
