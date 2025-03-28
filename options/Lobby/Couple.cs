using System.Linq; // Importation des extensions LINQ (Language Integrated Query) pour les collections.
using HarmonyLib; // Importation de la bibliothèque Harmony pour patcher les méthodes de jeu.
using System; // Importation du namespace System qui fournit des classes de base.
using System.Collections.Generic; // Importation des collections génériques.
using UnityEngine; // Importation des classes UnityEngine.
using RoyaleUs.Objects; // Importation des objets spécifiques à RoyaleUs.
using RoyaleUs.Utilities; // Importation des utilitaires spécifiques à RoyaleUs.
using static RoyaleUs.RoyaleUs; // Importation statique de la classe RoyaleUs.
using AmongUs.Data; // Importation des données du jeu Among Us.
using Hazel; // Importation de la bibliothèque Hazel pour la gestion réseau.
using Reactor.Utilities.Extensions; // Importation des extensions utilitaires de Reactor.

namespace RoyaleUs
{
    // Classe statique pour patcher les méthodes de RoyaleUs
    [HarmonyPatch]
    public static class RoyaleUs
    {
        // Générateur de nombres aléatoires basé sur les ticks actuels de DateTime
        public static readonly System.Random rnd = new System.Random((int)DateTime.Now.Ticks);

        // Méthode pour réinitialiser et recharger les rôles
        public static void clearAndReloadRoles() {
            Lovers.clearAndReload(); // Appel à la méthode de réinitialisation des amoureux
        }

        // Classe statique pour gérer les amoureux
        public static class Lovers
        {
            public static PlayerControl Lover1 { get; private set; } // Premier amoureux
            public static PlayerControl Lover2 { get; private set; } // Deuxième amoureux
            public static readonly Color color = new Color32(232, 57, 185, byte.MaxValue); // Couleur des amoureux

            public static bool bothDie = true; // Indicateur si les deux amoureux meurent ensemble
            public static bool enableChat = true; // Indicateur si le chat entre amoureux est activé
            public static bool notAckedExiledIsLover = false; // Indicateur si l'exilé non confirmé est un amoureux

            // Méthode pour vérifier s'il y a des amoureux existants et connectés
            public static bool existing() =>
                Lover1 != null && Lover2 != null && !Lover1.Data.Disconnected && !Lover2.Data.Disconnected;

            // Méthode pour vérifier s'il y a des amoureux existants et vivants
            public static bool existingAndAlive() =>
                existing() && !Lover1.Data.IsDead && !Lover2.Data.IsDead && !notAckedExiledIsLover;

            // Méthode pour obtenir l'autre amoureux
            public static PlayerControl otherLover(PlayerControl oneLover) {
                if (!existingAndAlive()) return null;
                return oneLover == Lover1 ? Lover2 : oneLover == Lover2 ? Lover1 : null;
            }

            // Méthode pour vérifier s'il y a des amoureux existants avec un tueur
            public static bool existingWithKiller() =>
                existing() && (Lover1 == Sidekick.sidekick || Lover2 == Sidekick.sidekick || Lover1.Data.Role.IsImpostor || Lover2.Data.Role.IsImpostor);

            // Extension pour vérifier si un joueur a un amoureux vivant qui peut tuer
            public static bool hasAliveKillingLover(this PlayerControl player) {
                if (!existingAndAlive() || !existingWithKiller())
                    return false;
                return player != null && (player == Lover1 || player == Lover2);
            }

            // Méthode pour réinitialiser et recharger les amoureux
            public static void clearAndReload() {
                Lover1 = null;
                Lover2 = null;
                notAckedExiledIsLover = false;
                bothDie = CustomOptionHolder.modifierLoverBothDie.getBool(); // Chargement de l'option si les deux meurent
                enableChat = CustomOptionHolder.modifierLoverEnableChat.getBool(); // Chargement de l'option de chat
            }

            // Méthode pour obtenir le partenaire d'un joueur
            public static PlayerControl getPartner(this PlayerControl player) {
                if (player == null)
                    throw new ArgumentNullException(nameof(player));
                return player == Lover1 ? Lover2 : player == Lover2 ? Lover1 : null;
            }
        }
    }
}
