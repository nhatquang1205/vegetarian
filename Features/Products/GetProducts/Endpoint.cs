using vegetarian.Commons.Schemas;
using vegetarian.Repositories.Products;
using FastEndpoints;

namespace vegetarian.Features.Products.GetProducts
{
    public class Endpoint : Endpoint<Request, PaginationResponse<ProductResponse>>
    {
        public IProductRepository ProductRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/products");
            AllowAnonymous();
            Description(x => x
                .WithName("Get Products")
                .Produces<PaginationResponse<ProductResponse>>(200)
                .Produces(400)
                .Produces(500));
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
        
            // Get the products
            var (total, products) = await ProductRepository.GetProductsAsync(req.Keyword, req);

            // Return the response
            await SendAsync(new PaginationResponse<ProductResponse>
            {
                Items = [.. products.Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Type = p.Type,
                    IsPublished = p.IsPublished,
                })],
                TotalCount = total
            }, 200, ct);
        }
    }
}