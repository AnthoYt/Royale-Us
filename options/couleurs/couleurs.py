import tkinter as tk
from tkinter import colorchooser

# Liste des couleurs personnalisées (au-delà de celles d'Among Us)
custom_colors = [
    "#ff6347", "#4682b4", "#32cd32", "#ff1493", "#ff8c00", "#8a2be2",
    "#adff2f", "#ffd700", "#40e0d0", "#8b0000", "#ff4500", "#c71585",
    "#7fff00", "#dc143c", "#ffb6c1", "#f0e68c", "#a52a2a", "#800080", "#00ff00"
]

# Fonction pour appliquer la couleur
def change_color(color_code):
    canvas.config(bg=color_code)
    label.config(text=f"Votre couleur : {color_code}", fg=color_code)

# Créer la fenêtre principale
root = tk.Tk()
root.title("Personnalisation de Couleur - Among Us")

# Ajouter un label pour les instructions
label = tk.Label(root, text="Sélectionnez une couleur personnalisée", font=("Arial", 14))
label.pack(pady=20)

# Canvas pour afficher la couleur choisie
canvas = tk.Canvas(root, width=300, height=300, bg="white")
canvas.pack(pady=20)

# Création des boutons pour les couleurs personnalisées
for color in custom_colors:
    button = tk.Button(root, bg=color, width=20, height=2, command=lambda color=color: change_color(color))
    button.pack(pady=2)

# Lancer l'interface
root.mainloop()
