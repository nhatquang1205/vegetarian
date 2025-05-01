using vegetarian.Commons.Schemas;
using vegetarian.Repositories.Categories;
using FastEndpoints;

namespace vegetarian.Features.Categories.GetCategories
{
    public class Endpoint : Endpoint<Request, PaginationResponse<CategoryResponse>>
    {
        public ICategoryRepository CategoryRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/categories");
            AllowAnonymous();
            Description(x => x
                .WithName("Get Categories")
                .Produces<PaginationResponse<CategoryResponse>>(200)
                .Produces(400)
                .Produces(500));
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
        
            // Get the categories
            var (total, categories) = await CategoryRepository.GetCategoriesAsync(req.Keyword, req);

            // Return the response
            await SendAsync(new PaginationResponse<CategoryResponse>
            {
                Items = [.. categories.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })],
                TotalCount = total
            }, 200, ct);
        }
    }
}