using NotificationService.Application.Contracts.Senders;
using NotificationService.Application.Models;
using System.Net;
using System.Net.Mail;

namespace NotificationService.Application.Senders;

public class SenderService : ISenderService
{
    private readonly Sender _sender;

    public SenderService(Sender sender)
    {
        _sender = sender;
    }

    public async Task Send(string recipient, string subject, string body)
    {
        using var smtpClient = new SmtpClient(_sender.SmtpServer, _sender.SmtpPort);
        smtpClient.Credentials = new NetworkCredential(_sender.Email, _sender.Password);
        smtpClient.EnableSsl = true;

        using var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_sender.Email);
        mailMessage.To.Add(recipient);
        mailMessage.Subject = subject;
        mailMessage.Body = body;

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}