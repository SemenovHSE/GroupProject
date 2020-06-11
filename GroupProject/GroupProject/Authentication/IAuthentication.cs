using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GroupProject.Database.ModelsExtensions;

namespace GroupProject.Authentication
{
    public interface IAuthentication
    {
        HttpContext HttpContext { get; set; }

        IPerson Login(string phoneNumber, string password, bool isPersistent);

        void Logout();

        IPrincipal CurrentUser { get; }
    }
}
