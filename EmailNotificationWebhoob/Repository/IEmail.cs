using Shared.DTOs;

namespace EmailNotificationWebhoob.Repository
{
    public interface IEmail
    {
        string SendEmail (EmailDTOs email);
    }
}
