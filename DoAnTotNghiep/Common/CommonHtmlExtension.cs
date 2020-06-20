using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnTotNghiep.Common
{
    public static class CommonHtmlExtension
    {
        public static IHtmlContent IframeButton<TModel>(this HtmlHelper<TModel> htmlHelper, string modalHref, string iframeTitle, string iframeUrl, string iconHtml, int width, int height, object htmlAttributes)
        {
            var tagName = "a";
            var tagBuilder = new TagBuilder(tagName);
            string result = "";
            var htmlAttributeDic = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            htmlAttributeDic.Add("data-href", modalHref);
            htmlAttributeDic.Add("data-event", "popup-modal");
            htmlAttributeDic.Add("data-title", iframeTitle);
            htmlAttributeDic.Add("data-url", iframeUrl);
            htmlAttributeDic.Add("data-width", width);
            htmlAttributeDic.Add("data-height", height);
            htmlAttributeDic.Add("href", "javascript:void(0);");
            if (htmlAttributeDic.Count(m => m.Key == "class") > 0)
            {
                htmlAttributeDic["class"] = htmlAttributeDic["class"] + " dsoft-btn-show-popup";
            }
            else
            {
                htmlAttributeDic.Add("class", "dsoft-btn-show-popup");
            }

            tagBuilder.MergeAttributes(htmlAttributeDic);
            tagBuilder.InnerHtml.AppendHtml(string.Concat(iconHtml, iframeTitle));
            using (var sw = new System.IO.StringWriter())
            {
                tagBuilder.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
                result = sw.ToString();
            }
            return new HtmlString(result);
        }

    }
}
