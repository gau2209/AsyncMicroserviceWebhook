using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Repository;
using Shared.DTOs;
using Shared.Models;

namespace ProductAPI.Service
{
    public class ProductService : IProduct
    {
        private readonly ProductDbContext _ProductDBContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductService (ProductDbContext ProductDBContext, IPublishEndpoint publishEndpoint)
        {
            _ProductDBContext = ProductDBContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ServiceResponse> AddProductAsync (Product product)
        {
            try
            {
                _ProductDBContext.Products.Add(product);
                await _ProductDBContext.SaveChangesAsync( );
                await _publishEndpoint.Publish(product);
                return new ServiceResponse
                {
                    flag = true,
                    Message = "Product added successfully."
                };
            }
            catch ( Exception ex )
            {
                return new ServiceResponse
                {

                    flag = false,
                    Message = $"An error occurred while adding the product: {ex.Message}"
                };
            }
        }

        public async Task<List<Product>> GetAllProductAsync () => await _ProductDBContext.Products.ToListAsync( );
        
    }
}
