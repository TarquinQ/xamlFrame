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
        public string Manufacturer { get; }
        public string Model { get; }
        public string OSVersion { get; }
        public string OSRevision { get; }
        public int RAMTotal { get; }
        //public ? WindowsExperience Rating -- Get-WmiObject -class Win32_WinSAT
        // * Primary IPv4 (w/ Default Route)
        // * Primary IPv6 (w/ Default Route)

        public BasicMachineInfo()
        {
            ManagementScope manScope = new ManagementScope(@"\\.\root\cimv2");

            // define the information we want to query - in this case, just grab all properties of the object
            ObjectQuery queryObj = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");

            // connect and set up our search
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(manScope, queryObj);
            ManagementObjectCollection wmiResults = Searcher.Get();

            foreach (ManagementObject result in wmiResults)
            {
                this.CurrUserName = "";
                this.Manufacturer = result["Manufacturer"].ToString();
                this.Model = result["Model"].ToString();
                this.RAMTotal = ((Int32)(result["TotalPhysicalMemory"]))/1024/1024 ;
            }
        }
    }
}
