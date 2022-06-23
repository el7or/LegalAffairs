using Moe.La.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Moe.La.Infrastructure.Extensions
{
    public static class IQueryableExtentions
    {
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, QueryObject queryObject, Dictionary<string, Expression<Func<T, object>>> columnMap)
        {
            if (String.IsNullOrEmpty(queryObject.SortBy) // there in not SortBy in the url query
                || !columnMap.ContainsKey(queryObject.SortBy)) // SortBy not avaliable in the columnMap!!!
                return query;

            if (queryObject.IsSortAscending)
                return query.OrderBy(columnMap[queryObject.SortBy]); //Get the expression from our dictionary
            else
                return query.OrderByDescending(columnMap[queryObject.SortBy]);
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, QueryObject queryObject)
        {
            if (queryObject.Page <= 0)
                queryObject.Page = 1;
            if (queryObject.PageSize <= 0)
                queryObject.PageSize = 20;

            return query.Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
        }
    }
}
