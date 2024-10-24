using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geraldine.GameLauncher.Models
{
    internal class Version
    {
        internal static Version zero = new Version(0, 0, 0);

        private int major;
        private int minor;
        private int subMinor;

        internal Version(short _major, short _minor, short _subMinor)
        {
            major = _major;
            minor = _minor;
            subMinor = _subMinor;
        }
        internal Version(string _version)
        {
            if (string.IsNullOrEmpty(_version))
                return;

            string[] versionStrings = _version.Split('.');
            if (versionStrings.Length != 3)
            {
                major = 0;
                minor = 0;
                subMinor = 0;
                return;
            }

            if(versionStrings[0].StartsWith("v"))
                versionStrings[0] = versionStrings[0].Substring(1);

            major = int.Parse(versionStrings[0]);
            minor = int.Parse(versionStrings[1]);
            subMinor = int.Parse(versionStrings[2]);
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
