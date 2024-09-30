using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Product.Request;
using BaseSolution.Application.DataTransferObjects.Product;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BaseSolution.Application.ValueObjects.Pagination;
using BaseSolution.Infrastructure.Extensions;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.Database.AppDbContext;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadOnly
{
    public class ProductReadOnlyRepository : IProductReadOnlyRepository
    {
        private readonly DbSet<Product> _product;
        private readonly IMapper _mapper;
        private readonly ProductReadOnlyDbContext _dbContext;
        private readonly ILocalizationService _localizationService;

        public ProductReadOnlyRepository(
            IMapper mapper,
            ILocalizationService localizationService,
            ProductReadOnlyDbContext dbContext)
        {
            _product = dbContext.Set<Product>();
            _mapper = mapper;
            _dbContext = dbContext;
            _localizationService = localizationService;
        }

        public async Task<RequestResult<ProductDto?>> GetProductByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _product
                    .AsNoTracking()
                    .Where(p => p.Id == id)
                    .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return RequestResult<ProductDto?>.Succeed(product);
            }
            catch (Exception e)
            {
                return RequestResult<ProductDto?>.Fail(_localizationService["Product not found"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToGet + "product"
                    }
                });
            }
        }

        public async Task<RequestResult<PaginationResponse<Product>>> GetProductsWithPaginationAsync(ViewProductWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _product.AsQueryable();

                var paginationResult = await query.PaginateAsync<Product>(request, cancellationToken);

                return RequestResult<PaginationResponse<Product>>.Succeed(paginationResult);
            }
            catch (Exception e)
            {
                return RequestResult<PaginationResponse<Product>>.Fail(_localizationService["Error retrieving products"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToGet + "products"
                    }
                });
            }
        }
    }
}
