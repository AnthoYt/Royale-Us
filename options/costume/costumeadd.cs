using System;
using System.IO;
using UnityEngine;

public class CostumeLoader : MonoBehaviour
{
    // Le dossier contenant les costumes personnalisés
    public string costumeFolder = "Costume";
    
    // Liste des costumes chargés
    private string[] costumeFiles;
    
    void Start()
    {
        // Vérifier si le dossier existe
        if (Directory.Exists(costumeFolder))
        {
            // Charger tous les fichiers PNG du dossier
            costumeFiles = Directory.GetFiles(costumeFolder, "*.png");

            if (costumeFiles.Length > 0)
            {
                Debug.Log("Costumes trouvés : " + costumeFiles.Length);
                LoadCostumes();
            }
            else
            {
                Debug.Log("Aucun costume trouvé dans le dossier.");
            }
        }
        else
        {
            Debug.LogError("Le dossier " + costumeFolder + " n'existe pas.");
        }
    }

    void LoadCostumes()
    {
        foreach (var costumePath in costumeFiles)
        {
            // Charger l'image du costume
            Texture2D costumeTexture = LoadTexture(costumePath);

            if (costumeTexture != null)
            {
                // Ici tu peux faire en sorte d'assigner cette texture à un personnage dans le jeu
                // Par exemple, assigner la texture à un modèle 3D ou une UI
                Debug.Log("Costume chargé : " + costumePath);
                ApplyCostumeToCharacter(costumeTexture);
            }
        }
    }

    Texture2D LoadTexture(string filePath)
    {
        // Charger le fichier image comme texture
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData)) // Retourne true si l'image est chargée avec succès
        {
            return texture;
        }
        else
        {
            Debug.LogError("Échec du chargement de l'image : " + filePath);
            return null;
        }
    }

    void ApplyCostumeToCharacter(Texture2D texture)
    {
        // Applique la texture au modèle du personnage
        // Tu devras modifier cette fonction pour l'adapter à ton jeu, ici on simule l'application de la texture.
        
        // Exemple : Assigner la texture à un SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Debug.Log("Costume appliqué au personnage !");
        }
        else
        {
            Debug.LogError("Aucun SpriteRenderer trouvé pour appliquer le costume.");
        }
    }
}
