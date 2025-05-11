
using vegetarian.Commons;
using vegetarian.Databases.Entities;
using static vegetarian.Commons.AppEnums;

namespace vegetarian.Database.InitDb
{
    public partial class DbInitializer
    {
        public async Task SeedDataDrinks()
        {
            try
            {
                _logger.LogInformation("[DbInitializer] Seeding drinks data");

                if (_context.Products.Any(x => x.Type == ProductType.Drink))
                {
                    return;
                }

                var category1 = new Category
                {
                    Name = "Sữa hạt",
                    Description = "Sữa hạt - Nuts milk",
                };

                var category2 = new Category
                {
                    Name = "Đồ uống lên men tự nhiên",
                    Description = "Thức uống được lên men tự nhiên tại nhà, không có cồn"
                };

                var category3 = new Category
                {
                    Name = "Trà hoa và trái cây",
                    Description = "Trà hoa và trái cây - Flower & Fruit Tea"
                };

                var category4 = new Category
                {
                    Name = "Sinh tố đạm thực vật",
                    Description = "Plant-based protien smoothies"
                };

                var category5 = new Category
                {
                    Name = "Cà phê và cacao",
                    Description = "Coffee & Cocoa"
                };

                var category6 = new Category
                {
                    Name = "Trà Nhật",
                    Description = "Japanese Tea",
                };

                await _context.Categories.AddAsync(category1);
                await _context.Categories.AddAsync(category2);
                await _context.Categories.AddAsync(category3);
                await _context.Categories.AddAsync(category4);
                await _context.Categories.AddAsync(category5);
                await _context.Categories.AddAsync(category6);
                await _context.SaveChangesAsync();


                var category1Products = new List<Product>
                {
                    new()
                    {
                        Name = "Pink Paradise",
                        Price = 45000,
                        Description = "Hạt điều, yến mạch, thanh long đỏ",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category1
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Sunrise",
                        Price = 45000,
                        Description = "Hạt điều, bí đỏ, mè trắng",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category1
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Morning Rise",
                        Price = 45000,
                        Description = "Óc chó, hạt điều, gạo rang",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category1
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Purple sunset",
                        Price = 45000,
                        Description = "Óc chó, hạt điều, khoai lang trắng",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category1
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Love in green",
                        Price = 50000,
                        Description = "Hạt điều, yến mạch, lúa mì non",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category1
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Black Magic",
                        Price = 45000,
                        Description = "Óc chó, yến mạch, mè đen",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category1
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Hoa Gom's Milo",
                        Price = 45000,
                        Description = "Hạt điều, yến mạch, ca cao",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category1
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Peace Maker",
                        Price = 50000,
                        Description = "Hạnh nhân, hạt điều, ca cao, chuối",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category1
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Sweet Dream",
                        Price = 50000,
                        Description = "Macca, hạt điều, mè đen",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category1
                            }
                        ]
                    },
                };

                var category2Products = new List<Product>
                {
                    new()
                    {
                        Name = "Kombucha sấu gừng Hà Nội",
                        Price = 50000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category2
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Kombucha thanh long đỏ",
                        Price = 60000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category2
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Kombucha mận Sapa",
                        Price = 60000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category2
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Sữa chua trái cây hạt chia",
                        Price = 55000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category2
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Sữa chua chanh dây và Granola Nhật",
                        Price = 50000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category2
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Sữa chua dâu tằm/mơ và Granola Nhật",
                        Price = 55000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category2
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Soda chanh muối Tây Ninh (không cồn)",
                        Price = 45000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category2
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Bia gừng nhà làm (không cồn)",
                        Price = 55000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category1
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Soda mía lên men (không cồn)",
                        Price = 55000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category2
                            }
                        ]
                    },
                };

                var category3Products = new List<Product>
                {
                    new()
                    {
                        Name = "Trà cam xí muội",
                        Price = 60000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category3
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Trà vỏ cà phê cam",
                        Price = 60000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category3
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Trà bưởi chanh dây",
                        Price = 60000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category3
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Tuỳ duyên trà",
                        Price = 55000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category3
                            }
                        ]
                    },
                };

                var category4Products = new List<Product>
                {
                    new()
                    {
                        Name = "Dragonlicious Bowl",
                        Price = 70000,
                        Description = "Thanh long đỏ, chuối, yến mạch, sữa chua, chà là, bột protein thực vật",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category4
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Avoca-Oh! Coffee",
                        Price = 80000,
                        Description = "Bơ, cà phê, chà là, bột protein thực vật, sữa hạt, sữa hạt đặc, dừa sấy",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category4
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Aussion Without N",
                        Price = 80000,
                        Description = "Chanh dây, xoài, hạt chia, sữa hạt điều, chà là, bột protein thực vật, sữa hạt đặc, cơm dừa sợi",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category4
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Matcha Coconut Drift",
                        Price = 95000,
                        Description = "Matcha Ceremonial Grade, cơm dừa tươi, nước dừa, bột protein thực vật, sữa hạt, sữa hạt đặc",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category4
                            }
                        ]
                    },
                };

                var category5Products = new List<Product>
                {
                    new()
                    {
                        Name = "Cacao tan chảy",
                        Price = 60000,
                        Description = "Cacao, sữa hạt, sữa đặc từ hạt, kem thuần chay",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category5
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Cacao quế nóng",
                        Price = 60000,
                        Description = "Cacao Cát Tiên - Lâm Đồng, tỉ lệ bơ 22%, quế hữu cơ, sữa hạt, sữa đặc từ hạt",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category5
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Phin đen đá",
                        Price = 35000,
                        Description = "Cà phê, đường mía",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category5
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Phin bạc xỉu",
                        Price = 50000,
                        Description = "Cà phê, sữa hạt, sữa đặt từ hạt",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category5
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Phin muối",
                        Price = 53000,
                        Description = "Cà phê, sữa hạt, sữa đặt từ hạt, kem thuần chay, muối hồng Himalaya",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category5
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Americano",
                        Price = 50000,
                        Description = "",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category5
                            }
                        ]
                    },
                    new()
                    {
                        Name = "Americano Cam",
                        Price = 55000,
                        Description = "Cà phê, nước cam tươi",
                        Type = ProductType.Drink,
                        IsPublished = true,
                        Categories =
                        [
                            new()
                            {
                                Category = category5
                            }
                        ]
                    },
                };

                await _context.Products.AddRangeAsync(category1Products);
                await _context.Products.AddRangeAsync(category2Products);
                await _context.Products.AddRangeAsync(category3Products);
                await _context.Products.AddRangeAsync(category4Products);
                await _context.Products.AddRangeAsync(category5Products);

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