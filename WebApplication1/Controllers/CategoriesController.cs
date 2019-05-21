using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Services;
using WebApplication1.Extensions;
using WebApplication1.Mapping;
using WebApplication1.Resources;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await _categoryService.ListAsync();
            if (!results.Success)
            {
                return BadRequest(results.Message);
            }
            var resources = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(results.Category);
            return Ok(resources);
            //var categories = await _categoryService.ListAsync();
            //var resources = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);
            //return resources;
        }

        [HttpGet("{id}", Name = "NewCategory")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _categoryService.FindByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            var resource = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(result.Category);
            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveCategoryResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var category = _mapper.Map<SaveCategoryResource, Category>(resource);
            var result = await _categoryService.SaveAsync(category);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryResource = (List<CategoryResource>)_mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(result.Category);
            return CreatedAtRoute("NewCategory", new { id = categoryResource[0].Id.ToString() }, categoryResource);
            //return Ok(categoryResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveCategoryResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var category = _mapper.Map<SaveCategoryResource, Category>(resource);
            var result = await _categoryService.UpdateAsync(id, category);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryResource = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(result.Category);
            return Ok(categoryResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _categoryService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Message);

            var categoryResource = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(result.Category);
            return Ok(categoryResource);
        }
    }
}