using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class InformationBlockGridPageModel
    {
        public IEnumerable<InformationBlockGridModel> InformationBlocks { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}