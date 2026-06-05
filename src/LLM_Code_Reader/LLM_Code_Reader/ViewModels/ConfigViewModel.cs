using LLM_Code_Reader.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LLM_Code_Reader.ViewModels
{
    class ConfigViewModel : BaseViewModel
    {
        private string _key;

        public string Key { get => _key; set { _key = value; OnPropertyChanged(); } }

        public ConfigViewModel()
        {
            _key = LLM_Code_Reader.Properties.Settings.Default.API_KEY;
            CommandeSaveKey = new RelayCommand(SaveKey, null);
        }

        public void SaveKey(object? obj)
        {
            LLM_Code_Reader.Properties.Settings.Default.API_KEY = _key;
            LLM_Code_Reader.Properties.Settings.Default.Save();
        }

        public RelayCommand CommandeSaveKey { get; set; }
    }
}
