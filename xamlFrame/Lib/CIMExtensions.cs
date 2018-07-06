using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Management.Infrastructure;

namespace xamlFrame.Lib
{
    /// <summary>
    /// This class extends the system CIM methods, to allow easier retrieval of correctly-typed values
    /// </summary>
    public static class CIMExtensions
    {
        /// <summary>
        /// This extension adds a Generic method of obtaining a variable of a given type from its CIM Value
        /// </summary>
        public static T Value<T>(this CimProperty cp, bool suppressErrorThrow = true)
        {
            dynamic val;
            string valueString = cp.Value.ToString();

            switch (cp.CimType)
            {
                case CimType.Boolean:
                    val = bool.Parse(valueString);
                    break;
                case CimType.UInt8:
                case CimType.UInt16:
                case CimType.SInt8:
                case CimType.SInt16:
                case CimType.SInt32:
                    val = int.Parse(valueString);
                    if (typeof(T) == typeof(long))
                    {
                        val = (long)val;
                    }
                    break;
                case CimType.UInt32:
                    val = uint.Parse(valueString);
                    if (typeof(T) == typeof(long))
                    {
                        val = (long)val;
                    }
                    break;
                case CimType.SInt64:
                    val = long.Parse(valueString);
                    break;
                case CimType.UInt64:
                    val = ulong.Parse(valueString);
                    break;
                case CimType.Real32:
                    val = float.Parse(valueString);
                    break;
                case CimType.Real64:
                    val = double.Parse(valueString);
                    break;
                case CimType.DateTime:
                    val = DateTime.Parse(valueString);
                    break;
                case CimType.Char16:
                    val = char.Parse(valueString);
                    if (typeof(T) == typeof(string))
                    {
                        val = (string)val;
                    }
                    break;
                default:
                    val = cp.Value.ToString();
                    break;
            }

            if (val.GetType() == typeof(T))
            {
                return (T)val;
            }
            else
            {
                if (suppressErrorThrow)
                {
                    return default(T);
                }
                else
                {
                    throw new InvalidCastException("CIMProperty is not the specified Type. Specified: " + typeof(T) + ", Actual: " + val.Gettype());
                }
            }

        }

        /// <summary>
        /// Extracts an integer from the Value.
        /// Extracting 'int's is a very common operation, so has a dedicated method.
        /// This is a nullable equivalent to calling cp.Value<int>
        /// </summary>
        public static int? ValueAsInt(this CimProperty cp)
        {
            string valueString = cp.Value.ToString();
            if (int.TryParse(valueString, out int val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }
    }
}
