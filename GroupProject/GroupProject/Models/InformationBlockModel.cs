using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Models
{
    public class InformationBlockModel
    {
        [Display(Name = "Идентификатор новости")]
        public int? Id { get; set; }

        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Новость")]
        public string Body { get; set; }

        [Display(Name = "Прикрепленные файлы")]
        public IEnumerable<string> Files { get; set; }

        [Display(Name = "Идентификаторы выбранных тегов")]
        public IEnumerable<string> TagsIds { get; set; }

        [Display(Name = "Теги")]
        public IEnumerable<SelectListItem> Tags { get; set; }

        [Display(Name = "Дата создания")]
        public string Date { get; set; }

        public InformationBlockModel()
        {
            Files = new List<string>();
            TagsIds = new List<string>();
            Tags = new List<SelectListItem>();
        }
    }
}