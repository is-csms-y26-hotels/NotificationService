// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable CA1506

using Itmo.Dev.Platform.Common.Extensions;
using Itmo.Dev.Platform.Events;
using Itmo.Dev.Platform.MessagePersistence.Extensions;
using Itmo.Dev.Platform.MessagePersistence.Postgres.Extensions;
using Itmo.Dev.Platform.Observability;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationService.Application.Extensions;
using NotificationService.Application.Models.Sender;
using NotificationService.Infrastructure.Persistence.Extensions;
using NotificationService.Presentation.Grpc.Extensions;
using NotificationService.Presentation.Kafka.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddOptions<JsonSerializerSettings>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JsonSerializerSettings>>().Value);

builder.Services.AddPlatform();
builder.AddPlatformObservability();
builder.Services.AddUtcDateTimeProvider();

// Null value ignore is needed to correctly deserialize oneof messages in inbox/outbox
builder.Services.AddOptions<JsonSerializerSettings>()
    .Configure(options => options.NullValueHandling = NullValueHandling.Ignore);

// Used as inbox and outbox infrastructure
builder.Services.AddPlatformMessagePersistence(selector => selector
    .UsePostgresPersistence(postgres => postgres
        .ConfigureOptions(optionsBuilder => optionsBuilder
            .BindConfiguration("Infrastructure:MessagePersistence:Persistence"))));

builder.Services.Configure<SenderMail>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddApplication();

builder.Services.AddPresentationGrpc();
builder.Services.AddInfrastructurePersistence();
builder.Services.AddPresentationKafka(builder.Configuration);

builder.Services.AddPlatformEvents(b => b.AddPresentationKafkaHandlers());

WebApplication app = builder.Build();

app.UseRouting();

app.UsePlatformObservability();

app.UsePresentationGrpc();

await app.RunAsync();