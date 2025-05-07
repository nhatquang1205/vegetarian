using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using vegetarian.Database;

namespace vegetarian.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                // Dùng AddDbContextPool để tạo ra một pool các DbContext, giúp tăng hiệu suất trong việc sử dụng các DbContext instances
                services.AddDbContextPool<DataContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                        .EnableSensitiveDataLogging() // cho phép log dữ liệu nhạy cảm
                        .EnableDetailedErrors());

                services.AddScoped<DbConnection>(provider =>
                {
                    return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
                });

                // Thêm IDbContextFactory để cho phép tạo ra các instance của DbContext 
                services.AddDbContextFactory<DataContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return services;
        }

        public static IServiceCollection AddCustomCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000", "http://localhost:9000")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials(); // Cho phép client gửi cookie qua cross-origin
                    });
            });

            return services;
        }
    }
}