using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xamlFrame.Lib.Extensions;


namespace xamlFrame.Lib.System
{
    class WindowsSecurityCenterProduct
    {
        public string DisplayName { get; }
        public string ProtectionStatus { get; }
        public string DefinitionStatus { get; }
        public bool StatusIsGood { get; }

        public WindowsSecurityCenterProduct(
            string displayName,
            string protectionStatus,
            string definitionStatus,
            bool statusIsGood)
        {
            DisplayName = displayName;
            ProtectionStatus = protectionStatus;
            DefinitionStatus = definitionStatus;
            StatusIsGood = statusIsGood;
        }
    }


    class WindowsSecurityCenterAVStatus
    {
        private readonly CIMLocalhostProxy CIMDataProvider;
        public List<WindowsSecurityCenterProduct> Products = new List<WindowsSecurityCenterProduct>();

        public WindowsSecurityCenterAVStatus()
        {
            CIMDataProvider = new CIMLocalhostProxy();
            string cimClassName;

            cimClassName = "AntiVirusProduct";
            string cimNamespace = @"root\SecurityCenter2";
            foreach (CimInstance av in CIMDataProvider.GetEnumeratedInstances(className: cimClassName, cimNamespace: cimNamespace))
            {
                string displayName = av.CimInstanceProperties["displayName"].Value<string>();
                int productState = av.CimInstanceProperties["productState"].Value<int>();
                var decodedState = DecodeWSCProductState(productState);
                Products.Add(new WindowsSecurityCenterProduct(
                    displayName: displayName,
                    protectionStatus: decodedState.Item1,
                    definitionStatus: decodedState.Item2,
                    statusIsGood: decodedState.Item3
                 ));
            }
        }

        public Tuple<string, string, bool> DecodeWSCProductState(int productState)
        {
            string realTimeProtectionStatus;
            string definitionStatus;
            bool stateIsGood = false;
            bool defnsAreGood = false;
            bool statusIsGood = false;

            // convert to productState binary, add an additional '0's left if necesarry
            string bin = Convert.ToString(productState, 2).PadLeft(20, '0');
            // $WSC_SECURITY_PROVIDER = bin.Substring(0,16) # WSC_SECURITY_PROVIDER is a binary-flag value, ignore for now
            string WSC_SECURITY_PRODUCT_STATE = bin.Substring(16, 2);
            string WSC_SECURITY_SIGNATURE_STATUS = bin.Substring(18, 2);

            switch (WSC_SECURITY_PRODUCT_STATE) {
                case "00":
                    realTimeProtectionStatus = "On";
                    stateIsGood = true;
                    break;
                case "01":
                    realTimeProtectionStatus = "Off";
                    break;
                case "10":
                    realTimeProtectionStatus = "Snoozed";
                    break;
                case "11":
                    realTimeProtectionStatus = "Expired";
                    break;
                default:
                    realTimeProtectionStatus = "Unknown";
                    break;
            }; // As per https://msdn.microsoft.com/en-us/library/jj155490(v=vs.85).aspx

            switch (WSC_SECURITY_SIGNATURE_STATUS) {
                case "00":
                    definitionStatus = "Up to Date";
                    defnsAreGood = true;
                    break;
                case "01":
                    definitionStatus = "Out of Date";
                    break;
                default:
                    definitionStatus = "Unknown";
                break;
            } // As per https://msdn.microsoft.com/en-us/library/jj155491(v=vs.85).aspx

            if (stateIsGood && defnsAreGood) {
                statusIsGood = true;
            }
            return new Tuple<string, string, bool>(realTimeProtectionStatus, definitionStatus, statusIsGood);
        }
    }
}
