using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class ViewInformationBlockModel
    {
        [Display(Name = "Идентификатор новости")]
        public int Id { get; set; }

        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Новость")]
        public string Body { get; set; }

        [Display(Name = "Файлы")]
        public IEnumerable<string> Files { get; set; }

        [Display(Name = "Теги")]
        public IEnumerable<string> Tags { get; set; }

        [Display(Name = "Дата создания")]
        public string Date { get; set; }

        public ViewInformationBlockModel()
        {
            Files = new List<string>();
            Tags = new List<string>();
        }
    }
}