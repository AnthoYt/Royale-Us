private class ChatNotificationColorsPatch {
    public static bool Prefix(ChatNotification __instance, PlayerControl sender, string text) {
        ...
    }
}

[HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
private static class PlayerTabEnablePatch {
    public static void Postfix(PlayerTab __instance) {
        ...
    }
}

[HarmonyPatch(typeof(LegacySaveManager), nameof(LegacySaveManager.LoadPlayerPrefs))]
private static class LoadPlayerPrefsPatch {
    ...
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckColor))]
private static class PlayerControlCheckColorPatch {
    ...
}
