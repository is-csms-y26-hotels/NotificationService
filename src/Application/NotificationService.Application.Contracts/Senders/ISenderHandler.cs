using NotificationService.Application.Models.Account;
using NotificationService.Application.Models.Booking;
using NotificationService.Application.Models.Sender;

namespace NotificationService.Application.Contracts.Senders;

public interface ISenderHandler
{
    Task<EmailMessage>? GetMessageForBooking(BookingMessage bookingMessage);

    Task<EmailMessage> GetMessageForAccount(AccountMessage accountMessage);
}