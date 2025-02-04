import tkinter as tk
from tkinter import messagebox

# Fonction pour créer une partie avec le nombre de joueurs choisi
def create_game():
    selected_players = player_count.get()  # Récupérer la valeur sélectionnée dans le menu déroulant
    if selected_players:
        # Afficher une boîte de message pour confirmer la création de la partie
        messagebox.showinfo("Création de la partie", f"La partie a été créée avec {selected_players} joueurs !")
    else:
        messagebox.showerror("Erreur", "Veuillez sélectionner le nombre de joueurs.")

# Créer la fenêtre principale
root = tk.Tk()
root.title("Création de Partie - Among Us")

# Ajouter un label pour expliquer
label = tk.Label(root, text="Sélectionnez le nombre de joueurs (15 à 20)", font=("Arial", 14))
label.pack(pady=20)

# Créer un menu déroulant pour choisir le nombre de joueurs
player_count = tk.StringVar()
player_count.set("15")  # Valeur par défaut

# Liste des options de joueurs possibles (de 15 à 20)
player_options = ["15", "16", "17", "18", "19", "20"]

dropdown = tk.OptionMenu(root, player_count, *player_options)
dropdown.pack(pady=20)

# Ajouter un bouton pour créer la partie
button = tk.Button(root, text="Créer la partie", command=create_game, font=("Arial", 14))
button.pack(pady=20)

# Lancer l'interface
root.mainloop()
