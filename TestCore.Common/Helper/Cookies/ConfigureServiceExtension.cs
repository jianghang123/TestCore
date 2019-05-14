﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace TestCore.Common.Helper.Cookies
{
    /// <summary>
    /// Extension methods for setting up Cookie manager services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ConfigureServiceExtension
    {
        /// <summary>
        /// Adds Cookie manager services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddCookieManager(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Transient<ICookie, HttpCookie>());
            services.TryAdd(ServiceDescriptor.Transient<ICookieManager, DefaultCookieManager>());

            return services;
        }

        /// <summary>
        /// Adds Cookie manager services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// /// <param name="options">CookieManagerOptions to add other functionality </param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddCookieManager(this IServiceCollection services, Action<CookieManagerOptions> options)
        {
            AddCookieManager(services);
            services.Configure(options);

            return services;
        }
    }
}
