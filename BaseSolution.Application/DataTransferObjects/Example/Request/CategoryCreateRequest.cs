using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Example.Request
{
    public class CategoryCreateRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
