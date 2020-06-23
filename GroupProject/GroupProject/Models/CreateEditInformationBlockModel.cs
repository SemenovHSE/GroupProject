using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class CreateEditInformationBlockModel
    {
        [Display(Name = "Идентификатор новости")]
        public int? Id { get; set; }

        [Display(Name = "Заголовок новости")]
        public string Title { get; set; }

        [Display(Name = "Новость")]
        public string Body { get; set; }

        [Display(Name = "Файлы")]
        public HttpPostedFileBase[] Files { get; set; }

        [Display(Name = "Идентификаторы выбранных тегов")]
        public IEnumerable<string> TagsIds { get; set; }
    }
}