using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Authentication
{
    public class AuthenticationHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(this.Authenticate);
        }


        private void Authenticate(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;
            IAuthentication authentication = DependencyResolver.Current.GetService<IAuthentication>();
            authentication.HttpContext = context;
            context.User = authentication.CurrentUser;
        }


        public void Dispose() { }
    }
}