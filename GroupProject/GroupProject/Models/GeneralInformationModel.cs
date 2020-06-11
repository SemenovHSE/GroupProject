using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class GeneralInformationModel
    {
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Квартира")]
        public int SettingNumber { get; set; }

        [Display(Name = "Подъезд")]
        public int Entrance { get; set; }

        [Display(Name = "Площадь")]
        public double? Size { get; set; }

        [Display(Name = "Количество комнат")]
        public int? RoomsNumber { get; set; }
    }
}