namespace Microsoft.Extensions.DependencyInjection
{
    using GoalSystems.WebApi.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Data;

    /// <summary>
    /// Extensions methods to configure the needed service of Master Data Management.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure DI for WebApi DbContexts to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="configuration">The <see cref="IConfiguration"/></param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddDataServices(this IServiceCollection services, IDbConnection connection)
            => services
            .AddDatabaseContext(connection);

        /// <summary>
        /// Add Automapper.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="assemblies">Assemblies to look for Profiles</param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IDbConnection connection)
        {
            // Injects DbContext
            services
                  .AddDbContext<DatabaseContext>(options =>
                   options.UseSqlServer(
                       connection.ConnectionString,
                       sqlOptions => sqlOptions
                          .MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)
                          .EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null)
                      ));

            // Create the database or apply migrations.
            var context = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return services;
        }
    }
}
