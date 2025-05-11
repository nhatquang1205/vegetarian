using FastEndpoints;
using vegetarian.Repositories.Blogs;

namespace vegetarian.Features.Blogs.Save
{
    public class Endpoint : Endpoint<Request, Response>
    {
        public IBlogRepository BlogRepository { get; set; } = null!;
        public override void Configure()
        {
            Post("/blogs/save");
            AllowAnonymous();
            Description(x => x
                .WithName("Save Blog")
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
                var blog = await BlogRepository.GetBlogByIdAsync(req.Id.Value);
                if (blog == null)
                {
                    await SendNotFoundAsync(ct);
                    return;
                }
                blog.Title = req.Title;
                blog.Content = req.Content;
                blog.PublishedAt = req.PublishedAt;
                blog.Status = req.Status;
                blog.AuthorName = req.AuthorName;
                blog.AuthorId = req.AuthorId;
                blog.MaxUsers = req.MaxUsers;
                await BlogRepository.UpdateBlogAsync(blog);
            }
            else
            {
                var newBlog = new Databases.Entities.Blog
                {
                    Title = req.Title,
                    Content = req.Content,
                    PublishedAt = req.PublishedAt,
                    Status = req.Status,
                    AuthorName = req.AuthorName,
                    AuthorId = req.AuthorId,
                    MaxUsers = req.MaxUsers,
                    Type = req.Type,
                };
                req.Id = await BlogRepository.AddBlogAsync(newBlog);
            }

            // Return the response
            await SendAsync(new Response
            {
                BlogId = req.Id,
            }, 201, ct);
        }
    }
}