
using vegetarian.Commons;
using vegetarian.Databases.Entities;
using static vegetarian.Commons.AppEnums;

namespace vegetarian.Database.InitDb
{
    public partial class DbInitializer
    {
        public async Task SeedDataDefault()
        {
            try
            {
                _logger.LogInformation("[DbInitializer] Seeding default data");

                if (_context.Users.Any())
                {
                    return;
                }

                User user1 = new()
                {
                    Email = "admin",
                    Username = "admin",
                    Phone = "admin",
                    Password = Security.GetSHA256(Security.GetSimpleMD5("Admin@123")),
                    IsActive = true,
                    Role = UserRole.Admin,
                };

                await _context.Users.AddAsync(user1);

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