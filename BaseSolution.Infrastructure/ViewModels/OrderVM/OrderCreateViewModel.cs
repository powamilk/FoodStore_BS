using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseSolution.Infrastructure.ViewModels.OrderVM
{
    public class OrderCreateViewModel : ViewModelBase<CreateOrderRequest>
    {
        private readonly IOrderReadWriteRepository _orderReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public OrderCreateViewModel(IOrderReadWriteRepository orderReadWriteRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _orderReadWriteRepository = orderReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var orderEntity = _mapper.Map<Order>(request);
                var createResult = await _orderReadWriteRepository.CreateOrderAsync(orderEntity, cancellationToken);

                if (createResult.Success)
                {
                    Success = true;
                    Message = _localizationService["Order created successfully"];
                    Data = createResult.Data;
                }
                else
                {
                    Success = false;
                    ErrorItems = createResult.Errors;
                    Message = createResult.Message;
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = _localizationService["Error occurred while creating the order"],
                        FieldName = LocalizationString.Common.FailedToCreate + "Order",
                        Code = ex.Message
                    }
                };
            }
        }
    }
}
