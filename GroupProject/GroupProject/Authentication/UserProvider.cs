using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using GroupProject.Database.Context;

namespace GroupProject.Authentication
{
    public class UserProvider : IPrincipal
    {
        private UserIdentity userIdentity;

        public IIdentity Identity
        {
            get
            {
                return userIdentity;
            }
        }


        public UserProvider(string name, IDatabaseContext databaseContext)
        {
            userIdentity = new UserIdentity();
            userIdentity.Initialize(name, databaseContext);
        }
        

        public override string ToString()
        {
            return userIdentity.Name;
        }


        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}