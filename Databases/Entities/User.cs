using static vegetarian.Commons.AppEnums;

namespace vegetarian.Databases.Entities
{
    public class User : BaseEntity<int>
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Password { get; set; }
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
    }
}