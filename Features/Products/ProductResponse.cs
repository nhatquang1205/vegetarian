using static vegetarian.Commons.AppEnums;

namespace vegetarian.Features.Products
{
    public class ImageResponse
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class CategoryResponse
    {
        public int Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public List<ProductResponse> Products { get; set; } = [];
    }

    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
        public bool IsPublished { get; set; }
        public bool IsCanDelete { get; set; }
        public List<int> CategoryIds { get; set; } = [];
        public List<int> ChildrenIds { get; set; } = [];
        public List<ImageResponse> Images { get; set; } = [];
        public List<ProductResponse> Children { get; set; } = []; 
    }
}