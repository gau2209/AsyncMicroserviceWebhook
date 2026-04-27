using MassTransit;
using OrderAPI.Data;
using Shared.Models;

namespace OrderAPI.Consumer
{
    public class ProductConsumer : IConsumer<Product>
    {
        private readonly OrderDBContext _orderDBContext;

        public ProductConsumer (OrderDBContext orderDBContext)
        {
            _orderDBContext = orderDBContext;
        }

        public async Task Consume (ConsumeContext<Product> context)
        {
            var Product = new Product
            {
                ID = 0,
                Name = context.Message.Name,
                Price = context.Message.Price
            };
            _orderDBContext.Products.Add(Product);
            await _orderDBContext.SaveChangesAsync( );
        }
    }
}
