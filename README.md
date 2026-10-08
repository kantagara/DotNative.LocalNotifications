# DotNative.LocalNotifications

Show an immediate local notification on Android and iOS:

```csharp
builder.Services.AddLocalNotifications();
await services.LocalNotifications.ShowAsync(1, "Title", "Message");
```

The Android app must declare `POST_NOTIFICATIONS` on Android 13+ and request it
through DotNative.Permissions. iOS authorization is requested on first use.
This first release supports immediate text notifications only; scheduling,
actions, attachments, badges and Windows/macOS/Linux backends are not included.

## Service access

Import `DotNative.LocalNotifications` to access the plugin through `IServiceProvider`:

```csharp
using DotNative.LocalNotifications;

var plugin = services.LocalNotifications;
```

The getter calls `GetRequiredService<ILocalNotifications>()` on every access, preserving
DI lifetimes and the usual missing-registration error. Register the plugin with
`AddLocalNotifications(...)` before building the provider.

A `net10.0` application uses the property syntax with C# 14 or later. A
`net9.0` application uses only the method equivalent:

```csharp
var plugin = services.LocalNotifications();
```

The package contains separate `net9.0` and `net10.0` assemblies. NuGet selects
the assembly matching the application target framework. `NET10_0_OR_GREATER`
selects the property; the `#else` branch selects the method.

Build and pack both targets with .NET 10 SDK. A source build using .NET 9 SDK
builds only `net9.0`; it does not produce the .NET 10 assembly.
