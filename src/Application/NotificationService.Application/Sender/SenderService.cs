using Microsoft.Extensions.Options;
using NotificationService.Application.Contracts.Senders;
using NotificationService.Application.Models.Sender;
using System.Net;
using System.Net.Mail;

namespace NotificationService.Application.Sender;

public class SenderService : ISenderService, IDisposable
{
    private readonly SmtpClient _smtpClient;
    private readonly MailMessage _mailMessage;

    public SenderService(IOptions<SenderMail> senderOptions)
    {
        SenderMail sender = senderOptions.Value;

        if (sender.Email is null)
            throw new NullReferenceException("Sender email address is null");

        _smtpClient = new SmtpClient(sender.SmtpServer, sender.SmtpPort)
        {
            Credentials = new NetworkCredential(sender.Email, sender.Password),
            EnableSsl = true,
        };

        _mailMessage = new MailMessage
        {
            From = new MailAddress(sender.Email),
        };
    }

    public async Task SendMessageAsync(EmailMessage message)
    {
        _mailMessage.To.Add(message.Email);
        _mailMessage.Subject = message.Subject;
        _mailMessage.Body = message.Body;

        try
        {
            await _smtpClient.SendMailAsync(_mailMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            _mailMessage.To.Clear();
        }
    }

    public void Dispose()
    {
        _smtpClient.Dispose();
        _mailMessage.Dispose();
    }
}