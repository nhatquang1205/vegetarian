using vegetarian.Commons.Schemas;
using vegetarian.Repositories.Products;
using FastEndpoints;

namespace vegetarian.Features.Products.GetPublicDrinks
{
    public class Endpoint : EndpointWithoutRequest<PaginationResponse<CategoryResponse>>
    {
        public IProductRepository ProductRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/public/drinks");
            AllowAnonymous();
            Description(x => x
                .WithName("Get drinks")
                .Produces<PaginationResponse<CategoryResponse>>(200)
                .Produces(400)
                .Produces(500));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
        
            // Get the products
            var categories = await ProductRepository.GetDrinks();

            // Return the response
            await SendAsync(new PaginationResponse<CategoryResponse>
            {
                Items = [.. categories.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Products = [.. c.Products
                        .Where((p) => p.Product.Type == Commons.AppEnums.ProductType.Drink && p.Product.IsPublished).
                        Select(p => new ProductResponse
                        {
                            Id = p.Product.Id,
                            Name = p.Product.Name,
                            Price = p.Product.Price,
                            Description = p.Product.Description
                        })]
                })],
            }, 200, ct);
        }
    }
}