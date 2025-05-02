using FastEndpoints;
using vegetarian.Repositories.Products;

namespace vegetarian.Features.Products.GetProduct
{
    public class Endpoint : Endpoint<Request, ProductResponse>
    {
        public IProductRepository ProductRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/products/{id}");
            AllowAnonymous();
            Description(x => x
                .WithName("GetProduct")
                .Produces<ProductResponse>(200)
                .Produces(404)
                .Produces(500));
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            // Simulate fetching category from a database
            var product = await ProductRepository.GetProductByIdAsync(request.Id);
            if (product == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Type = product.Type,
                IsPublished = product.IsPublished,
                CategoryIds = [.. product.Categories.Select(c => c.CategoryId)],
                ChildrenIds = [.. product.Children.Select(c => c.Id)],
                Images = [.. product.Images.Select(i => new ImageResponse
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl
                })]
            };

            await SendAsync(response, cancellation: ct);
        }
    }
}