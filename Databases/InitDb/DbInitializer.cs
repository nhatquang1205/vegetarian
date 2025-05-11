namespace vegetarian.Database.InitDb
{
    public interface IDbInitializer
    {
        Task Initialize();
    }

    public partial class DbInitializer(DataContext context, ILogger<DbInitializer> logger) : IDbInitializer
    {
        private readonly DataContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly ILogger<DbInitializer> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task Initialize()
        {
            await _context.Database.EnsureCreatedAsync();
            await SeedDataDefault();
            await SeedDataProducts();
        }
    }
}