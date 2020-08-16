namespace GoalSystems.WebApi.Mvc.Infrastructure
{
    using GoalSystems.WebApi.Mvc.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Custom Startup class for WebApi project.
    /// </summary>
    public abstract class BaseStartup
    {
        /// <summary>
        /// <see cref="IConfiguration"/>.
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// <see cref="IHostEnvironment"/>.
        /// </summary>
        protected IHostEnvironment Environment { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="environment"><see cref="IHostEnvironment"/></param>
        protected BaseStartup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        /// <summary>
        /// This method gets called by the runtime.Use this method to add services to the container.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public virtual void ConfigureServices(IServiceCollection services)
            => services.AddDefault(Configuration);


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
        /// <param name="apiVersionProvider"><see cref="IApiVersionDescriptionProvider"/></param>
        public virtual void Configure(IApplicationBuilder applicationBuilder, IApiVersionDescriptionProvider apiVersionProvider)
            => applicationBuilder.UseDefault(Environment, apiVersionProvider);

    }
}
