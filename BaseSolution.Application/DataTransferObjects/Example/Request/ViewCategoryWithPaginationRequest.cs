using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseSolution.Application.ValueObjects.Pagination;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Example.Request
{
    public class ViewCategoryWithPaginationRequest : PaginationRequest
    {
        public string? Name { get; set; }
        public string? SortField { get; set; }
        public string? SortDirection { get; set; }
    }
}
