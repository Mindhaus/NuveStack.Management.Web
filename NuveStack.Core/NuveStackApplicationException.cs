using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuveStack.Core
{
    public class NuveStackApplicationException : Exception
    {
        public string FriendlyMessage { get; set; }
        public Exception OriginalException { get; set; }

        public NuveStackApplicationException(string friendlyMessage)
        {
            this.FriendlyMessage = friendlyMessage;
        }

        public NuveStackApplicationException(string friendlyMessage, Exception originalException) : this(friendlyMessage)
        {
            this.OriginalException = originalException;
        }
    }
}
