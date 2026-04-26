using EmailNotificationWebhoob.Consumer;
using EmailNotificationWebhoob.Repository;
using EmailNotificationWebhoob.Service;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace EmailNotificationWebhoob
{
    public class Program
    {
        public static void Main (string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient<Repository.IEmail, EmailService>( );
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<WebhookConsumer>( );
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host("rabbitmq://localhost", c =>
                    {
                        c.Username("guest");
                        c.Password("guest");
                    });
                    config.ReceiveEndpoint("email-webhook-queue", e =>
                    {
                        e.ConfigureConsumer<WebhookConsumer>(context);
                    });
                });
            });
            var app = builder.Build( );

            app.MapPost("/email-webhook", ( [FromBody] EmailDTOs email,IEmail EmailRepo) =>
            {
                string rs = EmailRepo.SendEmail(email);
                return Task.FromResult(rs);
            });

            app.Run( );
        }
    }
}
