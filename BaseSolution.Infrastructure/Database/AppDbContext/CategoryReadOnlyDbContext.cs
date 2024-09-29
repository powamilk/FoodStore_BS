using Microsoft.EntityFrameworkCore;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Database.AppDbContext
{
    public class CategoryReadOnlyDbContext : DbContext
    {
        public CategoryReadOnlyDbContext(DbContextOptions<CategoryReadOnlyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryReadOnlyDbContext).Assembly);
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<Product> Products { get; set; }

    }
}
