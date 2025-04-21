
using BackendPractice.ProductModel;
using BackendPractice.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendPractice.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ProductController : ControllerBase
        {
            private readonly IProductRepository productRepository;

            public ProductController(IProductRepository productRepository)
            {
                this.productRepository = productRepository;
            }

            [HttpPost]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> AddProduct([FromForm] ProductUIModel productUIModel)
            {
                if (await productRepository.AddProduct(productUIModel))
                {
                    return Ok(new { message = "Product added successfully." });
                }

                return BadRequest(new { message = "Product already exists." });
            }

            [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUIModel productUIModel)
            {
                if (await productRepository.UpdateProduct(id, productUIModel))
                {
                    return Ok(new { message = "Product updated successfully." });
                }

                return NotFound(new { message = "Product not found." });
            }

            [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
            {
                if (await productRepository.DeleteProduct(id))
                {
                    return Ok(new { message = "Product deleted successfully." });
                }

                return NotFound(new { message = "Product not found." });
            }

            [HttpGet("{id}")]
       
        public async Task<IActionResult> GetProduct(int id)
            {
                var product = await productRepository.GetProduct(id);

                if (product != null)
                {
                    return Ok( new
                    {
                        err = 0,
                        msg = "Successfully fetched",
                        data = product
                    } );
                }

                return NotFound(new { err = 1, message = "Product not found." });
            }

            [HttpGet]
            public async Task<IActionResult> GetAllProducts()
            {
                var products = await productRepository.GetAllProducts();
            return Ok(new
            {
                err = 0,
                msg = "Successfully fetched",
                data = products
            });
            }
        }
    }
