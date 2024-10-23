﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace GameLauncher
{
    enum LauncherStatus
    {
        ready,
        failed,
        updatingLauncher,
        downloadingGame,
        updatingGame
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string rootPath;
        private string launcherVersionFile;
        private string gameVersionFile;

        private const string LauncherVersionUrl = "https://raw.githubusercontent.com/Owen-Fitzgerald/OpenSpaceLauncher/refs/heads/master/Version.txt";
        private const string GameVersionUrl = "https://raw.githubusercontent.com/Owen-Fitzgerald/OpenSpaceLauncher/refs/heads/master/Version.txt";

        private LauncherStatus _status;
        internal LauncherStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                switch (_status)
                {
                    case LauncherStatus.ready:
                        StatusTextBlock.Text = "Done";
                        StatusTextBlock.Visibility = Visibility.Collapsed;
                        break;
                    case LauncherStatus.failed:
                        StatusTextBlock.Text = "Update Failed";
                        StatusTextBlock.Visibility = Visibility.Visible;
                        break;
                    case LauncherStatus.downloadingGame:
                        StatusTextBlock.Text = "Downloading Game";
                        StatusTextBlock.Visibility = Visibility.Visible;
                        break;
                    case LauncherStatus.updatingGame:
                        StatusTextBlock.Text = "Downloading Update";
                        StatusTextBlock.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            rootPath = Directory.GetCurrentDirectory();
            launcherVersionFile = Path.Combine(rootPath, "LauncherVersion.txt");
            gameVersionFile = Path.Combine(rootPath, "GameVersion.txt");
        }

        private void CheckForUpdates()
        {
            if (File.Exists(launcherVersionFile))
            {
                Version localVersion = new Version(File.ReadAllText(launcherVersionFile));
                LauncherVersionText.Text = localVersion.ToString();

                try
                {

                    HttpClient client = new HttpClient();
                    Version onlineVersion = new Version(client.GetStringAsync(LauncherVersionUrl).Result);
                    client.Dispose();

                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        InstallLauncherFiles();
                    }
                }
                catch (Exception ex)
                {
                    Status = LauncherStatus.failed;
                    MessageBox.Show($"Error checking for launcher updates: {ex}");
                }
            }
            else
            {
                InstallLauncherFiles();
            }

            if (File.Exists(gameVersionFile))
            {
                Version localVersion = new Version(File.ReadAllText(gameVersionFile));
                VersionText.Text = localVersion.ToString();

                try
                {
                    HttpClient client = new HttpClient();
                    Version onlineVersion = new Version(client.GetStringAsync(GameVersionUrl).Result);
                    client.Dispose();

                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        InstallGameFiles(true);
                    }
                    else
                    {
                        Status = LauncherStatus.ready;
                    }
                }
                catch (Exception ex)
                {
                    Status = LauncherStatus.failed;
                    MessageBox.Show($"Error checking for game updates: {ex}");
                }
            }
            else
            {
                InstallGameFiles(false);
            }
        }

        private void InstallLauncherFiles()
        {
            try
            {
                Status = LauncherStatus.updatingLauncher;
                HttpClient client = new HttpClient();
                Version onlineVersion = new Version(client.GetStringAsync(GameVersionUrl).Result);

                //client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadLauncherCompletedCallback);
                //client.DownloadFileAsync(new Uri("https://drive.google.com/uc?export=download&id=1SNA_3P5wVp4tZi5NKhiGAAD6q4ilbaaf"), gameZip, onlineVersion);
                client.Dispose();
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }

        private void InstallGameFiles(bool _isUpdate)
        {
            try
            {
                Status = _isUpdate ? LauncherStatus.updatingGame : LauncherStatus.downloadingGame;
                HttpClient client = new HttpClient();
                Version onlineVersion = new Version(client.GetStringAsync(GameVersionUrl).Result);

                //client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);
                //client.DownloadFileAsync(new Uri("https://drive.google.com/uc?export=download&id=1SNA_3P5wVp4tZi5NKhiGAAD6q4ilbaaf"), gameZip, onlineVersion);
                client.Dispose();
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error installing game files: {ex}");
            }
        }

        private void DownloadLauncherCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string onlineVersion = ((Version)e.UserState).ToString();
                //ZipFile.ExtractToDirectory(gameZip, rootPath, true);
                //File.Delete(gameZip);

                File.WriteAllText(gameVersionFile, onlineVersion);

                VersionText.Text = onlineVersion;
                Status = LauncherStatus.ready;
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error finishing download: {ex}");
            }
        }

        private void DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string onlineVersion = ((Version)e.UserState).ToString();
                //ZipFile.ExtractToDirectory(gameZip, rootPath, true);
                //File.Delete(gameZip);

                File.WriteAllText(gameVersionFile, onlineVersion);

                VersionText.Text = onlineVersion;
                Status = LauncherStatus.ready;
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Error finishing download: {ex}");
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            CheckForUpdates();
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

    struct Version
    {
        internal static Version zero = new Version(0, 0, 0);

        private short major;
        private short minor;
        private short subMinor;

        internal Version(short _major, short _minor, short _subMinor)
        {
            major = _major;
            minor = _minor;
            subMinor = _subMinor;
        }
        internal Version(string _version)
        {
            string[] versionStrings = _version.Split('.');
            if (versionStrings.Length != 3)
            {
                major = 0;
                minor = 0;
                subMinor = 0;
                return;
            }

            major = short.Parse(versionStrings[0]);
            minor = short.Parse(versionStrings[1]);
            subMinor = short.Parse(versionStrings[2]);
        }

        internal bool IsDifferentThan(Version _otherVersion)
        {
            if (major != _otherVersion.major)
            {
                return true;
            }
            else
            {
                if (minor != _otherVersion.minor)
                {
                    return true;
                }
                else
                {
                    if (subMinor != _otherVersion.subMinor)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"{major}.{minor}.{subMinor}";
        }
    }
}
