using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace xamlFrame.Lib
{
    class BasicMachineInfo
    {
        public string MachineName { get; }
        public string CurrUserName { get; }
        public string CurrUserDomain { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public string OSVersion { get; }
        public string OSRevision { get; }
        public int RAMTotalGB { get; }
        //public ? WindowsExperience Rating -- Get-WmiObject -class Win32_WinSAT
        // * Primary IPv4 (w/ Default Route)
        // * Primary IPv6 (w/ Default Route)
        private readonly GetLocalCIMData CIMDataProvider;

        public BasicMachineInfo()
        {
            CIMDataProvider = new GetLocalCIMData();

            string QueryString = "Select * from Win32_ComputerSystem";
            using (var result = CIMDataProvider.GetDataFromQuery(QueryString).First())
            {
                Manufacturer = result.CimInstanceProperties["Manufacturer"].Value.ToString();
                Model = result.CimInstanceProperties["Model"].Value.ToString();
                int RAMTotalKB = int.Parse(result.CimInstanceProperties["TotalPhysicalMemory"].Value.ToString());
                RAMTotalGB = RAMTotalKB / 1024 / 1024;
            }
            CurrUserDomain = Environment.UserDomainName;
            CurrUserName = Environment.UserName.Remove(0, (CurrUserDomain.Length+1)); // domain\username
        }
    }
}
