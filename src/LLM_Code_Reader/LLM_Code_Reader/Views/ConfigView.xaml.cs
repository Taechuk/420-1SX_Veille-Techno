using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using LLM_Code_Reader.ViewModels;

namespace LLM_Code_Reader.Views
{
    /// <summary>
    /// Logique d'interaction pour ConfigView.xaml
    /// </summary>
    public partial class ConfigView : Window
    {
        public ConfigView()
        {
            InitializeComponent();
            this.DataContext = new ConfigViewModel();
        }
    }
}
