using BaseSolution.Application.DataTransferObjects.Product.Request;
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
    public class ProductListWithPaginationViewModel : ViewModelBase<ViewProductWithPaginationRequest>
    {
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;

        public ProductListWithPaginationViewModel(IProductReadOnlyRepository productReadOnlyRepository)
        {
            _productReadOnlyRepository = productReadOnlyRepository;
        }

        public override async Task HandleAsync(ViewProductWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productReadOnlyRepository.GetProductsWithPaginationAsync(request, cancellationToken);

                Success = result.Success;
                Data = result.Data;
                Message = result.Message;
                ErrorItems = result.Errors;
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = "An error occurred while retrieving the list of products.",
                        FieldName = "Product",
                        Code = ex.Message
                    }
                };
            }
        }
    }
}
