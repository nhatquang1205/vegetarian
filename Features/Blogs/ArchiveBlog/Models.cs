namespace vegetarian.Features.Blogs.ArchiveBlog
{
    public class Request
    {
        public int Id { get; set; }
    }

    public class Response
    {
        public string Message => "Blog archived successfully";
        public int? BlogId { get; set; }
    }
}