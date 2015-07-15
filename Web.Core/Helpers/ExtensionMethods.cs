using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FindMyFamilies.Web.Helpers
{
    public static class ExtensionMethods
    {
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string selectedValue, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            if (string.IsNullOrEmpty(selectedValue))
            {
                selectedValue = string.Empty;
            }
            if (selectList != null)
            {
                foreach (SelectListItem sli in selectList)
                {
                    if (sli.Value.ToLower().Trim() == selectedValue.ToLower().Trim())
                    {
                        sli.Selected = true;
                        break;
                    }
                }
            }
            else
            {
                selectList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "",
                        Value = "",
                        Selected = true
                    }
                };
            }
            return helper.DropDownListFor(expression, selectedValue, selectList, optionLabel, htmlAttributes);
        }


        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string selectedValue, IEnumerable<SelectListItem> selectList, string optionLabel,
            IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(selectedValue))
            {
                selectedValue = string.Empty;
            }
            if (selectList != null)
            {
                foreach (SelectListItem sli in selectList)
                {
                    if (sli.Value.ToLower().Trim() == selectedValue.ToLower().Trim())
                    {
                        sli.Selected = true;
                        break;
                    }
                }
            }
            else
            {
                selectList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "",
                        Value = "",
                        Selected = true
                    }
                };
            }
            return helper.DropDownListFor(expression, selectedValue, selectList, optionLabel, htmlAttributes);
        }
    }
}