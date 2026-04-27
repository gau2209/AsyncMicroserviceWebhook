
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Consumer;
using OrderAPI.Data;
using OrderAPI.Repository;
using OrderAPI.Service;

namespace OrderAPI
{
    public class Program
    {
        public static void Main (string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers( );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer( );
            builder.Services.AddSwaggerGen( );
            builder.Services.AddDbContext<OrderDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductConsumer>( );
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host("rabbitmq://localhost", c =>
                    {
                        c.Username("guest");
                        c.Password("guest");
                    });
                    config.ReceiveEndpoint("product-queue", e =>
                    {
                        e.ConfigureConsumer<ProductConsumer>(context);
                    });
                });
            });

            builder.Services.AddScoped<IOrder, OrderService>( );

            var app = builder.Build( );

            // Configure the HTTP request pipeline.
            if ( app.Environment.IsDevelopment( ) )
            {
                app.UseSwagger( );
                app.UseSwaggerUI( );
            }

            app.UseHttpsRedirection( );

            app.UseAuthorization( );


            app.MapControllers( );

            app.Run( );
        }
    }
}
