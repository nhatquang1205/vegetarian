using FastEndpoints;
using FastEndpoints.Swagger;
using Minio;
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

bld.Services.AddSingleton(_ =>
    new MinioClient()
        .WithEndpoint(bld.Configuration["Minio:Endpoint"], 9000)
        .WithCredentials(bld.Configuration["Minio:AccessKey"], bld.Configuration["Minio:SecretKey"])
        .WithSSL(false)
        .Build()
); 

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
    .UseCors("AllowSpecificOrigin")
    .UseSwaggerGen();
app.Run();
