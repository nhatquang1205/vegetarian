using vegetarian.Commons.Schemas;
using vegetarian.Repositories.Products;
using FastEndpoints;

namespace vegetarian.Features.Products.GetPublicLunchMenu
{
    public class Endpoint : EndpointWithoutRequest<PaginationResponse<ProductResponse>>
    {
        public IProductRepository ProductRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/public/lunch-menu");
            AllowAnonymous();
            Description(x => x
                .WithName("Get lunch menu")
                .Produces<PaginationResponse<ProductResponse>>(200)
                .Produces(400)
                .Produces(500));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
        
            // Get the products
            var products = await ProductRepository.GetLunchMenu();

            // Return the response
            await SendAsync(new PaginationResponse<ProductResponse>
            {
                Items = [.. products.Select(c => new ProductResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price,
                    Children = [.. c.Children
                        .Select(p => new ProductResponse
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