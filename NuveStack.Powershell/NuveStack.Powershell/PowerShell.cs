using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace NuveStack.PowerShell
{
    public static class PowerShell
    {
        public static PowerShellResult ExecutePowershellCommand(PSCommand commands, IEnumerable<object> input = null)
        {
            var results = new PowerShellResult();
            try
            {
                using (var runspace = RunspaceFactory.CreateRunspace())
                {
                    runspace.Open();
                    using (var shell = System.Management.Automation.PowerShell.Create())
                    {
                        shell.Runspace = runspace;
                        shell.Commands = commands;
                        results.CommandStartTimeUtc = DateTime.UtcNow;

                        results.Output = input == null ? shell.Invoke() : shell.Invoke(input);

                        results.CommandEndTimeUtc = DateTime.UtcNow;

                        results.Errors.AddRange(shell.Streams.Error);

                        if (shell.InvocationStateInfo.State == PSInvocationState.Failed)
                            results.FailureMessage = shell.InvocationStateInfo.Reason.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                results.Exception = ex;
            }
            return results;
        }
    }
}
