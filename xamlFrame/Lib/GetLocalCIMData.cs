using System.Collections.Generic;
using Microsoft.Management.Infrastructure;

namespace xamlFrame.Lib
{
    class GetLocalCIMData
    {
        public static readonly string LocalComputer = "localhost";
        public static readonly string DefaultNamepace = @"root\cimv2";

        public IEnumerable<CimInstance> GetDataFromQuery(string QueryString)
        {
            return GetDataFromQuery(QueryString, DefaultNamepace);
        }

        public IEnumerable<CimInstance> GetDataFromQuery(string QueryString, string Namespace)
        {
            CimSession mySession = CimSession.Create(LocalComputer);
            return mySession.QueryInstances(Namespace, "WQL", QueryString);
        }

        public CimClass GetClass(string ClassName)
        {
            return GetClass(ClassName, DefaultNamepace);
        }

        public CimClass GetClass(string ClassName, string Namespace)
        {
            // className = "Win32_LogicalDisk";
            CimSession mySession = CimSession.Create(LocalComputer);
            return mySession.GetClass(namespaceName: Namespace, className: ClassName);
        }

        public IEnumerable<CimInstance> GetEnumeratedInstances(string ClassName)
        {
            return GetEnumeratedInstances(ClassName, DefaultNamepace);
        }

        public IEnumerable<CimInstance> GetEnumeratedInstances(string ClassName, string Namespace)
        {
            CimSession mySession = CimSession.Create(LocalComputer);
            return mySession.EnumerateInstances(namespaceName: Namespace, className: ClassName);
        }

    }
}
