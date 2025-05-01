namespace vegetarian.Features.Blogs.GetBlogUsers
{
    public class Request
    {
        public int Id { get; set; }
    }

    public class BlogsUserReponse
    {
        public int BlogId { get; set; }
        public required string UserFullName { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPhone { get; set; }
    }
}