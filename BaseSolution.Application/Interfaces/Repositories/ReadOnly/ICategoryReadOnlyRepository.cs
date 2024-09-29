using BaseSolution.Application.DataTransferObjects.Category;
using BaseSolution.Application.DataTransferObjects.Example.Request;
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
    public interface ICategoryReadOnlyRepository
    {
        Task<RequestResult<CategoryDto?>> GetCategoryByIdAsync(int id, CancellationToken cancellationToken);
        Task<RequestResult<PaginationResponse<Category>>> GetCategoryWithPaginationAsync(ViewCategoryWithPaginationRequest request, CancellationToken cancellationToken);
    }
}
