using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using NuveStack.Core.Extensions;

namespace NuveStack.Core
{
    public static class Configuration
    {
        public static NameValueCollection AppSettings
        {
            get 
            {
                return System.Configuration.ConfigurationManager.AppSettings;
            }
        }

        public static string StackUsersContainer
        {
            get
            {
                return AppSettings.GetValue<string>("StackUsersContainer");
            }
        }

        public static string PackageTypesContainer
        {
            get
            {
                return AppSettings.GetValue<string>("PackageTypesContainer");
            }
        }

        public static string DomainServer
        {
            get
            {
                return AppSettings.GetValue<string>("DomainServer");
            }
        }

        public static string DomainUser
        {
            get
            {
                return AppSettings.GetValue<string>("DomainUser");
            }
        }

        public static string DomainUserPassword
        {
            get
            {
                return AppSettings.GetValue<string>("DomainUserPassword");
            }
        }

    }
}
