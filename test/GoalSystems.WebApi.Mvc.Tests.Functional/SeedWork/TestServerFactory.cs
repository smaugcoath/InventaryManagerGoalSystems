namespace GoalSystems.WebApi.Mvc.Tests.Functional.SeedWork
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.IO;
    using System.Reflection;

    internal static class TestServerFactory
    {
        private const string EnvironmentKey = "ASPNETCORE_ENVIRONMENT";
        private const string EnvironmentValue = "Development";

        internal static TestServer Create<TStartup>(Action<IServiceCollection> servicesConfiguration, IConfiguration configuration)
        where TStartup : class
        {
            servicesConfiguration ??= ((IServiceCollection serviceCollection) => { });
            Environment.SetEnvironmentVariable(EnvironmentKey, EnvironmentValue);

            var contentRoot = Assembly
               .GetAssembly(typeof(ServerFixture))
               .Location;

            var webHostBuilder = WebHost.CreateDefaultBuilder<TStartup>(null)
                .UseContentRoot(Path.GetDirectoryName(contentRoot))
                .ConfigureAppConfiguration((context, builder) =>
                    builder.AddConfiguration(configuration)
                )
                .UseTestServer()
                .ConfigureTestContainer(servicesConfiguration)
                .UseConfiguration(configuration);


            var server = new TestServer(webHostBuilder);

            return server;
        }
    }
}
