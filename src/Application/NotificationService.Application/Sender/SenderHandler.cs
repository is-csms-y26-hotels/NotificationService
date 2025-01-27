using NotificationService.Application.Contracts.Senders;
using NotificationService.Application.Models.Account;
using NotificationService.Application.Models.Booking;
using NotificationService.Application.Models.Sender;

namespace NotificationService.Application.Sender;

public class SenderHandler : ISenderHandler
{
    public Task<EmailMessage>? GetMessageForBooking(BookingMessage bookingMessage)
    {
        string body, subject;

        switch (bookingMessage.State)
        {
            case BookingState.Created:
                subject = "Создание брони";
                body = $"Вы создали бронь в отеле {bookingMessage.HotelName}. " +
                          $"Дата заезда: {bookingMessage.CheckInDate}, дата выезда: {bookingMessage.CheckOutDate}";
                break;

            case BookingState.Cancelled:
                subject = "Отмена брони";
                body = $"Вы отменили бронь в отеле {bookingMessage.HotelName}. " +
                          $"В котором была дата заезда: {bookingMessage.CheckInDate} и была дата выезда: {bookingMessage.CheckOutDate}";
                break;

            case BookingState.Submitted:
                subject = "Подтверждение брони";
                body = $"Ваша бронь в отеле {bookingMessage.HotelName} подтверждена. " +
                          $"Дата заезда: {bookingMessage.CheckInDate}, дата выезда: {bookingMessage.CheckOutDate}";
                break;

            case BookingState.Completed:
                subject = "Спасибо за посещение";
                body = $"Спасибо что посетили отель {bookingMessage.HotelName}. " +
                          $"С {bookingMessage.CheckInDate} по {bookingMessage.CheckOutDate}";
                break;

            default:
                return null;
        }

        return Task.FromResult(new EmailMessage(bookingMessage.Email, subject, body));
    }

    public Task<EmailMessage> GetMessageForAccount(AccountMessage accountMessage)
    {
        throw new NotImplementedException();
    }
}