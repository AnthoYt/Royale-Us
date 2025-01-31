// LobbyUI.cs
// LobbyUI.cs
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public Toggle MapPolus; // Case à cocher pour avoir la map Polus 2.0

    void Start()
    {
        MapPolus.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        // Met à jour la variable du mod en fonction de la case cochée
        MainMod.MapPolus = isOn;
    }
}