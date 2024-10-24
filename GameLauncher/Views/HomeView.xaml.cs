using Geraldine.GameLauncher.ViewModels;
using System.Windows;
using System;
using System.Windows.Controls;

namespace Geraldine.GameLauncher.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        HomeViewModel VM => DataContext as HomeViewModel;

        public HomeView()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            //if (File.Exists(gameExe) && Status == LauncherStatus.ready)
            //{
            //    ProcessStartInfo startInfo = new ProcessStartInfo(gameExe);
            //    startInfo.WorkingDirectory = Path.Combine(rootPath, "Build");
            //    Process.Start(startInfo);
            //
            //    Close();
            //}
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            //if (File.Exists(gameExe) && Status == LauncherStatus.ready)
            //{
            //    ProcessStartInfo startInfo = new ProcessStartInfo(gameExe);
            //    startInfo.WorkingDirectory = Path.Combine(rootPath, "Build");
            //    Process.Start(startInfo);

            //    Close();
            //}
        }
    }
}
