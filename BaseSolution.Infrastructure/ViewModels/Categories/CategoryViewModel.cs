using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.Categories
{
    public class CategoryViewModel : ViewModelBase<int>
    {
        public readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public CategoryViewModel(ICategoryReadOnlyRepository categoryReadOnlyRepository, ILocalizationService localizationService)
        {
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(int idExample, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _categoryReadOnlyRepository.GetCategoryByIdAsync(idExample, cancellationToken);

                Data = result.Data!;
                Success = result.Success;
                ErrorItems = result.Errors;
                Message = result.Message;
                return;
            }
            catch
            {
                Success = false;
                ErrorItems = new[]
                {
                new ErrorItem
                {
                    Error = _localizationService["Error occurred while getting the Category"],
                    FieldName = string.Concat(LocalizationString.Common.FailedToGet, "Category")
                }
            };
            }
        }
    }
}
