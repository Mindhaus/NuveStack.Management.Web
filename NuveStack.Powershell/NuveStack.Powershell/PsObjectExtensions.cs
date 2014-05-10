using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management.Automation;

namespace NuveStack.PowerShell
{
    public static class PsObjectExtensions
    {
        public static T Value<T>(this PSObject source, string valueName)
        {
          if (source == null || source.Members == null)
            return default(T);

          var value = source.Members[valueName];

          return value == null ? default(T) : value.As<T>();
        }

        public static T As<T>(this object o)
        {
            T value = default(T);

            // given an empty string, return the default value
            if (o is String && string.IsNullOrEmpty(o.ToString())) return value;

            try
            {
                if (typeof(T) == typeof(String))
                {
                    return (T)(o.ToString() as object);
                }
                if (value is ValueType)
                {
                    // TODO: HAS 07/10/2011 Complete tests for all default value types.
                    if (value is Int32) return (T)(Convert.ToInt32(o) as object);
                    if (value is Double) return (T)(Convert.ToDouble(o) as object);
                    if (value is Decimal) return (T)(Convert.ToDecimal(o) as object);
                    if (value is Boolean) return (T)(Convert.ToBoolean(o) as object);
                    if (value is Guid) return (T)(new Guid(o.ToString()) as object);
                    if (typeof(T) == typeof(DateTime)) return (T)(DateTime.Parse(o.ToString()) as object);
                }
                value = (T)o;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Conversion Error: {0}", e));
            }
            return value;
        }
 }
}
