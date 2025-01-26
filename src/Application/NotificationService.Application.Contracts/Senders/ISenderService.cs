namespace NotificationService.Application.Contracts.Senders;

public interface ISenderService
{
    Task Send(string recipient, string subject, string body);
}