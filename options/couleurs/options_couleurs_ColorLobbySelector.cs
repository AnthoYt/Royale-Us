using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static RoyaleUs.Modules.Colors; // Pour accéder à la liste CustomColor
using static RoyaleUs.Modules.CustomColors; // Si tu utilises ColorStrings ou une méthode liée

public class ColorLobbySelector : MonoBehaviour
{
    public GameObject colorButtonPrefab; // À définir dans l’éditeur Unity
    public Transform buttonContainer;    // Panel/Zone où les boutons seront placés

    void Start()
    {
        CreateColorButtons();
    }

    void CreateColorButtons()
    {
        for (int i = 0; i < Colors.colors.Count; i++)
        {
            var colorData = Colors.colors[i];
            var btnObj = Instantiate(colorButtonPrefab, buttonContainer);
            var btn = btnObj.GetComponent<Button>();
            var img = btnObj.GetComponent<Image>();
            img.color = colorData.color;

            int colorIndex = i;
            btn.onClick.AddListener(() => OnColorPicked(colorIndex));
        }
    }

    void OnColorPicked(int colorIndex)
    {
        if (!RoyaleUs.Modules.GameStates.IsLobby) return; // On ne change la couleur que dans le lobby

        // Vérifie si la couleur est déjà prise
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p != PlayerControl.LocalPlayer && p.Data != null && p.Data.ColorId == colorIndex)
            {
                // Affichage feedback UI à faire si tu veux
                Debug.Log("Couleur déjà prise !");
                return;
            }
        }

        // Applique la couleur en local
        PlayerControl.LocalPlayer.SetColor(colorIndex);

        // Synchronise ce choix avec le réseau
        SendColorChangeNetwork(PlayerControl.LocalPlayer.PlayerId, colorIndex);
    }

    // À compléter suivant ton système réseau personnalisé/mod Among Us
    void SendColorChangeNetwork(byte playerId, int colorIndex)
    {
        // Ici, rédige ton RPC custom pour tous les clients
        // Ex :
        // var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRpcCallId.ColorChange, SendOption.Reliable);
        // writer.Write(playerId);
        // writer.Write(colorIndex);
        // AmongUsClient.Instance.FinishRpcImmediately(writer);
    }

    // Pour la réception côté client (patch/Harmony sur le RPC), applique la couleur au bon joueur
    // public static void OnReceiveColorChange(byte playerId, int colorIndex) { ... }
}