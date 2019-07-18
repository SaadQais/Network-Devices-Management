using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksManagement.Infrastructure.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void TryUpdateManyToMany<T, TKey>(this DbContext db, IEnumerable<T> currentItems, 
            IEnumerable<T> newItems, Func<T, TKey> getKey) where T : class
        {
            db.Set<T>().RemoveRange(currentItems.Except(newItems, getKey));
            db.Set<T>().AddRange(newItems.Except(currentItems, getKey));
        }

        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, 
            Func<T, TKey> getKeyFunc)
        {
            return items
                .GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems) => new { item, tempItems })
                .SelectMany(t => t.tempItems.DefaultIfEmpty(), (t, temp) => new { t, temp })
                .Where(t => t.temp == null || t.temp.Equals(default(T)))
                .Select(t => t.t.item);
        }

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
