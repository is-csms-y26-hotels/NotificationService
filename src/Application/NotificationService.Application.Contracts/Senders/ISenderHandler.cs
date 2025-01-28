using NotificationService.Application.Models.Booking;
using NotificationService.Application.Models.Sender;

namespace NotificationService.Application.Contracts.Senders;

public interface ISenderHandler
{
    EmailMessage? GetMessageForBooking(BookingMessage bookingMessage);
}