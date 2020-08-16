namespace GoalSystems.WebApi.Mvc.Infrastructure.Extensions
{
    using Hangfire;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Extensions methods to configure Master Data Management.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure. This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
        /// <param name="environment"><see cref="IHostEnvironment"/></param>
        /// <param name="apiVersionDescriptionProvider"><see cref="IApiVersionDescriptionProvider"/></param>
        public static void UseDefault(this IApplicationBuilder applicationBuilder, IHostEnvironment environment, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (environment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                applicationBuilder.UseHsts();
            }

            applicationBuilder
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHangfireDashboard();

                })
                .UseOpenApi(apiVersionDescriptionProvider);
        }

        /// <summary>
        /// Use Open Api provder
        /// </summary>
        /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
        /// <param name="provider"><see cref="IApiVersionDescriptionProvider"/></param>
        /// <returns>The <see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder applicationBuilder, IApiVersionDescriptionProvider provider) =>
            applicationBuilder
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        options.RoutePrefix = string.Empty;
                    }
                });


    }
}
