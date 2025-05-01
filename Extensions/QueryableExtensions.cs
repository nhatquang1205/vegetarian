using System.Linq.Expressions;
using System.Reflection;

namespace vegetarian.Extensions;
public static class IQueryableExtensions
{
    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string? orderBy, string direction)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return source;

        var prop = typeof(T).GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (prop == null) return source;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, prop);
        var lambda = Expression.Lambda(property, parameter);

        string method = direction.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? "OrderByDescending" : "OrderBy";

        var result = typeof(Queryable).GetMethods()
            .Where(m => m.Name == method && m.GetParameters().Length == 2)
            .Single()
            .MakeGenericMethod(typeof(T), prop.PropertyType)
            .Invoke(null, [source, lambda]);

        return (IQueryable<T>)result!;
    }
}
