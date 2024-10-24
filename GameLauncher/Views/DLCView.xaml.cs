using Geraldine.GameLauncher.ViewModels;
using System.Windows;
using System;
using System.Windows.Controls;

namespace Geraldine.GameLauncher.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class DLCView : UserControl
    {
        DLCViewModel VM => DataContext as DLCViewModel;

        public DLCView()
        {
            InitializeComponent();
            DataContext = new DLCViewModel();
        }
    }
}
