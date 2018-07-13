using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUApiLib;

namespace xamlFrame.Lib
{
    class InstalledUpdate
    {
        public InstalledUpdate(string Title, DateTime InstalledDate)
        {
            this.Title = Title;
            this.InstalledDate = InstalledDate;
        }

        public string Title { get; set; }
        public DateTime InstalledDate { get; set; }
    }

    class WindowsUpdatesClientStatus
    {
        public WindowsUpdatesClientStatus(bool enabled,
            bool rebootRequired,
            List<InstalledUpdate> installedUpdates,
            DateTime? lastSearchSuccess,
            DateTime? lastInstallationSuccess,
            int countOutstanding,
            DateTime oldestOutstanding)
        {
            this.Enabled = enabled;
            this.RebootRequired = rebootRequired;
            this.InstalledUpdates = installedUpdates;
            this.LastSearchSuccess = lastSearchSuccess;
            this.LastInstallationSuccess = lastInstallationSuccess;
            this.CountOutstanding = countOutstanding;
            this.OldestOutstanding = oldestOutstanding;
        }

        public bool Enabled { get; }
        public bool RebootRequired { get; }
        public List<InstalledUpdate> InstalledUpdates { get; }
        public DateTime? LastSearchSuccess { get; }
        public DateTime? LastInstallationSuccess { get; }
        public int CountOutstanding { get; }
        public DateTime OldestOutstanding { get; }
    }

    class WindowsUpdatesClientProxy
    {
        public WindowsUpdatesClientStatus GetInfoSummary(bool online = false)
        {
            List<InstalledUpdate> installedUpdates = GetInstalledUpdates();
            Tuple<DateTime?, DateTime?> lastSearchAndInstall = GetDatesLastSearchAndInstall();
            Tuple<int, DateTime, List<IUpdate>> outstandingUpdates = GetOutstandingUpdates(online: online);
            return new WindowsUpdatesClientStatus(
                enabled: IsWindowsUpdateEnabled(),
                rebootRequired: IsRebootPending(),
                installedUpdates: installedUpdates,
                lastSearchSuccess: lastSearchAndInstall.Item1,
                lastInstallationSuccess: lastSearchAndInstall.Item2,
                countOutstanding: outstandingUpdates.Item1,
                oldestOutstanding: outstandingUpdates.Item2
            );
        }

        public Boolean IsWindowsUpdateEnabled()
        {
            var updates = new AutomaticUpdates();
            return updates.ServiceEnabled;
        }

        public Boolean IsRebootPending()
        {
            // Note: This may need to be rewritten to use IUpdate2 for an IUpdateSearcher result of isInstalled=1
            // See:  https://docs.microsoft.com/en-us/previous-versions/windows/desktop/aa386100%28v%3dvs.85%29
            bool rebootRequired = false;

            string regKeyReboot = @"SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update\RebootRequired";
            RegistryView regOSNativeView = RegistryView.Registry64;
            if (! Environment.Is64BitOperatingSystem)
            {
                regOSNativeView = RegistryView.Registry32;
            }
            RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, regOSNativeView);
            using (RegistryKey key = baseKey.OpenSubKey(regKeyReboot))
            {
                if (key is null)
                {
                    return false;
                }

                foreach (string valueName in key.GetValueNames())
                {
                    string value = Convert.ToString(key.GetValue(valueName));
                    if (value.Equals("1"))
                    {
                        rebootRequired = true;
                    }
                }
            }
            return rebootRequired;
        }

        private IUpdateSearcher GetNewSearcher(bool online = false)
        {
            UpdateSession updateSession = new UpdateSession();
            IUpdateSearcher updateSearcher = updateSession.CreateUpdateSearcher();
            updateSearcher.Online = online;
            return updateSearcher;
        }

        public List<InstalledUpdate> GetInstalledUpdates()
        {
            IUpdateSearcher updateSearcher = GetNewSearcher(online: false);
            int count = updateSearcher.GetTotalHistoryCount();
            IUpdateHistoryEntryCollection history = updateSearcher.QueryHistory(0, count);
            List<InstalledUpdate> installedUpdates = new List<InstalledUpdate>();
            for (int i = 0; i < count; ++i)
            {
                installedUpdates.Add(new InstalledUpdate(Title: history[i].Title, InstalledDate: history[i].Date));
            }
            return installedUpdates;
        }

        public Tuple<int, DateTime, List<IUpdate>> GetOutstandingUpdates(bool online = false)
        {
            IUpdateSearcher updateSearcher = GetNewSearcher(online: online);
            ISearchResult requiredPatches = updateSearcher.Search("IsInstalled=0 And IsHidden=0");
            int countRequired = requiredPatches.Updates.Count;
            DateTime oldestUpdate = new DateTime();
            List<IUpdate> updates = new List<IUpdate>();
            foreach (IUpdate update in requiredPatches.Updates)
            {
                updates.Add(update);
                if (update.LastDeploymentChangeTime > oldestUpdate)
                {
                    oldestUpdate = update.LastDeploymentChangeTime;
                }
            }
            return new Tuple<int, DateTime, List<IUpdate>>(countRequired, oldestUpdate, updates);
        }

        public Tuple<DateTime?,DateTime?> GetDatesLastSearchAndInstall()
        {
            var auc = new AutomaticUpdates();

            DateTime? lastInstallationSuccessDateUtc = null;
            if (auc.Results.LastInstallationSuccessDate is DateTime)
                lastInstallationSuccessDateUtc = new DateTime(((DateTime)auc.Results.LastInstallationSuccessDate).Ticks, DateTimeKind.Utc);

            DateTime? lastSearchSuccessDateUtc = null;
            if (auc.Results.LastSearchSuccessDate is DateTime)
                lastSearchSuccessDateUtc = new DateTime(((DateTime)auc.Results.LastSearchSuccessDate).Ticks, DateTimeKind.Utc);

            return new Tuple<DateTime?, DateTime?>(lastSearchSuccessDateUtc, lastInstallationSuccessDateUtc);
        }
    }
}
