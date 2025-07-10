using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;
using server_api.Data;
using server_api.Models;
using server_api.Services;

namespace server_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class ProductsController : ControllerBase
    {
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}

		// GET: api/products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _productService.GetProductsAsync();
			return Ok(products);
		}

		// GET: api/products/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await _productService.GetProductAsync(id);
			if (product == null)
			{ 
				return NotFound();
			}

			return Ok(product);
		}

		// POST: api/products
		[HttpPost]
		public async Task<ActionResult<Product>> CreateProduct(Product product)
		{
			var createdProduct = await _productService.CreateProductAsync(product);
			return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
		}

		// PUT: api/products/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
		{
			if (id != updatedProduct.Id)
			{ 
				return BadRequest();
			}

			var success = await _productService.UpdateProductAsync(id, updatedProduct);
			if (!success)
			{ 
				return NotFound();
			}

			return NoContent();
		}

		// DELETE: api/products/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var success = await _productService.DeleteProductAsync(id);
			if (!success)
			{ 
				return NotFound();
			}

			return NoContent();
		}
	}
}
