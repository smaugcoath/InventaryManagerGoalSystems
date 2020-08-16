namespace Microsoft.Extensions.DependencyInjection
{
    using GoalSystems.WebApi.Infrastructure.Services.Mediator.Abstractions;
    using GoalSystems.WebApi.Infrastructure.Services.Mediator.MediatR;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extensions methods for <see cref="IServiceCollection"/> regarding different mediator providers.
    /// </summary>
    public static class MediatorServiceCollectionExtensions
    {
        /// <summary>
        /// Injects <see cref="IMediatorService"/> using MediatR as mediator provider.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="assemblies">An <see cref="Assembly"/> collection where the handlers, notifications and messages are going to be searched and registered.</param>
        /// <returns></returns>
        public static IServiceCollection AddMediatRMediatorService(this IServiceCollection services, IEnumerable<Assembly> assemblies = null)
        {
            assemblies = assemblies ?? AppDomain.CurrentDomain.GetAssemblies().AsEnumerable();

            services.AddMediatR(assemblies.ToArray());

            services.AddSingleton<IMediatorService, MediatRMediatorService>();

            return services;
        }
    }
}
