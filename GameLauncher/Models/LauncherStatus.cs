using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geraldine.GameLauncher.Models
{
    internal enum LauncherStatus
    {
        ready,
        failed,
        updatingLauncher,
        downloadingGame,
        updatingGame
    }
}
