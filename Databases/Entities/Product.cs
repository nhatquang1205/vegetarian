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
        public bool IsCanDelete { get; set; } = true;

        public virtual ICollection<ProductImage> Images { get; set; } = new HashSet<ProductImage>();
        public virtual ICollection<ProductChild> Children { get; set; } = new HashSet<ProductChild>();
        public virtual ICollection<ProductChild> Parents { get; set; } = new HashSet<ProductChild>();
        public virtual ICollection<ProductCategory> Categories { get; set; } = new HashSet<ProductCategory>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
    }
}