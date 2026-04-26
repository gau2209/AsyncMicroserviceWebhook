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
            _orderDBContext.Products.Add(context.Message);
            await _orderDBContext.SaveChangesAsync( );
        }
    }
}
