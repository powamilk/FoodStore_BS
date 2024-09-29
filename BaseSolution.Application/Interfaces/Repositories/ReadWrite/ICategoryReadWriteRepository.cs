using BaseSolution.Application.DataTransferObjects.Example.Request;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.Interfaces.Repositories.ReadWrite
{
    public interface ICategoryReadWriteRepository
    {
        Task<RequestResult<int>> AddCategoryAsync(Category entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> UpdateCategoryAsync(Category entity, CancellationToken cancellationToken);
        Task<RequestResult<int>> DeleteCategoryAsync(CategoryDeleteRequest request, CancellationToken cancellationToken);
    }
}
