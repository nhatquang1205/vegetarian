using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using vegetarian.Databases;
using vegetarian.Databases.Entities;

namespace vegetarian.Database
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Blog> Blogs { get; set; } = null!;
        public DbSet<BlogsUser> BlogsUsers { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<Databases.Entities.Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelCreate.OnModelCreating(modelBuilder);
            QueryFilter.HasQueryFilter(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task RollbackAsync(IDbContextTransaction transaction)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
        }

        /// <summary>
        /// Chuyển đổi tất cả DateTimeOffset sang UTC trước khi lưu vào database
        /// và cập nhật các trường CreatedAt, UpdatedAt
        /// </summary>
        private void UpdateTimestamps()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditableEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = DateTime.UtcNow;
                        entity.UpdatedAt = DateTime.UtcNow;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = DateTime.UtcNow;
                    } else if (entry.State == EntityState.Deleted)
                    {
                        entity.DeletedAt = DateTime.UtcNow;
                        entity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                    }
                }

                // Chuyển đổi các property kiểu DateTimeOffset sang UTC
                var properties = entry.Entity.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));

                foreach (var prop in properties)
                {
                    var value = (DateTimeOffset?)prop.GetValue(entry.Entity);
                    if (value.HasValue)
                    {
                        prop.SetValue(entry.Entity, value.Value.ToUniversalTime());
                    }
                }
            }
        }
    }
}