using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Repository;
using Shared.DTOs;
using Shared.Models;
using System.Globalization;
using System.Text;

namespace OrderAPI.Service
{
    public class OrderService : IOrder
    {
        private readonly OrderDBContext _orderDBContext;
        private readonly IPublishEndpoint _PublishEndpoint;

        public OrderService (OrderDBContext orderDBContext, IPublishEndpoint publishEndpoint)
        {
            _orderDBContext = orderDBContext;
            _PublishEndpoint = publishEndpoint;
        }

        public async Task<ServiceResponse> AddOrderAsync (Order order)
        {
            _orderDBContext.Orders.Add(order);
            await _orderDBContext.SaveChangesAsync( );
            var orderSummary = await GetOrderSummaryAsync( );
            string content = BuildOrderEmailBody(orderSummary.ID, orderSummary.ProductName, orderSummary.ProductPrice, orderSummary.Quantity, orderSummary.TotalAmount,orderSummary.Date);
            await _PublishEndpoint.Publish(new EmailDTOs("Order Infomation", content));
            await ClearOrderTable( );
            return new ServiceResponse(true,"Order placed successfully.");

        }

        public async Task<List<Order>> GetAllOrderAsync () => await _orderDBContext.Orders.ToListAsync( );


        public async Task<OrderSummary> GetOrderSummaryAsync ()
        {
            var Order = await _orderDBContext.Orders.FirstOrDefaultAsync( );
            var product = await _orderDBContext.Products.ToListAsync( );
            var ProductInfo = product.Find(x => x.ID == Order!.ProductID);
            return new OrderSummary(
                Order.ID,
                Order.ProductID,
                ProductInfo.Name,
                ProductInfo.Price,
                Order.Quantity,
                ProductInfo.Price * Order.Quantity,
                Order.Date
                );

        }

        private string BuildOrderEmailBody (int orderId, string productName, decimal price, int orderQuantity, decimal totalAmount,DateTime date)
        {
            var sb = new StringBuilder( );

            sb.AppendLine("<h1><strong>Order Information</strong></h1>");
            sb.AppendLine($"<p><strong>Order ID:</strong> {orderId}</p>");

            sb.AppendLine("<h2>Order Item:</h2>");
            sb.AppendLine("<ul>");

            sb.AppendLine($"<li>Name: {productName}</li>");
            sb.AppendLine($"<li>Price: {price}</li>");
            sb.AppendLine($"<li>Quantity: {orderQuantity}</li>");
            sb.AppendLine($"<li>Total Amount: {totalAmount}</li>");
            sb.AppendLine($"<li>Order Date: {date}</li>");

            sb.AppendLine("</ul>");

            sb.AppendLine("<p>Thank you for your order!</p>");

            return sb.ToString( );
        }

        private async Task ClearOrderTable ( )
        {
            _orderDBContext.Orders.Remove(await _orderDBContext.Orders.FirstOrDefaultAsync());
            await _orderDBContext.SaveChangesAsync( );
        }
    }
}
