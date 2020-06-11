using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using GroupProject.Database.Context;
using GroupProject.Database.ModelsExtensions;
using GroupProject.Database.ModelsGenerated;

namespace GroupProject.Authentication
{
    public class UserIdentity : IIdentity, IUserIdentity
    {
        public IPerson User { get; set; }

        public string AuthenticationType
        {
            get
            {
                if (User is Resident)
                {
                    return AuthenticationTypeEnum.Resident.ToString();
                }
                if (User is Employee)
                {
                    return AuthenticationTypeEnum.Employee.ToString();
                }
                return AuthenticationTypeEnum.Anonymous.ToString();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.FullName;
                }
                return "anonym";
            }
        }


        public void Initialize(string phoneNumber, IDatabaseContext databaseContext)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                User = databaseContext.GetUser(phoneNumber);
            }
        }
    }
}