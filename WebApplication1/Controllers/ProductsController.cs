using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Services;
using WebApplication1.Extensions;
using WebApplication1.Resources;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await _productService.ListAsync();
            if (!results.Success)
            {
                return BadRequest(results.Message);
            }
            var resources = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResource>>(results.Product);
            return Ok(resources);
        }

        [HttpGet("{id}", Name = "NewProduct")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _productService.FindByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            var resource = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResource>>(result.Product);
            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveProductResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var product = _mapper.Map<SaveProductResource, Product>(resource);
            var result = await _productService.SaveAsync(product);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var productResource = (List<ProductResource>)_mapper.Map<IEnumerable<Product>, IEnumerable<ProductResource>>(result.Product);
            return CreatedAtRoute("NewProduct", new { id = productResource[0].Id.ToString() }, productResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveProductResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var product = _mapper.Map<SaveProductResource, Product>(resource);
            var result = await _productService.UpdateAsync(id, product);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var productResource = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResource>>(result.Product);
            return Ok(productResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _productService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var productResource = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResource>>(result.Product);
            return Ok(productResource);
        }
    }
}
