using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;

namespace RoyaleUs.Modules;

public class BepInExUpdater : MonoBehaviour
{
    // Mettez à jour la version requise et l'URL de téléchargement avec la dernière version de BepInEx.
    public const string RequiredBepInExVersion = "6.0.0-be.725+e1974e26fd7702c66b54c0d6879c90b988cc4920";  // Assurez-vous de la mettre à jour
    public const string BepInExDownloadURL = "https://builds.bepinex.dev/projects/bepinex_be/725/BepInEx-Unity.IL2CPP-win-x86-6.0.0-be.725%2Be1974e2.zip";
    public static bool UpdateRequired => Paths.BepInExVersion.ToString() != RequiredBepInExVersion;

    public void Awake()
    {
        RoyaleUsPlugin.Logger.LogMessage("BepInEx Update Required...");
        RoyaleUsPlugin.Logger.LogMessage($"{Paths.BepInExVersion}, {RequiredBepInExVersion} ");
        this.StartCoroutine(CoUpdate());
    }

    [HideFromIl2Cpp]
    public IEnumerator CoUpdate()
    {
        // Affiche un message pendant le téléchargement
        Task.Run(() => MessageBox(GetForegroundWindow(), "Required BepInEx update is downloading, please wait...", "RoyaleUs", 0));

        // Utilisation de UnityWebRequest pour télécharger le fichier ZIP
        UnityWebRequest www = UnityWebRequest.Get(BepInExDownloadURL);
        yield return www.SendWebRequest();  // Utilisez SendWebRequest pour envoyer la requête.

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            RoyaleUsPlugin.Logger.LogError(www.error);
            yield break;
        }

        // Chemin de stockage du fichier ZIP téléchargé
        var zipPath = Path.Combine(Paths.GameRootPath, ".bepinex_update");
        File.WriteAllBytes(zipPath, www.downloadHandler.data);

        // Préparez le chemin d'exécution de l'exécutable pour lancer la mise à jour
        var tempPath = Path.Combine(Path.GetTempPath(), "RoyaleUsUpdater.exe");
        var asm = Assembly.GetExecutingAssembly();
        var exeName = asm.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith("RoyaleUsUpdater.exe"));

        using (var resource = asm.GetManifestResourceStream(exeName))
        {
            using (var file = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                resource!.CopyTo(file);
            }
        }

        // Lance l'exécutable de mise à jour avec les arguments nécessaires
        var startInfo = new ProcessStartInfo(tempPath, $"--game-path \"{Paths.GameRootPath}\" --zip \"{zipPath}\"")
        {
            UseShellExecute = false
        };
        Process.Start(startInfo);
        Application.Quit();
    }

    // Déclarations des méthodes externes utilisées dans le code pour afficher des messages
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);

    [DllImport("user32.dll")]
    public static extern int MessageBoxTimeout(IntPtr hwnd, String text, String title, uint type, Int16 wLanguageId, Int32 milliseconds);
}

// Harmony patch pour empêcher le chargement de l'écran principal tant que la mise à jour n'est pas terminée
[HarmonyPatch(typeof(SplashManager), nameof(SplashManager.Update))]
public static class StopLoadingMainMenu
{
    public static bool Prefix()
    {
        return !BepInExUpdater.UpdateRequired;
    }
}
