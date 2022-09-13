using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace LH.Net.Mvc.Pagination.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _htmlGenerator;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private string _action;

        public int Page { get; set; }
        public int TotalPages { get; set; }
        public object RouteData { get; set; }

        public PaginationTagHelper(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _action = ViewContext.RouteData.Values["Action"].ToString();

            int previousPage = Page - 1;
            int nextPage = Page + 1;

            string firstPageStyle = Page == 1 ? " disabled" : "";
            string previousPageStyle = Page > 1 ? "" : " disabled";
            string nextPageStyle = Page < TotalPages ? "" : " disabled";
            string lastPageStyle = Page >= TotalPages ? " disabled" : "";

            HtmlContentBuilder builder = new();
            builder.AppendHtml("<ul class=\"pagination\">");

            CreateListItem(ref builder, "First", 1, firstPageStyle);
            CreateListItem(ref builder, "Previous", previousPage, previousPageStyle);

            int startIndex = 1;
            if (TotalPages - 4 <= 0 || TotalPages - 8 <= 0)
            {
                startIndex = 1;
            }
            else if (Page > TotalPages - 4)
            {
                startIndex = TotalPages - 8;
            }
            else if (Page > 4)
            {
                startIndex = Page - 4;
            }

            for (int i = 0; i < 9; i++)
            {
                int pageNo = startIndex + i;

                if (pageNo <= TotalPages)
                {
                    string cssClass = pageNo == Page ? " active" : "";
                    CreateListItem(ref builder, pageNo.ToString(), pageNo, cssClass);
                }
            }

            CreateListItem(ref builder, "Next", nextPage, nextPageStyle);
            CreateListItem(ref builder, "Last", TotalPages, lastPageStyle);

            builder.AppendHtml("</ul>");

            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add(new TagHelperAttribute("aria-label", "Page navigation"));

            output.Content.SetHtmlContent(builder);
        }

        private void CreateListItem(ref HtmlContentBuilder builder, string linkText, int pageNo, string cssClass = "")
        {
            IDictionary<string, object> result = new ExpandoObject();
            if (RouteData != null)
            {
                foreach (PropertyInfo property in RouteData.GetType().GetProperties())
                {
                    if (property.CanRead)
                    {
                        result[property.Name] = property.GetValue(RouteData);
                    }
                }
            }
            result["page"] = pageNo;

            builder.AppendHtml("<li class=\"page-item" + cssClass + "\">");

            TagBuilder link = _htmlGenerator.GenerateActionLink(
                viewContext: ViewContext,
                linkText: linkText,
                actionName: _action,
                controllerName: null,
                protocol: null,
                hostname: null,
                fragment: null,
                routeValues: result,
                htmlAttributes: new { @class = "page-link" }
            );

            builder.AppendHtml(link);
            builder.AppendHtml("</li>");
        }
    }
}
