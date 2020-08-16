namespace GoalSystems.WebApi.Mvc.Tests.Functional.SeedWork
{
    using Acheve.TestHost;
    using GoalSystems.WebApi.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <inheritdoc/>
    internal sealed class TestStartup : BaseStartup
    {
        /// <inheritdoc/>
        public TestStartup(IConfiguration configuration, IHostEnvironment environment) : base(configuration, environment) { }

        /// <inheritdoc/>
        public override void ConfigureServices(IServiceCollection services)
            => services
            .AddDefault(Configuration)
            .PostConfigure<AuthenticationOptions>(TestServerDefaults.AuthenticationScheme, options =>
            {
                options.DefaultAuthenticateScheme = TestServerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = TestServerDefaults.AuthenticationScheme;
            });
    }
}
