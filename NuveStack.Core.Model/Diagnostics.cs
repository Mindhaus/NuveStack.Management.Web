using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuveStack.Core.Model
{
    public class Diagnostics
    {
        public string CLRVersion { get; set; }
        public string PSVersion { get; set; }
        public string MachineName { get; set; }
        public string Architecture { get; set; }
    }
}