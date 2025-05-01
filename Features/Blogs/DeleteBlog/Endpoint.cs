using FastEndpoints;
using vegetarian.Repositories.Blogs;

namespace vegetarian.Features.Blogs.DeleteBlog
{
    public class Endpoint : Endpoint<Request, Response>
    {
        public IBlogRepository BlogRepository { get; set; } = default!;
        public override void Configure()
        {
            Delete("/blogs/{id}");
            AllowAnonymous();
            Description(x => x
                .WithName("DeleteBlog")
                .Produces<Response>(200)
                .Produces(404)
                .Produces(500));
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            var category = await BlogRepository.GetBlogByIdAsync(request.Id);
            if (category == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await BlogRepository.DeleteBlogAsync(category.Id);

            var response = new Response
            {
                BlogId = request.Id
            };

            await SendAsync(response, cancellation: ct);
        }
    }
}