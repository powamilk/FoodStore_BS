using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Product.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Infrastructure.ViewModels.ProductVM;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var vm = new ProductViewModel(_productReadOnlyRepository);
                await vm.HandleAsync(id, cancellationToken);
                if (!vm.Success)
                {
                    return NotFound(new { message = "Product not found" });
                }
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var vm = new ProductCreateViewModel(_productReadWriteRepository, _mapper);
                await vm.HandleAsync(request, cancellationToken);
                if (vm.Success)
                {
                    return CreatedAtAction(nameof(GetProductById), new { id = vm.Data }, vm);
                }
                return BadRequest(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new { message = "Product ID mismatch" });
                }

                var vm = new ProductUpdateViewModel(_productReadWriteRepository, _productReadOnlyRepository, _mapper);
                await vm.HandleAsync(request, cancellationToken);

                if (vm.Success)
                {
                    return NoContent();  
                }
                return BadRequest(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new ProductDeleteRequest { Id = id };
                var vm = new ProductDeleteViewModel(_productReadWriteRepository);
                await vm.HandleAsync(request, cancellationToken);

                if (vm.Success)
                {
                    return NoContent(); 
                }
                return BadRequest(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}
