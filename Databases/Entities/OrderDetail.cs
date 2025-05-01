namespace vegetarian.Databases.Entities
{
    public class OrderDetail : BaseEntity<int>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}