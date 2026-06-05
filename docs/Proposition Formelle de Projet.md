# Veille Techno - Proposition formelle de projet
#### Larry-Félix Paquette  
  
  



## Préface



  

Je ne suis pas quelqu’un qui apprécie les avancées récentes de l’IA. Bien que je puisse voir les utilités possibles grâce à la technologie, je ne suis pas quelqu’un qui pense vraiment à intégrer des outils d’IA dans les projets. Il y a probablement des utilités que je ne peux pas m’imaginer dû à ma “fermeture” envers cette technologie.  
  
Je ne nie pas qu’elle puisse être extrêmement utile dans certains domaines avec de l'entraînement spécialisé, mais la part que je me distancie le plus est l’utilisation de l’IA générative. Avoir accès à la création d’images, de musique et de beaucoup plus via l’entraînement sur du contenu humain (souvent sans leur consentement) me répugne et beaucoup d’autres personnes autour de moi. Les LLMs sont aussi dans cette catégorie, quoique je puisse plus comprendre leur existence et leur utilité. Je ne comprends pas exactement comment l’IA pourrait aider lors du développement d’applications si ce n’est pas pour aider à chercher et comprendre certaines choses, ce que des LLMs sont surprenamment bon à faire.  
  
Le type d’IA que j’ai vu le plus souvent avant était des IA dans le type de neural-network, qui permet d’en avoir une hyperspécialisée dans certains domaines, et pourrait techniquement être implémenté dans presque n’importe quel domaine (comme par exemple cette vidéo sur une IA qui apprend à jouer à Trackmania [cette vidéo sur une IA qui apprend à jouer à Trackmania](https://www.youtube.com/watch?v=Dw3BZ6O_8LY)). Si j’étais pour faire un travail sérieux avec l’IA, ce type serait probablement ce que je préférerais, mais dans le cadre du cours, un tel IA serait probablement trop compliqué à accomplir.

  

Sur ce, je vais faire de mon mieux pour quand même avoir des idées utilisant différents types d’IA.  
  
### Idée 1  
  

[character.ai](http://character.ai) est un site où des personnes peuvent parler avec un personnage fictif de leur choix où le personnage est joué par un LLM entraîné sur ses personnages. Il s’agit d’un outil souvent utilisé par certaines personnes cherchant à roleplay avec ses personnages ou simplement pour s’imaginer de parler avec les personnages, avec leurs personnalités semi-intact (rien n’est parfait). Le projet serait d'intégrer [character.ai](http://character.ai) dans un bot discord afin de permettre aux gens d’utiliser une interface différente du site et de parfois permettre aux gens d'interagir à plusieurs (ou juste de voir) dans la discussion. Il existe plusieurs API existant pour le site, quoique tous non-officiel tel que node_character.ai ([node_character.ai](https://github.com/realcoloride/node_characterai/tree/2.0)), donc il serait probablement faisable de faire un lien entre l’API et le bot discord afin de permettre le chat. Le minimum fonctionnel serait d’avoir un bot qui peut changer d'apparence et qui peut suivre une conversation, ce qui semble être relativement simple selon à quel point l’API non-officiel fonctionne. La raison pour laquelle ce projet est ma première idée est qu’elle a été lancée un peu à la blague par une de mes amis, qui fait du roleplay. J’ai quelques amis dont ce projet pourrait les intéresser et, bien que ce soit un travail qui ne va probablement pas rester longtemps en utilisation, le proof of concept serait néanmoins intéressant pour des projets futurs. Bien que ce projet ne corresponde pas exactement aux critères requis, je pense que le projet est intéressant.

  

### Idée 2

  

Installation d’un bot/IA local qui analyse les repos des usagers dans le but de trouver des bugs ou pour aider dans le développement d’applications de tout genre. Le LLM sera entraîné sur spécifiquement les concepts de programmations de plusieurs langages afin d’être le plus efficace possible et le tout sera hébergé sur la machine de l’utilisateur. L’idée principale est d’avoir un outil d’analyse général qui aidera à débugger sans des cas où le problème n’est pas très facile à trouver et est relié à un bug dans un fichier complètement à part de ce qui est travaillé. Le bout le plus dur d’après moi est d’installer un LLM et de l'entraîner comme je n’ai aucune expérience dans ce domaine, mais assumant que le setup n’est pas ultra compliqué, un IA qui est capable de trouver des bugs simples comme une erreur de frappe ou une boucle mal fermé quelque part dans un dossier serait déjà un bon point de départ.

  

### Idée 3

  

Création d’une IA de type neural-network afin de lui apprendre à faire un tour d’une map de Sonic Riders. Il s’agit d’un projet commun dans plusieurs jeux, comme l’IA de Trackmania mentionnée plus tôt, et j’ai toujours voulu tenter l’expérience et de voir un IA que j’ai concu s’améliorer dans un jeu que je joue. Sonic Riders est un jeu assez simple à contrôler, donc je ne pense pas que faire la liaison entre la machine et les contrôles soit difficile, surtout qu’utiliser un émulateur permet d’avoir accès aux adresses de mémoires pour aider à juger l’IA. La difficulté, comme dans la majorité des projets dans le genre, est d’ajuster comment elle sera récompensé afin de la pousser à améliorer son temps encore et encore. Bien sûr, je pense que ce projet est beaucoup plus personnel et ne s’applique, encore une fois, pas à l’attente requis au cours.

  
  

## Analyse des idées

  

Je pense que l’idée la plus intéressante à faire, et qui me pousserait le plus à sortir de ma zone de confort, serait probablement la deuxième idée.  
  
L’idée de character.ai , quoique assez pertinent et potentiellement intéressant pour plusieurs, compte plusieurs risques, surtout liés aux termes de services du site, mais aussi au manque de support officiel d’un API et reposant entièrement sur l’API de quelqu’un de non-affilié au site. Le projet demanderait aussi la création d’un bot Discord complet, ce qui ferait en sorte que le projet prenne beaucoup plus qu’une semaine à compléter.

L’idée de Sonic Riders, de son côté, est une idée vue et revue dans plusieurs autres jeux et n’est pas unique, et n’est pas exactement liée au thème émis étant donné la nature du projet. L’entraînement d’une IA de type neural-network n’est pas simple non-plus, et requiert beaucoup de temps et de détails, ce qui ne serait pas réaliste dans un timeframe d’une semaine. Même si le temps de création du projet n’était pas un problème, avoir un résultat concluant prendrait beaucoup de temps.

  

Le bot d’analyse de repo semble beaucoup plus utile et plus en lien avec le thème, surtout qu’elle serait potentiellement utile dans le futur. Un bot local étant capable d’aider dans le debugging serait très utile pour plusieurs dev, incluant moi-même. Ce serait aussi une nouvelle expérience, me forçant à vraiment travailler avec les nouvelles technologies liées à l’IA. J’ai vu plusieurs personnes en ligne parler d’installer leur propre LLM local et je pense qu’un tel projet est assez intéressant. Avoir à faire les deux côtés, installer un LLM et l'entraîner, et le programmer pour analyser un repo va être compliqué à faire entrer dans le temps accordé, mais je pense que ce serait faisable.

  
  

# Projet - IA d’Analyse de Projet d’Informatique

  

Le but général du projet sera de mettre en place une IA (ChatGPT ou Claude sont les deux IA que j’ai en tête présentement) et de l'entraîner sur des données relié à l’informatique afin de pouvoir analyser un fichier ou un dossier d’un projet d’informatique afin qu’il puisse nous aider à trouver des bugs ou peut-être même donner des directions sur comment progresser. Avoir un IA capable de lire des fichiers simples et de trouver une erreur simple serait déjà un bon succès, mais le meilleur cas possible serait une analyse de dossier complet, ce qui inclut potentiellement des logs selon la nature du langage, permettant une meilleure aide et un meilleur résultat, mais il s’agit du cas optimal et final dans le meilleur cas.

  

Un projet fonctionnel et “terminé” serait d’avoir un chat bot local qui peut prendre des questions ou un fichier et répondre d’une façon simple et correcte. Le cas demandé n’est pas nécessairement un cas complexe, comme je pense qu’avoir au moins une bonne réponse est un bon résultat pour la progression du projet.
___
  

Le procédurier du projet n’est probablement pas exactement correct selon la technologie, puisque je n’ai pas d’expérience avec cette technologie. Cependant, je vais quand même faire de mon mieux pour donner quelque chose qui semble sensé.

  

Les premiers jours seraient consacrés à l’installation initiale de l’IA, avec la décision de quel modèle sera utilisé. Cela mettrait en place la fondation du projet et est la pièce centrale autour duquel elle tournera.

  

Après ça, jour 3 (potentiellement 2 selon la vitesse de l’installation de l’IA) sera consacrée à l'entraînement de l’IA et la spécialiser. S’assurer que l’IA est bien entraînée est clé pour le bon fonctionnement du projet, donc je pense que cette étape étant séparée de l’installation initiale est valide.

Jours 4-5 seront consacrés à configurer l’IA à lire directement des fichiers textes, puisque la majorité des fichiers de code sont simplement du texte avec des extensions spéciales. Le but de l’étape est que l’IA soit capable de facilement lire les données et le texte, mais aussi qu’elle soit dans le mode correct, ne s’imaginant pas qu’un fichier C++ soit un fichier C. Ce côté semble être assez facile à éviter, simplement de lire l’extension de fichier, et l’IA pourrait définitivement trouver quel langage il s’agit juste au code, mais je pense que c’est un point nécessaire de ne pas devoir lui dire à chaque fois le langage demandé.

  

Jour 6-7 seront dédiées à ajouter la fonction de lire un dossier et leurs sous-dossiers afin de faire une analyse approfondie du tout. Il s’agit d’un bond drastique en terme d’implémentation, mais les multiples types de fichiers qui se communiquent entre eux (prenons par exemple un projet Godot utilisant du GDScript et du C#) augmente la complexité de la tâche mais est nécessaire pour avoir un bon produit. Bien sûr, le minimum requis pour que le projet soit fonctionnel ne sera pas ultra complexe, mais pouvoir lire plusieurs fichiers et les prendre comme un entier est demandé dans ce scénario afin de pouvoir analyser et trouver des erreurs potentielles.

  ___

Le résultat final sera une application ou un bot capable de recevoir un dossier et de donner une analyse simple sur les bugs ou erreurs trouvés, et donner une solution sur ce qui est à régler. J’ai probablement un ancien petit projet en Python que je pourrais utiliser afin de faire le test du fonctionnement, avec une fonction non-fonctionnelle qui pourrait faire office de test pour voir si il serait capable de trouver et régler le problème.




___
Aucun IA n'a été utilisé dans la rédaction de ce document.