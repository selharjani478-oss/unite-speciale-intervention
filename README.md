# Unité spéciale : Intervention

Prototype Unity 3D jouable en français, généré entièrement par scripts et formes intégrées — aucun asset externe requis.

## Version recommandée
Unity 6 LTS (6000.0.x), template **3D Core**. Aucun package externe. Les scripts utilisent l’ancienne API `Input`; dans **Edit > Project Settings > Player > Other Settings > Active Input Handling**, choisir **Both** ou **Input Manager (Old)**, puis redémarrer l’éditeur si demandé.

## Installation débutant
1. Dans Unity Hub, créez un projet **3D Core**. Créez `Assets/Scripts`.
2. Téléchargez/copiez les fichiers de `Assets/Scripts/` du dépôt dans ce dossier (le dossier `Assets` est déjà inclus).
3. Ouvrez la scène vide `SampleScene`. Ajoutez un objet vide `Bootstrap`, attachez-lui `GameBootstrap.cs`.
4. Enregistrez la scène sous `Assets/Scenes/Intervention.unity`, créez le dossier si nécessaire. Dans **File > Build Profiles** (ou Build Settings), ajoutez cette scène et placez-la en premier.
5. Appuyez sur Play. Le jeu construit la banque et son interface automatiquement.

Le dépôt contient les scripts source, pas un projet Unity compilé ni des fichiers de scène binaires. Unity génère le niveau au démarrage. Le code n’a pas été exécuté dans Unity dans cet environnement : compilation et fonctionnement restent à vérifier après import.

## Contrôles
- WASD : déplacement ; souris : regarder ; Échap : libérer/reprendre le curseur.
- E : interagir / secourir un civil / évacuer un civil à la zone de sortie.
- F : ouvrir la négociation quand le responsable de crise est proche ; 1, 2, 3 pour choisir.
- Q : parcourir les ordres d’équipe (Suivre, Rester, Accompagner le civil le plus proche).

## Mission
Explorez la banque, approchez les civils et pressez E pour les escorter. La sortie est le marqueur vert près de l’entrée : pressez E à proximité pour les mettre à l’abri. Parlez au responsable de crise avec F; les choix influencent le score et l’issue. La mission se termine quand tous les civils sont évacués ou à la fin du délai. Une issue non violente et la protection de tous les civils donnent la meilleure évaluation. R : recommencer depuis l’écran de bilan.

## Arborescence
`Assets/Scripts/` contient le bootstrap, le contrôleur, la mission, l’interface, les civils et l’équipe. La géométrie, les matériaux et les éclairages sont fabriqués en code avec des primitives Unity.

## Build PC
Dans Unity, vérifiez la scène `Intervention` dans **Build Profiles**, sélectionnez Windows/Mac/Linux puis **Build**. Aucun asset ou service payant requis.
