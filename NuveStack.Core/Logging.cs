using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace NuveStack.Core

{
    public static class Logging
    {
        static Logging()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void LogException(this object o, Exception ex)
        {
            log4net.LogManager.GetLogger(o.GetType()).Error(ex);
        }

        public static void LogInfo(this object o, string msg)
        {
            log4net.LogManager.GetLogger(o.GetType()).Info(msg);
        }
    }
}
