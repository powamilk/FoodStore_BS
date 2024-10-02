using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Product.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Infrastructure.ViewModels.ProductVM;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BaseSolution.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;
        private readonly IProductReadWriteRepository _productReadWriteRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductCreateRequest> _createProductValidator;
        private readonly IValidator<ProductUpdateRequest> _updateProductValidator;
        private readonly IValidator<ProductDeleteRequest> _deleteProductValidator;

        public ProductController(
            IProductReadOnlyRepository productReadOnlyRepository,
            IProductReadWriteRepository productReadWriteRepository,
            IMapper mapper,
            IValidator<ProductCreateRequest> createProductValidator,
            IValidator<ProductUpdateRequest> updateProductValidator,
            IValidator<ProductDeleteRequest> deleteProductValidator)
        {
            _productReadOnlyRepository = productReadOnlyRepository;
            _productReadWriteRepository = productReadWriteRepository;
            _mapper = mapper;
            _createProductValidator = createProductValidator;
            _updateProductValidator = updateProductValidator;
            _deleteProductValidator = deleteProductValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsWithPagination([FromQuery] ViewProductWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var vm = new ProductListWithPaginationViewModel(_productReadOnlyRepository);
                await vm.HandleAsync(request, cancellationToken);
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var viewModel = new ProductViewModel(_productReadOnlyRepository);
            await viewModel.HandleAsync(id, CancellationToken.None);

            if (!viewModel.Success || viewModel.Data == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Product not found",
                    error_items = viewModel.ErrorItems
                });
            }

            return Ok(new
            {
                success = true,
                data = viewModel.Data
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _createProductValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var vm = new ProductCreateViewModel(_productReadWriteRepository, _mapper);
                await vm.HandleAsync(request, cancellationToken);
                if (vm.Success)
                {
                    return CreatedAtAction(nameof(GetProductById), new { id = vm.Data }, vm);
                }
                return StatusCode(500, new { message = "Internal server error" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequest updateRequest, CancellationToken cancellationToken)
        {
            var validationResult = await _updateProductValidator.ValidateAsync(updateRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (id != updateRequest.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Product ID mismatch"
                });
            }

            var viewModel = new ProductUpdateViewModel(_productReadWriteRepository, _productReadOnlyRepository, _mapper);
            await viewModel.HandleAsync(updateRequest, cancellationToken);

            if (!viewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = viewModel.Message,
                    error_items = viewModel.ErrorItems
                });
            }

            return Ok(new
            {
                success = true,
                message = "Product updated successfully",
                data = viewModel.Data
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            var deleteRequest = new ProductDeleteRequest { Id = id };
            var validationResult = await _deleteProductValidator.ValidateAsync(deleteRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var viewModel = new ProductDeleteViewModel(_productReadWriteRepository);
            await viewModel.HandleAsync(deleteRequest, cancellationToken);

            if (!viewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Product not found",
                    error_items = viewModel.ErrorItems
                });
            }

            return Ok(new
            {
                success = true,
                message = "Product deleted successfully"
            });
        }
    }
}
