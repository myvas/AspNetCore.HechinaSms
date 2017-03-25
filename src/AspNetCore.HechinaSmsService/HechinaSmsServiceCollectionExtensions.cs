using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AspNetCore.HechinaSmsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.HechinaSmsService
{
    public static class HechinaSmsServiceCollectionExtensions
    {
        /// <summary>
        /// Using Sms Middleware
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> passed to the configuration method.</param>
        /// <param name="setupAction">Middleware configuration options.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddHechinaSms(this IServiceCollection services, Action<HechinaSmsOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services.TryAddSingleton<HechinaSmsManager>();
            services.AddSingleton<ISmsSender, SmsSender>();
            services.AddSingleton<IVerificationCodeSmsSender, SmsSender>();

            return services;
        }
    }
}
