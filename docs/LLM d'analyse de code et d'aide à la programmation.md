
## But initial

Le but du projet est de faire un bot IA qui va aider à l'analyse de code afin d'aider l'utilisateur à trouver des solutions, de meilleurs façon de faire certains codes ou simplement de débugger l'utilisateur. L'idée initiale est que l'IA soit local afin d'aider à la distribution et l'utilisation, sans demander nécessairement une connexion Internet immédiate et un travail plus focalisé sur la tâche.

La raison principale pour faire le projet est surtout pour me forcer à sortir de ma zone de confort, surtout avec mon aversion générale à l'IA générative. Il s'agit aussi d'un projet que, si il est bien implémenté, pourrait m'être utile dans le futur pour aider à plusieurs projets.

## SLM vs LLM

Comme le but est que l'IA soit *self-hosted*, il faut au moins savoir qu'est-ce qui sera utilisé lors du projet. Un des premier point qui m'est venu en tête est la taille du LLM, ce qui posera problème plus loin en terme de ressource requise pour le fonctionnement et l'accessibilité, ce qui me pousse à voir les options possibles.

Les LLMs sont beaucoup plus généralisé. Ils sont généralement utilisé pour des tâches plus communes ou moins spécifiques. Ils peuvent faire presque tout ce qu'on leur demande grâce à leur énorme quantité de données, d'entrainement et de ressources. 

Les SLMs (Small Language Model), de leur côté, sont beaucoup plus petit et concentré sur une tâche ou un concept spécifique. Ils doivent être entrainé sur des données concernant ce qu'on veut faire et sont beaucoup moins général que les LLMs standard. Ils sont beaucoup plus utilisé dans des scénarios où une connaissance générale n'est pas requise, que se soit en tant que support à la clientèle ou pour, similairement au projet ici, aider à acquérir de l'information sur un sujet spécifique.

Dans le cadre du projet, je pense qu'un SLM va être beaucoup plus adapté au but, dû à la taille et la spécificité de ce dernier. Maintenant, il faudrait savoir quel genre de modèle il y a, si ils sont disponibles et comment ils performent.

## Les Modèles

Il existe plusieurs modèles autre que les gros LLMs que l'on entend constamment parler, tel que Chat-GPT et Gemini, surtout lorsqu'on se penche plus sur leurs petits cousins. Beaucoup de modèles on été fait spécifiquement pour être plus lightweight et varié. Il existe plusieurs listes en ligne, surtout de la communauté HuggingFace, qui les énumère.

Ces agents généralement utilisent des modèles préexistant avec de l'entrainement pré-fait. Dans le cadre du projet, il serait sage d'utiliser un de ses modèles afin de sauver du temps et des ressources sur l'installation et la préparation de l'agent afin de pouvoir s'occuper plus concrètement sur le lien principal qui est de permettre la lecture de fichier en tant qu'input. Il reste à savoir les points positifs et négatifs de ses agents.

#### Phi-4-mini

Phi-4 est un modèle par Microsoft qui comporte deux versions : mini et multimodal. Le model Phi-4-mini comporte 3.8 milliards de paramètres et est entrainé sur des données obtenu par le web ou des données synthétiques. Bien que la version multimodal possède des capacités visuelles et audio, dans le cadre du projet, la version mini semble plus appropriée. Elle possède également une bonne performance dans plusieurs langues, mais n'est pas spécialisé en programmation et est encore assez générale, quoique les SLM sont facilement modifiable pour être spécialisé. C'est une option intéressante, mais je pense qu'il pourrait y avoir mieux.

#### Qwen3-Coder-Next

Qwen3 est un modèle qui est basée sur OpenAI et est pré-entrainé pour plusieurs scénario. Il est segmenté en plusieurs variantes et tailles, une d'entre elle étant Qwen3-Coder-Next. Ce dernier comporte 3 milliards de paramètres actifs et 80 milliards de paramètres totaux. Comme le nom indique, cette version est spécialisé pour le codage, capable d'analyse et de génération de code. La quantité de paramètres semble énorme, et ce sera quelques chose à garder en tête, mais la spécialisation ici semble très tentant et pourrait potentiellement être le modèle utilisé pour le projet. D'autres versions sont disponible, tel que Qwen2.5-Coder 14B, mais, pour l'instant, je ne vais retenir que Qwen3-Coder-Next.

#### Cogito

Cogito v1 est un modèle par Deep Cogito. Quoiqu'un LLM général, il est optimisé pour le codage, les sciences et technologies et l'aide générale. D'après eux, Cogito serait plus efficace que d'autres modèles plus entrainé. Bien que je ne suis pas en position de juger si cette information est vraie, Cogito a plusieurs versions qui représentent la quantité des paramètres, allant de 3 milliards à 70 milliards. Dans le cadre du projet, des versions 3B et 8B vont être utilisé pour les comparaisons.

Ses 3 modèles vont être ceux que je vais explorer dans le cadre du projet avant de choisir le principal, à moins que je décide de permettre de changer de modèle afin d'avoir plusieurs opinions.


## Les Ressources Demandés

Un des points principal qui m'inquiète pour ce projet est la quantité de ressources requise pour faire fonctionné un petit LLM (ou SLM) localement. Les LLMs demandent une quantité énorme de mémoire afin de pouvoir calculer se qu'ils doivent dire, allant dans la centaine de Go pour les plus gros LLMs, et les LLMs commerciaux prennent plusieurs fois cette quantité de mémoire (dû au fait qu'ils ont plusieurs personnes les utilisant à la fois et à quel point ils sont polyvalent).

Ce qui est requis pour faire fonctionner un LLM de façon local dépend principalement du nombre de paramètres et de la quantité d'entrainement du modèle. En général, un modèle ayant 3 milliards de paramètres prendra entre 2 et 6 Go de mémoire vive. La quantité de RAM et de VRAM requise dépend aussi du format utilisé, avec le format FP16 généralement utilisant plus de mémoire.

Pour phi-4-mini, il est recommandé d'avoir entre 3 et 9 Go de VRAM, selon si le format utilisé est INT4 jusqu'au format FP16. Il est généralement recommandé d'avoir au minimum une RTX 3060 Ti pour un bon fonctionnement, mais je pense que ce n'est pas un minimum strict.

Pour Qwen3-Coder-Next, quoique le modèle n'a que 3 milliards de paramètres actifs, les 80 milliards de paramètres inactifs augmentent grandement la quantité de mémoire requise, à un minimum de RAM et VRAM requise d'environ 20 Go, et 45 Go pour des versions plus générales. Il est possible de le faire fonctionner sur mes plus petites cartes, mais pour une utilisation générale, je ne pense pas que ce modèle soit compatible. Pour un projet personnel, par contre, si les ressources sont disponibles à l'usager, ce modèle devrait s'avéré très performant.

Du côté de Cogito, la grande variance entre les tailles de modèle disponible nous permet d'avoir un meilleur contrôle sur ce qu'on a besoin. Le modèle 3B semble prendre entre 2 et 7 Go de RAM, quoique le 8B ne devrait pas être trop loin en terme de taille.

Pour simplifier toute l'opération, Cogito v1 3B va être utilisé, mais je vais quand même regarder pour la version 8B. Il est également disponible directement sur Ollama, qui va être le moteur sous-jacent.

## Ollama

Ollama est un moteur pour LLM local qui permet la discussion avec, la pensée de et l'utilisation de celui-ci. Quoique Ollama supporte la lecture de fichier, je ne pense pas qu'elle supporte nativement la lecture d'un dossier complet, donc je planifie utiliser une application WPF .NET en C# afin de pouvoir implémenter une fonction pour envoyer des fichiers au LLM pour qu'il puisse les lire.

Il existe des librairies pour faire la connexion entre une interface et Ollama, puisque Ollama dispose d'un REST API. Le but sera de configurer une fenêtre utilisant les classes disponibles pour pouvoir faire office de fenêtre de discussion, essentiellement remplaçant l'interface de Ollama pour la mienne, et de rajouter la fonction de lire des fichiers via mon application. Ollama ne possède pas l'option de choisir un dossier ou sous-dossier à lire, et donc il sera mon travail (et la valeur ajouté) d'ajouter cette fonction.

Cogito demeure un IA général, malgré le fait qu'il soit "optimisé pour le codage". Il va falloir le spécialiser afin qu'il reste dans le contexte d'un aide pour la programmation.

## MCP

Un MCP, ou "Model Context Protocol", est une façon de lier l'agent à une interface de manière à personnaliser ce que le modèle à accès et concentrer ce que le LLM puisse faire. Il s'agirait d'une étape cruciale dans ce scénario puisque sinon, le LLM qui est lié à l'application n'est que celui plus général, ce qui pourrait entrainer un plus grand risque d'hallucination et un moins bon service.


Bien que faire son propre MCP semble intéressant, la complexité d'un tel pourrait causer problème lors du développement, surtout qu'il y a déjà beaucoup de MCP basé sur la programmation qui sont disponible et qui peuvent être utilisé sur le projet. Un de ces MCP est Context7. Context7 permet aux agents d'aller chercher directement la documentation requise pour le code demander. Cet outil semble parfait pour la situation présente, et quoiqu'il serait probablement plus sage d'ajouter plus d'outils à l'arsenal, je pense que la simple inclusion de Context7 sera suffisante pour avoir un meilleur projet. 


# Sources

_Large language models (LLMs) vs Small language models (SLMs)_. (2024). Redhat.com. https://www.redhat.com/en/topics/ai/llm-vs-slm

Johnson, J. (2025, February 25). _Small Language Models (SLM): A Comprehensive Overview_. Huggingface.co. https://huggingface.co/blog/jjokah/small-language-model

_The Best Open-Source Small Language Models (SLMs) in 2026_. (2019). Bentoml.com. https://www.bentoml.com/blog/the-best-open-source-small-language-models

_microsoft/Phi-4-mini-instruct · Hugging Face_. (2025, May). Huggingface.co. https://huggingface.co/microsoft/Phi-4-mini-instruct

Chen, W. (2025, February 26). _Empowering innovation: The next generation of the Phi family | Microsoft Azure Blog_. Microsoft Azure Blog. https://azure.microsoft.com/en-us/blog/empowering-innovation-the-next-generation-of-the-phi-family/

Microsoft, Abouelenin, A., Ashfaq, A., Atkinson, A., Awadalla, H., Bach, N., Bao, J., Benhaim, A., Cai, M., Chaudhary, V., Chen, C., Chen, D., Chen, D., Chen, J., Chen, W., Chen, Y.-C., Chen, Y., Dai, Q., & Dai, X. (2025). _Phi-4-Mini Technical Report: Compact yet Powerful Multimodal Language Models via Mixture-of-LoRAs_. ArXiv.org. https://arxiv.org/pdf/2503.01743

_Qwen/Qwen3-Coder-Next · Hugging Face_. (2026, February 3). Huggingface.co. https://huggingface.co/Qwen/Qwen3-Coder-Next

QwenLM. (2025). _GitHub - QwenLM/Qwen3-Coder: Qwen3-Coder is the code version of Qwen3, the large language model series developed by Qwen team, Alibaba Cloud._ GitHub. https://github.com/QwenLM/Qwen3-Coder

_Introducing Cogito Preview_. (2025). Deepcogito.com. https://www.deepcogito.com/research/cogito-v1-preview

_Ollama_. (2025). Ollama. https://ollama.com/library/cogito

LocalLLM.in. (2025, August 14). _How to Run a Local LLM: A Comprehensive Guide for 2025_. Localllm.in; LocalLLM.in. https://localllm.in/blog/how-to-run-local-llm-guide-2025

_Local LLM Hardware Requirements in 2026 | AI Hub_. (2026). Overchat.ai. https://overchat.ai/ai-hub/llm-hardware-requirements

Pinggy. (2025, September 8). _How to Self-Host Any LLM – Step by Step Guide_. Pinggy Blog. https://pinggy.io/blog/how_to_self_host_any_llm_step_by_step_guide/

_Phi-4-Mini: Specifications and GPU VRAM Requirements_. (2025). Apxml.com. https://apxml.com/models/phi-4-mini

Red Stapler. (2026, February 25). _I ran 80B model on 16GB GPU - It’s surprisingly good! (Qwen 3 Coder Next Review)_. YouTube. https://www.youtube.com/watch?v=_UEvlmNx0cs

_unsloth/Qwen3-Coder-Next-GGUF · Hugging Face_. (2026, May 18). Huggingface.co. https://huggingface.co/unsloth/Qwen3-Coder-Next-GGUF

_cortexso/cogito-v1 · Hugging Face_. (2026). Huggingface.co. https://huggingface.co/cortexso/cogito-v1

Ollama. (n.d.). _Ollama_. Ollama.com. https://ollama.com/

_Ollama_. (2025). Ollama. https://ollama.com/blog/new-app

_Introduction - Ollama_. (2025). Ollama.com; Ollama. https://docs.ollama.com/api/introduction

Aspire. (2026). _Connect to Ollama_. Aspire. https://aspire.dev/integrations/ai/ollama/ollama-connect/

_Getting started | OllamaSharp_. (2026). Github.io. https://awaescher.github.io/OllamaSharp/docs/getting-started.html

*‌Introduction - Model Context Protocol*. (2025). Modelcontextprotocol.io; Model Context Protocol. https://modelcontextprotocol.io/docs/getting-started/intro

alexwolfmsft. (2025, November 22). _Get started with .NET AI and MCP - .NET_. Microsoft.com. https://learn.microsoft.com/en-us/dotnet/ai/get-started-mcp

KodeKloud. (2025, July 21). _MCP Tutorial: Build Your First MCP Server and Client from Scratch (Free Labs)_. YouTube. https://www.youtube.com/watch?v=RhTiAOGwbYE

_6 Must-Have MCP Servers (and How to Use Them) | Docker_. (2026, January 7). Docker. https://www.docker.com/blog/top-mcp-servers-2025/

_Context7 MCP Server | Docker MCP Catalog_. (2026). Docker.com. https://hub.docker.com/mcp/server/context7/overview?_gl=1

upstash. (2026, February 23). _GitHub - upstash/context7: Context7 MCP Server -- Up-to-date code documentation for LLMs and AI code editors_. GitHub. https://github.com/upstash/context7

‌