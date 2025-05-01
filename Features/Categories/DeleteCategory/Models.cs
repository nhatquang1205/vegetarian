using FluentValidation;

namespace vegetarian.Features.Categories.DeleteCategory
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
        public string Message => "Category deleted successfully";
        public int? CategoryId { get; set; }
    }
}