using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nlSystem
{
    internal class sstOS
    {
        static void DisplayOSVersion()
        {
            OperatingSystem os = Environment.OSVersion;

            Version ver = os.Version;



            Console.WriteLine("OS: {0}\n\tMajor: {1}\n" +

                "\tMinor: {2}\n\tBuild: {3}",

                os.VersionString,

                ver.Major, ver.Minor,

                ver.Build);

        }
    }
}
