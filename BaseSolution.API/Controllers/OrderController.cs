using AutoMapper;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.ViewModels.OrderVM;
using Microsoft.AspNetCore.Mvc;

namespace BaseSolution.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderCreateViewModel _orderCreateViewModel;
        private readonly OrderUpdateViewModel _orderUpdateViewModel;
        private readonly OrderDeleteViewModel _orderDeleteViewModel;
        private readonly OrderListWithPaginationViewModel _orderListWithPaginationViewModel;
        private readonly OrderViewModel _orderViewModel;
        private readonly ILocalizationService _localizationService;

        public OrderController(
            OrderCreateViewModel orderCreateViewModel,
            OrderUpdateViewModel orderUpdateViewModel,
            OrderDeleteViewModel orderDeleteViewModel,
            OrderListWithPaginationViewModel orderListWithPaginationViewModel,
            OrderViewModel orderViewModel,
            ILocalizationService localizationService)
        {
            _orderCreateViewModel = orderCreateViewModel;
            _orderUpdateViewModel = orderUpdateViewModel;
            _orderDeleteViewModel = orderDeleteViewModel;
            _orderListWithPaginationViewModel = orderListWithPaginationViewModel;
            _orderViewModel = orderViewModel;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersWithPagination([FromQuery] ViewOrderWithPaginationRequest request, CancellationToken cancellationToken)
        {
            await _orderListWithPaginationViewModel.HandleAsync(request, cancellationToken);
            if (!_orderListWithPaginationViewModel.Success)
                return BadRequest(_orderListWithPaginationViewModel.ErrorItems);

            return Ok(_orderListWithPaginationViewModel.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            await _orderViewModel.HandleAsync(id, CancellationToken.None);
            if (!_orderViewModel.Success || _orderViewModel.Data == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Order not found",
                    error_items = _orderViewModel.ErrorItems
                });
            }

            return Ok(new
            {
                success = true,
                data = _orderViewModel.Data
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            await _orderCreateViewModel.HandleAsync(request, cancellationToken);
            if (!_orderCreateViewModel.Success)
                return BadRequest(_orderCreateViewModel.ErrorItems);

            return CreatedAtAction(nameof(GetOrderById), new { id = _orderCreateViewModel.Data }, _orderCreateViewModel.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequest updateRequest)
        {
            if (id != updateRequest.Id)
            {
                return BadRequest(new { success = false, message = "Order ID mismatch" });
            }

            await _orderUpdateViewModel.HandleAsync(updateRequest, CancellationToken.None);
            if (!_orderUpdateViewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = _orderUpdateViewModel.Message,
                    error_items = _orderUpdateViewModel.ErrorItems
                });
            }

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleteRequest = new DeleteOrderRequest { Id = id };
            await _orderDeleteViewModel.HandleAsync(deleteRequest, CancellationToken.None);
            if (!_orderDeleteViewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Order not found",
                    error_items = _orderDeleteViewModel.ErrorItems
                });
            }

            return NoContent(); 
        }
    }
}
