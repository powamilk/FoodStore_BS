using BaseSolution.Application.DataTransferObjects.Product.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.Database.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadWrite
{
    public class ProductReadWriteRepository : IProductReadWriteRepository
    {
        private readonly ProductReadWriteDbContext _dbContext;
        private readonly ILocalizationService _localizationService;

        public ProductReadWriteRepository(ILocalizationService localizationService, ProductReadWriteDbContext dbContext)
        {
            _dbContext = dbContext;
            _localizationService = localizationService;
        }

        public async Task<RequestResult<int>> AddProductAsync(Product entity, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Products.AddAsync(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception e)
            {
                return RequestResult<int>.Fail(_localizationService["Unable to create product"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToCreate + "product"
                    }
                });
            }
        }

        public async Task<RequestResult<int>> DeleteProductAsync(int productId, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await GetProductByIdAsync(productId, cancellationToken);
                if (entity == null)
                {
                    return RequestResult<int>.Fail(_localizationService["Product not found"], new[]
                    {
                        new ErrorItem
                        {
                            Error = "Product not found",
                            FieldName = "Product"
                        }
                    });
                }

                _dbContext.Products.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<int>.Succeed(1);
            }
            catch (Exception e)
            {
                return RequestResult<int>.Fail(_localizationService["Unable to delete product"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToDelete + "product"
                    }
                });
            }
        }

        public async Task<RequestResult<int>> UpdateProductAsync(Product entity, CancellationToken cancellationToken)
        {
            try
            {
                var entityProd = await GetProductByIdAsync(entity.Id, cancellationToken);
                if (entityProd == null)
                {
                    return RequestResult<int>.Fail(_localizationService["Product not found"], new[]
                    {
                        new ErrorItem
                        {
                            Error = "Product not found",
                            FieldName = "Product"
                        }
                    });
                }

                entityProd.Name = string.IsNullOrWhiteSpace(entity.Name) ? entityProd.Name : entity.Name;
                entityProd.Price = entity.Price;
                entityProd.Description = entity.Description;
                entityProd.Stock = entity.Stock;

                _dbContext.Products.Update(entityProd);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<int>.Succeed(1);
            }
            catch (Exception e)
            {
                return RequestResult<int>.Fail(_localizationService["Unable to update product"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToUpdate + "product"
                    }
                });
            }
        }

        private async Task<Product?> GetProductByIdAsync(int productId, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);
            return product;
        }
    }
}
