using System;
using Microsoft.Extensions.DependencyInjection;

namespace DotNative.LocalNotifications;

public static class LocalNotificationsServiceProviderExtensions
{
#if NET10_0_OR_GREATER
    extension(IServiceProvider services)
    {
        /// <summary>Resolves the registered plugin using the provider's DI lifetime.</summary>
        public ILocalNotifications LocalNotifications =>
            services.GetRequiredService<ILocalNotifications>();
    }
#else
    /// <summary>Resolves the registered plugin using the provider's DI lifetime.</summary>
    public static ILocalNotifications LocalNotifications(this IServiceProvider services) =>
        services.GetRequiredService<ILocalNotifications>();
#endif
}
