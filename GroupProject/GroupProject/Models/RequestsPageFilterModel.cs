using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GroupProject.Enums;

namespace GroupProject.Models
{
    public class RequestsPageFilterModel
    {
        public bool UserRequests { get; set; }

        public int? StatusId { get; set; }

        public string Theme { get; set; }

        public int Page { get; set; }

        public RequestsPageFilterModel()
        {
            UserRequests = true;
            StatusId = (int)RequestStatus.All;
            Theme = string.Empty;
            Page = 1;
        }
    }
}