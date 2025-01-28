using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts.Senders;
using NotificationService.Application.Sender;

namespace NotificationService.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        // TODO: add services
        collection.AddScoped<ISenderService, SenderService>();
        collection.AddScoped<ISenderHandler, SenderHandler>();
        return collection;
    }
}