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
        public string CurrUserNameFull { get; }
        public string CurrUserName { get; }
        public string CurrUserDomain { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public string MachineSerial { get; }
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

            MachineName = Environment.MachineName;
            CurrUserNameFull = Environment.UserName; // domain\username
            CurrUserDomain = Environment.UserDomainName;
            CurrUserName = Environment.UserName.Remove(0, (CurrUserDomain.Length + 1));

            string cimClassName = "Win32_ComputerSystem";
            CIMDataProvider.GetEnumeratedInstances(className: cimClassName, firstInstance: out var result);

            Manufacturer = result.CimInstanceProperties["Manufacturer"].Value.ToString();
            Model = result.CimInstanceProperties["Model"].Value.ToString();
            int RAMTotalKB = result.CimInstanceProperties["TotalPhysicalMemory"].Value<int>();
            RAMTotalGB = RAMTotalKB / 1024 / 1024;

            cimClassName = "Win32_Bios";
            CIMDataProvider.GetEnumeratedInstances(className: cimClassName, firstInstance: out result);
            MachineSerial = result.CimInstanceProperties["SerialNumber"].Value.ToString();
        }
    }
}

