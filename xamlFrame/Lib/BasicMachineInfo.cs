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
        private readonly GetLocalCIMData CIMDataProvider;

        public BasicMachineInfo()
        {
            CIMDataProvider = new GetLocalCIMData();
            string QueryString = "Select * from Win32_ComputerSystem";

            foreach (var result in CIMDataProvider.GetDataFromQuery(QueryString))
            {
                CurrUserName = "";
                Manufacturer = result.CimInstanceProperties["Manufacturer"].ToString();
                Model = result.CimInstanceProperties["Model"].ToString();
                RAMTotal = ((Int32)(result.CimInstanceProperties["TotalPhysicalMemory"].ToString()))/1024/1024 ;
            }
        }
    }
}
