namespace GoalSystems.WebApi.Infrastructure.Configuration
{
    /// <summary>
    /// App settings data.
    /// </summary>
    public class ConfigurationApp
    {
        public const string Position = "Configuration";

        /// <summary>
        /// Information regarding Security
        /// </summary>
        public SecurityConfiguration Security { get; set; }

        /// <summary>
        /// Information regarding Security.
        /// </summary>
        public class SecurityConfiguration
        {
            /// <summary>
            /// Authority Uri
            /// </summary>
            public string Authority { get; set; }

            /// <summary>
            /// Api Scope
            /// </summary>
            public string ApiScope { get; set; }

            /// <summary>
            /// Name of the API scope.
            /// </summary>
            public string ApiScopeName { get; set; }
        }
    }
}
