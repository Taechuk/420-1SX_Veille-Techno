using LLM_Code_Reader.Models;
using LLM_Code_Reader.ViewModels.Commands;
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

        public ObservableCollection<Message> Messages { get; set; }
        public Chat? Conversation { get; set; }
        public OllamaConnector? Connector { get; set; }

        public string SelectedModel { get => _model; set { _model = value; OnPropertyChanged(nameof(SelectedModel)); } }

        public string Content { get => _content; set { _content = value; OnPropertyChanged(nameof(Content)); } }

        public bool Thinking { get; set; }

        public ChatViewModel()
        {
            Messages = new ObservableCollection<Message>();
            Messages.Add(new Message("Currently Offline", "System"));

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
            Messages.Add(new Message(_content, "User"));
            Content = string.Empty;
            Thinking = true;
            Messages.Add(new Message("Thinking...", "System"));

            string response = string.Empty;
            await foreach(var token in Conversation.SendAsync(_content))
            {
                response += token;
            }
            Messages.RemoveAt(Messages.Count - 1);
            Messages.Add(new Message(response, "Model"));

            Thinking = false;

        }

        public AsyncCommand CommandeCreateConnector { get; set; }
        public AsyncCommand CommandeSendMessage { get; set; }
    }
}
