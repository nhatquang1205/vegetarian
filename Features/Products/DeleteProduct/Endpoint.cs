using FastEndpoints;
using vegetarian.Repositories.Products;

namespace vegetarian.Features.Products.DeleteProduct
{
    public class Endpoint : Endpoint<Request, Response>
    {
        public IProductRepository ProductRepository { get; set; } = default!;
        public override void Configure()
        {
            Delete("/products/{id}");
            AllowAnonymous();
            Description(x => x
                .WithName("DeleteProduct")
                .Produces<Response>(200)
                .Produces(404)
                .Produces(500));
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            var product = await ProductRepository.GetProductByIdAsync(request.Id);
            if (product == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await ProductRepository.DeleteProductAsync(product.Id);

            var response = new Response
            {
                ProductId = request.Id
            };

            await SendAsync(response, cancellation: ct);
        }
    }
}