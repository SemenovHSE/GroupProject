using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class RequestProgressModel
    {
        public RequestModel Request { get; set; }

        public IEnumerable<ReplyModel> Replies { get; set; }
    }
}