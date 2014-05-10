using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management.Automation;
namespace NuveStack.PowerShell
  {
        public class PowerShellResult
        {
        public PowerShellResult()
        {
        }

        public DateTime CommandStartTimeUtc { get; set; }
        public DateTime CommandEndTimeUtc { get; set; }
        public string FailureMessage { get; set; }

        private Exception _Exception; 
        public Exception Exception 
        {
            get
            {
                return _Exception;
            }
            set
            {
                _Exception = value;
                if (_Exception != null)
                {
                    this.FailureMessage += Exception.Message;
                }
            }
        }


        public PSCommand Command { get; set; }

        private List<ErrorRecord> _Errors = new List<ErrorRecord>();
        public List<ErrorRecord> Errors
        {
            get
            {
                return _Errors;
            }
        }

        private List<WarningRecord> _Warnings = new List<WarningRecord>();
        public List<WarningRecord> Warnings
        {
            get
            {
                return _Warnings;
            }
        }

        public System.Collections.ObjectModel.Collection<PSObject> Output { get; set; }
        }
}
