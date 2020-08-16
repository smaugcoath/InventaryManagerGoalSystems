namespace Microsoft.Extensions.DependencyInjection
{
    using AutoMapper;
    using GoalSystems.WebApi.Business.Services.ItemService;
    using System.Data;

    /// <summary>
    /// Extensions methods to configure the needed service of Master Data Management.
    /// </summary>
    public static class BusinessServiceCollectionExtensions
    {
        /// <summary>
        /// Configure all the services needed in WebApi to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddBusinesServices(this IServiceCollection services, IDbConnection dbConnection)
            => services
            .AddDataServices(dbConnection)
            .AddMappers()
            .AddServices();

        /// <summary>
        /// Add Automapper.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="assemblies">Assemblies to look for Profiles</param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
            => services
            .AddScoped<IItemService, ItemService>();

        /// <summary>
        /// Add Automapper.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="assemblies">Assemblies to look for Profiles</param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        private static IServiceCollection AddMappers(this IServiceCollection services)
            => services.AddAutoMapper(new[] { typeof(BusinessServiceCollectionExtensions) });

    }
}
