namespace GoalSystems.WebApi.Mvc.Infrastructure.OpenApi
{
    using GoalSystems.WebApi.Infrastructure.Configuration;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private const string OPEN_API_FILENAME = "OpenApiDocumentation.xml";
        private readonly ConfigurationApp _configuration;
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="apiVersionDescription">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IOptions<ConfigurationApp> configuration, IApiVersionDescriptionProvider apiVersionDescription)
        {
            _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
            _apiVersionDescriptionProvider = apiVersionDescription ?? throw new ArgumentNullException(nameof(apiVersionDescription));
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(type => type.FullName);
            options.ResolveConflictingActions(x => x.First());

            options.OperationFilter<AuthorizeCheckOperationFilter>();
            options.OperationFilter<ApiVersionOperationFilter>();

            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfoForApiVersion(description));
            }


            //TODO: Example of how to configure Swagger to use OAuth2 with an implicit flow. Authorization data are setup assuming that the security is going to be handled by Identity Server 4.

            //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //{
            //    Type = SecuritySchemeType.OAuth2,
            //    Flows = new OpenApiOAuthFlows
            //    {
            //        Implicit = new OpenApiOAuthFlow
            //        {
            //            AuthorizationUrl = new Uri($"{_configuration.Security.Authority}/connect/authorize", UriKind.Absolute),
            //            Scopes = new Dictionary<string, string>
            //            {
            //                { _configuration.Security.ApiScope, _configuration.Security.ApiScopeName },
            //            }
            //        }
            //    }
            //});

            var xmlPath = Path.Combine(AppContext.BaseDirectory, OPEN_API_FILENAME);
            options.IncludeXmlComments(xmlPath);
        }

        private static OpenApiInfo CreateOpenApiInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Example Rest API",
                Version = description.ApiVersion.ToString(),
                Description = "Example of a Rest Api",
                Contact = new OpenApiContact() { Name = "Enrique Carrasco Contreras", Email = "kike@beyoursoft.com" },
                License = new OpenApiLicense() { Name = "", Url = new Uri("https://opensource.org/licenses/MIT") }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}