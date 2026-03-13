using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Evently.Modules.Events.Presentation.Categories;
using Evently.Modules.Events.Presentation.Events;
using Evently.Modules.Events.Presentation.TicketTypes;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        EventEndpoints.MapEndpoints(app);
        TicketTypeEndpoints.MapEndpoints(app);
        CategoryEndpoints.MapEndpoints(app);
    }

    extension(IServiceCollection services)
    {
        public void AddEventsModule(IConfiguration configuration)
        {
            services.AddInfrastructure(configuration);
        }

        private void AddInfrastructure(IConfiguration configuration)
        {
            string databaseConnectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<EventsDbContext>(options =>
                options.UseNpgsql(databaseConnectionString,
                        npgsqlOptionsAction =>
                            npgsqlOptionsAction.MigrationsHistoryTable(HistoryRepository.DefaultTableName,
                                Schemas.Events))
                    .UseSnakeCaseNamingConvention()
                    .AddInterceptors());

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }
    }
}
