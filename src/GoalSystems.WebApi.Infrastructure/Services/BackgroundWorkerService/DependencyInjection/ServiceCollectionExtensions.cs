namespace Microsoft.Extensions.DependencyInjection
{
    using GoalSystems.WebApi.Infrastructure.Services.BackgroundWorkerService.Abstractions;
    using GoalSystems.WebApi.Infrastructure.Services.BackgroundWorkerService.Hangfire;
    using Hangfire;
    using Hangfire.MemoryStorage;

    /// <summary>
    /// Extensions methods for <see cref="IServiceCollection"/> regarding different background workers providers.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Injects <see cref="IBackgroundWorkerService"/> to enqeue and schedule background works. The internal implementations is based on Hangfire In Memory, so you can provide the Dashboard adding the corresponding Handfire Dasboard to the Mvc endpoints extension methods.
        /// </summary>
        public static IServiceCollection AddHangFireInMemoryBackgroundWorker(this IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage());

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddSingleton<IBackgroundWorkerService, HangfireBackgroundWorkerService>();

            return services;
        }
    }
}
