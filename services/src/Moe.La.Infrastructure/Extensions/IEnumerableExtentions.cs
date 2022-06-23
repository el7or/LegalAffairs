using Moe.La.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moe.La.Infrastructure.Extensions
{
    public static class IEnumerableExtentions
    {
        public static IEnumerable<T> ApplySorting<T>(this IEnumerable<T> enumerable, QueryObject queryObject, Dictionary<string, Func<T, object>> columnMap)
        {
            if (String.IsNullOrEmpty(queryObject.SortBy) || !columnMap.ContainsKey(queryObject.SortBy))
            {
                return enumerable;
            }

            if (queryObject.IsSortAscending)
            {
                return enumerable.OrderBy(columnMap[queryObject.SortBy]).ToList();
            }
            else
            {
                return enumerable.OrderByDescending(columnMap[queryObject.SortBy]);
            }
        }

        public static IEnumerable<T> ApplyPaging<T>(this IEnumerable<T> query, QueryObject queryObject)
        {
            if (queryObject.Page <= 0)
            {
                queryObject.Page = 1;
            }

            if (queryObject.PageSize <= 0)
            {
                queryObject.PageSize = 20;
            }

            return query.Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
        }
    }
}
