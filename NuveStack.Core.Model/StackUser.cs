using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuveStack.Core.Model
{
    public class StackUser : IApiReferenceObject
    {
        public string Id 
        {
            get
            {
                return this.AccountName;
            }
            set
            {
                this.AccountName = value;
            }
        }
        public string Upn { get; set; }
        //public string TenantId { get; set; }
        public string DisplayName { get; set; }
        public string PackageType { get; set; }
        //public string Ou { get; set; }
        public string AccountName { get; set; }
        public string NewPassword { get; set; }
    }
}