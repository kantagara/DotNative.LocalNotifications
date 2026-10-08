import Foundation
@preconcurrency import UserNotifications

@MainActor final class LocalNotificationsPlugin {
    private static var instance: LocalNotificationsPlugin?
    static func register() {
        if instance == nil {
            instance = LocalNotificationsPlugin()
        }
    }

    private init() {
        NativeChannels.channel("dotnative.local-notifications").handle("show") {
            args, reply in
            let fields = args.fields
            guard let id = fields["id"]?.integer, let title = fields["title"]?.string,
                !title.isEmpty,
                let body = fields["body"]?.string, !body.isEmpty
            else {
                reply.failure("invalid_argument", "A notification id, title and body are required")
                return
            }
            UNUserNotificationCenter.current().requestAuthorization(options: [
                .alert, .sound, .badge,
            ]) {
                granted, error in
                Task {
                    @MainActor in
                    if let error {
                        reply.failure("permission_denied", error.localizedDescription)
                        return
                    }
                    guard granted else {
                        reply.failure("permission_denied", "Notification permission was denied")
                        return
                    }
                    let content = UNMutableNotificationContent()
                    content.title = title
                    content.body = body
                    let request = UNNotificationRequest(
                        identifier: "dotnative.\(id)", content: content, trigger: nil)
                    do {
                        try await UNUserNotificationCenter.current().add(request)
                        reply.success()
                    } catch {
                        reply.failure("notification_failed", error.localizedDescription)
                    }
                }
            }
        }
    }
}
