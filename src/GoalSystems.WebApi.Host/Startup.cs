namespace GoalSystems.WebApi.Host
{
    using GoalSystems.WebApi.Mvc.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
            : base(configuration, hostingEnvironment)
        {
        }

    }
}
