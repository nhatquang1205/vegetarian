using System.ComponentModel.DataAnnotations;

namespace vegetarian.Databases.Entities
{
    public class ProductCategory
    {
        [Key]
        public int ProductId { get; set; }
        [Key]
        public int CategoryId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}