using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace NetworksManagement.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)
        {
            return items.Select(i => new SelectListItem
            {
                Text = i.GetPropertyValue("Name"),
                Value = i.GetPropertyValue("Id"),
                Selected = i.GetPropertyValue("Id").Equals(selectedValue.ToString())
            });
        }

        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items)
        {
            return items.Select(i => new SelectListItem
            {
                Text = i.GetPropertyValue("Name"),
                Value = i.GetPropertyValue("Id")
            });
        }
    }
}
