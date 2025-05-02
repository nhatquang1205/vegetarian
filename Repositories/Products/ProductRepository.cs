using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using vegetarian.Commons.Schemas;
using vegetarian.Database;
using vegetarian.Databases.Entities;
using vegetarian.Extensions;

namespace vegetarian.Repositories.Products
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<(int, IEnumerable<Product>)> GetProductsAsync(string? keyword, PaginationRequest paginationRequest);
        Task<int> AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task DeleteProductCategoriesAsync(int productId);
        Task<IEnumerable<Product>> GetProductsByIds(IEnumerable<int> ids);
        Task DeleteProductImagesAsync(IEnumerable<int> imageIds);
        Task AddProductImagesAsync(IEnumerable<ProductImage> images);
        Task SaveChangesAsync();
    }

    public class ProductRepository(DataContext context, IMinioClient minio) : IProductRepository
    {
        private readonly DataContext _context = context;
        private readonly IMinioClient _minio = minio;

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Children)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(int, IEnumerable<Product>)> GetProductsAsync(string? keyword, PaginationRequest paginationRequest)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDynamic(paginationRequest.OrderBy, paginationRequest.OrderDirection);

            return (totalCount, await query
                .Skip((paginationRequest.Page - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync());
        }

        public async Task<int> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);


            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product != null)
            {
                _context.ProductCategories.RemoveRange(product.Categories);
                await DeleteProductImagesAsync(product.Images.Select(i => i.Id));

                foreach (var child in product.Children)
                {
                    child.ParentId = null;
                    _context.Products.Update(child);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductCategoriesAsync(int productId)
        {
            var productCategories = await _context.ProductCategories
                .Where(pc => pc.ProductId == productId)
                .ToListAsync();

            if (productCategories.Count != 0)
            {
                _context.ProductCategories.RemoveRange(productCategories);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByIds(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return [];
            }

            var products = await _context.Products
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            return products;
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency exception
                throw new Exception("Concurrency error occurred while saving changes.", ex);
            }
            catch (DbUpdateException ex)
            {
                // Handle update exception
                throw new Exception("An error occurred while updating the database.", ex);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw new Exception("An unexpected error occurred.", ex);
            }
        }

        public async Task DeleteProductImagesAsync(IEnumerable<int> imageIds)
        {
            if (imageIds == null || !imageIds.Any())
            {
                return;
            }

            var images = await _context.ProductImages
                .Where(i => imageIds.Contains(i.Id))
                .ToListAsync();

            if (images.Count != 0)
            {
                _context.ProductImages.RemoveRange(images);
                await _context.SaveChangesAsync();
            }

            foreach (var image in images)
            {
                // Assuming you have a method to delete the image from the file system
                // await DeleteImageFromFileSystem(image.Url);
                _minio.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket("vegetarian")
                    .WithObject(image.ImageUrl), CancellationToken.None).Wait();
            }
        }

        public Task AddProductImagesAsync(IEnumerable<ProductImage> images)
        {
            if (images == null || !images.Any())
            {
                return Task.CompletedTask;
            }

            foreach (var image in images)
            {
                _context.ProductImages.Add(image);
            }

            return _context.SaveChangesAsync();
        }
    }
}