using FastEndpoints;
using FluentValidation;
using static vegetarian.Commons.AppEnums;

namespace vegetarian.Features.Blogs.Save
{
    public class Request
    {
        public int? Id { get; set;}
        public required string Title { get; set; }
        public required string Content { get; set; }
        public DateTime? PublishedAt { get; set; }
        public BlogStatus Status { get; set; }
        public BlogType Type { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int? AuthorId { get; set;}
        public int MaxUsers { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title must be less than 100 characters");
            
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required");
            
            RuleFor(x => x.PublishedAt)
                .Must(x => x == null || x > DateTime.UtcNow).WithMessage("PublishedAt must be in the future");
            
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status must be a valid enum value");
            
            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Type must be a valid enum value");
            
            RuleFor(x => x.AuthorName)
                .MaximumLength(50).WithMessage("AuthorName must be less than 50 characters");

            RuleFor(x => x.AuthorId)
                .Must(x => x == null || x > 0).WithMessage("AuthorId must be a positive integer");
        }
    }
    
    public class Response
    {
        public string Message => "Blog saved successfully";
        public int? BlogId { get; set; }
    }
}