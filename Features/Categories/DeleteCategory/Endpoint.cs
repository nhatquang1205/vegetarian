using FastEndpoints;
using vegetarian.Repositories.Categories;

namespace vegetarian.Features.Categories.DeleteCategory
{
    public class Endpoint : Endpoint<Request, Response>
    {
        public ICategoryRepository CategoryRepository { get; set; } = default!;
        public override void Configure()
        {
            Delete("/categories/{id}");
            AllowAnonymous();
            Description(x => x
                .WithName("DeleteCategory")
                .Produces<Response>(200)
                .Produces(404)
                .Produces(500));
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            var category = await CategoryRepository.GetCategoryByIdAsync(request.Id);
            if (category == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await CategoryRepository.DeleteCategoryAsync(category.Id);

            var response = new Response
            {
                CategoryId = request.Id
            };

            await SendAsync(response, cancellation: ct);
        }
    }
}