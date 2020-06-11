using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class RequestModel
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public string Theme { get; set; }

        public string Status { get; set; }

        public string Date { get; set; }

        public string ViewAction { get; set; }
    }
}