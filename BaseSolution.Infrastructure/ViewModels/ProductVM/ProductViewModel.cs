using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseSolution.Infrastructure.ViewModels.ProductVM
{
    public class ProductViewModel : ViewModelBase<int>
    {
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;

        public ProductViewModel(IProductReadOnlyRepository productReadOnlyRepository)
        {
            _productReadOnlyRepository = productReadOnlyRepository;
        }

        public override async Task HandleAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productReadOnlyRepository.GetProductByIdAsync(id, cancellationToken);

                Success = result.Success;
                Data = result.Data;
                ErrorItems = result.Errors;
                Message = result.Message;
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = "An error occurred while retrieving the product.",
                        FieldName = "Product",
                        Code = ex.Message
                    }
                };
            }
        }
    }
}
