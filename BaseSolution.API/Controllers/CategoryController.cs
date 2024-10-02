using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Example.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.ViewModels.Categories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IValidator<CategoryCreateRequest> _createCategoryValidator;
        private readonly IValidator<CategoryUpdateRequest> _updateCategoryValidator;
        private readonly IValidator<CategoryDeleteRequest> _deleteCategoryValidator;

        public CategoryController(
            ICategoryReadOnlyRepository categoryReadOnlyRepository,
            ICategoryReadWriteRepository categoryReadWriteRepository,
            ILocalizationService localizationService,
            IMapper mapper,
            IValidator<CategoryCreateRequest> createCategoryValidator,
            IValidator<CategoryUpdateRequest> updateCategoryValidator,
            IValidator<CategoryDeleteRequest> deleteCategoryValidator)
        {
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _categoryReadWriteRepository = categoryReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _createCategoryValidator = createCategoryValidator;
            _updateCategoryValidator = updateCategoryValidator;
            _deleteCategoryValidator = deleteCategoryValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesWithPagination([FromQuery] ViewCategoryWithPaginationRequest request, CancellationToken cancellationToken)
        {
            CategoryListWithPaginationViewModel vm = new(_categoryReadOnlyRepository, _localizationService);

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
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _createCategoryValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var vm = new CategoryCreateViewModel(_categoryReadWriteRepository, _localizationService, _mapper, _categoryReadOnlyRepository);
            await vm.HandleAsync(request, cancellationToken);

            if (!vm.Success)
            {
                return BadRequest(vm.ErrorItems);
            }

            return Ok(vm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateRequest updateRequest, CancellationToken cancellationToken)
        {
            var validationResult = await _updateCategoryValidator.ValidateAsync(updateRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (id != updateRequest.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Category ID mismatch"
                });
            }

            var viewModel = new CategoryUpdateViewModel(_categoryReadWriteRepository, _categoryReadOnlyRepository, _mapper, _localizationService);
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
                message = "Category updated successfully",
                data = viewModel.Data
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            var deleteRequest = new CategoryDeleteRequest { Id = id };
            var validationResult = await _deleteCategoryValidator.ValidateAsync(deleteRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var viewModel = new CategoryDeleteViewModel(_categoryReadWriteRepository, _localizationService);
            await viewModel.HandleAsync(deleteRequest, cancellationToken);

            if (!viewModel.Success)
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
                message = "Category deleted successfully"
            });
        }
    }
}
