using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.OrderVM
{
    public class OrderDeleteViewModel : ViewModelBase<DeleteOrderRequest>
    {
        private readonly IOrderReadWriteRepository _orderReadWriteRepository;
        private readonly ILocalizationService _localizationService;

        public OrderDeleteViewModel(IOrderReadWriteRepository orderReadWriteRepository, ILocalizationService localizationService)
        {
            _orderReadWriteRepository = orderReadWriteRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(DeleteOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await _orderReadWriteRepository.DeleteOrderAsync(request.Id, cancellationToken);

                Success = deleteResult.Success;
                Message = deleteResult.Message;
                ErrorItems = deleteResult.Errors;
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = _localizationService["Error occurred while deleting the order"],
                        FieldName = LocalizationString.Common.FailedToDelete + "Order",
                        Code = ex.Message
                    }
                };
            }
        }
    }
}
