using vegetarian.Commons.Schemas;

namespace vegetarian.Features.Blogs.GetBlogs
{
    public class Request : PaginationRequest
    {
        public string? Keyword { get; set; } = string.Empty;
    }
}