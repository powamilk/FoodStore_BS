using BaseSolution.Application.DataTransferObjects.Example.Request;
using Microsoft.AspNetCore.Mvc;
using BaseSolution.Application.Interfaces.Services;
using AutoMapper;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Infrastructure.ViewModels.Categories;

namespace BaseSolution.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
        private readonly ICategoryReadWriteRepository _categoryReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public CategoryController(
            ICategoryReadOnlyRepository categoryReadOnlyRepository,
            ICategoryReadWriteRepository categoryReadWriteRepository,
            ILocalizationService localizationService,
            IMapper mapper)
        {
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _categoryReadWriteRepository = categoryReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesWithPagination([FromQuery] ViewCategoryWithPaginationRequest request, CancellationToken cancellationToken)
        {
            var vm = new CategoryListWithPaginationViewModel(_categoryReadOnlyRepository, _localizationService);
            await vm.HandleAsync(request, cancellationToken);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var viewModel = new CategoryViewModel(_categoryReadOnlyRepository, _localizationService);
            await viewModel.HandleAsync(id, CancellationToken.None);

            if (!viewModel.Success || viewModel.Data == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Category not found",
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
        public async Task<IActionResult> CreateCategory(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            var vm = new CategoryCreateViewModel(_categoryReadWriteRepository, _localizationService, _mapper, _categoryReadOnlyRepository);
            await vm.HandleAsync(request, cancellationToken);
            return Ok(vm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateRequest updateRequest)
        {
            if (id != updateRequest.Id)
            {
                return BadRequest(new { success = false, message = "Category ID mismatch" });
            }

            var viewModel = new CategoryUpdateViewModel(_categoryReadWriteRepository, _categoryReadOnlyRepository, _mapper, _localizationService);
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
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var viewModel = new CategoryDeleteViewModel(_categoryReadWriteRepository, _localizationService);
            var deleteRequest = new CategoryDeleteRequest { Id = id };
            await viewModel.HandleAsync(deleteRequest, CancellationToken.None);

            if (!viewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Category not found",
                    error_items = viewModel.ErrorItems
                });
            }

            return NoContent(); 
        }
    }
}
