using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.OrderVM
{
    public class OrderUpdateViewModel : ViewModelBase<UpdateOrderRequest>
    {
        private readonly IOrderReadWriteRepository _orderReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public OrderUpdateViewModel(IOrderReadWriteRepository orderReadWriteRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _orderReadWriteRepository = orderReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var orderEntity = _mapper.Map<Order>(request);
                var updateResult = await _orderReadWriteRepository.UpdateOrderAsync(orderEntity, cancellationToken);

                if (updateResult.Success)
                {
                    Success = true;
                    Message = _localizationService["Order updated successfully"];
                    Data = updateResult.Data;
                }
                else
                {

                    Success = false;
                    ErrorItems = updateResult.Errors;
                    Message = updateResult.Message ?? _localizationService["Unable to update the order."];
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = _localizationService["An unexpected error occurred while updating the order."],
                        FieldName = LocalizationString.Common.FailedToUpdate + "Order",
                        Code = ex.Message 
                    }
                };
            }
        }
    }
}
