using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Infrastructure.ViewModels.OrderVM;
using Microsoft.AspNetCore.Http;
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

        public OrderController(
            OrderCreateViewModel orderCreateViewModel,
            OrderUpdateViewModel orderUpdateViewModel,
            OrderDeleteViewModel orderDeleteViewModel,
            OrderListWithPaginationViewModel orderListWithPaginationViewModel,
            OrderViewModel orderViewModel)
        {
            _orderCreateViewModel = orderCreateViewModel;
            _orderUpdateViewModel = orderUpdateViewModel;
            _orderDeleteViewModel = orderDeleteViewModel;
            _orderListWithPaginationViewModel = orderListWithPaginationViewModel;
            _orderViewModel = orderViewModel;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersWithPagination([FromQuery] ViewOrderWithPaginationRequest request, CancellationToken cancellationToken)
        {
            await _orderListWithPaginationViewModel.HandleAsync(request, cancellationToken);
            if (!_orderListWithPaginationViewModel.Success)
                return BadRequest(_orderListWithPaginationViewModel.ErrorItems);

            return Ok(_orderListWithPaginationViewModel.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id, CancellationToken cancellationToken)
        {
            await _orderViewModel.HandleAsync(id, cancellationToken);
            if (!_orderViewModel.Success)
                return NotFound(_orderViewModel.ErrorItems);

            return Ok(_orderViewModel.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            await _orderCreateViewModel.HandleAsync(request, cancellationToken);
            if (!_orderCreateViewModel.Success)
                return BadRequest(_orderCreateViewModel.ErrorItems);

            return CreatedAtAction(nameof(GetOrderById), new { id = _orderCreateViewModel.Data }, _orderCreateViewModel.Data);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
                return BadRequest("Order ID mismatch");

            await _orderUpdateViewModel.HandleAsync(request, cancellationToken);
            if (!_orderUpdateViewModel.Success)
                return BadRequest(_orderUpdateViewModel.ErrorItems);

            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
        {
            var request = new DeleteOrderRequest { Id = id };
            await _orderDeleteViewModel.HandleAsync(request, cancellationToken);
            if (!_orderDeleteViewModel.Success)
                return BadRequest(_orderDeleteViewModel.ErrorItems);

            return NoContent();
        }
    }
}
