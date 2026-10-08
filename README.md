# DotNative.LocalNotifications

Show an immediate local notification on Android and iOS:

```csharp
builder.Services.AddLocalNotifications();
await services.GetRequiredService<ILocalNotifications>().ShowAsync(1, "Title", "Message");
```

The Android app must declare `POST_NOTIFICATIONS` on Android 13+ and request it
through DotNative.Permissions. iOS authorization is requested on first use.
This first release supports immediate text notifications only; scheduling,
actions, attachments, badges and Windows/macOS/Linux backends are not included.
