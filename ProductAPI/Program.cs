
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Repository;
using ProductAPI.Service;

namespace ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host("rabbitmq://localhost", c =>
                    {
                        c.Username("guest");
                        c.Password("guest");
                    });
                  
                });
            });

            builder.Services.AddScoped<IProduct, ProductService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
