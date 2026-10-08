using DotNative.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace DotNative.LocalNotifications;
public interface ILocalNotifications { Task ShowAsync(int id, string title, string body, CancellationToken cancellationToken = default); }
public static class LocalNotificationsServices { public static IServiceCollection AddLocalNotifications(this IServiceCollection services) { services.TryAddSingleton<ILocalNotifications, NativeNotifications>(); return services; } }
internal sealed class NativeNotifications(IPlatformChannels channels) : ILocalNotifications {
 public Task ShowAsync(int id, string title, string body, CancellationToken cancellationToken = default) {
  ArgumentOutOfRangeException.ThrowIfNegative(id); ArgumentException.ThrowIfNullOrWhiteSpace(title); ArgumentException.ThrowIfNullOrWhiteSpace(body);
  if (title.Length > 256 || body.Length > 4096) throw new ArgumentOutOfRangeException(nameof(body));
  return channels.Get(LocalNotificationsChannel.Id).InvokeAsync("show", new Dictionary<string, object?> { ["id"] = id, ["title"] = title, ["body"] = body }, cancellationToken);
 }
}
