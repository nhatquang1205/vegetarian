using FastEndpoints;
using FluentValidation;
using static vegetarian.Commons.AppEnums;

namespace vegetarian.Features.Products.Save
{
    public class ImageRequest
    {
        public int? Id { get; set; }
        public required string Url { get; set; }
    }
    public class Request
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public required decimal Price { get; set; }
        public ProductType Type { get; set; }
        public bool IsPublished { get; set; }
        public List<int> ChildrenIds { get; set; } = [];
        public List<int> Categories { get; set; } = [];
        public List<ImageRequest> Images { get; set; } = [];
        public List<int> DeletedImages { get; set; } = [];
        public List<int> DeletedChildren { get; set; } = [];
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must be less than 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be less than 500 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Type must be a valid enum value");

            RuleFor(x => x.IsPublished)
                .Must(x => x == true || x == false).WithMessage("IsPublished must be true or false");

            RuleFor(x => x.ChildrenIds)
                .Must(x => x.Count >= 0).When(x => x.Type == ProductType.Buffet)
                .WithMessage("Childrent must have at least 0 items");
        }
    }

    public class Response
    {
        public string Message => "Product saved successfully";
        public int? ProductId { get; set; }
    }
}