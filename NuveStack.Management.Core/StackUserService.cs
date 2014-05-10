using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NuveStack.Core;
using NuveStack.Core.Extensions;
using NuveStack.Core.Model;

namespace NuveStack.Management.Core
{
    public class StackUserService
    {
        public StackUserService()
        {
        }
     
        public IEnumerable<StackUser> GetUsers(string tenantId)
        {
            var searchBase = Configuration.StackUsersContainer;
            var cmd = PowerShell.Commands.GetUsers(searchBase);
            var getUserResult = PowerShell.PowerShell.ExecutePowershellCommand(cmd);

            if (getUserResult.FailureMessage.IsNotEmpty())
            {
                throw new NuveStackApplicationException(getUserResult.FailureMessage, getUserResult.Exception);
            }

            var result = new List<StackUser>();

           foreach(var user in getUserResult.Output)
           {
            result.Add(new StackUser()
            {
                AccountName = user.Properties["sAMAccountName"].Value.As<string>(),
                DisplayName = user.Properties["displayName"].Value.As<string>(),
               Upn = user.Properties["userPrincipalName"].Value.As<string>(),
                PackageType = FindPackage(user.Properties["memberOf"])
            });
           }
            return result;
        }

        private string FindPackage(System.Management.Automation.PSPropertyInfo pSPropertyInfo)
        {
            if (pSPropertyInfo.Value == null)
                return null;

            var list = (IEnumerable)pSPropertyInfo.Value;
            foreach(string group in list)
            {
                if (group.EndsWith(Configuration.PackageTypesContainer))
                {
                    return group.Substring(3, group.IndexOf(",") - 3);
                }
            }
            
            return null;
        }

        public StackUser GetUser(string id, string tenantId)
        {
            var cmd = PowerShell.Commands.GetUser(id);
            var getUserResult = PowerShell.PowerShell.ExecutePowershellCommand(cmd);

            if (getUserResult.FailureMessage.IsNotEmpty())
            {
                throw new NuveStackApplicationException(getUserResult.FailureMessage);
            }

            var result = new StackUser()
            {
                AccountName = getUserResult.Output[0].Properties["sAMAccountName"].Value.As<string>(),
                DisplayName = getUserResult.Output[0].Properties["DisplayName"].Value.As<string>(),
                Upn = getUserResult.Output[0].Properties["userPrincipalName"].Value.As<string>(),
                PackageType = FindPackage(getUserResult.Output[0].Properties["memberOf"])
            };
            return result;  
        }

        public StackUser AddUser(StackUser newUser)
        {
            //required: NewPassword, AccountName, Tenant?
            var cmd = PowerShell.Commands.CreateUser(newUser);
            var createUserResult = PowerShell.PowerShell.ExecutePowershellCommand(cmd);

            var group =  "Os-Only";
            if (newUser.PackageType != "Os-Only")
            {
                group = "Os-Office";
            }

            group = string.Format("CN={0},{1}", group, Configuration.PackageTypesContainer);
            var groupCmd = PowerShell.Commands.AddUserToGroup(newUser.AccountName, group);
            var addGroupResult = PowerShell.PowerShell.ExecutePowershellCommand(groupCmd);

            if (string.IsNullOrEmpty(createUserResult.FailureMessage))
            {
                return newUser;
            }
            else return null;
        }

        public void ChangePackage(string id, string newPackage, string oldPackage)
        {
            var oldGroup = string.Format("CN={0},{1}", oldPackage, Configuration.PackageTypesContainer);
            var removeCmd = PowerShell.Commands.RemoveUserFromGroup(id, oldGroup);
            var removeGroupResult = PowerShell.PowerShell.ExecutePowershellCommand(removeCmd);

            if (removeGroupResult.FailureMessage.IsNotEmpty())
            {
                throw new NuveStackApplicationException(removeGroupResult.FailureMessage);
            }

            var newGroup = "Os-Only";
            if (string.Compare(newPackage, "Os-Only", true) != 0)
            {
                newGroup = "Os-Office";
            }

            newGroup = string.Format("CN={0},{1}", newGroup, Configuration.PackageTypesContainer);

            var addCmd = PowerShell.Commands.AddUserToGroup(id, newGroup);
            var addToGroupResult = PowerShell.PowerShell.ExecutePowershellCommand(addCmd);

            if (removeGroupResult.FailureMessage.IsNotEmpty())
            {
                throw new NuveStackApplicationException(addToGroupResult.FailureMessage);
            }
        }
    }
}
