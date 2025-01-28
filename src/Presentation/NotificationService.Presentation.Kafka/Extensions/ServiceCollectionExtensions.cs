using Itmo.Dev.Platform.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Kafka.Contracts;
using NotificationService.Presentation.Kafka.ConsumerHandlers;

namespace NotificationService.Presentation.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationKafka(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        const string consumerKey = "Presentation:Kafka:Consumers";

        collection.AddPlatformKafka(builder => builder
            .ConfigureOptions(configuration.GetSection("Presentation:Kafka"))
            .AddConsumer(b => b
            .WithKey<BookingNotificationKey>()
             .WithValue<BookingNotificationValue>()
            .WithConfiguration(configuration.GetSection($"{consumerKey}:BookingNotifications"))
             .DeserializeKeyWithProto()
             .DeserializeValueWithProto()
             .HandleInboxWith<BookingNotificationHandler>()));

        return collection;
    }
}