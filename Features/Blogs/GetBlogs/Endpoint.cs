using vegetarian.Commons.Schemas;
using vegetarian.Repositories.Blogs;
using FastEndpoints;

namespace vegetarian.Features.Blogs.GetBlogs
{
    public class Endpoint : Endpoint<Request, PaginationResponse<BlogResonse>>
    {
        public IBlogRepository BlogRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/blogs");
            AllowAnonymous();
            Description(x => x
                .WithName("Get Blogs")
                .Produces<PaginationResponse<BlogResonse>>(200)
                .Produces(400)
                .Produces(500));
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
        
            // Get the categories
            var (total, blogs) = await BlogRepository.GetBlogsAsync(req.Keyword, req);

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
                    AuthorId = c.AuthorId
                })],
                TotalCount = total
            }, 200, ct);
        }
    }
}