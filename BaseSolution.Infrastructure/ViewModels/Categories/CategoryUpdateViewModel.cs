using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Category;
using BaseSolution.Application.DataTransferObjects.Example.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.Implements.Repositories.ReadWrite;
using BaseSolution.Infrastructure.Implements.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.Categories
{
    public class CategoryUpdateViewModel : ViewModelBase<CategoryUpdateRequest>
    {
        private readonly ICategoryReadWriteRepository _exampleReadWriteRepository;
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public CategoryUpdateViewModel(ICategoryReadWriteRepository exampleReadWriteRepository, ICategoryReadOnlyRepository categoryReadOnlyRepository, IMapper mapper, ILocalizationService localizationService)
        {
            _exampleReadWriteRepository = exampleReadWriteRepository;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var updateResult = await _exampleReadWriteRepository.UpdateCategoryAsync(_mapper.Map<Category>(request), cancellationToken);

                if (updateResult.Success)
                {
                    var result = await _categoryReadOnlyRepository.GetCategoryByIdAsync(updateResult.Data, cancellationToken);

                    Success = true;
                    Message = _localizationService["Category created successfully"];
                    Data = result.Data;
                }
                else
                {
                    Success = false;
                    ErrorItems = updateResult.Errors;
                    Message = updateResult.Message;
                }
            }
            catch (Exception)
            {
                Success = false;
                ErrorItems = new[]
                    {
                    new ErrorItem
                    {
                        Error = _localizationService["Error occurred while updating the Category"],
                        FieldName = string.Concat(LocalizationString.Common.FailedToUpdate, "Category")
                    }
                };
            }
        }
    }
}
