using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GroupProject.Models;

namespace GroupProject.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PageInfo pageInfo)
        {
            StringBuilder result = new StringBuilder();
            for (int page = 1; page <= pageInfo.TotalPages; page++)
            {
                TagBuilder tag = new TagBuilder("div");
                string stringPage = page.ToString();
                tag.Attributes.Add(new KeyValuePair<string, string>("page", stringPage));
                tag.InnerHtml = stringPage;
                if (page == pageInfo.PageNumber)
                {
                    tag.AddCssClass("btn-primary");
                    tag.AddCssClass("selected");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag);
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}