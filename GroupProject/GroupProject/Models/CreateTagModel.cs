using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class CreateTagModel
    {
        [Display(Name = "Название тега")]
        public string Name { get; set; }
    }
}