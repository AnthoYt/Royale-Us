using System.Linq;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using RoyaleUs.Objects;
using RoyaleUs.Utilities;
using static RoyaleUs.RoyaleUs;
using AmongUs.Data;
using Hazel;
using Reactor.Utilities.Extensions;

namespace RoyaleUs
{
    [HarmonyPatch]
    public static class RoyaleUs
    {
        public static readonly System.Random rnd = new System.Random((int)DateTime.Now.Ticks);

        public static void clearAndReloadRoles() {
            Lovers.clearAndReload();
        }

        public static class Lovers
        {
            public static PlayerControl Lover1 { get; private set; }
            public static PlayerControl Lover2 { get; private set; }
            public static readonly Color color = new Color32(232, 57, 185, byte.MaxValue);

            public static bool bothDie = true;
            public static bool enableChat = true;
            public static bool notAckedExiledIsLover = false;

            public static bool existing() =>
                Lover1 != null && Lover2 != null && !Lover1.Data.Disconnected && !Lover2.Data.Disconnected;

            public static bool existingAndAlive() =>
                existing() && !Lover1.Data.IsDead && !Lover2.Data.IsDead && !notAckedExiledIsLover;

            public static PlayerControl otherLover(PlayerControl oneLover) {
                if (!existingAndAlive()) return null;
                return oneLover == Lover1 ? Lover2 : oneLover == Lover2 ? Lover1 : null;
            }

            public static bool existingWithKiller() =>
                existing() && (Lover1 == Sidekick.sidekick || Lover2 == Sidekick.sidekick || Lover1.Data.Role.IsImpostor || Lover2.Data.Role.IsImpostor);

            public static bool hasAliveKillingLover(this PlayerControl player) {
                if (!existingAndAlive() || !existingWithKiller())
                    return false;
                return player != null && (player == Lover1 || player == Lover2);
            }

            public static void clearAndReload() {
                Lover1 = null;
                Lover2 = null;
                notAckedExiledIsLover = false;
                bothDie = CustomOptionHolder.modifierLoverBothDie.getBool();
                enableChat = CustomOptionHolder.modifierLoverEnableChat.getBool();
            }

            public static PlayerControl getPartner(this PlayerControl player) {
                if (player == null)
                    throw new ArgumentNullException(nameof(player));
                return player == Lover1 ? Lover2 : player == Lover2 ? Lover1 : null;
            }
        }
    }
}
