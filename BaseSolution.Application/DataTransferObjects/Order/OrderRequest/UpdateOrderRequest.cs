using BaseSolution.Application.DataTransferObjects.OrderItem.OrderItemRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Order.OrderRequest
{
    public class UpdateOrderRequest
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemUpdateRequest> Items { get; set; } = new List<OrderItemUpdateRequest>();
        public int Id { get; set; }
    }
}
