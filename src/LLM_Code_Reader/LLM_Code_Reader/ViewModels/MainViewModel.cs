using System;
using System.Collections.Generic;
using System.Text;

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
        }


        public BaseViewModel CurrentViewModel { 
            get { return _currentViewModel; }
            set { _currentViewModel = value; }
        }

    }
}
