using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerCommon.Timing
{
    public static class GlobalCounter
    {
        static Stopwatch Watch { get; set; }

        public static double HostTickF;

        public static void Init()
        {
            Watch = new Stopwatch();

            Watch.Start();

            HostTickF = 1.0 / Stopwatch.Frequency;
        }

        public static ulong cntpct_el0
        {
            get
            {
                double Ticks = Watch.ElapsedTicks * HostTickF;

                return (ulong)(Ticks * 19_200_000);
            }
        }
    }
}
