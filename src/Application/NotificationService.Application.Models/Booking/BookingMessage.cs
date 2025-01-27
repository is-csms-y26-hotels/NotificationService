namespace NotificationService.Application.Models.Booking;

public record BookingMessage(
    string Email,
    string HotelName,
    DateTimeOffset CheckInDate,
    DateTimeOffset CheckOutDate,
    BookingState State);