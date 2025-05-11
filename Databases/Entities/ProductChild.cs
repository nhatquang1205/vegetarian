namespace vegetarian.Databases.Entities
{
    public class ProductChild : BaseEntity<int>
    {
        public int ParentId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Parent { get; set; } = default!;
        public virtual Product Product { get; set; } = default!;
    }
}