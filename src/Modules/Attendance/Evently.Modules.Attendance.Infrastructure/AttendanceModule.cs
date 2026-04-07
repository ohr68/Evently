using Evently.Common.Application.Messaging;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Attendance.Application.Abstractions.Authentication;
using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.Infrastructure.Attendees;
using Evently.Modules.Attendance.Infrastructure.Authentication;
using Evently.Modules.Attendance.Infrastructure.Database;
using Evently.Modules.Attendance.Infrastructure.Events;
using Evently.Modules.Attendance.Infrastructure.Outbox;
using Evently.Modules.Attendance.Infrastructure.Tickets;
using Evently.Modules.Attendance.Presentation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Modules.Attendance.Infrastructure;

public static class AttendanceModule
{
    extension(IServiceCollection services)
    {
        public void AddAttendanceModule(IConfiguration configuration)
        {
            services.AddDomainEventHandlers();

            services.AddInfrastructure(configuration);

            services.AddEndpoints([AssemblyReference.Assembly]);
        }

        private void AddInfrastructure(IConfiguration configuration)
        {
            services.AddDbContext<AttendanceDbContext>((sp, options) =>
                options
                    .UseNpgsql(
                        configuration.GetConnectionString("Database"),
                        npgsqlOptions => npgsqlOptions
                            .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Attendance))
                    .UseSnakeCaseNamingConvention()
                    .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AttendanceDbContext>());

            services.AddScoped<IAttendeeRepository, AttendeeRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            services.AddScoped<IAttendanceContext, AttendanceContext>();

            services.Configure<OutboxOptions>(configuration.GetSection("Attendance:Outbox"));

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
