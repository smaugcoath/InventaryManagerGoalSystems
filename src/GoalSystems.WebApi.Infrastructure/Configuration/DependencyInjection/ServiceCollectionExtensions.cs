namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.Extensions.Configuration;
    using System;

    /// <summary>
    /// Extensions method for <see cref="IConfiguration"/>
    /// </summary>
    public static class ConfigurationExtensions
    {

        /// <summary>
        /// Binds a section from the App Settings into a typed object.
        /// </summary>
        /// <typeparam name="TConfiguration">Typed object. The property names must match with the Settings names.</typeparam>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="key">If no key is specified, the name of the class used as generic TConfiguration will be used as key of the main node in App Settings to be deserialized.</param>
        /// <returns>Returns the <see cref="IServiceCollection"/></returns>
        public static IServiceCollection BindConfiguration<TConfiguration>(this IServiceCollection serviceCollection, IConfiguration configuration, string key = null)
            where TConfiguration : new()
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
            key = key ?? typeof(TConfiguration).Name;

            //serviceCollection

            var section = new TConfiguration();
            configuration.GetSection(key).Bind(section);

            return serviceCollection;
        }
    }
}
