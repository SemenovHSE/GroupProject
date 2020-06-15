using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class PersonalCabinetTagModel
    {
        [Display(Name = "Идентификатор тега")]
        public int TagId { get; set; }

        [Display(Name = "Наименование тега")]
        public string Name { get; set; }

        [Display(Name = "Подписка на тег")]
        public bool ResidentIsSubscribed { get; set; }
    }
}