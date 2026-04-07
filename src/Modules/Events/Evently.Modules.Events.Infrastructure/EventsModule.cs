using Evently.Common.Application.Messaging;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Infrastructure.Outbox;
using Evently.Modules.Events.Infrastructure.PublicApi;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Evently.Modules.Events.Presentation;
using Evently.Modules.Events.PublicApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    extension(IServiceCollection services)
    {
        public void AddEventsModule(IConfiguration configuration)
        {
            services.AddDomainEventHandlers();

            services.AddEndpoints([AssemblyReference.Assembly]);

            services.AddInfrastructure(configuration);
        }

        private void AddInfrastructure(IConfiguration configuration)
        {
            string databaseConnectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<EventsDbContext>((sp, options) =>
                options.UseNpgsql(databaseConnectionString,
                        npgsqlOptionsAction =>
                            npgsqlOptionsAction.MigrationsHistoryTable(HistoryRepository.DefaultTableName,
                                Schemas.Events))
                    .UseSnakeCaseNamingConvention()
                    .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IEventsApi, EventsApi>();

            services.Configure<OutboxOptions>(configuration.GetSection("Events:Outbox"));

            services.ConfigureOptions<ConfigureProcessOutboxJob>();
        }


        private void AddDomainEventHandlers()
        {
            Type[] domainEventHandlers = Application.AssemblyReference.Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
                .ToArray();

            foreach (Type domainEventHandler in domainEventHandlers)
            {
                services.TryAddScoped(domainEventHandler);

                Type domainEvent = domainEventHandler
                    .GetInterfaces()
                    .Single(i => i.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

                services.Decorate(domainEventHandler, closedIdempotentHandler);
            }
        }
    }
}
