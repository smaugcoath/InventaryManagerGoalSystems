namespace Microsoft.Extensions.DependencyInjection
{
    using AutoMapper;
    using GoalSystems.WebApi.Infrastructure.Configuration;
    using GoalSystems.WebApi.Mvc.Infrastructure;
    using GoalSystems.WebApi.Mvc.Infrastructure.Filters;
    using GoalSystems.WebApi.Mvc.Infrastructure.OpenApi;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Extensions of IServiceCollection
    /// </summary>
    public static class GoalSystemsMvcServiceCollectionExtensions
    {
        private const string ConnectionStringName = "Default";

        /// <summary>
        /// Configure all the services. This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns>A service collection</returns>
        public static IServiceCollection AddDefault(this IServiceCollection services, IConfiguration configuration)
            => services
            //.BindConfiguration<Configuration>(configuration)
            .Configure<ConfigurationApp>(configuration.GetSection(ConfigurationApp.Position))
            .AddHttpContextAccessor()
            .AddCustomApiVersioning()
            .AddCustomAuthentication()
            .AddCustomAuthorization()
            .AddMediatRMediatorService()
            .AddHangFireInMemoryBackgroundWorker()
            .AddTransient<IDbConnection>(x => new SqlConnection(configuration.GetConnectionString(ConnectionStringName)))
            .AddBusinesServices(services.BuildServiceProvider().GetRequiredService<IDbConnection>())
            .AddMvcServices()
            .AddOpenApi()
            .AddCors()
            //.AddApplicationInsightsTelemetry(configuration)
            .AddCustomMvc();

        /// <summary>
        /// Configure all the Web Api services needed for this project to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddMvcServices(this IServiceCollection services)
            => services.AddMappers();

        /// <summary>
        /// Add mappers.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="assemblies">Assemblies to look for Profiles</param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        private static IServiceCollection AddMappers(this IServiceCollection services)
            => services.AddAutoMapper(new[] { typeof(GoalSystemsMvcServiceCollectionExtensions) });

        /// <summary>
        /// Add services to customize the API versioning to the service collection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns>A service collection</returns>
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
            => services
            .AddVersionedApiExplorer()
            .AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader(),
                    new HeaderApiVersionReader()
                    {
                        HeaderNames = { "api-version" }
                    });

                setup.AssumeDefaultVersionWhenUnspecified = true;
            });

        /// <summary>
        /// Configure services to use Open Api provider
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns>A service collection</returns>
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
            => services
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
            .AddSwaggerGen();

        /// <summary>
        /// Add authentication services to the service collection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="AuthenticationBuilder"/></returns>
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
            => services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var configuration = services.BuildServiceProvider().GetRequiredService<IOptions<ConfigurationApp>>().Value;
                options.Authority = configuration.Security.Authority;
                options.RequireHttpsMetadata = false;
                options.Audience = configuration.Security.ApiScope;
            })
            .Services;


        /// <summary>
        /// Configures the Web Api custom authorization
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
            => services
            .AddAuthorization(setup => { })
            .AddPolicyHandlers();

        /// <summary>
        /// Adds custom policy handlers
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPolicyHandlers(this IServiceCollection services)
            => services;

        /// <summary>
        /// Adds customized Mvc services.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
            => services
            .AddControllers(options =>
            {
                options.Filters.Add<NotFoundExceptionFilter>();
                options.Filters.Add<ItemDuplicatedExceptionFilter>();
                options.SuppressAsyncSuffixInActionNames = false;
            })
            //.AddNewtonsoftJson()
            .AddApplicationPart(typeof(BaseStartup).Assembly)
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .Services;
    }
}
