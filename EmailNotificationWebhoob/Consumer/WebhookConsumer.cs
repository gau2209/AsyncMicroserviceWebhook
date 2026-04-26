using MassTransit;
using Shared.DTOs;

namespace EmailNotificationWebhoob.Consumer
{
    public class WebhookConsumer : IConsumer<EmailDTOs>
    {
        private readonly HttpClient httpClient;

        public WebhookConsumer (HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task Consume (ConsumeContext<EmailDTOs> context)
        {
            var result = await httpClient.PostAsJsonAsync("https://localhost:7083/email-webhook", new EmailDTOs(context.Message.Title,context.Message.Content));
            result.EnsureSuccessStatusCode();
        }
    }
}
