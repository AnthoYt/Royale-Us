using System;
using System.IO;
using UnityEngine;

public class CostumeLoader : MonoBehaviour {
    public string costumeFolder = "Costume";
    private string[] costumeFiles;

    void Start() {
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

    void LoadCostumes() {
        foreach (var costumePath in costumeFiles) {
            Texture2D costumeTexture = LoadTexture(costumePath);
            if (costumeTexture != null) {
                Debug.Log("Costume chargé : " + costumePath);
                ApplyCostumeToCharacter(costumeTexture);
            }
        }
    }

    Texture2D LoadTexture(string filePath) {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(fileData)) {
            return texture;
        } else {
            Debug.LogError("Échec du chargement de l'image : " + filePath);
            return null;
        }
    }

    void ApplyCostumeToCharacter(Texture2D texture) {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Debug.Log("Costume appliqué au personnage !");
        } else {
            Debug.LogError("Aucun SpriteRenderer trouvé pour appliquer le costume.");
        }
    }
}
