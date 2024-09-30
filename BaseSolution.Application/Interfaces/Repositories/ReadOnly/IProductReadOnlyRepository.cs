using BaseSolution.Application.DataTransferObjects.Product;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Application.ValueObjects.Pagination;
using BaseSolution.Application.DataTransferObjects.Product.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseSolution.Domain.Entities;

namespace BaseSolution.Application.Interfaces.Repositories.ReadOnly
{
    public interface IProductReadOnlyRepository
    {
        Task<RequestResult<ProductDto?>> GetProductByIdAsync(int id, CancellationToken cancellationToken);
        Task<RequestResult<PaginationResponse<Product>>> GetProductsWithPaginationAsync(ViewProductWithPaginationRequest request, CancellationToken cancellationToken);
    }
}
