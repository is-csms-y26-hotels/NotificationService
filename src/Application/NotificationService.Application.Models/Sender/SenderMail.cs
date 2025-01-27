namespace NotificationService.Application.Models.Sender;

public record SenderMail(string Email, string SmtpServer, int SmtpPort, string Password);