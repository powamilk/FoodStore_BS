using BaseSolution.Application.DataTransferObjects.Product.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.ProductVM
{
    public class ProductDeleteViewModel : ViewModelBase<ProductDeleteRequest>
    {
        private readonly IProductReadWriteRepository _productReadWriteRepository;

        public ProductDeleteViewModel(IProductReadWriteRepository productReadWriteRepository)
        {
            _productReadWriteRepository = productReadWriteRepository;
        }

        public override async Task HandleAsync(ProductDeleteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productReadWriteRepository.DeleteProductAsync(request.Id, cancellationToken);

                Success = result.Success;
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
                Error = "An error occurred while deleting the product.",
                FieldName = "Product",
                Code = ex.Message
            }
        };
            }
        }

    }
}
