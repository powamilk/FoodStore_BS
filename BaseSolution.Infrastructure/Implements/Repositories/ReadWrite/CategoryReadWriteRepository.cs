using Azure.Core;
using BaseSolution.Application.DataTransferObjects.Example.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using BaseSolution.Domain.Enums;
using BaseSolution.Infrastructure.Database.AppDbContext;
using BaseSolution.Infrastructure.Implements.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadWrite
{
    public class CategoryReadWriteRepository : ICategoryReadWriteRepository
    {
        private readonly CategoryReadWriteDbContext _dbContext;
        private readonly ILocalizationService _localizationService;

        public CategoryReadWriteRepository(ILocalizationService localizationService, CategoryReadWriteDbContext dbContext)
        {
            _dbContext = dbContext;
            _localizationService = localizationService;
        }

        public async Task<RequestResult<int>> AddCategoryAsync(Category entity, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Categories.AddAsync(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception e)
            {
                return RequestResult<int>.Fail(_localizationService["Unable to create example"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToCreate + "example"
                    }
                });
            }
        }

        public async Task<RequestResult<int>> DeleteCategoryAsync(CategoryDeleteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await GetCategoryByIdAsync( request.Id , cancellationToken);
                _dbContext.Categories.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<int>.Succeed(1);
            }
            catch (Exception e)
            {
                return RequestResult<int>.Fail(_localizationService["Unable to delete category"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToDelete + "category"
                    }
                });
            }
        }

        public async Task<RequestResult<int>> UpdateCategoryAsync(Category entity, CancellationToken cancellationToken)
        {
            try
            {
                var entityCate = await GetCategoryByIdAsync(entity.Id, cancellationToken);
                entityCate!.Name = string.IsNullOrWhiteSpace(entity.Name) ? entityCate.Name : entity.Name;
                _dbContext.Categories.Update(entityCate);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<int>.Succeed(1);
            }
            catch (Exception e)
            {
                return RequestResult<int>.Fail(_localizationService["Unable to update Category"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToUpdate + "Category"
                    }
                });
            }
        }


        private async Task<Category?> GetCategoryByIdAsync(int idCategory, CancellationToken cancellationToken)
        {
            var example = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == idCategory, cancellationToken);

            return example;
        }
    }
}
