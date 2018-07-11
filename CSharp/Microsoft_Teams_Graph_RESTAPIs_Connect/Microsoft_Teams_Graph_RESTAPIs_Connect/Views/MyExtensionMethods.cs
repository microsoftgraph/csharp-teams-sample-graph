using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Views
{
    public static class MyExtensionMethods
    {
        public static MvcHtmlString Concat(this MvcHtmlString first, params MvcHtmlString[] strings)
        {
            return MvcHtmlString.Create(first.ToString() + string.Concat(strings.Select(s => s.ToString())));
        }

        public static MvcHtmlString ShowLabeledTextBox<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper,
            bool show,
            Expression<Func<TModel, TProperty>> expression)
        {
            if (show)
            {
                return MvcHtmlString.Create("<br/>").Concat(
                    htmlHelper.LabelFor(expression),
                    htmlHelper.TextBoxFor(expression));
            }
            return MvcHtmlString.Empty;
        }
    }
}