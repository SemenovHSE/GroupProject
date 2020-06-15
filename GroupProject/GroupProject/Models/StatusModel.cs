using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class StatusModel
    {
        [Display(Name = "Идентификатор статуса")]
        public int Id { get; set; }

        [Display(Name = "Наименование статуса")]
        public string Name { get; set; }
    }
}