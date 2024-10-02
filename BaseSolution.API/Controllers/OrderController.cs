using AutoMapper;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.ViewModels.OrderVM;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using BaseSolution.Infrastructure.Implements.Repositories.ReadOnly;

namespace BaseSolution.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IValidator<CreateOrderRequest> _createOrderValidator;
        private readonly IValidator<UpdateOrderRequest> _updateOrderValidator;
        private readonly IValidator<DeleteOrderRequest> _deleteOrderValidator;
        private readonly OrderCreateViewModel _orderCreateViewModel;
        private readonly OrderUpdateViewModel _orderUpdateViewModel;
        private readonly OrderDeleteViewModel _orderDeleteViewModel;
        private readonly OrderListWithPaginationViewModel _orderListWithPaginationViewModel;
        private readonly OrderViewModel _orderViewModel;
        private readonly IOrderReadOnlyRepository _orderReadOnlyRepository; // Add this to fetch order details after creation

        public OrderController(
            IValidator<CreateOrderRequest> createOrderValidator,
            IValidator<UpdateOrderRequest> updateOrderValidator,
            IValidator<DeleteOrderRequest> deleteOrderValidator,
            OrderCreateViewModel orderCreateViewModel,
            OrderUpdateViewModel orderUpdateViewModel,
            OrderDeleteViewModel orderDeleteViewModel,
            OrderListWithPaginationViewModel orderListWithPaginationViewModel,
            OrderViewModel orderViewModel,
            IOrderReadOnlyRepository orderReadOnlyRepository // Inject IOrderReadOnlyRepository
        )
        {
            _createOrderValidator = createOrderValidator;
            _updateOrderValidator = updateOrderValidator;
            _deleteOrderValidator = deleteOrderValidator;
            _orderCreateViewModel = orderCreateViewModel;
            _orderUpdateViewModel = orderUpdateViewModel;
            _orderDeleteViewModel = orderDeleteViewModel;
            _orderListWithPaginationViewModel = orderListWithPaginationViewModel;
            _orderViewModel = orderViewModel;
            _orderReadOnlyRepository = orderReadOnlyRepository; // Assign IOrderReadOnlyRepository
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
            var validationResult = await _createOrderValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _orderCreateViewModel.HandleAsync(request, cancellationToken);
            if (!_orderCreateViewModel.Success)
                return BadRequest(_orderCreateViewModel.ErrorItems);

            var orderDetails = await _orderReadOnlyRepository.GetOrderByIdAsync(_orderCreateViewModel.Data, cancellationToken);
            if (orderDetails == null || !orderDetails.Success)
            {
                return StatusCode(500, new { message = "Failed to retrieve order details" });
            }
            return CreatedAtAction(nameof(GetOrderById), new { id = _orderCreateViewModel.Data }, orderDetails.Data);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequest request)
        {
            var validationResult = await _updateOrderValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (id != request.Id)
            {
                return BadRequest(new { success = false, message = "Order ID mismatch" });
            }

            await _orderUpdateViewModel.HandleAsync(request, CancellationToken.None);
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
            var request = new DeleteOrderRequest { Id = id };

            var validationResult = await _deleteOrderValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _orderDeleteViewModel.HandleAsync(request, CancellationToken.None);
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
