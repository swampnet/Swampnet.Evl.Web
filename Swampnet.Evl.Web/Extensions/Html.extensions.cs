using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web
{
    public static class HtmlExtensions
    {
        public static IHtmlContent FormatNewLines(this IHtmlHelper helper, object input)
        {
            var text = input.ToString();
            return helper.Raw(helper.Encode(text).Replace("\n", "<br />"));
        }
    }
}
