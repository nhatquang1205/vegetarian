using vegetarian.Commons.Schemas;
using vegetarian.Repositories.Blogs;
using FastEndpoints;

namespace vegetarian.Features.Blogs.GetPublicBlogs
{
    public class Endpoint : Endpoint<PaginationRequest, PaginationResponse<BlogResonse>>
    {
        public IBlogRepository BlogRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/public/blogs");
            AllowAnonymous();
            Description(x => x
                .WithName("Get Public Blogs")
                .Produces<PaginationResponse<BlogResonse>>(200)
                .Produces(400)
                .Produces(500));
        }

        public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
        {
            // Get the categories
            var (total, blogs) = await BlogRepository.GetPublicBlogsAsync(req, Commons.AppEnums.BlogType.Blog);

            // Return the response
            await SendAsync(new PaginationResponse<BlogResonse>
            {
                Items = [.. blogs.Select(c => new BlogResonse
                {
                    Id = c.Id,
                    Title = c.Title,
                    Content = c.Content,
                    PublishedAt = c.PublishedAt,
                    Status = c.Status,
                    Type = c.Type,
                    AuthorName = c.AuthorName,
                    AuthorId = c.AuthorId,
                    MaxUsers = c.MaxUsers,
                    CurrentUserCount = c.BlogsUsers.Count
                })],
                TotalCount = total
            }, 200, ct);
        }
    }
}