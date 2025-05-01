using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace vegetarian.Databases
{
    public class QueryFilter
    {
        public static ModelBuilder HasQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (typeof(IAuditableEntity).IsAssignableFrom(clrType))
                {
                    var parameter = Expression.Parameter(clrType, "e");
                    var isDeletedProperty = Expression.Property(parameter, nameof(IAuditableEntity.IsDeleted));

                    var filter = Expression.Lambda(
                        Expression.Equal(isDeletedProperty, Expression.Constant(false)),
                        parameter
                    );

                    modelBuilder.Entity(clrType).HasQueryFilter(filter);
                }
            }
            return modelBuilder;
        }
    }
}
