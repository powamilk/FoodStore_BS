using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Product.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using BaseSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseSolution.Infrastructure.ViewModels.ProductVM
{
    public class ProductUpdateViewModel : ViewModelBase<ProductUpdateRequest>
    {
        private readonly IProductReadWriteRepository _productReadWriteRepository;
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;
        private readonly IMapper _mapper;

        public ProductUpdateViewModel(
            IProductReadWriteRepository productReadWriteRepository,
            IProductReadOnlyRepository productReadOnlyRepository,
            IMapper mapper)
        {
            _productReadWriteRepository = productReadWriteRepository;
            _productReadOnlyRepository = productReadOnlyRepository;
            _mapper = mapper;
        }

        public override async Task HandleAsync(ProductUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var updateResult = await _productReadWriteRepository.UpdateProductAsync(_mapper.Map<Product>(request), cancellationToken);

                if (updateResult.Success)
                {
                    var result = await _productReadOnlyRepository.GetProductByIdAsync(updateResult.Data, cancellationToken);

                    Success = true;
                    Message = "Product updated successfully";
                    Data = result.Data;
                }
                else
                {
                    Success = false;
                    ErrorItems = updateResult.Errors;
                    Message = updateResult.Message;
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = "An error occurred while updating the product.",
                        FieldName = "Product",
                        Code = ex.Message
                    }
                };
            }
        }
    }
}
