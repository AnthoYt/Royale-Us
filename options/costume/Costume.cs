https://github.com/astra1dev/AUnlocker/releases (Source)

using System;
using System.IO;
using UnityEngine;

public class CostumeLoader : MonoBehaviour {
    public string costumeFolder = "Costume"; // Nom du dossier des costumes
    private string[] costumeFiles;  // Tableau des fichiers de costumes
    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogError("Aucun SpriteRenderer trouvé !");
            return;
        }

        if (Directory.Exists(costumeFolder)) {
            costumeFiles = Directory.GetFiles(costumeFolder, "*.png");
            if (costumeFiles.Length > 0) {
                Debug.Log("Costumes trouvés : " + costumeFiles.Length);
                LoadCostumes();
            } else {
                Debug.Log("Aucun costume trouvé dans le dossier.");
            }
        } else {
            Debug.LogError("Le dossier " + costumeFolder + " n'existe pas.");
        }
    }

    // Charge tous les costumes dans le dossier
    void LoadCostumes() {
        foreach (var costumePath in costumeFiles) {
            Texture2D costumeTexture = LoadTexture(costumePath);
            if (costumeTexture != null) {
                Debug.Log("Costume chargé : " + costumePath);
                ApplyCostumeToCharacter(costumeTexture);
            }
        }
    }

    // Charge une texture à partir d'un fichier
    Texture2D LoadTexture(string filePath) {
        try {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(fileData)) {
                return texture;
            } else {
                Debug.LogError("Échec du chargement de l'image : " + filePath);
                return null;
            }
        } catch (Exception e) {
            Debug.LogError("Erreur lors du chargement de la texture : " + e.Message);
            return null;
        }
    }

    // Applique un costume à l'objet
    void ApplyCostumeToCharacter(Texture2D texture) {
        if (spriteRenderer != null) {
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Debug.Log("Costume appliqué au personnage !");
        } else {
            Debug.LogError("Aucun SpriteRenderer trouvé pour appliquer le costume.");
        }
    }

    // Permet de changer de costume par index (si plusieurs costumes sont chargés)
    public void ChangeCostume(int index) {
        if (index >= 0 && index < costumeFiles.Length) {
            Texture2D costumeTexture = LoadTexture(costumeFiles[index]);
            if (costumeTexture != null) {
                ApplyCostumeToCharacter(costumeTexture);
            }
        } else {
            Debug.LogError("Index de costume invalide.");
        }
    }

    // Méthode pour changer un costume aléatoirement parmi ceux disponibles
    public void ChangeCostumeRandom() {
        if (costumeFiles.Length > 0) {
            int randomIndex = UnityEngine.Random.Range(0, costumeFiles.Length);
            ChangeCostume(randomIndex);
        } else {
            Debug.LogError("Aucun costume disponible pour un changement aléatoire.");
        }
    }
}
