using static vegetarian.Commons.AppEnums;

namespace vegetarian.Features.Blogs
{
    public class BlogResonse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public DateTime? PublishedAt { get; set; }
        public BlogStatus Status { get; set; }
        public BlogType Type { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int? AuthorId { get; set;}
        public int MaxUsers { get; set; }
        public int CurrentUserCount { get; set; }
    }
}