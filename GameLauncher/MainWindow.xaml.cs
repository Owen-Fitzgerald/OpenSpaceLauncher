using Geraldine.GameLauncher.Models;
using Geraldine.GameLauncher.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Geraldine.GameLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal MainWindowViewModel VM => DataContext as MainWindowViewModel;
        private UserControl currentTabControl;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            currentTabControl = HomeViewControl;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.Dispatcher.InvokeAsync(async () =>
            {
                await VM.CheckForUpdates();
            });
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if ((bool)rb.IsChecked)
                {
                    if (currentTabControl != null) { currentTabControl.Visibility = Visibility.Collapsed; }
                    if(rb == TabButtonHome)
                    {
                        currentTabControl = HomeViewControl;
                    }
                    else if(rb == TabButtonDLC)
                    {
                        currentTabControl = DLCViewControl;
                    }
                    else if (rb == TabButtonPlaysets)
                    {
                        currentTabControl = PlaysetsViewControl;
                    }
                    else if (rb == TabButtonMods)
                    {
                        currentTabControl = ModsViewControl;
                    }
                    else if (rb == TabButtonSettings)
                    {
                        currentTabControl = SettingsViewControl;
                    }
                    if (currentTabControl != null) { currentTabControl.Visibility = Visibility.Visible; }
                }
            }
        }

        private void ToggleMaximizedMinimizedState()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaxMinButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleMaximizedMinimizedState();
        }

        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                ToggleMaximizedMinimizedState();
            }
            else if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        //private void InstallLauncherFiles()
        //{
        //    try
        //    {
        //        UpdateStatus(LauncherStatus.updatingLauncher);
        //        HttpClient client = new HttpClient();
        //        Version onlineVersion = new Version(client.GetStringAsync(GameVersionUrl).Result);

        //        //client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadLauncherCompletedCallback);
        //        //client.DownloadFileAsync(new Uri("https://drive.google.com/uc?export=download&id=1SNA_3P5wVp4tZi5NKhiGAAD6q4ilbaaf"), gameZip, onlineVersion);
        //        client.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateStatus(LauncherStatus.failed);
        //        MessageBox.Show($"Error installing game files: {ex}");
        //    }
        //}

        //private void InstallGameFiles(bool _isUpdate)
        //{
        //    try
        //    {
        //        UpdateStatus(_isUpdate ? LauncherStatus.updatingGame : LauncherStatus.downloadingGame);
        //        HttpClient client = new HttpClient();
        //        Version onlineVersion = new Version(client.GetStringAsync(GameVersionUrl).Result);

        //        //client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);
        //        //client.DownloadFileAsync(new Uri("https://drive.google.com/uc?export=download&id=1SNA_3P5wVp4tZi5NKhiGAAD6q4ilbaaf"), gameZip, onlineVersion);
        //        client.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateStatus(LauncherStatus.failed);
        //        MessageBox.Show($"Error installing game files: {ex}");
        //    }
        //}

        //private void DownloadLauncherCompletedCallback(object sender, AsyncCompletedEventArgs e)
        //{
        //    try
        //    {
        //        string onlineVersion = ((Version)e.UserState).ToString();
        //        //ZipFile.ExtractToDirectory(gameZip, rootPath, true);
        //        //File.Delete(gameZip);

        //        File.WriteAllText(gameVersionFile, onlineVersion);

        //        VersionString = onlineVersion;
        //        UpdateStatus(LauncherStatus.ready);
        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateStatus(LauncherStatus.failed);
        //        MessageBox.Show($"Error finishing download: {ex}");
        //    }
        //}

        //private void DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        //{
        //    try
        //    {
        //        string onlineVersion = ((Version)e.UserState).ToString();
        //        //ZipFile.ExtractToDirectory(gameZip, rootPath, true);
        //        //File.Delete(gameZip);

        //        File.WriteAllText(gameVersionFile, onlineVersion);

        //        VersionString = onlineVersion;
        //        UpdateStatus(LauncherStatus.ready);
        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateStatus(LauncherStatus.failed);
        //        MessageBox.Show($"Error finishing download: {ex}");
        //    }
        //}
    }
}
