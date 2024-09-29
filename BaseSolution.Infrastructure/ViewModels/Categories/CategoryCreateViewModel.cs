using AutoMapper;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using BaseSolution.Domain.Entities;
using BaseSolution.Application.DataTransferObjects.Category.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BaseSolution.Application.DataTransferObjects.Example.Request;
using BaseSolution.Infrastructure.Implements.Repositories.ReadOnly;

namespace BaseSolution.Infrastructure.ViewModels.Categories
{
    public class CategoryCreateViewModel : ViewModelBase<CategoryCreateRequest>
    {
        public readonly ICategoryReadWriteRepository _categoryReadWriteRepository;
        public readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public CategoryCreateViewModel(ICategoryReadWriteRepository categoryReadWriteRepository, ILocalizationService localizationService, IMapper mapper, ICategoryReadOnlyRepository categoryReadOnlyRepository)
        {
            _categoryReadWriteRepository = categoryReadWriteRepository;
            _localizationService = localizationService;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _mapper = mapper;
        }

        public override async Task HandleAsync(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var categoryEntity = _mapper.Map<Category>(request);
                var createResult = await _categoryReadWriteRepository.AddCategoryAsync(categoryEntity, cancellationToken);

                if (createResult.Success)
                {
                    var result = await _categoryReadOnlyRepository.GetCategoryByIdAsync(createResult.Data, cancellationToken);

                    Success = true;
                    Message = _localizationService["Category created successfully"];
                    Data = result.Data;
                }
                else
                {
                    Success = false;
                    ErrorItems = createResult.Errors;
                    Message = createResult.Message;
                }
            }
            catch (Exception)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = _localizationService["Error occurred while creating the category"],
                        FieldName = string.Concat(LocalizationString.Common.FailedToCreate, "Category")
                    }
                };
            }
        }
    }
}
