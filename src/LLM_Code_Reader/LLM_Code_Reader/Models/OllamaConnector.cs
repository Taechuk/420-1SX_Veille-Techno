using OllamaSharp;
using OllamaSharp.Models;
using OllamaSharp.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LLM_Code_Reader.Models
{
    class OllamaConnector
    {
        private Uri _uri;
        private OllamaApiClient client;
        private string _model;
        private string _prompt;
        private bool _available;

        public bool Available => _available;

        public string Model => _model;
        public OllamaApiClient Client => client;

        public OllamaConnector(string model)
        {
            _uri = new Uri($"http://localhost:11434");
            client = new OllamaApiClient(_uri);
            _model = model;
            _available = false;

            _prompt = $"Tu es un assistant de développement logiciel spécialisé dans la lecture et l'analyse de code. Tu aides les développeurs à comprendre, expliquer et résoudre des problèmes liés au code source. Tu peux fournir des explications détaillées sur le fonctionnement du code, identifier les erreurs potentielles, suggérer des améliorations et répondre à des questions spécifiques sur la structure et la logique du code. Ton objectif est d'aider les développeurs à mieux comprendre leur code et à améliorer leur productivité. Si une question qui n'a pas de connection avec l'informatique, la programmation ou le codage t'est demandé, rappelle à l'utilisateur ton but initial, sinon, tu n'as pas besoin de le rappeller. Lorsqu'un fichier t'es envoyé, analyse ce dernier pour des bugs ou des points à améliorer afin d'aider l'utilisateur. Tente de priorisé des questions ou fichiers plus récent, mais n'hésite pas à faire des références à des informations eu plus tôt.";
                      // le prompt à été auto-completé initiallement par copilot
        }

        public Chat CreateChat(string neededContext)
        {
            int ctx;
            if (neededContext == "strong")
                switch (_model) // switch pour au cas où plusieurs modèles ont besoin de valeurs plus spécifiques, présentement un if serait suffisant
                {
                    case "codellama":
                        ctx = 16384; //contexte plus petit puisque le modèle ne supporte pas 32768 tokens
                        break;
                    default:
                        ctx = 32768; //contexte plus grand pour les modèles qui le supportent
                        break;

                }
            else
                ctx = 4096;

            return new Chat(client, _prompt)
            { 
                Options = new RequestOptions
                {
                    Temperature = 0.05f, // contrôle la créativité des réponses, plus bas = plus précis
                    TopP = 0.1f, // contrôle la diversité des réponses, plus bas = plus conservateur
                    NumCtx = ctx, // nombre de tokens de contexte à utiliser pour la conversation
                }
            };
        }

        public async Task SetupModel()
        {
            _available = await IsPulled(_model);

            if (Available) //modèle déja présent localement, on le sélectionne pour l'utiliser
            {
                client.SelectedModel = _model;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Le modèle n'est pas présent sur la machine. Êtes-vous sur de vouloir l'installer? \n(Assurez-vous d'avoir assez d'espace et une connection stable.)", "Installer le modèle?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await foreach (var progress in client.PullModelAsync(_model))
                    {
                        if (progress != null) //pull le modèle pour qu'il soit disponible
                            Console.WriteLine($"Pulling model {_model}: [Ollama Download] {progress.Status} -> {progress.Completed} / {progress.Total}%"); //à trouver comment afficher le progrès
                    }

                    Console.WriteLine($"[Ollama] {_model} downloaded successfully!");

                    _available = await IsPulled(_model); //vérification que le modèle est bien présent après le téléchargement
                    client.SelectedModel = _model;
                }
            }
        }

        public async Task StopModel()
        {
            if (Available)
            {   
                var request = new ChatRequest { Model = _model, KeepAlive = "0" }; //KeepAlive = 0 pour arrêter le modèle après la requete
                await foreach (var response in client.ChatAsync(request))
                {
                    if (response != null)
                    Console.WriteLine($"Stopping model {_model}: [Ollama Stop]");
                }
                
            }
        }


        public async Task<bool> IsPulled(string model)
        {
            try
            {
                var models = await client.ListLocalModelsAsync(); //collection de la liste de modèles locaux disponibles

                return models.Any(m => // vérification du modèle spécifié dans la liste des modèles locaux
                m.Name.Equals(model, StringComparison.OrdinalIgnoreCase) ||     
                m.Name.Equals($"{model}:latest", StringComparison.OrdinalIgnoreCase)
                );

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error connecting to Ollama: {ex.Message}"); //ollama n'est pas fonctionnel
                return false;
            }
        }

    }
}
