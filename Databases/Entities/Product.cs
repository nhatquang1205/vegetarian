using static vegetarian.Commons.AppEnums;

namespace vegetarian.Databases.Entities
{
    public class Product : BaseEntity<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public int? ParentId { get; set; }
        public ProductType Type { get; set; }
        public bool IsPublished { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; } = new HashSet<ProductImage>();
        public virtual ICollection<Product> Children { get; set; } = new HashSet<Product>();
        public virtual ICollection<ProductCategory> Categories { get; set; } = new HashSet<ProductCategory>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
        public virtual Product? Parent { get; set; }
    }
}