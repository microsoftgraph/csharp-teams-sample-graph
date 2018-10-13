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

        public static MvcHtmlString MaybeShowLabeledTextBox<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, bool show, Expression<Func<TModel, TProperty>> member)
        {
            if (show)
            {
                return MvcHtmlString.Create("<tr>").Concat(
                    MvcHtmlString.Create("<td>"),
                    htmlHelper.LabelFor(member),
                    MvcHtmlString.Create("</td>"),
                    MvcHtmlString.Create("<td>"),
                    htmlHelper.TextBoxFor(member),
                    MvcHtmlString.Create("</td>"),
                    MvcHtmlString.Create("</tr>")
                    );
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString MaybeShowDropdown<TModel, TItem, TProperty>
             (this HtmlHelper<TModel> htmlHelper, bool show, IEnumerable<TItem> items,
                        Func<TItem, string> member1, Func<TItem, string> member2, Expression<Func<TModel, TProperty>> selection)
        {
            if (show)
            {
                var listitems = items.Select(t => new SelectListItem() { Text = member1(t), Value = member2(t) });
                return MvcHtmlString.Create("<tr>").Concat(
                    MvcHtmlString.Create("<td>"),
                    htmlHelper.LabelFor(selection),
                    MvcHtmlString.Create("</td>"),
                    MvcHtmlString.Create("<td>"),
                    htmlHelper.DropDownListFor(selection, listitems),
                    MvcHtmlString.Create("</td>"),
                    MvcHtmlString.Create("</tr>")
                    );
            }
            return MvcHtmlString.Empty;
        }


        public static MvcHtmlString MaybeShowResultsTable<TModel, TItem>
           (this HtmlHelper<TModel> htmlHelper, bool show, IEnumerable<TItem> items, 
            Func<TItem, string> member1, Func<TItem, string> member2)
        {
            if (show)
            {
                var listitems = items.Select(t => new SelectListItem() { Text = member1(t), Value = member2(t) });
                return MvcHtmlString.Create("<br/>").Concat(
                    htmlHelper.Partial("_ResultsTable", listitems)
                    );
            }
            return MvcHtmlString.Empty;
        }

    }
}