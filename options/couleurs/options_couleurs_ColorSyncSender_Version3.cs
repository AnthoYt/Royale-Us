using Hazel;

public static class ColorSyncSender
{
    // Envoi réseau : tous les clients reçoivent l'info du changement de couleur
    public static void SendColorChange(byte playerId, int colorIndex)
    {
        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRpcCalls.ColorChange, SendOption.Reliable);
        writer.Write(playerId);
        writer.Write((byte)colorIndex);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
    }
}

public enum CustomRpcCalls : byte
{
    ColorChange = 70 // Numéro arbitraire supérieur à ce que Among Us utilise
}