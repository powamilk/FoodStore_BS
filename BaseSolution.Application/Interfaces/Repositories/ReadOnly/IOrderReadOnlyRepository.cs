using BaseSolution.Application.DataTransferObjects.Order;
using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Application.ValueObjects.Pagination;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadOnly
{
    public interface IOrderReadOnlyRepository
    {
        Task<RequestResult<OrderDto?>> GetOrderByIdAsync(int id, CancellationToken cancellationToken);
        Task<RequestResult<PaginationResponse<Order>>> GetOrdersWithPaginationAsync(ViewOrderWithPaginationRequest request, CancellationToken cancellationToken);
    }
}
