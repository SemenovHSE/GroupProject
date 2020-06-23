using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class MenuTagsPageModel
    {
        public IEnumerable<MenuTagModel> Tags { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}