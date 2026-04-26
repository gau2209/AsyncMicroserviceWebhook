using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Repository;
using Shared.DTOs;
using Shared.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productRepository;

        public ProductController (IProduct productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddProduct (Product product)
        {
            var response = await _productRepository.AddProductAsync(product);
            return response.flag ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProductAsync () => await _productRepository.GetAllProductAsync( );

    }
}
