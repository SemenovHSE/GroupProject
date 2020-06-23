using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class RequestModel
    {
        [Display(Name = "Идентификатор запроса")]
        public int RequestId { get; set; }

        [Display(Name = "ФИО жителя")]
        public string ResidentFullName { get; set; }

        [Display(Name = "Тема")]
        public string Theme { get; set; }

        [Display(Name = "Заявка")]
        public string Body { get; set; }

        [Display(Name = "Путь к папке с файлами")]
        public IEnumerable<string> Files { get; set; }

        [Display(Name = "Статус")]
        public StatusModel Status { get; set; }

        [Display(Name = "Дата и время создания")]
        public string Date { get; set; }

        public RequestModel()
        {
            Files = new List<string>();
        }
    }
}