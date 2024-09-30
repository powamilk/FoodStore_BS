using BaseSolution.Application.ValueObjects.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Product.Request
{
    public class ViewProductWithPaginationRequest : PaginationRequest
    {
        public string? Name { get; set; }
        public string? SortField { get; set; }
        public string? SortDirection { get; set; }
    }
}
