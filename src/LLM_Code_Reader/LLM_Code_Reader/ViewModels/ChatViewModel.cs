using LLM_Code_Reader.Models;
using LLM_Code_Reader.ViewModels.Commands;
using Microsoft.Extensions.AI;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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
            ModelList = ["cogito:3b", "cogito:8b", "phi4-mini", "qwen3.5", "qwen3.6"];
            Messages = new ObservableCollection<Message>();
            Messages.Add(new Message("Currently Offline", "System"));
            Content = string.Empty;
            Thinking = false;

            CommandeCreateConnector = new AsyncCommand(CreateConnector, null);
            CommandeSendMessage = new AsyncCommand(SendMessage, (obj) => { return Conversation != null && !Thinking && !Content.IsWhiteSpace(); });
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
                await foreach (var token in Conversation.SendAsync("")) //attente de réponse
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
            await foreach(var token in Conversation.SendAsync(sentContent)) //attente de réponse
            {
                response += token; //tout mettre en un message vu que le token est envoyé sur plusieurs packets
            }
            Messages.RemoveAt(Messages.Count - 1); //enlever le message du système
            Messages.Add(new Message(response, Connector.Model)); //message du bot

            Thinking = false; //le user peut continuer

        }

        public AsyncCommand CommandeCreateConnector { get; set; }
        public AsyncCommand CommandeSendMessage { get; set; }
    }
}
