using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;

namespace RoyaleUs.Modules
{
    public class BepInExUpdater : MonoBehaviour
    {
        // Met la version souhaitée ici
        public const string RequiredBepInExVersion = "6.0.0-be.733+995f049";
        public static readonly string BepInExDownloadURL_x86 =
            "https://builds.bepinex.dev/projects/bepinex_be/733/BepInEx-Unity.IL2CPP-win-x86-6.0.0-be.733%2B995f049.zip";
        public static readonly string BepInExDownloadURL_x64 =
            "https://builds.bepinex.dev/projects/bepinex_be/733/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.733%2B995f049.zip";

        public static bool UpdateRequired
        {
            get
            {
                // Comparer la version actuelle à la version requise
                var current = Paths.BepInExVersion.ToString();
                return !string.Equals(current, RequiredBepInExVersion, StringComparison.OrdinalIgnoreCase);
            }
        }

        public void Awake()
        {
            if (!UpdateRequired)
            {
                return;
            }

            RoyaleUsPlugin.Logger.LogMessage($"BepInEx Update Required: {Paths.BepInExVersion} → {RequiredBepInExVersion}");
            this.StartCoroutine(CoUpdate());
        }

        [HideFromIl2Cpp]
        public IEnumerator CoUpdate()
        {
            // Message pendant le téléchargement
            Task.Run(() => MessageBox(GetForegroundWindow(),
                "BepInEx update is downloading, please wait...", "RoyaleUs Updater", 0));

            // Choisir la bonne URL selon l'architecture
            bool is64 = Environment.Is64BitProcess; // ou une autre méthode pour détecter x64
            string url = is64 ? BepInExDownloadURL_x64 : BepInExDownloadURL_x86;

            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                RoyaleUsPlugin.Logger.LogError($"Failed to download BepInEx: {www.error}");
                yield break;
            }

            var zipPath = Path.Combine(Paths.GameRootPath, ".bepinex_update.zip");
            File.WriteAllBytes(zipPath, www.downloadHandler.data);

            // Extraire l'archive
            try
            {
                var extractPath = Path.Combine(Paths.GameRootPath, "BepInEx_new");
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath, true);

                // Supprimer l'ancien BepInEx (optionnel : à gérer avec soin)
                var oldDir = Path.Combine(Paths.GameRootPath, "BepInEx");
                if (Directory.Exists(oldDir))
                    Directory.Delete(oldDir, true);

                // Renommer le nouveau dossier
                Directory.Move(extractPath, oldDir);
            }
            catch (Exception ex)
            {
                RoyaleUsPlugin.Logger.LogError($"Extraction or replacement failed: {ex}");
                yield break;
            }

            // Lancer l'exécutable de mise à jour si tu en as un, sinon redémarrer
            Application.Quit();
            yield break;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);
    }

    [HarmonyPatch(typeof(SplashManager), nameof(SplashManager.Update))]
    public static class StopLoadingMainMenu
    {
        public static bool Prefix()
        {
            return !BepInExUpdater.UpdateRequired;
        }
    }
}
