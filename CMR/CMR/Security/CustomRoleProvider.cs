using CMR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CMR.Security
{
    public class CustomRoleProvider : RoleProvider
    {
        CRMContext db = new CRMContext();
        public override bool IsUserInRole(string username, string roleName)
        {
            Account user = db.Accounts.SingleOrDefault(u => u.userName == username);
            if (user == null)
                return false;
            Role userInRole = db.Roles.SingleOrDefault(r => r.roleId == user.roleId);
            if(userInRole.roleName!=roleName)
                return false;
            return true;
        }

        public override string[] GetRolesForUser(string username)
        {
            Account user = db.Accounts.SingleOrDefault(u => u.userName == username);
            if (user == null)
                return new string[] { };

            var roles = from r in db.Roles
                        where user.roleId == r.roleId
                        select r.roleName;

            if (roles != null)
                return roles.ToArray();
            else
            {
                return new string[] { };
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }


        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}