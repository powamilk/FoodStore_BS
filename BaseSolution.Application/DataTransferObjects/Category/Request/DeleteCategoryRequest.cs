using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Category.Request
{
    public class DeleteCategoryRequest
    {
        public int Id { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
