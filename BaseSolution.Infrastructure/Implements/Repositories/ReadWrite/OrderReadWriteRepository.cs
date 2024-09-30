using BaseSolution.Application.DataTransferObjects.OrderItem.OrderItemRequest;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.Database.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadWrite
{
    public class OrderReadWriteRepository : IOrderReadWriteRepository
    {
        private readonly ProductReadWriteDbContext _dbContext;
        private readonly ILocalizationService _localizationService;

        public OrderReadWriteRepository(ProductReadWriteDbContext dbContext, ILocalizationService localizationService)
        {
            _dbContext = dbContext;
            _localizationService = localizationService;
        }

        public async Task<RequestResult<int>> CreateOrderAsync(Order entity, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Orders.AddAsync(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception e)
            {
                return RequestResult<int>.Fail(_localizationService["Unable to create order"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToCreate + "order"
                    }
                });
            }
        }

        public async Task<RequestResult<int>> UpdateOrderAsync(Order entity, CancellationToken cancellationToken)
        {
            try
            {
                var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == entity.Id, cancellationToken);
                if (existingOrder == null)
                {
                    return RequestResult<int>.Fail("Order not found");
                }

                existingOrder.CustomerId = entity.CustomerId;
                existingOrder.OrderDate = entity.OrderDate;
                existingOrder.TotalAmount = entity.TotalAmount;

                _dbContext.Orders.Update(existingOrder);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<int>.Succeed(1);
            }
            catch (Exception e)
            {
                return RequestResult<int>.Fail(_localizationService["Unable to update order"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToUpdate + "order"
                    }
                });
            }
        }

        public async Task<RequestResult<int>> DeleteOrderAsync(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
                if (order == null)
                {
                    return RequestResult<int>.Fail("Order not found");
                }

                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<int>.Succeed(1);
            }
            catch (Exception e)
            {
                return RequestResult<int>.Fail(_localizationService["Unable to delete order"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = LocalizationString.Common.FailedToDelete + "order"
                    }
                });
            }
        }

        public async Task<RequestResult<int>> DeleteOrderItemAsync(DeleteOrderItemRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var orderItem = await _dbContext.OrderItems
                    .FirstOrDefaultAsync(oi => oi.OrderId == request.OrderId && oi.ProductId == request.ProductId, cancellationToken);

                if (orderItem == null)
                {
                    return RequestResult<int>.Fail("Order item not found");
                }

                _dbContext.OrderItems.Remove(orderItem);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return RequestResult<int>.Succeed(1);
            }
            catch (Exception e)
            {
                return RequestResult<int>.Fail("Unable to delete order item", new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = "OrderItem"
                    }
                });
            }
        }
    }
}
