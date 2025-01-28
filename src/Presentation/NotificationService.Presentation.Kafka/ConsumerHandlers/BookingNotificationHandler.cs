using Itmo.Dev.Platform.Kafka.Consumer;
using Notifications.Kafka.Contracts;
using NotificationService.Application.Contracts.Senders;
using NotificationService.Application.Models.Booking;
using NotificationService.Application.Models.Sender;

namespace NotificationService.Presentation.Kafka.ConsumerHandlers;

internal class BookingNotificationHandler : IKafkaInboxHandler<BookingNotificationKey, BookingNotificationValue>
{
    private readonly ISenderService _senderService;
    private readonly ISenderHandler _senderHandler;

    public BookingNotificationHandler(ISenderHandler senderHandler, ISenderService senderService)
    {
        _senderService = senderService;
        _senderHandler = senderHandler;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<BookingNotificationKey, BookingNotificationValue>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<BookingNotificationKey, BookingNotificationValue> message in messages)
        {
             await ProcessMessageAsync(message.Value, cancellationToken);
        }
    }

    private static BookingState MapBookingState(BookingNotificationValue.Types.BookingState bookingState)
    {
        return bookingState switch
        {
            BookingNotificationValue.Types.BookingState.Created => BookingState.Created,
            BookingNotificationValue.Types.BookingState.Submitted => BookingState.Submitted,
            BookingNotificationValue.Types.BookingState.Cancelled => BookingState.Cancelled,
            BookingNotificationValue.Types.BookingState.Completed => BookingState.Completed,
            BookingNotificationValue.Types.BookingState.Unspecified or _ =>
                throw new ArgumentOutOfRangeException(nameof(bookingState), bookingState, null),
        };
    }

    private async Task ProcessMessageAsync(BookingNotificationValue bookingNotification, CancellationToken cancellationToken)
    {
        BookingMessage? bookingMessage = null;
        switch (bookingNotification.EventCase)
        {
            case BookingNotificationValue.EventOneofCase.BookingCreatedNotification:
                bookingMessage = new(
                    bookingNotification.BookingCreatedNotification.UserEmail,
                    bookingNotification.BookingCreatedNotification.HotelName,
                    bookingNotification.BookingCreatedNotification.CheckInDate.ToDateTimeOffset(),
                    bookingNotification.BookingCreatedNotification.CheckOutDate.ToDateTimeOffset(),
                    BookingState.Created);
                break;

            case BookingNotificationValue.EventOneofCase.BookingUpdatedNotification:
                bookingMessage = new(
                    bookingNotification.BookingUpdatedNotification.UserEmail,
                    bookingNotification.BookingUpdatedNotification.HotelName,
                    bookingNotification.BookingUpdatedNotification.CheckInDate.ToDateTimeOffset(),
                    bookingNotification.BookingUpdatedNotification.CheckOutDate.ToDateTimeOffset(),
                    MapBookingState(bookingNotification.BookingUpdatedNotification.BookingState));
                break;
        }

        if (bookingMessage is null)
            return;

        EmailMessage? emailMessage = _senderHandler.GetMessageForBooking(bookingMessage);

        if (emailMessage is null)
            return;

        await _senderService.SendMessageAsync(emailMessage);
    }
}