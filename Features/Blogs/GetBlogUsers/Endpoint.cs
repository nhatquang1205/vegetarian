using FastEndpoints;
using vegetarian.Repositories.Blogs;

namespace vegetarian.Features.Blogs.GetBlogUsers
{
    public class Endpoint : Endpoint<Request, List<BlogsUserReponse>>
    {
        public IBlogRepository BlogRepository { get; set; } = null!;
        public override void Configure()
        {
            Get("/blogs/{id}/users");
            AllowAnonymous();
            Description(x => x
                .WithName("GetBlogUsers")
                .Produces<List<BlogsUserReponse>>(200)
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

            var users = await BlogRepository.GetBlogsUsersAsync(request.Id);

            var userResponses = users.Select(user => new BlogsUserReponse
            {
                BlogId = user.BlogId,
                UserFullName = user.UserFullName,
                UserEmail = user.UserEmail,
                UserPhone = user.UserPhone
            }).ToList();

            await SendAsync(userResponses, cancellation: ct);
        }
    }
}