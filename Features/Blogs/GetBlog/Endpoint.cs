using FastEndpoints;
using vegetarian.Repositories.Blogs;

namespace vegetarian.Features.Blogs.GetBlog
{
    public class Endpoint : Endpoint<Request, BlogResonse>
    {
        public IBlogRepository BlogRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/blogs/{id}");
            AllowAnonymous();
            Description(x => x
                .WithName("GetBlog")
                .Produces<BlogResonse>(200)
                .Produces(404)
                .Produces(500));
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            // Simulate fetching category from a database
            var category = await BlogRepository.GetBlogByIdAsync(request.Id);
            if (category == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var response = new BlogResonse
            {
                Id = category.Id,
                Title = category.Title,
                Content = category.Content,
                PublishedAt = category.PublishedAt,
                Status = category.Status,
                Type = category.Type,
                AuthorName = category.AuthorName,
                AuthorId = category.AuthorId,
                MaxUsers = category.MaxUsers,
                CurrentUserCount = category.BlogsUsers.Count
            };

            await SendAsync(response, cancellation: ct);
        }
    }
}