using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Product.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Infrastructure.ViewModels.ProductVM;
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

        public ProductController(
            IProductReadOnlyRepository productReadOnlyRepository,
            IProductReadWriteRepository productReadWriteRepository,
            IMapper mapper)
        {
            _productReadOnlyRepository = productReadOnlyRepository;
            _productReadWriteRepository = productReadWriteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsWithPagination([FromQuery] ViewProductWithPaginationRequest request, CancellationToken cancellationToken)
        {
            var vm = new ProductListWithPaginationViewModel(_productReadOnlyRepository);
            await vm.HandleAsync(request, cancellationToken);
            return Ok(vm);
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
            var vm = new ProductCreateViewModel(_productReadWriteRepository, _mapper);
            await vm.HandleAsync(request, cancellationToken);
            return Ok(vm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequest updateRequest)
        {
            if (id != updateRequest.Id)
            {
                return BadRequest(new { success = false, message = "Product ID mismatch" });
            }

            var viewModel = new ProductUpdateViewModel(_productReadWriteRepository, _productReadOnlyRepository, _mapper);
            await viewModel.HandleAsync(updateRequest, CancellationToken.None);

            if (!viewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = viewModel.Message,
                    error_items = viewModel.ErrorItems
                });
            }

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var viewModel = new ProductDeleteViewModel(_productReadWriteRepository);
            var deleteRequest = new ProductDeleteRequest { Id = id };
            await viewModel.HandleAsync(deleteRequest, CancellationToken.None);

            if (!viewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Product not found",
                    error_items = viewModel.ErrorItems
                });
            }

            return NoContent(); 
        }
    }
}
