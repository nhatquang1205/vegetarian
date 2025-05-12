namespace vegetarian.Databases.Entities
{
    public class Category : BaseEntity<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public virtual ICollection<ProductCategory> Products { get; set; } = new HashSet<ProductCategory>();
    }
}