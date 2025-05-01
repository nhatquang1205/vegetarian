using System.ComponentModel.DataAnnotations;

namespace vegetarian.Databases.Entities
{
    public class BlogsUser : BaseEntity<int>
    {
        public int BlogId { get; set; }
        public required string UserFullName { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPhone { get; set; }

        public virtual Blog Blog { get; set; } = null!;
    }
}