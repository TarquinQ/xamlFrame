using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xamlFrame.Lib.System
{
    /// <summary>
    /// A class to represent the state of a machine's health
    /// </summary>
    /// This class will 
    class MachineHealth
    {
        public int ScoreOverallHealth { get; }
        public int DiskSpaceFree { get; }
        public int ScoreDiskSpaceFree { get; }
        public int AntiVirusStatus { get; }
        public int ScoreAntiVirusStatus { get; }
        public int PatchingStatus { get; }
        public int ScorePatchingStatus { get; }
        public int BatteryHealth { get; }
        public int ScoreBatteryHealth { get; }
        public int Reliability { get; }
        public int ScoreReliability { get; }

        //** Disk space(SystemDrive) : 2 points: <5GB free 0pts, 10GB free 1pt, 20GB free 2pts
        //** A/V Status: 1 point: Switched on & defns up-to-date
        //** Patches: 3 pts: 1 point if lastChecked< 7 days, 1 point if installed< 2 months, 1 point if oldestOutstanding< 2 months
        //** Reliability: 1 point: 1 point if reliability rating above x (low thresh.) for prev 3 months
        //** Battery Health: 1 point: Laptops: FullChargedCharge< 60% of Design Capacity  (Desktops: 1 point always)

    }
}
