using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static RoyaleUs.Modules.Colors;

/// <summary>
/// Attaché à un Panel Unity dans le lobby.
/// Génère les boutons de couleurs, gère la sélection et envoie le changement au réseau.
/// </summary>
public class ColorLobbySelector : MonoBehaviour
{
    public GameObject colorButtonPrefab; // Prefab bouton avec Image+Button
    public Transform buttonContainer; // Panel dans l'UI

    void Start()
    {
        // Afficher tous les boutons couleurs disponibles
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

    // Quand un joueur clique sur une couleur
    void OnColorPicked(int colorIndex)
    {
        if (!RoyaleUs.Modules.GameStates.IsLobby) return; // On ne change que dans le lobby

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p && p.Data != null && p.Data.ColorId == colorIndex)
            {
                Debug.Log("Couleur déjà prise !");
                return;
            }
        }
        // Appliquer côté local
        PlayerControl.LocalPlayer.SetColor(colorIndex);
        // Synchroniser côté réseau
        ColorSyncSender.SendColorChange(PlayerControl.LocalPlayer.PlayerId, colorIndex);
    }
}
