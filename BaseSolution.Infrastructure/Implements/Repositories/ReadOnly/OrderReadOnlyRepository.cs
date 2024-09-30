using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Application.DataTransferObjects.Order;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
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
using BaseSolution.Application.ValueObjects.Pagination;
using BaseSolution.Infrastructure.Extensions;
using AutoMapper.QueryableExtensions;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadOnly
{
    public class OrderReadOnlyRepository : IOrderReadOnlyRepository
    {
        private readonly DbSet<Order> _orders;
        private readonly IMapper _mapper;
        private readonly OrderReadOnlyDbContext _dbContext;
        private readonly ILocalizationService _localizationService;

        public OrderReadOnlyRepository(IMapper mapper, ILocalizationService localizationService, OrderReadOnlyDbContext dbContext)
        {
            _orders = dbContext.Set<Order>();
            _mapper = mapper;
            _dbContext = dbContext;
            _localizationService = localizationService;
        }

        public async Task<RequestResult<OrderDto?>> GetOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orders.AsNoTracking()
                    .Where(o => o.Id == id)
                    .Include(o => o.OrderItems) 
                    .ThenInclude(oi => oi.Product) 
                    .ProjectTo<OrderDto>(_mapper.ConfigurationProvider) 
                    .FirstOrDefaultAsync(cancellationToken);

                return RequestResult<OrderDto?>.Succeed(order);
            }
            catch (Exception e)
            {
                return RequestResult<OrderDto?>.Fail(_localizationService["Order is not found"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = "Order"
                    }
                });
            }
        }

        public async Task<RequestResult<PaginationResponse<Order>>> GetOrdersWithPaginationAsync(ViewOrderWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _orders.AsNoTracking()
                    .Include(o => o.OrderItems) 
                    .ThenInclude(oi => oi.Product) 
                    .AsQueryable();


                var paginationResult = await query.PaginateAsync<Order>(request, cancellationToken);

                return RequestResult<PaginationResponse<Order>>.Succeed(paginationResult);
            }
            catch (Exception e)
            {
                return RequestResult<PaginationResponse<Order>>.Fail(_localizationService["Error retrieving orders"], new[]
                {
                    new ErrorItem
                    {
                        Error = e.Message,
                        FieldName = "Order"
                    }
                });
            }
        }
    }
}
