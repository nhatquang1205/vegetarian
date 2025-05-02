using static vegetarian.Commons.AppEnums;

namespace vegetarian.Databases.Entities
{
    public class Blog : BaseEntity<int>
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public DateTime? PublishedAt { get; set; }
        public BlogStatus Status { get; set; }
        public BlogType Type { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int? AuthorId { get; set;}
        public int MaxUsers { get; set; }

        public virtual ICollection<BlogsUser> BlogsUsers { get; set; } = new HashSet<BlogsUser>();
        public virtual User Author { get; set; } = null!;
    }
}