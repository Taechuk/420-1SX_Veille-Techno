using LLM_Code_Reader.Models;
using LLM_Code_Reader.ViewModels.Commands;
using Microsoft.Extensions.AI;
using Microsoft.Win32;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace LLM_Code_Reader.ViewModels
{
    class ChatViewModel : BaseViewModel
    {
        private string _model = "qwen2.5-coder";
        private string _content;
        private Chat? _convo;
        private bool _highCtx = false;

        private List<string> _modelList;

        public ObservableCollection<Message> Messages { get; set; }
        public List<string> ModelList { get => _modelList; set { _modelList = value; OnPropertyChanged(nameof(ModelList)); } }

        public Chat? Conversation { get => _convo; set { _convo = value; OnPropertyChanged(nameof(IsNotActive)); } }
        public OllamaConnector? Connector { get; set; }

        public string SelectedModel { get => _model; set { _model = value; OnPropertyChanged(nameof(SelectedModel)); } }

        public string Content { get => _content; set { _content = value; OnPropertyChanged(nameof(Content)); } }
        public bool HighCtx { get => _highCtx; set { _highCtx = value; OnPropertyChanged(nameof(HighCtx)); } }

        public bool Thinking { get; set; }

        public bool IsNotActive { get => Conversation == null; }

        public ChatViewModel()
        {
            ModelList = ["qwen2.5-coder", "cogito:3b", "cogito:8b", "phi4-mini", "codellama", "qwen3.5", "qwen3.6"];
            Messages = new ObservableCollection<Message>();
            Messages.Add(new Message("Hors ligne", "System"));
            Content = string.Empty;
            Conversation = null;
            Thinking = false;

            CommandeCreateConnector = new AsyncCommand(CreateConnector, (obj) => { return Conversation == null; });
            CommandeSendMessage = new AsyncCommand(SendMessage, (obj) => { return Conversation != null && !Thinking && !Content.IsWhiteSpace(); });
            CommandeSendFile = new AsyncCommand(SendFile, (obj) => { return Conversation != null && !Thinking; });
            CommandeSendFolder = new AsyncCommand(SendFolder, (obj) => { return Conversation != null && !Thinking && HighCtx; });
            CommandeStopChat = new AsyncCommand(StopChat, (obj) => { return Conversation != null; });
        }

        public async Task CreateConnector(object? obj)
        {
            if (Conversation != null) //fermeture de l'ancien modèle (ne devrait pas être appelé, mais au cas oû)
            { 
                await Connector.StopModel();
                Conversation = null;    
            }

            Connector = new OllamaConnector(_model);
            Messages.Add(new Message($"Le modèle {_model} est en cours de chargement. Cela peut prendre un peu de temps.", "System"));

            await Connector.SetupModel();


            if (Connector.Available)
            {
                Conversation = await Connector.CreateChat(HighCtx ? "strong" : "weak"); //création du chat avec contexte requis selon la selection

                var response = string.Empty;
                await foreach (var token in Conversation.SendAsync("Début de la conversation, dis bonjour et présente-toi.")) //attente de réponse
                {
                    response += token; //tout mettre en un message vu que le token est envoyé sur plusieurs packets
                }
                Messages.Clear();
                Messages.Add(new Message(response, Connector.Model)); //message initial du bot
            }
            else
            {
                Messages.Clear();
                Messages.Add(new Message($"Le modèle {_model} n'est pas disponible et n'a pas pu être récuperé. Vérifiez que Ollama soit bien installé et que vous avez une connection stable si c'est la première fois que le modèle est utilisé.", "System"));
            }
        }

        public async Task SendMessage(object? obj)
        {
            if (Conversation == null) return;
            Messages.Add(new Message(Content, "User"));
            string sentContent = Content; //save l'input pour l'envoyer
            Content = string.Empty; //clear l'input
            Thinking = true; //empeche d'envoyer plusieurs messages
            Messages.Add(new Message("Hmmmm...", "System")); //action en cours

            string response = string.Empty;

            using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3))) //timeout de 3 minutes pour éviter les réponses infinies
            {
                try
                {
                    await foreach (var token in Conversation.SendAsync(sentContent, tools: Connector.McpTools, cancellationToken: cts.Token)) //attente de réponse
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
                    Messages.Add(new Message($"Une erreur est survenue. {ex}", "System")); //message du système
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

                string sentContent = $"{Content}\nVoici le contenu du fichier {openFileDialog.SafeFileName} :\n```\n{fileContent}```"; //contenu du fichier à envoyer

                if (sentContent.Length > Conversation.Options.NumCtx * 3.3) //limite de tokens pour éviter d'envoyer trop de données à la fois
                {
                    MessageBox.Show($"Le contenu du fichier est trop volumineux pour être envoyé. Veuillez sélectionner un fichier plus petits.", "Dossier trop volumineux", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                Messages.Add(new Message($"{Content}\nFichier {openFileDialog.SafeFileName} envoyé.", "User"));

                Thinking = true; //empeche d'envoyer plusieurs messages
                Content = string.Empty; //clear l'input
                Messages.Add(new Message("Hmmmm...", "System")); //action en cours
                string response = string.Empty;
                using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3))) //timeout de 3 minutes pour éviter les réponses infinies
                {
                    try
                    {
                        await foreach (var token in Conversation.SendAsync(sentContent, tools: Connector.McpTools, cancellationToken: cts.Token)) //attente de réponse
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
                        Messages.Add(new Message($"Une erreur est survenue. {ex}", "System")); //message du système
                    }
                }
                Messages.RemoveAt(Messages.Count - 1); //enlever le message du système
                Messages.Add(new Message(response, Connector.Model)); //message du bot
                Thinking = false; //le user peut continuer
            }
        }

        public async Task SendFolder(object? obj)
        {
            if (Conversation == null) return;
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();

            if(openFolderDialog.ShowDialog() == true)
            {
                string folderName = openFolderDialog.FolderName;

                var excludedDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {  //LISTE FAIT PAR GEMINI, exclus les dossiers et fichiers qui ne sont généralement pas importants pour l'analyse de code et qui peuvent être très volumineux ou contenir des données sensibles
                    // --- DOSSIERS ET FICHIERS DE CONFIG LOGICIELS ---
                    "node_modules", "bin", "obj", ".git", ".vs", ".gitignore", "__pycache__",

                    // --- IMAGES ET DESIGN (Aucun risque de code) ---
                    ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".ico", ".tiff", ".psd", ".ai",

                    // --- DOCUMENTS / BUREAUTIQUE ---
                    ".pdf", ".docx", ".doc", ".xlsx", ".xls", ".pptx", ".ppt", ".odt", ".ods",

                    // --- AUDIO / VIDÉO (Fichiers très lourds et inutiles pour un LLM) ---
                    ".mp3", ".wav", ".flac", ".aac", ".mp4", ".mkv", ".avi", ".mov", ".wmv", ".flv",

                    // --- ARCHIVES / COMPRESSION ---
                    ".zip", ".rar", ".7z", ".tar", ".gz", ".iso", ".dmg",

                    // --- POLICES DE CARACTÈRES ---
                    ".ttf", ".otf", ".woff", ".woff2", ".eot",

                    // --- COMPILATIONS ET BINAIRES NATIFS ---
                    ".exe", ".dll", ".so", ".dylib", ".bin", ".suo"
                };  //la liste n'est pas exhaustive mais devrait couvrir la très grande majorité des cas d'utilisation normal

                var files = Directory.EnumerateFiles(folderName, "*.*", SearchOption.AllDirectories).Where(file => //ignore les fichiers/dossiers exclus
                {
                    string extension = Path.GetExtension(file);
                    if(excludedDirs.Contains(extension)) return false;

                    string[] segments = file.Split(Path.DirectorySeparatorChar);
                    bool isExcluded = segments.Any(segment => excludedDirs.Contains(segment));
                    return !isExcluded;
                });
                var fileContents = new StringBuilder(); //construction du message
                fileContents.AppendLine($"{Content}\nVoici le contenu du dossier {Path.GetFileName(folderName)} :");

                int fileCount = 0;
                foreach (var file in files)
                {
                    fileCount++;
                    string relativePath = Path.GetRelativePath(folderName, file);
                    string content = await File.ReadAllTextAsync(file);

                    fileContents.AppendLine($"\nFichier {fileCount}: {relativePath}\n```\n{content}\n```"); //contenu du fichier à envoyer

                    if (fileContents.Length > Conversation.Options.NumCtx * 3.3) //limite de tokens pour éviter d'envoyer trop de données à la fois
                    {
                        MessageBox.Show($"Le contenu du dossier est trop volumineux pour être envoyé. Veuillez sélectionner un dossier avec moins de fichiers ou des fichiers plus petits.", "Dossier trop volumineux", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                if (fileCount == 0)
                {
                    MessageBox.Show($"Le dossier {Path.GetFileName(folderName)} est vide.", "Dossier vide", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                fileContents.AppendLine("Fin du dossier.\n Peux-tu me faire une analyse globale de ce code, voir si il y a des bugs à regler ou qu'est-ce qui pourrait être amélioré?"); //indique la fin du contenu du dossier
                Messages.Add(new Message($"{Content}\nDossier {Path.GetFileName(folderName)} envoyé avec {fileCount} fichiers.", "User"));

                Thinking = true; //empeche d'envoyer plusieurs messages
                Content = string.Empty; //clear l'input
                Messages.Add(new Message("Hmmmm...", "System")); //action en cours
                string response = string.Empty;
                using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3))) //timeout de 3 minutes pour éviter les réponses infinies
                {
                    try
                    {
                        await foreach (var token in Conversation.SendAsync(fileContents.ToString(), tools: Connector.McpTools, cancellationToken: cts.Token)) //attente de réponse
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
                        Messages.Add(new Message($"Une erreur est survenue. {ex}", "System")); //message du système
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
                Messages.Add(new Message($"{Connector.Model} s'est arrêté.", "System"));
                Conversation = null; //chat terminé
                Thinking = false;
            }
        }

        public AsyncCommand CommandeCreateConnector { get; set; }
        public AsyncCommand CommandeSendMessage { get; set; }
        public AsyncCommand CommandeSendFile { get; set; }
        public AsyncCommand CommandeSendFolder { get; set; }
        public AsyncCommand CommandeStopChat { get; set; }

    }
}
