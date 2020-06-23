using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class InformationBlockGridModel
    {
        [Display(Name = "Идентификатор новости")]
        public int Id { get; set; }

        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Дата создания")]
        public string Date { get; set; }
    }
}