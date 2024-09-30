using BaseSolution.Application.DataTransferObjects.Product.Request;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadWrite
{
    public interface IProductReadWriteRepository
    {
        Task<RequestResult<int>> AddProductAsync(Product entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> DeleteProductAsync(int productId, CancellationToken cancellationToken);
        Task<RequestResult<int>> UpdateProductAsync(Product entity, CancellationToken cancellationToken);
    }
}
