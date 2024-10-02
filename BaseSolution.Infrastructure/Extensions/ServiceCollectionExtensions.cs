using BaseSolution.Application.DataTransferObjects.Category.Request;
using BaseSolution.Application.DataTransferObjects.Example.Request;
using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.Database.AppDbContext;
using BaseSolution.Infrastructure.Extensions.Validation.Category;
using BaseSolution.Infrastructure.Extensions.Validation.Order;
using BaseSolution.Infrastructure.Implements.Repositories.ReadOnly;
using BaseSolution.Infrastructure.Implements.Repositories.ReadWrite;
using BaseSolution.Infrastructure.Implements.Services;
using BaseSolution.Infrastructure.ViewModels.OrderVM;
using FluentValidation;
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

            services.AddDbContextPool<OrderReadOnlyDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddDbContextPool<OrderReadWriteDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddTransient<IOrderReadOnlyRepository, OrderReadOnlyRepository>();
            services.AddTransient<IOrderReadWriteRepository, OrderReadWriteRepository>();

            services.AddTransient<ILocalizationService, LocalizationService>();

            services.AddTransient<IProductReadOnlyRepository, ProductReadOnlyRepository>();
            services.AddTransient<IProductReadWriteRepository, ProductReadWriteRepository>();

            services.AddTransient<ICategoryReadOnlyRepository, CategoryReadOnlyRepository>();
            services.AddTransient<ICategoryReadWriteRepository, CategoryReadWriteRepository>();

            services.AddTransient<OrderCreateViewModel>();
            services.AddTransient<OrderUpdateViewModel>();
            services.AddTransient<OrderDeleteViewModel>();
            services.AddTransient<OrderListWithPaginationViewModel>();
            services.AddTransient<OrderViewModel>();

            services.AddTransient<IValidator<CategoryCreateRequest>, CategoryCreateRequestValidator>();
            services.AddTransient<IValidator<CategoryUpdateRequest>, CategoryUpdateRequestValidator>();
            services.AddTransient<IValidator<CategoryDeleteRequest>, CategoryDeleteRequestValidator>();
            services.AddTransient<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();
            services.AddTransient<IValidator<UpdateOrderRequest>, UpdateOrderRequestValidator>();
            services.AddTransient<IValidator<DeleteOrderRequest>, DeleteOrderRequestValidator>();

            return services;
        }


    }
}