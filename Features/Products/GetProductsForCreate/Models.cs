using vegetarian.Commons.Schemas;
using static vegetarian.Commons.AppEnums;

namespace vegetarian.Features.Products.GetProductsForCreate
{
    public class Request : PaginationRequest
    {
        public string? Keyword { get; set; } = string.Empty;
        public ProductType? Type { get; set; }
    }
}