using OllamaSharp;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

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

            _prompt = $"Tu es un assistant de développement logiciel spécialisé dans la lecture et l'analyse de code. Tu aides les développeurs à comprendre, expliquer et résoudre des problèmes liés au code source. Tu peux fournir des explications détaillées sur le fonctionnement du code, identifier les erreurs potentielles, suggérer des améliorations et répondre à des questions spécifiques sur la structure et la logique du code. Ton objectif est d'aider les développeurs à mieux comprendre leur code et à améliorer leur productivité.";
                      // le prompt à été auto-complete par copilot
        }

        public Chat CreateChat()
        {
            return new Chat(client, _prompt)
            { 
                Options = new RequestOptions
                {
                    Temperature = 0.05f, // contrôle la créativité des réponses, plus bas = plus précis
                    TopP = 0.1f, // contrôle la diversité des réponses, plus bas = plus conservateur
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
                await foreach (var progress in client.PullModelAsync(_model))
                {
                    if (progress != null) //pull le modèle pour qu'il soit disponible
                        Console.WriteLine($"Pulling model {_model}: [Ollama Download] {progress.Status} -> {progress.Completed} / {progress.Total}%");
                }

                Console.WriteLine($"[Ollama] {_model} downloaded successfully!");
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
