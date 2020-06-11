using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GroupProject.Models
{
    public class LoginModel
    {
        [Display(Name = "Номер телефона")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        public string Password { get; set; }
    }
}