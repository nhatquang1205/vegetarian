using FluentValidation;

namespace vegetarian.Features.Products.DeleteProduct
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
        public string Message => "Product deleted successfully";
        public int? ProductId { get; set; }
    }
}