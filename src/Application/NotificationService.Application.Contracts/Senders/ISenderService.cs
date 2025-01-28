using NotificationService.Application.Models.Sender;

namespace NotificationService.Application.Contracts.Senders;

public interface ISenderService
{
    Task SendMessageAsync(EmailMessage message);
}