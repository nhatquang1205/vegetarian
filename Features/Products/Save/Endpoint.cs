using FastEndpoints;
using vegetarian.Repositories.Products;

namespace vegetarian.Features.Products.Save
{
    public class Endpoint : Endpoint<Request, Response>
    {
        public IProductRepository ProductRepository { get; set; } = null!;
        public override void Configure()
        {
            Post("/products/save");
            AllowAnonymous();
            Description(x => x
                .WithName("Save Product")
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
                var product = await ProductRepository.GetProductByIdAsync(req.Id.Value);
                if (product == null)
                {
                    await SendNotFoundAsync(ct);
                    return;
                }
                product.Name = req.Name;
                product.Description = req.Description;
                product.Price = req.Price;
                product.Type = req.Type;
                product.IsPublished = req.IsPublished;

                if (req.DeletedChildren != null && req.DeletedChildren.Count > 0)
                {
                    var deletedChildren = await ProductRepository.GetProductsByIds(req.DeletedChildren);
                    foreach (var child in deletedChildren)
                    {
                        child.ParentId = null;
                    }
                }

                var addedChildren = await ProductRepository.GetProductsByIds(req.ChildrenIds);
                foreach (var child in addedChildren)
                {
                    child.ParentId = product.Id;
                }

                await ProductRepository.DeleteProductCategoriesAsync(product.Id);
                product.Categories = [.. req.Categories.Select(x => new Databases.Entities.ProductCategory
                {
                    ProductId = x
                })];

                if (req.DeletedImages != null && req.DeletedImages.Count > 0)
                {
                    await ProductRepository.DeleteProductImagesAsync(req.DeletedImages);
                }

                if (req.Images != null && req.Images.Count > 0)
                {
                    await ProductRepository.AddProductImagesAsync(req.Images.Select(x => new Databases.Entities.ProductImage
                    {
                        ImageUrl = x.Url,
                        ProductId = product.Id
                    }));
                }

                await ProductRepository.SaveChangesAsync();
                await ProductRepository.UpdateProductAsync(product);
            }
            else
            {
                var children = await ProductRepository.GetProductsByIds(req.ChildrenIds);
                var newProduct = new Databases.Entities.Product
                {
                    Name = req.Name,
                    Description = req.Description,
                    Price = req.Price,
                    Type = req.Type,
                    IsPublished = req.IsPublished,
                    Categories = [.. req.Categories.Select(x => new Databases.Entities.ProductCategory
                    {
                        CategoryId = x
                    })],
                    Images = [.. req.Images.Select(x => new Databases.Entities.ProductImage
                    {
                        ImageUrl = x.Url
                    })]
                };
                req.Id = await ProductRepository.AddProductAsync(newProduct);
                foreach (var child in children)
                {
                    child.ParentId = req.Id;
                    await ProductRepository.UpdateProductAsync(child);
                }
            }
            await SendAsync(new Response
            {
                ProductId = req.Id,
            }, 201, ct);
        }
    }
}