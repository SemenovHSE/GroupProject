using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class ReplyModel
    {
        [Display(Name = "Идентификатор ответа")]
        public int ReplyId { get; set; }

        [Display(Name = "Ответ")]
        public string Body { get; set; }

        [Display(Name = "Путь к папке с файлами")]
        public IEnumerable<string> Files { get; set; }

        [Display(Name = "Дата и время ответа")]
        public string Date { get; set; }

        [Display(Name = "ФИО сотрудника")]
        public string EmployeeFullName { get; set; }

        public ReplyModel()
        {
            Files = new List<string>();
        }
    }
}