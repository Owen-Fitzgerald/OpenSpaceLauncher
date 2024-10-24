using Geraldine.GameLauncher.ViewModels;
using System.Windows;
using System;
using System.Windows.Controls;

namespace Geraldine.GameLauncher.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class ModsView : UserControl
    {
        DLCViewModel VM => DataContext as DLCViewModel;

        public ModsView()
        {
            InitializeComponent();
            DataContext = new DLCViewModel();
        }
    }
}
