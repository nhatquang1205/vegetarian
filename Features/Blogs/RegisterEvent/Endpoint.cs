using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Endpoints;
using FastEndpoints;
using vegetarian.Repositories.Blogs;

namespace vegetarian.Features.Blogs.RegisterEvent
{
    public class Endpoint : Endpoint<Request, Response>
    {
        public IBlogRepository BlogRepository { get; set; } = null!;
        public override void Configure()
        {
            Post("/blogs/register-event");
            AllowAnonymous();
            Description(x => x
                .WithName("Register Event")
                .Produces<Response>(200)
                .Produces(400)
                .Produces(500));
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            // Validate the request
            if (string.IsNullOrEmpty(req.UserFullName) || string.IsNullOrEmpty(req.UserEmail) || string.IsNullOrEmpty(req.UserPhone))
            {
                await SendAsync(new Response { Message = "Invalid request" }, 400, ct);
                return;
            }

            var blog = await BlogRepository.GetBlogByIdAsync(req.BlogId);

            if (blog == null)
            {
                await SendAsync(new Response { Message = "Blog not found" }, 404, ct);
                return;
            }

            if (blog.BlogsUsers.Any(x => x.UserEmail == req.UserEmail))
            {
                await SendAsync(new Response { Message = "User already registered for this event" }, 400, ct);
                return;
            }

            var currentUserCounts = blog.BlogsUsers.Count;
            if (currentUserCounts >= blog.MaxUsers)
            {
                await SendAsync(new Response { Message = "Event is full" }, 400, ct);
                return;
            }

            // Register the event
            var result = await BlogRepository.RegisterEventAsync(req.BlogId, req.UserFullName, req.UserEmail, req.UserPhone);

            // Return the response
            if (result)
            {
                await SendAsync(new Response { Message = "Event registered successfully" }, 200, ct);
            }
            else
            {
                await SendAsync(new Response { Message = "Failed to register event" }, 500, ct);
            }
        }
    }
}