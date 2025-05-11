using vegetarian.Commons.Schemas;
using vegetarian.Repositories.Products;
using FastEndpoints;

namespace vegetarian.Features.Products.GetProductsForCreate
{
    public class Endpoint : Endpoint<Request, PaginationResponse<ProductResponse>>
    {
        public IProductRepository ProductRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/products-for-create");
            AllowAnonymous();
            Description(x => x
                .WithName("Get Products For Create")
                .Produces<PaginationResponse<ProductResponse>>(200)
                .Produces(400)
                .Produces(500));
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
        
            // Get the products
            var (total, products) = await ProductRepository.GetProductsForCreateAsync(req.Keyword, req.Type, req);

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
                    IsCanDelete = p.IsCanDelete,
                })],
                TotalCount = total
            }, 200, ct);
        }
    }
}