using Geraldine.GameLauncher.Models;
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

namespace Geraldine.GameLauncher.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        private string _version;

        internal string StatusString
        {
            get
            {
                switch (MainWindowViewModel.Instance.LauncherStatus)
                {
                    case LauncherStatus.ready:
                        return "Done";
                    case LauncherStatus.failed:
                        return "Update Failed";
                    case LauncherStatus.downloadingGame:
                        return "Downloading Game";
                    case LauncherStatus.updatingGame:
                        return "Downloading Update";
                    default:
                        return "";
                }
            }
        }

        internal Visibility StatusStringVisibility
        {
            get
            {
                switch (MainWindowViewModel.Instance.LauncherStatus)
                {
                    case LauncherStatus.ready:
                        return Visibility.Collapsed;
                    default:
                        return Visibility.Visible;
                }
            }
        }

        internal string VersionString
        {
            get { return _version; }
            set 
            {
                _version = value;
                SetProperty(ref _version, value);
            }

        }

        internal HomeViewModel()
        {
        }
    }
}
