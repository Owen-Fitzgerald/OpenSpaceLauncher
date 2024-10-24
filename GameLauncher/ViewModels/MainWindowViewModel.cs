using Geraldine.GameLauncher.Models;
using Geraldine.GameLauncher.Views;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Version = Geraldine.GameLauncher.Models.Version;

namespace Geraldine.GameLauncher.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string rootPath;
        private string _launcherVersion = "init";
        private string _gameVersion;
        private string launcherVersionFile;
        private string gameVersionFile;
        private LauncherStatus _status;

        private static MainWindowViewModel _instance;

        public static MainWindowViewModel Instance
        { 
            get
            {
                if(_instance == null)
                {
                    _instance = new MainWindowViewModel();
                }

                return _instance;
            }
        }

        internal LauncherStatus LauncherStatus
        {
            get { return _status; }
            set
            {
                _status = value;
                SetProperty(ref _status, value);
            }

        }

        internal string LauncherVersionString
        {
            get { return _launcherVersion; }
            set
            {
                _launcherVersion = value;
                SetProperty(ref _launcherVersion, value);
            }

        }

        internal string GameVersionString
        {
            get { return _gameVersion; }
            set
            {
                _gameVersion = value;
                SetProperty(ref _gameVersion, value);
            }

        }

        public MainWindowViewModel()
        {
            _instance = this;
            rootPath = Directory.GetCurrentDirectory();
        }

        public async Task CheckForUpdates()
        {
            //Check for launcher updates
            Version onlineVersion = new Version(await GitHubReleaseDownloader.GetLatestLauncherVersionAsync());
            if (File.Exists(launcherVersionFile))
            {
                Version localVersion = new Version(File.ReadAllText(launcherVersionFile));
                LauncherVersionString = localVersion.ToString();

                try
                {

                    HttpClient client = new HttpClient();
                    client.Dispose();

                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        UpdateStatus(LauncherStatus.updatingLauncher);
                        await GitHubReleaseDownloader.DownloadLauncherAsync();
                        LauncherVersionString = onlineVersion.ToString();
                    }
                }
                catch (Exception ex)
                {
                    UpdateStatus(LauncherStatus.failed);
                    MessageBox.Show($"Error checking for launcher updates: {ex}");
                }
            }
            else
            {
                UpdateStatus(LauncherStatus.updatingLauncher);
                await GitHubReleaseDownloader.DownloadLauncherAsync();
                onlineVersion = new Version(await GitHubReleaseDownloader.GetLatestLauncherVersionAsync());
                LauncherVersionString = onlineVersion.ToString();
            }

            //Check for game updates
            onlineVersion = new Version(await GitHubReleaseDownloader.GetLatestGameVersionAsync());
            if (File.Exists(gameVersionFile))
            {
                Version localVersion = new Version(File.ReadAllText(gameVersionFile));
                GameVersionString = localVersion.ToString();

                try
                {
                    HttpClient client = new HttpClient();
                    client.Dispose();

                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        UpdateStatus(LauncherStatus.updatingGame);
                        await GitHubReleaseDownloader.DownloadGameAsync();
                        GameVersionString = onlineVersion.ToString();
                    }
                    else
                    {
                        UpdateStatus(LauncherStatus.ready);
                    }
                }
                catch (Exception ex)
                {
                    UpdateStatus(LauncherStatus.failed);
                    MessageBox.Show($"Error checking for game updates: {ex}");
                }
            }
            else
            {
                UpdateStatus(LauncherStatus.updatingGame);
                await GitHubReleaseDownloader.DownloadGameAsync();
                onlineVersion = new Version(await GitHubReleaseDownloader.GetLatestGameVersionAsync());
                GameVersionString = onlineVersion.ToString();
            }
        }

        private void UpdateStatus(LauncherStatus status)
        {
            _status = status;
        }
    }
}
