using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using Microsoft.Management.Infrastructure;
using Microsoft.Win32;

namespace xamlFrame.Lib
{
    class BasicMachineInfo
    {
        public string CurrUserNameFull { get; }
        public string CurrUserName { get; }
        public string CurrUserDomain { get; }
        public string MachineName { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public string MachineSerial { get; }
        public long RAMTotalB { get; }
        public long RAMTotalGB { get; }
        public string OSName { get; }
        public string OSVersion { get; }
        public string OSRevision { get; }
        public string OSArchitecture { get; }
        public DateTime LastBootUpTime { get; }
        public string SystemDriveLetter { get; }
        public long SystemDriveSpaceTotal { get; }
        public long SystemDriveSpaceUsed { get; }
        public long SystemDriveSpaceFree { get; }
        public long SystemDriveSpaceTotalGB { get; }
        public long SystemDriveSpaceUsedGB { get; }
        public long SystemDriveSpaceFreeGB { get; }
        public int SystemDriveSpaceFreePercent { get; }
        //public ? WindowsExperience Rating -- Get-WmiObject -class Win32_WinSAT
        // * Primary IPv4 (w/ Default Route)
        // * Primary IPv6 (w/ Default Route)

        private readonly GetLocalCIMData CIMDataProvider;

        public BasicMachineInfo()
        {
            CIMDataProvider = new GetLocalCIMData();
            string cimClassName;
            CimInstance result;

            MachineName = Environment.MachineName;
            CurrUserNameFull = Environment.UserName; // domain\username
            CurrUserDomain = Environment.UserDomainName;
            CurrUserName = Environment.UserName.Remove(0, (CurrUserDomain.Length + 1));

            cimClassName = "Win32_ComputerSystem";
            CIMDataProvider.GetEnumeratedInstances(className: cimClassName, firstInstance: out result);
            Manufacturer = result.CimInstanceProperties["Manufacturer"].Value.ToString();
            Model = result.CimInstanceProperties["Model"].Value.ToString();

            cimClassName = "Win32_Bios";
            CIMDataProvider.GetEnumeratedInstances(className: cimClassName, firstInstance: out result);
            MachineSerial = result.CimInstanceProperties["SerialNumber"].Value.ToString();

            cimClassName = "Win32_PhysicalMemory";
            foreach (CimInstance physicalMemory in CIMDataProvider.GetEnumeratedInstances(className: cimClassName)) {
                RAMTotalB += physicalMemory.CimInstanceProperties["Capacity"].Value<long>();
            }
            RAMTotalGB = RAMTotalB / 1024 / 1024 / 1024;

            //Caption, Version, BuildNumber, OSArchitecture
            string releaseId = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString();
            cimClassName = "Win32_OperatingSystem";
            CIMDataProvider.GetEnumeratedInstances(className: cimClassName, firstInstance: out result);
            OSArchitecture = result.CimInstanceProperties["OSArchitecture"].Value.ToString();
            OSName = result.CimInstanceProperties["Caption"].Value.ToString() + " (" + OSArchitecture + ")";
            OSVersion = result.CimInstanceProperties["Version"].Value.ToString();
            if (String.IsNullOrWhiteSpace(releaseId))  // Is Not Win10...
            {
                OSRevision = result.CimInstanceProperties["BuildNumber"].Value.ToString();
            }
            else  // Is Win10
            {
                OSRevision = releaseId + " (" + result.CimInstanceProperties["BuildNumber"].Value.ToString() + ")";
            }
            LastBootUpTime = ManagementDateTimeConverter.ToDateTime(result.CimInstanceProperties["LastBootUpTime"].Value.ToString());


            SystemDriveLetter = Path.GetPathRoot(Environment.SystemDirectory).Substring(0, 1);
            DriveInfo systemDrive = new DriveInfo(SystemDriveLetter);
            SystemDriveSpaceTotal = systemDrive.TotalSize;
            SystemDriveSpaceFree = systemDrive.TotalFreeSpace;
            SystemDriveSpaceUsed = SystemDriveSpaceTotal - SystemDriveSpaceFree;
            SystemDriveSpaceTotalGB = SystemDriveSpaceTotal / 1024 / 1024 / 1024;
            SystemDriveSpaceFreeGB = SystemDriveSpaceFree / 1024 / 1024 / 1024;
            SystemDriveSpaceUsedGB = SystemDriveSpaceTotalGB - SystemDriveSpaceFreeGB;
            SystemDriveSpaceFreePercent = unchecked((int)((100 * SystemDriveSpaceFree) / SystemDriveSpaceTotal));
        }
    }
}

