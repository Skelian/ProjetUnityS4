# ProjetUnityS4

Bonjour et bienvenue sur la page du projet S4 du groupe 4B de l'Institut Universitaire d'Orsay. Pour ce projet s'inscrivant dans notre formation de 2ème année à l'IUT d'Orsay, nous sommes chargés de créer un Minecraft-like, c'est-à-dire un jeu reprenant les bases de Minecraft mais avec nos propres concepts !

## Equipe

* Chef de Projet: [Thomas Nomine](mailto:thomas.nomine@u-psud.fr)
* Chef de Projet Adjoint: [Arthur Chauveau](mailto:arthur.chauveau@u-psud.fr)
* [Alexis Aune](mailto:alexis.aune@u-psud.fr)
* [Camille Beaugendre](mailto:camille.beaugendre@u-psud.fr)
* [Guillaume Boutry](mailto:guillaume.boutry@u-psud.fr)
* [Dylan MS](https://dylanms.fr)
* [Florian Cezard](mailto:florian.cezard@u-psud.fr)
* [Allan Couderette](mailto:allan.couderette@u-psud.fr)
* [Ludovic Lecrocq](mailto:ludovic.locrocq@u-psud.fr)
* [Grégoire Lesniewski](mailto:gregoire.lesniewski@u-psud.fr)
* [Théo Machon](mailto:theo.machon@u-psud.fr)
* [Julien Quéant](mailto:julien.queant@u-psud.fr)
* [Issam Tebib](mailto:issam.tebib@u-psud.fr)



# Pré-requis

Vous avez besoin d'avoir GIT, Unity et GitHub for Unity


### GIT

##### Windows
[GIT WINDOWS](https://git-scm.com/download/win)
###### Si vous utilisez Chocolatey
`choco install git`

##### Mac
[GIT MAC](https://git-scm.com/download/mac)
###### Si vous utilisez HomeBrew
`brew install git`

### Unity

[Lien Unity](https://store.unity.com/download?ref=personal)

Suivez les instructions de l'installateur.

### GitHub for Unity

[Lien GitHub for Unity](https://github.com/github-for-unity/Unity/releases/download/v0.26.1-alpha/github-for-unity-0.26.1.3631-634ae54f.unitypackage)

(N'y touchez pas tout de suite)

## Mise en place du projet sur votre machine

### Télécharger le projet

Depuis la console, naviguez vers le dossier où vous voulez mettre le projet. 

Par exemple:
`F:/Projects/Unity/`

Puis entrez la commande:

`git clone https://github.com/Skelian/ProjetUnityS4.git`

Cette commande va télécharger la dernière version du projet depuis GitHub dans le dossier ProjetUnityS4, donc si vous avez la même racine que moi vous aurez une structure comme ceci:

`F:/Projects/Unity/ProjetUnityS4/`

### Importation dans Unity

Lancez Unity, puis choisissez open:
![Image indiquant open](https://image.ibb.co/cRDp66/Capture.png)

Ouvrez le dossier ProjetUnityS4 
(Si une pop-up apparait en disant mauvais numéro de version... Appuyez sur Ok, ça ne pose pas de problème)

Maintenant, double-cliquez sur le fichier github-for-unit.[...].unitypackage

Cela devrait vous ouvrir une nouvelle fenêtre, vous n'avez qu'à cliquer sur import (si vous êtes sur Windows vous pouvez décocher la ligne MAC et inversement si vous êtes sur MAC)
Une fois cela fait, vous aurez une nouvelle option dans la barre de menu en haut à gauche.

![Position du menu GitHub](https://image.ibb.co/iKoveR/Capture.png)

Sélectionnez-le, puis sélectionnez Authenticate. Entrez les vos identifiants GitHub (pseudo et mot de passe).

Ensuite, toujours dans le menu GitHub, sélectionnez l'option Show window, cela fera apparaître une nouvelle fenêtre dans l'éditeur.

Voila, GitHub for Unity est installé !

N'hésitez pas pour savoir si un nouveau commit a été fait de faire un Fetch dans l'onglet History. S'il y a eu de nouveaux commits, un nombre apparaîtra derrière pull du style pull (1).

Pour pouvoir commit, allez dans l'onglet Changes, sélectionnez les fichiers que vous voulez inclure dans le commit (les fichiers que vous avez modifiés), mettez un titre court dans commit summary, par exemple Ajout fichiers, Fix du script X...

Dans Commit Description, n'hésitez pas à détailler votre commit !

Ensuite cliquez sur Commit to [master].

Et enfin dans l'onglet History cliquez sur Push !

S'il y a eu des commits entre-temps, vous aurez un message d'erreur et vous devrez pull pour inclure les modifications avant de pouvoir push sur la branche.
# Vous avez tous les outils en mains pour collaborer sur le projet !

Pour me contacter: [Guillaume Boutry](mailto:guillaume.boutry@u-psud.fr)
