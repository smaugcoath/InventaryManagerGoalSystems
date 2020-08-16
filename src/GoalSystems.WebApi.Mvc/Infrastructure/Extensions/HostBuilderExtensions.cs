namespace GoalSystems.WebApi.Mvc.Infrastructure.Extensions
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    /// <summary>
    /// Helper methods for <see cref="Host"/> and <see cref="WebHost"/>.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds custom Serilog configuration.
        /// </summary>
        /// <param name="webHostBuilder"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseCustomSerilog(this IWebHostBuilder webHostBuilder) =>
            webHostBuilder.UseSerilog((context, configuration) =>
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext());
    }
}
