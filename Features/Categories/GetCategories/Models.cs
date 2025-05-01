using vegetarian.Commons.Schemas;

namespace vegetarian.Features.Categories.GetCategories
{
    public class Request : PaginationRequest
    {
        public string? Keyword { get; set; } = string.Empty;
    }
}