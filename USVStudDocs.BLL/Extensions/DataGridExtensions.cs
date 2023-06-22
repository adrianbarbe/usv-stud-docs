using System.Linq.Expressions;
using System.Reflection;
using ePlato.CoreApp.Models.Shared.DataGrid;
using USVStudDocs.BLL.Helpers.Common;

namespace USVStudDocs.BLL.Extensions
{
    public static class DataGridExtensions
    {
        public static IQueryable<TSource> Filtering<TSource>(this IQueryable<TSource> query,
            RequestQueryModel requestQuery)
        {
            if (requestQuery == null || requestQuery.Filter == null)
            {
                return query;
            }

            foreach (var filter in requestQuery.Filter)
            {
                if (string.IsNullOrEmpty(filter.Value))
                {
                    break;
                }

                if (string.IsNullOrEmpty(filter.Field))
                {
                    throw new Exception("Filter field name is empty");
                }

                if (string.IsNullOrEmpty(filter.Operator))
                {
                    throw new Exception("Filter operator is empty");
                }

                query = query.Where(
                    ExpressionUtils.BuildPredicate<TSource>(filter.Field, filter.Operator, filter.Value));
            }

            return query;
        }

        public static IQueryable<TSource> PaginateSorting<TSource>(this IQueryable<TSource> query,
            RequestQueryModel requestQuery)
        {
            if (requestQuery == null)
            {
                return query;
            }

            if ((!string.IsNullOrEmpty(requestQuery.SortBy) && requestQuery.SortBy != "null") &&
                (!string.IsNullOrEmpty(requestQuery.SortDirection) && requestQuery.SortDirection != "null"))
            {
                if (typeof(TSource).GetProperty(requestQuery.SortBy,
                        BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public) == null)
                {
                    throw new ArgumentNullException(
                        $"Property {requestQuery.SortBy} doesn't exist in {typeof(TSource)} entity.");
                }

                switch (requestQuery.SortDirection)
                {
                    case "asc":
                        query = query.OrderBy(requestQuery.SortBy);
                        break;
                    case "desc":
                        query = query.OrderBy(requestQuery.SortBy, true);
                        break;
                }
            }

            if (requestQuery.ItemsPerPage > 0 && requestQuery.PageNumber > 0)
            {
                var skip = (requestQuery.PageNumber - 1) * requestQuery.ItemsPerPage;
                query = query.Skip(skip)
                    .Take(requestQuery.ItemsPerPage);
            }

            return query;
        }

        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName,
            bool descending, bool anotherLevel)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var property = Expression.PropertyOrField(param, propertyName);
            var sort = Expression.Lambda(property, param);

            var call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, false);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool descending)
        {
            return OrderingHelper(source, propertyName, descending, false);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, true);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName,
            bool descending)
        {
            return OrderingHelper(source, propertyName, descending, true);
        }
    }
}