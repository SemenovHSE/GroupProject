using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class RequestsPageModel
    {
        public IEnumerable<RequestModel> Requests { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}