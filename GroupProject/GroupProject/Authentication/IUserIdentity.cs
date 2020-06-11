using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupProject.Database.ModelsExtensions;

namespace GroupProject.Authentication
{
    public interface IUserIdentity
    {
        IPerson User { get; set; }
    }
}
