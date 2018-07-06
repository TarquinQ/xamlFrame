using System.Collections.Generic;
using System.Linq;
using Microsoft.Management.Infrastructure;

namespace xamlFrame.Lib
{
    class GetLocalCIMData
    {
        public static readonly string LocalComputer = "localhost";
        public static readonly string DefaultCIMNamespace = @"root\cimv2";

        public CimClass GetClass(string className)
        {
            return GetClass(className: className, cimNamespace: DefaultCIMNamespace);
        }

        public CimClass GetClass(string className, string cimNamespace)
        {
            CimSession mySession = CimSession.Create(LocalComputer);
            return mySession.GetClass(namespaceName: cimNamespace, className: className);
        }

        // CIM Query methods

        public IEnumerable<CimInstance> GetDataFromQuery(string queryString)
        {
            return GetDataFromQuery(queryString: queryString, cimNamespace: DefaultCIMNamespace, firstInstance: out _);
        }

        public IEnumerable<CimInstance> GetDataFromQuery(string queryString, string cimNamespace)
        {
            return GetDataFromQuery(queryString: queryString, cimNamespace: cimNamespace, firstInstance: out _);
        }

        public IEnumerable<CimInstance> GetDataFromQuery(string queryString, out CimInstance firstInstance)
        {
            return GetDataFromQuery(queryString: queryString, cimNamespace: DefaultCIMNamespace, firstInstance: out firstInstance);
        }

        /// <summary>
        /// This is the base function for executing CIM Queries
        /// </summary>
        public IEnumerable<CimInstance> GetDataFromQuery(string queryString, string cimNamespace, out CimInstance firstInstance)
        {
            CimSession mySession = CimSession.Create(LocalComputer);
            IEnumerable<CimInstance> instances = mySession.QueryInstances(namespaceName: cimNamespace, queryDialect: "WQL", queryExpression: queryString);
            firstInstance = instances.FirstOrDefault();
            return instances;
        }

        // Enumerated Instances methods

        public IEnumerable<CimInstance> GetEnumeratedInstances(string className)
        {
            return GetEnumeratedInstances(className: className, cimNamespace: DefaultCIMNamespace, firstInstance: out _);
        }

        public IEnumerable<CimInstance> GetEnumeratedInstances(string className, string cimNamespace)
        {
            return GetEnumeratedInstances(className: className, cimNamespace: cimNamespace, firstInstance: out _);
        }

        public IEnumerable<CimInstance> GetEnumeratedInstances(string className, out CimInstance firstInstance)
        {
            return GetEnumeratedInstances(className: className, cimNamespace: DefaultCIMNamespace, firstInstance: out firstInstance);
        }

        /// <summary>
        /// This is the base function for getting CIM EnumeratedInstances
        /// </summary>
        public IEnumerable<CimInstance> GetEnumeratedInstances(string className, string cimNamespace, out CimInstance firstInstance)
        {
            CimSession mySession = CimSession.Create(LocalComputer);
            IEnumerable<CimInstance> instances = mySession.EnumerateInstances(namespaceName: cimNamespace, className: className);
            firstInstance = instances.FirstOrDefault();
            return instances;
        }
    }
}
