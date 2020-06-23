using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class InformationBlockPageFilterModel
    {
        public bool UserTags { get; set; }

        public int Page { get; set; }

        public InformationBlockPageFilterModel()
        {
            UserTags = true;
            Page = 1;
        }
    }
}