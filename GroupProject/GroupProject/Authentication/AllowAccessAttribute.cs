using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GroupProject.Database.ModelsExtensions;
using GroupProject.Database.ModelsGenerated;

namespace GroupProject.Authentication
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AllowAccessAttribute : ActionFilterAttribute
    {
        private AuthenticationTypeEnum[] AuthenticationTypes { get; set; }

        public AllowAccessAttribute(AuthenticationTypeEnum[] allowedAuthenticationTypes)
        {
            AuthenticationTypes = allowedAuthenticationTypes;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IPerson currentUser = ((IUserIdentity)filterContext.HttpContext.User.Identity).User;
            AuthenticationTypeEnum authenticationType = GetAuthenticationType(currentUser);
            if (!AuthenticationTypes.Contains(authenticationType))
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }
        }

        private AuthenticationTypeEnum GetAuthenticationType(IPerson user)
        {
            if (user is Employee)
            {
                return AuthenticationTypeEnum.Employee;
            }
            if (user is Resident)
            {
                return AuthenticationTypeEnum.Resident;
            }
            return AuthenticationTypeEnum.Anonymous;
        }
    }
}