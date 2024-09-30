using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.OrderItem.OrderItemRequest
{
    public class OrderItemUpdateRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
