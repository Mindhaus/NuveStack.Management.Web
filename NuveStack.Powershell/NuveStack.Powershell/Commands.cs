using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

using NuveStack.Core;
using NuveStack.Core.Extensions;
using NuveStack.Core.Model;

namespace NuveStack.PowerShell
{
    public static class Commands
    {
        private readonly static string _DomainServer;
        private readonly static string _DomainUser;
        private readonly static string _DomainUserPassword;
        private readonly static string _NewUserContainer;

        static Commands()
        {
            _DomainServer = Configuration.DomainServer;
            _DomainUser = Configuration.DomainUser;
            _DomainUserPassword = Configuration.DomainUserPassword;
            _NewUserContainer = Configuration.StackUsersContainer;
        }

        public static PSCommand GetEnvironment
        {
            get
            {
                var commands = new PSCommand();
                var command = new Command(EmbeddedResources.GetResourceAsString("diagnostics.ps1"), true);
                commands.AddCommand(command);
                return commands;
            }
        }

        public static PSCommand CreateUser(StackUser user)
        {
            var commands = Commands.ImportADModule;
            var command = new Command("New-ADUser");
            SetAdConnectionParameters(command);

            command.Parameters.Add("SamAccountName", user.AccountName);
            command.Parameters.Add("Name", user.DisplayName);
            command.Parameters.Add("DisplayName", user.DisplayName);

            var securePassword = new System.Security.SecureString();
            user.NewPassword.ToCharArray().Each(securePassword.AppendChar);

            command.Parameters.Add("AccountPassword", securePassword);

            command.Parameters.Add("Enabled", true);
            command.Parameters.Add("Path", _NewUserContainer);
             
            commands.AddCommand(command);
            return commands;
        }

        public static PSCommand AddUserToGroup(string id, string groupName)
        {
            var commands = Commands.ImportADModule;
            var command = new Command("Add-ADGroupMember");
            SetAdConnectionParameters(command);

            command.Parameters.Add("Identity", groupName);
            command.Parameters.Add("Member", id);
            commands.AddCommand(command);
            return commands;
        }

        public static PSCommand RemoveUserFromGroup(string id, string groupName)
        {
            var commands = Commands.ImportADModule;
            var command = new Command("Remove-ADGroupMember");
            SetAdConnectionParameters(command);

            command.Parameters.Add("Identity", groupName);
            command.Parameters.Add("Member", id);
            command.Parameters.Add("Confirm", false);
            commands.AddCommand(command);
            return commands;
        }

        public static PSCommand GetUser(string samAccountName)
        {
            var commands = Commands.ImportADModule;
            var command = new Command("Get-ADUser");
            SetAdConnectionParameters(command);
            command.Parameters.Add("Identity", samAccountName);
            command.Parameters.Add("Properties", UserProperties);
            commands.AddCommand(command);
            return commands;
        }

        public static PSCommand GetUsers(string searchRoot, string filter = "*")
        {
            var commands = Commands.ImportADModule;
            var command = new Command("Get-ADUser");
            SetAdConnectionParameters(command);
            command.Parameters.Add("SearchBase", searchRoot);
            command.Parameters.Add("Filter", filter);
            command.Parameters.Add("Properties", UserProperties);
            commands.AddCommand(command);
            return commands;
        }

        public static string[] UserProperties
        { 
            get
            {
                return new string[]{
                    "memberOf",
                    "userPrincipalName",
                    "displayName",
                    "sAMAccountName"
                };
            }
        }

        public static PSCommand ImportADModule
        {
            get
            {
                var commands = new PSCommand();
                var command = new Command("Import-Module");
                command.Parameters.Add("Name", "ActiveDirectory");
                return commands;
            }
        }

        private static void SetAdConnectionParameters(Command command)
        {
            if (_DomainServer.IsNotEmpty())
            {
                command.Parameters.Add("Server", _DomainServer);
                command.Parameters.Add("Credential", GetPsCredential());
            }
        }

        private static PSCredential GetPsCredential()
        {
            var securePassword = new System.Security.SecureString();
            _DomainUserPassword.ToCharArray().Each(securePassword.AppendChar);
            var psCred = new PSCredential(_DomainUser, securePassword);
            return psCred;
        }

    }
}
