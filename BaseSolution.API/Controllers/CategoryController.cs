using BaseSolution.Application.DataTransferObjects.Example.Request;
using Microsoft.AspNetCore.Mvc;
using BaseSolution.Application.Interfaces.Services;
using AutoMapper;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Infrastructure.Implements.Repositories.ReadWrite;
using BaseSolution.Infrastructure.Implements.Repositories.ReadOnly;
using BaseSolution.Infrastructure.ViewModels.Example;
using Azure.Core;
using BaseSolution.Infrastructure.ViewModels.Categories;

namespace BaseSolution.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
        public readonly ICategoryReadWriteRepository _categoryReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryReadOnlyRepository categoryReadOnlyRepository, ICategoryReadWriteRepository categoryeReadWriteRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _categoryReadWriteRepository = categoryeReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesWithPagination([FromQuery] ViewCategoryWithPaginationRequest request, CancellationToken cancellationToken)
        {
            CategoryListWithPaginationViewModel vm = new(_categoryReadOnlyRepository, _localizationService);

            await vm.HandleAsync(request, cancellationToken);

            return Ok(vm);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            CategoryViewModel vm = new(_categoryReadOnlyRepository, _localizationService);

            await vm.HandleAsync(id, cancellationToken);

            return Ok(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            CategoryCreateViewModel vm = new (_categoryReadWriteRepository, _localizationService, _mapper , _categoryReadOnlyRepository);

            await vm.HandleAsync(request, cancellationToken);

            return Ok(vm);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory( CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            CategoryUpdateViewModel vm = new( _categoryReadWriteRepository, _categoryReadOnlyRepository, _mapper, _localizationService );

            await vm.HandleAsync(request, cancellationToken);

            return Ok(vm);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(CategoryDeleteRequest request, CancellationToken cancellationToken)
        {
            CategoryDeleteViewModel vm = new ( _categoryReadWriteRepository , _localizationService);

            await vm.HandleAsync(request, cancellationToken);

            return Ok(vm);
        }
    }
}
