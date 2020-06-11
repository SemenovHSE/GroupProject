using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Authentication
{
    public enum AuthenticationTypeEnum
    {
        // Сотрудник УК
        Employee = 0,
        
        // Житель
        Resident = 1,

        // Аноним
        Anonymous = 2,
    }
}