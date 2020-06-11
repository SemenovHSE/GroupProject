using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using GroupProject.Database.Context;
using GroupProject.Database.ModelsExtensions;
using GroupProject.Logging;
using Unity;

namespace GroupProject.Authentication
{
    public class ProjectAuthentication : IAuthentication
    {
        [Dependency]
        public static ILogger Logger { get; set; }

        private const string cookieName = "__AUTH_COOKIE";

        public HttpContext HttpContext { get; set; }

        [Dependency]
        public IDatabaseContext DatabaseContext { get; set; }

        private IPrincipal currentUser;
        public IPrincipal CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    try
                    {
                        HttpCookie authenticationCookie = HttpContext.Request.Cookies[cookieName];
                        if (authenticationCookie != null && !string.IsNullOrEmpty(authenticationCookie.Value))
                        {
                            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authenticationCookie.Value);
                            currentUser = new UserProvider(ticket.Name, DatabaseContext);
                        }
                        else
                        {
                            currentUser = new UserProvider(null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Failed authentication: " + ex.Message);
                        currentUser = new UserProvider(null, null);
                    }
                }
                return currentUser;
            }
        }


        public IPerson Login(string phoneNumber, string password, bool isPersistent)
        {
            IPerson user = DatabaseContext.Login(phoneNumber, password);
            if (user != null)
            {
                CreateCookie(phoneNumber, isPersistent);
            }
            return user;
        }


        private void CreateCookie(string phoneNumber, bool isPersistent = false)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, phoneNumber, DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent, string.Empty, FormsAuthentication.FormsCookiePath);
            string ticketEncrypted = FormsAuthentication.Encrypt(ticket);
            HttpCookie authenticationCookie = new HttpCookie(cookieName)
            {
                Value = ticketEncrypted,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(authenticationCookie);
        }


        public void Logout()
        {
            HttpCookie cookie = HttpContext.Response.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Value = string.Empty;
            }
        }
    }
}