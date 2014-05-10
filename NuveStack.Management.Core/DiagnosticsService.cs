using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation.Runspaces;

using NuveStack.Core.Model;
using NuveStack.PowerShell;

namespace NuveStack.Management.Core
{
    public class DiagnosticsService
    {
        public Diagnostics GetPowerShellEnvironment()
        {
            var cmdResults = NuveStack.PowerShell.PowerShell.ExecutePowershellCommand(Commands.GetEnvironment);
            var resultTable = cmdResults.Output[0].BaseObject.As<System.Collections.Hashtable>();
            
            var result = new Diagnostics()
            {
                CLRVersion = string.Format("{0}", resultTable["CLR Version"]),
                PSVersion = string.Format("{0}", resultTable["PS Version"]),
                MachineName = string.Format("{0}", resultTable["Machine Name"]),
                Architecture = string.Format("{0}", resultTable["Architecture"])
            };
            return result;
        }
    }
}
