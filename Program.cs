using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using vegetarian.Database.InitDb;
using vegetarian.Extensions;
using vegetarian.Repositories.Blogs;
using vegetarian.Repositories.Categories;

var bld = WebApplication.CreateBuilder();

bld.Services.AddDataContext(bld.Configuration)
    .AddCustomCorsConfig()
    .AddEndpointsApiExplorer();

bld.Services.AddScoped<IDbInitializer, DbInitializer>();
bld.Services.AddScoped<IBlogRepository, BlogRepository>();
bld.Services.AddScoped<ICategoryRepository, CategoryRepository>();

bld.Services
    // .AddAuthenticationJwtBearer(s => s.SigningKey = bld.Configuration["Jwt:SecretKey"])
    // .AddAuthorization()
    .AddFastEndpoints()
    .SwaggerDocument();

var app = bld.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInitializer.Initialize(); // or .Seed()
}

app
    // .UseAuthentication()
    // .UseAuthorization()
    .UseFastEndpoints()
    .UseSwaggerGen();
app.Run();
