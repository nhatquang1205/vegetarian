using FastEndpoints;
using vegetarian.Repositories.Blogs;
using static vegetarian.Commons.AppEnums;

namespace vegetarian.Features.Blogs.ArchiveBlog
{
    public class Endpoint : Endpoint<Request, Response>
    {
        public IBlogRepository BlogRepository { get; set; } = null!;
        public override void Configure()
        {
            Put("/blogs/{id}/archive");
            AllowAnonymous();
            Description(x => x
                .WithName("ArchiveBlog")
                .Produces<Response>(200)
                .Produces(404)
                .Produces(500));
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            var blog = await BlogRepository.GetBlogByIdAsync(request.Id);
            if (blog == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            // Archive the blog
            blog.Status = BlogStatus.Archived;
            await BlogRepository.UpdateBlogAsync(blog);

            var response = new Response
            {
                BlogId = request.Id
            };

            await SendAsync(response, cancellation: ct);
        }
    }
}