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
        public static System.Random rnd = new System.Random((int)DateTime.Now.Ticks);

        public static void clearAndReloadRoles() {
            Lovers.clearAndReload();
        }


public static class Lovers
{
    public static PlayerControl lover1;
    public static PlayerControl lover2;
    public static Color color = new Color32(232, 57, 185, byte.MaxValue);

    public static bool bothDie = true;
    public static bool enableChat = true;
    // Les amoureux sauvegardent si le prochain à être exilé est un amoureux, car le RPC de fin de partie vient avant le RPC de l'exilé
    public static bool notAckedExiledIsLover = false;

    public static bool existing() {
        return lover1 != null && lover2 != null && !lover1.Data.Disconnected && !lover2.Data.Disconnected;
    }

    public static bool existingAndAlive() {
        return existing() && !lover1.Data.IsDead && !lover2.Data.IsDead && !notAckedExiledIsLover; // ADD NOT ACKED IS LOVER
    }

    public static PlayerControl otherLover(PlayerControl oneLover) {
        if (!existingAndAlive()) return null;
        if (oneLover == lover1) return lover2;
        if (oneLover == lover2) return lover1;
        return null;
    }

    public static bool existingWithKiller() {
        return existing() && (lover1 == Sidekick.sidekick    || lover2 == Sidekick.sidekick
                           || lover1.Data.Role.IsImpostor || lover2.Data.Role.IsImpostor);
    }

    public static bool hasAliveKillingLover(this PlayerControl player) {
        if (!Lovers.existingAndAlive() || !existingWithKiller())
            return false;
        return (player != null && (player == lover1 || player == lover2));
    }

    public static void clearAndReload() {
        lover1 = null;
        lover2 = null;
        notAckedExiledIsLover = false;
        bothDie = CustomOptionHolder.modifierLoverBothDie.getBool();
        enableChat = CustomOptionHolder.modifierLoverEnableChat.getBool();
    }

    public static PlayerControl getPartner(this PlayerControl player) {
        if (player == null)
            return null;
        if (lover1 == player)
            return lover2;
        if (lover2 == player)
            return lover1;
        return null;
    }
}
