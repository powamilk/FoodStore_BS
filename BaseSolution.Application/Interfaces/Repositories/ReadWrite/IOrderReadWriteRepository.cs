using BaseSolution.Application.DataTransferObjects.OrderItem.OrderItemRequest;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadWrite
{
    public interface IOrderReadWriteRepository
    {
        Task<RequestResult<int>> CreateOrderAsync(Order entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> UpdateOrderAsync(Order entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> DeleteOrderAsync(int orderId, CancellationToken cancellationToken);
        Task<RequestResult<int>> DeleteOrderItemAsync(DeleteOrderItemRequest request, CancellationToken cancellationToken);
    }
}
