using FastEndpoints;
using FluentValidation;

namespace vegetarian.Features.Categories.Save
{
    public class Request
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(100)
                .WithMessage("Name must be less than 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must be less than 500 characters");
        }
    }
    
    public class Response
    {
        public string Message => "Category saved successfully";
        public int? CategoryId { get; set; }
    }
}