using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
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
    public class OrderListWithPaginationViewModel : ViewModelBase<ViewOrderWithPaginationRequest>
    {
        private readonly IOrderReadOnlyRepository _orderReadOnlyRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public OrderListWithPaginationViewModel(IOrderReadOnlyRepository orderReadOnlyRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _orderReadOnlyRepository = orderReadOnlyRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        public override async Task HandleAsync(ViewOrderWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var paginationResult = await _orderReadOnlyRepository.GetOrdersWithPaginationAsync(request, cancellationToken);

                Data = paginationResult.Data!;
                Success = paginationResult.Success;
                ErrorItems = paginationResult.Errors;
                Message = paginationResult.Message;
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = _localizationService["Error occurred while getting the list of orders"],
                        FieldName = LocalizationString.Common.FailedToGet + "list of Orders",
                        Code = ex.Message
                    }
                };
            }
        }
    }
}
