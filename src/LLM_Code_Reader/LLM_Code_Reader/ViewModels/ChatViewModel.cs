using LLM_Code_Reader.Models;
using LLM_Code_Reader.ViewModels.Commands;
using Microsoft.Extensions.AI;
using Microsoft.Win32;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;

namespace LLM_Code_Reader.ViewModels
{
    class ChatViewModel : BaseViewModel
    {
        private string _model = "cogito:3b";
        private string _content;

        private List<string> _modelList;

        public ObservableCollection<Message> Messages { get; set; }
        public List<string> ModelList { get => _modelList; set { _modelList = value; OnPropertyChanged(nameof(ModelList)); } }

        public Chat? Conversation { get; set; }
        public OllamaConnector? Connector { get; set; }

        public string SelectedModel { get => _model; set { _model = value; OnPropertyChanged(nameof(SelectedModel)); } }

        public string Content { get => _content; set { _content = value; OnPropertyChanged(nameof(Content)); } }

        public bool Thinking { get; set; }

        public ChatViewModel()
        {
            ModelList = ["cogito:3b", "cogito:8b", "phi4-mini", "codellama", "qwen2.5-coder", "qwen3.5", "qwen3.6"];
            Messages = new ObservableCollection<Message>();
            Messages.Add(new Message("Currently Offline", "System"));
            Content = string.Empty;
            Thinking = false;

            CommandeCreateConnector = new AsyncCommand(CreateConnector, null);
            CommandeSendMessage = new AsyncCommand(SendMessage, (obj) => { return Conversation != null && !Thinking && !Content.IsWhiteSpace(); });
            CommandeSendFile = new AsyncCommand(SendFile, (obj) => { return Conversation != null && !Thinking; });
            CommandeStopChat = new AsyncCommand(StopChat, (obj) => { return Conversation != null; });
        }

        public async Task CreateConnector(object? obj)
        {
            Connector = new OllamaConnector(_model);
            Messages.Add(new Message($"Model {_model} is currently loading. This can take a while.", "System"));

            await Connector.SetupModel();


            if (Connector.Available)
            {
                Messages.Clear();
                Messages.Add(new Message($"Model {_model} is ready to use.", "System"));

                Conversation = Connector.CreateChat();

                Messages.Add(new Message("Thinking...", "System")); //action en cours
                var response = string.Empty;
                await foreach (var token in Conversation.SendAsync("Début de la conversation, dis bonjour et présente-toi.")) //attente de réponse
                {
                    response += token; //tout mettre en un message vu que le token est envoyé sur plusieurs packets
                }
                Messages.RemoveAt(Messages.Count - 1); //enlever le message du système
                Messages.Add(new Message(response, Connector.Model)); //message initial du bot
            }
            else
            {
                Messages.Clear();
                Messages.Add(new Message($"Model {_model} is not available and could not be pulled. Verify that Ollama is installed and that you have an internet connection if this is the first time the model is loaded.", "System"));
            }
        }

        public async Task SendMessage(object? obj)
        {
            if (Conversation == null) return;
            Messages.Add(new Message(Content, "User"));
            string sentContent = Content; //save l'input pour l'envoyer
            Content = string.Empty; //clear l'input
            Thinking = true; //empeche d'envoyer plusieurs messages
            Messages.Add(new Message("Thinking...", "System")); //action en cours

            string response = string.Empty;

            using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5))) //timeout de 3 minutes pour éviter les réponses infinies
            {
                try
                {
                    await foreach (var token in Conversation.SendAsync(sentContent, cts.Token)) //attente de réponse
                    {
                        response += token; //tout mettre en un message vu que le token est envoyé sur plusieurs packets
                    }
                }
                catch (OperationCanceledException)
                {
                    Messages.RemoveAt(Messages.Count - 1); //enlever le message du système
                    Messages.Add(new Message("Le temps de réponse est échu, veuillez réessayer.", "System")); //message du système
                }
                catch (Exception ex)
                {
                    Messages.RemoveAt(Messages.Count - 1); //enlever le message du système
                    Messages.Add(new Message("Une erreur est survenue.", "System")); //message du système
                }
            }

            Messages.RemoveAt(Messages.Count - 1); //enlever le message du système
            Messages.Add(new Message(response, Connector.Model)); //message du bot

            Thinking = false; //le user peut continuer

        }

        public async Task SendFile(object? obj)
        {
            if (Conversation == null) return;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                string fileContent = await File.ReadAllTextAsync(openFileDialog.FileName);

                Messages.Add(new Message($"Fichier {openFileDialog.SafeFileName} envoyé.", "User"));

                string sentContent = $"Voici le contenu du fichier {openFileDialog.SafeFileName} :\n{fileContent}"; //contenu du fichier à envoyer
                Thinking = true; //empeche d'envoyer plusieurs messages
                Messages.Add(new Message("Thinking...", "System")); //action en cours
                string response = string.Empty;
                using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3))) //timeout de 3 minutes pour éviter les réponses infinies
                {
                    try
                    {
                        await foreach (var token in Conversation.SendAsync(sentContent, cts.Token)) //attente de réponse
                        {
                            response += token; //tout mettre en un message vu que le token est envoyé sur plusieurs packets
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Messages.RemoveAt(Messages.Count - 1); //enlever le message du système
                        Messages.Add(new Message("Le temps de réponse est échu, veuillez réessayer.", "System")); //message du système
                    }
                    catch (Exception ex)
                    {
                        Messages.RemoveAt(Messages.Count - 1); //enlever le message du système
                        Messages.Add(new Message("Une erreur est survenue.", "System")); //message du système
                    }
                }
                Messages.RemoveAt(Messages.Count - 1); //enlever le message du système
                Messages.Add(new Message(response, Connector.Model)); //message du bot
                Thinking = false; //le user peut continuer
            }
        }

        public async Task StopChat(object? obj)
        {
            if (Conversation == null) return;
            MessageBoxResult result = MessageBox.Show("Êtes-vous sur de vouloir arrêter le modèle?", "Arrêter le modèle?", MessageBoxButton.YesNo, MessageBoxImage.Warning); //confirmation
            if (result == MessageBoxResult.Yes)
            {
                await Connector.StopModel(); //arrêt du modèle
                Messages.Add(new Message($"Model {Connector.Model} has been stopped.", "System"));
                Conversation = null; //chat terminé
                Thinking = false;
            }
        }

        public AsyncCommand CommandeCreateConnector { get; set; }
        public AsyncCommand CommandeSendMessage { get; set; }
        public AsyncCommand CommandeSendFile { get; set; }
        public AsyncCommand CommandeStopChat { get; set; }
    }
}
