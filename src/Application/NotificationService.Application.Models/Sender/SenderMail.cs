namespace NotificationService.Application.Models.Sender;

public class SenderMail
{
    public string? Email { get; init; }

    public string? SmtpServer { get; init; }

    public int SmtpPort { get; init; }

    public string? Password { get; init; }

    // Основной конструктор для удобного создания
    public SenderMail(string email, string smtpServer, int smtpPort, string password)
    {
        Email = email;
        SmtpServer = smtpServer;
        SmtpPort = smtpPort;
        Password = password;
    }

    public SenderMail() { }
}
