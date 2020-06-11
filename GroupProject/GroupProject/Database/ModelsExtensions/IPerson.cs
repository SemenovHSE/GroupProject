using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Database.ModelsExtensions
{
    public interface IPerson
    {
        int Id { get; set; }

        string FullName { get; set; }

        string PhoneNumber { get; set; }
    }
}
