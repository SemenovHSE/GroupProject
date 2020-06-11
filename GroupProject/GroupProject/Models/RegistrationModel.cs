using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class RegistrationModel
    {
        [Display(Name = "Фамилия")]
        [Required]
        public string Surname { get; set; }

        [Display(Name = "Имя")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Отчество")]
        [Required]
        public string Patronymic { get; set; }

        [Display(Name = "Номер мобильного телефона")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Адрес")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Подъезд")]
        [Required]
        public int Entrance { get; set; }
        
        [Display(Name = "Квартира")]
        [Required]
        public int SettingNumber { get; set; }
    }
}