using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Application.DataTransferObjects.OrderItem.OrderItemRequest;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
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
        private readonly IOrderReadOnlyRepository _orderReadOnlyRepository;
        private readonly ILocalizationService _localizationService;

        public OrderDeleteViewModel(IOrderReadWriteRepository orderReadWriteRepository, IOrderReadOnlyRepository orderReadOnlyRepository, ILocalizationService localizationService)
        {
            _orderReadWriteRepository = orderReadWriteRepository;
            _orderReadOnlyRepository = orderReadOnlyRepository;
            _localizationService = localizationService;
        }

        public override async Task HandleAsync(DeleteOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var orderResult = await _orderReadOnlyRepository.GetOrderByIdAsync(request.Id, cancellationToken);
                if (!orderResult.Success || orderResult.Data == null)
                {
                    Success = false;
                    ErrorItems = new[]
                    {
                        new ErrorItem
                        {
                            FieldName = "Order",
                            Error = _localizationService["Order not found"]
                        }
                    };
                    Message = _localizationService["Order not found"];
                    return;
                }
                var order = orderResult.Data;

                foreach (var item in order.OrderItems)
                {
                    await _orderReadWriteRepository.DeleteOrderItemAsync(new DeleteOrderItemRequest
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId
                    }, cancellationToken);
                }
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
                        FieldName = "Order",
                        Error = _localizationService["An error occurred while deleting the order."],
                        Code = ex.Message
                    }
                };
            }
        }
    }
}
