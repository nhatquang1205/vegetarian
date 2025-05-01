using FastEndpoints;
using vegetarian.Repositories.Categories;

namespace vegetarian.Features.Categories.GetCategory
{
    public class Endpoint : Endpoint<Request, CategoryResponse>
    {
        public ICategoryRepository CategoryRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/categories/{id}");
            AllowAnonymous();
            Description(x => x
                .WithName("GetCategory")
                .Produces<CategoryResponse>(200)
                .Produces(404)
                .Produces(500));
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            // Simulate fetching category from a database
            var category = await CategoryRepository.GetCategoryByIdAsync(request.Id);
            if (category == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            await SendAsync(response, cancellation: ct);
        }
    }
}