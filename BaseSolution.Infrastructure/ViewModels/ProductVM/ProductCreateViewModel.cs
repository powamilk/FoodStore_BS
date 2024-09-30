using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Product.Request;
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
    public class ProductCreateViewModel : ViewModelBase<ProductCreateRequest>
    {
        private readonly IProductReadWriteRepository _productReadWriteRepository;
        private readonly IMapper _mapper;

        public ProductCreateViewModel(IProductReadWriteRepository productReadWriteRepository, IMapper mapper)
        {
            _productReadWriteRepository = productReadWriteRepository;
            _mapper = mapper;
        }

        public override async Task HandleAsync(ProductCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var productEntity = _mapper.Map<Product>(request);
                var createResult = await _productReadWriteRepository.AddProductAsync(productEntity, cancellationToken);

                if (createResult.Success)
                {
                    Success = true;
                    Message = "Product created successfully";
                    Data = createResult.Data;
                }
                else
                {
                    Success = false;
                    ErrorItems = createResult.Errors;
                    Message = createResult.Message;
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = "An error occurred while creating the product.",
                        FieldName = "Product",
                        Code = ex.Message
                    }
                };
            }
        }
    }
}
