using BaseSolution.Application.ValueObjects.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Order.OrderRequest
{
    public class ViewOrderWithPaginationRequest : PaginationRequest
    {
        public int? CustomerId { get; set; } 
        public DateTime? OrderDate { get; set; } 

        public string? SortField { get; set; } 
        public string? SortDirection { get; set; }
    }
}
