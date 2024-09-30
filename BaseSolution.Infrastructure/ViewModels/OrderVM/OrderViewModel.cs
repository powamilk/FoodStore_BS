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

namespace BaseSolution.Infrastructure.ViewModels.OrderVM
{
    public class OrderViewModel : ViewModelBase<int>
    {
        private readonly IOrderReadOnlyRepository _orderReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public OrderViewModel(IOrderReadOnlyRepository orderReadOnlyRepository, ILocalizationService localizationService)
        {
            _orderReadOnlyRepository = orderReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _orderReadOnlyRepository.GetOrderByIdAsync(orderId, cancellationToken);

                if (result.Success)
                {
                    Success = true;
                    Data = result.Data;
                }
                else
                {
                    Success = false;
                    ErrorItems = result.Errors;
                    Message = result.Message;
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = _localizationService["Error occurred while retrieving the order."],
                        FieldName = string.Concat(LocalizationString.Common.FailedToGet, "Order"),
                        Code = ex.Message
                    }
                };
            }
        }
    }
}
