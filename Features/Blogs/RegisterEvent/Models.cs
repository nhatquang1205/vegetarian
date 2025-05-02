namespace vegetarian.Features.Blogs.RegisterEvent
{
    public class Request
    {
        public int BlogId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
    }

    public class Response
    {
        public string Message { get; set; } = string.Empty;
    }

}