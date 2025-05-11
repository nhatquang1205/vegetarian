
using vegetarian.Commons;
using vegetarian.Databases.Entities;
using static vegetarian.Commons.AppEnums;

namespace vegetarian.Database.InitDb
{
    public partial class DbInitializer
    {
        public async Task SeedDataProducts()
        {
            try
            {
                _logger.LogInformation("[DbInitializer] Seeding products data");

                if (_context.Products.Any(x => x.Type == ProductType.Food || x.Type == ProductType.Buffet))
                {
                    return;
                }

                var combo1Product = new Product
                {
                    Name = "Combo Trưa 1",
                    Description = "Combo siêu hời dành cho buổi trưa",
                    Price = 65000,
                    Type = ProductType.Food,
                    IsPublished = true,
                    IsCanDelete = false,
                };

                var combo2Product = new Product
                {
                    Name = "Combo Trưa 2",
                    Description = "Combo siêu hời dành cho buổi trưa",
                    Price = 85000,
                    Type = ProductType.Food,
                    IsPublished = true,
                    IsCanDelete = false,
                };

                var buffetProduct = new Product
                {
                    Name = "Buffet chay",
                    Description = "Buffet chay",
                    Price = 129000,
                    Type = ProductType.Buffet,
                    IsPublished = true,
                    IsCanDelete = false,
                };

                await _context.Products.AddAsync(combo1Product);
                await _context.Products.AddAsync(combo2Product);
                await _context.Products.AddAsync(buffetProduct);

                await _context.SaveChangesAsync();
                _logger.LogInformation("[DbInitializer] Seeding default data completed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

    }
}