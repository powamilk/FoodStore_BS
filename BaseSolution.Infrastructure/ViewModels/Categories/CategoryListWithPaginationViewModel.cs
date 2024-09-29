﻿using BaseSolution.Application.DataTransferObjects.Example.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseSolution.Infrastructure.ViewModels.Categories
{
    public class CategoryListWithPaginationViewModel : ViewModelBase<ViewCategoryWithPaginationRequest>
    {
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public CategoryListWithPaginationViewModel(ICategoryReadOnlyRepository categoryReadOnlyRepository, ILocalizationService localizationService)
        {
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(ViewCategoryWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _categoryReadOnlyRepository.GetCategoryWithPaginationAsync(request, cancellationToken);

                Data = result.Data!;
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
                        Error = _localizationService["Error occurred while getting the list of Categories"],
                        FieldName = string.Concat(LocalizationString.Common.FailedToGet, "list of Categories")
                    }
                };
            }
        }
    }
}
