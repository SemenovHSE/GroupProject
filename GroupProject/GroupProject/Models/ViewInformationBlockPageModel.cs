using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class ViewInformationBlockPageModel
    {
        public IEnumerable<ViewInformationBlockModel> InformationBlocks { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}