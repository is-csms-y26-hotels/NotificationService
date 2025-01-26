namespace NotificationService.Application.Models;

public record Sender(string Email, string SmtpServer, int SmtpPort, string Password);