using FastEndpoints;
using vegetarian.Repositories.Categories;

namespace vegetarian.Features.Categories.Save
{
    public class Endpoint : Endpoint<Request, Response>
    {
        public ICategoryRepository CategoryRepository { get; set; } = null!;
        public override void Configure()
        {
            Post("/categories/save");
            AllowAnonymous();
            Description(x => x
                .WithName("Save Category")
                .Produces<Response>(201)
                .Produces(400)
                .Produces(500));
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            // Validate the request
            var validator = new Validator();
            var validationResult = await validator.ValidateAsync(req, ct);
            if (!validationResult.IsValid)
            {
                await SendErrorsAsync(400, ct);
                return;
            }

            if (req.Id.HasValue)
            {
                var category = await CategoryRepository.GetCategoryByIdAsync(req.Id.Value);
                if (category == null)
                {
                    await SendNotFoundAsync(ct);
                    return;
                }
                category.Name = req.Name;
                category.Description = req.Description;
                await CategoryRepository.UpdateCategoryAsync(category);
            }
            else
            {
                var newCategory = new Databases.Entities.Category
                {
                    Name = req.Name,
                    Description = req.Description
                };
                req.Id = await CategoryRepository.AddCategoryAsync(newCategory);
            }

            // Return the response
            await SendAsync(new Response
            {
                CategoryId = req.Id,
            }, 201, ct);
        }
    }
}