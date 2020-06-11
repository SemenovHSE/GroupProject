using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GroupProject.Authentication;
using GroupProject.Database.ModelsExtensions;
using GroupProject.Database.Context;
using GroupProject.Logging;
using Unity;

namespace GroupProject.Controllers
{
    public class BaseController : Controller
    {
        [Dependency]
        public IAuthentication Authentication { get; set; }

        [Dependency]
        public IDatabaseContext DatabaseContext { get; set; }

        [Dependency]
        public Logger Logger { get; set; }

        public IPerson CurrentUser
        {
            get
            {
                return ((IUserIdentity)Authentication.CurrentUser.Identity).User;
            }
        }
    }
}