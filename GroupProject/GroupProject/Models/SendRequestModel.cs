using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class SendRequestModel
    {
        [Display(Name = "Тема")]
        public string Theme { get; set; }

        [Display(Name = "Заявка")]
        public string Body { get; set; }

        [Display(Name = "Файлы")]
        public HttpPostedFileBase[] Files { get; set; }
    }
}