namespace GoalSystems.WebApi.Mvc.Tests.Functional.SeedWork
{
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Respawn;

    internal class ServerFixture
    {
        private static readonly Checkpoint _checkpoint = new Checkpoint
        {
            TablesToIgnore = new string[] { "__EFMigrationsHistory" },
            WithReseed = true
        };

        private const string AppSettingsFileName = "appsettings.json";
        private const string ConnectionStringName = "Default";

        private static IConfiguration _configuration;
        internal TestServer Server { get; private set; }

        public Given Given { get; }

        public ServerFixture()
        {
            _configuration = BuildConfiguration();
            RegisterServices();
            Given = new Given(this);
            Server = TestServerFactory.Create<TestStartup>(null, _configuration);
        }

        internal static void ResetDatabase()
        {
            var configuration = BuildConfiguration();

            _checkpoint
                .Reset(configuration.GetConnectionString(ConnectionStringName))
                .GetAwaiter()
                .GetResult();
        }

        internal TService GetService<TService>()
           => Server.Services.CreateScope().ServiceProvider.GetService<TService>();



        private static IConfiguration BuildConfiguration()
            => new ConfigurationBuilder()
            .AddJsonFile(AppSettingsFileName)
            .AddEnvironmentVariables()
            // No support for User Secret for now.
            //.AddUserSecrets<ServerFixture>()
            .Build();

        private void RegisterServices()
        {
            //var serviceCollection = new ServiceCollection();

            //serviceCollection
            ////    .AddDbContext<DatabaseContext>(options =>
            ////{
            ////    var connectionString = _configuration.GetConnectionString(ConnectionStringName);
            ////    options.UseSqlServer(connectionString, setup =>
            ////    {
            ////        setup.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName);
            ////    });
            ////})
            //.AddSingleton<ILoggerFactory, LoggerFactory>();


            //ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

          //  EnsureDatabase<DatabaseContext>(serviceProvider);
        }

        //private void EnsureDatabase<TContext>(ServiceProvider serviceProvider) where TContext : DbContext
        //{
        //    var context = serviceProvider.GetService<TContext>();

        //    try
        //    {
        //        context.Database.EnsureDeleted();
        //        context.Database.Migrate();
        //    }
        //    catch (Exception ex)
        //    {
        //        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        //        var logger = loggerFactory.CreateLogger<TContext>();
        //        logger.LogError(ex, $"An error occurred while migrating the database for context {nameof(TContext)}.");

        //        throw;
        //    }
        //}
    }


}
