using FluentValidation;

namespace vegetarian.Features.Blogs.DeleteBlog
{
    public class Request
    {
        public int Id { get; set; }
    }
    public class Validator : FastEndpoints.Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }

    public class Response
    {
        public string Message => "Blog deleted successfully";
        public int? BlogId { get; set; }
    }
}