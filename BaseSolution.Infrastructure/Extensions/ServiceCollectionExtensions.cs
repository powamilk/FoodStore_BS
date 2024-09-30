using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.Database.AppDbContext;
using BaseSolution.Infrastructure.Implements.Repositories.ReadOnly;
using BaseSolution.Infrastructure.Implements.Repositories.ReadWrite;
using BaseSolution.Infrastructure.Implements.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseSolution.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ExampleReadOnlyDbContext>(options =>
            {
                // Configure your DbContext options here
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddDbContextPool<ExampleReadWriteDbContext>(options =>
            {
                // Configure your DbContext options here
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddDbContextPool<CategoryReadWriteDbContext>(options =>
            {
                var dbConnect = configuration.GetConnectionString("DbConnection");
                // Configure your DbContext options here
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddDbContextPool<CategoryReadOnlyDbContext>(options =>
            {
                // Configure your DbContext options here
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddDbContextPool<ProductReadWriteDbContext>(options =>
            {
                var dbConnect = configuration.GetConnectionString("DbConnection");
                // Configure your DbContext options here
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddDbContextPool<ProductReadOnlyDbContext>(options =>
            {
                var dbConnect = configuration.GetConnectionString("DbConnection");
                // Configure your DbContext options here
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddTransient<ILocalizationService, LocalizationService>();

            services.AddTransient<IProductReadOnlyRepository, ProductReadOnlyRepository>();
            services.AddTransient<IProductReadWriteRepository, ProductReadWriteRepository>();

            services.AddTransient<ICategoryReadOnlyRepository, CategoryReadOnlyRepository>();
            services.AddTransient<ICategoryReadWriteRepository, CategoryReadWriteRepository>();


            return services;
        }


    }
}