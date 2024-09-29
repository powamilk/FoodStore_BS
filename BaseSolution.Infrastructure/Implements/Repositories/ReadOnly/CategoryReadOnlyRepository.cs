using AutoMapper;
using AutoMapper.QueryableExtensions;
using BaseSolution.Application.DataTransferObjects.Category;
using BaseSolution.Application.DataTransferObjects.Example;
using BaseSolution.Application.DataTransferObjects.Example.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ValueObjects.Pagination;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.Database.AppDbContext;
using BaseSolution.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadOnly
{
    public class CategoryReadOnlyRepository : ICategoryReadOnlyRepository
    {
        private readonly DbSet<Category> _category;
        private readonly IMapper _mapper;
        private readonly ExampleReadOnlyDbContext _dbContext;
        private readonly ILocalizationService _localizationService;

        public CategoryReadOnlyRepository(IMapper mapper, ILocalizationService localizationService,ExampleReadOnlyDbContext dbContext)
        {
            _category = dbContext.Set<Category>();
            _mapper = mapper;
            _dbContext = dbContext;
            _localizationService = localizationService;
        }

        public async Task<RequestResult<CategoryDto?>> GetCategoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var example = await _category.AsNoTracking().Where(c => c.Id == id).ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(cancellationToken);

                return RequestResult<CategoryDto?>.Succeed(example);
            }
            catch (Exception e)
            {
                return RequestResult<CategoryDto?>.Fail(_localizationService["Category is not found"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToGet + "category"
                    }
                });
            }
        }

        public async Task<RequestResult<PaginationResponse<Category>>> GetCategoryWithPaginationAsync(ViewCategoryWithPaginationRequest request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<Category>().AsQueryable();

            var paginationResult = await query.PaginateAsync<Category>(request, cancellationToken);
            return RequestResult<PaginationResponse<Category>>.Succeed(paginationResult);
        }
    }
}
