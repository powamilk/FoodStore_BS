using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Product.Request
{
    public class ProductUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
    }
}
