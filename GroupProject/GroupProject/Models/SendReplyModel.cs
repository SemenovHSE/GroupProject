using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class SendReplyModel
    {
        [Display(Name = "Идентификатор заявки")]
        public int RequestId { get; set; }

        [Display(Name = "Ответ")]
        public string Body { get; set; }

        [Display(Name = "Файлы")]
        public HttpPostedFileBase[] Files { get; set; }
    }
}