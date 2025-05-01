namespace vegetarian.Databases.Entities
{
    public class ProductImage : BaseEntity<int>
    {
        public required string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
}