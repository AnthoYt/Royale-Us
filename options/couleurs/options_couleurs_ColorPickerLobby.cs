using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;

public class ColorPickerLobby : MonoBehaviour
{
    public static List<Colors.CustomColor> AvailableColors;
    public GameObject colorButtonPrefab; // à créer dans Unity, un bouton de sélection

    void Start()
    {
        AvailableColors = Colors.colors;
        CreateColorButtons();
    }

    void CreateColorButtons()
    {
        for (int i = 0; i < AvailableColors.Count; i++)
        {
            var btn = Instantiate(colorButtonPrefab, transform); // parenté à un panel UI
            int colorIndex = i;
            btn.GetComponent<UnityEngine.UI.Image>().color = AvailableColors[i].color;
            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnColorPicked(colorIndex));
        }
    }

    void OnColorPicked(int colorIndex)
    {
        var myPlayer = PlayerControl.LocalPlayer;
        // Vérification : est-ce que la couleur est déjà prise ?
        bool alreadyTaken = false;
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p != myPlayer && p.Data != null && p.Data.ColorId == colorIndex)
                alreadyTaken = true;
        }
        if (alreadyTaken)
        {
            // Popup, etc.
            Debug.LogWarning("Cette couleur est déjà prise !");
            return;
        }
        // Application locale
        myPlayer.SetColor(colorIndex);
        // Synchronisation réseau 
        RpcSendColorChange(myPlayer.PlayerId, colorIndex);
    }

    void RpcSendColorChange(byte playerId, int colorIndex)
    {
        // Ouvre un MessageWriter ou utilise votre système RPC/Mod
        // Envoyer aux autres clients le changement de couleur
    }
}