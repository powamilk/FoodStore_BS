using BaseSolution.Application.DataTransferObjects.Example.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using BaseSolution.Infrastructure.Implements.Repositories.ReadWrite;
using BaseSolution.Infrastructure.Implements.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.Categories
{
    public class CategoryDeleteViewModel : ViewModelBase<CategoryDeleteRequest>
    {
        private readonly ICategoryReadWriteRepository _categoryReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public CategoryDeleteViewModel(ICategoryReadWriteRepository categoryReadWriteRepository, ILocalizationService localizationService)
        {
            _categoryReadWriteRepository = categoryReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(CategoryDeleteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _categoryReadWriteRepository.DeleteCategoryAsync(request, cancellationToken);

                Success = result.Success;
                ErrorItems = result.Errors;
                Message = result.Message;
                return;
            }
            catch (Exception)
            {
                Success = false;
                ErrorItems = new[]
                    {
                    new ErrorItem
                    {
                        Error = _localizationService["Error occurred while updating the Category"],
                        FieldName = string.Concat(LocalizationString.Common.FailedToDelete, "Category")
                    }
                };
            }
        }
    }
}
