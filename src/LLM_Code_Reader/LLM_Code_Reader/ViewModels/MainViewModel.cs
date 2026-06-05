using LLM_Code_Reader.ViewModels.Commands;
using LLM_Code_Reader.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Animation;

namespace LLM_Code_Reader.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;
        private ChatViewModel _chatViewModel;

        public MainViewModel()
        {
            _chatViewModel = new ChatViewModel();
            _currentViewModel = _chatViewModel;

            CommandeOpenConfig = new RelayCommand(OpenConfig, null);
            CommandeClosing = new AsyncCommand(Closing, null);
        }


        public BaseViewModel CurrentViewModel { 
            get { return _currentViewModel; }
            set { _currentViewModel = value; }
        }


        public void OpenConfig(object? obj) 
        {
            ConfigView configView = new ConfigView();
            configView.Owner = App.Current.MainWindow;
            configView.ShowDialog();
        }

        public async Task Closing(object? obj)
        { // fermer le modèle avant de fermer pour libérer la mémoire
            await _chatViewModel.Connector.StopModel();
        }

        public RelayCommand CommandeOpenConfig { get; set; }
        public AsyncCommand CommandeClosing { get; set; }

    }
}
